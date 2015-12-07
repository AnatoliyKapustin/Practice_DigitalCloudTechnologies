namespace DatMailReader.DataAccess.Providers
{
    using DatMailReader.DataAccess.Core;
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Enums;
    using MimeKit;
    using MimeKit.IO;
    using MimeKit.IO.Filters;
    using MimeKit.Tnef;
    using Models.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.Storage;

    public class DatParserProvider
    {
        private readonly static Lazy<DatParserProvider> instance = new Lazy<DatParserProvider>(() => new DatParserProvider(), true);

        public static DatParserProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public async Task<Message> OpenTnef(StorageFile datFile)
        {
            var message = new MimeMessage();
            var bodyFile = default(StorageFile);
            var targetFilesCollection = new List<FileInfo>();
            using (var stream = await datFile.OpenStreamForReadAsync())
            {
                using (var tnefReader = new TnefReader(stream))
                {
                    message = ExtractTnefMessage(tnefReader);
                    if (message.Sender == null)
                    {
                        message.Sender = new MailboxAddress(string.Empty, "Sender unknown");
                    }

                    foreach (MimePart mimePart in message.Attachments)
                    {
                        var isoFile = await LastExtractedFilesProvider.WriteFileToIsoStorage(mimePart.FileName, mimePart.ContentObject.Open());
                        var basicProperties = await isoFile.GetBasicPropertiesAsync();
                        var thumb = this.GetVectorThumbnailByType(isoFile.FileType);
                        var size = FileSizeString((double)basicProperties.Size);
                        targetFilesCollection.Add(new FileInfo(isoFile, thumb, size, datFile.DisplayName));
                    }

                    var body = Enumerable.FirstOrDefault<TextPart>(Enumerable.OfType<TextPart>(message.BodyParts));
                    if (body != null && !String.IsNullOrEmpty(body.Text))
                    {
                        string bodyFileName = "Message_Body.";
                        if (body.IsHtml)
                        {
                            bodyFileName += "html";
                        }
                        else if (body.IsPlain)
                        {
                            bodyFileName += "txt";
                        }
                        else if (body.IsRichText)
                        {
                            bodyFileName += "rtf";
                        }

                        bodyFile = await LastExtractedFilesProvider.WriteFileToIsoStorage(bodyFileName, body.Text);
                    }
                }
            }

            var targetMessage = new Message(message.Subject, message.Sender.ToString(), string.Empty, message.Date.ToString(), bodyFile, targetFilesCollection);

            return targetMessage;
        }

        private ImageStyles GetVectorThumbnailByType(string fileType)
        {
            var result = default(ImageStyles);
            switch (fileType)
            {
                case ".rar":
                case ".zip":
                case ".7z":
                    result = ImageStyles.Archive;
                    break;
                case ".pdf":
                case ".txt":
                case ".doc":
                case ".docx":
                case ".rtf":
                case ".odt":
                case ".chm":
                case ".ppt":
                case ".pptx":
                    result = ImageStyles.Text;
                    break;
                case ".gif":
                case ".jpg":
                case ".png":
                case ".tif":
                    result = ImageStyles.Picture;
                    break;
                default:
                    result = ImageStyles.Any;
                    break;
            }

            return result;
        }

        private string FileSizeString(double fileSize)
        {
            var strArray = new string[4]
            {
                "B",
                "KB",
                "MB",
                "GB"
            };
            var index = 0;
            while (fileSize >= 1000.0 && index + 1 < strArray.Length)
            {
                ++index;
                fileSize /= 1000.0;
            }
            return string.Format("{0:0.##} {1}", new object[2]
            {
                 fileSize,
                 strArray[index]
            });
        }

        private MimeMessage ExtractTnefMessage(TnefReader reader)
        {
            var tnefReader = reader.TnefPropertyReader;
            var builder = new BodyBuilder();
            var message = new MimeMessage();
            while (reader.ReadNextAttribute() && reader.AttributeLevel == TnefAttributeLevel.Message)
            {
                switch (reader.AttributeTag)
                {
                    case TnefAttributeTag.MapiProperties:
                        this.ExtractMapiProperties(reader, message, builder);
                        continue;
                    case TnefAttributeTag.DateReceived:
                        message.Date = tnefReader.ReadValueAsDateTime();
                        continue;
                    default:
                        continue;
                }
            }

            if (reader.AttributeLevel == TnefAttributeLevel.Attachment)
            {
                ExtractAttachments(reader, builder);
            }

            message.Body = builder.ToMessageBody();

            return message;
        }

        private void ExtractAttachments(TnefReader reader, BodyBuilder builder)
        {
            var tnefAttachMethod = TnefAttachMethod.ByValue;
            var bestEncodingFilter = new BestEncodingFilter();
            var tnefPropertyReader = reader.TnefPropertyReader;
            MimePart mimePart = null;
            do
            {
                int outputIndex;
                int outputLength;
                switch (reader.AttributeTag)
                {
                    case TnefAttributeTag.AttachData:
                        if (mimePart != null && tnefAttachMethod == TnefAttachMethod.ByValue)
                        {
                            byte[] numArray = tnefPropertyReader.ReadValueAsBytes();
                            bestEncodingFilter.Flush(numArray, 0, numArray.Length, out outputIndex, out outputLength);
                            mimePart.ContentTransferEncoding = bestEncodingFilter.GetBestEncoding(EncodingConstraint.SevenBit, 78);
                            mimePart.ContentObject = new ContentObject(new MemoryStream(numArray, false), ContentEncoding.Default);
                            bestEncodingFilter.Reset();
                            builder.Attachments.Add(mimePart);
                            break;
                        }
                        break;
                    case TnefAttributeTag.AttachRenderData:
                        tnefAttachMethod = TnefAttachMethod.ByValue;
                        mimePart = new MimePart();
                        break;
                    case TnefAttributeTag.Attachment:
                        if (mimePart != null)
                        {
                            while (tnefPropertyReader.ReadNextProperty())
                            {
                                switch (tnefPropertyReader.PropertyTag.Id)
                                {
                                    case TnefPropertyId.AttachData:
                                        using (var rawValueReadStream = tnefPropertyReader.GetRawValueReadStream())
                                        {
                                            using (var memoryStream = new MemoryStream())
                                            {

                                                byte[] buffer = new byte[16];
                                                if (tnefAttachMethod == TnefAttachMethod.EmbeddedMessage)
                                                {
                                                    TnefPart tnefPart = new TnefPart();
                                                    foreach (Parameter parameter in mimePart.ContentType.Parameters)
                                                    {
                                                        tnefPart.ContentType.Parameters[parameter.Name] = parameter.Value;
                                                    }

                                                    if (mimePart.ContentDisposition != null)
                                                    {
                                                        tnefPart.ContentDisposition = mimePart.ContentDisposition;
                                                    }

                                                    mimePart = tnefPart;
                                                }
                                                rawValueReadStream.Read(buffer, 0, 16);
                                                rawValueReadStream.CopyTo(memoryStream, 4096);
                                                byte[] input = memoryStream.ToArray();
                                                bestEncodingFilter.Flush(input, 0, (int)memoryStream.Length, out outputIndex, out outputLength);
                                                mimePart.ContentTransferEncoding = bestEncodingFilter.GetBestEncoding(EncodingConstraint.SevenBit, 78);
                                                mimePart.ContentObject = new ContentObject(memoryStream, ContentEncoding.Default);
                                                bestEncodingFilter.Reset();
                                                builder.Attachments.Add(mimePart);
                                            }
                                        }

                                        continue;
                                    case TnefPropertyId.AttachFilename:
                                        if (String.IsNullOrEmpty(mimePart.FileName))
                                        {
                                            mimePart.FileName = tnefPropertyReader.ReadValueAsString();
                                            continue;
                                        }

                                        continue;
                                    case TnefPropertyId.AttachMethod:
                                        tnefAttachMethod = (TnefAttachMethod)tnefPropertyReader.ReadValueAsInt32();
                                        continue;
                                    case TnefPropertyId.AttachLongFilename:
                                        mimePart.FileName = tnefPropertyReader.ReadValueAsString();
                                        continue;
                                    case TnefPropertyId.AttachMimeTag:
                                        string[] strArray = tnefPropertyReader.ReadValueAsString().Split('/');
                                        if (strArray.Length == 2)
                                        {
                                            mimePart.ContentType.MediaType = strArray[0].Trim();
                                            mimePart.ContentType.MediaSubtype = strArray[1].Trim();
                                            continue;
                                        }

                                        continue;
                                    case TnefPropertyId.AttachContentBase:
                                        var uriString = tnefPropertyReader.ReadValueAsString();
                                        mimePart.ContentBase = new Uri(uriString, UriKind.Absolute);
                                        continue;
                                    case TnefPropertyId.AttachContentId:
                                        mimePart.ContentId = tnefPropertyReader.ReadValueAsString();
                                        continue;
                                    case TnefPropertyId.AttachContentLocation:
                                        var uriString2 = tnefPropertyReader.ReadValueAsString();
                                        if (Uri.IsWellFormedUriString(uriString2, UriKind.Absolute))
                                        {
                                            mimePart.ContentLocation = new Uri(uriString2, UriKind.Absolute);
                                            continue;
                                        }

                                        if (Uri.IsWellFormedUriString(uriString2, UriKind.Relative))
                                        {
                                            mimePart.ContentLocation = new Uri(uriString2, UriKind.Relative);
                                            continue;
                                        }

                                        continue;
                                    case TnefPropertyId.AttachFlags:
                                        if ((tnefPropertyReader.ReadValueAsInt32() & 4) != 0)
                                        {
                                            if (mimePart.ContentDisposition == null)
                                            {
                                                mimePart.ContentDisposition = new ContentDisposition("inline");
                                                continue;
                                            }

                                            mimePart.ContentDisposition.Disposition = "inline";
                                            continue;
                                        }

                                        continue;
                                    case TnefPropertyId.AttachDisposition:
                                        var disposition = tnefPropertyReader.ReadValueAsString();
                                        if (mimePart.ContentDisposition == null)
                                        {
                                            mimePart.ContentDisposition = new ContentDisposition(disposition);
                                            continue;
                                        }

                                        mimePart.ContentDisposition.Disposition = disposition;
                                        continue;
                                    case TnefPropertyId.AttachSize:
                                        if (mimePart.ContentDisposition == null)
                                        {
                                            mimePart.ContentDisposition = new ContentDisposition();
                                        }

                                        mimePart.ContentDisposition.Size = new long?(tnefPropertyReader.ReadValueAsInt64());
                                        continue;
                                    case TnefPropertyId.DisplayName:
                                        mimePart.ContentType.Name = tnefPropertyReader.ReadValueAsString();
                                        continue;
                                    default:
                                        continue;
                                }
                            }
                            break;
                        }
                        break;
                    case TnefAttributeTag.AttachTitle:
                        var name = tnefPropertyReader.ReadValueAsString();
                        if (mimePart != null && String.IsNullOrEmpty(mimePart.FileName))
                        {
                            mimePart.FileName = name;
                            break;
                        }

                        break;
                    case TnefAttributeTag.AttachCreateDate:
                        var creationDateTime = tnefPropertyReader.ReadValueAsDateTime();
                        if (mimePart != null)
                        {
                            if (mimePart.ContentDisposition == null)
                            {
                                mimePart.ContentDisposition = new ContentDisposition();
                            }

                            mimePart.ContentDisposition.CreationDate = new DateTimeOffset(creationDateTime);
                            break;
                        }

                        break;
                    case TnefAttributeTag.AttachModifyDate:
                        var modifyDateTime = tnefPropertyReader.ReadValueAsDateTime();
                        if (mimePart != null)
                        {
                            if (mimePart.ContentDisposition == null)
                            {
                                mimePart.ContentDisposition = new ContentDisposition();
                            }

                            mimePart.ContentDisposition.ModificationDate = new DateTimeOffset(modifyDateTime);
                            break;
                        }

                        break;
                }
            }

            while (reader.ReadNextAttribute());
        }

        private void ExtractMapiProperties(TnefReader reader, MimeMessage message, BodyBuilder builder)
        {
            var tnefPropertyReader = reader.TnefPropertyReader;
            while (tnefPropertyReader.ReadNextProperty())
            {
                switch (tnefPropertyReader.PropertyTag.Id)
                {
                    case TnefPropertyId.RtfCompressed:
                        var memoryStream = new MemoryStream();
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Binary)
                        {
                            var textPart = new TextPart("rtf");
                            textPart.ContentType.Name = "body.rtf";
                            RtfCompressedToRtf rtfCompressedToRtf = new RtfCompressedToRtf();
                            //var memoryStream = new MemoryStream();
                            using (var filteredStream = new FilteredStream(memoryStream))
                            {
                                filteredStream.Add(rtfCompressedToRtf);
                                using (Stream rawValueReadStream = tnefPropertyReader.GetRawValueReadStream())
                                {
                                    rawValueReadStream.CopyTo(filteredStream, 4096);
                                    filteredStream.Flush();
                                }
                            }

                            textPart.ContentObject = new ContentObject(memoryStream, ContentEncoding.Default);
                            memoryStream.Position = 0L;
                            builder.Attachments.Add(textPart);
                            continue;
                        }

                        memoryStream.Dispose();
                        continue;
                    case TnefPropertyId.BodyHtml:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Binary)
                        {
                            var textPart = new TextPart("html");
                            textPart.ContentType.Name = "body.html";
                            textPart.Text = tnefPropertyReader.ReadValueAsString();
                            builder.Attachments.Add(textPart);
                            continue;
                        }

                        continue;
                    case TnefPropertyId.InternetMessageId:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode)
                        {
                            message.MessageId = tnefPropertyReader.ReadValueAsString();
                            continue;
                        }

                        continue;
                    case TnefPropertyId.Subject:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode)
                        {
                            message.Subject = tnefPropertyReader.ReadValueAsString();
                            continue;
                        }

                        continue;
                    case TnefPropertyId.Body:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Binary)
                        {
                            var textPart = new TextPart("plain");
                            textPart.ContentType.Name = "body.txt";
                            textPart.Text = tnefPropertyReader.ReadValueAsString();
                            builder.Attachments.Add(textPart);
                            continue;
                        }

                        continue;
                    case TnefPropertyId.ConversationTopic:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode)
                        {
                            message.Subject = tnefPropertyReader.ReadValueAsString();
                            continue;
                        }

                        continue;
                    case TnefPropertyId.SenderName:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode)
                        {
                            var sender = new MailboxAddress(string.Empty, tnefPropertyReader.ReadValueAsString());
                            message.Sender = sender;
                        }

                        continue;
                    case (TnefPropertyId)Mapi.ID.PR_PRIMARY_SEND_ACCOUNT:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode)
                        {
                            var senderEmail = new MailboxAddress(string.Empty, tnefPropertyReader.ReadValueAsString());
                            message.Sender = senderEmail;
                        }

                        continue;
                    default:
                        try
                        {
                            tnefPropertyReader.ReadValue();
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                }
            }
        }
    }
}

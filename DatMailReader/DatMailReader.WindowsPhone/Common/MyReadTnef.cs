using MimeKit;
using MimeKit.IO;
using MimeKit.IO.Filters;
using MimeKit.Tnef;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Winmaildat;

namespace DatMailReader.Common
{
    internal class MyReadTnef
    {
        private static MimeMessage message;

        public static async Task<ObservableCollection<FileInfo>> OpenTnef(StorageFile file, ObservableCollection<FileInfo> targetCollection)
        {
            string fileName = file.DisplayName;
            Stream sRead = await WindowsRuntimeStorageExtensions.OpenStreamForReadAsync(file);
            int num;
            StorageItemThumbnail _thumb;
            DateTimeOffset _date;
            string _size;
            string _from;
            double fileSize;
            try
            {
                TnefReader tnefReader = new TnefReader(sRead, 0, TnefComplianceMode.Loose);
                message = MyReadTnef.ExtractTnefMessage(tnefReader);
                string _subject = message.Subject;
                int resultCount = 0;
                foreach (MimePart mimePart in message.Attachments)
                {
                    StorageFile isoFile = await MyReadTnef.writeFileToIsoStorage(mimePart.FileName, mimePart.ContentObject.Open());
                    BasicProperties basicProperties = await isoFile.GetBasicPropertiesAsync();
                    fileSize = (double)basicProperties.Size;
                    _thumb = await isoFile.GetThumbnailAsync(ThumbnailMode.DocumentsView);
                    _date = (DateTimeOffset)mimePart.ContentDisposition.ModificationDate;
                    _size = FileSizeString(fileSize);
                    _from = file.Name;
                    targetCollection.Add(new FileInfo(isoFile, _thumb, _date, _size, _from, _subject, fileName));
                    ++resultCount;
                }
                TextPart body = Enumerable.FirstOrDefault<TextPart>(Enumerable.OfType<TextPart>(message.BodyParts));
                if (body != null && body.Text != null)
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
                    StorageFile bodyFile = await MyReadTnef.writeFileToIsoStorage(bodyFileName, body.Text);
                    targetCollection.Add(new FileInfo(bodyFile, null, _date, string.Empty, string.Empty, string.Empty, string.Empty));
                }
                num = resultCount;
            }
            catch
            {
                num = -1;
            }
            return targetCollection;
        }

        public static MimeMessage getMessage()
        {
            return message;
        } 

        private static string FileSizeString(double fileSize)
        {
            string[] strArray = new string[4]
            {
                "B",
                "KB",
                "MB",
                "GB"
            };
            int index = 0;
            while (fileSize >= 1024.0 && index + 1 < strArray.Length)
            {
                ++index;
                fileSize /= 1024.0;
            }

            return string.Format("{0:0.##} {1}", new object[2]
            {
                (object) fileSize,
                (object) strArray[index]
            });
        }

        private static MimeMessage ExtractTnefMessage(TnefReader reader)
        {
            BodyBuilder builder = new BodyBuilder();
            MimeMessage message = new MimeMessage();
            while (reader.ReadNextAttribute() && reader.AttributeLevel != TnefAttributeLevel.Attachment)
            {
                int num = (int)reader.AttributeLevel;
                TnefPropertyReader tnefPropertyReader = reader.TnefPropertyReader;
                switch (reader.AttributeTag)
                {
                    case TnefAttributeTag.MapiProperties:
                        MyReadTnef.ExtractMapiProperties(reader, message, builder);
                        continue;
                    case TnefAttributeTag.Body:
                        builder.TextBody = tnefPropertyReader.ReadValueAsString();
                        continue;
                    case TnefAttributeTag.DateSent:
                        message.Date = (DateTimeOffset)tnefPropertyReader.ReadValueAsDateTime();
                        continue;
                    default:
                        continue;
                }
            }

            if (reader.AttributeLevel == TnefAttributeLevel.Attachment)
                MyReadTnef.ExtractAttachments(reader, builder);
            message.Body = builder.ToMessageBody();
            return message;
        }

        private static void ExtractAttachments(TnefReader reader, BodyBuilder builder)
        {
            TnefAttachMethod tnefAttachMethod = TnefAttachMethod.ByValue;
            BestEncodingFilter bestEncodingFilter = new BestEncodingFilter();
            TnefPropertyReader tnefPropertyReader = reader.TnefPropertyReader;
            MimePart mimePart = null;
            do
            {
                int num = (int)reader.AttributeLevel;
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
                                        Stream rawValueReadStream = tnefPropertyReader.GetRawValueReadStream();
                                        MemoryStream memoryStream = new MemoryStream();
                                        byte[] buffer = new byte[16];
                                        if (tnefAttachMethod == TnefAttachMethod.EmbeddedMessage)
                                        {
                                            TnefPart tnefPart = new TnefPart();
                                            foreach (Parameter parameter in mimePart.ContentType.Parameters)
                                                tnefPart.ContentType.Parameters[parameter.Name] = parameter.Value;
                                            if (mimePart.ContentDisposition != null)
                                                tnefPart.ContentDisposition = mimePart.ContentDisposition;
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
                                        continue;
                                    case TnefPropertyId.AttachFilename:
                                        if (mimePart.FileName == null)
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
                                        string uriString1 = tnefPropertyReader.ReadValueAsString();
                                        mimePart.ContentBase = new Uri(uriString1, UriKind.Absolute);
                                        continue;
                                    case TnefPropertyId.AttachContentId:
                                        mimePart.ContentId = tnefPropertyReader.ReadValueAsString();
                                        continue;
                                    case TnefPropertyId.AttachContentLocation:
                                        string uriString2 = tnefPropertyReader.ReadValueAsString();
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
                                        string disposition = tnefPropertyReader.ReadValueAsString();
                                        if (mimePart.ContentDisposition == null)
                                        {
                                            mimePart.ContentDisposition = new ContentDisposition(disposition);
                                            continue;
                                        }
                                        mimePart.ContentDisposition.Disposition = disposition;
                                        continue;
                                    case TnefPropertyId.AttachSize:
                                        if (mimePart.ContentDisposition == null)
                                            mimePart.ContentDisposition = new ContentDisposition();
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
                        string str = tnefPropertyReader.ReadValueAsString();
                        if (mimePart != null && string.IsNullOrEmpty(mimePart.FileName))
                        {
                            mimePart.FileName = str;
                            break;
                        }
                        break;
                    case TnefAttributeTag.AttachCreateDate:
                        DateTime dateTime1 = tnefPropertyReader.ReadValueAsDateTime();
                        if (mimePart != null)
                        {
                            if (mimePart.ContentDisposition == null)
                                mimePart.ContentDisposition = new ContentDisposition();
                            mimePart.ContentDisposition.CreationDate = new DateTimeOffset?((DateTimeOffset)dateTime1);
                            break;
                        }
                        break;
                    case TnefAttributeTag.AttachModifyDate:
                        DateTime dateTime2 = tnefPropertyReader.ReadValueAsDateTime();
                        if (mimePart != null)
                        {
                            if (mimePart.ContentDisposition == null)
                            { mimePart.ContentDisposition = new ContentDisposition(); }
                            mimePart.ContentDisposition.ModificationDate = new DateTimeOffset?((DateTimeOffset)dateTime2);
                            break;
                        }
                        break;
                }
            }
            while (reader.ReadNextAttribute());
        }

        private static async Task<StorageFile> writeFileToIsoStorage(string fileName, Stream content)
        {
            StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            StorageFile sampleFile = await temporaryFolder.CreateFileAsync(fileName, (CreationCollisionOption)1);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                content.CopyTo(memoryStream);
                await FileIO.WriteBytesAsync(sampleFile, memoryStream.ToArray());
            }
            return sampleFile;
        }

        private static async Task<StorageFile> writeFileToIsoStorage(string fileName, string content)
        {
            StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            StorageFile sampleFile = await temporaryFolder.CreateFileAsync(fileName, (CreationCollisionOption)1);
            await FileIO.WriteTextAsync(sampleFile, content);
            return sampleFile;
        }

        private static void ExtractMapiProperties(TnefReader reader, MimeMessage message, BodyBuilder builder)
        {
            TnefPropertyReader tnefPropertyReader = reader.TnefPropertyReader;
            while (tnefPropertyReader.ReadNextProperty())
            {
                switch (tnefPropertyReader.PropertyTag.Id)
                {
                    case TnefPropertyId.RtfCompressed:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Binary)
                        {
                            TextPart textPart = new TextPart("rtf");
                            textPart.ContentType.Name = "body.rtf";
                            RtfCompressedToRtf rtfCompressedToRtf = new RtfCompressedToRtf();
                            MemoryStream memoryStream = new MemoryStream();
                            using (FilteredStream filteredStream = new FilteredStream(memoryStream))
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
                        continue;
                    case TnefPropertyId.BodyHtml:
                        if (tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.String8 || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Unicode || tnefPropertyReader.PropertyTag.ValueTnefType == TnefPropertyType.Binary)
                        {
                            TextPart textPart = new TextPart("html");
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
                            TextPart textPart = new TextPart("plain");
                            textPart.ContentType.Name = "body.txt";
                            textPart.Text = tnefPropertyReader.ReadValueAsString();
                            builder.Attachments.Add(textPart);
                            continue;
                        }
                        continue;
                    case TnefPropertyId.SenderEmailAddress:
                        if (tnefPropertyReader.PropertyTag != null)
                        {
                            MailboxAddress addr = new MailboxAddress(string.Empty, tnefPropertyReader.ReadValueAsString());
                            message.Sender = addr;
                            continue;
                        }
                        continue;
                    case TnefPropertyId.ExpiryTime:               //DisplayTo //SenderName DisplayCc
                        if (tnefPropertyReader.PropertyTag != null)
                        {
                            //DateTime d = tnefPropertyReader.ReadValueAsDateTime();
                            string s = tnefPropertyReader.ReadValueAsString();
                            continue;
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

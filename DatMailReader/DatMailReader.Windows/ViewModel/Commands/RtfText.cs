using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace DatMailReader.ViewModel
{
    public static class RtfText
    {
        public static string GetRichText(DependencyObject obj)
        {
            return (string)obj.GetValue(RichTextProperty);
        }
        public static void SetRichText(DependencyObject obj, string value)
        {
            obj.SetValue(RichTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for RichText. This enables animation, styling, binding, etc...

        public static readonly DependencyProperty RichTextProperty =
        DependencyProperty.RegisterAttached("RichText", typeof(string), typeof(RtfText), new PropertyMetadata(string.Empty, Callback));
        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var reb = (RichEditBox)d;
            Canvas.SetZIndex(reb, 0);
            reb.Visibility = Visibility.Collapsed;
            if (e.NewValue != null)
            {
                reb.IsColorFontEnabled = true;
                reb.IsReadOnly = false;
                reb.Document.SetText(TextSetOptions.FormatRtf, e.NewValue.ToString());
                reb.IsReadOnly = true;
                reb.Visibility = Visibility.Visible;
                Canvas.SetZIndex(reb, 1);
            }
            else
            {
                reb.Visibility = Visibility.Collapsed;
                Canvas.SetZIndex(reb, 0);
            }
        }
    }
}

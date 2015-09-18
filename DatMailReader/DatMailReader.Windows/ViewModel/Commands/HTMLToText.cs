using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DatMailReader.ViewModel
{
    public class HTMLToText
    {
        public static string GetHTMLText(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLTextProperty);
        }

        public static void SetHTMLText(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for RichText. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HTMLTextProperty =
        DependencyProperty.RegisterAttached("HTMLText", typeof(string), typeof(HTMLToText), new PropertyMetadata(string.Empty, Callback));
        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var reb = (WebView)d;
            Canvas.SetZIndex(reb, 0);
            reb.Visibility = Visibility.Collapsed;
            if (e.NewValue != null)
            {
                reb.NavigateToString((string)e.NewValue);
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

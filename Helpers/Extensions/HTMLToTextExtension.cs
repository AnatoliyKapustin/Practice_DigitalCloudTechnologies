namespace DatMailReader.Helpers.Extensions
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class HTMLToTextExtension
    {
        public static string GetHTMLText(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLTextProperty);
        }

        public static void SetHTMLText(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLTextProperty, value);
        }

        public static readonly DependencyProperty HTMLTextProperty =
        DependencyProperty.RegisterAttached("HTMLText", typeof(string), typeof(HTMLToTextExtension), new PropertyMetadata(string.Empty, Callback));
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DatMailReader.Helpers.Extensions
{
    public static class TextBlockNavigationClickExtension
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(TextBlockNavigationClickExtension), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBlock;
            if (control != null)
            {
                if (e.OldValue != null)
                {
                    control.Tapped -= OnTapped;
                }

                if (e.NewValue != null)
                {
                    control.Tapped += OnTapped;
                }
            }
        }

        private static void OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var control = sender as TextBlock;
            if (control != null)
            {
                var command = GetCommand(control);
                if (command != null)
                {
                    command.Execute(null);
                }
            }
        }
    }
}

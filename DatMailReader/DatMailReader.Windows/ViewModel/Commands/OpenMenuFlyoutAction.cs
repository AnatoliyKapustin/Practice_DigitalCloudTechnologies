using Microsoft.Xaml.Interactivity;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace DatMailReader.ViewModel
{
    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        public static object ClickedItem;
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(OpenMenuFlyoutAction), new PropertyMetadata(null, OnCommandPropertyChanged));

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
            var control = d as MenuFlyoutItem;
            if (control != null)
            {
                control.Click+= OnItemClick;
            }
        }

        private static void OnItemClick(object sender, RoutedEventArgs e)
        {
            var control = sender as MenuFlyoutItem;
            var command = GetCommand(control);
            var obj = ClickedItem;
            if (command != null && command.CanExecute(obj))
                command.Execute(obj);
        }
        public object Execute(object sender, object parameter)
        {  
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            ClickedItem = senderElement.DataContext;
            flyoutBase.ShowAt(senderElement);
            return ClickedItem;
        }
    }
}
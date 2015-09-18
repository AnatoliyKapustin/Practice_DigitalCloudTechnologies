using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Winmaildat;

namespace DatMailReader.ViewModel
{
    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {  
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            var ClickedItem = senderElement.DataContext;
            flyoutBase.ShowAt(senderElement);
            
            return ClickedItem;
        }
    }
}
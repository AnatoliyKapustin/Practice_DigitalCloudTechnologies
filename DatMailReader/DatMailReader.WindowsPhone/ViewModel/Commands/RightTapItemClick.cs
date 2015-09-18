using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace DatMailReader.ViewModel
{
    public static class RightTapItemClick
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand),
        typeof(ItemClickCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListViewBase;
            if (control != null)
                control.RightTapped += OnItemClick;
        }

        private static void OnItemClick(object sender, RightTappedRoutedEventArgs e)
        {
            var control = sender as ListViewBase;
            var command = GetCommand(control);
            var RightTappedItem = sender;
            /*   if (command != null && command.CanExecute(e.))
                   command.Execute(e.ClickedItem);*/
        }
    }
}

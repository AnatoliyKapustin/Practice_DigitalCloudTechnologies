namespace DatMailReader.Helpers.Extensions
{
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public static class ItemClickCommandExtension
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand),typeof(ItemClickCommandExtension), new PropertyMetadata(null, OnCommandPropertyChanged));

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
            var control = d as ListViewBase;
            if (control != null)
            {
                if(e.OldValue != null)
                {
                    control.ItemClick -= OnItemClick;
                }

                if (e.NewValue != null)
                {
                    control.ItemClick += OnItemClick;
                }
            }
        }

        private static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var control = sender as ListViewBase;
            if (control != null)
            {
                var command = GetCommand(control);
                var obj = e.ClickedItem;
                if (command != null && command.CanExecute(e.ClickedItem))
                {
                    command.Execute(e.ClickedItem);
                }
            }
        }
    }
}
﻿using DatMailReader.ViewModelLocator;
using ViewModels.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DatMailReader.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainViewModel Vm
        {
            get
            {
                return (MainViewModel)DataContext;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Vm.Init();
        }
    }
}
        

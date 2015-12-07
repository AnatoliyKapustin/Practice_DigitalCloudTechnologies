using DatMailReader.Models.Model;
using DatMailReader.Shared.Helpers;
using DatMailReader.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
namespace DatMailReader.View
{
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ExtractedAttachments : Page
    {
        private NavigationHelper navigationHelper;

        public ExtractedAttachments()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.Loaded += OnLoaded; 
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, GetState(e.NewSize.Width), true);
        }

        private string GetState(double width)
        {
            //if (width <= 500)
            //    return "Snapped";

            //if (width <= 660)
            //    return "EvenSmaller";

            if (width <= 755)
                return "Smaller";

            return "Default";
        }

        public NavigationHelper NavigationHelper
        {
            get
            {
                return this.navigationHelper;
            }
        }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var extractedAttachmentsViewModel = DataContext as ExtractedAttachmentsViewModel;
            if (extractedAttachmentsViewModel != null)
            {
                extractedAttachmentsViewModel.Initialize();
            }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
    }
}

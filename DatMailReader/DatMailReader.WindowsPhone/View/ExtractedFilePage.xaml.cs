namespace DatMailReader.View
{
    using DatMailReader.Models.Interfaces;
    using DatMailReader.Services;
    using DatMailReader.Shared.Helpers;
    using DatMailReader.ViewModels.ViewModels;
    using System;
    using Windows.Storage;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ExtractedFilePage : Page
    {
        private NavigationHelper navigationHelper;

        public ExtractedFilePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

        }

        public NavigationHelper NavigationHelper
        {
            get
            {
                return this.navigationHelper;
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
            this.navigationHelper.OnNavigatedTo(e);
            var fileSaveService = FilesSaveService.Instance;
            fileSaveService.Initialize();
            var viewModel = DataContext as ExtractedFilesViewModel;
            viewModel.FileSaveService = fileSaveService;
            var navigableViewModel = this.DataContext as INavigable;
            if (navigableViewModel != null)
            {
                navigableViewModel.Activate(e.Parameter);
            }    
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
    }
}

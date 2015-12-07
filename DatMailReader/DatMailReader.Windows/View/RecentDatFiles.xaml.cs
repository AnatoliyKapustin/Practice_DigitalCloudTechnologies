using DatMailReader.Shared.Helpers;
using DatMailReader.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DatMailReader.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecentDatFiles : Page
    {
        private NavigationHelper navigationHelper;

        public RecentDatFiles()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.Loaded += OnLoaded;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }

        private async void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var RecentDatFilesViewModel = DataContext as RecentDatFilesViewModel;
            if (RecentDatFilesViewModel != null)
            {
                await RecentDatFilesViewModel.Initialize();
            }
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

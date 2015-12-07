using DatMailReader.DataAccess.Providers;
using DatMailReader.Shared.Services;
using DatMailReader.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DatMailReader.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, GetState(e.NewSize.Width), true);
        }

        private string GetState(double width)
        {
            if (width <= 500)
                return "Snapped";

            //if (width <= 660)
            //    return "EvenSmaller";

            //if (width <= 755)
            //    return "Smaller";

            return "Default";
        }

        private async void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var fileService = FileSelectionService.Instance;
            fileService.Initialize();
            var viewModel = DataContext as MainViewModel;
            await viewModel.Initialize();
            viewModel.FileOpenService = fileService;
        }
    }
}

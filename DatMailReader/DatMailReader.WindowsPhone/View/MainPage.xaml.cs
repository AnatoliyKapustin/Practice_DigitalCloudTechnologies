// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DatMailReader.View
{
    using DatMailReader.Shared.Services;
    using DatMailReader.ViewModels.ViewModels;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
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
        

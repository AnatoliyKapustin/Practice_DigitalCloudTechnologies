namespace DatMailReader.ViewModels.ViewModels
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Practices.ServiceLocation;

    public class ViewModelLocatorPCL
    {
        public const string SecondPageKey = "OpenedFilePage";
        public static INavigationService NavigationService;

        public ViewModelLocatorPCL()
        {
            /// var OpenFileService = new FileSelectionService();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            ///SimpleIoc.Default.Register<IFileSelectionService>(() => OpenFileService);
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}

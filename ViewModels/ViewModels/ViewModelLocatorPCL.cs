namespace DatMailReader.ViewModels.ViewModels
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Practices.ServiceLocation;

    public class ViewModelLocatorPCL
    {
        public const string ExtractedFilePageKey = "ExractedFilePage";
        public const string AllDatFilesPage = "DatFilesPage";
        public const string AllAttachmentsPage = "AttacmentsPage";
        public static INavigationService NavigationService;

        public ViewModelLocatorPCL()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public static void Cleanup()
        {
        }
    }
}

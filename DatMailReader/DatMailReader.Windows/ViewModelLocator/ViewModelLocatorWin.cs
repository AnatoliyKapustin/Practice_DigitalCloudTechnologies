using DatMailReader.View;
using DatMailReader.ViewModels.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace DatMailReader.ViewModelLocator
{
    public class ViewModelLocatorWin : ViewModelLocatorPCL
    {
        static ViewModelLocatorWin()
        {
           var nav = new NavigationService();
           var FileSelectionService = new WindowsFileExecuteProvider();
           nav.Configure(ViewModelLocatorPCL.SecondPageKey, typeof(OpenedFilePage));
           SimpleIoc.Default.Register<INavigationService>(() => nav); 
        }

    }
}

using DatMailReader.View;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using DatMailReader.ViewModels.ViewModels;

namespace DatMailReader.ViewModelLocator
{
    public class ViewModelLocatorWin : ViewModelLocatorPCL
    {
        static ViewModelLocatorWin()
        {
            var nav = new NavigationService();
            nav.Configure(ViewModelLocatorPCL.SecondPageKey, typeof(OpenedFilePage));
            SimpleIoc.Default.Register<INavigationService>(() => nav); 
        }
    }
}

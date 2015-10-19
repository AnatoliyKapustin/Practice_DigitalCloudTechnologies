namespace DatMailReader.ViewModelLocator
{
    using DatMailReader.View;
    using DatMailReader.ViewModels.ViewModels;
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Views;

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

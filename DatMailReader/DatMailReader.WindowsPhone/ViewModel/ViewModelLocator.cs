using DatMailReader.View;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace DatMailReader.ViewModel
{
    public class ViewModelLocator
    {
        public const string SecondPageKey1 = "OpenedFilePageWP";

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public DetailViewModel detailViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DetailViewModel>();
            }
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var nav = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            nav.Configure(ViewModelLocator.SecondPageKey1, typeof(OpenedFilePageWP));
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DetailViewModel>();
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}

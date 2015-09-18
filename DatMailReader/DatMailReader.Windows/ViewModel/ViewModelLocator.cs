using DatMailReader.View;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace DatMailReader.ViewModel
{
    public class ViewModelLocator
    {
        public const string SecondPageKey = "OpenedFilePage";

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
            nav.Configure(ViewModelLocator.SecondPageKey, typeof(OpenedFilePage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);
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

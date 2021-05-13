using Prism.Mvvm;
using Prism.Navigation;
using PropertyChanged;
using System.Threading.Tasks;

namespace XamarinUtility.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IInitialize, IInitializeAsync
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        [DoNotNotify]
        protected INavigationService NavigationService { get; }

        protected virtual void OnPageLoaded(INavigationParameters parameters)
        {

        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var mode = parameters.GetNavigationMode();
            switch (mode)
            {
                case NavigationMode.New:
                    OnPageLoaded(parameters);
                    break;
            }
        }
    }
}

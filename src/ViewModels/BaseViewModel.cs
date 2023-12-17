using Prism.Mvvm;
using Prism.Navigation;
using PropertyChanged;
using System.Threading.Tasks;

namespace XamarinUtility.ViewModels;

public class BaseViewModel(INavigationService navigationService) : BindableBase, INavigationAware, IInitialize, IInitializeAsync
{
    [DoNotNotify]
    protected INavigationService NavigationService { get; } = navigationService;

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

using CrossUtility.Extensions;
using CrossUtility.Helpers;
using Prism.Navigation;
using PropertyChanged;
using System.Threading.Tasks;
using XamarinUtility.Enums;

namespace XamarinUtility.ViewModels
{
    public class ModalViewModel : ModalViewModel<ModalResult>
    {
        public ModalViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }

    public abstract class ModalViewModel<TReturnType> : BaseViewModel, IDismissableModal
    {
        protected ModalViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            TaskCompletion = parameters[nameof(TaskCompletion)] as TaskCompletionSource<TReturnType?>;
        }

        protected async Task DismissWithResult(TReturnType? result, bool animated = true)
        {
            // TODO cast to object if null
            var parameter = result?.AsDictionary();
            var navParam = new NavigationParameters();
            if (parameter != null)
                foreach (var entry in parameter)
                    navParam.Add(entry.Key, entry.Value);
            var navigationResult = await NavigationService.GoBackAsync(navParam, null, animated: animated);
            // TODO https://github.com/dansiegel/Prism.Plugin.Popups/issues/129
            if (navigationResult.Success)
            {
                SetResult(result);
            }
            else
            {
                Contract.NotNull(TaskCompletion);
                TaskCompletion!.SetException(navigationResult.Exception);
            }
        }

        protected void SetResult(TReturnType? result)
        {
            Contract.NotNull(TaskCompletion);
            TaskCompletion!.TrySetResult(result);
        }

        public virtual bool DismissOnBackgroundClick()
        {
            return false;
        }

        public virtual bool DismissOnBackButtonPress()
        {
            return false;
        }

        #region Properties

        [DoNotNotify]
        public TaskCompletionSource<TReturnType?>? TaskCompletion { get; private set; }

        #endregion
    }
}

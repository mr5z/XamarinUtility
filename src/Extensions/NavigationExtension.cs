using CrossUtility.Helpers;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinUtility.Enums;
using XamarinUtility.Exceptions;
using XamarinUtility.Extensions.Navigations;
using XamarinUtility.ViewModels;

namespace XamarinUtility.Extensions
{
    public static class NavigationExtension
    {
        private const string KnownViewModelPattern = "ViewModel";
        private const string KnownPagePattern = "Page";

        public static Func<INavigationService, INavigationParameters?, ViewModelMetadata, Task<bool>>? Validator { get; set; }

        public static async Task<INavigationResult> PushAsync<TViewModel>(this INavigationService navigationService,
            INavigationParameters? parameters = null, bool? useModalNavigation = null, bool animated = true)
            where TViewModel : BaseViewModel
        {
            var pageName = ToPageName<TViewModel>();

            var validationResult = await PerformValidation(typeof(TViewModel), navigationService, parameters);
            if (validationResult != null)
                return validationResult;

            var navigationResult = await navigationService.NavigateAsync(pageName, parameters, useModalNavigation: useModalNavigation, animated: animated);
            return navigationResult;
        }

        public static IPageLink Absolute(this INavigationService navigationService, bool withNavigation = false)
        {
            var rootPage = "/" + (withNavigation ? nameof(NavigationPage) : string.Empty);
            return new PageLink(navigationService, rootPage);
        }

        public static IPageLink Relative(this INavigationService navigationService, bool withNavigation = false)
        {
            var rootPage = withNavigation ? nameof(NavigationPage) : string.Empty;
            return new PageLink(navigationService, rootPage);
        }

        public static IPageLink Push<TViewModel>(this IPageLink pageLink, object? parameter = null) where TViewModel : BaseViewModel
        {
            var page = ToPageName<TViewModel>();
            return pageLink.AppendSegment(page, typeof(TViewModel), parameter);
        }

        public static IPageLink Pop(this IPageLink pageLink, int popCount = 1)
        {
            var page = "..";
            for (var i = 0; i < popCount; ++i)
                pageLink.AppendSegment(page);
            return pageLink;
        }

        public static async Task<INavigationResult> NavigateAsync(this IPageLink pageLink,
            INavigationParameters? parameters = null,
            bool? useNavigationModal = null,
            bool animated = true)
        {
            var navigationService = (pageLink as PageLink)!.NavigationService;

            foreach(var page in pageLink.PageTypes)
            {
                if (page == null)
                    continue;

                var validationResult = await PerformValidation(page, navigationService, parameters);
                if (validationResult != null)
                    return validationResult;
            }

            var fullPath = pageLink.FullPath;
            var navigationResult = await navigationService.NavigateAsync(fullPath, parameters, useNavigationModal, animated);
            return navigationResult;
        }

        public static Task<ModalResult> PushModal<TModalPopup>(this INavigationService navigationService, INavigationParameters? parameters = null, bool animated = true)
            where TModalPopup : ModalViewModel
        {
            return PushModal<TModalPopup, ModalResult>(navigationService, parameters, animated);
        }

        public static async Task<TReturnType?> PushModal<TModalPopup, TReturnType>(this INavigationService navigationService, INavigationParameters? parameters = null, bool animated = true)
            where TModalPopup : ModalViewModel<TReturnType>
        {
            parameters ??= new NavigationParameters();
            var completion = new TaskCompletionSource<TReturnType>();
            parameters.Add(nameof(ModalViewModel.TaskCompletion), completion);
            var navResult = await navigationService.PushAsync<TModalPopup>(parameters, animated: animated);

            Contract.ThrowOn(navResult.Exception);

            return await completion.Task;
        }

        private static string ToPageName<TViewModel>() where TViewModel : BaseViewModel
        {
            return typeof(TViewModel).Name.Replace(KnownViewModelPattern, KnownPagePattern);
        }

        private static async Task<NavigationResult?> PerformValidation(
            Type pageType,
            INavigationService navigationService,
            INavigationParameters? parameters)
        {
            if (Validator == null)
                return null;

            var pageInfo = new ViewModelMetadata(pageType);
            var shouldProceed = await Validator.Invoke(navigationService, parameters, pageInfo);

            if (shouldProceed)
                return null;

            return new NavigationResult
            {
                Success = false,
                Exception = new NavigationValidationException(pageInfo)
            };
        }
    }
}

using Rg.Plugins.Popup.Pages;
using XamarinUtility.ViewModels;

namespace XamarinUtility.Pages.Modals
{
    public class BaseModal : PopupPage
    {
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            if (BindingContext is IDismissableModal viewModel)
                return viewModel.DismissOnBackButtonPress();
            return false;
        }

        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            if (BindingContext is IDismissableModal viewModel)
                return viewModel.DismissOnBackgroundClick();
            return false;
        }
    }
}

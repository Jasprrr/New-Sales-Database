namespace SchoolsMailing
{
    using Models;
    using SchoolsMailing.ViewModels.Common;
    using System.IO;
    using Windows.Foundation.Metadata;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class MainPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += this.OnLoaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += this.OnBackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            this.DataContextChanged += this.OnDataContextChanged;
        }

        private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var viewModel = this.DataContext as PageViewModel;
            viewModel?.SetPageFrame(this.AppFrame);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
            var viewModel = this.DataContext as PageViewModel;
            viewModel?.NavigateBack(args);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ToggleMenuButton.Focus(FocusState.Programmatic);
        }

        private void OnMenuButtonChecked(object sender, RoutedEventArgs e)
        {
            if (this.AppMenu.DisplayMode == SplitViewDisplayMode.Inline
                || this.AppMenu.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                this.ToggleMenuButton.TransformToVisual(this);
            }
        }
    }
}
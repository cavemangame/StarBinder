using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Core.Factories;
using XF.Core.ViewModels;

namespace XF.Core.Services
{
    public class Navigator : INavigator
    {
        private readonly Lazy<INavigation> navigation;
        private readonly IViewFactory viewFactory;

        public Navigator(Lazy<INavigation> navigation, IViewFactory viewFactory)
        {
            this.navigation = navigation;
            this.viewFactory = viewFactory;
        }

        private INavigation Navigation
        {
            get { return navigation.Value; }
        }

        public async Task<IViewModel> PopAsync()
        {
            Page view = await Navigation.PopAsync();
            var viewModel = view.BindingContext as IViewModel;
            viewModel.NavigatedFrom();
            return view.BindingContext as IViewModel;
        }

        public async Task<IViewModel> PopModalAsync()
        {
            Page view = await Navigation.PopModalAsync();
            var viewModel = view.BindingContext as IViewModel;
            viewModel.NavigatedFrom();
            return view.BindingContext as IViewModel;
        }

        public async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync();
        }

        public async Task<TViewModel> ChangeRootAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            Navigation.InsertPageBefore(view, Navigation.NavigationStack[0]);
            await Navigation.PopToRootAsync();
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> ChangeRootAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = viewFactory.Resolve(viewModel);
            Navigation.InsertPageBefore(view, Navigation.NavigationStack[0]);
            await Navigation.PopToRootAsync();
            viewModel.NavigatedTo();
            return viewModel;
        }


        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IViewModel
        {
            var view = viewFactory.Resolve(viewModel);
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            await Navigation.PushModalAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IViewModel
        {
            var view = viewFactory.Resolve(viewModel);
            await Navigation.PushModalAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }
    }
}

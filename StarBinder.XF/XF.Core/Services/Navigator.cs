using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Core.Factories;
using XF.Core.ViewModels;

namespace XF.Core.Services
{
    public class Navigator : INavigator
    {
        private readonly IViewFactory viewFactory;
        private readonly Func<Page> getPage;

        public Navigator(IViewFactory viewFactory, Func<Page> getPage)
        {
            this.viewFactory = viewFactory;
            this.getPage = getPage;
        }

        private INavigation Navigation { get { return getPage().Navigation; } }

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
            Application.Current.MainPage = view;
            viewModel.NavigatedTo();

            return viewModel;
        }

        public async Task<TViewModel> ChangeRootAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = viewFactory.Resolve(viewModel);
            Application.Current.MainPage = view;
            viewModel.NavigatedTo();

            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            
            var canNavigate = viewModel as ICanNavigatableViewModel;
            if (canNavigate != null && !await canNavigate.CheckCanNavigate())
            {
                return viewModel;
            }

            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();

            var md = Application.Current.MainPage as MasterDetailPage;
            if (md != null) md.IsPresented = false;

            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var canNavigate = viewModel as ICanNavigatableViewModel;
            if (canNavigate != null && !await canNavigate.CheckCanNavigate())
            {
                return viewModel;
            }
            
            var view = viewFactory.Resolve(viewModel);
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();

            var md = Application.Current.MainPage as MasterDetailPage;
            if (md != null) md.IsPresented = false;

            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);

            var canNavigate = viewModel as ICanNavigatableViewModel;
            if (canNavigate != null && !await canNavigate.CheckCanNavigate())
            {
                return viewModel;
            }

            await Navigation.PushModalAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var canNavigate = viewModel as ICanNavigatableViewModel;
            if (canNavigate != null && !await canNavigate.CheckCanNavigate())
            {
                return viewModel;
            }
            
            var view = viewFactory.Resolve(viewModel);
            await Navigation.PushModalAsync(view);
            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> SetDetail<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var md = Application.Current.MainPage as MasterDetailPage;
            if (md == null)
            {
                throw new InvalidOperationException("App.MainPage is not master detail");
            }

            var canNavigate = viewModel as ICanNavigatableViewModel;
            if (canNavigate != null && !await canNavigate.CheckCanNavigate())
            {
                return viewModel;
            }

            var view = viewFactory.Resolve(viewModel);
            viewModel.NavigatedTo();
            md.Detail = new NavigationPage(view);
            md.IsPresented = false;

            return viewModel;
        }

        public async Task<TViewModel> SetDetail<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var md = Application.Current.MainPage as MasterDetailPage;
            if (md == null)
            {
                throw new InvalidOperationException("App.MainPage is not master detail");
            }

            TViewModel viewModel;
            var view = viewFactory.Resolve(out viewModel, setStateAction);

            var canNavigate = viewModel as ICanNavigatableViewModel;
            if (canNavigate != null && !await canNavigate.CheckCanNavigate())
            {
                return viewModel;
            }

            viewModel.NavigatedTo();
            md.Detail = new NavigationPage(view);
            md.IsPresented = false;

            return viewModel;
        }
    }
}

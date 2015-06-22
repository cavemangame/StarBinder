using System;
using System.Collections.Generic;
using Autofac;
using Xamarin.Forms;
using XF.Core.ViewModels;

namespace XF.Core.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> map = new Dictionary<Type, Type>();
        private readonly IComponentContext componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public void Register<TViewModel, TView>() 
            where TViewModel : class, IViewModel 
            where TView : Page
        {
            map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve<TViewModel>(out viewModel, setStateAction);
        }

        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel
        {
            viewModel = componentContext.Resolve<TViewModel>();
            
            var viewType = map[typeof(TViewModel)];
            var view = componentContext.Resolve(viewType) as Page;
            
            if (setStateAction != null) setStateAction(viewModel);

            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IViewModel
        {
            var viewType = map[typeof(TViewModel)];
            var view = componentContext.Resolve(viewType) as Page;
            view.BindingContext = viewModel;
            return view;
        }
    }
}

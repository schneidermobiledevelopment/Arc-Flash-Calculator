using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class LeftMenuViewModel : BindableBase
  {
    //public DelegateCommand NavigateCommand { get; set; }
    private INavigationService NavigationService { get; }

    private bool isPresented;

    public bool IsPresented;
    MenuItem _menuItem;
    public MenuItem SelectedMenu
    {
      get { return _menuItem; }
      set
      {
        if (SetProperty(ref _menuItem, value))
          OnSelectedChangedCommand.Execute(value);
      }
    }
    IEnumerable<MenuItem> _menu;
    public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }

    DelegateCommand<MenuItem> _onSelectedChangedCommand;
    ICommand OnSelectedChangedCommand
    {
      get
      {

        return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<MenuItem>((item) =>
        {
          if (item == null)
            return;
          NavigationService.NavigateAsync(item.PageName);
          this.isPresented = false;
          //var vmType = item.ViewModelType;
          
          // We demand to clear the Navigation stack as we are changing the section
          //var presentationBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "ClearStack" } });

          // Show the ViewModel in the Detail NavigationPage
          //ChangePresentation(new MvxClosePresentationHint(this));
          //SelectedMenu = null;
          //ShowViewModel(vmType, presentationBundle: presentationBundle);

        }));
      }
    }

    //public ObservableCollection<MenuItem> Menu { get; } = new ObservableCollection<MenuItem>
    //{
    //   new MenuItem { Title = "About", PageName = "About",icon =ImageSource.FromFile("about.png" ), Uri="About"},
    //    new MenuItem { Title = "Settings", PageName = "Settings",icon =ImageSource.FromFile("settings.png" ) },
    //    new MenuItem { Title = "Start Over", PageName = "CommonInput" ,icon =ImageSource.FromFile("startOver")},
    //};
    //IEnumerable<MenuItem> _menu;
    //public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }
    //DelegateCommand<MenuItem> _onSelectedChangedCommand;
    //ICommand OnSelectedChangedCommand
    //{
    //  get
    //  {

    //    return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<MenuItem>((item) =>
    //    {
    //      if (item == null)
    //        return;

    //      //var vmType = item.ViewModelType;

    //      //// We demand to clear the Navigation stack as we are changing the section
    //      //var presentationBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "ClearStack" } });

    //      //// Show the ViewModel in the Detail NavigationPage
    //      ////ChangePresentation(new MvxClosePresentationHint(this));
    //      //SelectedMenu = null;
    //      //ShowViewModel(vmType, presentationBundle: presentationBundle);

    //    }));
    //  }

    public LeftMenuViewModel(INavigationService navigationService)
    {
      NavigationService = navigationService;
      Menu = new[] 
      {
        new MenuItem { Title = "About", PageName = "About", icon = ImageSource.FromFile("about.png") },
        new MenuItem { Title = "Settings", PageName = "Settings", icon = ImageSource.FromFile("settings.png") },
        new MenuItem { Title = "Start Over", PageName = "CommonInput", icon = ImageSource.FromFile("startOver") },
    };
      //NavigateCommand = new DelegateCommand(Navigate);
    }

    //private void Navigate()
    //{
    //  NavigationService.
    //}
  }
  public class MenuItem
  {
    public string Title { get; set; }
    public string PageName { get; set; }
    //public string Uri { get; set; }
    public ImageSource icon { get; set; }
  }
}

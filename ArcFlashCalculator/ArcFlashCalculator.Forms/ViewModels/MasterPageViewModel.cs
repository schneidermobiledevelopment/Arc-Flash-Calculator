using ArcFlashCalculator.Core.Locals;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class MasterPageViewModel : BindableBase
  {
    private readonly INavigationService _navigationService;
    IEnumerable<MenuItem> _menu;
    public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }
    private MenuItem _menuItem;
    public MenuItem SelectedMenu
    {
      get { return _menuItem; }
      set
      {
        if (SetProperty(ref _menuItem, value))
          OnSelectedChangedCommand.Execute(value);
      }
    }
    public MasterPageViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
      InitList();
    }

    public void InitList()
    {
      Menu = new[]
      {
        new MenuItem { Title = AppResources.About, PageName = "About", icon = "about.png" },
        new MenuItem { Title = AppResources.Settings, PageName = "Settings", icon = ImageSource.FromFile("Settings.png") },
        new MenuItem { Title = "Calculator", PageName = "CommonInput", icon = "Calculator.png" },
      };
    }
    public void UpdateList()
    {
      foreach (var item in Menu)
      {
       switch (item.PageName)
        {
          case "About":
            item.Title = AppResources.About;
            break;
          case "Settings":
            item.Title= AppResources.Settings;
            break;
          case "CommonInput":
            
            break;
        }
      }
    }
    private DelegateCommand<MenuItem> _onSelectedChangedCommand;
    public DelegateCommand<MenuItem> OnSelectedChangedCommand
    {
      get {
        return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<MenuItem>(async (item) =>
        {
          if (item == null)
            return;
          await _navigationService.NavigateAsync("NavPage/" + item.PageName, animated: false);
        }));
        }
      set { SetProperty(ref _onSelectedChangedCommand, value); }
    }
  }
}

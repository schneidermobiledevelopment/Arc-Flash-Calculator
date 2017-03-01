using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class MasterDetailViewModel : BindableBase
  {

    private INavigationService NavigationService { get; }
    //public DelegateCommand<string> NavigateCommand { get; set; }

    public MasterDetailViewModel(INavigationService navigationService)
    {
      this.NavigationService = navigationService;
      //NavigateCommand = new DelegateCommand<string>(Navigate);
    }

  }
}

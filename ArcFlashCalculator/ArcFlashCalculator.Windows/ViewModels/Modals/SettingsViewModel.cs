using System;
using System.Linq;
using ArcFlashCalculator.Core;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.WPF.Commands;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ArcFlashCalculator.WPF.ViewModels.Modals
{
  public class SettingsViewModel : BindableBase
  {
    private readonly IEventAggregator _eventAggregator;

    public SettingsViewModel(IEventAggregator eventAggregator)
    {
      //events
      _eventAggregator = eventAggregator;

      //commands
      CloseModalCommand = new DelegateCommand(CloseModal);
    }

    public DelegateCommand CloseModalCommand { get; set; }

    private void CloseModal()
    {
      _eventAggregator.GetEvent<CloseModalEvent>().Publish(Constants.SETTINGS_VIEW);
    }
  }
}

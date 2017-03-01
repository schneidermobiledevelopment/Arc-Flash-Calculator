using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.Forms.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class SettingsViewModel : BindableBase
  {
    private INavigationService _navigationService;

    public SettingsViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
      InitList();
    }

    public void InitList() {
      Settings = new[]
      {
        new SettingItem { Option = AppResources.Language, PageName="Language" },
        new SettingItem { Option = AppResources.Units_of_Measure, PageName="MeasuresUnits" },
        new SettingItem { Option = AppResources.Date_Format, PageName="DateFormat" },
      };
    }
    private IEnumerable<SettingItem> _settings;
    public IEnumerable<SettingItem> Settings
    {
      get { return _settings; }
      set { SetProperty(ref _settings, value); }
    }
    private SettingItem _selectedSetting;
    public SettingItem SelectedSetting
    {
      get { return _selectedSetting; }
      set
      {
        if (SetProperty(ref _selectedSetting, value))
          OnSelectedChangedCommand.Execute(value);
      }
    }
    private DelegateCommand<SettingItem> _onSelectedChangedCommand;
    public DelegateCommand<SettingItem> OnSelectedChangedCommand
    {
      get
      {
        return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<SettingItem>(async (item) =>
        {
          if (item == null)
            return;

          _selectedSetting = default(SettingItem);
          await _navigationService.NavigateAsync(item.PageName);
        }));
      }
      set { SetProperty(ref _onSelectedChangedCommand, value); }
    }
  }
}

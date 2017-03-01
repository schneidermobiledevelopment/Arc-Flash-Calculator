using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.Forms.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class LanguageViewModel : BindableBase
  {
    private readonly ILocalize _localize;
    private readonly INavigationService _navigationService;
    public LanguageViewModel(INavigationService navigationService, ILocalize localize)
    {
      _localize = localize;
      TabTitle = AppResources.Language;
      _navigationService = navigationService;
      Languages = new[]
       {
        new LanguageItem { Language = "English"},
        new LanguageItem { Language = "Español"},
        new LanguageItem { Language = "Français"},
        new LanguageItem { Language = "中国"}
      };
      foreach (var item in Languages)
      {
        if (LanguageSettings.CurrentCulture.TwoLetterISOLanguageName == "en" && item.Language == "English")
        {
          item.Checked = true;
        }
        else if (LanguageSettings.CurrentCulture.TwoLetterISOLanguageName == "es" && item.Language == "Español")
        {
          item.Checked = true;
        }
      }
    }

    private string _tabTitle;
    public string TabTitle
    {
      get { return _tabTitle; }
      set { SetProperty(ref _tabTitle, value); }
    }

    private IEnumerable<LanguageItem> _languages;
    public IEnumerable<LanguageItem> Languages
    {
      get { return _languages; }
      set { SetProperty(ref _languages, value); }
    }

    private LanguageItem _selectedLanguage;
    public LanguageItem SelectedLanguage
    {
      get { return _selectedLanguage; }
      set
      {
        if (SetProperty(ref _selectedLanguage, value))
          OnSelectedChangedCommand.Execute(value);
      }
    }
    private DelegateCommand<LanguageItem> _onSelectedChangedCommand;
    public DelegateCommand<LanguageItem> OnSelectedChangedCommand
    {
      get
      {
        return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<LanguageItem>((item) =>
        {
          if (item == null)
            return;
          foreach (var lang in Languages)
          {
            lang.Checked = false;
          }
          item.Checked = true;
          switch (item.Language)
          {
            case "English":
              LanguageSettings.CurrentCulture = new CultureInfo("en");
              break;
            case "Español":
              item.Checked = true;
              LanguageSettings.CurrentCulture = new CultureInfo("es");
              break;
          }
          AppResources.Culture = LanguageSettings.CurrentCulture;
          _localize.SetLocale(LanguageSettings.CurrentCulture);
          
        }));
      }
      set { SetProperty(ref _onSelectedChangedCommand, value); }
    }


  }
  public class LanguageItem : BindableBase
  {
    public string Language { get; set; }
    private bool _checked;
    public bool Checked
    {
      get { return _checked; }
      set { SetProperty(ref _checked, value); }
    }

  }
}

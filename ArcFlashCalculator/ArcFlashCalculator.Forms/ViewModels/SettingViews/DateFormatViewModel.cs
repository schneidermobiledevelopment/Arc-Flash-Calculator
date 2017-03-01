using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class DateFormatViewModel : BindableBase
  {
    private readonly INavigationService _navigationService;
    public DateFormatViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
      DateFormats = new[]
       {
        new DateFormatItem { DateFormat = "MM-DD-YYYY", Checked= true},
        new DateFormatItem { DateFormat = "DD-MM-YYYY"},
      };
    }

    private IEnumerable<DateFormatItem> _dateFormats;
    public IEnumerable<DateFormatItem> DateFormats
    {
      get { return _dateFormats; }
      set { SetProperty(ref _dateFormats, value); }
    }

    private DateFormatItem _selectedDateFormat;
    public DateFormatItem SelectedDateFormat
    {
      get { return _selectedDateFormat; }
      set
      {
        if (SetProperty(ref _selectedDateFormat, value))
          OnSelectedChangedCommand.Execute(value);
      }
    }
    private DelegateCommand<DateFormatItem> _onSelectedChangedCommand;
    public DelegateCommand<DateFormatItem> OnSelectedChangedCommand
    {
      get
      {
        return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<DateFormatItem>((item) =>
        {
        foreach (var DateFormat in DateFormats)
        {
          DateFormat.Checked = false;
        }
        item.Checked = true;
        }));
      }
      set { SetProperty(ref _onSelectedChangedCommand, value); }
    }


  }
  public class DateFormatItem : BindableBase
  {
    public string DateFormat { get; set; }
    private bool _checked;
    public bool Checked
    {
      get { return _checked; }
      set { SetProperty(ref _checked, value); }
    }

  }
}

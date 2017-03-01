using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class MeasuresUnitsViewModel : BindableBase
  {
    private readonly INavigationService _navigationService;
    public MeasuresUnitsViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
      UnitsOfMeasure = new[]
       {
        new UnitOfMeasureItem { UnitOfMeasure = "Imperial(feet and inches)", Checked= true},
        new UnitOfMeasureItem { UnitOfMeasure = "Metric/SI(meters or cm)"},
      };
    }

    private IEnumerable<UnitOfMeasureItem> _unitsOfMeasure;
    public IEnumerable<UnitOfMeasureItem> UnitsOfMeasure
    {
      get { return _unitsOfMeasure; }
      set { SetProperty(ref _unitsOfMeasure, value); }
    }

    private UnitOfMeasureItem _selectedUnitOfMeasure;
    public UnitOfMeasureItem SelectedUnitOfMeasure
    {
      get { return _selectedUnitOfMeasure; }
      set
      {
        if (SetProperty(ref _selectedUnitOfMeasure, value))
          OnSelectedChangedCommand.Execute(value);
      }
    }
    private DelegateCommand<UnitOfMeasureItem> _onSelectedChangedCommand;
    public DelegateCommand<UnitOfMeasureItem> OnSelectedChangedCommand
    {
      get
      {
        return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new DelegateCommand<UnitOfMeasureItem>((item) =>
        {
          foreach (var UnitOfMeasure in UnitsOfMeasure)
          {
            UnitOfMeasure.Checked = false;
          }
          item.Checked = true;
        }));
      }
      set { SetProperty(ref _onSelectedChangedCommand, value); }
    }


  }
  public class UnitOfMeasureItem : BindableBase
  {
    public string UnitOfMeasure { get; set; }
    private bool _checked;
    public bool Checked
    {
      get { return _checked; }
      set { SetProperty(ref _checked, value); }
    }

  }
}

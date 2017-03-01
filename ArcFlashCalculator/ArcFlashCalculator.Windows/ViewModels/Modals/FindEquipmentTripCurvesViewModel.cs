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
  public class FindEquipmentTripCurvesViewModel : BindableBase
  {
    private readonly IEventAggregator _eventAggregator;

    public FindEquipmentTripCurvesViewModel(IEventAggregator eventAggregator)
    {
      //events
      _eventAggregator = eventAggregator;

      //commands
      CloseModalCommand = new DelegateCommand(CloseModal);

      SquareDTripCurvesLink =
        @"http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
    }

    private void CloseModal()
    {
      _eventAggregator.GetEvent<CloseModalEvent>().Publish(Constants.FIND_EQUIPMENT_TRIP_CURVES_VIEW);
    }

    public DelegateCommand CloseModalCommand { get; set; }

    private string _squareDTripCurvesLink;
    public string SquareDTripCurvesLink
    {
      get { return _squareDTripCurvesLink; }
      set { SetProperty(ref _squareDTripCurvesLink, value); }
    }
  }
}

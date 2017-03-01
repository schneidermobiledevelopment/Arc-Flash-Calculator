using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ParameterSummaryViewModel : BindableBase,INavigationAware
  {
    private INavigationService _navigationService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IUnityContainer _container;
    private bool _isClicked;


    public ParameterSummaryViewModel(INavigationService navigationService, IUnityContainer container, ICalculationInput calculationInput, ICalculationOutput calculationOutput, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _calculationOutput= calculationOutput as CalculationOutput;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
      

    }

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      InitList();
      _eventAggregator.GetEvent<CalculationInputUpdated>().Unsubscribe(HandleCalculationInputUpdated);
    }

    private CalculationInput _calculationInput;
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      { SetProperty(ref _calculationInput, value);
      }
    }
    private CalculationOutput _calculationOutput;
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set
      {
        SetProperty(ref _calculationOutput, value);
      }
    }
    private ObservableCollection<SummaryItem> _summary;
    public ObservableCollection<SummaryItem> Summary
    {
      get { return _summary; }
      set { SetProperty(ref _summary, value); }
    }

    public void InitList()
    {
      string temp=null;
      var p = new NavigationParameters();
      p.Add("calculationInput", CalculationInput);
      Summary = new ObservableCollection<SummaryItem>
      {
        new SummaryItem { Title = AppResources.Personnel, Description=_calculationInput.Personnel, EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field","Personnel");
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput",p);
          })},
        new SummaryItem { Title = AppResources.Location, Description = _calculationInput.Location,EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field","Location");
           await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput",p);
          })},
        new SummaryItem { Title = AppResources.Action, Description = _calculationInput.Action, EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field","Action");
           await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput",p);
          })},
        new SummaryItem { Title = AppResources.Date, Description = _calculationInput.CalculationDate.ToString("dddd, MMMM d, yyyy" ), EditCommand = new DelegateCommand(
          async () =>
          {
           await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput",p);
          })},
      };
      if (CalculationInput.IsAlternatingCurrent.Value)
      {
        Summary.Add(new SummaryItem { Title = AppResources.Voltage_Type, Description = AppResources.Voltage_Type_AC,
          EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput", p);
          })
        });
        Summary.Add(new SummaryItem { Title = AppResources.Equipment_Type, Description = CalculationInput.EquipmentType.Name, EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne", p);
          })
        });
        temp = (CalculationInput.IsSolidGround.Value) ? AppResources.Solidly_Grounded : AppResources.Ungrounded;
        Summary.Add(new SummaryItem { Title = AppResources.Grounding_Method, Description = temp ,
          EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne", p);
          })
        });
        Summary.Add(new SummaryItem { Title = AppResources.Nominal_Working_Voltage, Description = CalculationInput.NominalVoltage.ToString(), EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field", "Nominal");
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne", p);
          })
        });
        Summary.Add(new SummaryItem { Title = AppResources.Source_Fault_Current_amps, Description = CalculationInput.SourceFaultCurrent.ToString(), EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field", "Source");
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne", p);
          })
        });
        temp = (CalculationInput.IsOpenAir.Value) ? AppResources.Open_Air : AppResources.In_Box;
        Summary.Add(new SummaryItem { Title = AppResources.Equipment_Enclosure, Description = temp,
          EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne/ACInputTwo", p);
          })
        });
        
        temp = (CalculationInput.HasTransformer.Value) ? AppResources.Upstream_XFR : AppResources.No_Transformer;
        Summary.Add(new SummaryItem { Title = AppResources.Transformer, Description = temp ,
          EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne/ACInputTwo", p);
          })
        });
        if (CalculationInput.HasTransformer.Value)
        {
          var tempXmfr = (CalculationInput.HasCable.Value) ? "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACCableAndXfmr" : "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACHasXfmr";
          Summary.Add(new SummaryItem { Title = AppResources.Primary_Voltage, Description = CalculationInput.PrimaryVoltage.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("Field", "PrimaryVoltage");
                await _navigationService.NavigateAsync(tempXmfr, p);
              })
          });
          Summary.Add(new SummaryItem { Title = AppResources.Transformer_Impedence, Description = CalculationInput.XfmrImpedance.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("Field", "XfmrImpedance");
                await _navigationService.NavigateAsync(tempXmfr, p);
              })
          });
          Summary.Add(new SummaryItem { Title = AppResources.KVA, Description = CalculationInput.XfmrKVA.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("Field", "TransformerKVA");
                await _navigationService.NavigateAsync(tempXmfr, p);
              })
          });
        }
        temp = (CalculationInput.HasCable.Value) ? AppResources.Cable : AppResources.No_Cable;
        Summary.Add(new SummaryItem
        {
          Title = AppResources.Conductor,
          Description = temp,
          EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne/ACInputTwo", p);
          })
        });
        if (CalculationInput.HasCable.Value)
        {
          var tempCable = (CalculationInput.HasTransformer.Value) ? "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACCableAndXfmr" : "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACHasCable";
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Conductor_Size,
            Description = CalculationInput.ConductorSize.Size,
            EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync(tempCable);
          })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Conductors_per_Phase,
            Description = CalculationInput.ConductorPerPhase.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("Field", "ConductorPerPhase");
                await _navigationService.NavigateAsync(tempCable, p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Conductor_Length,
            Description = CalculationInput.ConductorLength.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("Field", "ConductorLenght");
                await _navigationService.NavigateAsync(tempCable, p);
              })
          });
        }
        if (CalculationInput.HasCable.Value)
        {
          if (CalculationInput.HasTransformer.Value)
          {
            temp = "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACCableAndXfmr/ACSensor";
          }
          else
          {
            temp = "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACHasCable/ACSensor";
          }
        }
        else if (CalculationInput.HasTransformer.Value)
        {
          temp = "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACHasXfmr/ACSensor";
        }
        else
        {
          temp = "app:///NavPage/CommonInput/ACInputOne/ACInputTwo/ACSensor";
        }
        Summary.Add(new SummaryItem { Title = AppResources.Sensor_Rating, Description = CalculationInput.SensorRating.ToString(),
          EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field", "Sensor");
            await _navigationService.NavigateAsync(temp, p);
          })
        });
        Summary.Add(new SummaryItem { Title = AppResources.Arc_Duration_seconds, Description = CalculationInput.ArcDurationValue.ToString(),
          EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field", "ArcDuration");
            await _navigationService.NavigateAsync(temp, p);
          })
        });
        if (CalculationInput.IsArcDurationCalculated)
        {
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Device_Manufacturer,
            Description = CalculationInput.ArcDuration.Manufacturer.Name,
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp,p);
                //await _navigationService.NavigateAsync("NavPage/CalculateArcDurationOne", p, true);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Breaker_Style,
            Description = CalculationInput.ArcDuration.BreakerStyle.Name,
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp);
                await _navigationService.NavigateAsync("NavPage/CalculateArcDurationOne", p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Trip_Unit_Type,
            Description = CalculationInput.ArcDuration.TripUnitType.Name,
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp);
                await _navigationService.NavigateAsync("NavPage/CalculateArcDurationOne", p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Long_Time_Pickup,
            Description = CalculationInput.ArcDuration.LongTimePickup.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                p.Add("Field", "Long Time Pickup");
                await _navigationService.NavigateAsync(temp, p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Long_Time_Delay,
            Description = CalculationInput.ArcDuration.LongTimeDelay.Value.ToString(),
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp, p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Short_Time_Pickup,
            Description = CalculationInput.ArcDuration.ShortTimePickup.Value,
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp, p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Short_Time_Delay,
            Description = CalculationInput.ArcDuration.ShortTimeDelay.Value,
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp, p);
              })
          });
          Summary.Add(new SummaryItem
          {
            Title = AppResources.Instantaneous,
            Description = CalculationInput.ArcDuration.Instantaneous.Value,
            EditCommand = new DelegateCommand(
              async () =>
              {
                p.Add("CalculationOutput", CalculationOutput);
                await _navigationService.NavigateAsync(temp, p);
              })
          });

        }


      }
      else if (!CalculationInput.IsAlternatingCurrent.Value)
      {
        Summary.Add(new SummaryItem { Title = AppResources.Voltage_Type, Description = AppResources.Voltage_Type_DC, EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput", p);
          })
        });
        Summary.Add(new SummaryItem { Title = AppResources.Maximum_Available_Short_Circuit, Description = CalculationInput.MaximumShortCircuitAvailable.ToString(),
          EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field", "MaximumAvailableShortCircuit");
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/DCInput", p);
          })
        });
        temp = (CalculationInput.InCabinet.Value) ? AppResources.In_Box : AppResources.Open_Air;
        Summary.Add(new SummaryItem { Title = AppResources.Battery_Enclosure, Description = temp, EditCommand = new DelegateCommand(
          async () =>
          {
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/DCInput", p);
          })
        });
        Summary.Add(new SummaryItem { Title = AppResources.Battery_String_Voltage, Description = CalculationInput.VoltageOfBattery.ToString(),
          EditCommand = new DelegateCommand(
          async () =>
          {
            p.Add("Field", "BatteryStringVoltage");
            await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/DCInput", p);
          })
        });

      }
    }

    public DelegateCommand BackCommand => new DelegateCommand(Back);

    private async void Back()
    {
      await _navigationService.GoBackAsync();
    }

    public DelegateCommand NextCommand => new DelegateCommand(Next);

    private async void Next()
    {
      var p = new NavigationParameters();
      CalculationInput.HasCustomWorkingDistance = false;
      p.Add("calculationInput", _calculationInput);
      p.Add("Summary",true);
      await _navigationService.NavigateAsync("ACResult",p);
    }

    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver, CanStartOver);

    private bool CanStartOver()
    {
      return !_isClicked;
    }

    private async void StartOver()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new StartOverPopup());
      _isClicked = false;
    }

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      //if (parameters.ContainsKey("CalculationOutput"))
      //  CalculationOutput = (CalculationOutput)parameters["CalculationOutput"];
      //if (parameters.ContainsKey("calculationInput"))
      //{
       // CalculationInput = (CalculationInput)parameters["calculationInput"];
      //  InitList();
      //}
    }
  }
  public class SummaryItem
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public DelegateCommand EditCommand { get; set; }
  }
}

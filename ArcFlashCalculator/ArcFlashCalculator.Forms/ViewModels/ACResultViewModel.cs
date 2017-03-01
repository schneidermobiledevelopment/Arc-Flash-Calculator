using System;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Forms.Views;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ACResultViewModel : BindableBase, INavigationAware
  {
    private INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private CalculationInput _calculationInput;
    private ICalculatorService _calculatorService;
    private CalculationOutput _calculationOutput;
    private readonly IEventAggregator _eventAggregator;
    private bool _isClicked;
    private readonly IPageDialogService _dialogService;
    private string _clothingDescription;
    private string _requiredMinimumArcRating;
    private readonly ISaveAndLoad _saveAndLoad;
    private readonly IPdfCreatorService _pdfCreator;

    public ACResultViewModel(IPdfCreatorService pdfCreator, ISaveAndLoad saveAndLoad, INavigationService navigationService, IUnityContainer container, IDataService dataService,ICalculationOutput calculationOutput, ICalculationInput calculationInput,ICalculatorService calculatorService, IPageDialogService dialogService, IEventAggregator eventAggregator)
    {
      _pdfCreator = pdfCreator;
      _saveAndLoad = saveAndLoad;
      _navigationService = navigationService;
      _dataService = dataService;
      _calculationInput = calculationInput as CalculationInput;
      _calculationOutput = calculationOutput as CalculationOutput;
      _container = container;
      _calculatorService = calculatorService;
      _dialogService = dialogService;
      _eventAggregator = eventAggregator;
      //_eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);
      //_eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
    }

    //private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    //{
    //  CalculationInput = calculationInput;
    //  CalculationOutput = (CalculationOutput)_calculatorService.Calculate(CalculationInput);
     
    //  _eventAggregator.GetEvent<CalculationInputUpdated>().Unsubscribe(HandleCalculationInputUpdated);

    //}
    //private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    //{
    //  CalculationOutput = calculationOutput;
      
    //}

    private ObservableCollection<CellItem> _resultsP1;
    public ObservableCollection<CellItem> ResultsP1
    {
      get { return _resultsP1; }
      set { SetProperty(ref _resultsP1, value); }
    }
    private ObservableCollection<CellItem> _resultsP2;
    public ObservableCollection<CellItem> ResultsP2
    {
      get { return _resultsP2; }
      set { SetProperty(ref _resultsP2, value); }
    }
    private ObservableCollection<CellItem> _resultsP3;
    public ObservableCollection<CellItem> ResultsP3
    {
      get { return _resultsP3; }
      set { SetProperty(ref _resultsP3, value); }
    }
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set { SetProperty(ref _calculationInput, value); }
    }
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }
    private int _position;
    public int Position
    {
      get { return _position; }
      set { SetProperty(ref _position, value); }
    }
    private ObservableCollection<ResultItems> _result;
    public ObservableCollection<ResultItems> Result
    {
      get { return _result; }
      set { SetProperty(ref _result, value); }
    }

    private decimal? _originalWorkingDistance;
    public decimal? OriginalWorkingDistance
    {
      get { return _originalWorkingDistance; }
      set { SetProperty(ref _originalWorkingDistance, value); }
    }

    private decimal? _originalIncidentEnergy;
    public decimal? OriginalIncidentEnergy
    {
      get { return _originalIncidentEnergy; }
      set { SetProperty(ref _originalIncidentEnergy, value); }
    }

    #region ListInitialization
    public void InitResults()
    {
      Result = new ObservableCollection<ResultItems>
      {
        new ResultItems {ViewItem = ResultsP1},
        new ResultItems {ViewItem = ResultsP2},
        new ResultItems {ViewItem = ResultsP3},
      };
    }

    public void InitResultP1()
    {
      if (CalculationOutput.HazardCat == "0")
      {
        _clothingDescription = AppResources.ClothingDescriptionDetailsOnePointTwo;
        _requiredMinimumArcRating = AppResources.LessThanEqualToOnePointTwo;
      }
      else if (CalculationOutput.HazardCat=="2")
      {
        _clothingDescription = AppResources.ClothingDescriptionDetails8PartA + " " + AppResources.Or + " " +
                              AppResources.ClothingDescriptionDetails8PartB;
        _requiredMinimumArcRating = 8.ToString();
      }
      else if (CalculationOutput.HazardCat=="4")
      {
        _clothingDescription = AppResources.ClothingDescriptionDetails40PartA + " " + AppResources.Or + " " +
                              AppResources.ClothingDescriptionDetails40PartB;
        _requiredMinimumArcRating = 40.ToString();
      }

      ResultsP1 = new ObservableCollection<CellItem>();
      if (CalculationInput.IsAlternatingCurrent.Value)
      {
       ACView1();
      }
      else
      {
        DCView1();
      }
    }

    public void InitResultP2()
    {
      ResultsP2 = new ObservableCollection<CellItem>();
      if (CalculationInput.IsAlternatingCurrent.Value)
      {
        ACView2();
      }
      else
      {
        DCView2();
      }
    }

    public void InitResultP3()
    {
      ResultsP3 = new ObservableCollection<CellItem>();
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.General_Heading,
        CellType = "Header"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Personnel,
        Text = CalculationInput.Personnel,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Location,
        Text = CalculationInput.Location,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Action,
        Text = CalculationInput.Action,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Date,
        Text = CalculationInput.CalculationDate.ToString("dddd, MMMM d, yyyy"),
        TextColor = Color.Black,
        CellType = "Normal"
      });
      var temp = (CalculationInput.IsAlternatingCurrent.Value)
        ? AppResources.Voltage_Type_AC
        : AppResources.Voltage_Type_DC;
      ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Nominal_Working_Voltage,
          Text = temp,
          TextColor = Color.Black,
          CellType = "Normal"
        });
      if (CalculationInput.IsAlternatingCurrent.Value)
      {
        ACView3();
      }
      else
      {
        DCView3();
      }
    }



    #endregion

    #region AC

    public void ACView1()
    {
      if (CalculationOutput.HazardCat != "Danger")
      {
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Incident_Energy,
          Text = $"{CalculationOutput.IncidentEnergy:N2} cal/cm²",
          TextColor = Color.FromHex("#3DCD58"),
          CellType = "TextCenteredWithHeader",
          HasError = false,
          Aligment = LayoutOptions.CenterAndExpand
        });
        if (CalculationInput.WorkingDistance == 18m || CalculationInput.WorkingDistance == 24m ||
            CalculationInput.WorkingDistance == 36m)
          ResultsP1.Add(new CellItem()
          {
            Title = AppResources.At_Working_Distance,
            Text = $"{CalculationInput.WorkingDistance} inches",
            CellType = "TextCentered",
            TextColor = Color.FromHex("#3DCD58"),
            TitleColor = Color.Black
          });
        else
          ResultsP1.Add(new CellItem()
          {
            Title = AppResources.At_Working_Distance,
            Text = $"{CalculationInput.WorkingDistance:N2} inches (NON-STANDARD)",
            CellType = "TextCentered",
            TextColor = Color.FromHex("#3DCD58"),
            TitleColor = Color.Black
          });
        if (CalculationInput.WorkingDistance != 18m && CalculationInput.WorkingDistance != 24m &&
            CalculationInput.WorkingDistance != 36m)
        {
          ResultsP1.Add(new CellItem()
          {
            Text = "Find Safe Working Distance for Available PPE",
            CellType = "Button",
            Command = FindSafeWorkingDistanceCommand
          });
        }
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.PPE_LEVEL_REQUIRED,
          Text = CalculationOutput.HazardCat,
          CellType = "TextCentered",
          TextColor = Color.FromHex("#3DCD58"),
          TitleColor = Color.Black
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Clothing_Description,
          Text = _clothingDescription,
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Required_Minimum_Arc_Rating,
          Text = _requiredMinimumArcRating,
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Insulting Gloves Class",
          Text = CalculationOutput.InsulatingGloveClass.ToString(),
          TextColor = Color.FromHex("#3DCD58"),
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Voltage Shock Hazard",
          Text = $"{CalculationInput.NominalVoltage}V AC ",
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Arc_Flash_Boundary_Feet,
          Text = $"{CalculationOutput.FPB:N2}",
          CellType = "HorizontalText",
        });
      }
      else if (CalculationOutput.HazardCat == "Danger")
      {
        ResultsP1.Add(new CellItem()
        {
          Title = "Incident Energy Exceeds 40 cal/cm²",
          Text = $"{CalculationOutput.IncidentEnergy:N2} Cal/cm²",
          TitleColor = Color.Red,
          TextColor = Color.Red,
          HasError = true,
          CellType = "TextCenteredWithHeader",
          Aligment = LayoutOptions.StartAndExpand
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.At_Working_Distance,
          Text = $"{CalculationInput.WorkingDistance} inches",
          CellType = "TextCentered",
          TextColor = Color.Red,
          TitleColor = Color.Black
        });
        ResultsP1.Add(new CellItem()
        {
          Text = "Find Safe Working Distance for Available PPE",
          CellType = "Button",
          Command = FindSafeWorkingDistanceCommand
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.PPE_LEVEL_REQUIRED,
          Text = CalculationOutput.HazardCat.ToUpper(),
          TextColor = Color.Red,
          TitleColor= Color.Black,
          CellType = "TextCenteredWithError",
          Command = ProtectiveClothingCommand
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Clothing_Description,
          Text = "N/A",
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Required_Minimum_Arc_Rating,
          Text = "N/A",
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Insulting Gloves Class",
          Text = CalculationOutput.InsulatingGloveClass.ToString(),
          TextColor = Color.Red,
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Voltage Shock Hazard",
          Text = $"{CalculationInput.NominalVoltage}V",
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Arc_Flash_Boundary_Feet,
          Text = $"{CalculationOutput.FPB:N2}",
          CellType = "HorizontalText"
        });
      }
    }
    public void ACView2()
    {
      ResultsP2.Add(new CellItem()
      {
        Title = AppResources.Nominal_Working_Voltage,
        Text = CalculationInput.NominalVoltage.ToString(),
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP2.Add(new CellItem()
      {
        Title = AppResources.Arc_Flash_Boundary_Feet,
        TitleColor = Color.FromHex("#339152"),
        Text = $"{CalculationOutput.FPB:N2}",
        TextColor = Color.Black,
        CellType = "Normal",
        FontAttributes = FontAttributes.Bold,
        
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Bolted Fault Current (amps)",
        TitleColor = Color.FromHex("#339152"),
        Text = $"{(CalculationOutput.BoltedFaultCurrent*1000):N2}",
        TextColor = Color.Black,
        CellType = "NormalWithButton",
        FontAttributes = FontAttributes.Bold,
        Command = NewCalculationCommand
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Estimated Arc Fault Current (amps)",
        TitleColor = Color.FromHex("#339152"),
        Text = $"{(CalculationOutput.EstimatedArcFaultCurrent*1000):N2}",
        TextColor = Color.Black,
        CellType = "Normal",
        FontAttributes = FontAttributes.Bold,
        
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Incident Energy 18\" (cal/cm²) ",
        TitleColor = Color.FromHex("#339152"),
        Text = $"{CalculationOutput.IE18:N2}",
        TextColor = Color.Black,
        CellType = "Normal",
        FontAttributes = FontAttributes.Bold,
        
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Incident Energy 24\" (cal/cm²) ",
        TitleColor = Color.FromHex("#339152"),
        Text = $"{CalculationOutput.IE24:N2}",
        TextColor = Color.Black,
        CellType = "Normal",
        FontAttributes = FontAttributes.Bold,
        
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Incident Energy 36\" (cal/cm²) ",
        TitleColor = Color.FromHex("#339152"),
        Text = $"{CalculationOutput.IE36:N2}",
        TextColor = Color.Black,
        CellType = "Normal",
        FontAttributes = FontAttributes.Bold,
        
      });
      ResultsP2.Add(new CellItem()
      {
        Text = AppResources.Standard_Working_Distance,
        CellType = "Link",
        Command = WorkingDistanceTableCommand
      });
    }

    public void ACView3()
    {
      string temp;
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Equipment_Type,
        Text = CalculationInput.EquipmentType.Name,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      temp = (CalculationInput.IsSolidGround.Value) ? AppResources.Solidly_Grounded: AppResources.Ungrounded;
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Grounding_Method,
        Text = temp,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Source_Fault_Current_amps,
        Text = CalculationInput.SourceFaultCurrent.ToString(),
        TextColor = Color.Black,
        CellType = "Normal"
      });
      temp = (CalculationInput.IsOpenAir.Value) ? "Open Air" : "In Box";
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Equipment_Enclosure,
        Text = temp,
        TextColor = Color.Black,
        CellType = "Normal"
      });

      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Transformer_Heading,
        CellType = "Header"
      });
      temp = (CalculationInput.HasTransformer.Value) ? AppResources.Upstream_XFR : AppResources.No_Transformer;
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Upstream_Transformer,
        Text = temp,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      if (CalculationInput.HasTransformer.Value)
      {
        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Primary_Voltage,
          Text = CalculationInput.PrimaryVoltage.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Transformer_Impedence,
          Text = CalculationInput.XfmrImpedance.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Transformer_KVA,
          Text = CalculationInput.XfmrKVA.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
      }
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Conductor_Heading,
        CellType = "Header"
      });

      temp = (CalculationInput.HasCable.Value) ?AppResources.Cable : AppResources.No_Cable;
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Conductor,
        Text = temp,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      if (CalculationInput.HasCable.Value)
      {
        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Conductor_Size,
          Text = CalculationInput.ConductorSize.Size,
          TextColor = Color.Black,
          CellType = "Normal"
        });

        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Conductors_per_Phase,
          Text = CalculationInput.ConductorPerPhase.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Conductor_Length,
          Text = CalculationInput.ConductorLength.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
      }
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Arc_Duration,
        CellType = "Header"
      });
      ResultsP3.Add(new CellItem()
      {
        Title =AppResources.Sensor_Rating,
        Text = CalculationInput.SensorRating.ToString(),
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Multiple_Of_Sensor_Rating,
        Text = $"{CalculationOutput.MultipleOfSensorRating:N2}",
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Estimated_Arc_Fault_Current_amps,
        Text = $"{CalculationOutput.EstimatedArcFaultCurrent:N2}",
        TextColor = Color.Black,
        CellType = "Normal"
      });
      var calculated = (CalculationInput.IsArcDurationCalculated) ? "CALCULATED" : "MANUAL ENTRY";
      ResultsP3.Add(new CellItem()
      {
        Title = AppResources.Arc_Duration_seconds,
        Text = $"{CalculationInput.ArcDurationValue:N2}",
        TextColor = Color.Black,
        Comment=calculated,
        CellType = "Normal"
      });
      if (CalculationInput.IsArcDurationCalculated)
      {
        ResultsP3.Add(new CellItem()
        {
          Title = AppResources.Breaker_Settings_Heading,
          CellType = "Header"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Device Manufacturer",
          Text = CalculationInput.ArcDuration.Manufacturer.Name,
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Breaker Style",
          Text = CalculationInput.ArcDuration.BreakerStyle.Name,
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Trip Unit Type",
          Text = CalculationInput.ArcDuration.TripUnitType.Name,
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Long Time Pickup",
          Text = CalculationInput.ArcDuration.LongTimePickup.Value.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Long Time Delay",
          Text = CalculationInput.ArcDuration.LongTimeDelay.Value.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Short Time Pickup",
          Text = CalculationInput.ArcDuration.ShortTimePickup.Value.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Short Time Delay",
          Text = CalculationInput.ArcDuration.ShortTimeDelay.Value.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
        ResultsP3.Add(new CellItem()
        {
          Title = "Instantaneous",
          Text = CalculationInput.ArcDuration.Instantaneous.Value.ToString(),
          TextColor = Color.Black,
          CellType = "Normal"
        });
      }

    }


    #endregion

    #region DC

    public void DCView1()
    {
      if (CalculationOutput.HazardCat != "Danger")
      {
        ResultsP1.Add(new CellItem()
        {
          Title = "Incident Energy",
          Text = $"{CalculationOutput.IncidentEnergy:N2} Cal/cm²",
          TextColor = Color.FromHex("#3DCD58"),
          CellType = "TextCenteredWithHeader",
          HasError = false,
          Aligment = LayoutOptions.CenterAndExpand
        });
        if (CalculationInput.WorkingDistance == 18m)
          ResultsP1.Add(new CellItem()
          {
            Title = "@ Working Distance",
            Text = $"{CalculationInput.WorkingDistance} inches",
            CellType = "TextCentered",
            TextColor = Color.FromHex("#3DCD58"),
            TitleColor = Color.Black
          });
        else
          ResultsP1.Add(new CellItem()
          {
            Title = "@ Working Distance",
            Text = $"{CalculationInput.WorkingDistance:N2} inches (NON-STANDARD)",
            CellType = "TextCentered",
            TextColor = Color.FromHex("#3DCD58"),
            TitleColor = Color.Black
          });
        if (CalculationInput.WorkingDistance != 18m && CalculationInput.WorkingDistance != 24m &&
            CalculationInput.WorkingDistance != 36m)
        {
          ResultsP1.Add(new CellItem()
          {
            Text = "Find Safe Working Distance for Available PPE",
            CellType = "Button",
            Command = FindSafeWorkingDistanceCommand
          });
        }
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.PPE_LEVEL_REQUIRED,
          Text = CalculationOutput.HazardCat,
          CellType = "TextCentered",
          TextColor = Color.FromHex("#3DCD58"),
          TitleColor = Color.Black
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Clothing_Description,
          Text = _clothingDescription,
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Required_Minimum_Arc_Rating,
          Text = _requiredMinimumArcRating,
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Insulting Gloves Class",
          Text = CalculationOutput.InsulatingGloveClass.ToString(),
          TextColor = Color.FromHex("#3DCD58"),
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Voltage Shock Hazard",
          Text = $"{CalculationInput.VoltageOfBattery}V",
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Arc_Flash_Boundary_Inches,
          Text = $"{CalculationOutput.ArcFlashBoundary:N2}",
          CellType = "HorizontalText"
        });
      }
      else if (CalculationOutput.HazardCat == "Danger")
      {
        ResultsP1.Add(new CellItem()
        {
          Title = "Incident Energy Exceeds 40 cal/cm²",
          Text = $"{CalculationOutput.IncidentEnergy:N2} Cal/cm²",
          TitleColor = Color.Red,
          TextColor = Color.Red,
          HasError = true,
          CellType = "TextCenteredWithHeader",
          Aligment = LayoutOptions.StartAndExpand
        });
          ResultsP1.Add(new CellItem()
          {
            Title = AppResources.At_Working_Distance,
            Text = $"{CalculationInput.WorkingDistance} inches",
            CellType = "TextCentered",
            TextColor = Color.Red,
            TitleColor = Color.Black
          });
        ResultsP1.Add(new CellItem()
        {
          Text = "Find Safe Working Distance for Available PPE",
          CellType = "Button",
          Command = FindSafeWorkingDistanceCommand
          //TO-DO check safe working distances fot DC
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.PPE_LEVEL_REQUIRED,
          Text = CalculationOutput.HazardCat.ToUpper(),
          TextColor = Color.Red,
          CellType = "TextCenteredWithError",
          Command = ProtectiveClothingCommand,
          TitleColor =  Color.Black
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Clothing_Description,
          Text= "N/A",
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Required_Minimum_Arc_Rating,
          Text = "N/A",
          CellType = "Normal"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Insulting Gloves Class",
          Text = CalculationOutput.InsulatingGloveClass.ToString(),
          TextColor = Color.Red,
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = "Voltage Shock Hazard",
          Text = $"{CalculationInput.VoltageOfBattery}V",
          CellType = "HorizontalText"
        });
        ResultsP1.Add(new CellItem()
        {
          Title = AppResources.Arc_Flash_Boundary_Inches,
          Text = $"{CalculationOutput.ArcFlashBoundary:N2}",
          CellType = "HorizontalText"
        });
      }
    }

    public void DCView2()
    {
      ResultsP2.Add(new CellItem()
      {
        Title = "Battery String Voltage (volts)",
        Text = CalculationInput.VoltageOfBattery.ToString(),
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Maximum Available Short Circuit (amps)",
        Text = CalculationInput.MaximumShortCircuitAvailable.ToString(),
        TextColor = Color.Black,
        CellType = "Normal"
      });
      var temp = (CalculationInput.InCabinet.Value) ? "In Box" : "Open Air";
      ResultsP2.Add(new CellItem()
      {
        Title = "Battery Enclosure?",
        Text = temp,
        TextColor = Color.Black,
        CellType = "Normal"
      });
      ResultsP2.Add(new CellItem()
      {
        Title = "Incident Energy at less than 50 volts",
        TitleColor = Color.FromHex("#339152"),
        Text = $"{CalculationOutput.IE50v:N2} cal/cm²",
        TextColor = Color.Black,
        CellType = "Normal",
        FontAttributes = FontAttributes.Bold,
        
      });
    }

    public void DCView3()
    {
      

    }
    #endregion

    #region Commands

    public DelegateCommand ExportCommand=> new DelegateCommand(Export);

    private async void Export()
    {
      var action = await _dialogService.DisplayActionSheetAsync("Export Results","Cancel",null,"Archive to BFS...", "Export PDF File...");
      switch (action)
      {
        case "Export PDF File...":
          MemoryStream memoryStream = _pdfCreator.CreatePdfMemoryStream(CalculationInput, CalculationOutput);
          await _saveAndLoad.SaveAndCreatePdfAsync("ArcFlashEstimateReport.pdf", memoryStream);
          await _navigationService.NavigateAsync("NavPage/PdfView", null, true);
          break;
          
      }
      Debug.WriteLine("Answer: " + action);
    }

    public DelegateCommand BackCommand => new DelegateCommand(Back);

    private async void Back()
    {
      await _navigationService.GoBackAsync();
    }
    public DelegateCommand WorkingDistanceTableCommand => new DelegateCommand(WorkingDistanceTable, CanClick);

    public async void WorkingDistanceTable()
    {
      _isClicked = true;
      await _navigationService.NavigateAsync("NavPage/WorkingDistanceTable", null, true);
      _isClicked = false;
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

    public bool CanClick()
    {
      return !_isClicked;
    }
    public DelegateCommand NewCalculationCommand => new DelegateCommand(NewCalculation, CanClick);

    private async void NewCalculation()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      var personnel = CalculationInput.Personnel;
      var location = CalculationInput.Location;
      var action = CalculationInput.Action;
      var calculationDate = CalculationInput.CalculationDate;
      var isAlternatingCurrent = CalculationInput.IsAlternatingCurrent;
      var sourceFaultCurrent =CalculationOutput.BoltedFaultCurrent.Value;
      CalculationInput = _container.Resolve<ICalculationInput>() as CalculationInput;

      CalculationInput.Personnel = personnel;
      CalculationInput.Location = location;
      CalculationInput.Action = action;
      CalculationInput.CalculationDate = calculationDate;
      CalculationInput.IsAlternatingCurrent = isAlternatingCurrent;
      CalculationInput.SourceFaultCurrent = Math.Round(sourceFaultCurrent*1000);
      await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput/ACInputOne");
      _isClicked = false;
    }

    public DelegateCommand FindSafeWorkingDistanceCommand => new DelegateCommand(FindSafeWorkingDistance,CanClick);

    private async void FindSafeWorkingDistance()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", CalculationInput);
      p.Add("calculationOutput",CalculationOutput);
      p.Add("OriginalIncidentEnergy",OriginalIncidentEnergy);
      p.Add("OriginalWorkingDistance",OriginalWorkingDistance);
      await _navigationService.NavigateAsync("NavPage/FindSafeWorkingDistance",p,true);
      _isClicked = false;
    }

    public DelegateCommand ProtectiveClothingCommand => new DelegateCommand(ProtectiveClothing, CanClick);

    private async void ProtectiveClothing()
    {
      _isClicked = true;
      await _navigationService.NavigateAsync("NavPage/ProtectingClothing",null,true);
      _isClicked = false;
    }

    #endregion
    #region NavigationAware

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      if (parameters.ContainsKey("calculationInput"))
      {
        CalculationInput = (CalculationInput) parameters["calculationInput"];
      }
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
      //_eventAggregator.GetEvent<CalculationOutputUpdated>().Publish(CalculationOutput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {

      if (parameters.ContainsKey("calculationInput"))
      {
        CalculationInput = (CalculationInput)parameters["calculationInput"];
      CalculationOutput = (CalculationOutput)_calculatorService.Calculate(_calculationInput);
      if (parameters.ContainsKey("Summary"))
        {
          if ((bool) parameters["Summary"])
          {
            OriginalWorkingDistance = CalculationInput.WorkingDistance;
            OriginalIncidentEnergy = CalculationOutput.IncidentEnergy;
          }
        }
      //InitResultP1();
      //InitResultP2();
      //InitResultP3();
      //InitResults();
      //_eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
      InitResultP1();
      InitResultP2();
      InitResultP3();
      InitResults();
      }
    }
    #endregion
  }


  public class CellItem
  {
    public string Title { get; set; }
    public string Text { get; set; }
    public string CellType { get; set; }
    public  Color TitleColor { get; set; }
    public Color TextColor { get; set; }
    public DelegateCommand Command { get; set; }
    public bool HasError { get; set; }
    public  FontAttributes FontAttributes { get; set; }
    public Color BackgroundColor { get; set; }
    public LayoutOptions Aligment { get; set; }
    public string Comment { get; set; }
  }

  public class ResultItems
  {
    public ObservableCollection<CellItem> ViewItem{ get; set; }
  }
}

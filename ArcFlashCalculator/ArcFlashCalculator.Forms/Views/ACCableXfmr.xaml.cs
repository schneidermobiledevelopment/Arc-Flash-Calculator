using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACCableXfmr : ContentPage
  {

    private ACCableXfmrViewModel ViewModel => this.BindingContext as ACCableXfmrViewModel;
    public ACCableXfmr()
    {
      InitializeComponent();
     // if (ViewModel.CalculationInput.HasCable.Value)
      {
        LoadCable();
      }
     // if (ViewModel.CalculationInput.HasTransformer.Value)
      {
        LoadTransformer();
      }
    }

    public void LoadCable()
    {
      var ConductorSizeCell = new CustomViews.CustomPickerCell()
      {
        LabelText = "Conductor Size",
        IsRequired = true,
        PickerTitle = "Choose...>"
        , ItemSource = ViewModel.ConductorSizes
      };
      //ConductorSizeCell.SetBinding(CustomViews.CustomPickerCell.ItemsSourceProperty, new Binding("ConductorSizes") { Mode= BindingMode.TwoWay }); // Converter = new Converters.ConductorSizeItemsConverter()
      ConductorSizeCell.SetBinding(CustomViews.CustomPickerCell.SelectedItemProperty, new Binding("CalculationInput.ConductorSize") { Converter = new Converters.ConductorSizeConverter()});

      var ConductorPerPhaseCell = new CustomViews.CustomEntryCell()
      {
        LabelText = "# Conductors per Phase",
        WidthC1 = new GridLength(60, GridUnitType.Star),
        WidthC2 = new GridLength(40, GridUnitType.Star),
        IsRequired = true,
        Keyboard = Keyboard.Numeric,
        EntryPlaceholder = "Example: 12"
      };

      ConductorPerPhaseCell.SetBinding(CustomViews.CustomEntryCell.EntryTextProperty, new Binding("CalculationInput.ConductorPerPhase") { Converter = new Converters.IntConverter() });

      var ConductorLengthCell = new CustomViews.CustomEntryCell()
      {
        LabelText = "# Conductor Length",
        WidthC1 = new GridLength(60, GridUnitType.Star),
        WidthC2 = new GridLength(40, GridUnitType.Star),
        IsRequired = true,
        Keyboard = Keyboard.Numeric,
        EntryPlaceholder = "Example: 200"
      };

      ConductorLengthCell.SetBinding(CustomViews.CustomEntryCell.EntryTextProperty, new Binding("CalculationInput.ConductorLength") { Converter = new Converters.IntConverter() });
      tableSection.Add(ConductorSizeCell);
      tableSection.Add(ConductorPerPhaseCell);
      tableSection.Add(ConductorLengthCell);
    }

    public void LoadTransformer()
    {
      var PrimaryVoltageCell = new CustomViews.CustomEntryCell()
      {
        LabelText = "Primary Voltage (volts)",
        WidthC1 = new GridLength(55, GridUnitType.Star),
        WidthC2 = new GridLength(45, GridUnitType.Star),
        IsRequired = true,
        Keyboard = Keyboard.Numeric,
        EntryPlaceholder = "Example: 20000"
      };

      PrimaryVoltageCell.SetBinding(CustomViews.CustomEntryCell.EntryTextProperty, new Binding("CalculationInput.PrimaryVoltage") { Converter = new Converters.DecimalConverter() });
      PrimaryVoltageCell.SetBinding(CustomViews.CustomEntryCell.ErrorTextProperty, new Binding("PrimaryVoltageErrorMessage"));
      var XfmrImpedanceCell = new CustomViews.CustomEntryCell()
      {
        LabelText = "Transformer Impedance (%Z)",
        WidthC1 = new GridLength(55, GridUnitType.Star),
        WidthC2 = new GridLength(45, GridUnitType.Star),
        IsRequired = true,
        Keyboard = Keyboard.Numeric,
        EntryPlaceholder = "Example: 5.75"
      };

      XfmrImpedanceCell.SetBinding(CustomViews.CustomEntryCell.EntryTextProperty, new Binding("CalculationInput.XfmrImpedance") { Converter = new Converters.DecimalConverter() });
      XfmrImpedanceCell.SetBinding(CustomViews.CustomEntryCell.ErrorTextProperty, new Binding("XfmrImpedanceErrorMessage"));
      var XfmrKVACell = new CustomViews.CustomEntryCell()
      {
        LabelText = "Transformer KVA",
        WidthC1 = new GridLength(55, GridUnitType.Star),
        WidthC2 = new GridLength(45, GridUnitType.Star),
        IsRequired = true,
        Keyboard = Keyboard.Numeric,
        EntryPlaceholder = "Example: 500"
      };

      XfmrKVACell.SetBinding(CustomViews.CustomEntryCell.EntryTextProperty, new Binding("CalculationInput.XfmrKVA") { Converter = new Converters.DecimalConverter() });
      XfmrKVACell.SetBinding(CustomViews.CustomEntryCell.ErrorTextProperty, new Binding("XfmrKVAErrorMessage"));
      tableSection.Add(PrimaryVoltageCell);
      tableSection.Add(XfmrImpedanceCell);
      tableSection.Add(XfmrKVACell);
    }
  }
}

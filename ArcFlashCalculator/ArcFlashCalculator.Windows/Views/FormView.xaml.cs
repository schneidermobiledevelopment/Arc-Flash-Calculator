using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ArcFlashCalculator.WPF.ViewModels;
using Prism.Events;
using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.WPF.Controls;

namespace ArcFlashCalculator.WPF.Views
{
  /// <summary>
  /// Interaction logic for MainFormView.xaml
  /// </summary>
  public partial class FormView : UserControl
  {
    private Control focusedControl;

    public FormView()
    {
      InitializeComponent();
    }

    private void GotFocus(object sender, RoutedEventArgs e)
    {
      Control control = (Control)sender;
      focusedControl = control;


      //System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
      //dispatcherTimer.Tick += dispatcherTimer_Tick;
      //dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
      //dispatcherTimer.Start();

      Adorners.SetIsVisible(control, true);

      string tooltipMessage = "TODO:Add Tooltip Message";
      double toolTipLeftMargin = 252;
      switch (control.Name)
      {
        case "PersonnelTextBox":
          tooltipMessage = AppResources.Personnel_Tooltip_Message;
          break;
        case "LocationTextBox":
          tooltipMessage = AppResources.Location_Tooltip_Message;
          break;
        case "ActionTextBox":
          tooltipMessage = AppResources.Action_Tooltip_Message;
          break;
        case "CalculationDate":
          tooltipMessage = AppResources.Calculation_Date_Tooltip_Message;
          break;
        case "AlternatingCurrentRadio":
          tooltipMessage = AppResources.Alternating_Current_Tooltip_Message;
          break;
        case "DirectCurrentRadio":
          toolTipLeftMargin = 171;
          tooltipMessage = AppResources.Direct_Current_Tooltip_Message;
          break;
        case "EquipmentTypeComboBox":
          tooltipMessage = AppResources.Equipment_Type_Tooltip_Message;
          break;
        case "SolidGroundingRadioButton":
          tooltipMessage = AppResources.Solid_Grounding_Tooltip_Message;
          break;
        case "UnGroundedRadioButton":
          tooltipMessage = AppResources.UnGrounded_Tooltip_Message;
          break;
        case "NominalVoltageTextBox":
          if (!((FormViewModel)DataContext).CalculationInput.EquipmentTypeId.HasValue || ((FormViewModel)DataContext).CalculationInput.EquipmentTypeId == 0 || ((FormViewModel)DataContext).CalculationInput.EquipmentTypeId == 1)
          {
            tooltipMessage = AppResources.Nominal_Voltage_Tooltip_Message_Default;
          }
          else if (((FormViewModel)DataContext).CalculationInput.EquipmentTypeId == 2 || ((FormViewModel)DataContext).CalculationInput.EquipmentTypeId == 4)
          {
            tooltipMessage = AppResources.Nominal_Voltage_Tooltip_Message_Low_Voltage;
          }
          else
          {
            tooltipMessage = AppResources.Nominal_Voltage_Tooltip_Message_Medium_Voltage;
          }
          break;
        case "SourceFaultCurrentTextBox":
          tooltipMessage = AppResources.Source_Fault_Current_Tooltip_Message;
          break;
        case "OpenAirRadioButton":
          tooltipMessage = AppResources.Open_Air_Tooltip_Message;
          break;
        case "InBoxRadioButton":
          toolTipLeftMargin = 140;
          tooltipMessage = AppResources.In_Box_Tooltip_Message;
          break;

        //cable tips
        case "HasCableRadioButton":
          tooltipMessage = AppResources.Has_Cable_Tooltip_Message;
          break;
        case "NoCableRadioButton":
          toolTipLeftMargin = 139;
          tooltipMessage = AppResources.No_Cable_Tooltip_Message;
          break;
        case "ConductorSizeComboBox":
          tooltipMessage = AppResources.Conductor_Size_Tooltip_Message;
          break;
        case "ConductorPerPhaseTextBox":
          tooltipMessage = AppResources.Conductor_Per_Phase_Tooltip_Message;
          break;
        case "ConductorLengthTextBox":
          tooltipMessage = AppResources.Conductor_Length_Tooltip_Message;
          break;
        case "SensorRatingTextBox":
          tooltipMessage = AppResources.Sensor_Rating_Tooltip_Message;
          break;
        case "ArcDurationTextBox":
          tooltipMessage = AppResources.Arc_Duration_Tooltip_Message;
          break;
        case "CalculateArcDurationExpander":
          tooltipMessage = AppResources.Calculate_Arc_Duration_Tooltip_Message;
          break;

        //transformer tips
        case "HasTransformerRadioButton":
          tooltipMessage = AppResources.Has_Transformer_Tooltip_Message;
          break;
        case "NoTransformerRadioButton":
          toolTipLeftMargin = 141;
          tooltipMessage = AppResources.No_Transformer_Tooltip_Message;
          break;
        case "PrimaryVoltageTextBox":
          tooltipMessage = AppResources.Primary_Voltage_Tooltip_Message;
          break;
        case "XfmrImpedanceTextBox":
          tooltipMessage =
            AppResources.Xfmr_Impedance_Tooltip_Message;
          break;
        case "XfmrKVATextBox":
          tooltipMessage = AppResources.Xfmr_KVA_Tooltip_Message;
          break;


        //DC tips
        case "MaximumShortCircuitAvailableTextBox":
          tooltipMessage = AppResources.Maximum_Short_Circuit_Available_Tooltip_Message;
          break;
        case "DirectCurrentInCabinetRadioButton":
          toolTipLeftMargin = 168;
          tooltipMessage = AppResources.Direct_Current_In_Cabinet_Tooltip_Message;
          break;
        case "DirectCurrentOpenAirRadioButton":
          tooltipMessage = AppResources.Direct_Current_Open_Air_Tooltip_Message;
          break;
        case "VoltageOfBatteryTextBox":
          tooltipMessage = AppResources.Voltage_Of_Battery_Tooltip_Message;
          break;

      }
      ((FormViewModel)DataContext).ToolTipText = tooltipMessage;
      ((FormViewModel)DataContext).LeftToolTipMargin = new Thickness(toolTipLeftMargin, -12, 0, 0);
    }

    private void LostFocus(object sender, RoutedEventArgs e)
    {
      Adorners.SetIsVisible((UIElement)sender, false);
    }

    /// <summary>
    /// Create a new animation to the x,y offset.  I beleive you need the have .NET 3.5 SP1 installed for
    /// this to work.  If you don't you can't call Storyboard.SetTarget.
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    private void ScrollToPosition(double x, double y)
    {
      DoubleAnimation vertAnim = new DoubleAnimation();
      vertAnim.From = mainViewer.VerticalOffset;
      vertAnim.To = y;
      vertAnim.DecelerationRatio = .2;
      vertAnim.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
      DoubleAnimation horzAnim = new DoubleAnimation();
      horzAnim.From = mainViewer.HorizontalOffset;
      horzAnim.To = x;
      horzAnim.DecelerationRatio = .2;
      horzAnim.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
      Storyboard sb = new Storyboard();
      sb.Children.Add(vertAnim);
      sb.Children.Add(horzAnim);
      Storyboard.SetTarget(vertAnim, mainViewer);
      Storyboard.SetTargetProperty(vertAnim, new PropertyPath(CustomScrollViewer.CurrentVerticalOffsetProperty));
      Storyboard.SetTarget(horzAnim, mainViewer);
      Storyboard.SetTargetProperty(horzAnim, new PropertyPath(CustomScrollViewer.CurrentHorizontalOffsetProperty));
      sb.Begin();
    }

    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      Point relativePoint = focusedControl.TransformToAncestor(FormGrid).Transform(new Point(0, 0));
      ScrollToPosition(relativePoint.X, relativePoint.Y);
    }
  }
}

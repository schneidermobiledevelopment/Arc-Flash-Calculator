using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class TechnicalHelpViewModel : BindableBase, INavigationAware
  {
    private INavigationService _navigationService;
    private string page;
    public TechnicalHelpViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
    }

    public void InitLabels()
    {
      switch (page)
      {
        case "CommonInput":
          Title1 = "Personnel";
          Subtitle1 = "All personnel who should be informed of the available arc flash levels.";
          Title2 = "Location:";
          Subtitle2 = "Facility and specific equipment where the arc flash levels are being calculated.";
          Title3 = "Action:";
          Subtitle3 = "Describe the type of energized work that will be performed (example: troubleshooting, testing, verify de-energized).";
          Title4 = "Date:";
          Subtitle4 = "Please select a date.";
          Title5 = "Voltage Type:";
          Subtitle5 = "Select equipment voltage type where work will be performed.";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "ACInputOne":
          Title1 = "Equipment Type";
          Subtitle1 = "Choose the type of equipment where energized work will be performed.";
          Title2 = "Grounding";
          Subtitle2 = "Select the system grounding method.";
          Title3 = "Nominal Voltage";
          Subtitle3 = "Bus voltage of the equipment to be worked on OR - if transformer is present, transformer secondary voltage.  Valid range: 208 - 38000";
          Title4 = "Source Fault Current";
          Subtitle4 = "The available short-circuit current supplied by the utility or on-site generator.  If not known use 99999 at the primary of the serving transformer. ";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "ACInputTwo":
          Title1 = "Equipment Enclosure";
          Subtitle1 = "Select whether arc potential is in open air or in an enclosure.";
          Title2 = "Voltage Conductor";
          Subtitle2 = "Select whether equipment voltage is supplied from a cable or bus only.";
          Title3 = "Voltage from Transformer";
          Subtitle3 = "Select whether an upstream transformer is present.";
          Title4 = "";
          Subtitle4 = "";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "ACHasCable":
          Title1 = "Conductor Size";
          Subtitle1 = "Choose size of conductors or cable from transformer to equipment or from location where fault current is known to the equipment to be worked on. Conductor impedance is based on copper (CU)conductors";
          Title2 = "# Conductors per Phase";
          Subtitle2 = "If parallel conductors are used, enter the number of conductors.";
          Title3 = "Conductor Length";
          Subtitle3 = "Enter the cable length in feet.";
          Title4 = "";
          Subtitle4 = "";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "ACHasXfmr":
          Title1 = "Primary Voltage";
          Subtitle1 = "Voltage at the transformer primary Valid range: 208 - 138000";
          Title2 = "Transformer Impedance (%Z)";
          Subtitle2 = "Transformer percent impedance (%Z or %X). Typcially found on transformer nameplate.Valid range: 1.00 - 15.00";
          Title3 = "Transformer KVA";
          Subtitle3 = "Transformer kVA is require to determine available fault current on secondary of transformer.Data may be found on transformer nameplate.";
          Title4 = "";
          Subtitle4 = "";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "ACCableAndXfmr":
          Title1 = "Conductor Size";
          Subtitle1 = "Choose size of conductors or cable from transformer to equipment or from location where fault current is known to the equipment to be worked on. Conductor impedance is based on copper (CU)conductors";
          Title2 = "# Conductors per Phase";
          Subtitle2 = "If parallel conductors are used, enter the number of conductors.";
          Title3 = "Conductor Length";
          Subtitle3 = "Enter the cable length in feet.";
          Title4 = "Primary Voltage";
          Subtitle4 = "Voltage at the transformer primary Valid range: 208 - 138000";
          Title5 = "Transformer Impedance (%Z)";
          Subtitle5 = "Transformer percent impedance (%Z or %X). Typcially found on transformer nameplate.Valid range: 1.00 - 15.00";
          Title6 = "Transformer KVA";
          Subtitle6 = "Transformer kVA is require to determine available fault current on secondary of transformer.Data may be found on transformer nameplate.";
          break;
        case "ACSensor":
          Title1 = "Sensor Rating";
          Subtitle1 = "If using a Square D time current curve to determine device opening time, enter the sensor rating based on curve data.";
          Title2 = "Arc Duration";
          Subtitle2 = "Time for upstream protective device (located outside of the enclosure to be worked-on) to operate for the arcing fault current calculated. Consider equipment condition and  maintenance. If protective device clearing time is unknown:a) Enter 2 seconds; or b) Consult time current curve for the device; or c) Request support from Engineering Services. Valid range: 0.01 - 2.00";
          Title3 = "";
          Subtitle3 = "";
          Title4 = "";
          Subtitle4 = "";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "DCInput":
          Title1 = "Maximum Available Short Circuit";
          Subtitle1 =
            "If Manufacturer does not publish \"Maximum Available Short Circuit\" level but \"1 minute current to 1.75 volts per cell(VPC)\" is published, then the Maximum Short Circuit can be conservatively calculated by multiplying the \"1 minute amp to 1.75 VPC\" value by 10.";
          Title2 = "Battery Enclosue";
          Subtitle2 = "";
          Title3 = "Battery String Voltage";
          Subtitle3 = "Maximum string voltage, or partial string voltage if all batteries are not connected.";
          Title4 = "";
          Subtitle4 = "";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;
        case "ArcDuration":
          Title1 = "Arc Duration";
          Subtitle1 =
            "Time for upstream protective device (located outside of the enclosure to be worked-on) to operate for the arcing fault current calculated. " +
            "\nConsider equipment condition and maintenance." +
            "\n" +
            "\n" +
            "If protective device clearing time is unknown:\n" +
            "a) Enter 2 seconds; -or- \n" +
            "b) Consult time current curve for the device; -or- \n" +
            "c) Request support from Engineering Services.\n" +
            "Valid range: 0.02-2.00";
          Title2 = "Device not listed?";
          Subtitle2 = "Go back & choose \"Find Equipment Trip Curves\" for more information. ";
          Title3 = "";
          Subtitle3 = "";
          Title4 = "";
          Subtitle4 = "";
          Title5 = "";
          Subtitle5 = "";
          Title6 = "";
          Subtitle6 = "";
          break;

      }
    }


    private string _title1;
    public string Title1
    {
      get { return _title1; }
      set { SetProperty(ref _title1, value); }
    }
    private string _title2;
    public string Title2
    {
      get { return _title2; }
      set { SetProperty(ref _title2, value); }
    }
    private string _title3;
    public string Title3
    {
      get { return _title3; }
      set { SetProperty(ref _title3, value); }
    }
    private string _title4;
    public string Title4
    {
      get { return _title4; }
      set { SetProperty(ref _title4, value); }
    }
    private string _title5;
    public string Title5
    {
      get { return _title5; }
      set { SetProperty(ref _title5, value); }
    }
    private string _title6;
    public string Title6
    {
      get { return _title6; }
      set { SetProperty(ref _title6, value); }
    }
    private string _subtitle1;
    public string Subtitle1
    {
      get { return _subtitle1; }
      set { SetProperty(ref _subtitle1, value); }
    }
    private string _subtitle2;
    public string Subtitle2
    {
      get { return _subtitle2; }
      set { SetProperty(ref _subtitle2, value); }
    }
    private string _subtitle3;
    public string Subtitle3
    {
      get { return _subtitle3; }
      set { SetProperty(ref _subtitle3, value); }
    }
    private string _subtitle4;
    public string Subtitle4
    {
      get { return _subtitle4; }
      set { SetProperty(ref _subtitle4, value); }
    }
    private string _subtitle5;
    public string Subtitle5
    {
      get { return _subtitle5; }
      set { SetProperty(ref _subtitle5, value); }
    }
    private string _subtitle6;
    public string Subtitle6
    {
      get { return _subtitle6; }
      set { SetProperty(ref _subtitle6, value); }
    }

    public DelegateCommand CloseCommand => new DelegateCommand(Close);
    private async void Close()
    {
      await _navigationService.GoBackAsync(null, true);
    }

    public void OnNavigatedFrom(NavigationParameters parameters)
    {

    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      if (parameters.ContainsKey("help"))
      {
        page = (string)parameters["help"];
        InitLabels();
      }
    }
  }
}

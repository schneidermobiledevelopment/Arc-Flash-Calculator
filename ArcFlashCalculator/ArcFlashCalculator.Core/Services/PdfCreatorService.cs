using System;
using System.IO;
using System.Reflection;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.Core.Model;
using Xfinium.Pdf;
using Xfinium.Pdf.FlowDocument;
using Xfinium.Pdf.Graphics;

namespace ArcFlashCalculator.Core.Services
{
  public class PdfCreatorService : IPdfCreatorService
  {
    public MemoryStream CreatePdfMemoryStream(CalculationInput input, CalculationOutput output)
    {
      //PdfFlowDocument document = new PdfFlowDocument();

      //PdfFlowContent attendantsSection = BuildAttendantsList();
      //document.AddContent(attendantsSection);

      // Create a font and a brush
      PdfStandardFont headerFont = new PdfStandardFont(PdfStandardFontFace.HelveticaBold, 24);
      PdfStandardFont subHeaderFont = new PdfStandardFont(PdfStandardFontFace.HelveticaBold, 16);
      PdfStandardFont labelFont = new PdfStandardFont(PdfStandardFontFace.HelveticaBold, 12);
      PdfStandardFont valueFont = new PdfStandardFont(PdfStandardFontFace.Helvetica, 12);
      PdfStandardFont tableLabelFont = new PdfStandardFont(PdfStandardFontFace.HelveticaBold, 10);
      PdfStandardFont boxFont = new PdfStandardFont(PdfStandardFontFace.HelveticaBold, 8);
      PdfBrush brush = new PdfBrush();
      PdfBrush greyBrush = new PdfBrush();
      greyBrush.Color = PdfRgbColor.Gray;

      // Create a fixed document
      PdfFixedDocument document = new PdfFixedDocument();

      // Add a new page
      PdfPage page = document.Pages.Add();

      //draw line
      PdfPen blackPen = new PdfPen(PdfRgbColor.Black, 2);
      page.Graphics.DrawLine(blackPen, 0, 65, 600, 65);

      //rectangles
      page.Graphics.DrawRectangle(blackPen, 2, 182, 192, 180);
      page.Graphics.DrawRectangle(blackPen, 202, 182, 192, 180);
      page.Graphics.DrawRectangle(blackPen, 402, 182, 192, 180);

      page.Graphics.DrawRectangle(blackPen, 2, 370, 392, 400);

      int rowCount = 4; //for dc
      if (input.IsAlternatingCurrent.Value)
      {
        if (!input.HasTransformer.Value && !input.HasCable.Value) rowCount = 10;
        if (!input.HasTransformer.Value && input.HasCable.Value) rowCount = 13;
        if (input.HasTransformer.Value && !input.HasCable.Value) rowCount = 13;
        if (input.HasTransformer.Value && input.HasCable.Value) rowCount = 16;
        if (input.HasTransformer.Value && input.IsArcDurationCalculated) rowCount += 8;
      }
      //table
      int twenty = 0;
      for (int i = 0; i < rowCount; i++)
      {
        page.Graphics.DrawRectangle(blackPen, 2, 370 + twenty, 197, 20);
        page.Graphics.DrawRectangle(blackPen, 200, 370 + twenty, 194, 20);
        twenty += 20;

        switch (i)
        {
          case 0://row number
            if (input.IsAlternatingCurrent.Value)
            {
              page.Graphics.DrawString(AppResources.Nominal_Working_Voltage, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.NominalVoltage.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            else
            {
              page.Graphics.DrawString(AppResources.Voltage_Type, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString("DC", tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 1://row number
            if (input.IsAlternatingCurrent.Value)
            {
              page.Graphics.DrawString(AppResources.Arc_Flash_Boundary_Feet, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(Math.Round((double) output.FPB, 2).ToString(), tableLabelFont, brush, 204,
                356 + twenty);
            }
            else
            {
              page.Graphics.DrawString(AppResources.Maximum_Available_Short_Circuit, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.MaximumShortCircuitAvailable.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 2://row number
            if (input.IsAlternatingCurrent.Value)
            {
              page.Graphics.DrawString(AppResources.Bolted_Fault_Current_amps, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(Math.Round((double)output.BoltedFaultCurrent.Value * 1000, 2).ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            else
            {
              page.Graphics.DrawString(AppResources.Battery_Enclosure, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.InCabinet.HasValue && input.InCabinet.Value ? AppResources.In_Box : AppResources.Open_Air, tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 3://row number
            if (input.IsAlternatingCurrent.Value)
            {
              page.Graphics.DrawString(AppResources.Estimated_Arc_Fault_Current_amps, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(Math.Round((double)output.EstimatedArcFaultCurrent.Value * 1000, 2).ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            else
            {
              page.Graphics.DrawString(AppResources.Battery_String_Voltage, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.VoltageOfBattery.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 4://row number
            page.Graphics.DrawString(AppResources.Incident_Energy_18, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(Math.Round((double)output.IE18, 2).ToString(), tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 5://row number
            page.Graphics.DrawString(AppResources.Incident_Energy_24, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(Math.Round((double)output.IE24, 2).ToString(), tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 6://row number
            page.Graphics.DrawString(AppResources.Incident_Energy_36, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(Math.Round((double)output.IE36, 2).ToString(), tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 7://row number
            page.Graphics.DrawString(AppResources.Arc_Duration_seconds, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(input.ArcDurationValue.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 8://row number
            page.Graphics.DrawString(AppResources.Source_Fault_Current_amps, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(input.SourceFaultCurrent.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 9://row number
            string groundingString = AppResources.Solidly_Grounded;
            page.Graphics.DrawString(AppResources.Grounding, tableLabelFont, brush, 5, 356 + twenty);
            if (!input.IsSolidGround.Value)
            {
              groundingString = AppResources.Ungrounded;
            }
            page.Graphics.DrawString(groundingString, tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 10://row number
            page.Graphics.DrawString(AppResources.Potential_Arc_Location, tableLabelFont, brush, 5, 356 + twenty);
            string arcLocation = AppResources.Open_Air;
            if (!input.IsOpenAir.Value)
            {
              arcLocation = AppResources.In_Box;
            }
            page.Graphics.DrawString(arcLocation, tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 11://row number
            page.Graphics.DrawString(AppResources.Conductor, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(input.HasCable.HasValue && input.HasCable.Value ? AppResources.Cable : AppResources.No_Cable, tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 12://row number
            if (input.HasCable.Value)
            {
              page.Graphics.DrawString(AppResources.Conductor_Size, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.ConductorSize.Size, tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 13://row number
            if (input.HasCable.Value)
            {
              page.Graphics.DrawString(AppResources.Conductors_per_Phase, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.ConductorPerPhase.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 14://row number
            if (input.HasCable.Value)
            {
              page.Graphics.DrawString(AppResources.Conductor_Length, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.ConductorLength.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 15://row number
            page.Graphics.DrawString(AppResources.Upstream_Transformer, tableLabelFont, brush, 5, 356 + twenty);
            page.Graphics.DrawString(input.HasTransformer.HasValue && input.HasTransformer.Value ? AppResources.Upstream_XFR : AppResources.No_Transformer, tableLabelFont, brush, 204, 356 + twenty);
            break;
          case 16://row number
            if (input.HasTransformer.Value)
            {
              page.Graphics.DrawString(AppResources.Primary_Voltage, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.PrimaryVoltage.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 17://row number
            if (input.HasTransformer.Value)
            {
              page.Graphics.DrawString(AppResources.Transformer_Impedence, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.XfmrImpedance.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 18://row number
            if (input.HasTransformer.Value)
            {
              page.Graphics.DrawString(AppResources.Transformer_KVA, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.XfmrKVA.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 19://row number
            if (input.IsAlternatingCurrent.Value)
            {
              page.Graphics.DrawString(AppResources.Sensor_Rating, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.SensorRating.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 20://row number
            if (input.IsAlternatingCurrent.Value)
            {
              page.Graphics.DrawString(AppResources.Multiple_Of_Sensor_Rating, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(output.MultipleOfSensorRating.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
          case 21://row number
            if (input.IsArcDurationCalculated)
            {
              page.Graphics.DrawString(AppResources.Manufacturer, tableLabelFont, brush, 5, 356 + twenty);
              page.Graphics.DrawString(input.ArcDuration.Manufacturer.Name.ToString(), tableLabelFont, brush, 204, 356 + twenty);
            }
            break;
        }
      }

      //diagram box
      page.Graphics.DrawRectangle(blackPen, 402, 370, 192, 400);
      page.Graphics.DrawString("CIRCUIT DIAGRAM PREVIEW", labelFont, greyBrush, 410, 375);

      // Title
      page.Graphics.DrawString(AppResources.ARC_FLASH_ESTIMATE_REPORT, headerFont, brush, 50, 40);

      //job details
      page.Graphics.DrawString("Job Details", subHeaderFont, brush, 5, 75);
      page.Graphics.DrawString("Location:", labelFont, brush, 5, 100);
      if(input.Location!=null) page.Graphics.DrawString(input.Location, valueFont, brush, 69, 100);

      page.Graphics.DrawString("Action:", labelFont, brush, 18, 120);
      if (input.Action != null) page.Graphics.DrawString(input.Action, valueFont, brush, 68, 120);

      if (input.IsAlternatingCurrent.Value)
      {
        page.Graphics.DrawString("Eqipment Type:", labelFont, brush, 5, 140);
        page.Graphics.DrawString(input.EquipmentType.Name, valueFont, brush, 98, 140);
      }

      page.Graphics.DrawString("Date:", labelFont, brush, 330, 100);
      page.Graphics.DrawString(input.CalculationDate.ToString("D"), valueFont, brush, 373, 100);

      page.Graphics.DrawString("Personnel:", labelFont, brush, 300, 120);
      if (input.Personnel != null) page.Graphics.DrawString(input.Personnel, valueFont, brush, 375, 120);

      //Arc Flash Estimate
      page.Graphics.DrawString("Arc Flash Estimate", subHeaderFont, brush, 10, 165);

      //incident energy
      page.Graphics.DrawString("INCIDENT ENERGY", labelFont, greyBrush, 35, 190);
      page.Graphics.DrawString(output.IncidentEnergy.ToString(), headerFont, brush, 70, 220);
      page.Graphics.DrawString("cal/cm²", labelFont, brush, 72, 250);

      //at working distance
      page.Graphics.DrawString("AT WORKING DISTANCE:", labelFont, greyBrush, 15, 280);
      page.Graphics.DrawString(Math.Round(input.WorkingDistance.Value, 2, MidpointRounding.ToEven).ToString(), headerFont, brush, 70, 310);
      page.Graphics.DrawString("inches", labelFont, brush, 100, 316);

      //PPE Level
      PdfStringAppearanceOptions sao = new PdfStringAppearanceOptions();
      PdfStringLayoutOptions slo = new PdfStringLayoutOptions();
      slo.HorizontalAlign = PdfStringHorizontalAlign.Justified;
      slo.VerticalAlign = PdfStringVerticalAlign.Top;
      sao.Brush = brush;
      sao.Font = valueFont;

      page.Graphics.DrawString("PPE LEVEL REQUIRED", labelFont, greyBrush, 230, 190);
      page.Graphics.DrawString(output.HazardCat, headerFont, brush, 290, 220);
      slo.X = 210;
      slo.Width = 170;
      page.Graphics.DrawString("CLOTHING DESCRIPTION:", labelFont, greyBrush, 230, 280);
      if (output.IncidentEnergy <= 1.2m)
      {
        slo.Y = 250;
        page.Graphics.DrawString(String.Format(AppResources.Required_Minimum_Arc_Rating_Pdf, 1.2m), sao, slo);
        slo.Y = 300;
        page.Graphics.DrawString(AppResources.ClothingDescriptionDetailsOnePointTwo, sao, slo);
      }

      if (output.IncidentEnergy > 1.2m && output.IncidentEnergy <= 8)
      {
        slo.Y = 250;
        page.Graphics.DrawString(String.Format(AppResources.Required_Minimum_Arc_Rating_Pdf, 8), sao, slo);
        slo.Y = 300;
        page.Graphics.DrawString(AppResources.ClothingDescriptionDetails8PartB, sao, slo);
      }

      if (output.IncidentEnergy > 8 && output.IncidentEnergy <= 40)
      {
        slo.Y = 250;
        page.Graphics.DrawString(String.Format(AppResources.Required_Minimum_Arc_Rating_Pdf, 40), sao, slo);
        slo.Y = 300;
        page.Graphics.DrawString(AppResources.ClothingDescriptionDetails40PartB, sao, slo);
      }

      //glove class
      page.Graphics.DrawString("INSULATING GLOVE CLASS", labelFont, greyBrush, 410, 190);
      page.Graphics.DrawString(output.InsulatingGloveClass.ToString(), headerFont, brush, 490, 220);

      //shock hazard
      slo.X = 420;
      slo.Y = 280;
      slo.Width = 170;
      if (input.IsAlternatingCurrent.Value)
      {
        page.Graphics.DrawString(String.Format(AppResources.Voltage_Shock_Hazard_AC, input.NominalVoltage), sao, slo);
      }
      else
      {
        page.Graphics.DrawString(String.Format(AppResources.Voltage_Shock_Hazard_DC, input.VoltageOfBattery), sao, slo);
      }
      
      Stream explosionIconStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.pdf_explosion.png");
      Stream logoStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.pdf_logo.png");
      Stream warningStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.warning.png");
      
      PdfPngImage explosion = new PdfPngImage(explosionIconStream);
      page.Graphics.DrawImage(explosion, 10, 30, 32, 32);

      PdfPngImage logo = new PdfPngImage(logoStream);
      page.Graphics.DrawImage(logo, 525, 30, 75, 30);

      PdfPngImage warning = new PdfPngImage(warningStream);
      page.Graphics.DrawImage(warning, 472, 235, 50, 50);

      //dc
      if (!input.IsAlternatingCurrent.Value)
      {
        Stream dcStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.dc_source.png");
        PdfPngImage dc = new PdfPngImage(dcStream);
        page.Graphics.DrawImage(dc, 410, 400, 75, 75);

        //Max Available box
        page.Graphics.DrawRectangle(blackPen, 490, 400, 100, 50);
        page.Graphics.DrawString(AppResources.Maximum_Available, boxFont, brush, 505, 405);
        page.Graphics.DrawString(AppResources.Short_Circuit_Amps, boxFont, brush, 505, 420);
        page.Graphics.DrawString(input.MaximumShortCircuitAvailable.Value + "A", tableLabelFont, brush, 525, 437);

        //Point of work
        page.Graphics.DrawRectangle(blackPen, 490, 460, 100, 16);
        page.Graphics.DrawString(AppResources.Point_Of_Work, tableLabelFont, brush, 505, 464);
      }
      else
      {
        Stream acSourceStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.ac_source.png");
        PdfPngImage acSource = new PdfPngImage(acSourceStream);
        page.Graphics.DrawImage(acSource, 430, 400, 25, 25);
        //source fault current box
        page.Graphics.DrawRectangle(blackPen, 460, 400, 130, 35);
        page.Graphics.DrawString(AppResources.Source_Fault_Current, tableLabelFont, brush, 480, 405);
        page.Graphics.DrawString(String.Format(AppResources.SourceFaultCurrent, input.SourceFaultCurrent), tableLabelFont, brush, 510, 420);
     
        //has xfmr and no cable
        if (input.HasTransformer.Value && !input.HasCable.Value)
        {
          Stream xfmrStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.xfmr.png");
          PdfPngImage xfmr = new PdfPngImage(xfmrStream);
          page.Graphics.DrawImage(xfmr, 430, 426, 25, 73);

          Stream breakerStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.breaker.png");
          PdfPngImage breaker = new PdfPngImage(breakerStream);
          page.Graphics.DrawImage(breaker, 438, 499, 16, 97);

          Stream workPointStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.work_point.png");
          PdfPngImage workPoint = new PdfPngImage(workPointStream);
          page.Graphics.DrawImage(workPoint, 434, 595, 16, 38);

          //transformer box
          page.Graphics.DrawRectangle(blackPen, 460, 460, 130, 65);
          page.Graphics.DrawString(String.Format(AppResources.TransformerKVA, input.XfmrKVA), tableLabelFont, brush, 485, 465);
          page.Graphics.DrawString(String.Format(AppResources.PrimaryVoltage, input.PrimaryVoltage), tableLabelFont, brush, 480, 480);
          page.Graphics.DrawString(String.Format(AppResources.SecondaryVoltage, input.NominalVoltage), tableLabelFont, brush, 480, 495);
          page.Graphics.DrawString(String.Format(AppResources.TransformerImpedence, input.XfmrImpedance), tableLabelFont, brush, 490, 510);

          //arc duration box
          page.Graphics.DrawRectangle(blackPen, 460, 560, 130, 40);
          page.Graphics.DrawString(AppResources.Arc_Duration, tableLabelFont, brush, 495, 565);
          page.Graphics.DrawString(String.Format(AppResources.Arc_Duration_With_Unit, input.ArcDurationValue), tableLabelFont, brush, 510, 580);

          //point of work box
          page.Graphics.DrawRectangle(blackPen, 460, 620, 130, 16);
          page.Graphics.DrawString(AppResources.Point_Of_Work, tableLabelFont, brush, 500, 625);
        }

        //no xfmr and has cable
        if (!input.HasTransformer.Value && input.HasCable.Value)
        {
          Stream cableStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.cable.png");
          PdfPngImage cable = new PdfPngImage(cableStream);
          page.Graphics.DrawImage(cable, 437, 425, 10, 94);

          Stream breakerStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.breaker.png");
          PdfPngImage breaker = new PdfPngImage(breakerStream);
          page.Graphics.DrawImage(breaker, 438, 515, 16, 97);

          Stream workPointStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.work_point.png");
          PdfPngImage workPoint = new PdfPngImage(workPointStream);
          page.Graphics.DrawImage(workPoint, 434, 612, 16, 38);
          
          //Cable box
          page.Graphics.DrawRectangle(blackPen, 460, 480, 130, 50);
          page.Graphics.DrawString(input.ConductorLength.Value + "ft.", tableLabelFont, brush, 505, 485);
          page.Graphics.DrawString(String.Format(AppResources.ConductorsPerPhase, input.ConductorPerPhase), tableLabelFont, brush, 465, 500);
          page.Graphics.DrawString(String.Format(AppResources.ConductorSize, input.ConductorSize.Size), tableLabelFont, brush, 470, 515);

          //arc duration box
          page.Graphics.DrawRectangle(blackPen, 460, 570, 130, 40);
          page.Graphics.DrawString(AppResources.Arc_Duration, tableLabelFont, brush, 495, 575);
          page.Graphics.DrawString(String.Format(AppResources.Arc_Duration_With_Unit, input.ArcDurationValue), tableLabelFont, brush, 510, 595);

          //point of work box
          page.Graphics.DrawRectangle(blackPen, 460, 635, 130, 16);
          page.Graphics.DrawString(AppResources.Point_Of_Work, tableLabelFont, brush, 500, 640);
        }

        //has xfmr and cable
        if (input.HasTransformer.Value && input.HasCable.Value)
        {
          Stream xfmrStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.xfmr.png");
          PdfPngImage xfmr = new PdfPngImage(xfmrStream);
          page.Graphics.DrawImage(xfmr, 430, 426, 25, 73);
        
          Stream cableStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.cable.png");
          PdfPngImage cable = new PdfPngImage(cableStream);
          page.Graphics.DrawImage(cable, 437, 499, 10, 94);

          Stream breakerStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.breaker.png");
          PdfPngImage breaker = new PdfPngImage(breakerStream);
          page.Graphics.DrawImage(breaker, 438, 593, 16, 97);

          Stream workPointStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.work_point.png");
          PdfPngImage workPoint = new PdfPngImage(workPointStream);
          page.Graphics.DrawImage(workPoint, 434, 690, 16, 38);

          //transformer box
          page.Graphics.DrawRectangle(blackPen, 460, 460, 130, 65);
          page.Graphics.DrawString(String.Format(AppResources.TransformerKVA, input.XfmrKVA), tableLabelFont, brush, 485, 465);
          page.Graphics.DrawString(String.Format(AppResources.PrimaryVoltage, input.PrimaryVoltage), tableLabelFont, brush, 480, 480);
          page.Graphics.DrawString(String.Format(AppResources.SecondaryVoltage, input.NominalVoltage), tableLabelFont, brush, 480, 495);
          page.Graphics.DrawString(String.Format(AppResources.TransformerImpedence, input.XfmrImpedance), tableLabelFont, brush, 490, 510);

          //Cable box
          page.Graphics.DrawRectangle(blackPen, 460, 550, 130, 50);
          page.Graphics.DrawString( input.ConductorLength.Value + "ft.", tableLabelFont, brush, 505, 555);
          page.Graphics.DrawString(String.Format(AppResources.ConductorsPerPhase, input.ConductorPerPhase), tableLabelFont, brush, 465, 570);
          page.Graphics.DrawString(String.Format(AppResources.ConductorSize, input.ConductorSize.Size), tableLabelFont, brush, 470, 585);

          //arc duration box
          page.Graphics.DrawRectangle(blackPen, 460, 640, 130, 50);
          page.Graphics.DrawString(AppResources.Arc_Duration, tableLabelFont, brush, 495, 645);
          page.Graphics.DrawString(String.Format(AppResources.Arc_Duration_With_Unit, input.ArcDurationValue), tableLabelFont, brush, 510, 665);

          //point of work box
          page.Graphics.DrawRectangle(blackPen, 460, 720, 130, 16);
          page.Graphics.DrawString(AppResources.Point_Of_Work, tableLabelFont, brush, 500, 725);
        }

        //no xfmr and no cable
        if (!input.HasTransformer.Value && !input.HasCable.Value)
        {
          Stream breakerStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.breaker.png");
          PdfPngImage breaker = new PdfPngImage(breakerStream);
          page.Graphics.DrawImage(breaker, 437, 425, 20, 121);

          Stream workPointStream = GetImageStreamByName("ArcFlashCalculator.Core.Images.work_point.png");
          PdfPngImage workPoint = new PdfPngImage(workPointStream);
          page.Graphics.DrawImage(workPoint, 432, 545, 20, 48);

          //arc duration box
          page.Graphics.DrawRectangle(blackPen, 460, 500, 130, 50);
          page.Graphics.DrawString(AppResources.Arc_Duration, tableLabelFont, brush, 495, 505);
          page.Graphics.DrawString(String.Format(AppResources.Arc_Duration_With_Unit, input.ArcDurationValue), tableLabelFont, brush, 510, 525);

          //point of work box
          page.Graphics.DrawRectangle(blackPen, 460, 580, 130, 16);
          page.Graphics.DrawString(AppResources.Point_Of_Work, tableLabelFont, brush, 500, 585);
        }


      }

      //compress
      page.Graphics.CompressAndClose();

      //Creates a new Memory stream
      MemoryStream memoryStream = new MemoryStream();

      // Saves the document as stream
      document.Save(memoryStream);

      return memoryStream;
    }

    private Stream GetImageStreamByName(string name)
    {
      string[] names = GetType().GetTypeInfo().Assembly.GetManifestResourceNames();

      return GetType().GetTypeInfo().Assembly.GetManifestResourceStream(name); 
    }

    private static PdfFlowContent BuildAttendantsList()
    {
      PdfStandardFont jobDetailsFontValue = new PdfStandardFont(PdfStandardFontFace.Helvetica, 12);


      PdfFlowTableContent attendantsTable = new PdfFlowTableContent(2);
      attendantsTable.Border = new PdfPen(PdfRgbColor.Black, 0.5);
      attendantsTable.MinRowHeight = 15;
      (attendantsTable.DefaultCell as PdfFlowTableStringCell).Font = jobDetailsFontValue;
      attendantsTable.Columns[0].VerticalAlign = PdfGraphicAlign.Center;
      attendantsTable.Columns[0].Width = 180;
      attendantsTable.Columns[0].WidthIsRelativeToTable = false;
      attendantsTable.Columns[1].VerticalAlign = PdfGraphicAlign.Center;
      attendantsTable.Columns[1].Width = 180;
      attendantsTable.Columns[1].WidthIsRelativeToTable = false;

      attendantsTable.HeaderRows.AddRowWithCells(AppResources.Nominal_Working_Voltage, "480V AC");

      return attendantsTable;
    }
  }
}

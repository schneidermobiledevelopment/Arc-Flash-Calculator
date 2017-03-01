using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Cells
{
  public class TemplateSelector:DataTemplateSelector
  {
    private readonly DataTemplate _textCenteredWithHeader;
    private readonly DataTemplate _textCentered;
    private readonly DataTemplate _horizontalText;
    private readonly DataTemplate _horizontalTextWithLink;
    private readonly DataTemplate _normal;
    private readonly DataTemplate _normalWithButton;
    private readonly DataTemplate _horizontalTextDangerWithLink;
    private readonly DataTemplate _button;
    private readonly DataTemplate _link;
    private readonly DataTemplate _header;
    private readonly DataTemplate _textCenteredWithError;

    public TemplateSelector()
    {
      _textCenteredWithHeader = new DataTemplate(typeof(TextCenteredWithHeader)); ;
      _textCentered= new DataTemplate(typeof(TextCentered));
      _horizontalText = new DataTemplate(typeof(HorizontalText));
      _horizontalTextWithLink= new DataTemplate(typeof(HorizontalTextWithLink));
      _normal = new DataTemplate(typeof(Normal));
      _normalWithButton = new DataTemplate(typeof(NormalWithButton));
      _horizontalTextDangerWithLink = new DataTemplate(typeof(HorizontalTextDangerWithLink));
      _button = new DataTemplate(typeof(Button));
      _link= new DataTemplate(typeof(Link));
      _header= new DataTemplate(typeof(Header));
      _textCenteredWithError= new DataTemplate(typeof(TextCenteredWithError));

    }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)

    {

      if (item is CellItem)
      {
        var estimatedP1Item = item as CellItem;
        if (estimatedP1Item != null)
          switch (estimatedP1Item.CellType)
          {
            case "TextCenteredWithHeader":
              return _textCenteredWithHeader;
            case "TextCentered":
              return _textCentered;
            case "HorizontalText":
              return _horizontalText;
            case "HorizontalTextWithLink":
              return _horizontalTextWithLink;
            case "Normal":
              return _normal;
            case "NormalWithButton":
              return _normalWithButton;
            case "HorizontalTextDangerWithLink":
              return _horizontalTextDangerWithLink;
            case "Button":
              return _button;
            case "Link":
              return _link;
            case "Header":
              return _header;
            case "TextCenteredWithError":
              return _textCenteredWithError;
          }
      }
      return null;
    }
  }
}

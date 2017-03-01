using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public partial class RadioButton : ContentView
  {
    public static readonly BindableProperty CheckedProperty =
      BindableProperty.Create("Checked", typeof(bool?), typeof(RadioButton), null, BindingMode.TwoWay, propertyChanged: onValueChanged);

    private static void onValueChanged(BindableObject bindable, object oldvalue, object newvalue)
    {

      var view = bindable as RadioButton;
      if (newvalue == null)
        view.img.Source = "Unchecked.png";
      else if (oldvalue == null && (bool)newvalue == true)
        view.img.Source = "Checked.png";
      else if (oldvalue == null && (bool)newvalue == false)
        view.img.Source = "Unchecked.png";
      else if ((bool) oldvalue == false && (bool) newvalue == true)
        view.img.Source = "Checked.png";
      else if ((bool)newvalue == false && (bool)oldvalue == true)
        view.img.Source = "Unchecked.png";
    }

    public string Text
    {
      set
      {
        text.Text = value;
      }
    }

    public bool? Checked
    {
      get
      {
        return (bool?)GetValue(CheckedProperty);
      }
      set
      {
        SetValue(CheckedProperty, value);
      }
    }

    public RadioButton()
    {
      InitializeComponent();
      var tgr= new TapGestureRecognizer();
      tgr.Tapped += (s, e) => OnButtonClicked();
      radio.GestureRecognizers.Add(tgr);
    }

    private void OnButtonClicked()
    {
      this.Checked = true;
    }
  }
}

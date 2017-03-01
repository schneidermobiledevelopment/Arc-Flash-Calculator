using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public partial class CustomPickerCell : ViewCell
  {

    public static readonly BindableProperty ItemsSourceProperty =
    BindableProperty.Create("ItemSource", typeof(IList), typeof(CustomPickerCell), default(IList), BindingMode.TwoWay, propertyChanged:OnPropertyChanged);

    private static void OnPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
      var view = bindable as CustomPickerCell;
      if (newvalue != null)
      {
        view.picker.ItemsSource = (IList) newvalue;
      }
    }

    public static readonly BindableProperty SelectedItemProperty =
    BindableProperty.Create("SelectedItem", typeof(object), typeof(CustomPickerCell), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

    public static readonly BindableProperty IsPickerEnabledProperty =
    BindableProperty.Create("IsPickerEnabled", typeof(bool), typeof(CustomPickerCell), true, BindingMode.TwoWay, propertyChanged:OnIsEnabledChanged);

    private static void OnIsEnabledChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
      var view = bindable as CustomPickerCell;
      if (newvalue != null)
      {
        view.picker.IsEnabled = (bool)newvalue;
      }
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
      var view = bindable as CustomPickerCell;
      if (newvalue != null)
      {
        view.picker.SelectedItem = newvalue;
      }
    }

    public string LabelText
    {
      set
      {
        lbl.Text = value;
      }
    }

    public bool IsRequired
    {
      set
      {
        lbl.IsRequired = value;
      }
    }

    public string PickerTitle
    {
      get { return picker.Title; }
      set { picker.Title = value; }
    }

    public IList ItemSource
    {
      get { return (IList)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    public object SelectedItem
    {
      get { return GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }

    public bool IsPickerEnabled
    {
      get { return (bool)GetValue(IsPickerEnabledProperty); }
      set { SetValue(IsPickerEnabledProperty, value); }
    }

    public CustomPickerCell()
    {
      InitializeComponent();
    }
  }
}

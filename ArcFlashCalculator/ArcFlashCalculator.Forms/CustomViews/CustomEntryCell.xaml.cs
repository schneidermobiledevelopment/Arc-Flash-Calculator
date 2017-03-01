using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public partial class CustomEntryCell : ViewCell
  {
    public static readonly BindableProperty EntryTextProperty =
      BindableProperty.Create("EntryText", typeof(string),typeof(CustomEntryCell), null,BindingMode.TwoWay, propertyChanged: OnTextChanged);

    public static readonly BindableProperty ErrorTextProperty =
      BindableProperty.Create("ErrorText", typeof(string), typeof(CustomEntryCell), null, BindingMode.OneWay, propertyChanged: OnInputChanged);

    public static readonly BindableProperty FocusProperty = BindableProperty.Create("EntryText",typeof(bool),typeof(CustomEntryCell),false,BindingMode.OneWayToSource,propertyChanged:OnFocusChanged);

    private static void OnFocusChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
      var entry = (CustomEntryCell) bindable;
      if ((bool) newvalue)
      {
        entry.WillFocus = true;
        entry.OnAppearing();
        
      }
  }

    protected override void OnTapped()
    {
      base.OnTapped();
      this.WillFocus = true;
      this.OnAppearing();
    }

    public event EventHandler<TextChangedEventArgs> TextChanged;

    static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var entry = (CustomEntryCell)bindable;

      entry.TextChanged?.Invoke(entry, new TextChangedEventArgs((string)oldValue, (string)newValue));
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      FocusEntry();
    }

    private void FocusEntry()
    {
      if(WillFocus)
        entry.Focus();
    }

    public string LabelText
    {
      set
      {
        lbl.Text = value;
      }
    }

    public Keyboard Keyboard {set
      {
        entry.Keyboard=value;
      }
    }

    public GridLength WidthC1 { set { C1.Width=value ; } }

    public GridLength WidthC2 { set { C2.Width = value; } }

    public bool IsRequired {
      set
      {
        lbl.IsRequired=value;
      }
    }

    public string EntryPlaceholder {
      set
      {
        entry.Placeholder=value;
      }
    }

    public bool Focus
    {
      get
      {
        return (bool)GetValue(FocusProperty);
      }
      set
      {
        SetValue(FocusProperty, value);
      }
    }

    public string EntryText {
      get
      {
        return (string)GetValue(EntryTextProperty);
      }
      set
      {
        SetValue(EntryTextProperty, value);
      }
    }

    public string ErrorText
    {
      get
      {
        return (string)GetValue(ErrorTextProperty);
      }
      set
      {
        SetValue(ErrorTextProperty, value);
      }
    }

    private static void OnInputChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var View = bindable as CustomEntryCell;
      if (newValue == null)
      {
        View.entry.TextColor = Color.Black;
        if (Device.OS == TargetPlatform.Android)
        {
          View.errorStack.IsVisible = false;
        }
        else if (Device.OS == TargetPlatform.iOS)
        {
          View.errorImg.IsVisible = false;
          View.error.IsVisible = false;
        }
      }
      else
      {
        View.entry.TextColor = Color.Red;
        if (Device.OS == TargetPlatform.Android)
        {
          View.errorStack.IsVisible = true;
        }
        else if (Device.OS == TargetPlatform.iOS)
        {
          View.errorImg.IsVisible = true;
          View.error.IsVisible = true;
        }
       }
    }

    public bool WillFocus { get; set; }
    protected override void OnBindingContextChanged()
    {
      base.OnBindingContextChanged();
      this.ForceUpdateSize();
    }

    public CustomEntryCell():base()
    {
      InitializeComponent();
      if (Device.OS == TargetPlatform.iOS)
      {
        this.errorStack.HeightRequest = 15;
        this.errorImg.IsVisible = false;
        this.error.IsVisible = false;
      }
    }
  }
}

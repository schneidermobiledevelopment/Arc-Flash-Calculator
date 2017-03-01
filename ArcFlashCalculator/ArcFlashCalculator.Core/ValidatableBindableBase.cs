using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace ArcFlashCalculator.Core.Model
{
  public abstract class ValidatableBindableBase : BindableBase
  {
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public abstract void ValidateAllProperties();
    public abstract void UpdateErrors();
    public abstract void PublishObject();

    protected virtual bool DoSetterWork<T>(
        ref T storage,
        T value,
        [CallerMemberName] string propertyName = null)
    {
      // ReSharper disable once ExplicitCallerInfoArgument
      var result = this.SetProperty(ref storage, value, propertyName);

      if (result)
      {
        this.ValidateAllProperties();
        this.UpdateErrors();
        this.PublishObject();
      }

      return result;
    }    

    protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
    {
      var handler = this.ErrorsChanged;
      if (handler != null)
      {
        handler(this, e);
      }
    }
  }
}

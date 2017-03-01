using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentValidation;

namespace ArcFlashCalculator.Core.Model
{

  public class FluentValidator<T> : AbstractValidator<T>, IValidator<T>
  {
    private readonly IDictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors
    {
      get { return _errors.Count > 0; }
    }

    IDictionary<string, List<string>> IValidator<T>.Validate(T instance)
    {
      ValidateAndUpdateErrors(instance);

      return _errors;
    }

    public IEnumerable GetErrors(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName))
      {
        // The caller requests all errors associated with this object.
        return GetAllErrors();
      }

      ThrowIfInvalidPropertyName(propertyName);

      return ExtractErrorMessageOf(propertyName);
    }

    public IList<string> GetAllErrors()
    {
      return new List<string>(); //errors.Select(error => error.Value).ToList();
    }

    protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
    {
      var handler = ErrorsChanged;
      handler?.Invoke(this, e);
    }

    private static void ThrowIfInvalidPropertyName(string propertyName)
    {
      var propertyInfo = typeof(T).GetRuntimeProperty(propertyName);
      if (propertyInfo == null)
      {
        var msg = string.Format("No such property name '{0}' in {1}", propertyName, typeof(T));
        throw new ArgumentException(msg, propertyName);
      }
    }

    private void ValidateAndUpdateErrors(T instance)
    {
      _errors.Clear();
      var result = Validate(instance);
      if (result.IsValid)
      {
        return;
      }

      foreach (var err in result.Errors)
      {
        if (_errors.ContainsKey(err.PropertyName))
        {
          _errors[err.PropertyName].Add(err.ErrorMessage);
        }
        else
        {
          _errors.Add(err.PropertyName, new List<string>() { err.ErrorMessage });
        }
      }
    }

    private IEnumerable ExtractErrorMessageOf(string propertyName)
    {
      var result = new List<string>();
      if (_errors.ContainsKey(propertyName))
      {
        result.Add(_errors[propertyName][0]);
      }

      return result;
    }
  }
}

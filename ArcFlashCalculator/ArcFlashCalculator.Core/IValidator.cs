using System.Collections.Generic;
using System.ComponentModel;

namespace ArcFlashCalculator.Core.Model
{ 
  public interface IValidator<in T> : INotifyDataErrorInfo
  {
    IDictionary<string, List<string>> Validate(T instance);
  }
}

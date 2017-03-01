using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace ArcFlashCalculator.Core.Model
{
//git checkin
  public abstract class BaseModel : BindableBase
  {
    public int Id { get; set; }
  }
}

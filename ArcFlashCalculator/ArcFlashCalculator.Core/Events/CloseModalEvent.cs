using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace ArcFlashCalculator.Core.Events
{
  public class CloseModalEvent : PubSubEvent<string>
  {
  }
}

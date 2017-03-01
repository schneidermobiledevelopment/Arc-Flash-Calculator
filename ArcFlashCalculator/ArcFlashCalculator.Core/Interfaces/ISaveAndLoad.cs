using System.IO;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Interfaces
{
  public interface ISaveAndLoad
  {
    Task SaveAndCreatePdfAsync(string filename, MemoryStream memoryStream);
    bool FileExists(string filename);
  }
}

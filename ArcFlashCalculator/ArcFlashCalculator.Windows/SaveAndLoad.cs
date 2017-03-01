using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Core.Interfaces;

namespace ArcFlashCalculator.WPF
{
  public class SaveAndLoad : ISaveAndLoad
  {
    public async Task SaveAndCreatePdfAsync(string filename, MemoryStream memoryStream)
    {
      var path = CreatePathToFile(filename);

      if (FileExists(filename))
      {
        File.Delete(path);
      }

      byte[] bytes = memoryStream.ToArray();

      using (FileStream sourceStream = new FileStream(path,
          FileMode.Append, FileAccess.Write, FileShare.None,
          bufferSize: 4096, useAsync: true))
      {
        await sourceStream.WriteAsync(bytes, 0, bytes.Length);
      };
    }

    public bool FileExists(string filename)
    {
      return File.Exists(CreatePathToFile(filename));
    }

    string CreatePathToFile(string filename)
    {
      var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      return Path.Combine(docsPath, filename);
    }
  }
}

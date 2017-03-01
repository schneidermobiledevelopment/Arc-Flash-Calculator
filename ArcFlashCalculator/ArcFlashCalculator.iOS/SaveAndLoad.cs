using System;
using System.IO;
using System.Threading.Tasks;
using ArcFlashCalculator.Core.Interfaces;
using Foundation;

namespace ArcFlashCalculator.iOS
{
  internal class SaveAndLoad : ISaveAndLoad
  {
    #region ISaveAndLoad implementation

    public bool FileExists(string filename)
    {
      return File.Exists(CreatePathToFile(filename));
    }

    #endregion

    string CreatePathToFile(string filename)
    {
      var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      Directory.CreateDirectory(docsPath + "SchneiderElectric/ArcFlashCalculator");
      return Path.Combine(docsPath, filename);
    }

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
  }
}
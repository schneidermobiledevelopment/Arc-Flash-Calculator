using System;
using Xamarin.Forms;
using ArcFlashCalculator.Droid;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using ArcFlashCalculator.Core.Interfaces;
using Xfinium.Pdf;
using Xfinium.Pdf.Graphics;

namespace ArcFlashCalculator.Droid
{
  public class SaveAndLoad : ISaveAndLoad
  {
    #region ISaveAndLoad implementation

    public bool FileExists(string filename)
    {
      return File.Exists(CreatePathToFile(filename));
    }

    #endregion

    string CreatePathToFile(string filename)
    {
      var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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
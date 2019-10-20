// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.DirectorySorter
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Messaging;
using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImageOrganizer.Helpers
{
  internal sealed class DirectorySorter : MessagingBase, IDirectorySorter, IProgressReporter, IMessageSender
  {
    private readonly ISystemContext systemContext;

    public DirectorySorter(ISystemContext systemContext)
    {
      this.systemContext = systemContext;
    }

    public void SortDirectory(string directory, string fileNamePattern)
    {
      if (string.IsNullOrWhiteSpace(directory))
        throw new ArgumentException(Resources.Error_InvalidInputDirectory, nameof (directory));
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_SortingDirectory, (object) directory), MessageType.Information));
      fileNamePattern = fileNamePattern == null ? string.Empty : fileNamePattern.Trim();
      IOrderedEnumerable<string> source1 = this.systemContext.GetFiles(directory, "*.jpg", SearchOption.TopDirectoryOnly).OrderBy<string, DateTime>((Func<string, DateTime>) (file => this.systemContext.GetFileCreationTime(file)));
      int num1 = source1.Count<string>();
      string fileNameFormat = DirectorySorter.ResolveFileNameFormat(fileNamePattern, num1, ".jpg");
      string firstImage = (string) null;
      int counter = 1;
      int num2 = 0;
      int errors = 0;
      foreach (string source2 in (IEnumerable<string>) source1)
      {
        try
        {
          string targetFileName = this.DetermineTargetFileName(fileNameFormat, ref counter, directory);
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_MovingFile, (object) source2, (object) targetFileName), MessageType.Verbose));
          this.systemContext.MoveFile(source2, targetFileName);
          if (num2 == 0)
            firstImage = targetFileName;
        }
        catch (Exception ex)
        {
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Warning_FailedToProcessFile, (object) source2, (object) ex.Message), MessageType.Warning));
          ++errors;
        }
        this.OnProgressChanged(new ProgressChangedEventArgs(MessagingBase.CalculatePercentage(++num2, num1), 0));
        ++counter;
      }
      this.OnProgressChanged(new ProgressChangedEventArgs(100, 100));
      this.OnCompleted(new CompletedEventArgs(DirectorySorter.PrepareSummary(num1, counter, errors), directory, firstImage));
    }

    private static string ResolveFileNameFormat(
      string fileNamePattern,
      int totalFilesCount,
      string extension)
    {
      string replacement = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{0:{0}}}", (object) new string('0', (int) Math.Log10((double) totalFilesCount) + 1));
      string str = Regex.Replace(fileNamePattern, "(\\{(C|c)\\})|(\\[(C|c)\\])", replacement);
      if (string.IsNullOrWhiteSpace(str))
        str = replacement;
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}{1}", (object) str, (object) extension);
    }

    private static string CreateFilePath(string fileNameFormat, int counter, string directory)
    {
      string path2 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, fileNameFormat, (object) counter);
      return Path.Combine(directory, path2);
    }

    private static IDictionary<string, object> PrepareSummary(
      int filesCount,
      int fileNameCounter,
      int errors)
    {
      return (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          Resources.Summary_FileNameConflictsResolved,
          (object) (fileNameCounter - filesCount - 1)
        },
        {
          Resources.Summary_Errors,
          (object) errors
        }
      };
    }

    private string DetermineTargetFileName(
      string fileNameFormat,
      ref int counter,
      string directory)
    {
      string filePath;
      for (filePath = DirectorySorter.CreateFilePath(fileNameFormat, counter, directory); this.systemContext.FileExists(filePath); filePath = DirectorySorter.CreateFilePath(fileNameFormat, counter, directory))
        ++counter;
      return filePath;
    }
  }
}

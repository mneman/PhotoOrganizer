// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Manipulators.ImageDirectoryMerger
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Helpers;
using ImageOrganizer.Messaging;
using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ImageOrganizer.Manipulators
{
  internal sealed class ImageDirectoryMerger : MessagingBase, IImageDirectoryMerger, IMessageSender, IProgressReporter
  {
    private readonly ISystemContext systemContext;
    private readonly IComponentsFactory componentsFactory;
    private int currentTotalPercentage;
    private string firstFile;
    private IDictionary<string, object> directorySorterSummary;

    public ImageDirectoryMerger(IComponentsFactory componentsFactory, ISystemContext systemContext)
    {
      this.systemContext = systemContext;
      this.componentsFactory = componentsFactory;
      this.OnMessageSent(new MessageSentEventArgs(Resources.Info_ImageDirectoryMergerInitialized, MessageType.Information));
    }

    public void MergeDirectories(
      IList<string> inputDirectories,
      string outputDirectory,
      string fileNamePattern,
      bool deleteSourceDirectories)
    {
      if (inputDirectories == null)
        throw new ArgumentException(Resources.Error_InvalidDirectoriesList, nameof (inputDirectories));
      if (string.IsNullOrWhiteSpace(outputDirectory))
        throw new ArgumentException(Resources.Error_InvalidOutputDirectory, outputDirectory);
      this.EnsureDirectoryExists(outputDirectory);
      this.currentTotalPercentage = 0;
      this.firstFile = (string) null;
      this.directorySorterSummary = (IDictionary<string, object>) null;
      int processedDirectoriesCount = 0;
      int filesCount = 0;
      int deletedDirectories = 0;
      int errorsCount = 0;
      int count = inputDirectories.Count;
      this.OnProgressChanged(new ProgressChangedEventArgs(0, 0));
      if (inputDirectories.IndexOf(outputDirectory) > 0 && inputDirectories.Remove(outputDirectory))
        inputDirectories.Insert(0, outputDirectory);
      for (int index = 0; index < count; ++index)
      {
        string inputDirectory = inputDirectories[index];
        if (this.systemContext.DirectoryExists(inputDirectory))
        {
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_ProcessingDirectory, (object) inputDirectory), MessageType.Information));
          this.MoveImagesToOutputDirectory(inputDirectory, outputDirectory, ref filesCount, ref errorsCount);
          ++processedDirectoriesCount;
        }
        this.currentTotalPercentage = MessagingBase.CalculatePercentage(index + 1, count + 1);
        this.OnProgressChanged(new ProgressChangedEventArgs(0, this.currentTotalPercentage));
      }
      if (processedDirectoriesCount > 0)
        this.SortOutputDirectory(outputDirectory, fileNamePattern);
      if (deleteSourceDirectories)
      {
        this.OnMessageSent(new MessageSentEventArgs(Resources.Info_CleaningSourceDirectories, MessageType.Information));
        inputDirectories.Remove(outputDirectory);
        this.CleanSourceDirectories((IEnumerable<string>) inputDirectories, ref deletedDirectories, ref errorsCount);
      }
      this.OnProgressChanged(new ProgressChangedEventArgs(100, 100));
      this.OnCompleted(new CompletedEventArgs(this.PrepareSummary(filesCount, processedDirectoriesCount, deletedDirectories, errorsCount), outputDirectory, this.firstFile));
    }

    private void SortOutputDirectory(string outputDirectory, string fileNamePattern)
    {
      IDirectorySorter directorySorter = this.componentsFactory.CreateDirectorySorter(this.systemContext);
      directorySorter.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(this.HandleDirectorySorterProgressChanged);
      directorySorter.Completed += new EventHandler<CompletedEventArgs>(this.HandleDirectorySorterCompleted);
      directorySorter.MessageSent += new EventHandler<MessageSentEventArgs>(this.HandleDirectorySorterMessageSent);
      directorySorter.UserInput += new EventHandler<UserInputEventArgs>(this.HandleDirectorySorterUserInput);
      directorySorter.SortDirectory(outputDirectory, fileNamePattern);
      directorySorter.ProgressChanged -= new EventHandler<ProgressChangedEventArgs>(this.HandleDirectorySorterProgressChanged);
      directorySorter.Completed -= new EventHandler<CompletedEventArgs>(this.HandleDirectorySorterCompleted);
      directorySorter.MessageSent -= new EventHandler<MessageSentEventArgs>(this.HandleDirectorySorterMessageSent);
      directorySorter.UserInput -= new EventHandler<UserInputEventArgs>(this.HandleDirectorySorterUserInput);
    }

    private void CleanSourceDirectories(
      IEnumerable<string> sourceDirectories,
      ref int deletedDirectories,
      ref int errorsCount)
    {
      string str = (string) null;
      foreach (string sourceDirectory in sourceDirectories)
      {
        try
        {
          bool recursive;
          if (this.systemContext.DirectoryEntries(sourceDirectory).Any<string>())
          {
            if (string.IsNullOrWhiteSpace(str))
            {
              UserInputEventArgs e = new UserInputEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Warning_DeletingNonEmptyDirectory, (object) sourceDirectory), UserInputType.Warning);
              this.OnUserInput(e);
              str = (e.UserInput ?? string.Empty).ToUpper(CultureInfo.CurrentCulture);
            }
            switch (str)
            {
              case "YA":
                recursive = true;
                break;
              case "NA":
                continue;
              case "Y":
              case "YES":
                str = (string) null;
                goto case "YA";
              default:
                str = (string) null;
                continue;
            }
          }
          else
            recursive = false;
          this.systemContext.DeleteDirectory(sourceDirectory, recursive);
          ++deletedDirectories;
        }
        catch (Exception ex)
        {
          this.OnMessageSent(new MessageSentEventArgs(ex.Message, MessageType.Warning));
          ++errorsCount;
        }
      }
    }

    private void HandleDirectorySorterUserInput(
      object sender,
      UserInputEventArgs userInputEventArgs)
    {
      this.OnUserInput(userInputEventArgs);
    }

    private void HandleDirectorySorterMessageSent(
      object sender,
      MessageSentEventArgs messageSentEventArgs)
    {
      this.OnMessageSent(messageSentEventArgs);
    }

    private void HandleDirectorySorterProgressChanged(
      object sender,
      ProgressChangedEventArgs progressChangedEventArgs)
    {
      this.OnProgressChanged(new ProgressChangedEventArgs(progressChangedEventArgs.Progress, this.currentTotalPercentage));
    }

    private void HandleDirectorySorterCompleted(
      object sender,
      CompletedEventArgs completedEventArgs)
    {
      this.firstFile = completedEventArgs.FirstImage;
      this.directorySorterSummary = completedEventArgs.Summary;
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_DirectorySorted, (object) completedEventArgs.Directory), MessageType.Verbose));
    }

    private void EnsureDirectoryExists(string directory)
    {
      if (this.systemContext.DirectoryExists(directory))
        return;
      this.systemContext.CreateDirectory(directory);
    }

    private void MoveImagesToOutputDirectory(
      string inputDirectory,
      string outputDirectory,
      ref int filesCount,
      ref int errorsCount)
    {
      ICollection<string> files = this.systemContext.GetFiles(inputDirectory, "*.jpg", SearchOption.TopDirectoryOnly);
      int count = files.Count;
      filesCount += count;
      int num = 0;
      foreach (string file in (IEnumerable<string>) files)
      {
        try
        {
          this.ProcessFile(file, outputDirectory);
        }
        catch (Exception ex)
        {
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Warning_FailedToProcessFile, (object) file, (object) ex.Message), MessageType.Warning));
          ++errorsCount;
        }
        this.OnProgressChanged(new ProgressChangedEventArgs(MessagingBase.CalculatePercentage(++num, count), this.currentTotalPercentage));
      }
    }

    private void ProcessFile(string file, string outputDirectory)
    {
      string targetFileName = this.GetTargetFileName(outputDirectory, Path.GetExtension(file));
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_MovingFile, (object) file, (object) targetFileName), MessageType.Verbose));
      this.systemContext.MoveFile(file, targetFileName);
    }

    private string GetTargetFileName(string outputDirectory, string extension)
    {
      string path2_1 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TemporaryFileNameFormat, (object) Guid.NewGuid(), (object) extension);
      string path;
      string path2_2;
      for (path = Path.Combine(outputDirectory, path2_1); this.systemContext.FileExists(path); path = Path.Combine(outputDirectory, path2_2))
        path2_2 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TemporaryFileNameFormat, (object) Guid.NewGuid(), (object) extension);
      return path;
    }

    private IDictionary<string, object> PrepareSummary(
      int filesCount,
      int processedDirectoriesCount,
      int deletedDirectoriesCount,
      int errorsCount)
    {
      int num1 = 0;
      int num2 = 0;
      if (this.directorySorterSummary != null)
      {
        if (this.directorySorterSummary.ContainsKey(Resources.Summary_Errors))
          num1 = (int) this.directorySorterSummary[Resources.Summary_Errors];
        if (this.directorySorterSummary.ContainsKey(Resources.Summary_FileNameConflictsResolved))
          num2 = (int) this.directorySorterSummary[Resources.Summary_FileNameConflictsResolved];
      }
      return (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          Resources.Summary_FilesProcessed,
          (object) filesCount
        },
        {
          Resources.Summary_DirectoriesProcessed,
          (object) processedDirectoriesCount
        },
        {
          Resources.Summary_DirectoriesDeleted,
          (object) deletedDirectoriesCount
        },
        {
          Resources.Summary_FileNameConflictsResolved,
          (object) num2
        },
        {
          Resources.Summary_Errors,
          (object) (errorsCount + num1)
        }
      };
    }
  }
}

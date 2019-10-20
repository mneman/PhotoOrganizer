// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Manipulators.ImageSorter
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Helpers;
using ImageOrganizer.Imaging;
using ImageOrganizer.Messaging;
using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ImageOrganizer.Manipulators
{
  internal sealed class ImageSorter : MessagingBase, IImageSorter, IMessageSender, IProgressReporter
  {
    private readonly IComponentsFactory componentsFactory;
    private readonly ISystemContext systemContext;
    private IDictionary<string, object> directorySorterSummary;

    public ImageSorter(IComponentsFactory componentsFactory, ISystemContext systemContext)
    {
      this.componentsFactory = componentsFactory;
      this.systemContext = systemContext;
      this.OnMessageSent(new MessageSentEventArgs(Resources.Info_ImageSorterInitialized, MessageType.Information));
    }

    public void SortImages(
      string directory,
      bool rotateImages,
      string outputDirectory,
      string fileNamePattern)
    {
      if (string.IsNullOrWhiteSpace(directory))
        throw new ArgumentException(Resources.Error_InvalidInputDirectory, nameof (directory));
      if (!this.systemContext.DirectoryExists(directory))
        throw new FileNotFoundException(Resources.Error_InputDirectoryNotFound, directory);
      if (string.IsNullOrWhiteSpace(outputDirectory))
        throw new ArgumentException(Resources.Error_InvalidOutputDirectory, nameof (outputDirectory));
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_ProcessingDirectory, (object) directory), MessageType.Information));
      this.directorySorterSummary = (IDictionary<string, object>) null;
      this.OnProgressChanged(new ProgressChangedEventArgs(0, 0));
      int filesCount;
      int errorsCount;
      int rotatedImagesCount;
      IList<string> subDirectories = this.DistributeImagesToSubDirectories(directory, outputDirectory, rotateImages, out filesCount, out errorsCount, out rotatedImagesCount);
      this.OnProgressChanged(new ProgressChangedEventArgs(0, 50));
      this.SortSubDirectories((IEnumerable<string>) subDirectories, fileNamePattern);
      this.OnProgressChanged(new ProgressChangedEventArgs(100, 100));
      this.OnCompleted(new CompletedEventArgs(this.PrepareSummary(filesCount, rotatedImagesCount, subDirectories.Count, errorsCount), outputDirectory, (string) null));
    }

    private static void SaveImageWithRotation(
      IImageWrapper image,
      string targetFileName,
      EncoderParameters encoderParameters)
    {
      ImageCodecInfo codec = ((IEnumerable<ImageCodecInfo>) ImageCodecInfo.GetImageEncoders()).Where<ImageCodecInfo>((Func<ImageCodecInfo, bool>) (enc => enc.MimeType == "image/jpeg")).FirstOrDefault<ImageCodecInfo>();
      image.Save(targetFileName, codec, encoderParameters);
    }

    private IList<string> DistributeImagesToSubDirectories(
      string directory,
      string outputDirectory,
      bool rotateImages,
      out int filesCount,
      out int errorsCount,
      out int rotatedImagesCount)
    {
      errorsCount = 0;
      rotatedImagesCount = 0;
      ICollection<string> files = this.systemContext.GetFiles(directory, "*.jpg", SearchOption.TopDirectoryOnly);
      IList<string> subDirectories = (IList<string>) new List<string>();
      filesCount = files.Count;
      int num = 0;
      foreach (string file in (IEnumerable<string>) files)
      {
        try
        {
          bool imageRotated;
          this.ProcessFile(file, rotateImages, outputDirectory, subDirectories, out imageRotated);
          if (imageRotated)
            ++rotatedImagesCount;
        }
        catch (Exception ex)
        {
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Warning_FailedToProcessFile, (object) file, (object) ex.Message), MessageType.Warning));
          ++errorsCount;
        }
        this.OnProgressChanged(new ProgressChangedEventArgs(MessagingBase.CalculatePercentage(++num, filesCount), 0));
      }
      this.OnProgressChanged(new ProgressChangedEventArgs(100, 0));
      return subDirectories;
    }

    private void ProcessFile(
      string file,
      bool rotateImage,
      string outputDirectory,
      IList<string> subDirectories,
      out bool imageRotated)
    {
      imageRotated = false;
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_ProcessingFile, (object) file), MessageType.Verbose));
      DateTime dateTimeTaken;
      string targetDirectory;
      string targetFileName;
      using (IImageWrapper imageWrapper = this.componentsFactory.CreateImageWrapper(file))
      {
        dateTimeTaken = imageWrapper.DateTimeTaken;
        targetFileName = this.GetTargetFileName(outputDirectory, dateTimeTaken, Path.GetExtension(file), out targetDirectory);
        EncoderParameters encoderParameters = rotateImage ? this.RotateImage(imageWrapper) : (EncoderParameters) null;
        if (encoderParameters != null)
        {
          ImageSorter.SaveImageWithRotation(imageWrapper, targetFileName, encoderParameters);
          imageRotated = true;
        }
      }
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_MovingFile, (object) Path.GetFileName(file), (object) targetFileName), MessageType.Verbose));
      if (!subDirectories.Contains(targetDirectory))
        subDirectories.Add(targetDirectory);
      if (imageRotated)
        this.systemContext.DeleteFile(file);
      else
        this.systemContext.MoveFile(file, targetFileName);
      this.SetFileDates(targetFileName, dateTimeTaken);
    }

    private void SortSubDirectories(IEnumerable<string> subDirectories, string fileNamePattern)
    {
      IDirectorySorter directorySorter = this.componentsFactory.CreateDirectorySorter(this.systemContext);
      directorySorter.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(this.HandleDirectorySorterProgressChanged);
      directorySorter.Completed += new EventHandler<CompletedEventArgs>(this.HandleDirectorySorterCompleted);
      directorySorter.MessageSent += new EventHandler<MessageSentEventArgs>(this.HandleDirectorySorterMessageSent);
      directorySorter.UserInput += new EventHandler<UserInputEventArgs>(this.HandleDirectorySorterUserInput);
      foreach (string subDirectory in subDirectories)
        directorySorter.SortDirectory(subDirectory, fileNamePattern);
      directorySorter.ProgressChanged -= new EventHandler<ProgressChangedEventArgs>(this.HandleDirectorySorterProgressChanged);
      directorySorter.Completed -= new EventHandler<CompletedEventArgs>(this.HandleDirectorySorterCompleted);
      directorySorter.MessageSent -= new EventHandler<MessageSentEventArgs>(this.HandleDirectorySorterMessageSent);
      directorySorter.UserInput -= new EventHandler<UserInputEventArgs>(this.HandleDirectorySorterUserInput);
    }

    private EncoderParameters RotateImage(IImageWrapper image)
    {
      IImageRotator imageRotator = this.componentsFactory.CreateImageRotator();
      imageRotator.MessageSent += new EventHandler<MessageSentEventArgs>(this.HandleMessageSent);
      EncoderParameters encoderParameters = imageRotator.Rotate(image);
      imageRotator.MessageSent -= new EventHandler<MessageSentEventArgs>(this.HandleMessageSent);
      return encoderParameters;
    }

    private string GetTargetFileName(
      string outputDirectory,
      DateTime creationDateTime,
      string extension,
      out string targetDirectory)
    {
      string path2_1 = creationDateTime.ToString("yyyy.MM.dd", (IFormatProvider) CultureInfo.InvariantCulture);
      targetDirectory = Path.Combine(outputDirectory, path2_1);
      this.EnsureDirectoryExists(targetDirectory);
      string path2_2 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TemporaryFileNameFormat, (object) Guid.NewGuid(), (object) extension);
      string path;
      string path2_3;
      for (path = Path.Combine(targetDirectory, path2_2); this.systemContext.FileExists(path); path = Path.Combine(targetDirectory, path2_3))
        path2_3 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TemporaryFileNameFormat, (object) Guid.NewGuid(), (object) extension);
      return path;
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
      this.OnProgressChanged(new ProgressChangedEventArgs(progressChangedEventArgs.Progress, 50));
    }

    private void HandleDirectorySorterCompleted(
      object sender,
      CompletedEventArgs completedEventArgs)
    {
      this.directorySorterSummary = completedEventArgs.Summary;
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_DirectorySorted, (object) completedEventArgs.Directory), MessageType.Verbose));
    }

    private void EnsureDirectoryExists(string directory)
    {
      if (this.systemContext.DirectoryExists(directory))
        return;
      this.systemContext.CreateDirectory(directory);
    }

    private void SetFileDates(string file, DateTime dateTime)
    {
      this.systemContext.SetFileCreationTime(file, dateTime);
      this.systemContext.SetFileLastAccessTime(file, dateTime);
      this.systemContext.SetFileLastWriteTime(file, dateTime);
    }

    private IDictionary<string, object> PrepareSummary(
      int filesCount,
      int rotatedImagesCount,
      int subDirectoriesCount,
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
          Resources.Summary_ImagesRotated,
          (object) rotatedImagesCount
        },
        {
          Resources.Summary_SubDirectoriesCreated,
          (object) subDirectoriesCount
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

    private void HandleMessageSent(object sender, MessageSentEventArgs messageSentEventArgs)
    {
      this.OnMessageSent(messageSentEventArgs);
    }
  }
}

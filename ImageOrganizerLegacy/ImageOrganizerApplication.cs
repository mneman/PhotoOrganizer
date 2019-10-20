// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.ImageOrganizerApplication
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Helpers;
using ImageOrganizer.Manipulators;
using ImageOrganizer.Messaging;
using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ImageOrganizer
{
  internal sealed class ImageOrganizerApplication : MessagingBase
  {
    private readonly Stopwatch stopWatch = new Stopwatch();
    private readonly ICommandLineParser commandLineParser;
    private readonly IImageManipulatorsFactory imageManipulatorFactory;
    private readonly IComponentsFactory componentsFactory;
    private readonly ISystemContext systemContext;

    public ImageOrganizerApplication(string[] commandLineArgs)
      : this((ICommandLineParser) new CommandLineParser(commandLineArgs), (IImageManipulatorsFactory) new ImageManipulatorsFactory(), (IComponentsFactory) new ComponentsFactory(), (ISystemContext) new SystemContext())
    {
    }

    internal ImageOrganizerApplication(
      ICommandLineParser commandLineParser,
      IImageManipulatorsFactory imageManipulatorFactory,
      IComponentsFactory componentsFactory,
      ISystemContext systemContext)
    {
      this.commandLineParser = commandLineParser;
      this.imageManipulatorFactory = imageManipulatorFactory;
      this.componentsFactory = componentsFactory;
      this.systemContext = systemContext;
    }

    public event EventHandler<InitializationEventArgs> Initialized;

    public void Start()
    {
      this.SendMessage(new MessageSentEventArgs(Resources.Application_Started, MessageType.Verbose));
      if (this.commandLineParser.IsHelp)
        this.DisplayApplicationHelp();
      else if (this.commandLineParser.IsMerge)
        this.StartMerge();
      else if (this.commandLineParser.IsSort)
        this.StartSort();
      else if (this.HandleEmptyCommandLine())
        this.StartSort();
      this.SendMessage(new MessageSentEventArgs(Resources.Application_PressAnyKeyToExit, MessageType.Information));
    }

    private void DisplayApplicationHelp()
    {
      this.SendMessage(new MessageSentEventArgs(Resources.Application_HelpText, MessageType.Information));
    }

    private void StartMerge()
    {
      try
      {
        this.RaiseMergerInitialized();
        IImageDirectoryMerger imageDirectoryMerger = this.imageManipulatorFactory.CreateImageDirectoryMerger(this.componentsFactory, this.systemContext);
        imageDirectoryMerger.MessageSent += new EventHandler<MessageSentEventArgs>(this.HandleMessageSent);
        imageDirectoryMerger.UserInput += new EventHandler<UserInputEventArgs>(this.HandleUserInput);
        imageDirectoryMerger.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(this.HandleProgressChanged);
        imageDirectoryMerger.Completed += new EventHandler<CompletedEventArgs>(this.HandleImageDirectoryMergerCompleted);
        this.stopWatch.Start();
        imageDirectoryMerger.MergeDirectories((IList<string>) this.commandLineParser.MergeDirectories.ToList<string>(), this.commandLineParser.OutputDirectory, this.commandLineParser.NamePattern, this.commandLineParser.IsClean);
        imageDirectoryMerger.MessageSent -= new EventHandler<MessageSentEventArgs>(this.HandleMessageSent);
        imageDirectoryMerger.UserInput -= new EventHandler<UserInputEventArgs>(this.HandleUserInput);
        imageDirectoryMerger.ProgressChanged -= new EventHandler<ProgressChangedEventArgs>(this.HandleProgressChanged);
        imageDirectoryMerger.Completed -= new EventHandler<CompletedEventArgs>(this.HandleImageDirectoryMergerCompleted);
      }
      catch (Exception ex)
      {
        this.SendMessage(new MessageSentEventArgs(ex.Message, MessageType.Error));
      }
    }

    private void StartSort()
    {
      try
      {
        string str = this.commandLineParser.SortDirectory ?? this.systemContext.GetCurrentWorkingDirectory();
        string outputDirectory = string.IsNullOrWhiteSpace(this.commandLineParser.OutputDirectory) ? str : this.commandLineParser.OutputDirectory;
        this.RaiseSorterInitialized(str, outputDirectory);
        IImageSorter imageSorter = this.imageManipulatorFactory.CreateImageSorter(this.componentsFactory, this.systemContext);
        imageSorter.MessageSent += new EventHandler<MessageSentEventArgs>(this.HandleMessageSent);
        imageSorter.UserInput += new EventHandler<UserInputEventArgs>(this.HandleUserInput);
        imageSorter.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(this.HandleProgressChanged);
        imageSorter.Completed += new EventHandler<CompletedEventArgs>(this.HandleImageSorterCompleted);
        this.stopWatch.Start();
        imageSorter.SortImages(str, !this.commandLineParser.IsNoRotate, outputDirectory, this.commandLineParser.NamePattern);
        imageSorter.MessageSent -= new EventHandler<MessageSentEventArgs>(this.HandleMessageSent);
        imageSorter.UserInput -= new EventHandler<UserInputEventArgs>(this.HandleUserInput);
        imageSorter.ProgressChanged -= new EventHandler<ProgressChangedEventArgs>(this.HandleProgressChanged);
        imageSorter.Completed -= new EventHandler<CompletedEventArgs>(this.HandleImageSorterCompleted);
      }
      catch (Exception ex)
      {
        this.SendMessage(new MessageSentEventArgs(ex.Message, MessageType.Error));
      }
    }

    private bool HandleEmptyCommandLine()
    {
      UserInputEventArgs e = new UserInputEventArgs(Resources.Warning_NoCommandLineArgs, UserInputType.Warning);
      this.OnUserInput(e);
      string str = string.IsNullOrWhiteSpace(e.UserInput) ? string.Empty : e.UserInput.ToUpper(CultureInfo.CurrentCulture);
      if (!(str == "Y"))
        return str == "YES";
      return true;
    }

    private void HandleProgressChanged(
      object sender,
      ProgressChangedEventArgs progressChangedEventArgs)
    {
      this.OnProgressChanged(progressChangedEventArgs);
    }

    private void HandleUserInput(object sender, UserInputEventArgs userInputEventArgs)
    {
      this.OnUserInput(userInputEventArgs);
    }

    private void HandleMessageSent(object sender, MessageSentEventArgs messageSentEventArgs)
    {
      this.SendMessage(messageSentEventArgs);
    }

    private void HandleImageSorterCompleted(object sender, CompletedEventArgs completedEventArgs)
    {
      this.stopWatch.Stop();
      if (!this.commandLineParser.IsNoPreview)
      {
        if (!string.IsNullOrWhiteSpace(completedEventArgs.Directory) && this.systemContext.DirectoryExists(completedEventArgs.Directory))
        {
          ProcessStartInfo startInfo = new ProcessStartInfo(completedEventArgs.Directory)
          {
            ErrorDialog = true,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal
          };
          this.OnMessageSent(new MessageSentEventArgs(Resources.Info_LaunchingPreview, MessageType.Information));
          this.systemContext.StartProcess(startInfo);
        }
        else
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Warning_FailedToStartPreviewDueToInvalidOutputDirectory, (object) (string.IsNullOrWhiteSpace(completedEventArgs.Directory) ? string.Empty : Path.GetFullPath(completedEventArgs.Directory))), MessageType.Warning));
      }
      this.RaiseCompletedEvent(completedEventArgs.Summary);
    }

    private void HandleImageDirectoryMergerCompleted(
      object sender,
      CompletedEventArgs completedEventArgs)
    {
      this.stopWatch.Stop();
      if (!this.commandLineParser.IsNoPreview)
      {
        if (!string.IsNullOrWhiteSpace(completedEventArgs.FirstImage) && this.systemContext.FileExists(completedEventArgs.FirstImage))
        {
          ProcessStartInfo startInfo = new ProcessStartInfo(Path.GetFileName(completedEventArgs.FirstImage))
          {
            ErrorDialog = true,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal,
            WorkingDirectory = Path.GetFullPath(completedEventArgs.Directory)
          };
          this.OnMessageSent(new MessageSentEventArgs(Resources.Info_LaunchingPreview, MessageType.Information));
          this.systemContext.StartProcess(startInfo);
        }
        else
          this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Warning_FailedToStartPreviewDueToInvalidFirstImage, (object) (string.IsNullOrWhiteSpace(completedEventArgs.FirstImage) ? string.Empty : Path.GetFullPath(completedEventArgs.FirstImage))), MessageType.Warning));
      }
      this.RaiseCompletedEvent(completedEventArgs.Summary);
    }

    private void RaiseCompletedEvent(IDictionary<string, object> operationSummary)
    {
      operationSummary.Add(Resources.Summary_OperationDuration, (object) this.stopWatch.Elapsed);
      this.OnCompleted(new CompletedEventArgs(operationSummary, (string) null, (string) null));
    }

    private void SendMessage(MessageSentEventArgs messageSentEventArgs)
    {
      if (!this.commandLineParser.IsVerbose && messageSentEventArgs.MessageType <= MessageType.Verbose)
        return;
      this.OnMessageSent(messageSentEventArgs);
    }

    private void RaiseSorterInitialized(string inputDirectory, string outputDirectory)
    {
      this.OnInitialized(new InitializationEventArgs(Resources.Operation_Sorting, (this.commandLineParser.IsVerbose ? 1 : 0) != 0, new bool?(!this.commandLineParser.IsNoRotate), new bool?(), (!this.commandLineParser.IsNoPreview ? 1 : 0) != 0, (IList<string>) new List<string>()
      {
        inputDirectory
      }, outputDirectory, this.commandLineParser.NamePattern));
    }

    private void RaiseMergerInitialized()
    {
      this.OnInitialized(new InitializationEventArgs(Resources.Operation_Merging, this.commandLineParser.IsVerbose, new bool?(), new bool?(this.commandLineParser.IsClean), !this.commandLineParser.IsNoPreview, (IList<string>) this.commandLineParser.MergeDirectories.ToList<string>(), this.commandLineParser.OutputDirectory, this.commandLineParser.NamePattern));
    }

    private void OnInitialized(InitializationEventArgs e)
    {
      EventHandler<InitializationEventArgs> initialized = this.Initialized;
      if (initialized == null)
        return;
      initialized((object) this, e);
    }
  }
}

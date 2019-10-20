// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Program
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Messaging;
using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ImageOrganizer
{
  public static class Program
  {
    private const ConsoleColor BackgroundColor = ConsoleColor.Black;
    private const ConsoleColor VerboseColor = ConsoleColor.Gray;
    private const ConsoleColor InformationColor = ConsoleColor.White;
    private const ConsoleColor WarningColor = ConsoleColor.Yellow;
    private const ConsoleColor ErrorColor = ConsoleColor.Red;
    private const ConsoleColor ProgressBarColor = ConsoleColor.Blue;
    private const ConsoleColor ProgressBarBackColor = ConsoleColor.Black;
    private const int FlagsPaneLeft = 45;
    private const int FlagsPaneTop = 2;
    private const int IoPaneTop = 7;
    private const int ProgressBarTop = 14;

    public static void Main(string[] args)
    {
      Program.SetWindowSize();
      Console.Title = Resources.Application_Title;
      Console.BackgroundColor = ConsoleColor.Black;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Clear();
      ImageOrganizerApplication organizerApplication = new ImageOrganizerApplication(args);
      organizerApplication.Initialized += new EventHandler<InitializationEventArgs>(Program.HandleApplicationInitialized);
      organizerApplication.MessageSent += new EventHandler<MessageSentEventArgs>(Program.HandleApplicationMessageSent);
      organizerApplication.UserInput += new EventHandler<UserInputEventArgs>(Program.HandleApplicationUserInput);
      organizerApplication.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(Program.HandleApplicationProgressChanged);
      organizerApplication.Completed += new EventHandler<CompletedEventArgs>(Program.HandleApplicationCompleted);
      organizerApplication.Start();
      organizerApplication.Initialized -= new EventHandler<InitializationEventArgs>(Program.HandleApplicationInitialized);
      organizerApplication.MessageSent -= new EventHandler<MessageSentEventArgs>(Program.HandleApplicationMessageSent);
      organizerApplication.UserInput -= new EventHandler<UserInputEventArgs>(Program.HandleApplicationUserInput);
      organizerApplication.ProgressChanged -= new EventHandler<ProgressChangedEventArgs>(Program.HandleApplicationProgressChanged);
      organizerApplication.Completed -= new EventHandler<CompletedEventArgs>(Program.HandleApplicationCompleted);
      Console.ReadKey();
      Console.ResetColor();
    }

    private static void HandleApplicationInitialized(
      object sender,
      InitializationEventArgs initializationEventArgs)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine(Resources.Application_Title);
      Console.WriteLine();
      Console.WriteLine(Resources.View_Operation, (object) initializationEventArgs.Operation);
      Program.DisplayFlagsPane(initializationEventArgs);
      Program.DisplayInputOutputDirectories(initializationEventArgs);
      Program.ShowProgressBar(0, 14);
      Program.ShowProgressBar(0, 15);
      Console.SetCursorPosition(0, 17);
      Console.WriteLine(new string('-', Console.WindowWidth));
      Console.SetCursorPosition(0, 18);
    }

    private static void DisplayFlagsPane(InitializationEventArgs initializationEventArgs)
    {
      Console.SetCursorPosition(45, 2);
      Dictionary<string, bool?> dictionary = new Dictionary<string, bool?>()
      {
        {
          Resources.View_ImageRotation,
          initializationEventArgs.ImageRotation
        },
        {
          Resources.View_Clean,
          initializationEventArgs.Clean
        },
        {
          Resources.View_Preview,
          new bool?(initializationEventArgs.Preview)
        },
        {
          Resources.View_Verbose,
          new bool?(initializationEventArgs.Verbose)
        }
      };
      int num1 = 0;
      foreach (KeyValuePair<string, bool?> keyValuePair in dictionary)
      {
        num1 = keyValuePair.Key.Length > num1 ? keyValuePair.Key.Length : num1;
        Console.WriteLine(keyValuePair.Key);
        Console.CursorLeft = 45;
      }
      int num2 = num1 + 1;
      Console.SetCursorPosition(45 + num2, 2);
      foreach (KeyValuePair<string, bool?> keyValuePair in dictionary)
      {
        Console.WriteLine(Resources.View_ItemValueString, keyValuePair.Value.HasValue ? (keyValuePair.Value.Value ? (object) Resources.View_True : (object) Resources.View_False) : (object) Resources.View_NotApplicable);
        Console.CursorLeft = 45 + num2;
      }
    }

    private static void DisplayInputOutputDirectories(
      InitializationEventArgs initializationEventArgs)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      int count = initializationEventArgs.InputDirectories.Count;
      if (count == 0)
        stringBuilder1.AppendLine(Resources.View_None);
      if (count == 1)
      {
        stringBuilder1.AppendLine(initializationEventArgs.InputDirectories[0]);
      }
      else
      {
        stringBuilder1.AppendLine();
        for (int index = 0; index < 2 && index < count; ++index)
          stringBuilder1.AppendLine(string.Format((IFormatProvider) CultureInfo.CurrentCulture, " > {0}", (object) initializationEventArgs.InputDirectories[index]));
        if (count >= 3)
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          string str;
          if (count != 3)
            str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.View_MoreInputDirectories, (object) (count - 2));
          else
            str = string.Format((IFormatProvider) CultureInfo.CurrentCulture, " > {0}", (object) initializationEventArgs.InputDirectories[2]);
          stringBuilder2.AppendLine(str);
        }
      }
      Console.SetCursorPosition(0, 7);
      Console.WriteLine(Resources.View_InputDirectory, (object) stringBuilder1);
      Console.WriteLine(Resources.View_OutputDirectory, (object) initializationEventArgs.OutputDirectory);
    }

    private static void HandleApplicationProgressChanged(
      object sender,
      ProgressChangedEventArgs progressChangedEventArgs)
    {
      Program.ShowProgressBar(progressChangedEventArgs.Progress, 14);
      Program.ShowProgressBar(progressChangedEventArgs.TotalProgress, 15);
    }

    private static void HandleApplicationCompleted(
      object sender,
      CompletedEventArgs completedEventArgs)
    {
      string str1 = new string('-', Console.WindowWidth);
      string str2 = new string('-', Resources.View_SummaryTitle.Length);
      Console.WriteLine();
      Console.WriteLine(str1);
      Console.WriteLine(Resources.View_SummaryTitle);
      Console.WriteLine(str2);
      int cursorTop = Console.CursorTop;
      int num1 = 0;
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) completedEventArgs.Summary)
      {
        num1 = keyValuePair.Key.Length > num1 ? keyValuePair.Key.Length : num1;
        Console.WriteLine(keyValuePair.Key);
      }
      int num2;
      Console.SetCursorPosition(num2 = num1 + 1, cursorTop);
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) completedEventArgs.Summary)
      {
        Console.WriteLine(Resources.View_ItemValueString, keyValuePair.Value);
        Console.CursorLeft = num2;
      }
    }

    private static void HandleApplicationUserInput(
      object sender,
      UserInputEventArgs userInputEventArgs)
    {
      Console.ForegroundColor = userInputEventArgs.UserInputType != UserInputType.Warning ? ConsoleColor.White : ConsoleColor.Yellow;
      Console.Write(userInputEventArgs.Message);
      userInputEventArgs.UserInput = Console.ReadLine();
    }

    private static void HandleApplicationMessageSent(
      object sender,
      MessageSentEventArgs messageSentEventArgs)
    {
      switch (messageSentEventArgs.MessageType)
      {
        case MessageType.Information:
          Console.ForegroundColor = ConsoleColor.White;
          break;
        case MessageType.Warning:
          Console.ForegroundColor = ConsoleColor.Yellow;
          break;
        case MessageType.Error:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        default:
          Console.ForegroundColor = ConsoleColor.Gray;
          break;
      }
      Console.WriteLine(messageSentEventArgs.Message);
    }

    private static void SetWindowSize()
    {
      int width = Console.LargestWindowWidth / 2;
      int height = Console.LargestWindowHeight * 3 / 4;
      Console.SetWindowSize(width, height);
      Console.SetBufferSize(width, 500);
    }

    private static void ShowProgressBar(int progress, int top)
    {
      int cursorTop = Console.CursorTop;
      int num1 = (int) ((double) Console.WindowWidth * 0.75);
      int left = (Console.WindowWidth - num1 - 10) / 2;
      Console.SetCursorPosition(left, top);
      Console.BackgroundColor = ConsoleColor.Black;
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write(Resources.View_ProgressBeginning);
      Console.CursorLeft = left + num1 + 2;
      Console.Write(Resources.View_ProgressEnd);
      float num2 = (float) num1 / 100f;
      Console.BackgroundColor = ConsoleColor.Blue;
      int num3 = left + 1;
      for (int index = 0; (double) index < (double) num2 * (double) progress + 0.5; ++index)
      {
        Console.CursorLeft = num3++;
        Console.Write(Resources.View_ProgressChunk);
      }
      Console.BackgroundColor = ConsoleColor.Black;
      for (int index = num3; index <= left + num1 + 1; ++index)
      {
        Console.CursorLeft = num3++;
        Console.Write(Resources.View_ProgressChunk);
      }
      Console.CursorLeft = left + num1 + 5;
      Console.ForegroundColor = ConsoleColor.White;
      Console.BackgroundColor = ConsoleColor.Black;
      Console.Write(Resources.View_Progress, (object) progress);
      Console.SetCursorPosition(0, cursorTop);
    }
  }
}

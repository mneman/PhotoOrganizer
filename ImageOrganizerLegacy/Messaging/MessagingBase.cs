// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Messaging.MessagingBase
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;

namespace ImageOrganizer.Messaging
{
  internal abstract class MessagingBase : IMessageSender, IProgressReporter
  {
    public event EventHandler<MessageSentEventArgs> MessageSent;

    public event EventHandler<UserInputEventArgs> UserInput;

    public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    public event EventHandler<CompletedEventArgs> Completed;

    protected static int CalculatePercentage(int count, int totalCount)
    {
      return (int) ((double) count * 100.0 / (double) totalCount);
    }

    protected void OnMessageSent(MessageSentEventArgs e)
    {
      EventHandler<MessageSentEventArgs> messageSent = this.MessageSent;
      if (messageSent == null)
        return;
      messageSent((object) this, e);
    }

    protected void OnUserInput(UserInputEventArgs e)
    {
      EventHandler<UserInputEventArgs> userInput = this.UserInput;
      if (userInput == null)
        return;
      userInput((object) this, e);
    }

    protected void OnProgressChanged(ProgressChangedEventArgs e)
    {
      EventHandler<ProgressChangedEventArgs> progressChanged = this.ProgressChanged;
      if (progressChanged == null)
        return;
      progressChanged((object) this, e);
    }

    protected void OnCompleted(CompletedEventArgs e)
    {
      EventHandler<CompletedEventArgs> completed = this.Completed;
      if (completed == null)
        return;
      completed((object) this, e);
    }
  }
}

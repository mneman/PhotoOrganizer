// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Messaging.UserInputEventArgs
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;

namespace ImageOrganizer.Messaging
{
  internal sealed class UserInputEventArgs : EventArgs
  {
    public UserInputEventArgs(string message, UserInputType userInputType)
    {
      this.Message = message;
      this.UserInputType = userInputType;
    }

    public string Message { get; private set; }

    public UserInputType UserInputType { get; private set; }

    public string UserInput { get; set; }
  }
}

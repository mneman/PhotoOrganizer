// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Messaging.MessageSentEventArgs
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;

namespace ImageOrganizer.Messaging
{
  internal sealed class MessageSentEventArgs : EventArgs
  {
    public MessageSentEventArgs(string message, MessageType messageType)
    {
      this.Message = message;
      this.MessageType = messageType;
    }

    public string Message { get; private set; }

    public MessageType MessageType { get; private set; }
  }
}

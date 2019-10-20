// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Messaging.ProgressChangedEventArgs
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;

namespace ImageOrganizer.Messaging
{
  internal sealed class ProgressChangedEventArgs : EventArgs
  {
    public ProgressChangedEventArgs(int progress, int totalProgress)
    {
      this.Progress = ProgressChangedEventArgs.Normalize(progress);
      this.TotalProgress = ProgressChangedEventArgs.Normalize(totalProgress);
    }

    public int Progress { get; private set; }

    public int TotalProgress { get; private set; }

    private static int Normalize(int value)
    {
      if (value < 0)
        return 0;
      if (value <= 100)
        return value;
      return 100;
    }
  }
}

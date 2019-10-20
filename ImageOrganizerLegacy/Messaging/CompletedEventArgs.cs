// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Messaging.CompletedEventArgs
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;
using System.Collections.Generic;

namespace ImageOrganizer.Messaging
{
  internal sealed class CompletedEventArgs : EventArgs
  {
    public CompletedEventArgs(
      IDictionary<string, object> summaryItems,
      string targetDirectory,
      string firstImage)
    {
      this.Summary = summaryItems ?? (IDictionary<string, object>) new Dictionary<string, object>();
      this.Directory = targetDirectory;
      this.FirstImage = firstImage;
    }

    public IDictionary<string, object> Summary { get; private set; }

    public string Directory { get; private set; }

    public string FirstImage { get; private set; }
  }
}

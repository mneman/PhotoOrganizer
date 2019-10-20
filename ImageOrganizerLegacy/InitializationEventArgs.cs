// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.InitializationEventArgs
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;
using System.Collections.Generic;

namespace ImageOrganizer
{
  internal sealed class InitializationEventArgs : EventArgs
  {
    public InitializationEventArgs(
      string operation,
      bool verbose,
      bool? imageRotation,
      bool? clean,
      bool preview,
      IList<string> inputDirectories,
      string outputDirectory,
      string fileNamePattern)
    {
      this.Operation = operation;
      this.Verbose = verbose;
      this.ImageRotation = imageRotation;
      this.Clean = clean;
      this.Preview = preview;
      this.InputDirectories = inputDirectories;
      this.OutputDirectory = outputDirectory;
      this.FileNamePattern = fileNamePattern;
    }

    public string Operation { get; private set; }

    public bool Verbose { get; private set; }

    public bool? ImageRotation { get; private set; }

    public bool? Clean { get; private set; }

    public bool Preview { get; private set; }

    public IList<string> InputDirectories { get; private set; }

    public string OutputDirectory { get; private set; }

    public string FileNamePattern { get; private set; }
  }
}

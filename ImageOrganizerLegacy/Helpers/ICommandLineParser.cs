// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.ICommandLineParser
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System.Collections.Generic;

namespace ImageOrganizer.Helpers
{
  internal interface ICommandLineParser
  {
    bool IsHelp { get; }

    bool IsSort { get; }

    bool IsMerge { get; }

    bool IsVerbose { get; }

    bool IsClean { get; }

    bool IsNoRotate { get; }

    bool IsNoPreview { get; }

    string OutputDirectory { get; }

    string SortDirectory { get; }

    IEnumerable<string> MergeDirectories { get; }

    string NamePattern { get; }
  }
}

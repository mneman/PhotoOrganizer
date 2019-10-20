// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.CommandLineParser
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ImageOrganizer.Helpers
{
  internal sealed class CommandLineParser : ICommandLineParser
  {
    private readonly char[] argumentPrefixes = new char[3]
    {
      '-',
      '/',
      '\\'
    };
    private readonly string[] helpStrings = new string[3]
    {
      "h",
      "?",
      "help"
    };
    private readonly string[] sortStrings = new string[2]
    {
      "s",
      "sort"
    };
    private readonly string[] mergeStrings = new string[2]
    {
      "m",
      "merge"
    };
    private readonly string[] outputStrings = new string[2]
    {
      "o",
      "output"
    };
    private readonly string[] noRotateStrings = new string[4]
    {
      "no_rotate",
      "norotate",
      "no-rotate",
      "nr"
    };
    private readonly string[] verboseStrings = new string[2]
    {
      "v",
      "verbose"
    };
    private readonly string[] cleanStrings = new string[2]
    {
      "c",
      "clean"
    };
    private readonly string[] noPreviewStrings = new string[4]
    {
      "no_preview",
      "nopreview",
      "no-preview",
      "np"
    };
    private readonly string[] namePatternStrings = new string[5]
    {
      "name_pattern",
      "name-pattern",
      "namepattern",
      "pattern",
      "p"
    };
    private readonly string[] commandLineArgs;
    private bool? cachedHelp;
    private bool? cachedSort;
    private bool? cachedMerge;
    private bool? cachedVerbose;
    private bool? cachedClean;
    private bool? cachedNoPreview;
    private bool? cachedNoRotate;
    private string cachedSortDirectory;
    private IEnumerable<string> cachedMergeDirectories;
    private string cachedOutputDirectory;
    private string cachedNamePattern;

    public CommandLineParser(string[] commandLineArgs)
    {
      this.commandLineArgs = commandLineArgs ?? new string[0];
    }

    public bool IsHelp
    {
      get
      {
        if (!this.cachedHelp.HasValue)
          this.cachedHelp = new bool?(this.FindArgument((IEnumerable<string>) this.helpStrings));
        return this.cachedHelp.Value;
      }
    }

    public bool IsSort
    {
      get
      {
        if (!this.cachedSort.HasValue)
          this.cachedSort = new bool?(this.FindArgument((IEnumerable<string>) this.sortStrings));
        return this.cachedSort.Value;
      }
    }

    public bool IsMerge
    {
      get
      {
        if (!this.cachedMerge.HasValue)
          this.cachedMerge = new bool?(this.FindArgument((IEnumerable<string>) this.mergeStrings));
        return this.cachedMerge.Value;
      }
    }

    public bool IsVerbose
    {
      get
      {
        if (!this.cachedVerbose.HasValue)
          this.cachedVerbose = new bool?(this.FindArgument((IEnumerable<string>) this.verboseStrings));
        return this.cachedVerbose.Value;
      }
    }

    public bool IsClean
    {
      get
      {
        if (!this.cachedClean.HasValue)
          this.cachedClean = new bool?(this.FindArgument((IEnumerable<string>) this.cleanStrings));
        return this.cachedClean.Value;
      }
    }

    public bool IsNoRotate
    {
      get
      {
        if (!this.cachedNoRotate.HasValue)
          this.cachedNoRotate = new bool?(this.FindArgument((IEnumerable<string>) this.noRotateStrings));
        return this.cachedNoRotate.Value;
      }
    }

    public bool IsNoPreview
    {
      get
      {
        if (!this.cachedNoPreview.HasValue)
          this.cachedNoPreview = new bool?(this.FindArgument((IEnumerable<string>) this.noPreviewStrings));
        return this.cachedNoPreview.Value;
      }
    }

    public string OutputDirectory
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.cachedOutputDirectory))
          this.cachedOutputDirectory = this.GetArgumentValue((IEnumerable<string>) this.outputStrings).FirstOrDefault<string>();
        return this.cachedOutputDirectory;
      }
    }

    public string SortDirectory
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.cachedSortDirectory))
          this.cachedSortDirectory = this.GetArgumentValue((IEnumerable<string>) this.sortStrings).FirstOrDefault<string>();
        return this.cachedSortDirectory;
      }
    }

    public IEnumerable<string> MergeDirectories
    {
      get
      {
        return this.cachedMergeDirectories ?? (this.cachedMergeDirectories = this.GetArgumentValue((IEnumerable<string>) this.mergeStrings));
      }
    }

    public string NamePattern
    {
      get
      {
        if (string.IsNullOrEmpty(this.cachedNamePattern))
          this.cachedNamePattern = this.GetArgumentValue((IEnumerable<string>) this.namePatternStrings).FirstOrDefault<string>();
        return this.cachedNamePattern;
      }
    }

    private bool FindArgument(IEnumerable<string> argumentStrings)
    {
      return ((IEnumerable<string>) this.commandLineArgs).Where<string>((Func<string, bool>) (arg =>
      {
        if (((IEnumerable<char>) this.argumentPrefixes).Any<char>((Func<char, bool>) (prefix => (int) arg[0] == (int) prefix)))
          return argumentStrings.Contains<string>(arg.Trim(this.argumentPrefixes).ToLower(CultureInfo.CurrentCulture));
        return false;
      })).Any<string>();
    }

    private IEnumerable<string> GetArgumentValue(IEnumerable<string> argumentStrings)
    {
      return ((IEnumerable<string>) this.commandLineArgs).SkipWhile<string>((Func<string, bool>) (arg =>
      {
        if (!((IEnumerable<char>) this.argumentPrefixes).All<char>((Func<char, bool>) (prefix => (int) arg[0] != (int) prefix)))
          return !argumentStrings.Contains<string>(arg.Trim(this.argumentPrefixes).ToLower(CultureInfo.CurrentCulture));
        return true;
      })).Skip<string>(1).TakeWhile<string>((Func<string, bool>) (arg => !((IEnumerable<char>) this.argumentPrefixes).Any<char>((Func<char, bool>) (prefix => (int) arg[0] == (int) prefix))));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.ISystemContext
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ImageOrganizer.Helpers
{
  internal interface ISystemContext
  {
    bool FileExists(string path);

    void MoveFile(string source, string destination);

    void DeleteFile(string path);

    void SetFileCreationTime(string path, DateTime creationTime);

    void SetFileLastAccessTime(string path, DateTime accessTime);

    void SetFileLastWriteTime(string path, DateTime writeTime);

    DateTime GetFileCreationTime(string path);

    bool DirectoryExists(string path);

    void CreateDirectory(string path);

    void DeleteDirectory(string path, bool recursive);

    IEnumerable<string> DirectoryEntries(string path);

    ICollection<string> GetFiles(
      string directory,
      string searchPattern,
      SearchOption searchOption);

    string GetCurrentWorkingDirectory();

    void StartProcess(ProcessStartInfo startInfo);
  }
}

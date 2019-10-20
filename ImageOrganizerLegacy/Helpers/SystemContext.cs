// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.SystemContext
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ImageOrganizer.Helpers
{
  internal sealed class SystemContext : ISystemContext
  {
    public bool FileExists(string path)
    {
      return File.Exists(path);
    }

    public void MoveFile(string source, string destination)
    {
      File.Move(source, destination);
    }

    public void DeleteFile(string path)
    {
      File.Delete(path);
    }

    public void SetFileCreationTime(string path, DateTime creationTime)
    {
      File.SetCreationTime(path, creationTime);
    }

    public void SetFileLastWriteTime(string path, DateTime writeTime)
    {
      File.SetLastWriteTime(path, writeTime);
    }

    public DateTime GetFileCreationTime(string path)
    {
      return File.GetCreationTime(path);
    }

    public void SetFileLastAccessTime(string path, DateTime accessTime)
    {
      File.SetLastWriteTime(path, accessTime);
    }

    public bool DirectoryExists(string path)
    {
      return Directory.Exists(path);
    }

    public void CreateDirectory(string path)
    {
      Directory.CreateDirectory(path);
    }

    public void DeleteDirectory(string path, bool recursive)
    {
      Directory.Delete(path, recursive);
    }

    public IEnumerable<string> DirectoryEntries(string path)
    {
      return Directory.EnumerateFileSystemEntries(path);
    }

    public ICollection<string> GetFiles(
      string directory,
      string searchPattern,
      SearchOption searchOption)
    {
      return (ICollection<string>) Directory.GetFiles(directory, searchPattern, searchOption);
    }

    public string GetCurrentWorkingDirectory()
    {
      return Environment.CurrentDirectory;
    }

    public void StartProcess(ProcessStartInfo startInfo)
    {
      new Process() { StartInfo = startInfo }.Start();
    }
  }
}

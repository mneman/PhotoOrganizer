// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Imaging.IImageWrapper
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System;
using System.Drawing.Imaging;

namespace ImageOrganizer.Imaging
{
  internal interface IImageWrapper : IDisposable
  {
    ImageOrientation Orientation { get; set; }

    DateTime DateTimeTaken { get; }

    void Save(string targetFile, ImageCodecInfo codec, EncoderParameters parameters);
  }
}

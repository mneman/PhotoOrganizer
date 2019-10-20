// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Imaging.IImageRotator
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Messaging;
using System.Drawing.Imaging;

namespace ImageOrganizer.Imaging
{
  internal interface IImageRotator : IMessageSender
  {
    EncoderParameters Rotate(IImageWrapper image);
  }
}

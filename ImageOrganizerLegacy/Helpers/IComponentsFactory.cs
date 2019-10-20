// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.IComponentsFactory
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Imaging;

namespace ImageOrganizer.Helpers
{
  internal interface IComponentsFactory
  {
    IImageRotator CreateImageRotator();

    IImageWrapper CreateImageWrapper(string imagePath);

    IDirectorySorter CreateDirectorySorter(ISystemContext systemContext);
  }
}

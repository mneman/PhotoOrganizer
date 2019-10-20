// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Manipulators.ImageManipulatorsFactory
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Helpers;

namespace ImageOrganizer.Manipulators
{
  internal sealed class ImageManipulatorsFactory : IImageManipulatorsFactory
  {
    public IImageSorter CreateImageSorter(
      IComponentsFactory componentsFactory,
      ISystemContext systemContext)
    {
      return (IImageSorter) new ImageSorter(componentsFactory, systemContext);
    }

    public IImageDirectoryMerger CreateImageDirectoryMerger(
      IComponentsFactory componentsFactory,
      ISystemContext systemContext)
    {
      return (IImageDirectoryMerger) new ImageDirectoryMerger(componentsFactory, systemContext);
    }
  }
}

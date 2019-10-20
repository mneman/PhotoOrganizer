// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Helpers.ComponentsFactory
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Imaging;
using System.Drawing;

namespace ImageOrganizer.Helpers
{
  internal sealed class ComponentsFactory : IComponentsFactory
  {
    public IImageRotator CreateImageRotator()
    {
      return (IImageRotator) new ImageRotator();
    }

    public IImageWrapper CreateImageWrapper(string imagePath)
    {
      return (IImageWrapper) new ImageWrapper(Image.FromFile(imagePath));
    }

    public IDirectorySorter CreateDirectorySorter(ISystemContext systemContext)
    {
      return (IDirectorySorter) new DirectorySorter(systemContext);
    }
  }
}

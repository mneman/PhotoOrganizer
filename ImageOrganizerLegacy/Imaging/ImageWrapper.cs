// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Imaging.ImageWrapper
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ImageOrganizer.Imaging
{
  internal sealed class ImageWrapper : IImageWrapper, IDisposable
  {
    private const int PropertyTagOrientation = 274;
    private const int PropertyTagDateTime = 306;
    private const int PropertyTagDateTimeOriginal = 36867;
    private const int PropertyTagDateTimeDigitized = 36868;
    private readonly Image internalImage;
    private DateTime? cachedDateTimeTaken;
    private ImageOrientation? cachedImageOrientation;

    public ImageWrapper(Image image)
    {
      this.internalImage = image;
    }

    public ImageOrientation Orientation
    {
      get
      {
        if (!this.cachedImageOrientation.HasValue)
        {
          PropertyItem property = ImageWrapper.GetProperty((IEnumerable<PropertyItem>) this.internalImage.PropertyItems, 274);
          ImageOrientation imageOrientation = ImageOrientation.TopLeft;
          if (property != null)
            imageOrientation = ImageWrapper.ConvertToOrientation(property.Value);
          this.cachedImageOrientation = new ImageOrientation?(imageOrientation);
        }
        return this.cachedImageOrientation.Value;
      }
      set
      {
        PropertyItem property = ImageWrapper.GetProperty((IEnumerable<PropertyItem>) this.internalImage.PropertyItems, 274);
        if (property != null)
        {
          byte[] bytes = BitConverter.GetBytes((short) value);
          property.Value = bytes;
          this.internalImage.SetPropertyItem(property);
        }
        this.cachedImageOrientation = new ImageOrientation?();
      }
    }

    public DateTime DateTimeTaken
    {
      get
      {
        if (!this.cachedDateTimeTaken.HasValue)
        {
          DateTime dateTime = DateTime.Now;
          PropertyItem propertyItem = (ImageWrapper.GetProperty((IEnumerable<PropertyItem>) this.internalImage.PropertyItems, 36867) ?? ImageWrapper.GetProperty((IEnumerable<PropertyItem>) this.internalImage.PropertyItems, 36868)) ?? ImageWrapper.GetProperty((IEnumerable<PropertyItem>) this.internalImage.PropertyItems, 306);
          if (propertyItem != null)
            dateTime = ImageWrapper.ConvertToDateTime(propertyItem.Value);
          this.cachedDateTimeTaken = new DateTime?(dateTime);
        }
        return this.cachedDateTimeTaken.Value;
      }
    }

    public void Save(string targetFile, ImageCodecInfo codec, EncoderParameters parameters)
    {
      this.internalImage.Save(targetFile, codec, parameters);
    }

    public void Dispose()
    {
      if (this.internalImage == null)
        return;
      this.internalImage.Dispose();
    }

    private static PropertyItem GetProperty(
      IEnumerable<PropertyItem> imageProperties,
      int propertyId)
    {
      if (imageProperties != null)
        return imageProperties.Where<PropertyItem>((Func<PropertyItem, bool>) (p => p.Id == propertyId)).FirstOrDefault<PropertyItem>();
      return (PropertyItem) null;
    }

    private static ImageOrientation ConvertToOrientation(byte[] bytes)
    {
      if (bytes.Length != 2)
        throw new ArgumentException(Resources.Warning_FailedToRetrieveOrientation);
      return (ImageOrientation) BitConverter.ToInt16(bytes, 0);
    }

    private static DateTime ConvertToDateTime(byte[] bytes)
    {
      if (bytes.Length != 20)
        throw new ArgumentException(Resources.Warning_FailedToRetrieveCreationDate);
      return DateTime.ParseExact(Encoding.ASCII.GetString(bytes, 0, bytes.Length - 1), "yyyy:MM:d H:m:s", (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Imaging.ImageRotator
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using ImageOrganizer.Messaging;
using ImageOrganizer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;

namespace ImageOrganizer.Imaging
{
  internal sealed class ImageRotator : MessagingBase, IImageRotator, IMessageSender
  {
    private static readonly IDictionary<ImageOrientation, List<EncoderValue>> RotationMap = (IDictionary<ImageOrientation, List<EncoderValue>>) new Dictionary<ImageOrientation, List<EncoderValue>>()
    {
      {
        ImageOrientation.TopLeft,
        new List<EncoderValue>()
      },
      {
        ImageOrientation.TopRight,
        new List<EncoderValue>()
        {
          EncoderValue.TransformFlipHorizontal
        }
      },
      {
        ImageOrientation.BottomRight,
        new List<EncoderValue>() { EncoderValue.TransformRotate180 }
      },
      {
        ImageOrientation.BottomLeft,
        new List<EncoderValue>()
        {
          EncoderValue.TransformFlipVertical
        }
      },
      {
        ImageOrientation.LeftTop,
        new List<EncoderValue>()
        {
          EncoderValue.TransformRotate90,
          EncoderValue.TransformFlipHorizontal
        }
      },
      {
        ImageOrientation.RightTop,
        new List<EncoderValue>() { EncoderValue.TransformRotate90 }
      },
      {
        ImageOrientation.RightBottom,
        new List<EncoderValue>()
        {
          EncoderValue.TransformRotate90,
          EncoderValue.TransformFlipVertical
        }
      },
      {
        ImageOrientation.LeftBottom,
        new List<EncoderValue>() { EncoderValue.TransformRotate270 }
      }
    };

    public EncoderParameters Rotate(IImageWrapper image)
    {
      ImageOrientation orientation = image.Orientation;
      if (orientation == ImageOrientation.TopLeft)
        return (EncoderParameters) null;
      List<EncoderValue> rotation = ImageRotator.RotationMap[orientation];
      EncoderParameters encoderParameters = new EncoderParameters(rotation.Count);
      Encoder transformation = Encoder.Transformation;
      for (int index = 0; index < rotation.Count; ++index)
        encoderParameters.Param[index] = new EncoderParameter(transformation, (long) rotation[index]);
      image.Orientation = ImageOrientation.TopLeft;
      this.OnMessageSent(new MessageSentEventArgs(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.Info_ImageRotated, (object) orientation), MessageType.Verbose));
      return encoderParameters;
    }
  }
}

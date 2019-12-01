using System;
using System.Collections.Generic;
using System.Composition;
using System.Drawing.Imaging;
using PhotoOrganizer.Core.Imaging;

namespace PhotoOrganizer.Core.Manipulators
{
    /// <summary>
    /// Image rotation implementation.
    /// </summary>
    [Export(typeof(IImageRotator))]
    public sealed class ImageRotator : IImageRotator
    {
        /// <summary>
        /// The instance of <see cref="IImageFactory"/>.
        /// </summary>
        private readonly IImageFactory imageFactory;

        private readonly IDictionary<ImageOrientation, IList<EncoderValue>> RotationMap = new Dictionary<ImageOrientation, IList<EncoderValue>>()
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
                new List<EncoderValue>()
                {
                    EncoderValue.TransformRotate180
                }
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
                new List<EncoderValue>()
                {
                    EncoderValue.TransformRotate90
                }
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
                new List<EncoderValue>()
                {
                    EncoderValue.TransformRotate270
                }
            }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRotator"/> class.
        /// </summary>
        /// <param name="imageFactory">The instance of <see cref="IImageFactory"/>.</param>
        [ImportingConstructor]
        public ImageRotator(IImageFactory imageFactory)
        {
            this.imageFactory = imageFactory;
        }

        /// <summary>
        /// Gets image transformation <see cref="EncoderParameters"/> for the image rotation.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns>An array of <see cref="EncoderParameters"/> for the rotation of the image.</returns>
        public EncoderParameters GetRotationParameters(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentException(nameof(imagePath));
            }

            var image = this.imageFactory.OpenImage(imagePath);
            return this.GetRotationParametersInternal(image);
        }

        /// <summary>
        /// Gets image transformation <see cref="EncoderParameters" /> for the image rotation.
        /// </summary>
        /// <param name="image">The <see cref="IImage" /> instace.</param>
        /// <returns>
        /// An array of <see cref="EncoderParameters" /> for the rotation of the image.
        /// </returns>
        public EncoderParameters GetRotationParameters(IImage image)
        {
            if (image == null)
            {
                throw new ArgumentException(nameof(image));
            }

            return this.GetRotationParametersInternal(image);
        }

        /// <summary>
        /// Gets the rotation parameters.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>An array of <see cref="EncoderParameters" /> for the rotation of the image.</returns>
        private EncoderParameters GetRotationParametersInternal(IImage image)
        {
            var orientation = image.Orientation;

            if (orientation == ImageOrientation.TopLeft)
            {
                return null;
            }

            IList<EncoderValue> rotation = this.RotationMap[orientation];
            EncoderParameters encoderParameters = new EncoderParameters(rotation.Count);
            Encoder transformation = Encoder.Transformation;
            for (int index = 0; index < rotation.Count; ++index)
                encoderParameters.Param[index] = new EncoderParameter(transformation, (long)rotation[index]);
            //image.Orientation = ImageOrientation.TopLeft;
            return encoderParameters;
        }
    }
}

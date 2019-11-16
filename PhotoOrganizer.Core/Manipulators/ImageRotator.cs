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

        /// <summary>
        /// The instance of <see cref="IImageMetadataParser"/>.
        /// </summary>
        private readonly IImageMetadataParser imageMetadataParser;

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
        /// <param name="imageMetadataParser">The instance of <see cref="IImageMetadataParser"/>.</param>
        [ImportingConstructor]
        public ImageRotator(IImageFactory imageFactory, IImageMetadataParser imageMetadataParser)
        {
            this.imageFactory = imageFactory;
            this.imageMetadataParser = imageMetadataParser;
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
            var orientation = this.imageMetadataParser.GetOrientation(image);

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

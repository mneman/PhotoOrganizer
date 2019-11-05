using System;
using System.Composition;
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
        /// The image metadata parser
        /// </summary>
        private readonly IImageMetadataParser imageMetadataParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRotator"/> class.
        /// </summary>
        /// <param name="imageMetadataParser">The image metadata parser.</param>
        [ImportingConstructor]
        public ImageRotator(IImageMetadataParser imageMetadataParser)
        {
            this.imageMetadataParser = imageMetadataParser;
        }

        /// <summary>
        /// Rotates the image.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Rotate(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentException(nameof(imagePath));
            }

            throw new System.NotImplementedException();
        }
    }
}

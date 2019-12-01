using System;
using System.Composition;
using System.Drawing;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Factory for the <see cref="IImage"/> instances.
    /// </summary>
    /// <seealso cref="PhotoOrganizer.Core.Imaging.IImageFactory" />
    [Export(typeof(IImageFactory))]
    internal sealed class ImageFactory : IImageFactory
    {
        private readonly IImageMetadataConverter imageMetadataParser;

        [ImportingConstructor]
        public ImageFactory(IImageMetadataConverter imageMetadataParser)
        {
            this.imageMetadataParser = imageMetadataParser;
        }

        /// <summary>
        /// Opens an image and wraps it in an <see cref="IImage" /> object.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns>
        /// An instance of <see cref="IImage" /> class.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public IImage OpenImage(string imagePath)
        {
            return new ImageWrapper(Image.FromFile(imagePath), this.imageMetadataParser);
        }
    }
}

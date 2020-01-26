using System;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Factory for the <see cref="IImage"/> instances.
    /// </summary>
    /// <seealso cref="PhotoOrganizer.Core.Imaging.IImageFactory" />
    [Export(typeof(IImageFactory))]
    [ExcludeFromCodeCoverage]
    internal sealed class ImageFactory : IImageFactory
    {
        /// <summary>
        /// The <see cref="IImageMetadataConverter"/> instance.
        /// </summary>
        private readonly IImageMetadataConverter imageMetadataConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFactory"/> class.
        /// </summary>
        /// <param name="imageMetadataParser">The image metadata parser.</param>
        [ImportingConstructor]
        public ImageFactory(IImageMetadataConverter imageMetadataParser)
        {
            this.imageMetadataConverter = imageMetadataParser;
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
            return new ImageWrapper(Image.FromFile(imagePath), this.imageMetadataConverter);
        }
    }
}

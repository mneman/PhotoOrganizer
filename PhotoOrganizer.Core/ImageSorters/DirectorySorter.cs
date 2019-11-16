using System;
using System.Composition;
using System.Drawing.Imaging;
using System.IO;
using PhotoOrganizer.Core.Imaging;
using PhotoOrganizer.Core.Manipulators;
using PhotoOrganizer.Core.Utilities;

namespace PhotoOrganizer.Core.ImageSorters
{
    /// <summary>
    /// Image directory sorter default implementation.
    /// </summary>
    [Export(typeof(IDirectorySorter))]
    internal sealed class DirectorySorter : IDirectorySorter
    {
        /// <summary>
        /// The instance of <see cref="Options"/>.
        /// </summary>
        private readonly Options options;

        /// <summary>
        /// The instance of <see cref="IImageRotator"/>.
        /// </summary>
        private readonly IImageRotator imageRotator;

        /// <summary>
        /// The instance of <see cref="IImageFactory"/>.
        /// </summary>
        private readonly IImageFactory imageFactory;

        /// <summary>
        /// The instance of <see cref="IImageMetadataParser"/>.
        /// </summary>
        private readonly IImageMetadataParser imageMetadataParser;

        /// <summary>
        /// The instance of the <see cref="IFileSystem"/>.
        /// </summary>
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of <see cref="DirectorySorter"/>.
        /// </summary>
        /// <param name="options">The instance of <see cref="Options"/>.</param>
        /// <param name="imageRotator">The instance of <see cref="IImageRotator"/>.</param>
        /// <param name="imageFactory">The instance of <see cref="IImageFactory"/>.</param>
        /// <param name="imageMetadataParser">The instance of <see cref="IImageMetadataParser"/>.</param>
        /// <param name="fileSystem">The instance of <see cref="IFileSystem"/>.</param>
        [ImportingConstructor]
        public DirectorySorter(Options options, IImageRotator imageRotator, IImageFactory imageFactory, IImageMetadataParser imageMetadataParser, IFileSystem fileSystem)
        {
            this.options = options;
            this.imageRotator = imageRotator;
            this.imageFactory = imageFactory;
            this.imageMetadataParser = imageMetadataParser;
            this.fileSystem = fileSystem;
        }
        /// <summary>
        /// Sorts the images inside a directory.
        /// </summary>
        /// <param name="path">The directory to sort.</param>
        public void SortDirectory(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(nameof(path));
            }

            if (!this.fileSystem.DirectoryExists(path))
            {
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            }

            foreach (string file in this.fileSystem.EnumerateFiles(path, "*.jpg"))
            {
                IImage image = this.imageFactory.OpenImage(file);
                this.ProcessImage(image);
            }
        }

        /// <summary>
        /// Processes an image.
        /// </summary>
        /// <param name="image">The image to process.</param>
        private void ProcessImage(IImage image)
        {
            EncoderParameters rotationParameters = null;
            if (this.options.Rotate)
            {
                rotationParameters = this.imageRotator.GetRotationParameters(image);

                if (rotationParameters != null)
                {
                    byte[] bytes = BitConverter.GetBytes((short)ImageOrientation.TopLeft);
                    image.SetMetadata(ImageMetadataType.Orientation, bytes);
                }
            }
        }
    }
}

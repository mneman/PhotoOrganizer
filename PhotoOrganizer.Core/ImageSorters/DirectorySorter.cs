using System;
using System.Composition;
using System.Drawing.Imaging;
using System.Globalization;
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
        /// The instance of <see cref="IImageRotator"/>.
        /// </summary>
        private readonly IImageRotator imageRotator;

        /// <summary>
        /// The instance of <see cref="IImageFactory"/>.
        /// </summary>
        private readonly IImageFactory imageFactory;

        /// <summary>
        /// The instance of the <see cref="IFileSystem"/>.
        /// </summary>
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of <see cref="DirectorySorter"/>.
        /// </summary>
        /// <param name="imageRotator">The instance of <see cref="IImageRotator"/>.</param>
        /// <param name="imageFactory">The instance of <see cref="IImageFactory"/>.</param>
        /// <param name="fileSystem">The instance of <see cref="IFileSystem"/>.</param>
        [ImportingConstructor]
        public DirectorySorter(IImageRotator imageRotator, IImageFactory imageFactory, IFileSystem fileSystem)
        {
            this.imageRotator = imageRotator;
            this.imageFactory = imageFactory;
            this.fileSystem = fileSystem;
        }
        /// <summary>
        /// Sorts the images inside a directory.
        /// </summary>
        /// <param name="path">The directory to sort.</param>
        /// <param name="options">The <see cref="Options"/> for processing.</param>
        public void SortDirectory(string path, Options options)
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
                this.ProcessImage(file, options);
            }
        }

        /// <summary>
        /// Processes an image.
        /// </summary>
        /// <param name="file">The image file to process.</param>
        /// <param name="options">The options.</param>
        private void ProcessImage(string file, Options options)
        {
            IImage image = this.imageFactory.OpenImage(file);

            EncoderParameters rotationParameters = null;
            if (options.Rotate)
            {
                rotationParameters = this.RotateImage(image);
            }

            var tempPath = this.GetTempPath(image, Path.GetExtension(file), options.OutputDirectory);
        }

        private EncoderParameters RotateImage(IImage image)
        {
            EncoderParameters rotationParameters = this.imageRotator.GetRotationParameters(image);

            if (rotationParameters != null)
            {
                image.Orientation = ImageOrientation.TopLeft;
                //byte[] bytes = BitConverter.GetBytes((short)ImageOrientation.TopLeft);
                //image.SetMetadata(ImageMetadataType.Orientation, bytes);
            }

            return rotationParameters;
        }

        private string GetTempPath(IImage image, string extension, string outputDirectory)
        {
            DateTime? imageDate = image.DateTimeOriginal
                                  ?? image.DateTimeDigitized
                                  ?? image.DateTimeTaken
                                  ?? DateTime.Today;

            string subDirectory = imageDate.Value.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);

            return Path.Join(outputDirectory, subDirectory, Guid.NewGuid().ToString(), extension);
        }
    }
}

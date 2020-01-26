using System;
using System.Collections.Generic;
using System.Composition;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using PhotoOrganizer.Core.Imaging;
using PhotoOrganizer.Core.Manipulators;
using PhotoOrganizer.Core.Utilities;

namespace PhotoOrganizer.Core
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

            IList<string> datedDirectoriesWithTempFiles = this.GetDatedDirectories(path, options).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();

            foreach (string folder in datedDirectoriesWithTempFiles)
            {
                throw new NotImplementedException();
            }
        }

        private IEnumerable<string> GetDatedDirectories(string path, Options options)
        {
            var imageFiles = options.ImageExtensions.SelectMany(ext => this.fileSystem.EnumerateFiles(path, ext));
            foreach (string file in imageFiles)
            {
                yield return this.ProcessImage(file, options);
            }
        }

        /// <summary>
        /// Processes an image.
        /// </summary>
        /// <param name="file">The image file to process.</param>
        /// <param name="options">The options.</param>
        private string ProcessImage(string file, Options options)
        {
            IImage image = this.imageFactory.OpenImage(file);

            EncoderParameters rotationParameters = null;
            if (options.Rotate)
            {
                rotationParameters = this.RotateImage(image);
            }

            return this.SaveToDatedDirectory(image, Path.GetExtension(file), options.OutputDirectory, rotationParameters);
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

        private string SaveToDatedDirectory(IImage image, string extension, string outputDirectory, EncoderParameters parameters)
        {
            DateTime? imageDate = image.DateTimeOriginal
                                  ?? image.DateTimeDigitized
                                  ?? image.DateTimeTaken
                                  ?? DateTime.Today;

            string subDirectory = Path.Join(outputDirectory, imageDate.Value.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture));
            string tempFileName = $"{Guid.NewGuid()}.{extension}";

            this.fileSystem.CreateDirectorySafe(subDirectory);

            var targetFile = Path.Join(subDirectory, tempFileName);

            image.Save(targetFile, image.CodecInfo, parameters);

            return subDirectory;
        }
    }
}

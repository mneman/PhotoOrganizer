using System;
using System.Drawing.Imaging;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Abstraction of the <see cref="System.Drawing.Image"/>. 
    /// </summary>
    public interface IImage : IDisposable
    {
        /// <summary>
        /// Gets or sets the image orientation.
        /// </summary>
        ImageOrientation Orientation { get; set; }

        /// <summary>
        /// Gets the date time the image was taken at. 
        /// </summary>
        DateTime? DateTimeTaken { get; }

        /// <summary>
        /// Gets the date time when the image was originally created at.
        /// </summary>
        DateTime? DateTimeOriginal { get; }

        /// <summary>
        /// Gets the date time the image was digitized at.
        /// </summary>
        DateTime? DateTimeDigitized { get; }

        /// <summary>
        /// Gets the MIME type of the image.
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Saves this <see cref="System.Drawing.Image"/> to the specified file, with the specified encoder and image-encoder parameters.
        /// </summary>
        /// <param name="targetFile">A string that contains the name of the file to which to save this System.Drawing.Image.</param>
        /// <param name="codec">The System.Drawing.Imaging.ImageCodecInfo for this System.Drawing.Image.</param>
        /// <param name="parameters">An System.Drawing.Imaging.EncoderParameters to use for this System.Drawing.Image.</param>
        void Save(string targetFile, ImageCodecInfo codec, EncoderParameters parameters);
    }
}

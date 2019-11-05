using System;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Retrieves the orientation of a <see cref="IImage"/>.
    /// </summary>
    public interface IImageMetadataParser
    {
        /// <summary>
        /// Gets the orientation of a <see cref="IImage"/>.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>One of the <see cref="ImageOrientation"/> values.</returns>
        ImageOrientation GetOrientation(IImage image);

        /// <summary>
        /// Gets the date time the image was taken.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>Date and time the image was taken.</returns>
        DateTime? GetDateTimeTaken(IImage image);

        /// <summary>
        /// Gets the date time when the image was originally created.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>Date and time the image was originally take.</returns>
        DateTime? GetDateTimeOriginal(IImage image);

        /// <summary>
        /// Gets the date time the image was digitized.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>Date and time the image was digitized.</returns>
        DateTime? GetDateTimeDigitized(IImage image);
    }
}

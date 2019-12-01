using System;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Retrieves the orientation of a <see cref="IImage"/>.
    /// </summary>
    internal interface IImageMetadataConverter
    {
        /// <summary>
        /// Gets the <see cref="ImageOrientation"/> based on the bytes array.
        /// </summary>
        /// <param name="metadataBytes">The image orientation metadata.</param>
        /// <returns>
        /// One of the <see cref="ImageOrientation" /> values.
        /// </returns>
        ImageOrientation ToOrientation(byte[] metadataBytes);

        /// <summary>
        /// Gets the date time the image was taken at.
        /// </summary>
        /// <param name="metadataBytes">The image metadata.</param>
        /// <returns>
        /// Date and time the image was taken at.
        /// </returns>
        DateTime? ToDateTimeTaken(byte[] metadataBytes);

        /// <summary>
        /// Gets the date time the image was digitized at.
        /// </summary>
        /// <param name="metadataBytes">The image metadata.</param>
        /// <returns>
        /// Date and time the image was digitized. at
        /// </returns>
        DateTime? ToDateTimeOriginal(byte[] metadataBytes);

        /// <summary>
        /// Gets the date time when the image was originally created at.
        /// </summary>
        /// <param name="metadataBytes">The image metadata.</param>
        /// <returns>
        /// Date and time the image was originally taken at.
        /// </returns>
        DateTime? ToDateTimeDigitized(byte[] metadataBytes);

        /// <summary>
        /// Converts <see cref="ImageOrientation"/> to metadata bytes.
        /// </summary>
        /// <param name="orientation">The <see cref="ImageOrientation"/> to convert.</param>
        /// <returns>Image orientation metadata byte array.</returns>
        byte[] ToBytes(ImageOrientation orientation);

        /// <summary>
        /// Converts <see cref="DateTime"/> to metadata bytes.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
        /// <returns>Image date-time metadat byte array.</returns>
        byte[] ToBytes(DateTime dateTime);
    }
}

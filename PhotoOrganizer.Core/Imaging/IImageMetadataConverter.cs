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
        /// Gets a <see cref="DateTime"/> image metadata value.
        /// </summary>
        /// <param name="metadataBytes">The <see cref="IImage"/> instance.</param>
        /// <returns>
        /// The date-time instance.
        /// </returns>
        DateTime? ToDateTime(byte[] metadataBytes);

        /// <summary>
        /// Gets a mime-type based on the <see cref="Guid"/> representing image raw format.
        /// </summary>
        /// <param name="rawFormat">The raw format <see cref="Guid"/>.</param>
        /// <returns>Image mime-type.</returns>
        string ToMimeType(Guid rawFormat);

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

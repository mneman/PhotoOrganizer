using System;
using System.Globalization;
using System.Text;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// The image orientation detector implementation.
    /// </summary>
    /// <seealso cref="IImageMetadataConverter" />
    internal sealed class ImageMetadataConverter : IImageMetadataConverter
    {
        /// <summary>
        /// Gets the <see cref="ImageOrientation"/> based on the bytes array.
        /// </summary>
        /// <param name="metadataBytes">The image orientation metadata.</param>
        /// <returns>
        /// One of the <see cref="ImageOrientation" /> values.
        /// </returns>
        public ImageOrientation ToOrientation(byte[] metadataBytes)
        {
            if (metadataBytes == null || metadataBytes.Length == 0)
            {
                return ImageOrientation.TopLeft;
            }

            try
            {
                return (ImageOrientation)BitConverter.ToInt16(metadataBytes, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ImageOrientation.TopLeft;
            }
        }

        /// <summary>
        /// Gets the date time the image was taken at.
        /// </summary>
        /// <param name="metadataBytes">The image metadata.</param>
        /// <returns>
        /// Date and time the image was taken at.
        /// </returns>
        public DateTime? ToDateTimeTaken(byte[] metadataBytes)
        {
            return this.GetDateTimeToken(metadataBytes, ImageMetadataType.DateTime);
        }

        /// <summary>
        /// Gets the date time the image was digitized at.
        /// </summary>
        /// <param name="metadataBytes">The image metadata.</param>
        /// <returns>
        /// Date and time the image was digitized. at
        /// </returns>
        public DateTime? ToDateTimeDigitized(byte[] metadataBytes)
        {
            return this.GetDateTimeToken(metadataBytes, ImageMetadataType.DateTimeDigitized);
        }

        /// <summary>
        /// Gets the date time when the image was originally created at.
        /// </summary>
        /// <param name="metadataBytes">The image metadata.</param>
        /// <returns>
        /// Date and time the image was originally taken at.
        /// </returns>
        public DateTime? ToDateTimeOriginal(byte[] metadataBytes)
        {
            return this.GetDateTimeToken(metadataBytes, ImageMetadataType.DateTimeOriginal);
        }

        /// <summary>
        /// Gets a datetime metadata value.
        /// </summary>
        /// <param name="dtBytes">The <see cref="IImage"/> instance.</param>
        /// <param name="token">The <see cref="ImageMetadataType"/> value.</param>
        /// <returns>The date-time instance.</returns>
        private DateTime? GetDateTimeToken(byte[] dtBytes, ImageMetadataType token)
        {
            if (dtBytes == null || dtBytes.Length == 0)
            {
                return null;
            }

            try
            {
                var dtString = Encoding.ASCII.GetString(dtBytes, 0, dtBytes.Length);
                return DateTime.ParseExact(dtString, "yyyy:MM:d H:m:s", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Converts to bytes.
        /// </summary>
        /// <param name="orientation">The <see cref="ImageOrientation" /> to convert.</param>
        /// <returns>
        /// Image orientation metadata byte array.
        /// </returns>
        public byte[] ToBytes(ImageOrientation orientation)
        {
            return BitConverter.GetBytes((short)orientation);
        }

        /// <summary>
        /// Converts to bytes.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /> to convert.</param>
        /// <returns>
        /// Image date-time metadat byte array.
        /// </returns>
        public byte[] ToBytes(DateTime dateTime)
        {
            string dtString = dateTime.ToString("yyyy:MM:d H:m:s", CultureInfo.InvariantCulture);
            return Encoding.ASCII.GetBytes(dtString);
        }
    }
}

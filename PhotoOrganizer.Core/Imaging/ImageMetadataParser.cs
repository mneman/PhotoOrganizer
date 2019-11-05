using System;
using System.Globalization;
using System.Text;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// The image orientation detector implementation.
    /// </summary>
    /// <seealso cref="PhotoOrganizer.Core.Imaging.IImageMetadataParser" />
    internal sealed class ImageMetadataParser : IImageMetadataParser
    {
        /// <summary>
        /// Gets the orientation of a <see cref="IImage" />.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// One of the <see cref="ImageOrientation" /> values.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public ImageOrientation GetOrientation(IImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            try
            {
                var orientationBytes = image.GetMetadata(ImageMetadataType.Orientation);

                if (orientationBytes == null || orientationBytes.Length == 0)
                {
                    return ImageOrientation.TopLeft;
                }

                return (ImageOrientation)BitConverter.ToInt16(orientationBytes, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ImageOrientation.TopLeft;
            }
        }

        /// <summary>
        /// Gets the date time the image was taken.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// Date and time the image was taken.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public DateTime? GetDateTimeTaken(IImage image)
        {
            return this.GetDateTimeToken(image, ImageMetadataType.DateTime);
        }

        /// <summary>
        /// Gets the date time the image was digitized.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// Date and time the image was digitized.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public DateTime? GetDateTimeDigitized(IImage image)
        {
            return this.GetDateTimeToken(image, ImageMetadataType.DateTimeDigitized);
        }

        /// <summary>
        /// Gets the date time when the image was originally created.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// Date and time the image was originally take.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public DateTime? GetDateTimeOriginal(IImage image)
        {
            return this.GetDateTimeToken(image, ImageMetadataType.DateTimeOriginal);
        }

        /// <summary>
        /// Gets a datetime metadata value.
        /// </summary>
        /// <param name="image">The <see cref="IImage"/> instance.</param>
        /// <param name="token">The <see cref="ImageMetadataType"/> value.</param>
        /// <returns>The date-time instance.</returns>
        private DateTime? GetDateTimeToken(IImage image, ImageMetadataType token)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            try
            {
                var dtBytes = image.GetMetadata(token);

                if (dtBytes == null || dtBytes.Length == 0)
                {
                    return null;
                }

                var dtString = Encoding.ASCII.GetString(dtBytes, 0, dtBytes.Length);
                return DateTime.ParseExact(dtString, "yyyy:MM:d H:m:s", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}

using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
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
        /// Gets a <see cref="DateTime"/> image metadata value.
        /// </summary>
        /// <param name="metadataBytes">The <see cref="IImage"/> instance.</param>
        /// <returns>The date-time instance.</returns>
        public DateTime? ToDateTime(byte[] metadataBytes)
        {
            if (metadataBytes == null || metadataBytes.Length == 0)
            {
                return null;
            }

            try
            {
                var dtString = Encoding.ASCII.GetString(metadataBytes, 0, metadataBytes.Length);
                return DateTime.ParseExact(dtString, "yyyy:MM:d H:m:s", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets a mime-type based on the <see cref="Guid"/> representing image raw format.
        /// </summary>
        /// <param name="rawFormat">The raw format <see cref="Guid"/>.</param>
        /// <returns>Image mime-type.</returns>
        public string ToMimeType(Guid rawFormat)
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == rawFormat);

            if (codec == null)
            {
                throw new NotSupportedException($"The {rawFormat} format is not supported.");
            }

            return codec.MimeType;
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

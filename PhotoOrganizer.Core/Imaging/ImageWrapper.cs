using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// The wrapper for the <see cref="Image"/>
    /// </summary>
    internal sealed class ImageWrapper : IImage
    {
        /// <summary>
        /// The <see cref="Image"/> wrappee.
        /// </summary>
        private readonly Image wrappee;

        /// <summary>
        /// Creates an instance of the <see cref="ImageWrapper"/>.
        /// </summary>
        /// <param name="wrappee">The <see cref="Image"/> wrappee.</param>
        public ImageWrapper(Image wrappee)
        {
            this.wrappee = wrappee;
        }

        /// <summary>
        /// Gets the image metadata item.
        /// </summary>
        /// <param name="id">The metadata identifier.</param>
        /// <returns>The metadata value (byte array)</returns>
        public byte[] GetMetadata(int id)
        {
            var item = this.wrappee.PropertyItems.FirstOrDefault(item => item.Id == id);

            return item == null ? new byte[] { } : item.Value;
        }

        /// <summary>
        /// Gets the image metadata item.
        /// </summary>
        /// <param name="type">The metadata type.</param>
        /// <returns>The metadata value (byte array)</returns>
        public byte[] GetMetadata(ImageMetadataType type)
        {
            return this.GetMetadata((int)type);
        }

        /// <summary>
        /// Saves this System.Drawing.Image to the specified file, with the specified encoder and image-encoder parameters.
        /// </summary>
        /// <param name="targetFile">A string that contains the name of the file to which to save this System.Drawing.Image.</param>
        /// <param name="codec">The System.Drawing.Imaging.ImageCodecInfo for this System.Drawing.Image.</param>
        /// <param name="parameters">An System.Drawing.Imaging.EncoderParameters to use for this System.Drawing.Image.</param>
        public void Save(string targetFile, ImageCodecInfo codec, EncoderParameters parameters)
        {
            this.wrappee.Save(targetFile, codec, parameters);
        }

        /// <summary>
        /// Disposes the underlying <see cref="Image"/> wrappee.
        /// </summary>
        public void Dispose()
        {
            this.wrappee.Dispose();
        }
    }
}

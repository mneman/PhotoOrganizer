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
        /// Gets the image metadata item.
        /// </summary>
        /// <param name="id">The metadata identifier.</param>
        /// <returns>The metadata value (byte array)</returns>
        byte[] GetMetadata(int id);

        /// <summary>
        /// Gets the image metadata item.
        /// </summary>
        /// <param name="type">The metadata type.</param>
        /// <returns>The metadata value (byte array)</returns>
        byte[] GetMetadata(ImageMetadataType type);

        /// <summary>
        /// Saves this System.Drawing.Image to the specified file, with the specified encoder and image-encoder parameters.
        /// </summary>
        /// <param name="targetFile">A string that contains the name of the file to which to save this System.Drawing.Image.</param>
        /// <param name="codec">The System.Drawing.Imaging.ImageCodecInfo for this System.Drawing.Image.</param>
        /// <param name="parameters">An System.Drawing.Imaging.EncoderParameters to use for this System.Drawing.Image.</param>
        void Save(string targetFile, ImageCodecInfo codec, EncoderParameters parameters);
    }
}

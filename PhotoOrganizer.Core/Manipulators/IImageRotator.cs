using System.Drawing.Imaging;

namespace PhotoOrganizer.Core.Manipulators
{
    /// <summary>
    /// The image rotator interface.
    /// </summary>
    public interface IImageRotator
    {
        /// <summary>
        /// Gets image transformation <see cref="EncoderParameters"/> for the image rotation.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns>An array of <see cref="EncoderParameters"/> for the rotation of the image.</returns>
        EncoderParameters GetRotationParameters(string imagePath);
    }
}

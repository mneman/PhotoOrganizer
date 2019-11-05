namespace PhotoOrganizer.Core.Manipulators
{
    /// <summary>
    /// The image rotator interface.
    /// </summary>
    public interface IImageRotator
    {
        /// <summary>
        /// Rotates the image.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        void Rotate(string imagePath);
    }
}

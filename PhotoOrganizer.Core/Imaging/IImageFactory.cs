namespace PhotoOrganizer.Core.Imaging
{
    /// <summary>
    /// Factory for <see cref="IImage"/> instance.
    /// </summary>
    public interface IImageFactory
    {
        /// <summary>
        /// Opens an image and wraps it in an <see cref="IImage"/> object.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns>An instance of <see cref="IImage"/> class.</returns>
        IImage OpenImage(string imagePath);    
    }
}

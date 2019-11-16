namespace PhotoOrganizer.Core
{
    /// <summary>
    /// The imag-processing options.
    /// </summary>
    public sealed class Options
    {

        public Options(bool rotate)
        {
            this.Rotate = rotate;
        }

        /// <summary>
        /// Gets a value indicating whether to rotate images.
        /// </summary>
        public bool Rotate { get; }
    }
}

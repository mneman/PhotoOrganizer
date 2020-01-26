namespace PhotoOrganizer.Core
{
    /// <summary>
    /// Image directory sorter interface.
    /// </summary>
    public interface IDirectorySorter
    {
        /// <summary>
        /// Sorts the images inside a directory.
        /// </summary>
        /// <param name="path">The directory to sort.</param>
        /// <param name="options">The <see cref="Options"/> for processing.</param>
        void SortDirectory(string path, Options options);
    }
}

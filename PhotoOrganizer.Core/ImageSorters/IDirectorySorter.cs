namespace PhotoOrganizer.Core.ImageSorters
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
        void SortDirectory(string path);
    }
}

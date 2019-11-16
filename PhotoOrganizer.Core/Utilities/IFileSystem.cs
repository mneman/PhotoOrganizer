using System.Collections.Generic;

namespace PhotoOrganizer.Core.Utilities
{
    /// <summary>
    /// File system utilities.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Checks whether the directory exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the directory exists; <c>false</c> otherwise.</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Enumerates the files in the directory.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <returns>The files from the directory.</returns>
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);

        /// <summary>
        /// Creates a directory if to does not exist.
        /// </summary>
        /// <param name="path">The directory path.</param>
        void CreateDirectorySafe(string path);
    }
}

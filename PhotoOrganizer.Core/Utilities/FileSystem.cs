using System.Collections.Generic;
using System.Composition;
using System.IO;

namespace PhotoOrganizer.Core.Utilities
{
    /// <summary>
    /// File system utilities.
    /// </summary>
    /// <seealso cref="PhotoOrganizer.Core.Utilities.IFileSystem" />
    [Export(typeof(IFileSystem))]
    internal sealed class FileSystem : IFileSystem
    {
        /// <summary>
        /// Checks whether the directory exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        ///   <c>true</c> if the directory exists; <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Enumerates the files in the directory.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <param name="searchPattern">The file search pattern.</param>
        /// <returns>
        /// The files from the directory.
        /// </returns>
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        /// <summary>
        /// Creates a directory if to does not exist.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CreateDirectorySafe(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}

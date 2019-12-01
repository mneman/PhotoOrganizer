﻿using System.Collections.Generic;
using System.Linq;

namespace PhotoOrganizer.Core
{
    /// <summary>
    /// The imag-processing options.
    /// </summary>
    public sealed class Options
    {

        public Options(bool rotate, IEnumerable<string> imageExtensions, string outputDirectory)
        {
            this.Rotate = rotate;
            this.ImageExtensions = imageExtensions.ToList().AsReadOnly();
            this.OutputDirectory = outputDirectory;
        }

        /// <summary>
        /// Gets a value indicating whether to rotate images.
        /// </summary>
        public bool Rotate { get; }

        /// <summary>
        /// Gets the list of image file extensions.
        /// </summary>
        public IReadOnlyCollection<string> ImageExtensions { get; }

        /// <summary>
        /// Gets the output directory.
        /// </summary>
        public string OutputDirectory { get; }
    }
}

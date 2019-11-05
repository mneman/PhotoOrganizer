using System;
using NSubstitute;
using NUnit.Framework;
using PhotoOrganizer.Core.Imaging;
using PhotoOrganizer.Core.Manipulators;

namespace PhotoOrganizer.Core.Tests
{
    public class ImageRotatorTests
    {
        [Test]
        public void Rotate_ImageNullOrWhiteSpace_ThrowsArgumentException([Values("", null, " ")]string path)
        {
            // Arrange
            var parser = Fake.ImageMetadataParser();
            var rotator = new ImageRotator(parser);

            // Act, Assert
            Assert.Throws<ArgumentException>(() => rotator.Rotate(path));
        }

        private static class Fake
        {
            public static IImageMetadataParser ImageMetadataParser()
            {
                var parser = Substitute.For<IImageMetadataParser>();

                return parser;
            }
        }
    }
}
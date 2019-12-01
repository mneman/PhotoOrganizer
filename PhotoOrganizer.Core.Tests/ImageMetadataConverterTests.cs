using System;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using PhotoOrganizer.Core.Imaging;

namespace PhotoOrganizer.Core.Tests
{
    [TestFixture]
    public sealed class ImageMetadataConverterTests
    {
        [Test]
        public void ToOrientation_MetadataBytesNullOrEmpty_ReturnsTopLeft([Values(null, new byte[] { })] byte[] emptyBytes)
        {
            // Arrange
            var converter = new ImageMetadataConverter();

            // Act, Assert
            Assert.AreEqual(ImageOrientation.TopLeft, converter.ToOrientation(emptyBytes));
        }

        [Test]
        public void GetOrientation_InvalidBytes_ReturnsTopLeft()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act
            var orientation = parser.ToOrientation(new byte[] { 1 });

            // Assert
            Assert.AreEqual(ImageOrientation.TopLeft, orientation);
        }

        [TestCase(ImageOrientation.TopLeft)]
        [TestCase(ImageOrientation.TopRight)]
        [TestCase(ImageOrientation.BottomRight)]
        [TestCase(ImageOrientation.BottomLeft)]
        [TestCase(ImageOrientation.LeftTop)]
        [TestCase(ImageOrientation.RightTop)]
        [TestCase(ImageOrientation.RightBottom)]
        [TestCase(ImageOrientation.LeftBottom)]
        public void GetOrientation_ImageWithDifferentOrientationProperty_ReturnsCorrectValue(ImageOrientation expectedOrientation)
        {
            // Arrange
            var orientationByteArray = BitConverter.GetBytes((short)expectedOrientation);

            var parser = new ImageMetadataConverter();

            // Act
            var orientation = parser.ToOrientation(orientationByteArray);

            // Assert
            Assert.AreEqual(expectedOrientation, orientation);
        }

        [Test]
        public void ToDateTimeTaken_MetadataBytesNullOrEmpty_ReturnsNull([Values(null, new byte[] { })] byte[] emptyBytes)
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act, Assert
            Assert.IsNull(parser.ToDateTimeTaken(emptyBytes));
        }

        [Test]
        public void ToDataTimeTaken_FailsToConvertToDateTime_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act
            var dt = parser.ToDateTimeTaken(new byte[] { 1 });

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void ToDateTimeTaken_SuccessfullyConvertsToDateTime_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            // Act
            var dtActual = parser.ToDateTimeTaken(dtBytes);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }

        [Test]
        public void ToDateTimeDigitized_MetadataBytesNullOrEmpty_ThrowsArgumentNulException([Values(null, new byte[] { })] byte[] emptyBytes)
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => parser.ToDateTimeDigitized(emptyBytes));
        }

        [Test]
        public void ToDataTimeDigitized_FailsToConvertToDateTime_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act
            var dt = parser.ToDateTimeDigitized(new byte[] { 1 });

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDateTimeDigitized_SuccessfullyConvertsToDateTime_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            // Act
            var dtActual = parser.ToDateTimeDigitized(dtBytes);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }

        [Test]
        public void GetDateTimeOriginal_MetadataBytesNullOrEmpty_ReturnsNull([Values(null, new byte[] { })] byte[] emptyBytes)
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => parser.ToDateTimeOriginal(emptyBytes));
        }

        [Test]
        public void GetDataTimeOriginal_FailsToConvertToDateTime_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            // Act
            var dt = parser.ToDateTimeOriginal(new byte[] { 1 });

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDateTimeOriginal_SuccessfullyConvertsToDateTime_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataConverter();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            // Act
            var dtActual = parser.ToDateTimeOriginal(dtBytes);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }
    }
}

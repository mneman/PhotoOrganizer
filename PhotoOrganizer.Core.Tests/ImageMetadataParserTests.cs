using System;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using PhotoOrganizer.Core.Imaging;

namespace PhotoOrganizer.Core.Tests
{
    [TestFixture]
    public sealed class ImageMetadataParserTests
    {
        [Test]
        public void GetOrientation_ImageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => parser.GetOrientation(null));
        }

        [Test]
        public void GetOrientation_ImageDoesNotContainOrientationTag_ReturnsTopLeft()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var image = Fake.Image(0, new byte[] { });

            // Act
            var orientation = parser.GetOrientation(image);

            // Assert
            Assert.AreEqual(ImageOrientation.TopLeft, orientation);
        }

        [Test]
        public void GetOrientation_ImageGetMetadataThrowsException_ReturnsTopLeft()
        {
            // Arrange
            var parser = new ImageMetadataParser();
            var image = Fake.Image(new Exception("test exception"));

            // Act
            var orientation = parser.GetOrientation(image);

            // Assert
            Assert.AreEqual(ImageOrientation.TopLeft, orientation);
        }

        [TestCase(1, ImageOrientation.TopLeft)]
        [TestCase(2, ImageOrientation.TopRight)]
        [TestCase(3, ImageOrientation.BottomRight)]
        [TestCase(4, ImageOrientation.BottomLeft)]
        [TestCase(5, ImageOrientation.LeftTop)]
        [TestCase(6, ImageOrientation.RightTop)]
        [TestCase(7, ImageOrientation.RightBottom)]
        [TestCase(8, ImageOrientation.LeftBottom)]
        public void GetOrientation_ImageWithDifferentOrientationProperty_ReturnsCorrectValue(short orientationByte, ImageOrientation expectedOrientation)
        {
            // Arrange
            var orientationByteArray = BitConverter.GetBytes(orientationByte);

            var parser = new ImageMetadataParser();
            var image = Fake.Image(ImageMetadataType.Orientation, orientationByteArray);

            // Act
            var orientation = parser.GetOrientation(image);

            // Assert
            Assert.AreEqual(expectedOrientation, orientation);
        }

        [Test]
        public void GetDateTimeTaken_ImageNull_ThrowsArgumentNulException()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => parser.GetDateTimeTaken(null));
        }

        [Test]
        public void GetDateTimeTaken_ImageDoesNotContainDateTimeToken_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var image = Fake.Image(0, new byte[] { });

            // Act
            var dtTaken = parser.GetDateTimeTaken(image);

            // Assert
            Assert.IsNull(dtTaken);
        }

        [Test]
        public void GetDataTimeTaken_ImageGetMetadataTokenThrowsException_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataParser();
            var image = Fake.Image(new Exception("test exception"));

            // Act
            var dt = parser.GetDateTimeTaken(image);

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDateTimeTaken_ImageWithDateTimeToken_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            var image = Fake.Image(ImageMetadataType.DateTime, dtBytes);

            // Act
            var dtActual = parser.GetDateTimeTaken(image);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }

        [Test]
        public void GetDateTimeDigitized_ImageNull_ThrowsArgumentNulException()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => parser.GetDateTimeDigitized(null));
        }

        [Test]
        public void GetDateTimeDigitized_ImageDoesNotContainDateTimeToken_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var image = Fake.Image(0, new byte[] { });

            // Act
            var dt = parser.GetDateTimeDigitized(image);

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDataTimeDigitized_ImageGetMetadataTokenThrowsException_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataParser();
            var image = Fake.Image(new Exception("test exception"));

            // Act
            var dt = parser.GetDateTimeDigitized(image);

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDateTimeDigitized_ImageWithDateTimeToken_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            var image = Fake.Image(ImageMetadataType.DateTimeDigitized, dtBytes);

            // Act
            var dtActual = parser.GetDateTimeDigitized(image);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }

        [Test]
        public void GetDateTimeOriginal_ImageNull_ThrowsArgumentNulException()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => parser.GetDateTimeOriginal(null));
        }

        [Test]
        public void GetDateTimeOriginal_ImageDoesNotContainDateTimeToken_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var image = Fake.Image(0, new byte[] { });

            // Act
            var dt = parser.GetDateTimeOriginal(image);

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDataTimeOriginal_ImageGetMetadataTokenThrowsException_ReturnsNull()
        {
            // Arrange
            var parser = new ImageMetadataParser();
            var image = Fake.Image(new Exception("test exception"));

            // Act
            var dt = parser.GetDateTimeOriginal(image);

            // Assert
            Assert.IsNull(dt);
        }

        [Test]
        public void GetDateTimeOriginal_ImageWithDateTimeToken_ReturnsDateTimeTaken()
        {
            // Arrange
            var parser = new ImageMetadataParser();

            var dt = DateTime.Today.AddHours(9).AddMinutes(1).AddSeconds(15);
            var dtString = dt.ToString("yyyy:MM:d H:m:s");
            var dtBytes = Encoding.ASCII.GetBytes(dtString);

            var image = Fake.Image(ImageMetadataType.DateTimeOriginal, dtBytes);

            // Act
            var dtActual = parser.GetDateTimeOriginal(image);

            // Assert
            Assert.AreEqual(dt, dtActual);
        }

        private static class Fake
        {
            public static IImage Image(ImageMetadataType metadataType, byte[] value)
            {
                var image = Substitute.For<IImage>();

                image.GetMetadata(Arg.Is(metadataType)).Returns(value);

                return image;
            }

            public static IImage Image(Exception metadataEx)
            {
                var image = Substitute.For<IImage>();

                image.GetMetadata(Arg.Any<ImageMetadataType>()).Returns(x => { throw metadataEx; });

                return image;
            }
        }
    }
}

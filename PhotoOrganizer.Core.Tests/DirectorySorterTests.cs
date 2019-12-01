using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using PhotoOrganizer.Core.ImageSorters;
using PhotoOrganizer.Core.Imaging;
using PhotoOrganizer.Core.Manipulators;
using PhotoOrganizer.Core.Utilities;

namespace PhotoOrganizer.Core.Tests
{
    [TestFixture]
    public sealed class DirectorySorterTests
    {
        [Test]
        public void SortDirectory_NoDirectoryProvided_ThrowsArgumentException([Values("", " ", null)]string dir)
        {
            // Arrange
            var sorter = new DirectorySorter(Fake.ImageRotator(), Fake.ImageFactory(), Fake.FileSystem(Enumerable.Empty<string>()));

            // Act, Assert
            Assert.Throws<ArgumentException>(() => sorter.SortDirectory(dir, Fake.Options()));
        }

        [Test]
        public void SortDirectory_DirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            var dir = "C:\\Images";
            var sorter = new DirectorySorter(Fake.ImageRotator(), Fake.ImageFactory(), Fake.FileSystem(null));

            // Act, Assert
            Assert.Throws<DirectoryNotFoundException>(() => sorter.SortDirectory(dir, Fake.Options()));
        }

        [Test]
        public void SortDirectory_ImagesDontExist_DoesNothing()
        {
            // Arrange
            var rotator = Fake.ImageRotator();
            var factory = Fake.ImageFactory();
            var fileSystem = Fake.FileSystem(Enumerable.Empty<string>());
            var sorter = new DirectorySorter(rotator, factory, fileSystem);

            // Act
            sorter.SortDirectory("C:\\images", Fake.Options());

            // Assert
            rotator.Received(0).GetRotationParameters(Arg.Any<string>());
            factory.Received(0).OpenImage(Arg.Any<string>());
        }

        [Test]
        public void SortDirectory_ImagesExist_OpensImages()
        {
            // Arrange
            var directory = "C:\\images";
            var files = new string[] { "image1.jpg", "image2.jpg" };
            var images = new IImage[] { Fake.Image(), Fake.Image() };

            var options = Fake.Options();
            var rotator = Fake.ImageRotator();
            var factory = Fake.ImageFactory(images);
            var fileSystem = Fake.FileSystem(files);

            var sorter = new DirectorySorter(rotator, factory, fileSystem);

            // Act
            sorter.SortDirectory(directory, options);

            // Assert
            factory.Received(1).OpenImage(Arg.Is(files[0]));
            factory.Received(1).OpenImage(Arg.Is(files[1]));
        }

        //[Test]
        //public void SortDirectory_RotationRequested_RotatesImages()
        //{
        //    // Arrange
        //    var directory = "C:\\images";
        //    var files = new string[] { "image1.jpg", "image2.jpg" };
        //    var images = new IImage[] { Fake.Image(), Fake.Image() };

        //    var options = Fake.Options(rotate: true);
        //    var rotator = Fake.ImageRotator(new[] { null, new EncoderParameters() });
        //    var factory = Fake.ImageFactory(images);
        //    var fileSystem = Fake.FileSystem(files);

        //    var sorter = new DirectorySorter(rotator, factory, fileSystem);

        //    // Act
        //    sorter.SortDirectory(directory, options);

        //    // Assert
        //    rotator.Received(1).GetRotationParameters(Arg.Is(images[0]));
        //    rotator.Received(1).GetRotationParameters(Arg.Is(images[1]));
        //    images[0].DidNotReceiveWithAnyArgs().SetMetadata(Arg.Any<ImageMetadataType>(), Arg.Any<byte[]>());
        //    images[1].Received(1).SetMetadata(Arg.Is(ImageMetadataType.Orientation), Arg.Do<byte[]>(bytes => Enumerable.SequenceEqual(bytes, BitConverter.GetBytes((short)ImageOrientation.TopLeft))));
        //}

        private static class Fake
        {
            public static Options Options(bool rotate = false, string outputDirectory = null, params string[] extensions)
            {
                var options = new Options(rotate, extensions, outputDirectory);

                return options;
            }

            public static IImageRotator ImageRotator(IEnumerable<EncoderParameters> parameters = null)
            {
                var rotator = Substitute.For<IImageRotator>();

                if (parameters != null)
                {
                    if (parameters.Count() == 1)
                    {
                        rotator.GetRotationParameters(Arg.Any<IImage>()).Returns(parameters.First());
                    }
                    else if (parameters.Count() > 1)
                    {
                        rotator.GetRotationParameters(Arg.Any<IImage>()).Returns(parameters.First(), parameters.Skip(1).ToArray());
                    }
                }

                return rotator;
            }

            public static IImageFactory ImageFactory(IEnumerable<IImage> images = null)
            {
                var factory = Substitute.For<IImageFactory>();

                if (images != null)
                {
                    if (images.Count() == 1)
                    {
                        factory.OpenImage(Arg.Any<string>()).Returns(images.First());
                    }
                    else if (images.Count() > 1)
                    {
                        factory.OpenImage(Arg.Any<string>()).Returns(images.First(), images.Skip(1).ToArray());
                    }
                }

                return factory;
            }

            //public static IImageMetadataConverter ImageMetadataProvider(IEnumerable<ImageOrientation> orientations = null)
            //{
            //    var provider = Substitute.For<IImageMetadataConverter>();

            //    if (orientations != null)
            //    {
            //        if (orientations.Count() == 1)
            //        {
            //            provider.GetOrientation(Arg.Any<IImage>()).Returns(orientations.First());
            //        }
            //        else if (orientations.Count() > 1)
            //        {
            //            provider.GetOrientation(Arg.Any<IImage>()).Returns(orientations.First(), orientations.Skip(1).ToArray());
            //        }
            //    }

            //    return provider;
            //}

            public static IFileSystem FileSystem(IEnumerable<string> files)
            {
                var system = Substitute.For<IFileSystem>();

                system.DirectoryExists(Arg.Any<string>()).Returns(files != null);
                system.EnumerateFiles(Arg.Any<string>(), Arg.Any<string>()).Returns(files);

                return system;
            }

            public static IImage Image()
            {
                var image = Substitute.For<IImage>();

                return image;
            }
        }
    }
}

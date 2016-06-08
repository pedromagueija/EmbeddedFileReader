// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFileReaderTests.cs" author="Pedro Magueija">
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Magueija.EmbeddedFileReader.UnitTests
{
    using System;

    using Magueija.EmbeddedFileReader;

    using NSubstitute;

    using NUnit.Framework;

    internal class EmbeddedFileReaderTests
    {

        [Test]
        public void CanReadAnEmbeddedFile()
        {
            var reader = new EmbeddedFileReader();
            string content = reader.ReadEmbeddedFile<EmbeddedFileReaderTests>("EmbeddedTextFile.txt");

            Assert.AreEqual("Lorem ipsum dolor", content);
        }

        [Test]
        public void ThrowsExceptionWhenNoEmbeddedFilesAreFound()
        {
            var finder = Substitute.For<EmbeddedResourceFinder>();
            var reader = new EmbeddedFileReader(finder);

            finder.FindAll<EmbeddedFileReaderTests>().Returns(new string[] { });

            Assert.Throws<InvalidOperationException>(() => reader.ReadEmbeddedFile<EmbeddedFileReaderTests>("SomeNotEmbeddedFile.txt"));
        }

        [Test]
        public void ThrowsExceptionWhenFileIsNotEmbedded()
        {
            var finder = Substitute.For<EmbeddedResourceFinder>();
            var reader = new EmbeddedFileReader(finder);

            finder.FindAll<EmbeddedFileReaderTests>().Returns(new[] { "EmbeddedTextFile.txt" });

            Assert.Throws<InvalidOperationException>(() => reader.ReadEmbeddedFile<EmbeddedFileReaderTests>("SomeNotEmbeddedFile.txt"));
        }
    }
}
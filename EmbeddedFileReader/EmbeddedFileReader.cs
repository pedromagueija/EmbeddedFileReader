// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFileReader.cs" author="Pedro Magueija">
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

namespace NUtil
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Reads an embedded file.<para/>
    /// File must be set as an embedded resource.
    /// </summary>
    public class EmbeddedFileReader
    {
        private readonly EmbeddedResourceFinder finder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedFileReader"/> class.
        /// </summary>
        public EmbeddedFileReader()
        {
            finder = new EmbeddedResourceFinder();
        }

        internal EmbeddedFileReader(EmbeddedResourceFinder finder)
        {
            this.finder = finder;
        }

        /// <summary>
        /// Reads the embedded file.
        /// </summary>
        /// <typeparam name="T">A type in the project where the embedded file is located.</typeparam>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>
        /// The contents of the resource as a string.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if no files are found with the specified name or it can't be read.
        /// </exception>
        public string ReadEmbeddedFile<T>(string resourceName) where T : class
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));

            return ReadEmbeddedFile(resourceName, assembly);
        }


        /// <summary>
        /// Reads the contents of an Embedded Resource file.
        /// </summary>
        /// <param name="resourceName">The name of the file.</param>
        /// <param name="containingAssembly">The assembly that contains the file.</param>
        /// <returns>
        /// The contents of the file.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the file is not an Embedded Resource, or it's impossible to read (permissions) the resource.
        /// </exception>
        public string ReadEmbeddedFile(string resourceName, Assembly containingAssembly)
        {
            string resource = finder.FindOne(resourceName, containingAssembly);
            if (string.IsNullOrEmpty(resource))
            {
                throw new InvalidOperationException(
                    $"The resource {resourceName} was not found in assembly {containingAssembly}. Consider making {resourceName} an Embedded Resource.");
            }

            using (Stream stream = containingAssembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Failed to open a stream to {resourceName}.");
                }

                return ReadContentsFromStream(stream);
            }
        }

        private string ReadContentsFromStream(Stream stream)
        {
            string contents;

            using (var reader = new StreamReader(stream))
            {
                contents = reader.ReadToEnd();
            }

            return contents;
        }
    }
}
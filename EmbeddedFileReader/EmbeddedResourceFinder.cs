// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmbeddedResourceFinder.cs" author="Pedro Magueija">
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

namespace Magueija.EmbeddedFileReader
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class EmbeddedResourceFinder
    {
        public virtual IEnumerable<string> FindAll<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));

            return assembly.GetManifestResourceNames();
        }

        public virtual string FindOne<T>(string resourceName)
        {
            string[] resources = FindAll<T>().ToArray();

            return resources.FirstOrDefault(name => name.EndsWith(resourceName));
        }
    }
}
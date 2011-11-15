using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mindstep.UnpackFinder
{
    class UnpackScanner
    {
        public static string[] getUnpackedWarez(string directory)
        {
            List<string> directories = new List<string>();
            List<string> files;
            try
            {
                files = new List<string>(Directory.EnumerateFiles(directory).Select(Path.GetFileNameWithoutExtension));
            }
            catch
            {
                return directories.ToArray();
            }

            foreach (string subDirectoryPath in Directory.EnumerateDirectories(directory))
            {
                string subDirectory = subDirectoryPath.Split('\\').Last();

                if (files.Contains(subDirectory))
                {
                    directories.Add(directory);
                }

                directories.AddRange(getUnpackedWarez(subDirectoryPath));
            }

            return directories.ToArray();
        }
    }
}

using System;
using System.IO;

namespace ModToMpg.Converter
{
    public class VideoDirectoryGenerator
    {
        private readonly DirectoryInfo _rootDirectory;

        public VideoDirectoryGenerator()
        {
        }

        public VideoDirectoryGenerator(DirectoryInfo rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public DirectoryInfo RootDirectory
        {
            get
            {
                return _rootDirectory ??
                       new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            }
        }

        public DirectoryInfo CreateDirectoryIfDoesNotExist(DateTime date, string format)
        {
            var formatString = "{0:" + format + "}";

            var directoryName = string.Format(formatString, date);
            
            if (!_rootDirectory.Exists)
                _rootDirectory.Create();

            if (!Directory.Exists(Path.Combine(_rootDirectory.FullName, directoryName)))
                _rootDirectory.CreateSubdirectory(directoryName);

            return new DirectoryInfo(Path.Combine(_rootDirectory.FullName, directoryName));
        }
    }
}

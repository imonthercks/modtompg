using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using MOIParser;

namespace ModToMpg.Converter
{
    public class ModVideoConverter
    {
        public ModVideoConverter()
        {
        }

        public string DirectoryDateFormat { get; set; }

        public void Convert(string sourcePath, string destinationPath)
        {
            // Parse all MOI files in sourcePath
            var moiParser = new MOIPathParser(sourcePath);
            moiParser.Parse();

            // Create new directories
            var generator = new VideoDirectoryGenerator(new DirectoryInfo(destinationPath));

            var newDateDirectories = new Dictionary<DateTime, DirectoryInfo>();
            var modFiles = new List<dynamic>();

            foreach (var parsedMoiFile in moiParser.ParsedMOIFiles)
            {
                var createDate = parsedMoiFile.CreationDate;
                var dateAtMidnight = new DateTime(createDate.Year, createDate.Month, createDate.Day);
                if (!newDateDirectories.ContainsKey(dateAtMidnight))
                {
                    var directory = generator.CreateDirectoryIfDoesNotExist(createDate, DirectoryDateFormat);
                    newDateDirectories.Add(dateAtMidnight, directory);
                }

                modFiles.Add(new
                                 {
                                     Dir = newDateDirectories[dateAtMidnight] , 
                                     FileName = Path.GetFileNameWithoutExtension(parsedMoiFile.FileName) + ".MOD", 
                                     CreateDate = createDate
                                 });
            }

            // Copy MOD files to new directory and Rename
            foreach (var modFile in modFiles)
            {
                var sourceFile = Path.Combine(sourcePath, modFile.FileName);
                var destFile = Path.Combine(
                    ((DirectoryInfo) (modFile.Dir)).FullName,
                    Path.GetFileNameWithoutExtension(string.Format("{0:yyyyMMddhhmmss}", modFile.CreateDate)) + ".mpg");

                if (!File.Exists(destFile))
                    File.Copy(sourceFile, destFile);
            }
        }
    }
}

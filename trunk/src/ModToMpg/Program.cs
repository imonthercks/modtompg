using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModToMpg.Converter;
using NDesk.Options;

namespace ModToMpg
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = null;
            string dest = null;

            var p = new OptionSet() {
            { "s|source=", "the {SOURCE} file path or directory containing one or more MOD/MOI files.",
              v => source = v },
            { "d|destination=", 
                "the {DESTINATION} path to copy converted files to.",
              v => dest = v }
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("ModToMpg: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `ModToMpg --help' for more information.");
                return;
            }

            var converter = new ModVideoConverter { DirectoryDateFormat = "yyyyMMdd" };
            converter.Convert(source, dest);
        }
    }
}

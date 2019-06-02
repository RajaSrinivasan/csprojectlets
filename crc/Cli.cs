using System;
using CommandLine;
namespace crc
{
    public class Cli
    {
        public class Options
        {
            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }


            [Option('h', "help", Required = false, HelpText = "Show Help message.")]
            public bool Help { get; set; }


            [Option('x', "hex-string", Required = false, HelpText = "Argument is a hexadecimal string")]
            public bool HexString { get; set; }

            [Option('s', "string", Required = false, HelpText = "Argument is a string")]
            public bool TextString { get; set; }

            [Option('f', "file", Required = false, HelpText = "Argument is a file name. calculate CRC of the file")]
            public bool File { get; set; }

            [Option('l', "lines", Required = false, HelpText = "Argument is a file name. CRC of each line of the file")]
            public bool Lines { get; set; }

            [Option('t', "table", Required = false, HelpText = "Compute and display the initial CRC Table.")]
            public bool Table { get; set; }

            [Option('n', "noise", Required = false, HelpText = "Add noise to the arg. Value is number of bits in error")]
            public int Noise { get; set; }

            [Option('p', "polynomial", Required = false, HelpText = "The polynomial for the CRC. use hex numbers e.g. 0xabab")]
            public string Polynomial { get; set; }

            [Value(0, MetaName = "Argument", HelpText = "Argument.")]
            public string Argument { get; set; }
        }

        public Options opt;

        public Cli(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
               .WithParsed<Options>(o =>
               {
                   opt = o;
               });

        }
    }
}

using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.CLI {
    public class Options {
        [Option('s', "source", Required = true, HelpText = "Swagger JSON定義檔案")]
        public string Source { get; set; }

        [Option('o', "output", Required = false, HelpText = "輸出目錄", Default = "./output")]
        public string Output { get; set; }

        [Option('b', "base", Required = false, HelpText = "base href")]
        public string Base { get; set; }
    }
}

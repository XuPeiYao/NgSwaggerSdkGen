using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NgSwaggerSdkGen.Models.CLI;
using NgSwaggerSdkGen.Models.Swagger;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NgSwaggerSdkGen {
    class Program {
        static void Main(string[] args) {
#if DEBUG
            args = new string[] {
                "-s",
                "https://econ-stage.gofa.cloud/swagger/v1/swagger.json"
            };
            //args = new string[] { "help" };
#endif

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o => GenSdkModule(o).GetAwaiter().GetResult());
        }

        public static async Task GenSdkModule(Options options) {
            HttpClient client = new HttpClient();


            var rawString = await client.GetStringAsync(options.Source);
            var settings = new JsonSerializerSettings();
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            settings.Error += new EventHandler<ErrorEventArgs>(delegate (object sender, ErrorEventArgs args) {
                args.ErrorContext.Handled = true;
            });

            var document = JsonConvert.DeserializeObject<Document>(rawString, settings);

            document.definitions = PreProcessing.FixTypeName(document.definitions);

            foreach (var d in document.definitions) {
                Console.WriteLine(string.Join("\r\n", d.Value.properties.Select(x => d.Key + ", " + x.Key + ", " + x.Value.GetTypeString())));
            }




        }
    }
}

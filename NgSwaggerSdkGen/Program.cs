using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NgSwaggerSdkGen.Models.CLI;
using NgSwaggerSdkGen.Models.Gen;
using NgSwaggerSdkGen.Models.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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


            #region 主程序
            // Model產生
            var models = new List<TsModelType>();
            foreach (var definition in document.definitions) {
                models.Add(new TsModelType() {
                    Name = definition.Key,
                    Properties = definition.Value.properties
                        .Select(x => new TsProperty() {
                            Type = x.Value.GetTypeString(),
                            Description = x.Value.description,
                            Name = x.Key
                        }).ToList()
                });
            }

            var services = new List<TsServiceType>();

            var serviceNames = document.paths.SelectMany(x => x.Value.GetTags()).Distinct().ToArray();
            foreach (var serviceName in serviceNames) {
                services.Add(new TsServiceType() {
                    Name = serviceName
                });
            }


            // Service
            foreach (var path in document.paths) {
                foreach (var method in path.Value.GetOperations()) {
                    var service = services.First(x => x.Name == method.Value.tags.FirstOrDefault());
                    if (service == null) continue;
                    if (service.Methods == null) {
                        service.Methods = new List<TsMethod>();
                    }

                    TsMethod methodInstance = null;

                    service.Methods.Add(methodInstance = new TsMethod() {
                        Name = ToCamelCase(method.Value.operationId.Split(new char[] { '_' }, 2).Last()),
                        Description = method.Value.summary,
                        ReturnType = method.Value.responses["200"].schema?.GetTypeString(),
                        Method = method.Key,
                        Parameters = new List<TsParameter>()
                    });

                    foreach (var param in method.Value.parameters ?? new Parameter[0]) {
                        methodInstance.Parameters.Add(new TsParameter() {
                            Name = param.name,
                            IsRequerd = param.required ?? false,
                            Description = param.description,
                            Default = param.@default == null ? null : (param.@default is string ? $"\'{Regex.Escape(param.@default.ToString())}\'" : param.@default.ToString()),
                            Type = param.GetTypeString(),
                            In = param.@in
                        });
                    }
                }
            }
            #endregion



        }

        public static string ToCamelCase(string str) {
            if (!string.IsNullOrEmpty(str) && str.Length > 1) {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}

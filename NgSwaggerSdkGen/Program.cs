using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NgSwaggerSdkGen.Models.CLI;
using NgSwaggerSdkGen.Models.Gen;
using NgSwaggerSdkGen.Models.Swagger;
using NgSwaggerSdkGen.Models.View;
using RazorLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NgSwaggerSdkGen {
    public class Program {
        static void Main(string[] args) {
#if DEBUG
            args = new string[] {
                "-s",
                "http://magic-modules.hamastar.com.tw/swagger/v1/swagger.json"
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
            settings.Error += new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) {
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
                        Parameters = new List<TsParameter>(),
                        Url = path.Key
                    });

                    if (methodInstance.ReturnType == "file") {
                        methodInstance.ReturnType = "Blob";
                    }

                    foreach (var param in method.Value.parameters ?? new Parameter[0]) {
                        TsParameter paramInstance = null;

                        methodInstance.Parameters.Add(paramInstance = new TsParameter() {
                            Name = param.name,
                            IsRequired = param.required ?? false,
                            Description = param.description,
                            Default = param.@default?.ToString(),
                            Type = param.GetTypeString(),
                            In = param.@in
                        });

                        if (paramInstance.Type == "file") {
                            paramInstance.Type = "File";
                        }

                        if (param.collectionFormat == "multi" && param.type != "array") {
                            paramInstance.Type += "[]";
                        }

                        if (paramInstance.Default != null) {
                            if (paramInstance.Type == "string") {
                                paramInstance.Default = '"' + Regex.Escape(param.@default.ToString()) + '"';
                            }
                            if (paramInstance.Type == "boolean") {
                                paramInstance.Default = paramInstance.Default.ToLower();
                            }
                        }
                    }
                }
            }
            #endregion

            #region 輸出目錄建構
            Console.Write("建立目錄結構...");
            if (Directory.Exists(options.Output)) {
                Directory.Delete(options.Output, true);
            }
            Directory.CreateDirectory(options.Output);
            Directory.CreateDirectory(Path.Combine(options.Output, "models"));
            Directory.CreateDirectory(Path.Combine(options.Output, "services"));
            Console.WriteLine("OK");
            #endregion

            #region index.ts
            Console.Write("建立index.ts...");
            File.WriteAllText(
                Path.Combine(options.Output, "index.ts"),
                    string.Join("\r\n",
                        $"export * from './services';",
                        $"export * from './models';",
                        $"export * from './config';",
                        $"export * from './{ToCamelCase(options.Module)}.module';",
                        $""
                    )
                );

            File.WriteAllText(
                Path.Combine(options.Output, "models", "index.ts"),
                    string.Join("\r\n",
                        models.Select(x => $"export * from './{ToCamelCase(x.Name)}';")
                    ) + "\r\n"
                );

            File.WriteAllText(
                Path.Combine(options.Output, "services", "index.ts"),
                    string.Join("\r\n",
                        services.Select(x => $"export * from './{ToCamelCase(x.Name)}.service';")
                    ) + "\r\n"
                );
            Console.WriteLine("OK");
            #endregion

            #region config.ts
            Console.Write("建立config.ts...");
            File.WriteAllText(
                Path.Combine(options.Output, "config.ts"),
                File.ReadAllText("./Templates/config.txt"));
            Console.WriteLine("OK");
            #endregion

            var engine = new RazorLightEngineBuilder().Build();

            #region Module
            Console.Write("建立Module...");
            var moduleFilename = Path.Combine(options.Output, $"{ToCamelCase(options.Module)}.module.ts");
            string moduleTemplate = System.IO.File.ReadAllText("./Templates/module.txt");

            string moduleOutput = WebUtility.HtmlDecode(engine.CompileRenderAsync(
                        Guid.NewGuid().ToString(),
                        moduleTemplate,
                        new NgModule() {
                            Name = options.Module,
                            Services = services.Select(x => x.Name)
                        },
                        typeof(NgModule)
                    ).GetAwaiter().GetResult().Trim());

            File.WriteAllText(moduleFilename, moduleOutput);

            Console.WriteLine("OK");
            #endregion

            #region Model
            Console.Write("建立Models...");
            string modelTemplate = System.IO.File.ReadAllText("./Templates/model.txt");

            foreach (var model in models) {
                var modelFilename = Path.Combine(options.Output, "models", $"{ToCamelCase(model.Name)}.ts");

                string modelOutput = WebUtility.HtmlDecode(engine.CompileRenderAsync(
                        Guid.NewGuid().ToString(),
                        modelTemplate,
                        new NgModel() {
                            Name = model.Name,
                            Models = models.Where(x => x != model).Select(x => x.Name),
                            Properties = model.Properties
                        },
                        typeof(NgModel)
                    ).GetAwaiter().GetResult().Trim());

                File.WriteAllText(modelFilename, modelOutput);
            }
            Console.WriteLine("OK");
            #endregion

            #region Service
            Console.Write("建立Services...");
            string serviceTemplate = System.IO.File.ReadAllText("./Templates/service.txt");

            foreach (var service in services) {
                var serviceFilename = Path.Combine(options.Output, "services", $"{ToCamelCase(service.Name)}.service.ts");

                string serviceOutput = WebUtility.HtmlDecode(engine.CompileRenderAsync(
                        Guid.NewGuid().ToString(),
                        serviceTemplate,
                        new NgService() {
                            Name = service.Name,
                            Models = models.Select(x => x.Name),
                            Methods = service.Methods
                        },
                        typeof(NgService)
                    ).GetAwaiter().GetResult().Trim());

                File.WriteAllText(serviceFilename, serviceOutput);
            }
            Console.WriteLine("OK");
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

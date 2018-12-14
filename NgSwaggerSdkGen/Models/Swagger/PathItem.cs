using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NgSwaggerSdkGen.Models.Swagger {
    public class PathItem {
        [JsonProperty("$ref")]
        public string @ref;

        public Operation get;

        public Operation put;

        public Operation post;

        public Operation delete;

        public Operation options;

        public Operation head;

        public Operation patch;

        public IList<Parameter> parameters;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();

        public string[] GetTags() {
            var properties = this.GetType().GetFields().Where(x => x.FieldType == typeof(Operation));

            return properties.SelectMany(x => ((Operation)x.GetValue(this))?.tags ?? new string[0]).Distinct().ToArray();
        }

        public KeyValuePair<string, Operation>[] GetOperations() {
            var properties = this.GetType().GetFields().Where(x => x.FieldType == typeof(Operation));

            return properties.Select(x => {
                return new KeyValuePair<string, Operation>(x.Name, (Operation)x.GetValue(this));
            }).Where(x => x.Value != null).ToArray();
        }
    }
}

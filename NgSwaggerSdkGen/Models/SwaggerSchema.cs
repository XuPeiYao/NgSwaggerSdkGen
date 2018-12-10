using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerSchema {
        [JsonProperty("$ref")]
        public string @ref;

        public string format;

        public string title;

        public string description;

        public object @default;

        public int? multipleOf;

        public int? maximum;

        public bool? exclusiveMaximum;

        public int? minimum;

        public bool? exclusiveMinimum;

        public int? maxLength;

        public int? minLength;

        public string pattern;

        public int? maxItems;

        public int? minItems;

        public bool? uniqueItems;

        public int? maxProperties;

        public int? minProperties;

        public IList<string> required;

        public IList<object> @enum;

        public string type;

        public SwaggerSchema items;

        public IList<SwaggerSchema> allOf;

        public IDictionary<string, SwaggerSchema> properties;

        public SwaggerSchema additionalProperties;

        public string discriminator;

        public bool? readOnly;

        public SwaggerXml xml;

        public SwaggerExternalDocs externalDocs;

        public object example;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerParameter : SwaggerPartialSchema {
        [JsonProperty("$ref")]
        public string @ref;

        public string name;

        public string @in;

        public string description;

        public bool? required;

        public SwaggerSchema schema;
    }
}

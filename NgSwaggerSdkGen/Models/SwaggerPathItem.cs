using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerPathItem {
        [JsonProperty("$ref")]
        public string @ref;

        public SwaggerOperation get;

        public SwaggerOperation put;

        public SwaggerOperation post;

        public SwaggerOperation delete;

        public SwaggerOperation options;

        public SwaggerOperation head;

        public SwaggerOperation patch;

        public IList<SwaggerParameter> parameters;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

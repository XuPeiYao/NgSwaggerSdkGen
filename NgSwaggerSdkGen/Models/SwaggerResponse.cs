using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerResponse {
        public string description;

        public SwaggerSchema schema;

        public IDictionary<string, SwaggerHeader> headers;

        public object examples;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

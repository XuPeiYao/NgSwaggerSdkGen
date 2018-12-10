using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerSecurityScheme {
        public string type;

        public string description;

        public string name;

        public string @in;

        public string flow;

        public string authorizationUrl;

        public string tokenUrl;

        public IDictionary<string, string> scopes;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

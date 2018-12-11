using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.Swagger {
    public class Response {
        public string description;

        public Schema schema;

        public IDictionary<string, Header> headers;

        public object examples;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

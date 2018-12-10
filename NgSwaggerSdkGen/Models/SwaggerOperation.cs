using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerOperation {
        public IList<string> tags;

        public string summary;

        public string description;

        public SwaggerExternalDocs externalDocs;

        public string operationId;

        public IList<string> consumes;

        public IList<string> produces;

        public IList<SwaggerParameter> parameters;

        public IDictionary<string, SwaggerResponse> responses;

        public IList<string> schemes;

        public bool? deprecated;

        public IList<IDictionary<string, IEnumerable<string>>> security;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

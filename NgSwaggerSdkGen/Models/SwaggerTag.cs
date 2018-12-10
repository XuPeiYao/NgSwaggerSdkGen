using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerTag {
        public string name;

        public string description;

        public SwaggerExternalDocs externalDocs;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

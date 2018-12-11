using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.Swagger {
    public class Tag {
        public string name;

        public string description;

        public ExternalDocs externalDocs;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

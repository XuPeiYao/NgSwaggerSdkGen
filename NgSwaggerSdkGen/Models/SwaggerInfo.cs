using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerInfo {
        public string version;

        public string title;

        public string description;

        public string termsOfService;

        public SwaggerContact contact;

        public SwaggerLicense license;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

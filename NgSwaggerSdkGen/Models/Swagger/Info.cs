using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.Swagger {
    public class Info {
        public string version;

        public string title;

        public string description;

        public string termsOfService;

        public Contact contact;

        public License license;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

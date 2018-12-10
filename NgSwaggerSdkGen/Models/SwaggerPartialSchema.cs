using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerPartialSchema {
        public string type;

        public string format;

        public SwaggerPartialSchema items;

        public string collectionFormat;

        public object @default;

        public int? maximum;

        public bool? exclusiveMaximum;

        public int? minimum;

        public bool? exclusiveMinimum;

        public int? maxLength;

        public int? minLength;

        public string pattern;

        public int? maxItems;

        public int? minItems;

        public bool? uniqueItems;

        public IList<object> @enum;

        public int? multipleOf;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }
}

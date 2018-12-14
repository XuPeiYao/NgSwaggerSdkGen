using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NgSwaggerSdkGen.Models.Swagger {
    public class PartialSchema {
        public string type;

        public string format;

        public PartialSchema items;

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

        public string GetTypeString() {
            string result = "";
            switch (type) {
                case "object":
                    result = this is Parameter param ? param.@ref : "object";
                    break;
                case "array":
                    return items.GetTypeString() + "[]";
                case "string":
                    if (@enum != null && @enum.Count > 0) {
                        return $"({string.Join(" | ", @enum.Select(x => x is string ? "'" + Regex.Escape(x.ToString()) + "'" : x))})";
                    }
                    return "string";
                default:
                    if (type != null) {
                        result = type;
                        break;
                    }

                    if (this is Parameter param_) {
                        result = param_.@ref;
                        result = result ?? ((param_.@in == "path") ? "string" : param_.schema.GetTypeString());
                        result = result ?? "object";
                    }
                    break;
            }

            if (result?.StartsWith("#") ?? false) {
                return PreProcessing.FixTypeName(result.Split('/').Last());
            }

            return result != null ? PreProcessing.FixTypeName(result) : null;
        }
    }
}

using System.Collections.Generic;

namespace NgSwaggerSdkGen.Models.Gen {
    public class TsParameter {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public string Default { get; set; }
        public string In { get; set; }
        public IList<object> Enum { get; internal set; }
        public bool IsArray {
            get {
                return Type.Contains("[]");
            }
        }
    }
}
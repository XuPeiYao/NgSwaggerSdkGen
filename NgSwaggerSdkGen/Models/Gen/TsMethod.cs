using System.Collections.Generic;

namespace NgSwaggerSdkGen.Models.Gen {
    public class TsMethod {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
        public string Method { get; set; }
        public List<TsParameter> Parameters { get; set; }
    }
}
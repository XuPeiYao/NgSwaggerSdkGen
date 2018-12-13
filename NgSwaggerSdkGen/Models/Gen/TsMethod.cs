using System.Collections.Generic;
using System.Linq;

namespace NgSwaggerSdkGen.Models.Gen {
    public class TsMethod {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
        public string Method { get; set; }
        public List<TsParameter> Parameters { get; set; }
        public bool IsFormData {
            get {
                if (Parameters == null) return false;
                return (Method == "post" || Method == "put") && Parameters.Any(x => x.Type == "file");
            }
        }
    }
}
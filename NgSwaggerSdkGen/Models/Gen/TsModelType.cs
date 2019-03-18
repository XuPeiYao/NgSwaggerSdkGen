using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.Gen {
    public class TsModelType {
        public string Name { get; set; }
        public string Extends { get; set; }
        public List<TsProperty> Properties { get; set; }
    }
}

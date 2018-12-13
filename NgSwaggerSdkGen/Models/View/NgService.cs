using NgSwaggerSdkGen.Models.Gen;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.Gen {
    public class NgService : TsServiceType {
        public IEnumerable<string> Models { get; set; }
    }
}

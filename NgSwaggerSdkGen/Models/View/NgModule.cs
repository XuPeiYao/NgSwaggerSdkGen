using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models.View {
    public class NgModule {
        public string Name { get; set; }
        public IEnumerable<string> Services { get; set; }
    }
}

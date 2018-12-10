using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen.Models {
    public class SwaggerDocument {
        public readonly string swagger = "2.0";

        public SwaggerInfo info;

        public string host;

        public string basePath;

        public IList<string> schemes;

        public IList<string> consumes;

        public IList<string> produces;

        public IDictionary<string, SwaggerPathItem> paths;

        public IDictionary<string, SwaggerSchema> definitions;

        public IDictionary<string, SwaggerParameter> parameters;

        public IDictionary<string, SwaggerResponse> responses;

        public IDictionary<string, SwaggerSecurityScheme> securityDefinitions;

        public IList<IDictionary<string, IEnumerable<string>>> security;

        public IList<SwaggerTag> tags;

        public SwaggerExternalDocs externalDocs;

        public Dictionary<string, object> vendorExtensions = new Dictionary<string, object>();
    }


























}

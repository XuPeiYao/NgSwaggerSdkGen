using NgSwaggerSdkGen.Models.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace NgSwaggerSdkGen {
    public static class PreProcessing {
        /// <summary>
        /// 修正定義類型名稱
        /// </summary>
        /// <param name="document">Swagger文件</param>
        public static IDictionary<string, Schema> FixTypeName(IDictionary<string, Schema> definitions) {
            Dictionary<string, Schema> temp = new Dictionary<string, Schema>();
            foreach (var def in definitions) {
                var newKey = FixTypeName(def.Key);
                temp[newKey] = def.Value;
            }

            return temp;
        }

        public static string FixTypeName(string typeName) {
            switch (typeName) {
                case "integer":
                case "double":
                case "long":
                    return "number";
            }
            return typeName.Replace("[]", "@!")
                .Replace("[", "").Replace("]", "")
                .Replace("<", "").Replace(">", "")
                .Replace("@!", "[]");
        }
    }
}

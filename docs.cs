using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace docsNET
{
    [Serializable]
    public class Docs
    {
        [JsonProperty("Class")]
        public string docClass;
        [JsonProperty("Description")]
        public string docDesc;
        [JsonProperty("Link")]
        public string docLink;
    }
}

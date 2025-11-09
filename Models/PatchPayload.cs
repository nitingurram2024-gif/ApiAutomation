using System.Collections.Generic;

namespace ApiAutomation.Models
{
    public class PatchPayload
    {
        public string name { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}

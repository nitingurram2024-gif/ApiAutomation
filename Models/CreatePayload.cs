using System.Collections.Generic;

namespace ApiAutomation.Models
{
    public class CreatePayload
    {
        public string name { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}

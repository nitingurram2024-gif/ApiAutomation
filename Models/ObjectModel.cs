using System.Collections.Generic;

namespace ApiAutomation.Models
{
    public class ObjectModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}

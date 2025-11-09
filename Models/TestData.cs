using System.Collections.Generic;

namespace ApiAutomation.Models
{
    public class TestData
    {
        public CreateObjectData createObject { get; set; }
        public UpdateObjectData updateObject { get; set; }
        public PatchObjectData patchObject { get; set; }
    }

    public class CreateObjectData
    {
        public string name { get; set; }
        public Dictionary<string, object> data { get; set; }
    }

    public class UpdateObjectData
    {
        public string name { get; set; }
        public Dictionary<string, object> data { get; set; }
    }

    public class PatchObjectData
    {
        public Dictionary<string, object> data { get; set; }
    }
}

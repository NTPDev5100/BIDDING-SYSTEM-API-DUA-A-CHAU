using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DomainModels
{
    public class ControllerModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Action { get; set; }
    }
    public class ActionModel
    {
        public string id { get; set; }

        public string name { get; set; }
        public bool grant { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities
{
    public class AppDomainResult
    {
        public AppDomainResult()
        {
            //Messages = new List<string>();
        }
        public bool Success { get; set; } = false;
        public object Data { get; set; }
        public int ResultCode { get; set; }
        //public IList<string> Messages { get; set; }
        public string ResultMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}

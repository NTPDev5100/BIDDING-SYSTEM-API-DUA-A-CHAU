using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Models
{
    public class WardsModel : AppDomainModel
    {
        public Guid? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

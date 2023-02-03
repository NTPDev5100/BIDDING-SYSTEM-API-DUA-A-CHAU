using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Models
{
    public class DistrictsModel : AppDomainModel
    {
        public Guid? CityId { get; set; }
        public string CityName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

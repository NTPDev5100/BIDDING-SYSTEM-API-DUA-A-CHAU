using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Search
{
    public class DistrictsSearch : BaseSearch
    {
        public Guid? CityId { get; set; }
    }
}

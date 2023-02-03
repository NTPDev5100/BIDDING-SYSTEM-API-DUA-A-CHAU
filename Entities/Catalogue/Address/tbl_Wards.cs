using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class tbl_Wards : DomainEntities.AppDomainCatalogue
    {
        public Guid? DistrictId { get; set; }
        [NotMapped]
        public string DistrictName { get; set; }
    }
}

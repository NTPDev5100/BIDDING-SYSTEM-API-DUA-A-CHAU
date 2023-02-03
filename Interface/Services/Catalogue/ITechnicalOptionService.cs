﻿using Entities.Catalogue;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.Catalogue
{
    public interface ITechnicalOptionService : ICatalogueService<tbl_TechnicalOptions, TechnicalOptionsSearch>
    {
    }
}

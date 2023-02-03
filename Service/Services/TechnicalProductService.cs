using AutoMapper;
using Entities;
using Entities.Search;
using Interface.DbContext;
using Interface.Services;
using Interface.UnitOfWork;
using Newtonsoft.Json;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace Service.Services
{
    public class TechnicalProductService : DomainServices.DomainService<tbl_TechnicalProduct, TechnicalProductSearch>, ITechnicalProductService
    {
        protected IAppDbContext coreDbContext;
        public TechnicalProductService(IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext) : base(unitOfWork, mapper)
        {
            this.coreDbContext = coreDbContext;
        }

        protected override string GetStoreProcName()
        {
            return "TechnicalProduct_GetPagingData";
        }


        public async Task AddItemTechnicalProduct(TechnicalProductCreate createModel, Guid idUserCreated)
        {
            List<ObjectTechnicalProduct> technicalOfProduct = JsonConvert.DeserializeObject<List<ObjectTechnicalProduct>>(createModel.TechnicalValue);
            List<ObjectTechnicalProduct> technicalInProduct = new List<ObjectTechnicalProduct>();
            foreach (ObjectTechnicalProduct itemTechnical in technicalOfProduct)
            {
                technicalInProduct.Add(new ObjectTechnicalProduct { FileName = itemTechnical.FileName, Link = itemTechnical.Link});
            }
            var technicalProductCreateModel = new tbl_TechnicalProduct()
            {
                ProductId = createModel.ProductId,
                TechnicalValue = JsonConvert.SerializeObject(technicalInProduct),
                Created = Timestamp.UtcNow(),
                CreatedBy = idUserCreated,
                Active = true,
                Deleted = false
            };
            await CreateAsync(technicalProductCreateModel);
        }


        public async Task UpdateItemTechnicalProduct(TechnicalProductUpdate updateModel, Guid idUserUpdate)
        {
            List<ObjectTechnicalProduct> technicalOfProduct = JsonConvert.DeserializeObject<List<ObjectTechnicalProduct>>(updateModel.TechnicalValue);
            List<ObjectTechnicalProduct> technicalInProduct = new List<ObjectTechnicalProduct>();
            foreach (ObjectTechnicalProduct itemTechnical in technicalOfProduct)
            {
                technicalInProduct.Add(new ObjectTechnicalProduct { FileName = itemTechnical.FileName, Link = itemTechnical.Link });
            }
            var technicalProductUpdateModel = new tbl_TechnicalProduct()
            {
                TechnicalValue = JsonConvert.SerializeObject(technicalInProduct),
                Updated = Timestamp.UtcNow(),
                UpdatedBy = idUserUpdate,
                Id = updateModel.Id
            };
            await UpdateAsync(technicalProductUpdateModel);
        }


        public async Task DeleteTechnicalByProductId(Guid productId)
        {
            var technical = await Queryable.Where(x => x.ProductId == productId && x.Deleted == false).AsNoTracking().FirstOrDefaultAsync();
            if(technical != null)
            {
                await this.DeleteDataAsync(technical.Id);
            }
        }

        public async Task<int> GetCountTechnicalByOptionId(Guid technicalOptionId)
        {

            var count = await Task.FromResult(Queryable.Where(x => x.TechnicalValue.Contains(technicalOptionId.ToString()) && x.Deleted == false).AsNoTracking().Count());
            return count;

        }
    }
}

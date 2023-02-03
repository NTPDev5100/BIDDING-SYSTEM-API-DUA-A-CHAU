using AutoMapper;
using Entities.DomainEntities;
using Interface.Services.DomainServices;
using Interface.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Service.Services.DomainServices
{
    public abstract class CatalogueService<E, T> : DomainService<E, T>, ICatalogueService<E, T> where E : AppDomainCatalogue, new() where T : BaseSearch, new()
    {
        protected IConfiguration configuration;
        public bool IsUseStore = false;
        public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            this.configuration = configuration;
        }

        public CatalogueService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public virtual E GetByCode(string code)
        {
            return unitOfWork.CatalogueRepository<E>()
                .GetQueryable()
                .Where(e => e.Code == code && !e.Deleted.Value)
                .FirstOrDefault();
        }

        public Task<AppDomainImportResult> ImportTemplateFile(Stream stream, string CreatedBy)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Lấy danh sách phân trang danh mục
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        public override async Task<PagedList<E>> GetPagedListData(T baseSearch)
        {
            if (IsUseStore)
                return await base.GetPagedListData(baseSearch);
            PagedList<E> pagedList = new PagedList<E>();
            int skip = (baseSearch.PageIndex - 1) * baseSearch.PageSize;
            int take = baseSearch.PageSize;

            var items = this.Queryable.Where(GetExpression(baseSearch));
            decimal itemCount = items.Count();
            if (baseSearch.OrderBy == 0)
            {
                pagedList = new PagedList<E>()
                {
                    TotalItem = (int)itemCount,
                    Items = await items.OrderByDescending(x => x.Created).Skip(skip).Take(baseSearch.PageSize).ToListAsync(),
                    PageIndex = baseSearch.PageIndex,
                    PageSize = baseSearch.PageSize,
                };
            }
            if (baseSearch.OrderBy == 1)
            {
                pagedList = new PagedList<E>()
                {
                    TotalItem = (int)itemCount,
                    Items = await items.OrderBy(x => x.Created).Skip(skip).Take(baseSearch.PageSize).ToListAsync(),
                    PageIndex = baseSearch.PageIndex,
                    PageSize = baseSearch.PageSize,
                };
            }
            return pagedList;
        }

        protected virtual Expression<Func<E, bool>> GetExpression(T baseSearch)
        {
            return e => !e.Deleted.Value
            && (string.IsNullOrEmpty(baseSearch.SearchContent)
                || (e.Code.ToLower().Contains(baseSearch.SearchContent.ToLower())
                || e.Name.ToLower().Contains(baseSearch.SearchContent.ToLower())
                || e.Description.ToLower().Contains(baseSearch.SearchContent.ToLower()))
                );
        }
        /// <summary>
        /// Check trùng mã
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<string> GetExistItemMessage(E item)
        {
            bool isExistCode = await this.Queryable.AnyAsync(x => !x.Deleted.Value && x.Id != item.Id && x.Code == item.Code);
            if (isExistCode)
                return "Mã đã tồn tại!";
            return string.Empty;
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities.DomainEntities;
using Extensions;
using Interface.Services.DomainServices;
using Interface.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Utilities;

namespace Service.Services.DomainServices
{
    public abstract class DomainService<E, T> : IDomainService<E, T> where E : Entities.DomainEntities.DomainEntities where T : BaseSearch, new()
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        protected IQueryable<E> Queryable
        {
            get
            {
                return unitOfWork.Repository<E>().GetQueryable().AsNoTracking();
            }
        }

        public DomainService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public virtual void LoadReferences(IList<E> items)
        {
        }

        public virtual bool Save(E item)
        {
            return Save(new List<E> { item });
        }

        public virtual bool Save(IList<E> items)
        {
            foreach (var item in items)
            {
                var exists = Queryable
                .AsNoTracking()
                .Where(e =>
                e.Id == item.Id
                && e.Deleted == false)
                .FirstOrDefault();
                if (exists != null)
                {
                    if (item.Deleted == false)
                    {
                        Delete(item.Id);
                    }
                    else
                    {
                        exists = mapper.Map<E>(item);
                        unitOfWork.Repository<E>().SetEntityState(exists, EntityState.Modified);
                    }
                }
                else
                {
                    unitOfWork.Repository<E>().Create(item);
                }
            }
            unitOfWork.Save();
            return true;
        }

        public virtual bool IsSafeDelete(int id)
        {
            return true;
        }

        public virtual bool Delete(Guid id)
        {
            var exists = Queryable
                .Where(e => e.Id == id)
                .FirstOrDefault();
            if (exists != null)
            {
                exists.Deleted = true;
                unitOfWork.Repository<E>().Update(exists);
                unitOfWork.Save();
                return true;
            }
            else
            {
                throw new Exception(id + " not exists");
            }
        }

        public virtual IList<E> GetAll()
        {
            return GetAll(null);
        }

        public virtual IList<E> GetAll(Expression<Func<E, E>> select)
        {
            var query = Queryable.Where(e => e.Deleted == false);
            if (select != null)
            {
                query = query.Select(select);
            }
            return query.ToList();
        }

        public virtual async Task<PagedList<E>> GetPagedListData(T baseSearch)
        {
            //if (!GetIsStore())
            //{
            //    PagedList<E> pagedList = new PagedList<E>();
            //    int skip = (baseSearch.PageIndex - 1) * baseSearch.PageSize;
            //    int take = baseSearch.PageSize;

            //    var items = this.Queryable.Where(x => !x.Deleted.Value);
            //    decimal itemCount = items.Count();
            //    pagedList = new PagedList<E>()
            //    {
            //        TotalItem = (int)itemCount,
            //        Items = await items.OrderBy(x => x.Created).Skip(skip).Take(baseSearch.PageSize).ToListAsync(),
            //        PageIndex = baseSearch.PageIndex,
            //        PageSize = baseSearch.PageSize,
            //    };
            //    return pagedList;
            //}
            //else
            //{
            //    PagedList<E> pagedList = new PagedList<E>();
            //    SqlParameter[] parameters = GetSqlParameters(baseSearch);
            //    pagedList = await this.unitOfWork.Repository<E>().ExcuteQueryPagingAsync(this.GetStoreProcName(), parameters);
            //    pagedList.PageIndex = baseSearch.PageIndex;
            //    pagedList.PageSize = baseSearch.PageSize;
            //    return pagedList;
            //}
            PagedList<E> pagedList = new PagedList<E>();
            SqlParameter[] parameters = GetSqlParameters(baseSearch);
            pagedList = await this.unitOfWork.Repository<E>().ExcuteQueryPagingAsync(this.GetStoreProcName(), parameters);
            pagedList.PageIndex = baseSearch.PageIndex;
            pagedList.PageSize = baseSearch.PageSize;
            return pagedList;
        }

        //protected virtual bool GetIsStore() => true;

        /// <summary>
        /// Lấy thông tin tên procedure cần exec
        /// </summary>
        /// <returns></returns>
        protected virtual string GetStoreProcName()
        {
            return string.Empty;
        }

        protected virtual SqlParameter[] GetSqlParameters(T baseSearch)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (PropertyInfo prop in baseSearch.GetType().GetProperties())
            {
                Type type = prop.PropertyType;
                var name = prop.Name;
                var value = prop.GetValue(baseSearch, null);
                //nếu param dạng list thì convert to string. lưu ý value khác null mới convert được.
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && value != null)
                {
                    List<object> result = ((IEnumerable)value).Cast<object>().ToList();
                    string arrayString = string.Join(",", result.ToArray());
                    sqlParameters.Add(new SqlParameter(name, arrayString));
                }
                else
                {
                    sqlParameters.Add(new SqlParameter(name, value));
                }
            }
            SqlParameter[] parameters = sqlParameters.ToArray();
            return parameters;
        }

        public virtual E GetById(Guid id)
        {
            return GetById(id, (IConfigurationProvider)null);
        }

        public virtual E GetById(Guid id, Expression<Func<E, E>> select)
        {
            var query = Queryable.Where(e => e.Deleted == false);
            if (select != null)
            {
                query = query.Select(select);
            }
            return query
                 .AsNoTracking()
                 .Where(e => e.Id == id)
                 .FirstOrDefault();
        }

        public IList<E> Get(Expression<Func<E, bool>> expression)
        {
            return Get(new Expression<Func<E, bool>>[] { expression });
        }

        public IList<E> Get(Expression<Func<E, bool>> expression, Expression<Func<E, E>> select)
        {
            return Get(new Expression<Func<E, bool>>[] { expression }, select);
        }

        public IList<E> Get(Expression<Func<E, bool>> expression, IConfigurationProvider mapperConfiguration)
        {
            if (mapperConfiguration == null)
            {
                return Get(expression);
            }
            else
            {
                return Get(new Expression<Func<E, bool>>[] { expression }, mapperConfiguration);
            }
        }

        public virtual async Task<bool> SaveAsync(E item)
        {
            return await SaveAsync(new List<E> { item });
        }

        public virtual async Task<bool> CreateAsync(E item)
        {
            return await CreateAsync(new List<E> { item });
        }

        public virtual async Task<bool> CreateAsync(IList<E> items)
        {
            foreach (var model in items)
            {
                foreach (PropertyInfo item in model.GetType().GetProperties())
                {
                    var value = item.GetValue(model);
                    var typeofItem = item.PropertyType.GenericTypeArguments.FirstOrDefault();
                    if (typeofItem == typeof(Boolean))
                    {
                        item.SetValue(model, value ?? false);
                    }
                    else if (typeofItem == typeof(Int32) || item.PropertyType == typeof(Double))
                    {
                        item.SetValue(model, value ?? 0);
                    }
                    else if (item.PropertyType == typeof(String))
                    {
                        item.SetValue(model, value ?? "");
                    }
                }
                if (LoginContext.Instance.CurrentUser != null)
                {
                    model.CreatedBy = LoginContext.Instance.CurrentUser.userId;
                }
                await unitOfWork.Repository<E>().CreateAsync(model);
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public virtual async Task<bool> UpdateAsync(E item)
        {
            return await UpdateAsync(new List<E> { item });
        }

        public async Task<bool> UpdateAsync(IList<E> items)
        {
            foreach (var model in items)
            {
                var entity = await Queryable
                 .AsNoTracking()
                 .Where(e => e.Id == model.Id && e.Deleted == false)
                 .FirstOrDefaultAsync();

                if (entity != null)
                {
                    foreach (PropertyInfo item_old in entity.GetType().GetProperties())
                    {
                        foreach (PropertyInfo item_new in model.GetType().GetProperties())
                        {
                            if (item_old.Name == item_new.Name)
                            {
                                var value_old = item_old.GetValue(entity);
                                var value_new = item_new.GetValue(model);
                                if (value_old != value_new)
                                {
                                    item_old.SetValue(entity, value_new ?? value_old);
                                }
                                break;
                            }
                        }
                    }
                    entity.UpdatedBy = LoginContext.Instance.CurrentUser == null ? Guid.Empty : LoginContext.Instance.CurrentUser.userId;
                    unitOfWork.Repository<E>().Update(entity);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        /// <summary>
        /// Cập nhật theo field
        /// </summary>
        /// <param name="item"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFieldAsync(E item, params Expression<Func<E, object>>[] includeProperties)
        {
            return await UpdateFieldAsync(new List<E> { item }, includeProperties);
        }

        /// <summary>
        /// Cập nhật theo field
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFieldAsync(IList<E> items, params Expression<Func<E, object>>[] includeProperties)
        {
            foreach (var item in items)
            {
                var exists = await Queryable
                 .AsNoTracking()
                 .Where(e => e.Id == item.Id && e.Deleted == false)
                 .FirstOrDefaultAsync();

                if (exists != null)
                {
                    exists = mapper.Map<E>(item);
                    unitOfWork.Repository<E>().UpdateFieldsSave(exists, includeProperties);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> SaveAsync(IList<E> items)
        {
            foreach (var item in items)
            {
                var exists = await Queryable
                 .AsNoTracking()
                 .Where(e => e.Id == item.Id && e.Deleted == false)
                 .FirstOrDefaultAsync();

                if (exists != null)
                {
                    exists = mapper.Map<E>(item);
                    unitOfWork.Repository<E>().Update(exists);
                }
                else
                {
                    await unitOfWork.Repository<E>().CreateAsync(item);
                }
            }
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var exists = Queryable
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
            if (exists != null)
            {
                exists.Deleted = true;
                unitOfWork.Repository<E>().Update(exists);
                await unitOfWork.SaveAsync();
                return true;
            }
            else
            {
                throw new Exception(id + " not exists");
            }
        }

        /// <summary>
        /// Xoá dữ liệu có trả về data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<E> DeleteDataAsync(Guid id)
        {
            E exists = Queryable.AsNoTracking().FirstOrDefault(e => e.Id == id);
            if (exists != null)
            {
                exists.Deleted = true;
                unitOfWork.Repository<E>().Update(exists);
                await unitOfWork.SaveAsync();
                return exists;
            }
            throw new Exception(id + " not exists");
        }

        public async Task<IList<E>> GetAllAsync()
        {
            return await Queryable.AsNoTracking().ToListAsync();
        }

        public virtual async Task<E> GetByIdAsync(Guid id)
        {
            return await Queryable.Where(e => e.Id == id && e.Deleted == false).AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<E> GetByIdAsync(Guid id, Expression<Func<E, E>> select)
        {
            var query = unitOfWork.Repository<E>()
               .GetQueryable()
               .Where(e => e.Deleted == false)
               .AsNoTracking();
            if (select != null)
            {
                query = query.Select(select);
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IList<E>> GetAllAsync(Expression<Func<E, E>> select)
        {
            return await Queryable
                .Select(select)
                .ToListAsync();
        }

        public async Task<IList<E>> GetAsync(Expression<Func<E, bool>> expression, Expression<Func<E, E>> select)
        {
            return await unitOfWork.Repository<E>()
                 .GetQueryable()
                 .Where(expression)
                 .Select(select)
                 .ToListAsync();
        }

        public async Task<IList<E>> GetAsync(Expression<Func<E, bool>> expression)
        {
            return await unitOfWork.Repository<E>()
                .GetQueryable()
                .Where(expression)
                .ToListAsync();
        }

        public async Task<IList<E>> GetAsync(Expression<Func<E, bool>> expression, bool useProjectTo)
        {
            if (useProjectTo)
                return await unitOfWork.Repository<E>()
                .GetQueryable()
                .ProjectTo<E>(mapper.ConfigurationProvider)
                .Where(expression)
                .ToListAsync();
            return await unitOfWork.Repository<E>()
                .GetQueryable()
                .ProjectTo<E>(mapper.ConfigurationProvider)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<E> GetSingleAsync(Expression<Func<E, bool>> expression, Expression<Func<E, E>> select)
        {
            return await unitOfWork.Repository<E>()
                 .GetQueryable()
                 .Where(expression)
                 .Select(select)
                 .FirstOrDefaultAsync();
        }

        public async Task<E> GetSingleAsync(Expression<Func<E, bool>> expression)
        {
            return await unitOfWork.Repository<E>()
                .GetQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async Task<E> GetSingleAsync(Expression<Func<E, bool>> expression, bool useProjectTo)
        {
            if (useProjectTo)
                return await unitOfWork.Repository<E>()
                .GetQueryable()
                .ProjectTo<E>(mapper.ConfigurationProvider)
                .Where(expression)
                .FirstOrDefaultAsync();
            return await unitOfWork.Repository<E>()
                .GetQueryable()
                .ProjectTo<E>(mapper.ConfigurationProvider)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public E GetById(Guid id, IConfigurationProvider mapperConfiguration)
        {
            var queryable = Queryable.Where(e => e.Deleted == false && e.Id == id);
            if (mapperConfiguration != null)
                queryable = queryable.ProjectTo<E>(mapperConfiguration);
            return queryable.AsNoTracking().FirstOrDefault();
        }

        public virtual async Task<E> GetByIdAsync(Guid id, IConfigurationProvider mapperConfiguration)
        {
            var queryable = Queryable.Where(e => e.Deleted == false && e.Id == id);
            if (mapperConfiguration != null)
                queryable = queryable.ProjectTo<E>(mapperConfiguration);
            return await queryable.AsNoTracking().FirstOrDefaultAsync();
        }

        public IList<E> Get(Expression<Func<E, bool>>[] expressions, Expression<Func<E, E>> select)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            if (select != null)
                queryable = queryable.Select(select);
            return queryable.ToList();
        }

        public IList<E> Get(Expression<Func<E, bool>>[] expressions)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            return queryable.ToList();
        }

        public IList<E> Get(Expression<Func<E, bool>>[] expressions, IConfigurationProvider mapperConfiguration)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            queryable = queryable
                .ProjectTo<E>(mapperConfiguration);
            return queryable.ToList();
        }

        public async Task<IList<E>> GetAsync(Expression<Func<E, bool>>[] expressions, Expression<Func<E, E>> select)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            if (select != null)
            {
                queryable = queryable.Select(select);
            }
            return await queryable.ToListAsync();
        }

        public async Task<IList<E>> GetAsync(Expression<Func<E, bool>>[] expressions)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }

            return await queryable.ToListAsync();
        }

        public async Task<IList<E>> GetAsync(Expression<Func<E, bool>>[] expressions, bool useProjectTo)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            if (useProjectTo)
            {
                queryable = queryable.ProjectTo<E>(mapper.ConfigurationProvider);
            }
            return await queryable.ToListAsync();
        }

        public async Task<E> GetSingleAsync(Expression<Func<E, bool>>[] expressions, Expression<Func<E, E>> select)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            if (select != null)
            {
                queryable = queryable.Select(select);
            }
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<E> GetSingleAsync(Expression<Func<E, bool>>[] expressions)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }

            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<E> GetSingleAsync(Expression<Func<E, bool>>[] expressions, bool useProjectTo)
        {
            var queryable = Queryable.Where(e => e.Deleted == false);
            foreach (var expression in expressions)
            {
                queryable = queryable.Where(expression);
            }
            if (useProjectTo)
            {
                queryable = queryable.ProjectTo<E>(mapper.ConfigurationProvider);
            }
            return await queryable.FirstOrDefaultAsync();
        }

        public virtual async Task<string> GetExistItemMessage(E item)
        {
            return await Task.Run(() =>
            {
                List<string> messages = new List<string>();
                string result = string.Empty;
                if (messages.Any())
                    result = string.Join(" ", messages);
                return result;
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class MappingDataTable
    {
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch
                        {
                        }
                    }
                }
                return objT;
            }).ToList();
        }

    }
}

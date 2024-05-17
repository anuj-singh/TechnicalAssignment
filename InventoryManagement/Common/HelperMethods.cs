using System.Data;

namespace InventoryManagement.Common
{
    public static class HelperMethods
    {
        /// <summary>
        /// Converts the data table object to Product list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
            public static List<T> ConvertToList<T>(DataTable dt)
            {
                var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
                var properties = typeof(T).GetProperties();
                return dt.AsEnumerable().Select(row => {
                    var objT = Activator.CreateInstance<T>();
                    foreach (var prop in properties)
                    {
                        if (columnNames.Contains(prop.Name.ToLower()))
                        {
                            try
                            {
                                if (prop.Name.Equals("Quantity",StringComparison.OrdinalIgnoreCase))
                                {
                                    prop.SetValue(objT, Convert.ToInt32(row[prop.Name]));
                                }
                                else if (prop.Name.Equals("CreatedDate", StringComparison.OrdinalIgnoreCase))
                                {
                                    prop.SetValue(objT,DateOnly.FromDateTime(Convert.ToDateTime(row[prop.Name])));
                                }
                                else if (prop.Name.Equals("UpdatedDate", StringComparison.OrdinalIgnoreCase))
                                {
                                    prop.SetValue(objT, DateOnly.FromDateTime(Convert.ToDateTime(row[prop.Name])));
                                }
                                else
                                {
                                    prop.SetValue(objT, row[prop.Name]);
                                }
                            }
                            catch (Exception ex) {
                                throw;
                            }
                        }
                    }
                    return objT;
                }).ToList();
            }
    }
}

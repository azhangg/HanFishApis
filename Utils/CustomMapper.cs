using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Utils
{
    public static class CustomMapper
    {
        public static T Map<T>(T t, object data) where T : class
        {
            try
            {
                var result = t;
                foreach (var propertyP in result.GetType().GetProperties())
                {
                    foreach (var propertyS in data.GetType().GetProperties())
                    {
                        if (propertyP.Name.Equals(propertyS.Name) && propertyP.PropertyType.Equals(propertyS.PropertyType))
                        {
                            propertyP.SetValue(result, propertyS.GetValue(data));
                        }
                    }
                }
                return result;
            }
            catch {
                throw new CustomException("类型映射错误");
            }
        }
    }
}

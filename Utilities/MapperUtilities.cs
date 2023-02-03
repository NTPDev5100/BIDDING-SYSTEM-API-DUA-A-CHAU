using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class MapperUtilities
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
(this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreCreatedMap<TSource, TDestination>
(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            foreach (var property in sourceType.GetProperties())
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];
                NoMapCreatedAttribute attribute = (NoMapCreatedAttribute)descriptor.Attributes[typeof(NoMapCreatedAttribute)];
                if(attribute != null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreUpdatedMap<TSource, TDestination>
(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            foreach (var property in sourceType.GetProperties())
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];
                NoMapUpdatedAttribute attribute = (NoMapUpdatedAttribute)descriptor.Attributes[typeof(NoMapUpdatedAttribute)];
                if (attribute != null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }

        private static void IgnoreUnmappedProperties(TypeMap map, IMappingExpression expr)
        {
            foreach (string propName in map.GetUnmappedPropertyNames())
            {
                if (map.SourceType.GetProperty(propName) != null)
                {
                    expr.ForMember(propName, x => x.Ignore());
                }
                if (map.DestinationType.GetProperty(propName) != null)
                {
                    expr.ForMember(propName, opt => opt.Ignore());
                }
            }
        }

        //public static void IgnoreUnmapped(this IProfileExpression profile)
        //{
        //    profile.ForAllMaps(IgnoreUnmappedProperties);
        //}

        //public static void IgnoreUnmapped(this IProfileExpression profile, Func<TypeMap, bool> filter)
        //{
        //    profile.ForAllMaps((map, expr) =>
        //    {
        //        if (filter(map))
        //        {
        //            IgnoreUnmappedProperties(map, expr);
        //        }
        //    });
        //}

        //public static void IgnoreUnmapped(this IProfileExpression profile, Type src, Type dest)
        //{
        //    profile.IgnoreUnmapped((TypeMap map) => map.SourceType == src && map.DestinationType == dest);
        //}

        //public static void IgnoreUnmapped<TSrc, TDest>(this IProfileExpression profile)
        //{
        //    profile.IgnoreUnmapped(typeof(TSrc), typeof(TDest));
        //}

    }
}

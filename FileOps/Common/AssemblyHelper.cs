using System;
using System.Collections.Generic;
using System.Linq;

namespace FileOps.Common
{
    internal static class AssemblyHelper
    {
        public static IEnumerable<Type> GetDerivedTypesFor(params Type[] baseTypes)
        {
            IEnumerable<Type> recognisedTypes =
                baseTypes.SelectMany(baseType =>
               AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes())
               .Where(baseType.IsAssignableFrom));

            return recognisedTypes;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FileOps.Common
{
    internal static class CollectionExtensions
    {
        public static IList<T> AsOneItemList<T>(this T item)
        {
            return item == null ? new List<T>() : new List<T> { item };
        }


        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }


        public static IList<T> ThrowExceptionIfNull<T>(this IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException($"{nameof(list)}");
            return list.ToList();
        }

    }
}

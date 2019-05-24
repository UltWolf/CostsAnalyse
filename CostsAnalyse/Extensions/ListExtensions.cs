using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Extensions
{
    public  static class ListExtensions
    {
        public static void Add<T>(this List<T> ourList, List<T> anotherList) {
            foreach (var item in anotherList) {
                ourList.Add(item);
            }
        }

    }
}

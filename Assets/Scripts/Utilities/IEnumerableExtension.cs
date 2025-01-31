using System.Collections.Generic;

namespace Project.Utilities{
    public static class IEnumerableExtension{
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> add){
            foreach(T ele in add){
                list.Add(ele);
            }
        }
    }
}
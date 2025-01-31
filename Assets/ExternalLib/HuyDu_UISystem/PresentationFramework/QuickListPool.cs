using System.Collections.Generic;
namespace HuyDu_UISystem
{
    public static class QuickListPool<T>
    {
        private static readonly Stack<List<T>> s_pool = new();
        public static List<T> Get() => s_pool.Count > 0 ? s_pool.Pop() : new List<T>();
        public static void Release(List<T> list){
            if(list != null){
                list.Clear();
                s_pool.Push(list);
            }
        }
    }
}
namespace Gemano.PWA.Common.Helpers.Extensions
{
    public static class ListExtensions
    {
        public static T Push<T>(this List<T> list, T item)
        {
            list.Add(item);

            return item;
        }   
    }
}

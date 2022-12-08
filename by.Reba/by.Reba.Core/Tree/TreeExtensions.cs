namespace by.Reba.Core.Tree
{
    public static class TreeExtensions
    {
        public static ITree<T> ToTree<T>(this IList<T> items, Func<T, T, bool> parentSelector)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector(parent, item)),
                child => child);

            return Tree<T>.FromLookup(lookup);
        }
    }
}

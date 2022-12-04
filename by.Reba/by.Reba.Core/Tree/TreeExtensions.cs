namespace by.Reba.Core.Tree
{
    public static class TreeExtensions
    {
        public static IEnumerable<TNode> Flatten<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> childrenSelector)
        {
            return nodes == null
                ? throw new ArgumentNullException(nameof(nodes))
                : nodes.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(nodes);
        }
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

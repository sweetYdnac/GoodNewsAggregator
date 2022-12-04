namespace by.Reba.Core.Tree
{
    public class Tree<T> : ITree<T>
    {
        public T Data { get; }
        public ITree<T> Parent { get; private set; }
        public IList<ITree<T>> Children { get; private set; }
        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children.Count == 0;
        public int Level => IsRoot ? 0 : Parent.Level + 1;
        private Tree(T data)
        {
            Children = new List<ITree<T>>();
            Data = data;
        }
        public static Tree<T> FromLookup(ILookup<T, T> lookup)
        {
            var rootData = lookup.Count == 1 ? lookup.First().Key : default;
            var root = new Tree<T>(rootData);
            root.LoadChildren(lookup);
            return root;
        }
        private void LoadChildren(ILookup<T, T> lookup)
        {
            foreach (var data in lookup[Data])
            {
                var child = new Tree<T>(data) { Parent = this };
                Children.Add(child);
                child.LoadChildren(lookup);
            }
        }

        public ITree<T> OrderBy<TKey>(Func<ITree<T>, TKey> keySelector)
        {
            Children = Children.OrderBy(keySelector).ToList();

            foreach (var item in Children)
            {
                item.OrderBy(keySelector);
            }

            return this;
        }

        public ITree<T> OrderByDescending<TKey>(Func<ITree<T>, TKey> keySelector)
        {
            Children = Children.OrderByDescending(keySelector).ToList();

            foreach (var item in Children)
            {
                item.OrderByDescending(keySelector);
            }

            return this;
        }
    }
}
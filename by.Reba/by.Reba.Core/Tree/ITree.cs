namespace by.Reba.Core.Tree
{
    public interface ITree<T>
    {
        T Data { get; }
        ITree<T> Parent { get; }
        IList<ITree<T>> Children { get; }
        bool IsRoot { get; }
        bool IsLeaf { get; }
        int Level { get; }
        ITree<T> OrderBy<TKey>(Func<ITree<T>, TKey> keySelector);
        ITree<T> OrderByDescending<TKey>(Func<ITree<T>, TKey> keySelector);
    }
}

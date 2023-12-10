using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WtfDll.Utils
{
    /// <summary>
    /// Defines the <see cref="ObservableCollectionExtensions" />.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// The RemoveMany.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="collection">The collection<see cref="ObservableCollection{T}"/>.</param>
        /// <param name="itemsToRemove">The itemsToRemove<see cref="IEnumerable{T}"/>.</param>
        public static void RemoveMany<T>(this ObservableCollection<T> collection, IEnumerable<T> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}

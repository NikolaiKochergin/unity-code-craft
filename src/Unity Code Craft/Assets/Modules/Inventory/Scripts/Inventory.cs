using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modules.Inventories
{
    public class Inventory : IEnumerable<Item>
    {
        private readonly Dictionary<Item, Vector2Int> _items = new();
        private readonly Item[,] _matrix;
        private readonly Vector2Int _size;
        
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width => _size.x;
        public int Height => _size.y;
        public int Count => _items.Count;

        public Inventory(int width, int height)
        {
            if(width < 1 || height < 1)
                throw new ArgumentException();
            
            _matrix = new Item[width, height];
            _size = new Vector2Int(width, height);
        }

        public Inventory(
            int width,
            int height,
            params KeyValuePair<Item, Vector2Int>[] items
        ) : this(width, height)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (KeyValuePair<Item, Vector2Int> item in items) 
                AddItem(item.Key, item.Value);
        }

        public Inventory(
            int width,
            int height,
            params Item[] items
        ) : this(width, height)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));
            
            foreach (Item item in items)
                if(FindFreePosition(item, out Vector2Int position))
                    AddItem(item, position);
        }

        public Inventory(
            int width,
            int height,
            IEnumerable<KeyValuePair<Item, Vector2Int>> items
        ) : this(width, height)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));
            
            foreach (KeyValuePair<Item, Vector2Int> item in items)
                AddItem(item.Key, item.Value);
        }

        public Inventory(
            int width,
            int height,
            IEnumerable<Item> items
        ) : this(width, height)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (Item item in items)
                if(FindFreePosition(item, out Vector2Int position))
                    AddItem(item, position);
        }

        /// <summary>
        /// Creates new inventory 
        /// </summary>
        public Inventory(Inventory inventory) 
            : this(
                inventory?.Width ?? throw new ArgumentNullException(nameof(inventory)), 
                inventory.Height)
        {
            foreach (KeyValuePair<Item, Vector2Int> item in inventory._items) 
                AddItem(item.Key, item.Value);
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(Item item, Vector2Int position)
        {
            if(item == null || _items.ContainsKey(item))
                return false;
            
            if(item.Size.x < 1 || item.Size.y < 1)
                throw new ArgumentException();
            
            if(!IsFitsInside(position, item.Size, _size))
                return false;
            
            return IsFreeSpace(position, item.Size);
        }

        public bool CanAddItem(Item item, int startX, int startY)
        {
            return CanAddItem(item, new Vector2Int(startX, startY));
        }

        /// <summary>
        /// Adds an item on a specified position
        /// </summary>
        public bool AddItem(Item item, Vector2Int position)
        {
            if(item == null || !CanAddItem(item, position) || !_items.TryAdd(item, position))
                return false;

            for (int i = position.x, k = position.x + item.Size.x; i < k; i++)
            for (int j = position.y, l = position.y + item.Size.y; j < l; j++)
                _matrix[i, j] = item;
            
            OnAdded?.Invoke(item, position);
            return true;
        }

        public bool AddItem(Item item, int startX, int startY)
        {
            return AddItem(item, new Vector2Int(startX, startY));
        }

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(Item item)
        {
            return item != null && !_items.ContainsKey(item) && FindFreePosition(item, out _);
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(Item item)
        {
            return FindFreePosition(item, out Vector2Int position) && AddItem(item, position);
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(Item item, out Vector2Int position)
        {
            if (item == null)
            {
                position = default;
                return false;
            }
            
            return FindFreePosition(item.Size, out position);
        }

        public bool FindFreePosition(Vector2Int size, out Vector2Int position)
        {
            position = default;

            if (size.x < 1 || size.y < 1)
                throw new ArgumentException();
            
            if (size.x > Width || size.y > Height)
                return false;

            for (int i = 0, k = Height - size.y; i <= k; i++)
            for (int j = 0, l = Width - size.x; j <= l; j++)
            {
                position = new Vector2Int(j, i);
                if (IsFreeSpace(position, size))
                    return true;
            }

            position = new Vector2Int();
            return false;
        }

        public bool FindFreePosition(int sizeX, int sizeY, out Vector2Int position)
        {
            return FindFreePosition(new Vector2Int(sizeX, sizeY), out position);
        }

        private bool IsFreeSpace(Vector2Int origin, Vector2Int size)
        {
            for (int x = origin.x, k = origin.x + size.x; x < k; x++)
            for (int y = origin.y, l = origin.y + size.y; y < l; y++)
                if (_matrix[x, y] != null)
                    return false;

            return true;
        }

        /// <summary>
        /// Checks if the specified element exists
        /// </summary>
        public bool Contains(Item item)
        {
            return item != null && _items.ContainsKey(item);
        }

        /// <summary>
        /// Checks if the specified position is occupied
        /// </summary>
        public bool IsOccupied(Vector2Int position)
        {
            return IsPointInside(position, Vector2Int.zero, _size) && _matrix[position.x, position.y] != null;
        }

        public bool IsOccupied(int x, int y)
        {
            return IsOccupied(new Vector2Int(x, y));
        }

        /// <summary>
        /// Checks if the specified position is free
        /// </summary>
        public bool IsFree(Vector2Int position)
        {
            return !IsOccupied(position);
        }

        public bool IsFree(int x, int y)
        {
            return IsFree(new Vector2Int(x, y));
        }

        /// <summary>
        /// Removes specified item
        /// </summary>
        public bool RemoveItem(Item item)
        {
            return RemoveItem(item, out _);
        }

        public bool RemoveItem(Item item, out Vector2Int position)
        {
            position = default;
            if (item == null || !_items.Remove(item, out position))
                return false;
            
            for (int x = position.x; x < position.x + item.Size.x; x++)
            for (int y = position.y; y < position.y + item.Size.y; y++)
                _matrix[x, y] = null;
            
            OnRemoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(Vector2Int position)
        {
            if (!IsPointInside(position, default, _size))
                throw new IndexOutOfRangeException();
            
            return _matrix[position.x, position.y];
        }

        public Item GetItem(int x, int y)
        {
            return GetItem(new Vector2Int(x, y));
        }

        public bool TryGetItem(Vector2Int position, out Item item)
        {
            item = null;
            if (!IsPointInside(position, default, _size))
                return false;
            
            item = GetItem(position);
            return item != null;
        }

        public bool TryGetItem(int x, int y, out Item item)
        {
            return TryGetItem(new Vector2Int(x, y), out item);
        }

        /// <summary>
        /// Returns positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(Item item)
        {
            if (item == null)
                throw new NullReferenceException(nameof(item));
            
            if (!_items.TryGetValue(item, out Vector2Int position))
                throw new KeyNotFoundException("Item not found");

            int width = item.Size.x;
            int height = item.Size.y;

            Vector2Int[] positions = new Vector2Int[width * height];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                positions[x * height + y] = new Vector2Int(position.x + x, position.y + y);

            return positions;
        }

        public bool TryGetPositions(Item item, out Vector2Int[] positions)
        {
            positions = null;
            if (item == null || !_items.ContainsKey(item))
                return false;
            
            positions = GetPositions(item);
            return positions.Length > 0;
        }

        /// <summary>
        /// Clears all items 
        /// </summary>
        public void Clear()
        {
            if(Count == 0)
                return;
            
            Array.Clear(_matrix, 0, _matrix.Length);
            _items.Clear();
            
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            int count = 0;
            foreach (KeyValuePair<Item, Vector2Int> item in _items)
                if (item.Key.Name == name)
                    count++;
            return count;
        }

        public bool MoveItem(Item item, Vector2Int newPosition)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            
            if (!_items.TryGetValue(item, out Vector2Int oldPosition) || !IsFitsInside(newPosition, item.Size, _size))
                return false;

            for (int x = oldPosition.x; x < oldPosition.x + item.Size.x; x++)
            for (int y = oldPosition.y; y < oldPosition.y + item.Size.y; y++)
                _matrix[x, y] = null;

            if (!IsFreeSpace(newPosition, item.Size))
            {
                for (int x = oldPosition.x; x < oldPosition.x + item.Size.x; x++)
                for (int y = oldPosition.y; y < oldPosition.y + item.Size.y; y++)
                    _matrix[x, y] = item;

                return false;
            }

            for (int x = newPosition.x; x < newPosition.x + item.Size.x; x++)
            for (int y = newPosition.y; y < newPosition.y + item.Size.y; y++)
                _matrix[x, y] = item;

            _items[item] = newPosition;
            OnMoved?.Invoke(item, newPosition);
            return true;
        }
        
        /// <summary>
        /// Rearranges an inventory space with max free slots 
        /// </summary>
        public void OptimizeSpace()
        {
            if (Count == 0)
                return;

            int count = _items.Count;
            Item[] buffer = ArrayPool<Item>.Shared.Rent(count);

            int i = 0;
            foreach (Item item in _items.Keys)
                buffer[i++] = item;

            Array.Sort(buffer, 0, count, ItemComparer.Instance);

            Array.Clear(_matrix, 0, _matrix.Length);
            _items.Clear();

            for (int n = 0; n < count; n++)
            {
                Item item = buffer[n];
                bool placed = false;

                for (int y = 0, k = Height - item.Size.y; y <= k && !placed; y++)
                for (int x = 0, l = Width - item.Size.x; x <= l && !placed; x++)
                {
                    Vector2Int pos = new(x, y);

                    if (IsFreeSpace(pos, item.Size))
                    {
                        _items[item] = pos;

                        for (int dx = 0; dx < item.Size.x; dx++)
                        for (int dy = 0; dy < item.Size.y; dy++)
                            _matrix[x + dx, y + dy] = item;

                        placed = true;
                    }
                }
            }

            ArrayPool<Item>.Shared.Return(buffer, clearArray: true);
        }
        
        private sealed class ItemComparer : IComparer<Item>
        {
            public static readonly ItemComparer Instance = new();

            public int Compare(Item a, Item b)
            {
                int areaA = a.Size.x * a.Size.y;
                int areaB = b.Size.x * b.Size.y;

                return areaB.CompareTo(areaA);
            }
        }

        /// <summary>
        /// Iterates by all items 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.Keys.GetEnumerator();
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _items.Keys.GetEnumerator();
        }

        /// <summary>
        /// Copies items to a specified matrix
        /// </summary>
        public void CopyTo(Item[,] matrix)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            for (int x = 0; x < matrix.GetLength(0); x++)
                matrix[x, y] = GetItem(x, y);
        }

        /// <summary>
        /// Returns an inventory matrix in string format
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Item item = GetItem(x, y);
                    sb.Append(item != null ? item.ToString() : "empty");
                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static bool IsPointInside(
            Vector2Int position,
            Vector2Int origin,
            Vector2Int size
            ) =>
            position.x >= origin.x &&
            position.y >= origin.y &&
            position.x < origin.x + size.x &&
            position.y < origin.y + size.y;
        
        private static bool IsFitsInside(
            Vector2Int itemPos, 
            Vector2Int itemSize,
            Vector2Int inventorySize
            ) =>
            itemPos.x >= 0 &&
            itemPos.y >= 0 &&
            itemPos.x + itemSize.x <= inventorySize.x &&
            itemPos.y + itemSize.y <= inventorySize.y;
    }
}
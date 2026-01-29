using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Modules.Inventories
{
    public class Inventory : IEnumerable<Item>
    {
        private readonly Dictionary<Item, Vector2Int> _items = new();
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
                _items.Add(item.Key, item.Value);
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
                    _items.Add(item, position);
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
                _items.Add(item.Key, item.Value);
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
                    _items.Add(item, position);
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
                _items.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(Item item, Vector2Int position)
        {
            if(item == null)
                return false;
            
            if(_items.ContainsKey(item))
                return false;
            
            if(item.Size.x < 1 || item.Size.y < 1)
                throw new ArgumentException();
            
            if(!IsFitsInside(position, item.Size, _size))
                return false;
            
            foreach (KeyValuePair<Item, Vector2Int> pair in _items)
                if(IsIntersects(position, item.Size, pair.Value, pair.Key.Size))
                    return false;
            
            return true;
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
            if(item == null)
                return false;

            if (!CanAddItem(item, position))
                return false;
            
            if (!_items.TryAdd(item, position)) 
                return false;
            
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
            if(item == null)
                return false;
            
            return !_items.ContainsKey(item) && FindFreePosition(item, out _);
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

            for (int i = 0; i <= Height - size.y; i++)
            for (int j = 0; j <= Width - size.x; j++)
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
            foreach (KeyValuePair<Item, Vector2Int> item in _items)
                if (IsIntersects(
                        origin, size,
                        item.Value, item.Key.Size))
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
            foreach (KeyValuePair<Item, Vector2Int> item in _items)
                if (position.x >= item.Value.x &&
                    position.x < item.Value.x + item.Key.Size.x &&
                    position.y >= item.Value.y &&
                    position.y < item.Value.y + item.Key.Size.y)
                    return true;

            return false;
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
            if (item == null)
                return false;
            
            if(!_items.Remove(item, out position))
                return false;
            
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
            
            foreach (KeyValuePair<Item, Vector2Int> item in _items)
                if (position.x >= item.Value.x &&
                    position.x < item.Value.x + item.Key.Size.x &&
                    position.y >= item.Value.y &&
                    position.y < item.Value.y + item.Key.Size.y)
                    return item.Key;

            return null;
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
            if (item == null)
                return false;

            if (!_items.ContainsKey(item))
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

        public bool MoveItem(Item item, Vector2Int position)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            
            if(!_items.ContainsKey(item))
                return false;
            
            if(!IsPointInside(position, default, _size - item.Size))
                return false;

            foreach (KeyValuePair<Item, Vector2Int> pair in _items)
            {
                if (Equals(item, pair.Key))
                    continue;
                
                if(IsIntersects(position, item.Size, pair.Value, pair.Key.Size))
                    return false;
            }
            
            _items[item] = position;
            OnMoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Rearranges an inventory space with max free slots 
        /// </summary>
        public void OptimizeSpace()
        {
            if(Count == 0)
                return;

            List<Item> items = new(_items.Keys);
            
            items.Sort((a, b) => (b.Size.x * b.Size.y).CompareTo(a.Size.x * a.Size.y));
            
            _items.Clear();
            foreach (Item item in items)
                if(FindFreePosition(item, out Vector2Int position))
                    _items.Add(item, position);
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
        
        private static bool IsIntersects(
            Vector2Int posA, Vector2Int sizeA,
            Vector2Int posB, Vector2Int sizeB
            ) =>
            posA.x < posB.x + sizeB.x && posA.x + sizeA.x > posB.x &&
            posA.y < posB.y + sizeB.y && posA.y + sizeA.y > posB.y;
    }
}
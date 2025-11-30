using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();

        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();
        public int Count => _items.Count;

        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }

        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;

        public IEnumerable<TodoItem> Find(string substring) =>
            _items.Where(i => i.Title.Contains(substring ?? string.Empty, StringComparison.OrdinalIgnoreCase));

        // === НОВОЕ: JSON-ПЕРСИСТЕНТНОСТЬ ===

        public void Save(string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(path, json);
        }

        public static TodoList Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл не найден: {path}");

            var json = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();

            var list = new TodoList();
            foreach (var item in items)
            {
                var newItem = new TodoItem(item.Title);
                if (item.IsDone) newItem.MarkDone();
                // Восстанавливаем Id
                newItem.Id = item.Id; // ← работает благодаря internal set
                list._items.Add(newItem);
            }

            return list;
        }
    }
}
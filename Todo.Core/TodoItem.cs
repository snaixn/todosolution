using System;
using System.Text.Json.Serialization;

namespace Todo.Core
{
    public class TodoItem
    {
        [JsonInclude]
        public Guid Id { get; internal set; } = Guid.NewGuid();

        [JsonInclude]
        public string Title { get; private set; }

        [JsonInclude]
        public bool IsDone { get; private set; }

        public TodoItem(string title)
        {
            Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
        }

        public void MarkDone() => IsDone = true;
        public void MarkUndone() => IsDone = false;

        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title is required", nameof(newTitle));
            Title = newTitle.Trim();
        }
    }
}
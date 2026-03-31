using System.Text.Json;
using TodoApi.Models;

namespace TodoApi.Repositories;

public class JsonTodoRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly object _lock = new();

    public JsonTodoRepository(IConfiguration configuration)
    {
        var dataDir = configuration["DataDirectory"] ?? Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(dataDir);
        _filePath = Path.Combine(dataDir, "todos.json");

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(new List<TodoItem>(), _jsonOptions));
        }
    }

    public IQueryable<TodoItem> GetAll()
    {
        return ReadAll().AsQueryable();
    }

    public TodoItem? GetById(int id)
    {
        return ReadAll().FirstOrDefault(t => t.Id == id);
    }

    public TodoItem Add(TodoItem item)
    {
        lock (_lock)
        {
            var items = ReadAll();
            item.Id = items.Count > 0 ? items.Max(t => t.Id) + 1 : 1;
            item.CreatedAt = DateTime.UtcNow;
            items.Add(item);
            WriteAll(items);
            return item;
        }
    }

    public TodoItem? Update(int id, TodoItem updated)
    {
        lock (_lock)
        {
            var items = ReadAll();
            var existing = items.FirstOrDefault(t => t.Id == id);
            if (existing is null) return null;

            existing.Title = updated.Title;
            existing.IsComplete = updated.IsComplete;
            WriteAll(items);
            return existing;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var items = ReadAll();
            var item = items.FirstOrDefault(t => t.Id == id);
            if (item is null) return false;

            items.Remove(item);
            WriteAll(items);
            return true;
        }
    }

    private List<TodoItem> ReadAll()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<TodoItem>>(json, _jsonOptions) ?? new List<TodoItem>();
    }

    private void WriteAll(List<TodoItem> items)
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(items, _jsonOptions));
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers;

public class TodoItemsController : ODataController
{
    private readonly JsonTodoRepository _repository;

    public TodoItemsController(JsonTodoRepository repository)
    {
        _repository = repository;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_repository.GetAll());
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        var item = _repository.GetById(key);
        if (item is null) return NotFound();
        return Ok(item);
    }

    public IActionResult Post([FromBody] TodoItem item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = _repository.Add(item);
        return Created(created);
    }

    public IActionResult Put(int key, [FromBody] TodoItem item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = _repository.Update(key, item);
        if (updated is null) return NotFound();
        return Updated(updated);
    }

    public IActionResult Patch(int key, [FromBody] Delta<TodoItem> delta)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var existing = _repository.GetById(key);
        if (existing is null) return NotFound();

        delta.Patch(existing);
        _repository.Update(key, existing);
        return Updated(existing);
    }

    public IActionResult Delete(int key)
    {
        var deleted = _repository.Delete(key);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

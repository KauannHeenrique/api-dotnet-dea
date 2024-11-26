using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoList.Data;
using TodoList.Models;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskModel>> Get()
    {
        return _context.tasks.ToList();
    }

    [HttpPost]
    public IActionResult Post(TaskModel tasks)
    {
        if (string.IsNullOrWhiteSpace(tasks.Description) || tasks.Description.Length < 10)
        {
            return BadRequest("Description precisa ter mais que 10 caracteres.");
        }

        _context.tasks.Add(tasks);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = tasks.Id }, tasks);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = _context.tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        _context.tasks.Remove(task);
        _context.SaveChanges();
        return NoContent();
    }
}
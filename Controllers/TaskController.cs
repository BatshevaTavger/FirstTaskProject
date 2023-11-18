// using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
using core.Interfaces;
using core.Models;
using Microsoft.AspNetCore.Authorization;


namespace core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize(Policy = "TaskManager")]
    public class TaskController : ControllerBase
    {
        private long userId;
        private ITaskService TaskService;
        public TaskController(ITaskService taskService)
        {
            this.TaskService=taskService;
            // this.userId = long.Parse(User.FindFirst("UserId")?.Value ?? "");
            var userIdClaim = User?.FindFirst("UserId")?.Value;
            this.userId = !string.IsNullOrEmpty(userIdClaim) && long.TryParse(userIdClaim, out var userIdParsed) ? userIdParsed : 0;

        }

        
        [HttpGet]
        [Authorize(Policy = "Agent")]
        public  ActionResult GetAll() =>
            NotFound();
        // TaskService.GetAll(userId);


        [HttpGet ("{id}")]
        public ActionResult<Task> Get(int id) 
        {   
           var Task = TaskService.Get(userId, id);

            if (Task == null)
                return NotFound();

            return Task;
        }  
        
        [HttpPost] 
        public IActionResult Create(Task Task)
        {
            TaskService.Add(userId, Task);
            return CreatedAtAction(nameof(Create), new {id=Task.Id}, Task);

        }

        [HttpPut("{id}")]
        public IActionResult Update(long userId, int id, Task Task)
        {
            if (id != Task.Id)
                return BadRequest();

            var existingTask = TaskService.Get(userId, id);
            if (existingTask is null)
                return  NotFound();

            TaskService.Update(userId, Task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var Task = TaskService.Get(userId, id);
            if (Task is null)
                return  NotFound();

            TaskService.Delete(userId, id);

            return Content(TaskService.Count(userId).ToString());
        }      
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using core.Models;
using core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using core.Services;


namespace core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskManagerController: ControllerBase
    {
        private List<TaskUser> users;
        public TaskManagerController()
        {
            users = new List<TaskUser>
            {
                new TaskUser { UserId = 1, Username = "a", Password = "a", TaskManager = true},
                new TaskUser { UserId = 2, Username = "b", Password = "b"},
                new TaskUser { UserId = 3, Username = "Yocheved", Password = "yyy#"}
            };
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] TaskUser User)
        {
            var dt = DateTime.Now;


            var user = users.FirstOrDefault(u =>
                u.Username == User.Username 
                && u.Password == User.Password
            );        

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "TaskManager" : "Agent"),
                new Claim("userId", user.UserId.ToString()),

            };

            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}
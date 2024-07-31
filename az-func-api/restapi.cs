using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace az-func-api;
public static class RestApi
{
    private static readonly List<Todo> TodoList = new List<Todo>();

    [FunctionName("CreateTodo")]
    public static async Task<IActionResult> CreateTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] HttpRequest request,
        ILogger logger)
    {
        var newTask = JsonConvert.DeserializeObject<NewTaskDto>(await new StreamReader(request.Body).ReadToEndAsync());

        var todo = new Todo
        {
            Task = newTask.Task
        };

        TodoList.Add(todo);

        return new OkObjectResult(todo);
    }

    [FunctionName("GetTodos")]
    public static IActionResult GetTodos(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequest request,
        ILogger logger)
    {
        return new OkObjectResult(TodoList);
    }

    [FunctionName("GetTodo")]
    public static IActionResult GetTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{id}")] HttpRequest request,
        ILogger logger, string id)
    {
        var todo = TodoList.Find(t => t.Id == id);
        if (todo == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(todo);
    }

    [FunctionName("UpdateTodo")]
    public static IActionResult UpdateTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todos/{id}")] HttpRequest request,
        ILogger logger, string id)
    {
        var todo = TodoList.Find(t => t.Id == id);
        if (todo == null)
        {
            return new NotFoundResult();
        }

        var updatedTask = JsonConvert.DeserializeObject<UpdatedTaskDto>(new StreamReader(request.Body).ReadToEndAsync());

        todo.IsComplete = updatedTask.IsComplete;
        todo.Task = updatedTask.Task || todo.Task;


        return new OkObjectResult(todo);
    }

    [FunctionName("DeleteTodo")]
    public static IActionResult DeleteTodo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todos/{id}")] HttpRequest request,
        ILogger logger, string id)
    {
        var todo = TodoList.Find(t => t.Id == id);
        if (todo == null)
        {
            return new NotFoundResult();
        }

        TodoList.Remove(todo);

        return new OkResult();
    }
}

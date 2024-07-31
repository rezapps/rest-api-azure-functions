using System;

namespace az-func-api;
public class Todo
{
    public string Id { get; set; } = Guid.NewGuid().ToString("n");
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string Task { get; set; }
    public bool IsComplete { get; set; } = false;
}


public class NewTaskDto
{
    public string Task { get; set; }
}

public class UpdatedTaskDto
{
    public string Task { get; set; }
    public bool IsComplete { get; set; }
}

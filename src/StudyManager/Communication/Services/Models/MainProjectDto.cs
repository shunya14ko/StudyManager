namespace Communication.Services.Models;

public class MainProjectDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public DateTime TimeInfo { get; set; } = DateTime.MinValue;
}
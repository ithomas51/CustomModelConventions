namespace InjectLoggerToServices.Entities;

public interface IHasLogger
{
    ILogger? Logger { get; set; }
}

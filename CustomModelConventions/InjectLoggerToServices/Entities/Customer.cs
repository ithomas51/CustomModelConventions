#region IHasLogger

namespace InjectLoggerToServices.Entities;

#endregion

#region CustomerIHasLogger
public class Customer : IHasLogger
{
    private string? _phoneNumber;

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            Logger?.LogInformation(1, "Updating phone number for {Name} from {PhoneNumber} {ValueValue}", Name, _phoneNumber, value);

            _phoneNumber = value;
        }
    }

    [NotMapped]
    public ILogger? Logger { get; set; }
}
#endregion

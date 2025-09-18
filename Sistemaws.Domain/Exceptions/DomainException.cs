namespace Sistemaws.Domain.Exceptions;

public class DomainException : Exception
{
    public Dictionary<string, string> Errors { get; }

    public DomainException(Dictionary<string, string> errors) : base("Erro de domínio")
    {
        Errors = errors;
    }

    public DomainException(string message) : base(message)
    {
        Errors = new Dictionary<string, string> { { "General", message } };
    }
}

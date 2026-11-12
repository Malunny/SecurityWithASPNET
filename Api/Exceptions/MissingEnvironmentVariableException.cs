namespace SecWithJWTeBearer.Exceptions;

public class MissingEnvironmentVariableException : Exception
{
    public string EnviromentVariable { get; }

    public MissingEnvironmentVariableException(string enviromentVariable) : base("Missing Environment Variable")
    {
        EnviromentVariable = enviromentVariable;
    }
}
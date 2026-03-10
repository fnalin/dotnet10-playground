namespace MyMediator.Core;

public sealed record ValidationError(string Property, string Message);
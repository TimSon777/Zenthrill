namespace Zenthrill.Application.Results;

public sealed record ValidationFailure(IDictionary<string, string[]> Errors);

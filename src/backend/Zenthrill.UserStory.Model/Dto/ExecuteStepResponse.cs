using Zenthrill.UserStory.Model.Models;

namespace Zenthrill.UserStory.Model.Dto;

public sealed class ExecuteStepResponse
{
    public required Fragment Fragment { get; set; }
    
    public required IEnumerable<Branch> OutputBranches { get; set; }
}
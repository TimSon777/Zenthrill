namespace Zenthrill.UserStory.WebAPI.Requests;

public sealed class ExecuteStepRequest
{
    public required Guid StoryInfoVersionId { get; set; }
   
    public required Guid? BranchId { get; set; }
}
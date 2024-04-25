using Zenthrill.UserStory.Model.Entities;

namespace Zenthrill.UserStory.Model.Dto;

public sealed class ExecuteStepRequest
{
   public required Guid StoryInfoVersionId { get; set; }
   
   public required Guid? BranchId { get; set; }

   public required User User { get; set; }
}
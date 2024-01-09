using Zenthrill.Domain.Entities;
using Zenthrill.Outbox.Core;

namespace Zenthrill.Application.OutboxMessages;

public sealed record CreateStoryEntrypointOutboxMessage(FragmentId FragmentId) : IOutboxMessage;
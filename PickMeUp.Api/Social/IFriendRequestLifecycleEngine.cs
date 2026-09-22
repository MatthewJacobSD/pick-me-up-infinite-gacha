namespace PickMeUp.Api.Social
{
    // ── Friend Request Lifecycle ───────────────────────
    // Manages Send, Accept, Decline, Cancel, Expire transitions.
    //
    // Each operation validates:
    //   - No block exists between either user
    //   - Not already friends (for Send)
    //   - No duplicate pending request (for Send)
    //   - Request exists and is pending (for Accept/Decline/Cancel)

    public interface IFriendRequestLifecycleEngine
    {
        Task SendAsync(string senderId, string receiverId);
        Task AcceptAsync(string receiverId, string senderId);
        Task DeclineAsync(string receiverId, string senderId);
        Task CancelAsync(string senderId, string receiverId);
        Task ExpireAsync(string senderId, string receiverId);
    }

    public sealed class FriendRequestLifecycleEngine(ISocialRepository repo) : IFriendRequestLifecycleEngine
    {
        private readonly ISocialRepository _repo = repo;

        // 1. Verify no block exists in either direction.
        // 2. Verify not already friends.
        // 3. Verify no duplicate pending request.
        // 4. Create and persist the request.
        public async Task SendAsync(string senderId, string receiverId)
        {
            if (await _repo.IsBlockedAsync(receiverId, senderId))
                throw new InvalidOperationException("Receiver has blocked sender.");

            if (await _repo.IsBlockedAsync(senderId, receiverId))
                throw new InvalidOperationException("Sender has blocked receiver.");

            if (await _repo.AreFriendsAsync(senderId, receiverId))
                throw new InvalidOperationException("Already friends.");

            if (await _repo.HasPendingRequestAsync(senderId, receiverId))
                throw new InvalidOperationException("Request already pending.");

            var request = new FriendRequest
            {
                FromUserId = senderId,
                ToUserId = receiverId,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddFriendRequestAsync(request);
        }

        // 1. Verify request exists and is pending.
        // 2. Verify no block exists in either direction.
        // 3. Mark request as accepted and create friendship.
        public async Task AcceptAsync(string receiverId, string senderId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                throw new InvalidOperationException("No pending request to accept.");

            if (await _repo.IsBlockedAsync(receiverId, senderId))
                throw new InvalidOperationException("Receiver has blocked sender.");

            if (await _repo.IsBlockedAsync(senderId, receiverId))
                throw new InvalidOperationException("Sender has blocked receiver.");

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Accepted);
            await _repo.AddFriendAsync(senderId, receiverId);
        }

        // 1. Verify request exists and is pending.
        // 2. Mark request as declined.
        public async Task DeclineAsync(string receiverId, string senderId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                throw new InvalidOperationException("No pending request to decline.");

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Declined);
        }

        // 1. Verify request exists and is pending.
        // 2. Mark request as cancelled.
        public async Task CancelAsync(string senderId, string receiverId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                throw new InvalidOperationException("No pending request to cancel.");

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Cancelled);
        }

        // 1. Verify request exists and is pending.
        // 2. Mark request as expired (silent no-op if already resolved).
        public async Task ExpireAsync(string senderId, string receiverId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                return;

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Expired);
        }
    }
}

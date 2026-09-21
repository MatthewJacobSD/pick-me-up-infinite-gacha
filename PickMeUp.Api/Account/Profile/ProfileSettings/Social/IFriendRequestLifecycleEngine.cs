namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // Manages the friend request lifecycle: Send, Accept, Decline, Cancel, Expire.
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

        public async Task SendAsync(string senderId, string receiverId)
        {
            // Block enforcement
            if (await _repo.IsBlockedAsync(receiverId, senderId))
                throw new InvalidOperationException("Receiver has blocked sender.");

            if (await _repo.IsBlockedAsync(senderId, receiverId))
                throw new InvalidOperationException("Sender has blocked receiver.");

            // Already friends?
            if (await _repo.AreFriendsAsync(senderId, receiverId))
                throw new InvalidOperationException("Already friends.");

            // Existing pending request?
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

        public async Task AcceptAsync(string receiverId, string senderId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                throw new InvalidOperationException("No pending request to accept.");

            // Block enforcement
            if (await _repo.IsBlockedAsync(receiverId, senderId))
                throw new InvalidOperationException("Receiver has blocked sender.");

            if (await _repo.IsBlockedAsync(senderId, receiverId))
                throw new InvalidOperationException("Sender has blocked receiver.");

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Accepted);
            await _repo.AddFriendAsync(senderId, receiverId);
        }

        public async Task DeclineAsync(string receiverId, string senderId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                throw new InvalidOperationException("No pending request to decline.");

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Declined);
        }

        public async Task CancelAsync(string senderId, string receiverId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                throw new InvalidOperationException("No pending request to cancel.");

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Cancelled);
        }

        public async Task ExpireAsync(string senderId, string receiverId)
        {
            var request = await _repo.GetFriendRequestAsync(senderId, receiverId);

            if (request is null || request.Status != FriendRequestStatus.Pending)
                return; // nothing to expire

            await _repo.UpdateFriendRequestStatusAsync(senderId, receiverId, FriendRequestStatus.Expired);
        }
    }
}

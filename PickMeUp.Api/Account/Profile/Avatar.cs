namespace PickMeUp.Api.Account.Profile
{
    // Avatar value object with Default, Static, and Custom types.

    public sealed class Avatar
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public AvatarTypeStatus AvatarType { get; set; } = AvatarTypeStatus.Default;
        public string Value { get; set; } = string.Empty;
        public string AvatarUrlPath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDefault { get; set; } = true;

        // Parameterless constructor for MongoDB deserialization.
        private Avatar() { }

        private Avatar(string value, string pathUrl, AvatarTypeStatus type)
        {
            Value = value;
            AvatarUrlPath = pathUrl;
            AvatarType = type;
            IsDefault = type == AvatarTypeStatus.Default;
        }

        public static Avatar Create(string value, string pathUrl, AvatarTypeStatus type = AvatarTypeStatus.Default)
        {
            if (string.IsNullOrWhiteSpace(pathUrl))
            {
                throw new ArgumentException("Avatar URL path cannot be empty");
            }

            return new Avatar(value, pathUrl, type);
        }

        public enum AvatarTypeStatus
        {
            Default = 0,
            Static = 1,
            Custom = 2,
        }
    }
}

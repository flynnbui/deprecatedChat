namespace ChatApp.Core.Models
{
    public class KeyBundle
    {
        public Guid UserId { get; set; }
        public string IdentityKey { get; set; }
        public int RegistrationId { get; set; }
        public PreKey PreKey { get; set; }
        public SignedPreKey SignedPreKey { get; set; }
    }

    public class PreKey
    {
        public int KeyId { get; set; }
        public string PublicKey { get; set; }
    }

    public class SignedPreKey
    {
        public int KeyId { get; set; }
        public string PublicKey { get; set; }
        public string Signature { get; set; }
    }
}

namespace WasteWatcherApp.Firebase
{
    public class FirebaseAuthentificationReply
    {
        public string idToken { get; set; }
        string email { get; set; }
        string refreshToken { get; set; }
        int expiresIn { get; set; }
        string localId { get; set; }
    }
}

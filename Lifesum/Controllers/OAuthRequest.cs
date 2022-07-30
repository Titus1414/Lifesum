namespace Lifesum.Controllers
{
    internal class OAuthRequest
    {
        public string Method { get; set; }
        public object Type { get; set; }
        public object SignatureMethod { get; set; }
        public object ConsumerKey { get; set; }
        public object ConsumerSecret { get; set; }
        public object RequestUrl { get; set; }
        public string CallbackUrl { get; set; }
    }
}
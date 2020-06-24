namespace GEO
{
    internal class Intent
    {
        private object actionView;
        private object streetViewUri;

        public Intent(object actionView, object streetViewUri)
        {
            this.actionView = actionView;
            this.streetViewUri = streetViewUri;
        }

        public static object ActionView { get; internal set; }
    }
}
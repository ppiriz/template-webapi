namespace webApi.template.tests.Integration
{
    class Settings
    {
        private Settings() { }


        public string SiteURL { get; private set; }

        private static Settings _instance;
        public static Settings Instance
        {
            get { return _instance ?? (_instance = new Settings() { SiteURL = "http://localhost:63300" }); }
        }
    }


}

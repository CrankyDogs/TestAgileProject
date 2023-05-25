namespace TestProjLarge.Entities
{
    public class NavApiConfig
    {

        public string NavApiBaseUrl { get; set; }

        public string NavPath { get; set; }

        public string NavCompanyId { get; set; }

        public string NavCompanyName{ get; set; }

        public string LiveCompanyId{ get; set; }

        public bool IsNtlmAuthentication { get; set; }
        public bool IsDev { get; set; }

        public string NavUserName { get; set; }
        public string NavPassword { get; set; }
    }
}

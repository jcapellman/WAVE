namespace WAVE.Objects
{

    public class Rootobject
    {
        public string CVE_data_type { get; set; }
        public string CVE_data_format { get; set; }
        public string CVE_data_version { get; set; }
        public string CVE_data_numberOfCVEs { get; set; }
        public string CVE_data_timestamp { get; set; }
        public CVE_Items[] CVE_Items { get; set; }
    }

    public class CVE_Items
    {
        public Cve cve { get; set; }
        public Configurations configurations { get; set; }
        public Impact impact { get; set; }
        public string publishedDate { get; set; }
        public string lastModifiedDate { get; set; }
    }

    public class Cve
    {
        public string data_type { get; set; }
        public string data_format { get; set; }
        public string data_version { get; set; }
        public CVE_Data_Meta CVE_data_meta { get; set; }
        public Problemtype problemtype { get; set; }
        public References references { get; set; }
        public Description1 description { get; set; }
    }

    public class CVE_Data_Meta
    {
        public string ID { get; set; }
        public string ASSIGNER { get; set; }
    }

    public class Problemtype
    {
        public Problemtype_Data[] problemtype_data { get; set; }
    }

    public class Problemtype_Data
    {
        public Description[] description { get; set; }
    }

    public class Description
    {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class References
    {
        public Reference_Data[] reference_data { get; set; }
    }

    public class Reference_Data
    {
        public string url { get; set; }
        public string name { get; set; }
        public string refsource { get; set; }
        public string[] tags { get; set; }
    }

    public class Description1
    {
        public Description_Data[] description_data { get; set; }
    }

    public class Description_Data
    {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class Configurations
    {
        public string CVE_data_version { get; set; }
        public Node[] nodes { get; set; }
    }

    public class Node
    {
        public string _operator { get; set; }
        public Cpe_Match[] cpe_match { get; set; }
        public Child[] children { get; set; }
    }

    public class Cpe_Match
    {
        public bool vulnerable { get; set; }
        public string cpe23Uri { get; set; }
        public string versionStartIncluding { get; set; }
        public string versionEndExcluding { get; set; }
        public string versionEndIncluding { get; set; }
        public string versionStartExcluding { get; set; }
    }

    public class Child
    {
        public string _operator { get; set; }
        public Cpe_Match1[] cpe_match { get; set; }
    }

    public class Cpe_Match1
    {
        public bool vulnerable { get; set; }
        public string cpe23Uri { get; set; }
        public string versionStartIncluding { get; set; }
        public string versionEndExcluding { get; set; }
        public string versionEndIncluding { get; set; }
        public string versionStartExcluding { get; set; }
    }

    public class Impact
    {
        public Basemetricv3 baseMetricV3 { get; set; }
        public Basemetricv2 baseMetricV2 { get; set; }
    }

    public class Basemetricv3
    {
        public Cvssv3 cvssV3 { get; set; }
        public float exploitabilityScore { get; set; }
        public float impactScore { get; set; }
    }

    public class Cvssv3
    {
        public string version { get; set; }
        public string vectorString { get; set; }
        public string attackVector { get; set; }
        public string attackComplexity { get; set; }
        public string privilegesRequired { get; set; }
        public string userInteraction { get; set; }
        public string scope { get; set; }
        public string confidentialityImpact { get; set; }
        public string integrityImpact { get; set; }
        public string availabilityImpact { get; set; }
        public float baseScore { get; set; }
        public string baseSeverity { get; set; }
    }

    public class Basemetricv2
    {
        public Cvssv2 cvssV2 { get; set; }
        public string severity { get; set; }
        public float exploitabilityScore { get; set; }
        public float impactScore { get; set; }
        public bool acInsufInfo { get; set; }
        public bool obtainAllPrivilege { get; set; }
        public bool obtainUserPrivilege { get; set; }
        public bool obtainOtherPrivilege { get; set; }
        public bool userInteractionRequired { get; set; }
    }

    public class Cvssv2
    {
        public string version { get; set; }
        public string vectorString { get; set; }
        public string accessVector { get; set; }
        public string accessComplexity { get; set; }
        public string authentication { get; set; }
        public string confidentialityImpact { get; set; }
        public string integrityImpact { get; set; }
        public string availabilityImpact { get; set; }
        public float baseScore { get; set; }
    }

}

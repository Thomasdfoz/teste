using System.Collections.Generic;

namespace PackageManager.Data
{
    [System.Serializable]
    public class PackageInfo
    {
        public string name;
        public string version;
        public string displayName;
        public string description;
        public string unity;
        public string unityRelease;
        public Dictionary<string, string> dependencies;
        public AuthorInfo author;
        public string type;
    }

    [System.Serializable]
    public class AuthorInfo
    {
        public string name;
        public string email;
        public string url;
    }
}

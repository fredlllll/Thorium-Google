using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Thorium_Google
{
    public static class GoogleCloudStorageBackendConfig
    {
        public static string ProjectId { get; private set; }

        static GoogleCloudStorageBackendConfig()
        {
            Load();
        }

        public static void Load()
        {
            FileInfo assembly = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string configFile = Path.Combine(assembly.Directory.FullName, "google_cloud_storage_backend_config.json");

            JObject obj = JObject.Parse(File.ReadAllText(configFile));

            var token = obj["projectId"];
            if(token != null && token.Type != JTokenType.Null)
            {
                ProjectId = token.Value<string>();
            }
        }
    }
}

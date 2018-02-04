using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RustBindFixer
{
    public class Settings
    {
        private const string SETTINGSFILE = "Settings.json";
        private static readonly object LoadLock;

        public static Settings Instance { get; private set; }

        static Settings ( )
        {
            LoadLock = new object ( );
            Instance = new Settings ( );
            Load ( );
            Save ( );
        }

        public static void Save ( )
        {
            lock ( LoadLock )
                File.WriteAllText ( SETTINGSFILE, JsonConvert.SerializeObject ( Instance, Formatting.Indented ) );
        }

        public static void Load ( )
        {
            lock ( LoadLock )
            {
                if ( File.Exists ( SETTINGSFILE ) )
                {
                    var settings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
                    Instance = JsonConvert.DeserializeObject<Settings> ( File.ReadAllText ( SETTINGSFILE ), settings );
                }
            }
        }

        #region Properties

        public string KeysConfigPath { get; set; } =
            "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Rust\\cfg\\keys.cfg";

        public string LaunchParameters { get; set; } = "-high -malloc=system -nolog";

        public List<Server> Servers { get; set; } = new List<Server>
        {
            new Server
            {
                Name = "Example name",
                Ip = "xxx.xxx.xxx.xxx:00000",
                Description = "Example description",
                PresentInServerList = false
            }
        };

        #endregion Properties
    }
}
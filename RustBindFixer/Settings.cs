using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RustBindFixer
{
	public class Settings
	{
		private static readonly object LoadLock;

		public static Settings Instance { get; private set; }

		private const string SETTINGSFILE = "Settings.json";

		#region Properties

		public string KeysConfigPath { get; set; }

        public string LaunchParameters { get; set; }

        public List<Server> Servers { get; set; }

		#endregion Properties

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
				File.WriteAllText ( SETTINGSFILE, JsonConvert.SerializeObject ( Instance, Newtonsoft.Json.Formatting.Indented ) );
		}

		public static void Load ( )
		{
			lock ( LoadLock )
			{
				if ( File.Exists ( SETTINGSFILE ) )
					Instance = JsonConvert.DeserializeObject<Settings> ( File.ReadAllText ( SETTINGSFILE ) );
			}
		}
	}
}
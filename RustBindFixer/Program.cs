using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace RustBindFixer
{
    internal class Program
    {
        private static void Main ( string[] args )
        {
            var servers = Settings.Instance.Servers;

            for ( var i = 0; i < servers.Count; ++i ) 
                Console.WriteLine ( $"[{(i + 1)}] - {servers[i]}" );

            Console.Write ( $"\nConnect to server [1 - { servers.Count }]: " );
            var selectedServer = Convert.ToInt32 ( Console.ReadLine ( ) );
            if ( selectedServer >= 0 && selectedServer <= servers.Count )
                servers[selectedServer - 1].Connect ( );

            while ( !ProcessExists ( "RustClient" ) ) Thread.Sleep ( 60 );
            FixConfig ( );
        }


        private static bool ProcessExists ( string name ) => Process.GetProcessesByName ( name ).Length > 0;

        private static void FixConfig ( )
        {
            var fixedConfig = new List<string> ( );
            var rustConfig = File.ReadAllLines ( Settings.Instance.KeysConfigPath );

            const string pattern = @"bind (\w+) ""chat.say\s(?:\\+)?(.*)";
            var re = new Regex ( pattern );

            foreach ( var line in rustConfig )
            {
                string fixedCommand;
                if ( line.Contains ( "bind" ) && line.Contains ( "chat.say" ) )
                    fixedCommand = $"{re.Replace ( line, "bind $1 chat.say $2" ).TrimEnd ( '\\', '"' )}\"\\";
                else
                    fixedCommand = line;

                fixedConfig.Add ( fixedCommand );
            }


            File.Delete ( Settings.Instance.KeysConfigPath );
            File.WriteAllLines ( Settings.Instance.KeysConfigPath, fixedConfig );
        }
    }
}
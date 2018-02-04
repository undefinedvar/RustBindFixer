using System.Diagnostics;
using System.IO;

namespace RustBindFixer
{
    public class Server
    {
        public string Description { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }

        public bool PresentInServerList { get; set; } = true;

        public void Connect ( )
        {
            if ( string.IsNullOrEmpty ( Ip ) )
                throw new InvalidDataException ( "Server IP is invalid!" );

            Process.Start ( $"steam://connect/{Ip}", Settings.Instance.LaunchParameters );
        }

        public override string ToString ( ) => $"{Name,-50}{Ip,-32}{Description}";
    }
}
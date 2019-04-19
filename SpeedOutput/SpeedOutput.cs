using GTA;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SpeedOutput
{
    public class Main : Script
    {
        private bool active = false;
        private bool firstTime = true;
        private string ModName = "SpeedOutput";
        private UdpClient udpClient;
        private int port;
        private string hostname;
        private bool disposedValue = false;
        ScriptSettings Config;

        public Main()
        {
            Tick += onTick;
            KeyDown += onKeyDown;
            Interval = 1;
            LoadIniFile(@"scripts//SpeedOutput.ini");
            try
            {
                if (this.udpClient == null)
                {
                    this.udpClient = new UdpClient(hostname, port);
                    UI.Notify("Connected to " + hostname + " port " + port);
                }
            }
            catch
            {
                this.udpClient = null;
                UI.Notify("~r~Error~w~: Failed to establish client.");
            }
        }

        void LoadIniFile(string iniName)
        {
            try
            {
            Config = ScriptSettings.Load(iniName);
            this.port = Config.GetValue<int>("Configurations", "Port", 4915);
            this.hostname = Config.GetValue<String>("Configurations", "Hostname", "127.0.0.1");
            }
            catch (Exception e)
            {
                UI.Notify("~r~Error~w~: Config.ini Failed To Load.");
            }
        }

        private void onTick(object sender, EventArgs e)
        {
            if (firstTime)
            {
                UI.Notify(ModName + " loaded!");
                this.firstTime = false;
            }

            Ped player = Game.Player.Character;

            if (player.IsInVehicle())
            {
                if (active)
                {
                    Byte[] sendBytes = Encoding.UTF8.GetBytes(player.CurrentVehicle.Speed.ToString("000.000"));
                    this.udpClient.SendAsync(sendBytes,sendBytes.Length);
                }
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad0)
            {
                if (!active)
                {
                    UI.Notify("SpeedOutput: Activated");
                    this.active = !active;
                }
                else
                {
                    UI.Notify("SpeedOutput: Deactivated");
                    this.active = !active;
                }
            }
        }

        protected override void Dispose(bool A_0)
        {
            if (!disposedValue)
            {
                if (A_0)
                {
                    this.udpClient.Close();
                }
                this.disposedValue = true;
            }
        }
    }
}
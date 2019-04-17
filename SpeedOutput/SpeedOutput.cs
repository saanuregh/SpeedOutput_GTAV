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
        private int port = 4915;
        private bool disposedValue = false;

        public Main()
        {
            Tick += onTick;
            KeyDown += onKeyDown;
            Interval = 1;
            try
            {
                if (this.udpClient == null)
                {
                    this.udpClient = new UdpClient();
                    this.udpClient.Connect("127.0.0.1", port);
                }
            }
            catch
            {
                this.udpClient = null;
            }
        }

        private void onTick(object sender, EventArgs e)
        {
            if (firstTime)
            {
                UI.Notify(ModName + " loaded!");
                firstTime = false;
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
                    udpClient.Close();
                }
                disposedValue = true;
            }
        }
    }
}
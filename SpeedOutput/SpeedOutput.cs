﻿using System;
using System.Text;
using System.Windows.Forms;
using GTA;
using WatsonTcp;


namespace SpeedOutput
{
    public class Main : Script
    {
        bool active = false;
        bool listening = false;
        bool firstTime = true;
        string ModName = "SpeedOutput";
        WatsonTcpClient client = null;
        int port = 4915;

        public Main()
        {
            Tick += onTick;
            KeyDown += onKeyDown;
            Interval = 1;
        }

        private void onTick(object sender, EventArgs e)
        {
            if (firstTime)
            {
                UI.Notify(ModName + " loaded!");
                firstTime = false;
            }

            Player player = Game.Player;
            if (player != null && player.CanControlCharacter && player.IsAlive && player.Character != null)
            {
                if (player.Character.IsInVehicle())
                {
                    string speedString = player.Character.CurrentVehicle.Speed.ToString("0000.0000");

                    if (active)
                    {
                        if (!listening)
                        {
                            this.client = new WatsonTcpClient("127.0.0.1", port);
                            this.client.ServerConnected = ServerConnected;
                            this.client.ServerDisconnected = ServerDisconnected;
                            this.client.Debug = false;
                            this.client.Start();
                            this.listening = true;
                        }
                        this.client.Send(Encoding.UTF8.GetBytes(speedString + '\0'));
                    }
                }
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad0)
            {
                if (!active)
                {
                    UI.Notify("SpeedOutput Activated");
                    this.active = !active;
                }
                else
                {
                    UI.Notify("SpeedOutput Deactivated");
                    this.active = !active;
                }
            }
        }

        static bool ServerConnected()
        {
            UI.Notify("Server connected");
            return true;
        }

        static bool ServerDisconnected()
        {
            UI.Notify("Server disconnected");
            return true;
        }
    }
}
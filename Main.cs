// This file is part of iRacingPitCrew Application.
//
// Copyright 2014 Dean Netherton
// https://github.com/vipoo/iRacingPitCrew.Net
//
// iRacingPitCrew is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// iRacingPitCrew is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with iRacingPitCrew.  If not, see <http://www.gnu.org/licenses/>.

using iRacingPitCrew.Support;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace iRacingPitCrew
{
    public partial class Main : Form
    {
        PitCrew pitCrew;

        public Main()
        {
            var filename = string.Format("iRacingPitCrew-{0}.log", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));

            LogListener.ToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename));

            InitializeComponent();

            this.toolStripMenuItem_Open.Click += toolStripMenuItem_Open_Click;
            this.toolStripMenuItem_Exit.Click += toolStripMenuItem_Exit_Click;
            notifyIcon.Visible = false;

            var dc = new DataCollector();
            pitCrew = new PitCrew(dc);

            dc.Connected += dc_Connected;
            dc.Disconnected += dc_Disconnected;
        }

        void dc_Disconnected()
        {
            iRacingConnectionStatus.Text = "Disconnected";
        }

        void dc_Connected()
        {
            iRacingConnectionStatus.Text = "Connected";
        }

        void Main_Load(object sender, EventArgs e)
        {
            pitCrew.Start();
        }

        void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.Hide();
            }
        }

        void notifyIcon_Click(object sender, EventArgs e)
        {
            notifyIcon.ContextMenuStrip.Show();
        }

        void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            pitCrew.Stop();
        }
    }
}

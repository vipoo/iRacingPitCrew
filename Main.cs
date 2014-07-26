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

using iRacingPitCrew.Properties;
using iRacingPitCrew.Support;
using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

namespace iRacingPitCrew
{
    public partial class Main : Form
    {
        PitCrew pitCrew;
        bool isChanging;
        private DataCollector dataCollector;

        public Main()
        {
            var filename = string.Format("iRacingPitCrew-{0}.log", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));

            LogListener.ToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename));

            InitializeComponent();

            this.toolStripMenuItem_Open.Click += toolStripMenuItem_Open_Click;
            this.toolStripMenuItem_Exit.Click += toolStripMenuItem_Exit_Click;
            notifyIcon.Visible = false;

            if (Settings.Default.CarConfigs == null)
            {
                Settings.Default.CarConfigs = new CarConfigurations();
                Settings.Default.Save();
            }

            dataCollector = new DataCollector(Settings.Default.CarConfigs);
            pitCrew = new PitCrew(dataCollector);

            Settings.Default.CarConfigs.Changed += CarConfigs_Changed;
            dataCollector.Connected += dc_Connected;
            dataCollector.Disconnected += dc_Disconnected;
            dataCollector.NewSessionData += dc_NewSessionData;
        }

        void CarConfigs_Changed(string carName)
        {
            if( carName == carListCombo.SelectedItem.ToString())
            {
                var config = Settings.Default.CarConfigs[carName];

                isChanging = true;
                raceDurationTextBox.Text = config.RaceDuration_Length.ToString();
                raceDurationInLapsButton.Checked = config.RaceDuration_Type == RaceType.Laps;
                raceDurationInMinutesButton.Checked = config.RaceDuration_Type == RaceType.Minutes;
                tankLimitTextBox.Text = config.TankSize.ToString();
                isChanging = false;
            }
        }

        void dc_NewSessionData(iRacingSDK.DataSample data)
        {
            var carPath = data.Telemetry.CamCar.CarPath;
            var configurations = Settings.Default.CarConfigs;
                        
            if (!configurations.ContainsKey(carPath))
            {
                configurations.Add(new CarConfiguration { CarName = carPath });
                Settings.Default.Save();

                carListCombo.Items.Add(carPath);
                carListCombo.SelectedItem = carPath;
            }
            else
            {
                carListCombo.SelectedItem = carPath;
            }
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

            var configurations = Settings.Default.CarConfigs;

            if (configurations == null)
            {
                configurations = Settings.Default.CarConfigs = new CarConfigurations();
                Settings.Default.Save();
            }

            foreach (var c in configurations)
                carListCombo.Items.Add(c.CarName);

            carListCombo.SelectedItem = Settings.Default.CurrentCarName;
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

        CarConfiguration SelectedCarConfiguration
        {
            get
            {
                var configurations = Settings.Default.CarConfigs;
                return configurations[carListCombo.SelectedItem.ToString()];
            }
        }

        void carListCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var config = SelectedCarConfiguration;
            Settings.Default.CurrentCarName = config.CarName;
            Settings.Default.Save();
            
            tankLimitTextBox.Text = config.TankSize.ToString();

            isChanging = true;
            if (config.RaceDuration_IsEmpty)
            {
                raceDurationTextBox.Text = "";
                raceDurationInMinutesButton.Checked = raceDurationInLapsButton.Checked = false;
            }
            else
            {
                raceDurationTextBox.Text = config.RaceDuration_Length.ToString();
                raceDurationInLapsButton.Checked = config.RaceDuration_Type == RaceType.Laps;
                raceDurationInMinutesButton.Checked = config.RaceDuration_Type == RaceType.Minutes;
            }
            isChanging = false;
        }

        void SaveTextValue( TextBox textBox, Action<int?> assign)
        {
            int result;
            if (int.TryParse(textBox.Text, out result))
                assign(result);
            else
                assign(null);

            Settings.Default.Save();
        }

        void tankLimitTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveTextValue(tankLimitTextBox, r => SelectedCarConfiguration.TankSize = r);
        }

        void raceDurationTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveTextValue(raceDurationTextBox, r => {

                var rd = RaceDuration.From(SelectedCarConfiguration).ForLength(r);
                rd.WriteTo(SelectedCarConfiguration);
            });
        }

        void raceDurationTypeButton_CheckedChanged(object sender, EventArgs e)
        {
            if (isChanging)
                return;

            var newType = raceDurationInMinutesButton.Checked ? RaceType.Minutes : RaceType.Laps;
            SelectedCarConfiguration.RaceDuration_Type = newType;
            SelectedCarConfiguration.RaceDuration_IsEmpty = false;
            Settings.Default.Save();
        }
    }
}

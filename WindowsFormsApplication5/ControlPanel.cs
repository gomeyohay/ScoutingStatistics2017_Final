using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class ControlPanel : Form
    {
        private static string ROOT_FOLDER_TXT_FILE_PATH
        {
            get
            {
                return @"Scouting2630_RootFolder.txt";
            }
        }

        public ControlPanel()
        {
            InitializeComponent();
        }

        private void AllTeamsStatisticsBtn_Click(object sender, EventArgs e)
        {
            AllTeamsStatistics all_teams_statistics_form = new AllTeamsStatistics(false);
            all_teams_statistics_form.Show();
        }

        private void TeamStatisticsBtn_Click(object sender, EventArgs e)
        {
            int team_id;
            if (int.TryParse(TeamIdTextInput.Text, out team_id))
            {
                TeamStatistics team_statistics_form = new TeamStatistics(team_id);
                team_statistics_form.Show();
            }
        }

        
        private void TeamsStatisticsHtmlFileBtn_Click(object sender, EventArgs e)
        {
            AllTeamsStatistics all_teams_statistics_form = new AllTeamsStatistics(true);

            Process m_adb_process = new Process();
            ProcessStartInfo m_adb_process_startInfo = new ProcessStartInfo();
            m_adb_process_startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            m_adb_process_startInfo.FileName = @"adb.exe";
            m_adb_process_startInfo.Arguments = @" push D:/Scouting2017/ISR2/HtmlTables/table.html /storage/emulated/0/Scouting";
            m_adb_process_startInfo.RedirectStandardOutput = true;
            m_adb_process_startInfo.UseShellExecute = false;
            m_adb_process.StartInfo = m_adb_process_startInfo;
            try
            {
                m_adb_process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBox.Show("Successfully transfered HTML file to phone with path: /Scouting/table.html");
        }

        private void emptyDbBtn_Click(object sender, EventArgs e)
        {
            var user_pass = Prompt.ShowDialog(
                "Wow, hold your horses... You have to enter a very-secret super-secure password first!", 
                "Wait! Password?",
                true
                );

            if (user_pass == "ScouterDagan")
            {
                //var confirm = MessageBox.Show("Are you really really sure you want to empty the database? Are you certain you understand the consequences?", "U sure 'bout dat?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ShowConfirm(
                    "Are you really really sure you want to empty the database? Are you certain you understand the consequences?", 
                    "U sure 'bout dat?", 
                    MessageBoxIcon.Stop))
                {
                    if (ShowConfirm(
                        "Absolutely certain? This will delete EVERYTHING", 
                        "One last clarification...", 
                        MessageBoxIcon.Warning))
                    {
                        MessageBox.Show("Emptied successfully!");
                        LocalServerConn.EmptyLocalDB();
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect password! You lier!");
            }
            //LocalServerConn.EmptyLocalDB();
        }

        public static bool ShowConfirm(string p_msg, string p_title, MessageBoxIcon p_icon)
        {
            var confirm = MessageBox.Show(p_msg, p_title, MessageBoxButtons.YesNo, p_icon, MessageBoxDefaultButton.Button2);
            if (confirm == DialogResult.Yes)
            {
                return true;
            }

            return false;
        }

        private void compareBtn_Click(object sender, EventArgs e)
        {
            RadarChart radar_chart = new RadarChart(teamsToCompare.Text);
            radar_chart.Show();
        }

        private void teamsToCompare_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                compareBtn.PerformClick();
            }
        }

        private void TeamIdTextInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                TeamStatisticsBtn.PerformClick();
            }
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                LocalServerConn.root_folder_path = folderBrowserDialog1.SelectedPath;

                File.WriteAllText(ROOT_FOLDER_TXT_FILE_PATH, LocalServerConn.root_folder_path);

                rootFolderLbl.Text = LocalServerConn.root_folder_path;

                MessageBox.Show(string.Format("Root folder updated successfully! It is now: {0}.", LocalServerConn.root_folder_path), "Root Folder Updated");
            }
        }

        private void updatePropsBtn_Click(object sender, EventArgs e)
        {
            if (ShowConfirm("Are you sure you want to update the properties?", "Are You Sure?", MessageBoxIcon.Question))
            {
                LocalServerConn.UpdateProps();

                MessageBox.Show("Props were updated successfully!", "Props Updated");
            }
        }

        public static string GetRootFolder()
        {
            try
            {
                string root_folder = File.ReadAllText(ROOT_FOLDER_TXT_FILE_PATH);

                if (root_folder != string.Empty && root_folder != null)
                {
                    return root_folder;
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            if (LocalServerConn.root_folder_path == null)
            {
                MessageBox.Show("You must select a root folder first!", "Wait! Where..?");
                browseBtn.PerformClick();
            }

            rootFolderLbl.Text = LocalServerConn.root_folder_path;

            LocalServerConn.Init();
        }

        private void chooseDaysBtn_Click(object sender, EventArgs e)
        {
            string days = Prompt.ShowDialog("Please enter the days you want to consider in the statistics.", "Choose Which Days");

            string[] days_arr = days.Split(',');
            LocalServerConn.days_numbers = new List<int>();
            int day_num;
            foreach (string day in days_arr)
            {
                if (int.TryParse(day, out day_num))
                {
                    LocalServerConn.days_numbers.Add(day_num);
                }
            }

            daysLbl.Text = days;
        }
    }
}

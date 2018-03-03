using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class TeamStatistics : Form
    {
        DataGridView team_games_grid = new DataGridView();
        DataGridView pit_scouting_grid = new DataGridView();

        const int Super_Scouter_Props = 5;
        //static int HEATMAP_DATA_COL_INDEX = 14;//13;//6;

        public TeamStatistics()
        {
            InitializeComponent();
        }

        public TeamStatistics(int p_team_id)
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            this.Text = "Team #" + p_team_id + " Statistics";

            CreateTeamDataTables2(p_team_id);

            //Heatmap heatmap = new Heatmap(CreatePositionsList(p_team_id));
           // heatmap.Show();
        }

        public static List<KeyValuePair<Heatmap.Position, int>> CreatePositionsList(int p_team_id)
        {
            var team_games_data = LocalServerConn.GetTeamGSData(p_team_id);

            /*List<List<string>> team_games_data = new List<List<string>>();

            team_games_data.Add(new List<string>() { "17", "A4,A5" });
            team_games_data.Add(new List<string>() { "17", "C8" });
            team_games_data.Add(new List<string>() { "17", "B2,C7" });
            team_games_data.Add(new List<string>() { "17", "D4,A4" });
            team_games_data.Add(new List<string>() { "17", "A4" });*/

            List<KeyValuePair<Heatmap.Position, int>> list = new List<KeyValuePair<Heatmap.Position, int>>();
            //string pos;
            bool pos_exists;
            string[] split;
            int pos_index = 0;
            int heatmap_prop_id = Heatmap.GetHeatmapPropId();
            KeyValuePair<Heatmap.Position, int> pair;
            foreach (var game_data in team_games_data)
            {
                if (game_data[heatmap_prop_id + 1] != string.Empty)
                {
                    split = game_data[heatmap_prop_id + 1].Split(',');
                    foreach (var pos in split)
                    {
                        //pos = game_data[HEATMAP_DATA_COL_INDEX + 1];

                        pos_exists = false;
                        for (int i = 0; i < list.Count; i++)
                        {
                            pair = list[i];
                            if (pair.Key.ToString() == pos)
                            {
                                pos_exists = true;
                                pos_index = i;
                                break;
                            }
                        }

                        if (pos_exists)
                        {
                            list[pos_index] = new KeyValuePair<Heatmap.Position, int>(list[pos_index].Key, list[pos_index].Value + 1);
                        }
                        else
                        {
                            char col = pos[0];
                            int row = int.Parse(pos.Substring(1));
                            list.Add(new KeyValuePair<Heatmap.Position, int>(new Heatmap.Position(col, row), 1));
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// creates a single team's table including game scouting data and average performance and pit scouting data 
        /// </summary>
        /// <param name="p_team_id">team number</param>
        void CreateTeamDataTables(int p_team_id)
        {
            LocalServerConn.SetTableStyle(team_games_grid, this);
            LocalServerConn.SetTableStyle(pit_scouting_grid, this);


            // set games table column names
            var GS_props = LocalServerConn.GetGSProps();
            //team_games_grid.Columns.Add("מספר משחק", "מספר משחק");
            //team_games_grid.Columns[team_games_grid.ColumnCount - 1].HeaderCell.Style.BackColor = Color.DarkGreen;
            for (int i = 0; i < GS_props.Count; i++)
            {
                team_games_grid.Columns.Add(i.ToString(), GS_props[i]);
                //team_games_grid.Columns[team_games_grid.ColumnCount - 1].HeaderCell.Style.BackColor = Color.DarkGreen;
            }

            // insert data into games table 
            var TeamGamesData = LocalServerConn.GetTeamGSData(p_team_id);
            foreach (var game_data in TeamGamesData)
            {
                team_games_grid.Rows.Add(game_data.GetRange(1, game_data.Count - 1).ToArray());

                team_games_grid.Rows[team_games_grid.RowCount - 2].HeaderCell.Value = game_data[0];
              //team_games_grid.Rows[team_games_grid.RowCount - 2].HeaderCell.Style.BackColor = Color.DarkGreen;
            }

            // add "Average" row at the bottom of the games table
            var team_avg_row = LocalServerConn.GetTeamGSAvg(p_team_id);
            //team_avg_row.Insert(0, "Average");
            int games_grid_last_row_index = team_games_grid.Rows.GetLastRow(DataGridViewElementStates.None);
            team_games_grid.Rows[games_grid_last_row_index].SetValues(team_avg_row.ToArray());
            team_games_grid.Rows[games_grid_last_row_index].HeaderCell.Value = "AVG";


            // set pit scouting column names
            var PS_props = LocalServerConn.GetPSProps();
            foreach (var prop_name in PS_props)
            {
                pit_scouting_grid.Columns.Add(prop_name, prop_name);
                pit_scouting_grid.Columns[pit_scouting_grid.ColumnCount - 1].HeaderCell.Style.BackColor = Color.DarkGreen;
            }

            // insert data into pit scouting table
            var PS_data = LocalServerConn.GetTeamPSData(p_team_id);
            int pit_grid_last_row_index = pit_scouting_grid.Rows.GetLastRow(DataGridViewElementStates.None);
            pit_scouting_grid.Rows[pit_grid_last_row_index].SetValues(PS_data.ToArray());
            //pit_scouting_grid.Rows[pit_grid_last_row_index].HeaderCell.Style.BackColor = Color.Black;

            team_games_grid.TopLeftHeaderCell.Value = p_team_id.ToString();

            team_games_grid.SortCompare += Team_games_grid_SortCompare;
            team_games_grid.CellMouseClick += Team_games_grid_CellMouseClick;

            pit_scouting_grid.Top = team_games_grid.Height;

            //team_games_grid.Width = 500;

            // adding the tables to the window
            this.Controls.Add(team_games_grid);
            this.Controls.Add(pit_scouting_grid);
            //team_games_grid.ClearSelection();
        }

        void CreateTeamDataTables2(int p_team_id)
        {
            LocalServerConn.SetTableStyle(team_games_grid, this);
            LocalServerConn.SetTableStyle(pit_scouting_grid, this);


            // set games table column names
            var GS_props = LocalServerConn.GetGSProps();
            //team_games_grid.Columns.Add("מספר משחק", "מספר משחק");
            //team_games_grid.Columns[team_games_grid.ColumnCount - 1].HeaderCell.Style.BackColor = Color.DarkGreen;
            for (int i = 0; i < GS_props.Count; i++)
            {
                team_games_grid.Columns.Add(i.ToString(), GS_props[i]);
                //team_games_grid.Columns[team_games_grid.ColumnCount - 1].HeaderCell.Style.BackColor = Color.DarkGreen;
            }

            // insert data into games table 
            var TeamGamesData = LocalServerConn.DictionaryGetTeamGSData(p_team_id);
            foreach (var game_data in TeamGamesData)
            {
                String[] game_data_array = new String[GS_props.Count];
                int index, not_null = 0;
                int last_props = GS_props.Count + ((Super_Scouter_Props - 1) * 2 + 2);
                var key_array = game_data.Keys.ToArray();

                for (index = 0; index < last_props - (((Super_Scouter_Props) * 3) + 2); index++)
                {
                    String Prop_val;
                    if (game_data.TryGetValue(index+1,out Prop_val))
                    {
                        game_data_array[index] = Prop_val;
                        not_null++;
                    }
                    else
                    {
                        game_data_array[index] = null;
                    }
                }

                if (key_array[not_null + 1] < (last_props-3))
                {
                    for (int i = key_array[not_null + 1]; i < (key_array[not_null + 1] + Super_Scouter_Props - 1); index++, i++)
                    {
                        String Prop_val;
                        if (game_data.TryGetValue(i, out Prop_val))
                        {
                            game_data_array[index] = Prop_val;
                        }
                        else
                        {
                            game_data_array[index] = null;
                        }
                    }
                }
                else
                {
                    index += 4;
                }
                for (int i = last_props - 1; i < last_props+2; i++,index++)
                {
                    String Prop_val;
                    if (game_data.TryGetValue(i, out Prop_val))
                    {
                        game_data_array[index] = Prop_val;
                    }
                    else
                    {
                        game_data_array[index] = null;
                    }
                }
                team_games_grid.Rows.Add(game_data_array);

                team_games_grid.Rows[team_games_grid.RowCount - 2].HeaderCell.Value = game_data[0];
                //team_games_grid.Rows[team_games_grid.RowCount - 2].HeaderCell.Style.BackColor = Color.DarkGreen;
            }

            // add "Average" row at the bottom of the games table
            var team_avg_row = LocalServerConn.GetTeamGSAvg(p_team_id);
            //team_avg_row.Insert(0, "Average");
            int games_grid_last_row_index = team_games_grid.Rows.GetLastRow(DataGridViewElementStates.None);
            team_games_grid.Rows[games_grid_last_row_index].SetValues(team_avg_row.ToArray());
            team_games_grid.Rows[games_grid_last_row_index].HeaderCell.Value = "AVG";


            // set pit scouting column names
            var PS_props = LocalServerConn.GetPSProps();
            foreach (var prop_name in PS_props)
            {
                pit_scouting_grid.Columns.Add(prop_name, prop_name);
                pit_scouting_grid.Columns[pit_scouting_grid.ColumnCount - 1].HeaderCell.Style.BackColor = Color.DarkGreen;
            }

            // insert data into pit scouting table
            var PS_data = LocalServerConn.GetTeamPSData(p_team_id);
            int pit_grid_last_row_index = pit_scouting_grid.Rows.GetLastRow(DataGridViewElementStates.None);
            pit_scouting_grid.Rows[pit_grid_last_row_index].SetValues(PS_data.ToArray());
            //pit_scouting_grid.Rows[pit_grid_last_row_index].HeaderCell.Style.BackColor = Color.Black;

            team_games_grid.TopLeftHeaderCell.Value = p_team_id.ToString();

            team_games_grid.SortCompare += Team_games_grid_SortCompare;
            team_games_grid.CellMouseClick += Team_games_grid_CellMouseClick;

            pit_scouting_grid.Top = team_games_grid.Height;

            //team_games_grid.Width = 500;

            // adding the tables to the window
            this.Controls.Add(team_games_grid);
            this.Controls.Add(pit_scouting_grid);
            //team_games_grid.ClearSelection();
        }

        private void Team_games_grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int row_index = e.RowIndex;
                if (row_index == team_games_grid.RowCount - 1)
                {
                    return;
                }

                int col_index = e.ColumnIndex;

                bool is_row_header = (col_index == -1);
                bool is_col_header = (row_index == -1);
                
                if (is_row_header)
                {
                    if (is_col_header)
                    {
                        int old_team_num;
                        if (int.TryParse(team_games_grid.TopLeftHeaderCell.Value.ToString(), out old_team_num))
                        {
                            string new_team_num_str = Prompt.ShowDialog("Please enter the new, correct team number:", "Update Team Number");
                            int new_team_num;
                            if (int.TryParse(new_team_num_str, out new_team_num))
                            {
                                if (ControlPanel.ShowConfirm(string.Format("Are you sure you want to change ALL records of team number {0} into team {1}?", old_team_num, new_team_num), "Sure?", MessageBoxIcon.Warning))
                                {
                                    LocalServerConn.UpdateTeamNumber(old_team_num, new_team_num);
                                    MessageBox.Show("Team number updated successfully! Please restart the program to complete the change.", "Team Number Updated");
                                }
                            }
                        }
                    }
                    else
                    {
                        int old_game_num;
                        if (int.TryParse(team_games_grid.Rows[row_index].HeaderCell.Value.ToString(), out old_game_num))
                        {
                            string new_game_num_str = Prompt.ShowDialog("Please enter the new, correct game number:", "Update Game Number");
                            int new_game_num;
                            if (int.TryParse(new_game_num_str, out new_game_num))
                            {
                                if (ControlPanel.ShowConfirm(string.Format("Are you sure you want to change team {0}'s game number {1} into game {2}?", team_games_grid.TopLeftHeaderCell.Value.ToString(), old_game_num, new_game_num), "Sure?", MessageBoxIcon.Warning))
                                {
                                    LocalServerConn.UpdateGameNumber(old_game_num, new_game_num);
                                    MessageBox.Show("Game number updated successfully! Please restart the program to complete the change.", "Game Number Updated");
                                }
                            }
                        }
                        //MessageBox.Show(team_games_grid.Rows[row_index].HeaderCell.Value.ToString());
                    }
                }
                else if (!is_col_header)
                {
                    int team_num;
                    if (int.TryParse(team_games_grid.TopLeftHeaderCell.Value.ToString(), out team_num))
                    {
                        int game_num;
                        if (int.TryParse(team_games_grid.Rows[row_index].HeaderCell.Value.ToString(), out game_num))
                        {
                            int prop_num;
                            if (int.TryParse(team_games_grid.Columns[col_index].Name, out prop_num))
                            {
                                string old_prop_val = team_games_grid.Rows[row_index].Cells[col_index].Value.ToString();
                                string new_prop_val = Prompt.ShowDialog("Please enter the new, correct value:", "Update Property Value");
                                if (new_prop_val != null && new_prop_val != string.Empty)
                                {
                                    if (ControlPanel.ShowConfirm(string.Format("Are you sure you want to update this property value from {0} to {1}?", old_prop_val, new_prop_val), "Sure?", MessageBoxIcon.Warning))
                                    {
                                        LocalServerConn.UpdatePropValue(team_num, game_num, prop_num, new_prop_val);
                                        MessageBox.Show("Property value updated successfully! Please restart the program to complete the change.", "Property Value Updated");
                                    }
                                }
                            }
                        }
                    }
                }
            } 
        }

        /// <summary>
        /// ?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Team_games_grid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            LocalServerConn.TableColumn_SortCompare(sender, e, true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class AllTeamsStatistics : Form
    {
        DataGridView all_teams_grid = new DataGridView();
        
        public AllTeamsStatistics(bool p_create_html_file)
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            this.Text = "All Teams Statistics";

            HTMLTable table = new HTMLTable("AllTeamsStatistics");

            //all_teams_grid.Columns.Add("מספר קבוצה", "מספר קבוצה");
            List<string> gs_props = LocalServerConn.GetGSProps();
            //gs_props.Insert(0, "מספר קבוצה");
            foreach (var prop in gs_props)
            {
                all_teams_grid.Columns.Add(prop, prop);
            }

            List<string> html_headers = new List<string>(gs_props);
            html_headers.Insert(0, "מספר קבוצה");
            table.AddHeaderRow(html_headers);
            
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LocalServerConn.LOCAL_DB_CONN_STR))
            {
                var team_numbers = LocalDB.TeamScoutings
                    .GroupBy(g => g.team_id)
                    .Select(s => new
                    {
                        team_id = s.FirstOrDefault().team_id
                    })
                    .ToList();

                team_numbers = team_numbers.OrderBy(ob => ob.team_id).ToList(); 

                List<string> row, html_row;
                foreach (var data_row in team_numbers)
                {
                    row = new List<string>();
                    //row.Insert(0, data_row.team_id.ToString());
                    row.AddRange(LocalServerConn.GetTeamGSAvg(data_row.team_id));

                    all_teams_grid.Rows.Add(row.ToArray());

                    all_teams_grid.Rows[all_teams_grid.RowCount - 2].HeaderCell.Value = data_row.team_id.ToString();

                    html_row = new List<string>(row);
                    html_row.Insert(0, data_row.team_id.ToString());
                    table.AddRow(html_row);
                }

                LocalServerConn.SetTableStyle(all_teams_grid, this);

                all_teams_grid.AllowUserToAddRows = false;

                all_teams_grid.SortCompare += All_teams_grid_SortCompare;

                all_teams_grid.CellDoubleClick += All_teams_grid_CellDoubleClick;

                this.Controls.Add(all_teams_grid);


                if (p_create_html_file)
                {
                    string html =
                        @"<html>
	                    <head>
                            <style>body { font-family: 'Arial', Sans-Serif; } table { font-size: 3vw; }" + HtmlExtensions.PureTableCss + @"</style>
		                    <script type='text/javascript'>" + HtmlExtensions.SorttableJs + @"</script>
                        </head>
                        <body>"
                                + table.TableStr +
                            @"</body>
                    </html>";

                    //richTextBox1.Text = html;

                    var path = @"D:\Scouting2017\ISR2\HtmlTables\table.html";
                    File.WriteAllText(path, html);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void All_teams_grid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            LocalServerConn.TableColumn_SortCompare(sender, e, false);
        }

        

        /// <summary>
        /// when column is double clicked it is reordered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void All_teams_grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                TeamStatistics ts = new TeamStatistics(int.Parse(all_teams_grid.Rows[e.RowIndex].HeaderCell.Value.ToString()));//Cells[e.ColumnIndex].Value.ToString()));
                ts.Show();
            }
        }

        private void FilterBtn_Click(object sender, EventArgs e)
        {
            var filterTeamsStr = FilterTeamsTxt.Text;
            if (filterTeamsStr == "")
            {
                ShowAllRows();
                return;
            }

            string[] teamsStr = filterTeamsStr.Split(',');
            int[] teams = new int[teamsStr.Length];
            int index = 0;
            int parse;
            foreach (string teamStr in teamsStr)
            {
                if (int.TryParse(teamStr, out parse))
                {
                    teams[index] = parse;
                    index++;
                }
                else
                {
                    MessageBox.Show("Invalid team list!!1");
                    return;
                }
            }

            FilterTeams(teams);
        }

        private void ShowAllRows()
        {
            FilterTeams(new int[0]);
        }

        private void FilterTeams(int[] teams)
        {
            bool showAllTeams = teams.Length == 0;
            foreach (DataGridViewRow row in all_teams_grid.Rows)
            {
                if (showAllTeams || teams.Contains(int.Parse(row.HeaderCell.Value.ToString())))
                {
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }
        }
    }
}


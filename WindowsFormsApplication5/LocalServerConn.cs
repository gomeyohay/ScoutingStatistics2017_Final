using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApplication5
{
    class LocalServerConn
    {
        public static string DEFAULT_ROOT_FOLDER_PATH = @"D:\Scouting2017\CHAMP";
        public static string root_folder_path = ControlPanel.GetRootFolder();// DEFAULT_ROOT_FOLDER_PATH;

        public static string LOCAL_DB_CONN_STR
        {
            get
            {
                //return @"metadata=res://*/ScoutingProps.csdl|res://*/ScoutingProps.ssdl|res://*/ScoutingProps.msl;provider=System.Data.SQLite.EF6;provider connection string='data source = " + root_folder_path + @"\ScoutingData.db';";
                return @"metadata=res://*/ScoutingProps.csdl|res://*/ScoutingProps.ssdl|res://*/ScoutingProps.msl;provider=System.Data.SQLite.EF6;provider connection string='data source = " + root_folder_path + @"\ScoutingData.db';";
            }
        }

        public static string LOCAL_PROPS_DB_CONN_STR
        {
            get
            {
                return @"metadata=res://*/ScoutingProps.csdl|res://*/ScoutingProps.ssdl|res://*/ScoutingProps.msl;provider=System.Data.SQLite.EF6;provider connection string='data source = " + root_folder_path + @"\ScoutingProps.db';";
            }
        }

        public static string XML_PROPS_PATH
        {
            get
            {
                return root_folder_path + @"\ScoutingProps_He.xml";
            }
        }

        static int TABLE_WIDTH = 1350;
        static int TABLE_HEIGHT = 600;

        static string DAY_NUM_PROP_DESC
        {
            get
            {
                return "day_number";
            }
        }

        static List<string> GS_prop_types; //= LocalServerConn.GetGSPropTypes();
        public static List<string> column_types;

        public static List<int> days_numbers = new List<int>();

        private static int day_prop_id;

        public static void Init()
        {
            GS_prop_types = GetGSPropTypes();
            day_prop_id = GetGSPropDescs().IndexOf(DAY_NUM_PROP_DESC);
        }

        public static void SetTableStyle(DataGridView p_table, Form p_form)
        {
            p_table.TopLeftHeaderCell.Style.BackColor = Color.DarkGreen;
            p_table.GridColor = Color.Black;
            p_table.BackgroundColor = Color.White;
            p_table.ForeColor = Color.Black;
            p_table.DefaultCellStyle.BackColor = Color.Green;
            p_table.DefaultCellStyle.SelectionBackColor = Color.Black;//p_table.DefaultCellStyle.BackColor;
            p_table.DefaultCellStyle.SelectionForeColor = Color.Green;//p_table.DefaultCellStyle.ForeColor;
            p_table.DefaultCellStyle.Font = new Font("Arial", 17);
            p_table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            p_table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //p_table.AutoSize = true;
            p_table.RowHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            p_table.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 17);
            p_table.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            p_table.EnableHeadersVisualStyles = false;
            p_table.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            p_table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            p_table.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            p_table.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            p_table.EditMode = DataGridViewEditMode.EditProgrammatically;
            p_table.ScrollBars = ScrollBars.Both;
            p_table.Size = new Size(TABLE_WIDTH, TABLE_HEIGHT);//p_form.Width, p_form.Height);
            p_table.BorderStyle = BorderStyle.None;
            p_table.RowHeadersDefaultCellStyle.Font = new Font("Arial", 17);
        }

        public static void EmptyLocalDB()
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                LocalDB.AllianceScoutings.RemoveRange(LocalDB.AllianceScoutings);
                LocalDB.GameScoutings.RemoveRange(LocalDB.GameScoutings);
                LocalDB.TeamScoutings.RemoveRange(LocalDB.TeamScoutings);
                LocalDB.PitScoutings.RemoveRange(LocalDB.PitScoutings);
                LocalDB.regional_table.RemoveRange(LocalDB.regional_table);

                LocalDB.SaveChanges();
                /*LocalDB.Database.ExecuteSqlCommand("TRUNCATE TABLE AllianceScouting");
                LocalDB.Database.ExecuteSqlCommand("TRUNCATE TABLE GameScouting");
                LocalDB.Database.ExecuteSqlCommand("TRUNCATE TABLE TeamScouting");
                LocalDB.Database.ExecuteSqlCommand("TRUNCATE TABLE PitScouting");
                LocalDB.Database.ExecuteSqlCommand("TRUNCATE TABLE regional_table");*/
            }
        }

        /// <summary>
        /// filters game scouting prop names from props table
        /// </summary>
        /// <returns></returns>
        public static List<string> GetGSProps()
        {
            var doc = XDocument.Load(XML_PROPS_PATH);

            List<string> GS_props_names = new List<string>();

            var GS_names = doc
                .Descendants("TeamScoutingProps")
                .Descendants("prop")
                .Select(s => new
                {
                    txt = s.Attribute("name").Value
                })
                .ToList();

            foreach (var name in GS_names)
            {
                GS_props_names.Add(name.txt);
            }

            return GS_props_names;
        }

        /// <summary>
        /// filters game scouting prop names from prop table
        /// </summary>
        /// <returns></returns>
        public static List<string> GetGSPropDescs()
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_PROPS_DB_CONN_STR))
            {
                var GS_prop_descs = LocalDB.TeamScoutingProps
                    .OrderBy(ob => ob.prop_id)
                    .Select(s => s.prop_desc)
                    .ToList();

                return GS_prop_descs;
            }
        }

        /// <summary>
        /// filters game scouting prop types from prop table
        /// </summary>
        /// <returns></returns>
        public static List<string> GetGSPropTypes()
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_PROPS_DB_CONN_STR))
            {
                //var tmp = LocalDB.TeamScoutingProps.ToList();
                var GS_prop_types = LocalDB.TeamScoutingProps
                    .OrderBy(ob => ob.prop_id)
                    .Select(s => s.prop_type)
                    .ToList();

                return GS_prop_types;
            }
        }

        /// <summary>
        /// filters game scouting prop styles from prop table
        /// </summary>
        /// <returns></returns>
        public static List<string> GetGSPropStyles()
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                var GS_prop_styles = LocalDB.TeamScoutingProps
                    .OrderBy(ob => ob.prop_id)
                    .Select(s => s.prop_style)
                    .ToList();

                return GS_prop_styles;
            }
        }

        /// <summary>
        /// selects a single teams props data from table with all teams data and insets it into table
        /// </summary>
        /// <param name="p_team_id">team number</param>
        /// <returns></returns>
        public static List<List<string>> GetTeamGSData(int p_team_id)
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                List<List<string>> GS_props_data = new List<List<string>>();

                var relevant_data = GetRelevantDataByDays(LocalDB.TeamScoutings);

                var table_data = relevant_data
                    .Where(w => w.team_id == p_team_id)
                    .OrderBy(ob => ob.prop_id)
                    .ThenBy(tb => tb.game_id)
                    .GroupBy(gb => gb.game_id)
                    .ToList();

                List<List<string>> result = new List<List<string>>();
                List<string> result_row;
                foreach (var group in table_data)
                {
                    result_row = new List<string>();

                    result_row.Add(group.Key.ToString());
                    foreach (var prop_row in group.ToList())
                    {
                        result_row.Add(prop_row.prop_value);
                    }

                    result.Add(result_row);
                }

                return result;
            }
        }

        public static List<Dictionary<int,string>> DictionaryGetTeamGSData(int p_team_id)
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
 //               List<List<string>> GS_props_data = new List<List<string>>();

                var relevant_data = GetRelevantDataByDays(LocalDB.TeamScoutings);

                var table_data = relevant_data
                    .Where(w => w.team_id == p_team_id)
                    .OrderBy(ob => ob.prop_id)
                    .ThenBy(tb => tb.game_id)
                    .GroupBy(gb => gb.game_id)
                    .ToList();

                List<Dictionary<int,string>> result = new List<Dictionary<int, string>>();

                Dictionary <int,string> result_row;

                foreach (var group in table_data)
                {
                    result_row = new Dictionary<int, string>();
                    result_row.Add(0,group.Key.ToString());
                    foreach (var prop_row in group.ToList())
                    {
                        result_row.Add(prop_row.prop_id+1,prop_row.prop_value);
                    }

                    result.Add(result_row);
                }

                return result;
            }
        }

        public static void UpdateTeamNumber(int p_old_team_num, int p_new_team_num)
        {
            using (ScoutingPropsEntities localDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                var team_scouting = localDB.TeamScoutings.Where(w => w.team_id == p_old_team_num).ToList();
                team_scouting.ForEach(fe => fe.team_id = p_new_team_num);

                var alliance_scouting = localDB.AllianceScoutings.Where(w => w.team_id == p_old_team_num).ToList();
                alliance_scouting.ForEach(fe => fe.team_id = p_new_team_num);

                var pit_scouting = localDB.PitScoutings.Where(w => w.team_id == p_old_team_num).ToList();
                pit_scouting.ForEach(fe => fe.team_id = p_new_team_num);

                localDB.SaveChanges();
            }
        }

        public static void UpdateGameNumber(int p_old_game_num, int p_new_game_num)
        {
            using (ScoutingPropsEntities localDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                var team_scouting = localDB.TeamScoutings.Where(w => w.game_id == p_old_game_num).ToList();
                team_scouting.ForEach(fe => fe.game_id = p_new_game_num);

                var alliance_scouting = localDB.AllianceScoutings.Where(w => w.game_id == p_old_game_num).ToList();
                alliance_scouting.ForEach(fe => fe.game_id = p_new_game_num);

                var game_scouting = localDB.GameScoutings.Where(w => w.game_id == p_old_game_num).ToList();
                if (game_scouting.Count == 0)
                {
                    game_scouting.ForEach(fe => fe.game_id = p_new_game_num);
                }
                else
                {
                    localDB.GameScoutings.RemoveRange(game_scouting);
                }

                localDB.SaveChanges();
            }
        }

        /// <summary>
        /// calculates teams average performance in game props and inserts into a row int single teams table
        /// </summary>
        /// <param name="p_team_id">team number</param>
        /// <returns></returns>
        public static List<string> GetTeamGSAvg(int p_team_id)
        {
            int avg, sum;
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                var relevant_data = GetRelevantDataByDays(LocalDB.TeamScoutings);
                var stats = relevant_data
                    .Where(w => w.team_id == p_team_id)
                    .OrderBy(ob => ob.prop_id)
                    .ThenBy(tb => tb.game_id)
                    .GroupBy(g => g.prop_id)
                    .AsEnumerable()
                    .Select(s => new
                    {
                        PropId = s.FirstOrDefault().prop_id,
                        Average = s.Average(av => (int.TryParse(av.prop_value, out avg) ? avg : 0)),
                        Sum = s.Sum(sm => (int.TryParse(sm.prop_value, out sum) ? sum : 0)),
                        Count = s.Count(),
                        Other = s.FirstOrDefault().prop_value
                    })
                    .Join(
                        LocalDB.TeamScoutingProps,
                        ts => ts.PropId,
                        tsp => tsp.prop_id,
                        (ts, tsp) => new
                        {
                            PropId = ts.PropId,
                            Type = tsp.prop_type,
                            Average = ts.Average,
                            Sum = ts.Sum,
                            Count = ts.Count
                        }
                    ).ToList();

                List<string> result = new List<string>();

                //calculates average according to type => boolean = amount of trues out of all tries, number = mathematical average 
                foreach (var stat in stats)
                {
                    switch (stat.Type)
                    {
                        case "boolean":
                            string bool_stat = string.Format("{0:0.##}% | {1}/{2}", (stat.Average * 100), stat.Sum, stat.Count);
                            //string bool_stat = (stat.Average * 100).ToString().PadLeft(3, '_') + "% - " + stat.Sum + "/" + stat.Count;
                            result.Add(bool_stat);
                            break;

                        case "text":
                            result.Add("-");
                            break;

                        case "number":
                        default:
                            if (stat.Average == -1)
                            {
                                result.Add("-");
                            }
                            else
                            {
                                string num_stat = string.Format("{0:0.##}", stat.Average);
                                result.Add(num_stat);
                            }
                            break;
                    }
                }

                return result;
            }
        }

        public static void UpdatePropValue(int p_team_num, int p_game_num, int p_prop_num, string p_new_prop_val)
        {
            using (ScoutingPropsEntities localDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                var game_scouting = localDB.TeamScoutings.Where(w => w.team_id == p_team_num);
                game_scouting = game_scouting.Where(w => w.game_id == p_game_num);
                game_scouting = game_scouting.Where(w => w.prop_id == p_prop_num);

                game_scouting.ToList().ForEach(fe => fe.prop_value = p_new_prop_val);

                localDB.SaveChanges();
            }
        }

        /// <summary>
        /// filters pit scouting prop names from props table
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPSProps()
        {
            var doc = XDocument.Load(XML_PROPS_PATH);

            List<string> PS_props_names = new List<string>();

            var GS_names = doc
                .Descendants("PitScoutingProps")
                .Descendants("prop")
                .Select(s => new
                {
                    txt = s.Attribute("name").Value
                })
                .ToList();

            foreach (var name in GS_names)
            {
                PS_props_names.Add(name.txt);
            }

            return PS_props_names;
        }

        /// <summary>
        /// selects pit scouting prop data from PS_props table
        /// </summary>
        /// <param name="p_team_id">team number</param>
        /// <returns></returns>
        public static List<string> GetTeamPSData(int p_team_id)
        {
            using (ScoutingPropsEntities LocalDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                var PS_data = LocalDB.PitScoutings
                    .Where(w => w.team_id == p_team_id)
                    .OrderBy(ob => ob.prop_id)
                    .Select(s => s.prop_value).ToList();

                return PS_data;
            }
        }

        /// <summary>
        /// sorts values in order according to type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="treat_bool_as_num"></param>
        public static void TableColumn_SortCompare(object sender, DataGridViewSortCompareEventArgs e, bool treat_bool_as_num)
        {
            if (column_types == null)
            {
                column_types = new List<string>(GS_prop_types);
                //column_types.Insert(0, "number");
            }

            string type = column_types[e.Column.Index];
            if (treat_bool_as_num && type == "boolean")
            {
                type = "number";
            }
            /*else if (!treat_bool_as_num && type == "boolean")
            {
                type = "text";
            }*/

            switch (type)
            {
                case "number":
                    e.SortResult = double.Parse(e.CellValue1.ToString()).CompareTo(double.Parse(e.CellValue2.ToString()));
                    break;

                case "boolean":
                    //gets start and end of first value
                    string cv1 = e.CellValue1.ToString();
                    //int cv1_start_index = cv1.IndexOf('(') + 1;
                    //int cv1_end_index = cv1.IndexOf('%') - 1;

                    //gets start and end of second value 
                    string cv2 = e.CellValue2.ToString();
                    //int cv2_start_index = cv2.IndexOf('(') + 1;
                    //int cv2_end_index = cv2.IndexOf('%') - 1;

                    double percent1 = -1, percent2 = -1;
                    double.TryParse(cv1.Split('%')[0], out percent1);
                    double.TryParse(cv2.Split('%')[0], out percent2);

                    //values placements 
                    //string cv1_percent_num = cv1.Substring(cv1_start_index, cv1_end_index - cv1_start_index + 1);
                    //string cv2_percent_num = cv2.Substring(cv2_start_index, cv2_end_index - cv2_start_index + 1);

                    //compares values
                    //e.SortResult = double.Parse(cv1_percent_num).CompareTo(double.Parse(cv2_percent_num));
                    if (percent1 != -1 && percent2 != -1)
                    {
                        e.SortResult = percent1.CompareTo(percent2);
                    }
                    else
                    {
                        e.SortResult = cv1.CompareTo(cv2);
                    }
                    break;

                case "text":
                default:
                    e.SortResult = e.CellValue1.ToString().CompareTo(e.CellValue2.ToString());
                    break;
            }

            e.Handled = true;
        }

        public static void UpdateProps()
        {
            using (ScoutingPropsEntities localDB = new ScoutingPropsEntities(LOCAL_DB_CONN_STR))
            {
                using (ScoutingPropsEntities propsDB = new ScoutingPropsEntities(LOCAL_PROPS_DB_CONN_STR))
                {
                    localDB.TeamScoutingProps.RemoveRange(localDB.TeamScoutingProps);
                    localDB.PitScoutingProps.RemoveRange(localDB.PitScoutingProps);

                    localDB.TeamScoutingProps.AddRange(propsDB.TeamScoutingProps);
                    localDB.PitScoutingProps.AddRange(propsDB.PitScoutingProps);
                    
                    localDB.SaveChanges();
                }
            }
        }

        private static List<TeamScouting> GetRelevantDataByDays(System.Data.Entity.DbSet<TeamScouting> p_data)
        {
            if (days_numbers.Count == 0)
            {
                return p_data.ToList();
            }

            List<TeamScouting> day_rows = p_data.Where(w => w.prop_id == day_prop_id).ToList();

            List<TeamScouting> sorted_data = new List<TeamScouting>();
            List<TeamScouting> game_rows;
            int prop_val;
            foreach (var row in day_rows)
            {
                foreach (var day in days_numbers)
                {
                    if (int.TryParse(row.prop_value, out prop_val) && prop_val == day)
                    {
                        game_rows = p_data.Where(w => w.game_id == row.game_id).ToList();
                        sorted_data.AddRange(game_rows.Where(w => w.team_id == row.team_id));
                    }
                }
            }

            return sorted_data;
        }
    }
}

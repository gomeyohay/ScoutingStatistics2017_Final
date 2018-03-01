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
    public partial class RadarChart : Form
    {
        string[] SCOUTING_PROPS_FOR_COMPARE = new string[] { "number_of_gears", "has_climbed", "driver_performance" };
        int[] SCOUTING_PROP_SCORES_FOR_COMPARE = new int[] { 2, 2, 1};
        List<int> compare_prop_indexes;
        List<string> compare_prop_names;

        Color[] COLORS = new Color[] { Color.Blue, Color.Red, Color.Yellow, Color.Green, Color.Black };

        public RadarChart()
        {
            InitializeComponent();
        }

        public RadarChart(string p_teams_list)
        {
            InitializeComponent();

            var prop_descs = LocalServerConn.GetGSPropDescs();
            var prop_names = LocalServerConn.GetGSProps();
            compare_prop_indexes = new List<int>();
            compare_prop_names = new List<string>();
            int index;
            foreach (var prop_desc in SCOUTING_PROPS_FOR_COMPARE)
            {
                index = IndexOf(prop_descs, prop_desc);
                if (index != -1)
                {
                    compare_prop_indexes.Add(index);
                    compare_prop_names.Add(prop_names[index]);
                }
            }

            string[] teams_arr_str = p_teams_list.Split(',');
            List<int> teams_list = new List<int>();
            int team_num;
            List<KeyValuePair<int, List<float>>> all_teams_data = new List<KeyValuePair<int, List<float>>>();
            List<float> prop_max_val = new List<float>(compare_prop_indexes.Count);
            for (int i = 0; i < prop_max_val.Capacity; i++)
            {
                prop_max_val.Add(0);
            }
            foreach (var team_str in teams_arr_str)
            {
                if (int.TryParse(team_str, out team_num))
                {
                    var team_data = GetTeamScoutingCompareData(team_num);
                    if (team_data != null)
                    {
                        all_teams_data.Add(new KeyValuePair<int, List<float>>(team_num, team_data));
                        for (int i = 0; i < team_data.Count; i++)
                        {
                            if (prop_max_val[i] < team_data[i])
                            {
                                prop_max_val[i] = team_data[i];
                            }
                        }
                    }
                }
            }

            List<int> normalized_data;
            foreach (var team_data in all_teams_data)
            {
                chart.Series.Add(team_data.Key.ToString());
                chart.Series.Last().Color = Color.FromArgb(0, 0, 0, 0);
                chart.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                chart.Series.Last().BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                chart.Series.Last().BorderColor = COLORS[(chart.Series.Count - 1) % COLORS.Length];
                chart.Series.Last().BorderWidth = 2;

                normalized_data = new List<int>();
                for (int i = 0; i < team_data.Value.Count; i++)
                {
                    normalized_data.Add((int)(team_data.Value[i] * SCOUTING_PROP_SCORES_FOR_COMPARE[i]));
                    //normalized_data.Add((int)((float)team_data.Value[i] / prop_max_val[i] * 100));
                }
                chart.Series.Last().Points.DataBindXY(compare_prop_names, normalized_data);
            }
        }

        private int IndexOf(List<string> p_arr, string p_val)
        {
            int index = -1;
            for (int i = 0; i < p_arr.Count; i++)
            {
                if (p_arr[i] == p_val)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private List<float> GetTeamScoutingCompareData(int p_team_id)
        {
            var team_GS_data = LocalServerConn.GetTeamGSAvg(p_team_id);
            List<float> compare_data = new List<float>();

            if (team_GS_data.Count != 0)
            {
                float num;
                foreach (int index in compare_prop_indexes)
                {
                    if (float.TryParse(team_GS_data[index], out num))
                    {
                        compare_data.Add(num);
                    }
                    else if(team_GS_data[index].Length > 7)
                    {
                        //if(float.TryParse(team_GS_data[index].Substring(7).Split('/')[0], out num))
                        if (float.TryParse(team_GS_data[index].Split('%')[0], out num))
                        {
                            compare_data.Add(num / 100.0f);
                        }
                        else
                        {
                            compare_data.Add(0);
                        }                                          
                    }
                    else
                    {
                        compare_data.Add(0);
                    }
                }

                return compare_data;
            }
            else
            {
                return null;
            }
        }
    }
}

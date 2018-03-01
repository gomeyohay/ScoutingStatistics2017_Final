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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string localConnectionString = @"metadata=res://*/ScoutingModel_New.csdl|res://*/ScoutingModel_New.ssdl|res://*/ScoutingModel_New.msl;provider=System.Data.SQLite.EF6;provider connection string=';data source=D:\Scouting2017\TestDBs\ScoutingTestDb.db';";//@"C:\Users\Software 3.1\Documents\FRC 2017\Scouting\ScoutingTestDb.db";
            string phoneConnectionString = @"metadata=res://*/ScoutingModel_New.csdl|res://*/ScoutingModel_New.ssdl|res://*/ScoutingModel_New.msl;provider=System.Data.SQLite.EF6;provider connection string=';data source=D:\Scouting2017\TestDBs\Phone.db';";
            CopyTeamScouting(localConnectionString, phoneConnectionString);
        }

        public void CopyTeamScouting(string localConnectionString, string phoneConnectionString)
        {
            // open local DB
            using (ScoutingEntities_New localDB = new ScoutingEntities_New(localConnectionString))
            {
                // open phone DB
                using (ScoutingEntities_New phoneDB = new ScoutingEntities_New(phoneConnectionString))
                {
                    // select all rows from team scounting table in phone DB
                    var phone_team_rows = phoneDB.TeamScoutings.ToList();
                    // add rows into team scouting table in local DB
                    localDB.TeamScoutings.AddRange(phone_team_rows);

                    var phone_game_rows = phoneDB.GameScoutings.ToList();
                    int phone_game_id = phone_game_rows.First().game_id;
                    var local_game_rows = localDB.GameScoutings.Where(x => x.game_id == phone_game_id).ToList();
                    if (local_game_rows.Count <= 0)
                    {
                        localDB.GameScoutings.AddRange(phone_game_rows);
                    }

                    var phone_alliance_rows = phoneDB.AllianceScoutings.ToList();
                    localDB.AllianceScoutings.AddRange(phone_alliance_rows);

                    var phone_pit_rows = phoneDB.PitScoutings.ToList();
                    localDB.PitScoutings.AddRange(phone_pit_rows);


                    // commit changes
                    localDB.SaveChanges();
                } // using
            } // using

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

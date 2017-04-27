using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Public_Schedule : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());

        EventsAndGolfers eventsAndGolfers = DatabaseFunctions.GetEventsAndPlayers(leagueID, currentSeasonID);
        Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);
        //Table topLevelTable = new Table();
        //Table table = new Table();
        TableSchedule.Width = 900;
        TableSchedule.HorizontalAlign = HorizontalAlign.Center;
        //TableSchedule.BorderStyle = BorderStyle.Solid;
        //TableSchedule.BorderWidth = 5;
        //TableSchedule.BorderColor = Color.Black;
        //TableSchedule.CellPadding = 5;
        TableSchedule.CellSpacing = 5;


        //List<int> eventIDs = eventsAndGolfers.events.Keys.ToList();
        //eventIDs.Sort();
        SortedDictionary<int, int> sortedEvents = new SortedDictionary<int, int>();
        foreach (int EventID in eventsAndGolfers.events.Keys)
        {
            sortedEvents.Add(EventID, EventID);
        }

        foreach (int EventID in sortedEvents.Values)
        {
            
            TableRow titlerow = new TableRow();
            
            titlerow.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            titlerow.Style.Add(HtmlTextWriterStyle.BorderWidth, "3px");
            TableCell titleCell = new TableCell();
            
            titleCell.Width = 300;
            Label eventNameLabel = new Label();
            eventNameLabel.ForeColor = Color.Black;
            eventNameLabel.Font.Bold = true;
            eventNameLabel.Font.Size = 16;
            eventNameLabel.Text = eventsAndGolfers.events[EventID].EventName;
            titleCell.Controls.Add(eventNameLabel);
            //titleCell.BorderStyle = BorderStyle.Solid;
            titleCell.BorderColor = Color.Black;
            titleCell.HorizontalAlign = HorizontalAlign.Center;
            titlerow.Cells.Add(titleCell);

            TableCell dateCell = new TableCell();
            dateCell.Text = eventsAndGolfers.events[EventID].Date;
            dateCell.Font.Bold = true;
            dateCell.HorizontalAlign = HorizontalAlign.Center;
           // dateCell.BorderStyle = BorderStyle.Solid;
            dateCell.BorderColor = Color.Black;
            titlerow.Cells.Add(dateCell);

            TableCell courseCell = new TableCell();
            courseCell.Text = eventsAndGolfers.events[EventID].CourseName;
            courseCell.Font.Bold = true;
            //courseCell.BorderStyle = BorderStyle.Solid;
            courseCell.HorizontalAlign = HorizontalAlign.Center;
            courseCell.BorderColor = Color.Black;
            titlerow.Cells.Add(courseCell);

            TableSchedule.Rows.Add(titlerow);
            Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(EventID);
            bool toggle = true;
            foreach (int team1ID in matchups.Keys)
            {         
                TableRow matchupRow = new TableRow();
                TableCell matchupCell = new TableCell();
                matchupCell.ColumnSpan = 3;
                matchupCell.HorizontalAlign = HorizontalAlign.Center;
                Label matchupLabel = new Label();
                int team2ID = matchups[team1ID];
                matchupLabel.Text = teamNames[team1ID] + " vs. " + teamNames[team2ID];
                matchupCell.Controls.Add(matchupLabel);
                matchupRow.Cells.Add(matchupCell);

                if (toggle)
                {
                    matchupRow.BackColor = Color.WhiteSmoke;
                }
                toggle = !toggle;
 
                TableSchedule.Rows.Add(matchupRow); 
            }
               
        }
       
        
    }
}
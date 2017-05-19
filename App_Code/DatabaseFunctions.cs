using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;



public struct CourseInfo
{
    public int CourseID;
    public List<int> holeParRatings;
    public List<int> holeHandicaps;
    public List<int> holeLengths;
    public double courseHandicapRating;
    public string name;
}

public struct EventsAndGolfers
{
    public Dictionary<int, string> golfers;
    public Dictionary<int, EventInfo> events;
}

public class EventInfo
{
    public string Date {get;set;}
    public string CourseName { get; set; }
    public string EventName { get; set; }
    public int EventID { get; set; }
    public bool HasScores { get; set; }
    public string Matchups { get; set; }
    public int CourseID { get; set; }
    public string SeasonName { get; set; }
}

public class Season
{
    public int SeasonID { get; set; }
    public string SeasonName { get; set; }
    public bool isCurrentSeason { get; set; }
    public int NumberOfEventsScheduled { get; set; }
}

public class HandicapCalculationData
{
    public List<byte> scores;
    public int GolferID;
    public int LeagueEventID;
    public int CourseID;
    public int? HandicapOvveride;
    public double HandicapDifferential;
}

public class Golfer
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string nickName { get; set; }
    public string emailAddress { get; set; }
    public int golferID { get; set; }
    public int numberOfRoundsPlayed { get; set; }
}

public class Team
{
    public int Golfer1ID { get; set; }
    public int Golfer2ID { get; set; }
    public string Golfer1Name { get; set; }
    public string Golfer2Name { get; set; }

    //public bool hasThirdGolfer;

    public int Golfer3ID { get; set; }

    public string Golfer3Name { get; set; }

    public string TeamName { get; set; }
    public int TeamID { get; set; }
    public bool hidden { get; set; }
}

public class IndividualScore
{
    public int Score{ get; set;}
    public DateTime date { get; set; }
    public string SeasonName { get; set; }
    public string CourseName { get; set; }
    public string EventName { get; set; }
    public int Handicap { get; set; }
}


/// <summary>
/// Summary description for ASPDatabaseToolbox
/// </summary>
public static class DatabaseFunctions
{
    private const string DB_URL = "localhost";
    private const string DB_NAME = "GolfLeague";
    private const string USERNAME = "sa";
    private const string PASSWORD = "rossmere";

    private const string databaseConnectionString = "Server=Tim-Server\\SQLSERVER2008R2; Database=GolfLeague; User Id=sa; password=rossmere; Pooling=false";

    //private const string databaseConnectionString = "Server=ND375714\\MSSQL2012; Database=GolfLeague; Trusted_Connection=Yes";


    public static Dictionary<int, string> GetLeagues()
    {
        Dictionary<int, string> retVal = new Dictionary<int, string>();

        List<Row> result = queryForAll("Leagues");

        foreach (Row row in result)
        {
            int LeagueID = (int)row.columnNameValuePairs["LeagueID"];
            string LeagueName = (string)row.columnNameValuePairs["LeagueName"];
            retVal.Add(LeagueID, LeagueName);
        }
        return retVal;
    }


    public static void AddMatchup(int LeagueEventID, string team1ID, string team2ID)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("LeagueEventID", LeagueEventID.ToString());
        row.Add("Team1ID", team1ID);
        row.Add("Team2ID", team2ID);
        insertRow("Matchups", row);    
    }

    public static int AddLeagueEvent(int LeagueID, int SeasonID, int EventNumber, DateTime date, int CourseID, int tees, string eventName)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("LeagueID", LeagueID.ToString());
        row.Add("SeasonID", SeasonID.ToString());
        row.Add("Date", date.Date.ToString());
        row.Add("CourseID", CourseID.ToString());
        row.Add("Tees", tees.ToString());
        row.Add("EventName", eventName);
        //row.Add("EventNumber", EventNumber.ToString());
        return insertRowAndRetrieveIdentityColumn("LeagueEvents", "LeagueEventID" ,row);
    }

    public static int AddSeason(int LeagueID, string seasonName)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("LeagueID", LeagueID.ToString());
        row.Add("SeasonName", seasonName);
        //row.Add("isCurrentSeason", "0");  
        return insertRowAndRetrieveIdentityColumn("Seasons", "SeasonID", row);
    }

 

    public static void UpdateSeasonName(int SeasonID, string seasonName)
    {
        Dictionary<string, string> updates = new Dictionary<string, string>();
        updates.Add("SeasonName", "'" + seasonName + "'" );
        updateRow("Seasons", " SeasonID = '" + SeasonID.ToString() + "'", updates);
    }


    public static int AddLeagueEvent(int LeagueID, int SeasonID, DateTime? date, int? CourseID, int? CourseIDSecondNine, int? Tees, string notes, string EventName)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("LeagueID", LeagueID.ToString());
        row.Add("SeasonID", SeasonID.ToString());
        if (date != null)
        {
            row.Add("Date", date.ToString());
        }
        if (CourseID != null)
        {
            row.Add("CourseID", CourseID.ToString());
        }
        if (CourseIDSecondNine != null)
        {
            row.Add("CourseIDSecondNine", CourseIDSecondNine.ToString());
        }
        if (Tees != null)
        {
            row.Add("Tees", Tees.ToString());
        }
        if (notes != null)
        {
            row.Add("Notes", notes.ToString());
        }
        if (EventName != null)
        {
            row.Add("EventName", EventName.ToString());
        }
        return insertRowAndRetrieveIdentityColumn("LeagueEvents", "LeagueEventID", row);

        //insertRow("LeagueEvents", row);

        ////get value of identity column and return it
        //List<EqualsCondition> conditions = new List<EqualsCondition>();
        //conditions.Add(new EqualsCondition("LeagueID", LeagueID.ToString()));
        //conditions.Add(new EqualsCondition("SeasonID", SeasonID.ToString()));
        ////conditions.Add(new EqualsCondition("EventNumber", EventNumber.ToString()));
        //List<Row> result = queryWithEqualsConditions("LeagueEvents", conditions);
        //return (int)result[0].columnNameValuePairs["LeagueEventID"];
    }

   

    public static Dictionary<int, int> GetMatchups(int LeagueEventID)
    {
        Dictionary<int, int> retVal = new Dictionary<int,int>();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
        List<Row> result = queryWithEqualsConditions("Matchups", conditions);
        foreach (Row matchup in result)
        {
            retVal.Add((int)matchup.columnNameValuePairs["Team1ID"], (int)matchup.columnNameValuePairs["Team2ID"]);
        }
        return retVal;
    }

    public static void EditEvent(int EventID, string date, string EventName, int CourseID)
    {
        string sqlStatement = "UPDATE LeagueEvents ";
        sqlStatement += "SET Date = '" + date + "' , EventName = '" + EventName + "' , CourseID = '" + CourseID.ToString() + "'";
        sqlStatement += " WHERE LeagueEventID = " + EventID.ToString();
        ExecuteSQLStatement(sqlStatement);
    }

    public static void EditGolfer(int GolferID, string firstName, string lastName, string nickName, string emailAddress)
    {
        string sqlStatement = "UPDATE Golfers ";
        sqlStatement += "SET firstName = '" + firstName + "' , lastName = '" + lastName + "' , nickName = '" + nickName + "' , Email = '" + emailAddress + "'";
        sqlStatement += " WHERE GolferID = " + GolferID.ToString();
        ExecuteSQLStatement(sqlStatement);
    }

    public static void DeleteMatchups(int EventID)
    {
        string sqlStatement = "Delete from dbo.Matchups where LeagueEventID = " + EventID.ToString();
        ExecuteSQLStatement(sqlStatement);
    }

    public static void DeleteEvent(int EventID)
    {
        string sqlStatement = "Delete from dbo.Matchups where LeagueEventID = " + EventID.ToString();
        ExecuteSQLStatement(sqlStatement);
        sqlStatement = "Delete from dbo.LeagueEvents where LeagueEventID = " + EventID.ToString();
        ExecuteSQLStatement(sqlStatement);
    }

    public static void DeleteSub(int EventID, int SubGolferID)
    {
        string sqlStatement = "Delete from dbo.Scores where LeagueEventID = " + EventID.ToString() + " and GolferID = " + SubGolferID;
        ExecuteSQLStatement(sqlStatement);
        sqlStatement = "Delete from dbo.Subs where LeagueEventID = " + EventID.ToString() + " and GolferId = " + SubGolferID ;
        ExecuteSQLStatement(sqlStatement);
    }

    public static void DeleteScore(int EventID, int GolferID)
    {
        string sqlStatement = "Delete from dbo.Scores where LeagueEventID = " + EventID.ToString() + " and GolferID = " + GolferID;
        ExecuteSQLStatement(sqlStatement);
    }

    public static List<int> GetGolfersByEvent(int LeagueEventID)
    {
        Dictionary<int, int> golferCountsByTeam = new Dictionary<int, int>();

        List<int> retVal = new List<int>();

        string sqlString = "Select * from Matchups Join TeamRosters on matchups.Team1ID = TeamRosters.TeamID or Matchups.Team2ID = TeamRosters.TeamID where Matchups.LeagueEventID = " + LeagueEventID.ToString(); 
            //String.Format("TeamID in (select Team1ID from Matchups where LeagueEventID = {0}) or TeamID in (select Team2ID from Matchups where LeagueEventID = {0}) order by TeamID", LeagueEventID.ToString());

        List<Row> result = executeQuery(sqlString); //queryWithWhereCondition("Matchups", whereCondition);
        
        if (result != null)
        {
            foreach (Row row in result)
            {
                int teamID = (int)row.columnNameValuePairs["TeamID"];
                if(golferCountsByTeam.ContainsKey(teamID))
                {
                    golferCountsByTeam[teamID]++;
                    if(golferCountsByTeam[teamID] > 2)
                    {
                        continue;
                    }
                }
                else
                {
                    golferCountsByTeam.Add(teamID, 1);
                }
                int GolferID = (int)row.columnNameValuePairs["GolferID"];
                retVal.Add(GolferID);
            }
        }

        return retVal;
    }

    public static Dictionary<int, string> GetGolfers(int TeamID)
    {
        Dictionary<int, string> retVal = new Dictionary<int, string>();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("TeamID", TeamID.ToString()));
        List<Row> result = queryWithEqualsConditions("TeamRosters", conditions);

        foreach (Row row in result)
        {
            int GolferID = (int)row.columnNameValuePairs["GolferID"];
            string GolferName = GetGolferName(GolferID);
            retVal.Add(GolferID, GolferName);
        }
        return retVal;
    }

    public static string GetGolferName(int GolferID)
    {
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("GolferID", GolferID.ToString()));
        List<Row> result = queryWithEqualsConditions("Golfers", conditions);
        return (string)result[0].columnNameValuePairs["FirstName"] + " " +(string)result[0].columnNameValuePairs["LastName"];
    }

    public static Golfer GetGolferInfo(int GolferID)
    {
        Golfer retVal = new Golfer();
        retVal.golferID = GolferID;
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("GolferID", GolferID.ToString()));
        List<Row> result = queryWithEqualsConditions("Golfers", conditions);
        retVal.firstName = (string)result[0].columnNameValuePairs["FirstName"];
        retVal.lastName = (string)result[0].columnNameValuePairs["LastName"];
        retVal.nickName = result[0].columnNameValuePairs["Nickname"] == DBNull.Value ? "" : (string)result[0].columnNameValuePairs["Nickname"];
        retVal.emailAddress = result[0].columnNameValuePairs["Email"] == DBNull.Value ? "" : (string)result[0].columnNameValuePairs["Email"];
        return retVal;
    }

    public static Dictionary<int, string> GetAllCourses()
    {
        Dictionary<int, string> retVal = new Dictionary<int, string>();
        List<Row> queryResults = queryForAll("Courses");
        foreach (Row row in queryResults)
        {
            retVal.Add((int)row.columnNameValuePairs["CourseID"], (string)row.columnNameValuePairs["Name"]);
        }
        return retVal;
    }

    public static List<EventInfo> GetEvents(int LeagueID, int SeasonID)
    {
        List<EventInfo> retVal = new List<EventInfo>();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueID", LeagueID.ToString()));
        conditions.Add(new EqualsCondition("SeasonID", SeasonID.ToString()));
        List<Row> result = queryWithEqualsConditions("LeagueEvents", conditions);
        if (result != null)
        {
            foreach (Row row in result)
            {
                EventInfo eventToAdd = new EventInfo();
                eventToAdd.Date = (string)row.columnNameValuePairs["Date"].ToString(); 
                eventToAdd.CourseName =  row.columnNameValuePairs["CourseID"] != DBNull.Value ?  DatabaseFunctions.GetCourseName((int)row.columnNameValuePairs["CourseID"]) : "";
                eventToAdd.EventID = (int)row.columnNameValuePairs["LeagueEventID"]; //(int)row.columnNameValuePairs["EventNumber"];
                eventToAdd.EventName = (string)row.columnNameValuePairs["EventName"];
                eventToAdd.CourseID = row.columnNameValuePairs["CourseID"] != DBNull.Value ? (int)row.columnNameValuePairs["CourseID"] : 0;
                retVal.Add(eventToAdd);
            }
        }
        return retVal;
    }

    public static List<Season> GetSeasons(int LeagueID)
    {
        List<Season> retVal = new List<Season>();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueID", LeagueID.ToString()));
        List<Row> result = queryWithEqualsConditions("Seasons", conditions);
        if (result == null)
        {
            return retVal;
        }
        int currentSeasonID = GetCurrentSeasonID(LeagueID.ToString());
        foreach (Row season in result)
        {
            Season seasonToAdd = new Season();
            seasonToAdd.SeasonID = (int)season.columnNameValuePairs["SeasonID"];
            seasonToAdd.SeasonName = (string)season.columnNameValuePairs["SeasonName"];
            seasonToAdd.isCurrentSeason = seasonToAdd.SeasonID == currentSeasonID;
            retVal.Add(seasonToAdd);
        }
        return retVal;
    }

    public static int GetLeagueIDFromLoggedInUserID(string userGUID)
    {
        int retVal;
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("CommissionerID", userGUID));
        List<Row> result = queryWithEqualsConditions("Leagues", conditions);
        if (result.Count == 0)
        {
            conditions = new List<EqualsCondition>();
            conditions.Add(new EqualsCondition("LoginGUID", userGUID));
            result = queryWithEqualsConditions("Commisioners", conditions);
        }
        return (int)result[0].columnNameValuePairs["LeagueID"];
    }

    public static string ValidateLeagueID(string LeagueID)
    {
        if (LeagueID != null)
        {
            int id = int.Parse(LeagueID);

            List<Row> leagues = queryWithWhereCondition("Leagues", "LeagueID = '" + id.ToString() + "';");

            if (leagues.Count > 0)
            {
                return (string)leagues[0].columnNameValuePairs["LeagueName"];
            }
        }

        return null; 
    }

    public static int addLeague(string CommissionerLoginGUID, string LeagueName)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("CommissionerID", CommissionerLoginGUID);
        row.Add("LeagueName", LeagueName);
        return insertRowAndRetrieveIdentityColumn("Leagues", "LeagueID", row);
    }

    public static int GetCurrentSeasonID(string LeagueID)
    {
            //List<EqualsCondition> equalsConditions = new List<EqualsCondition>();
            //EqualsCondition condition1 = new EqualsCondition("LeagueID", LeagueID);
            //EqualsCondition condition2 = new EqualsCondition("IsCurrent", "true");
            //equalsConditions.Add(condition1);
            //equalsConditions.Add(condition2);
            //List<Row> Season = queryWithEqualsConditions("Seasons", equalsConditions);
            //return (int)Season[0].columnNameValuePairs["SeasonID"];

            List<EqualsCondition> equalsConditions = new List<EqualsCondition>();
            EqualsCondition condition1 = new EqualsCondition("LeagueID", LeagueID);
            equalsConditions.Add(condition1);
            List<Row> League = queryWithEqualsConditions("Leagues", equalsConditions);
            if (League[0].columnNameValuePairs["CurrentSeasonID"] == DBNull.Value)
            {
                return 0;
            }
            return (int)League[0].columnNameValuePairs["CurrentSeasonID"];
    }

    public static Season GetCurrentSeasonInfo(string LeagueID)
    {
        Season retVal = new Season();
        string querySql = "select seasons.* from seasons join leagues on Seasons.SeasonID = Leagues.CurrentSeasonID where Seasons.LeagueID =" + LeagueID;
        List<Row> result = executeQuery(querySql);

        if (result == null || result.Count == 0)
        {
            return null; 
        }

        retVal.isCurrentSeason = true;
        retVal.SeasonID = (int)result[0].columnNameValuePairs["SeasonID"];
        retVal.SeasonName = (string)result[0].columnNameValuePairs["SeasonName"]; 

        return retVal;
    }

    public static List<int> GetCourseIDs(int LeagueID)
    {
        //List<EqualsCondition> equalsConditions = new List<EqualsCondition>();
        //EqualsCondition condition1 = new EqualsCondition("LeagueID", LeagueID);
        //EqualsCondition condition2 = new EqualsCondition("IsCurrent", "true");
        //equalsConditions.Add(condition1);
        //equalsConditions.Add(condition2);
        //List<Row> Season = queryWithEqualsConditions("Seasons", equalsConditions);
        //return (int)Season[0].columnNameValuePairs["SeasonID"];
        List<int> retVal = new List<int>();
        List<EqualsCondition> equalsConditions = new List<EqualsCondition>();
        EqualsCondition condition1 = new EqualsCondition("LeagueID", LeagueID.ToString());
        equalsConditions.Add(condition1);
        List<Row> results = queryWithEqualsConditions("LeagueEvents", equalsConditions);
        foreach (Row row in results)
        {
            if (row.columnNameValuePairs["CourseID"] != DBNull.Value && !retVal.Contains((int)row.columnNameValuePairs["CourseID"]))
            {
                retVal.Add((int)row.columnNameValuePairs["CourseID"]);
            }
        }
        return retVal;
    }

    public static Dictionary<int, List<byte>> GetScores(int LeagueEventID)
    {
        Dictionary<int, List<byte>> retVal = new Dictionary<int, List<byte>>();
        List<EqualsCondition> equalsConditions = new List<EqualsCondition>();
        EqualsCondition condition1 = new EqualsCondition("LeagueEventID", LeagueEventID.ToString());
        equalsConditions.Add(condition1);
        List<Row> Rounds = queryWithEqualsConditions("Scores", equalsConditions);
        foreach (Row round in Rounds)
        {
            List<byte> scores = new List<byte>();
            for(int i = 1; i < 10; i++)
            {
                scores.Add((byte)(round.columnNameValuePairs["Hole" + i.ToString()]));
            }
            retVal.Add((int)round.columnNameValuePairs["GolferID"], scores);
        }
        return retVal;
    }


    public static Dictionary<int, List<HandicapCalculationData>> GetHandicapCalculationData(int LeagueID)
    {
        Dictionary<int, List<HandicapCalculationData>> retVal = new Dictionary<int, List<HandicapCalculationData>>();
        SqlDataReader reader = ExecuteStoredProcedure("GetDataToCalculateHandicaps", "LeagueID", LeagueID);
        while (reader.Read())
        {
            HandicapCalculationData handicapData = new HandicapCalculationData();
            handicapData.GolferID = (int)reader["GolferID"];
            handicapData.LeagueEventID = (int)reader["LeagueEventID"];
            handicapData.CourseID = (int)reader["CourseID"];
            if (reader["HandicapOvveride"] == DBNull.Value)
            {
                handicapData.HandicapOvveride = null;
            }
            else
            {
                handicapData.HandicapOvveride = (int)reader["HandicapOvveride"];
            }
            handicapData.scores = new List<byte>();
            for (int i = 1; i < 10; i++)
            {
                handicapData.scores.Add((byte)reader["Hole" + i.ToString()]);
            }
            if (!retVal.ContainsKey(handicapData.GolferID))
            {
                retVal.Add(handicapData.GolferID, new List<HandicapCalculationData>()); 
            }
            retVal[handicapData.GolferID].Add(handicapData);
        }
        return retVal;
    }

    public static Dictionary<int, CourseInfo> GetCourses(int leagueID)
    {
        Dictionary<int, CourseInfo> retVal = new Dictionary<int, CourseInfo>();
        List<EqualsCondition> equalsConditions = new List<EqualsCondition>();
        EqualsCondition condition1 = new EqualsCondition("LeagueID", leagueID.ToString());
        equalsConditions.Add(condition1);
        List<Row> rows = queryWithEqualsConditions("CourseLeagueMapping", equalsConditions);

        foreach (Row row in rows)
        {
            int courseID = (int)row.columnNameValuePairs["CourseID"];
            CourseInfo courseInfo = GetCourseInfoFromCourseID(courseID);
            retVal.Add(courseID, courseInfo);
        }

        return retVal;
    }

    //returns newest rounds first
    public static void GetGolferScores(int GolferID, out List<CourseInfo> courseInfos, out List<List<byte>> scoreList, int LeagueEventID)
    {
        courseInfos = new List<CourseInfo>();
        scoreList =  new List<List<byte>>();

        //Query should return rounds oldest first
        List<Row> Rounds = QueryForScoresByDate(GolferID, LeagueEventID);
        Rounds.Reverse();

        foreach (Row round in Rounds)
        {
            List<byte> scores = new List<byte>();
            for (int i = 1; i < 10; i++)
            {
                scores.Add((byte)(round.columnNameValuePairs["Hole" + i.ToString()]));
            }
            scoreList.Add(scores);

            int CourseID = (int)(round.columnNameValuePairs["CourseID"]);
            CourseInfo courseInfo = DatabaseFunctions.GetCourseInfoFromCourseID(CourseID);
            courseInfos.Add(courseInfo);
        }    
    }


    public static CourseInfo GetCourseInfoFromCourseID(int courseID)
    {
        CourseInfo retVal = new CourseInfo();
       
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("CourseID", courseID.ToString()));
        List<Row> result = queryWithEqualsConditions("Holes", conditions);

        int[] holeLengths = new int[9];
        int[] parRatings = new int[9];
        int[] holeHandicaps = new int[9];

        foreach (Row row in result)
        {
            int HoleNumber = ((int)row.columnNameValuePairs["HoleNumber"]) - 1;
            holeLengths[HoleNumber] = (int)row.columnNameValuePairs["MensLength"];
            holeHandicaps[HoleNumber] = (int)row.columnNameValuePairs["MensHandicap"];
            parRatings[HoleNumber] = (int)row.columnNameValuePairs["MensPar"];
        }
        retVal.holeHandicaps = holeHandicaps.ToList();
        retVal.holeParRatings = parRatings.ToList();
        retVal.holeLengths = holeLengths.ToList();

        conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("CourseID", courseID.ToString()));
        result = queryWithEqualsConditions("Courses", conditions);
        retVal.courseHandicapRating = (double)result[0].columnNameValuePairs["MensHandicap"];
        retVal.name = (string)result[0].columnNameValuePairs["Name"];

        return retVal;
    }

    public static CourseInfo GetCourseInfo(int LeagueEventID)
    {
        CourseInfo retVal = new CourseInfo();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
        List<Row> result = queryWithEqualsConditions("LeagueEvents", conditions);
        int courseID = (int)result[0].columnNameValuePairs["CourseID"];
        retVal.CourseID = courseID;

        conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("CourseID", courseID.ToString()));
        result = queryWithEqualsConditions("Holes", conditions);
        
        int[] holeLengths = new int[9];
        int[] parRatings = new int[9];
        int[] holeHandicaps = new int[9];

        foreach(Row row in result)
        {
            int HoleNumber = ((int)row.columnNameValuePairs["HoleNumber"]) - 1;
            holeLengths[HoleNumber] = (int)row.columnNameValuePairs["MensLength"];
            holeHandicaps[HoleNumber] = (int)row.columnNameValuePairs["MensHandicap"];
            parRatings[HoleNumber] = (int)row.columnNameValuePairs["MensPar"];
        }
        retVal.holeHandicaps = holeHandicaps.ToList();
        retVal.holeParRatings = parRatings.ToList();
        retVal.holeLengths = holeLengths.ToList();


        //retVal.holeHandicaps = new List<int>();
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole1MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole2MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole3MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole4MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole5MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole6MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole7MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole8MensHandicap"]);
        //retVal.holeHandicaps.Add((int)result[0].columnNameValuePairs["Hole9MensHandicap"]);

        //retVal.holeLengths = new List<int>();
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole1MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole2MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole3MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole4MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole5MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole6MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole7MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole8MensLength"]);
        //retVal.holeLengths.Add((int)result[0].columnNameValuePairs["Hole9MensLength"]);

        //retVal.holeParRatings = new List<int>();
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole1MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole2MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole3MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole4MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole5MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole6MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole7MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole8MensPar"]);
        //retVal.holeParRatings.Add((int)result[0].columnNameValuePairs["Hole9MensPar"]);
        
        return retVal;
    }

    public static Dictionary<int, int> GetHandicapOverrides(int LeagueEventID)
    {
        Dictionary<int, int> retVal = new Dictionary<int, int>();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
        List<Row> result = queryWithEqualsConditions("HandicapOverrides", conditions);
        foreach (Row row in result)
        {
            retVal.Add((int)row.columnNameValuePairs["GolferID"], (int)row.columnNameValuePairs["Handicap"]);
        }
        return retVal;
    }

    public static Dictionary<int, int> GetSubs(int LeagueEventID)
    {
        Dictionary<int, int> retVal = new Dictionary<int, int>();
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
        List<Row> result = queryWithEqualsConditions("Subs", conditions);
        foreach (Row row in result)
        {
            retVal.Add((int)row.columnNameValuePairs["PlayerSubbedForID"], (int)row.columnNameValuePairs["GolferID"]);
        }
        return retVal;
    }


    public static Dictionary<string, string> GetLeagueSettings(int LeagueID)
    {
        Dictionary<string, string> retVal = new Dictionary<string, string>();
        if(LeagueID == 3)
        {
            retVal.Add("SubPtsLimit", "5");
            retVal.Add("ESCMaxOverPar", "4");
        }
        return retVal;
    }



    //public static List<int> GetNoShows(int LeagueEventID)
    //{
    //    List<int> retVal = new List<int>();
    //    List<EqualsCondition> conditions = new List<EqualsCondition>();
    //    conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
    //    List<Row> result = queryWithEqualsConditions("NoShows", conditions);
    //    foreach (Row row in result)
    //    {
    //        retVal.Add((int)row.columnNameValuePairs["GolferID"]);
    //    }
    //    return retVal;
    //}

    public static Dictionary<int, Team> GetTeams(int LeagueID)
    {
        Dictionary<int, Team> retVal = new Dictionary<int, Team>();

        string sql = @"SELECT Teams.* , Golfers.FirstName, Golfers.LastName, Golfers.GolferID FROM [GolfLeague].[dbo].[Teams] 
                    Join TeamRosters on TeamRosters.TeamID = Teams.TeamID Join Golfers 
                    on Golfers.GolferID = TeamRosters.GolferID
                    where LeagueID = " + LeagueID.ToString() +
                    " Group by  Teams.TeamID, Teams.LeagueID, Teams.TeamName, Teams.Hidden, Golfers.FirstName, Golfers.LastName, Golfers.GolferID";
        
        List<Row> result = executeQuery(sql);

        if (result != null)
        {
            foreach (Row row in result)
            {
                int teamID = (int)row.columnNameValuePairs["TeamID"];
                if(!retVal.ContainsKey(teamID))
                {                
                    Team team = new Team();
                    team.Golfer1ID = (int)row.columnNameValuePairs["GolferID"];
                    team.Golfer1Name = (string)row.columnNameValuePairs["FirstName"] + " " + (string)row.columnNameValuePairs["LastName"];
                    team.TeamID = (int)row.columnNameValuePairs["TeamID"];
                    team.TeamName = (string)row.columnNameValuePairs["TeamName"];
                    team.hidden = (bool)row.columnNameValuePairs["Hidden"];
                    retVal.Add(teamID ,team);
                }
                else
                {
                    if (string.IsNullOrEmpty(retVal[teamID].Golfer2Name))
                    {
                        retVal[teamID].Golfer2ID = (int)row.columnNameValuePairs["GolferID"];
                        retVal[teamID].Golfer2Name = (string)row.columnNameValuePairs["FirstName"] + " " + (string)row.columnNameValuePairs["LastName"];
                    }
                    else
                    {
                        retVal[teamID].Golfer3ID = (int)row.columnNameValuePairs["GolferID"];
                        retVal[teamID].Golfer3Name = (string)row.columnNameValuePairs["FirstName"] + " " + (string)row.columnNameValuePairs["LastName"];
                    }
                }
            }
            
        }

        return retVal;
    }


    public static Dictionary<int, string> GetTeamNames(int LeagueID)
    {
        List<Row> leagueEvents = queryWithWhereCondition("Teams", "LeagueID = '" + LeagueID.ToString() + "';");

        Dictionary<int, string> retVal = new Dictionary<int, string>();

        foreach (Row dbEvent in leagueEvents)
        {
            int teamID = (int)dbEvent.columnNameValuePairs["TeamID"];
            retVal.Add(teamID, (string)dbEvent.columnNameValuePairs["TeamName"]);
        }

        return retVal;
    }

    public static List<int> GetTeamsActiveInSeason(int LeagueId, int SeasonId)
    {
        List<int> retVal = new List<int>();
        string sql = "select Distinct(Teams.TeamID) from LeagueEvents Join Matchups on Matchups.LeagueEventID = LeagueEvents.LeagueEventID Join Teams on Teams.TeamID = Team1ID or Teams.TeamID = Team2ID Where LeagueEvents.LeagueID = " + LeagueId.ToString() + " and SeasonID = " + SeasonId.ToString();
        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
                retVal.Add((int)row.columnNameValuePairs["TeamID"]);
            }
        }

        return retVal;
    }

    public static List<int> GetTeamsActiveInCurrentSeason(int LeagueId)
    {
        string sql = "select * from Teams where ActiveInCurrentSeason = 1 and LeagueID = " + LeagueId.ToString();
        List<Row> result = executeQuery(sql);
        if (result == null || result.Count == 0)
        {
            return null;
        }
        else
        {
            List<int> retVal = new List<int>();
            foreach (Row row in result)
            {
                retVal.Add((int)row.columnNameValuePairs["TeamID"]);
            }
            return retVal;
        }
    }

    public static void UpdateEventDetails(int eventID, DateTime eventDate, int CourseID)
    {
        Dictionary<string, string> updates = new Dictionary<string, string>();
        updates.Add("Date",  "'" + eventDate.ToShortDateString() + "'");
        updates.Add("CourseID", CourseID.ToString());
        updateRow("LeagueEvents", " LeagueEventID = '" + eventID.ToString() + "'", updates);
    }

    public static void UpdateTeamName(string teamID, string teamName)
    {
        Dictionary<string, string> updates = new Dictionary<string, string>();
        updates.Add("TeamName", teamName);
        updateRow("Teams", " TeamID = '" + teamID.ToString() + "'", updates);
    }

    public static void UpdateCurrentSeason(int leagueID, int seasonID)
    {
        Dictionary<string, string> updates = new Dictionary<string, string>();
        updates.Add("CurrentSeasonID", seasonID.ToString());
        updateRow("Leagues", " LeagueID = '" + leagueID.ToString() + "'", updates);
    }

    public static void UpdateTeamActiveInCurrentSeason(int currentSeasonID, string teamID, bool ActiveInCurrentSeason)
    {
        if (!ActiveInCurrentSeason)
        {
            List<EqualsCondition> conditions = new List<EqualsCondition>();
            conditions.Add(new EqualsCondition("TeamID", teamID));
            conditions.Add(new EqualsCondition("SeasonID", currentSeasonID.ToString()));
            RemoveRowIfExisting("SeasonAffiliations", conditions);
        }
        else
        {
            AddSeasonAffiliation(int.Parse(teamID), currentSeasonID);
        }
    }
    
    //public static Dictionary<int, string> GetTeamNames(int LeagueID, int currentSeasonID)
    //{
    //    List<Row> leagueEvents = queryWithWhereCondition("Teams", "LeagueID = '" + LeagueID.ToString() + "';");

    //    Dictionary<int, string> retVal = new Dictionary<int, string>();

    //    foreach (Row dbEvent in leagueEvents)
    //    {
    //        int teamID = (int)dbEvent.columnNameValuePairs["TeamID"];
            
    //        if(isTeamActiveInSeason(teamID, currentSeasonID))
    //        {
    //            retVal.Add(teamID, (string)dbEvent.columnNameValuePairs["TeamName"]);
    //        }
    //    }

    //    return retVal;
    //}

    //public static bool isTeamActiveInSeason(int teamID, int seasonID)
    //{
    //    List<EqualsCondition> conditions = new List<EqualsCondition>(); 
    //    conditions.Add(new EqualsCondition("SeasonID", seasonID.ToString()));
    //    conditions.Add(new EqualsCondition("TeamID", teamID.ToString()));
    //    List<Row> result = queryWithEqualsConditions("SeasonAffiliations", conditions);
    //    return (result.Count > 0);
    //}

    //public static Dictionary<int, string> GetTeamsInSeason(int LeagueID, int currentSeasonID)
    //{

    //}

    public static void SaveLeagueSchedule(Dictionary<int, List<GolfLeagueWebsiteGlobals.Matchup>> schedule, int LeagueID, int SeasonID)
    {
        
        foreach(int week in schedule.Keys)
        {
            Dictionary<string, string> leagueEventRow = new Dictionary<string, string>();
            int leagueEventID = AddLeagueEvent(LeagueID, SeasonID, null, null, 1, 1, null, "Week " + week.ToString());
         
            foreach(GolfLeagueWebsiteGlobals.Matchup m in schedule[week])
            {
                AddMatchup(leagueEventID, m.Team1ID, m.Team2ID);
            }
            
        }
    }

    public static void AddCommissioner(int GolferID, string Password)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("GolferID", GolferID.ToString());
        row.Add("Password", Password.ToString());
        insertRow("Commisioners", row);
    }


    public static void AddSeasonAffiliation(int TeamID, int SeasonID)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("TeamID", TeamID.ToString()); 
        row.Add("SeasonID", SeasonID.ToString());
        insertRow("SeasonAffiliations", row);
    }

    public static void AddLeagueAffiliation(int GolferID, int LeagueID)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("GolferID", GolferID.ToString());
        row.Add("LeagueID", LeagueID.ToString());
        row.Add("isDeactivated", "false");
        insertRow("LeagueAffiliations", row);
    }


    public static int CreateSeason(int leagueID, string seasonName, bool isCurrentSeason)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("LeagueID", leagueID.ToString());
        row.Add("SeasonName", seasonName);
        row.Add("isCurrentSeason", isCurrentSeason.ToString());
        int seasonID = insertRowAndRetrieveIdentityColumn("Seasons", "SeasonID" ,row);
        if (isCurrentSeason)
        {
            Dictionary<string, string> updates = new Dictionary<string, string>();
            updates.Add("CurrentSeasonID", seasonID.ToString());
            updateRow("Leagues", " LeagueID = '" + leagueID.ToString() + "'", updates);  
        }
        return seasonID;
    }

    public static int AddGolfer(string firstName, string lastName, string nickName, string emailAddress)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("FirstName", firstName);
        row.Add("LastName", lastName);
        row.Add("Nickname", nickName);
        row.Add("Email", emailAddress);
        return insertRowAndRetrieveIdentityColumn("Golfers", "GolferID" , row);
    }


    public static void AddTeam(int LeagueID, string TeamName, int player1ID, int player2ID, int? player3ID, bool hidden)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("LeagueID", LeagueID.ToString());
        row.Add("TeamName", TeamName);
        row.Add("Hidden", hidden.ToString()); 
        int teamID = insertRowAndRetrieveIdentityColumn("Teams", "TeamID", row);
        row.Clear();
        row.Add("TeamID", teamID.ToString());
        row.Add("GolferID", player1ID.ToString());
        insertRow("TeamRosters", row);
        row.Clear();
        row.Add("TeamID", teamID.ToString());
        row.Add("GolferID", player2ID.ToString());
        insertRow("TeamRosters", row);
        row.Clear();
        if (player3ID != null)
        {
            row.Add("TeamID", teamID.ToString());
            row.Add("GolferID", player3ID.ToString());
            insertRow("TeamRosters", row);
        }
        //if (activeInCurrentSeason)
        //{
        //    int currentSeasonID = GetCurrentSeasonID(LeagueID.ToString());
        //    AddSeasonAffiliation(teamID, currentSeasonID);
        //}
    }

    public static void EditTeam(int TeamID , string TeamName, bool hidden)
    {
        Dictionary<string, string> updates = new Dictionary<string, string>();
        updates.Add("TeamName", "'" + TeamName + "'");
        updates.Add("Hidden", "'" +  hidden.ToString() + "'");
        updateRow("Teams", " TeamID = '" + TeamID.ToString() + "'", updates); 
    }


    //private bool CheckForDuplicateTeam(int GolferId1, int GolferId2, int LeagueID)
    //{
    //    string sqlStatement = "Select teamID from TeamRosters where golferID = " + GolferId1 + " and teamID in (Select teamID from TeamRosters where GolferID = " + GolferId2 + ")";
    //    SqlDataReader reader = ExecuteSQLStatement(sqlStatement);    
    //    return reader.HasRows;
    //}

   

    private static void AddMatchup(int LeagueEventID, int team1ID, int team2ID)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("Team1ID", team1ID.ToString());
        row.Add("Team2ID", team2ID.ToString());
        row.Add("LeagueEventID", LeagueEventID.ToString());
        insertRow("Matchups", row);
    }

    public static void AddSubs(int LeagueEventID, int subGolferID, int golferBeingSubbedForID)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("GolferID", subGolferID.ToString());
        row.Add("PlayerSubbedForID", golferBeingSubbedForID.ToString());
        row.Add("LeagueEventID", LeagueEventID.ToString());
        insertRow("Subs", row);
    }

    public static void AddNoShow(int LeagueEventID, int GolferID)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("GolferID", GolferID.ToString());
        row.Add("LeagueEventID", LeagueEventID.ToString());
        insertRow("NoShows", row);
    }

    public static void AddScore(int GolferID, int LeagueEventID, List<byte> scores)
    {
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("GolferID", GolferID.ToString()));
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));

        RemoveRowIfExisting("Scores", conditions);
        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("GolferID", GolferID.ToString());
        row.Add("LeagueEventID", LeagueEventID.ToString());
        for (int i = 1; i < (scores.Count+1); i++)
        {
            row.Add("Hole" + i.ToString(), scores[i-1].ToString());
        }
        insertRow("Scores", row);
    }

    

    public static void InsertHandicapOverride(int GolferID, int LeagueEventID, int handicap)
    {
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("GolferID", GolferID.ToString()));
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
        RemoveRowIfExisting("HandicapOverrides", conditions);

        Dictionary<string, string> row = new Dictionary<string, string>();
        row.Add("GolferID", GolferID.ToString());
        row.Add("LeagueEventID", LeagueEventID.ToString());
        row.Add("Handicap", handicap.ToString());
        insertRow("HandicapOverrides", row);
    }

    

    //public static List<Golfer> GetAllGolfersFullInfo(int LeagueID)
    //{
    //    List<Golfer> retVal = new List<Golfer>();

    //    List<Row> dbRetVal = queryWithWhereCondition("LeagueAffiliations", "LeagueID = '" + LeagueID + "';");
    //    foreach (Row row in dbRetVal)
    //    {
    //        Golfer golfer = new Golfer();
    //        golfer.golferID = (int)row.columnNameValuePairs["GolferID"];
    //        List<Row> golferTableRows = queryWithWhereCondition("Golfers", "GolferID = '" + golfer.golferID.ToString() + "' Order by LastName; ");
    //        golfer.firstName = (string)golferTableRows[0].columnNameValuePairs["FirstName"];
    //        golfer.lastName =  (string)golferTableRows[0].columnNameValuePairs["LastName"];
    //        golfer.nickName = golferTableRows[0].columnNameValuePairs["Nickname"] == DBNull.Value ? "" : (string)golferTableRows[0].columnNameValuePairs["Nickname"];
    //        golfer.emailAddress = golferTableRows[0].columnNameValuePairs["Email"] == DBNull.Value ? "" : (string)golferTableRows[0].columnNameValuePairs["Email"];
    //        golfer.numberOfRoundsPlayed = GetNumberOfRoundsPlayed(golfer.golferID); 
    //        retVal.Add(golfer);
    //    }
    //    return retVal;
    //}

    public static int GetNumberOfRoundsPlayed(int golferID)
    {
        List<Row> dbRetVal = queryWithWhereCondition("Scores", "GolferID = '" + golferID + "';");
        if(dbRetVal == null)
        {
            return 0;
        }
        return dbRetVal.Count;
    }

    //public static Dictionary<int, string> GetAllGolfers(int LeagueID)
    //{
    //    Dictionary<int, string> retVal = new Dictionary<int, string>();
    //    Dictionary<int, string> unsortedGolfers = new Dictionary<int, string>();
    //    unsortedGolfers = GetGolferNamesAndIDs(LeagueID.ToString());

    //    var items = from pair in unsortedGolfers
    //                orderby pair.Value ascending
    //                select pair;

    //    foreach (KeyValuePair<int, string> pair in items)
    //    {
    //        retVal.Add(pair.Key, pair.Value);
    //    }

    //    return retVal;
    //}

    public static EventsAndGolfers GetEventsAndPlayers(int LeagueID)
    {
        EventsAndGolfers retVal = new EventsAndGolfers();
        retVal.events = new Dictionary<int, EventInfo>();

        List<Row> leagueEvents = queryWithWhereCondition("LeagueEvents", "LeagueID = '" + LeagueID.ToString() + "';");

        foreach (Row dbEvent in leagueEvents)
        {
            EventInfo newEvent = new EventInfo();
            if (dbEvent.columnNameValuePairs["Date"] != DBNull.Value)
            {
                newEvent.Date = ((DateTime)dbEvent.columnNameValuePairs["Date"]).ToString();
            }
            else
            {
                newEvent.Date = null;
            }

            if (dbEvent.columnNameValuePairs["CourseID"] != DBNull.Value)
            {
                int courseID = (int)dbEvent.columnNameValuePairs["CourseID"];
                newEvent.CourseName = GetCourseName(courseID);
            } 
            
            int LeagueEventID = (int)dbEvent.columnNameValuePairs["LeagueEventID"];
            newEvent.EventName = (string)dbEvent.columnNameValuePairs["EventName"];
            retVal.events.Add(LeagueEventID, newEvent); 
        }

        retVal.golfers = GetGolferNamesAndIDs(LeagueID.ToString());

        return retVal;
    }

    public static Dictionary<int, EventInfo> GetAllEvents(int LeagueID)
    {
        Dictionary<int, EventInfo> retVal = new Dictionary<int, EventInfo>();

        string sql = "SELECT Courses.Name as CourseName, LeagueEvents.LeagueEventID, [Date], EventName, SeasonName FROM LeagueEvents "
                      + "Join Seasons "
                      + "on Seasons.SeasonID = LeagueEvents.SeasonID "
                      + "Join Courses "
                      + "on Courses.CourseID = LeagueEvents.CourseID "
                      + " where LeagueEvents.LeagueID = " + LeagueID.ToString();

        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
                EventInfo eventInfo = new EventInfo();
                eventInfo.CourseName = (string)row.columnNameValuePairs["CourseName"];
                eventInfo.EventID = (int)row.columnNameValuePairs["LeagueEventID"];
                eventInfo.EventName = (string)row.columnNameValuePairs["EventName"];
                eventInfo.SeasonName = (string)row.columnNameValuePairs["SeasonName"];
                eventInfo.Date = ((DateTime)row.columnNameValuePairs["Date"]).ToString();
                retVal.Add(eventInfo.EventID, eventInfo);
            }
        }

        return retVal;
    }

    public static Dictionary<int, EventInfo> GetEventsWithScores(int LeagueID)
    {
        Dictionary<int, EventInfo> retVal = new Dictionary<int, EventInfo>();
      
        string sql = "SELECT Courses.Name as CourseName, LeagueEvents.LeagueEventID, [Date], EventName, SeasonName FROM LeagueEvents " 
                      + "Join Seasons "
                      + "on Seasons.SeasonID = LeagueEvents.SeasonID "
                      + "Join Courses "
                      + "on Courses.CourseID = LeagueEvents.CourseID "
                      + " where LeagueEventID in (select LeagueEventID from scores) and LeagueEvents.LeagueID = " + LeagueID.ToString();
        
        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
                EventInfo eventInfo = new EventInfo();
                eventInfo.CourseName = (string)row.columnNameValuePairs["CourseName"];
                eventInfo.EventID = (int)row.columnNameValuePairs["LeagueEventID"];
                eventInfo.EventName = (string)row.columnNameValuePairs["EventName"];
                eventInfo.SeasonName = (string)row.columnNameValuePairs["SeasonName"];
                eventInfo.Date = ((DateTime)row.columnNameValuePairs["Date"]).ToString();               
                retVal.Add(eventInfo.EventID, eventInfo);
            }
        }

        return retVal;
    }

    public static EventInfo GetNextEvent(string LeagueID)
    {
        EventInfo eventInfo = new EventInfo();

        string sql = "SELECT Top 1 Courses.Name as CourseName, LeagueEvents.LeagueEventID, [Date], EventName, SeasonName FROM LeagueEvents "
                      + "Join Seasons "
                      + "on Seasons.SeasonID = LeagueEvents.SeasonID "
                      + "Join Courses "
                      + "on Courses.CourseID = LeagueEvents.CourseID "
                      + " where LeagueEventID not in (select LeagueEventID from scores) and LeagueEvents.LeagueID = " + LeagueID
                      + " Order By date desc";

        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {

                eventInfo.CourseName = (string)row.columnNameValuePairs["CourseName"];
                eventInfo.EventID = (int)row.columnNameValuePairs["LeagueEventID"];
                eventInfo.EventName = (string)row.columnNameValuePairs["EventName"];
                eventInfo.SeasonName = (string)row.columnNameValuePairs["SeasonName"];
                eventInfo.Date = ((DateTime)row.columnNameValuePairs["Date"]).ToString();

            }
        }

        return eventInfo;
    }

    public static EventInfo GetMostRecentEventWithScoresPosted(string leagueID)
    {
        EventInfo eventInfo = new EventInfo();

        string sql = "SELECT Top 1 Courses.Name as CourseName, LeagueEvents.LeagueEventID, [Date], EventName, SeasonName FROM LeagueEvents "
                      + "Join Seasons "
                      + "on Seasons.SeasonID = LeagueEvents.SeasonID "
                      + "Join Courses "
                      + "on Courses.CourseID = LeagueEvents.CourseID "
                      + " where LeagueEventID in (select LeagueEventID from scores) and LeagueEvents.LeagueID = " + leagueID
                      + " Order By date desc";

        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
               
                eventInfo.CourseName = (string)row.columnNameValuePairs["CourseName"];
                eventInfo.EventID = (int)row.columnNameValuePairs["LeagueEventID"];
                eventInfo.EventName = (string)row.columnNameValuePairs["EventName"];
                eventInfo.SeasonName = (string)row.columnNameValuePairs["SeasonName"];
                eventInfo.Date = ((DateTime)row.columnNameValuePairs["Date"]).ToString();
               
            }
        }

        return eventInfo;
    }

    public static DateTime? GetEventDate(int LeagueEventID)
    {
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
        List<Row> leagueEvents = queryWithEqualsConditions("LeagueEvents", conditions);

        if (leagueEvents[0].columnNameValuePairs["Date"] == DBNull.Value)
        {
            return null;
        }

        return (DateTime)leagueEvents[0].columnNameValuePairs["Date"];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="LeagueID"></param>
    /// <param name="currentSeasonID">Pass 0 to get events from All Seasons</param>
    /// <returns></returns>
    public static Dictionary<int, EventInfo> GetEventsWithScoresPosted(int LeagueID, int currentSeasonID)
    {
        Dictionary<int, EventInfo> retVal = new Dictionary<int,EventInfo>();
       
        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueID", LeagueID.ToString()));
        if (currentSeasonID != 0)
        {
            conditions.Add(new EqualsCondition("SeasonID", currentSeasonID.ToString()));
        } 
        List<Row> leagueEvents = queryWithEqualsConditions("LeagueEvents", conditions);


        foreach (Row dbEvent in leagueEvents)
        {
            int LeagueEventID = (int)dbEvent.columnNameValuePairs["LeagueEventID"];

            List<EqualsCondition> conditions2 = new List<EqualsCondition>();
            conditions2.Add(new EqualsCondition("LeagueEventID", LeagueEventID.ToString()));
            List<Row> scores = queryWithEqualsConditions("Scores", conditions2);

            if (dbEvent.columnNameValuePairs["Date"] != DBNull.Value && scores!= null && scores.Count > 0)
            {
                DateTime date = (DateTime)dbEvent.columnNameValuePairs["Date"];
                if (date < DateTime.Now)
                {
                    EventInfo newEvent = new EventInfo();
                    newEvent.Date = date.ToString();
                    if (dbEvent.columnNameValuePairs["CourseID"] != DBNull.Value)
                    {
                        int courseID = (int)dbEvent.columnNameValuePairs["CourseID"];
                        newEvent.CourseName = GetCourseName(courseID);
                    }
                    
                    newEvent.EventName = (string)dbEvent.columnNameValuePairs["EventName"];
                    retVal.Add(LeagueEventID, newEvent);
                }
            }
        }

        return retVal;
    }

    public static EventsAndGolfers GetEventsAndPlayers(int LeagueID, int currentSeasonID)
    {
        EventsAndGolfers retVal = new EventsAndGolfers();
        retVal.events = new Dictionary<int, EventInfo>();

        List<EqualsCondition> conditions = new List<EqualsCondition>();
        conditions.Add(new EqualsCondition("LeagueID", LeagueID.ToString()));
        conditions.Add(new EqualsCondition("SeasonID", currentSeasonID.ToString()));
        List<Row> leagueEvents = queryWithEqualsConditions("LeagueEvents", conditions);
        
        foreach (Row dbEvent in leagueEvents)
        {
            EventInfo newEvent = new EventInfo();
            if (dbEvent.columnNameValuePairs["Date"] != DBNull.Value)
            {
                newEvent.Date = ((DateTime)dbEvent.columnNameValuePairs["Date"]).ToString();
            }
            else
            {
                newEvent.Date = null;
            }

            if (dbEvent.columnNameValuePairs["CourseID"] != DBNull.Value)
            {
                newEvent.CourseID = (int)dbEvent.columnNameValuePairs["CourseID"];
                newEvent.CourseName = GetCourseName(newEvent.CourseID);
            }
            else
            {
                newEvent.CourseID = 0;
            }

            int LeagueEventID = (int)dbEvent.columnNameValuePairs["LeagueEventID"];
            newEvent.EventName = (string)dbEvent.columnNameValuePairs["EventName"];
            //newEvent.EventNumber = (int)dbEvent.columnNameValuePairs["EventNumber"];
            retVal.events.Add(LeagueEventID, newEvent);
        }

        retVal.golfers = GetGolferNamesAndIDs(LeagueID.ToString());

        return retVal;
    }


    private static string GetCourseName(int courseID)
    {
        try
        {
            List<Row> dbRetVal = queryWithWhereCondition("Courses", "CourseID = '" + courseID.ToString() + "';");
            return (string)dbRetVal[0].columnNameValuePairs["Name"];
        }
        catch
        {
            return null;
        }
    }

    public static Dictionary<int, Golfer> GetGolfersInfo(string LeagueID)
    {
        Dictionary<int, Golfer> retVal = new Dictionary<int, Golfer>();

        string sql = "Select Golfers.*, " +
                  "(Select count(*) " +
                  "from Scores " +
                  "where GolferID = Golfers.GolferID) As NumScores " +
                  "From Golfers " +
                  "join LeagueAffiliations on " +
                  "LeagueAffiliations.GolferID = Golfers.GolferID " +
                  "Where LeagueAffiliations.LeagueID = " + LeagueID +
                  " Order by LastName";

        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
                Golfer golfer = new Golfer();
                golfer.golferID = (int)row.columnNameValuePairs["GolferID"];
                golfer.firstName = (string)row.columnNameValuePairs["FirstName"];
                golfer.lastName = (string)row.columnNameValuePairs["LastName"];
                golfer.nickName = row.columnNameValuePairs["Nickname"] == DBNull.Value ? "" : (string)row.columnNameValuePairs["Nickname"];
                golfer.emailAddress = row.columnNameValuePairs["Email"] ==  DBNull.Value? "" :(string)row.columnNameValuePairs["Email"];
                golfer.numberOfRoundsPlayed = (int)row.columnNameValuePairs["NumScores"];

                retVal.Add(golfer.golferID, golfer);
            }
        }

        return retVal;
    }

    public static Dictionary<int, string> GetGolferNamesAndIDs(string LeagueID)
    {
        Dictionary<int, string> retVal = new Dictionary<int, string>();

        string sql = "  Select FirstName, LastName, Golfers.GolferID " 
                      + "From Golfers "
                      + "join LeagueAffiliations on "
                      + "LeagueAffiliations.GolferID = Golfers.GolferID "
                      + "Where LeagueAffiliations.LeagueID = '" + LeagueID + "' " 
                      + "Order by LastName";

        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
                int golferID = (int)row.columnNameValuePairs["GolferID"];
                string firstName = (string)row.columnNameValuePairs["FirstName"];
                string lastName = (string)row.columnNameValuePairs["LastName"];

                retVal.Add(golferID, firstName + " "  + lastName);
            }
        }

        return retVal;
    }


    public static List<int> GetNonPar3HolesOrderedByHandicap(int CourseID)
    {
        List<int> retVal = new List<int>();

        string sql = "  Select HoleNumber, MensHandicap "
                      + "From Holes "
                      + "Where CourseID = " + CourseID 
                      + " AND MensPar > 3 " 
                      +" Order by MensHandicap asc";

        List<Row> result = executeQuery(sql);
        if (result != null && result.Count != 0)
        {
            foreach (Row row in result)
            {
                retVal.Add((int)row.columnNameValuePairs["HoleNumber"]);           
            }
        }

        return retVal;
    }


    //public static Dictionary<int, string> GetGolferNamesAndIDs(string LeagueID)
    //{
    //    Dictionary<int, string> retVal = new Dictionary<int, string>();

    //    List<Row> dbRetVal = queryWithWhereCondition("LeagueAffiliations", "LeagueID = '" + LeagueID + "' Order by LastName");
    //    foreach (Row row in dbRetVal)
    //    {
    //        int golferID = (int)row.columnNameValuePairs["GolferID"];
    //        List<Row> golferTableRows = queryWithWhereCondition("Golfers", "GolferID = '" + golferID.ToString() + "';");
    //        string golferName = (string)golferTableRows[0].columnNameValuePairs["FirstName"];
    //        golferName += " " + (string)golferTableRows[0].columnNameValuePairs["LastName"];
    //        retVal.Add(golferID, golferName);
    //    }
    //    return retVal;
    //}

    public static SqlConnection OpenDBConnection()
    {
        SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
        sqlConnection.Open();
        return sqlConnection;
    }


    private static void updateRow(string tableName, string equalsCondition, Dictionary<string,string> ColumnNamesAndValuesToUpdate)
    {
        //build the SQL Statement to insert the row     
        String sqlString = "Update dbo.[" + tableName + "] Set ";

        foreach (String columnName in ColumnNamesAndValuesToUpdate.Keys)
        {
            sqlString += columnName + " = " +ColumnNamesAndValuesToUpdate[columnName] + ",";
        }

        sqlString = sqlString.TrimEnd(',');

        sqlString += "  Where " + equalsCondition; 

        SqlCommand command = new SqlCommand(sqlString, OpenDBConnection());
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            //may want to comment this out in case the log file gets too big.
            //writeToLogFile("Exception caught", "Inserting Row with SQL = " + sqlString, e.Message);
        }
    }

    private static void insertRow(string tableName, Dictionary<string, string> columnNamesAndValues)
    {
        //build the SQL Statement to insert the row     
        String sqlString = "insert into dbo.[" + tableName + "] (";
        String columnNames = "";
        String values = "";
        foreach (String columnName in columnNamesAndValues.Keys)
        {
            //columnNames += "[" + columnName + "],";
            columnNames += "\"" + columnName + "\"" + ",";
            values += "'" + columnNamesAndValues[columnName] + "',";
        }
        //remove the trailing character from both strings
        columnNames = columnNames.Remove(columnNames.Length - 1);
        values = values.Remove(values.Length - 1);

        //build the final command string
        sqlString += columnNames + ") Values (" + values + ")";

        SqlCommand command = new SqlCommand(sqlString, OpenDBConnection());
        try
        {
            command.ExecuteNonQuery();
        }
        catch 
        {
            //may want to comment this out in case the log file gets too big.
            //writeToLogFile("Exception caught", "Inserting Row with SQL = " + sqlString, e.Message);
        }
    }

    private static string EscapeSingleQuotes(string param)
    {
        string retVal = param.Replace("'", "''");
        return retVal;
    }

    private static int insertRowAndRetrieveIdentityColumn(string tableName, string IdentityColumnName, Dictionary<string, string> columnNamesAndValues)
    {
        //build the SQL Statement to insert the row     
        String sqlString = "insert into dbo.[" + tableName + "] (";
        String columnNames = "";
        String values = "";
        foreach (String columnName in columnNamesAndValues.Keys)
        {
            //columnNames += "[" + columnName + "],";
            columnNames += "\"" + columnName + "\"" + ",";
            string temp = columnNamesAndValues[columnName];
            temp = EscapeSingleQuotes(temp);
            values += "'" + temp + "',";
        }
        //remove the trailing character from both strings
        columnNames = columnNames.Remove(columnNames.Length - 1);
        values = values.Remove(values.Length - 1);

        //build the final command string
        sqlString += columnNames + ") OUTPUT INSERTED." + IdentityColumnName +" Values (" + values + ")";

        SqlCommand command = new SqlCommand(sqlString, OpenDBConnection());

        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);
       
        //SELECT SCOPE_IDENTITY(); 
        
        return (int)dataTable.Rows[0][0];
       
    }


    private static void RemoveRowIfExisting(string table, List<EqualsCondition> conditions)
    {
        string whereCondition = "";
        foreach (EqualsCondition condition in conditions)
        {
            whereCondition += condition.columnName + " = '" + condition.value + "' AND ";
        }
        //remove the last AND
        whereCondition = whereCondition.Remove(whereCondition.Length - 5);

        System.Data.SqlClient.SqlConnection sqlConnection1 = OpenDBConnection();

        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandText = "DELETE " + table + " WHERE " + whereCondition;
        cmd.Connection = sqlConnection1;

        //sqlConnection1.Open();
        cmd.ExecuteNonQuery();
        sqlConnection1.Close();
    }
   

  
    
    private struct EqualsCondition
    {
        public EqualsCondition(string ColumnName, string Value)
        {
            this.columnName = ColumnName;
            this.value = Value;
        }
        public string columnName;
        public string value;
    }

    private static List<Row> QueryForScoresByDate(int GolferID, int LeagueEventID)
    {
        string sqlString = "Select * From Scores, LeagueEvents Where LeagueEvents.LeagueEventID = Scores.LeagueEventID AND Scores.GolferID = '" + GolferID.ToString() + "' and Date <= (select Date from LeagueEvents where LeagueEventID = " + LeagueEventID.ToString() + ") Order by Date";
        return executeQuery(sqlString);
    }

    private static List<Row> queryWithEqualsConditions(string tablename,List<EqualsCondition> conditions)
    {
        string whereCondition = "";
        foreach(EqualsCondition condition in conditions)
        {
            whereCondition += condition.columnName + " = '" + condition.value + "' AND ";
        }
        //remove the last AND
        whereCondition = whereCondition.Remove(whereCondition.Length - 5);
        string sqlString = "Select * from dbo.[" + tablename + "] Where " + whereCondition + ";";    
       return executeQuery(sqlString);
    }

    //private static List<Row> getCountOfRowsWithWhereCondition(string tablename, string whereCondition)
    //{
    //    string sqlString = "Select Count(*) from dbo.[" + tablename + "] Where " + whereCondition + ";";
    //    return executeQuery(sqlString);
    //}

    private static List<Row> queryWithWhereCondition(string tablename,string whereCondition )
    {       
        string sqlString = "Select * from dbo.[" + tablename + "] Where " + whereCondition + ";"; 
        return executeQuery(sqlString);
    }

    private static List<Row> executeQuery(string sqlString)
    {
        List<Row> retVal = new List<Row>();
        try
        {
           
            SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
            SqlDataReader queryResults = sqlCommand.ExecuteReader();

            //format query results                               
            Row row;
            DataRowCollection resultsSchemaRows = queryResults.GetSchemaTable().Rows;
            while (queryResults.Read())
            {
                row = new Row();
                foreach (DataRow dataRow in resultsSchemaRows)
                {
                    if (!row.columnNameValuePairs.ContainsKey((string)dataRow["ColumnName"]))
                    {
                        row.columnNameValuePairs.Add((string)dataRow["ColumnName"], queryResults[(string)dataRow["ColumnName"]]);
                    }
                }
                retVal.Add(row);
            }
            //close connections
            queryResults.Close();
            sqlConnection.Close();
            return retVal;
        }
        catch (Exception e)
        {
            //not sure what the preferred web exception logging method is yet.
            //writeToLogFile("performing query :", sqlString, e.Message);
            return null;
        }

    }


    /// <summary>
    /// Gets the next row in a table with a Timestamp occuring after a specified time.
    /// </summary>
    /// <param name="TableName"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Row queryForNextRow(string TableName, DateTime time)
    {
        string whereCondition = "Timestamp = (Select min(Timestamp) from [" + TableName + "] where Timestamp > '" + time.ToString() + "')";
        List<Row> queryResults = queryWithWhereCondition(TableName, whereCondition);
        if (queryResults != null && queryResults.Count > 0)
        {
            return queryResults[0];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the first row in a table with a Timestamp occuring before a specified time.
    /// </summary>
    /// <param name="TableName"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Row queryForPreviousRow(string TableName, DateTime time)
    {
        string whereCondition = "Timestamp = (Select max(Timestamp) from [" + TableName + "] where Timestamp < '" + time.ToString() + "')";
        List<Row> queryResults = queryWithWhereCondition(TableName, whereCondition);
        if (queryResults != null && queryResults.Count > 0)
        {
            return queryResults[0];
        }
        else
        {
            return null;
        }
    }

    
    

    /// <summary>
    /// Retrieves a list of Rows containing a Timestamp and a particular Field from a particular Table that fall between a date range 
    /// </summary>
    /// <param name="tableName">Table to query</param>
    /// <param name="fieldName">Field to query for</param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static List<Row> queryForField(string tableName, string fieldName, DateTime startTime, DateTime endTime)
    {
        string whereCondition = "Timestamp > '" + startTime.ToString() + "' AND Timestamp < '" + endTime.ToString() +"'";
        List<Row> retVal = new List<Row>();
        string sqlString = "";
        try
        {
            //perform query
            sqlString = "Select Timestamp,[" + fieldName + "] from dbo.[" + tableName + "] Where " + whereCondition + ";";
            SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
            SqlDataReader queryResults = sqlCommand.ExecuteReader();

            //format query results                               
            Row row;
            DataRowCollection resultsSchemaRows = queryResults.GetSchemaTable().Rows;
            while (queryResults.Read())
            {
                row = new Row();
                foreach (DataRow dataRow in resultsSchemaRows)
                {
                    row.columnNameValuePairs.Add((string)dataRow["ColumnName"], queryResults[(string)dataRow["ColumnName"]]);
                }
                retVal.Add(row);
            }
            //close connections
            queryResults.Close();
            sqlConnection.Close();
            return retVal;
        }
        catch (Exception e)
        {
            //not sure what the preferred web exception logging method is yet.
            //writeToLogFile("performing query :", sqlString, e.Message);
            return null;
        }
    }

    private static SqlDataReader ExecuteStoredProcedure(string procedureName, string parameterName, object parameter)
    {
        SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
        sqlConnection.Open();
        SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);
        sqlCommand.CommandType = CommandType.StoredProcedure;
        if (!string.IsNullOrEmpty(parameterName))
        {
            SqlParameter param = new SqlParameter("@" + parameterName, parameter);
            param.Direction = ParameterDirection.Input;
            sqlCommand.Parameters.Add(param);
        }
        return sqlCommand.ExecuteReader();
    }

    private static SqlDataReader ExecuteSQLStatement(string SQLStatement)
    {
        SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
        sqlConnection.Open();
        SqlCommand sqlCommand = new SqlCommand(SQLStatement, sqlConnection);
        sqlCommand.CommandType = CommandType.Text;
        return sqlCommand.ExecuteReader();
    }


    /// <summary>
    /// Retrieves all rows from a table.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static List<Row> queryForAll(string tableName)
    {
        List<Row> retVal = new List<Row>();
        string sqlString = "";
        try
        {
            //perform query
           
            sqlString = "Select * from dbo.[" + tableName + "];";
            SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
            SqlDataReader queryResults = sqlCommand.ExecuteReader();

            //format query results                               
            Row row;
            DataRowCollection resultsSchemaRows = queryResults.GetSchemaTable().Rows;
            while (queryResults.Read())
            {
                row = new Row();
                foreach (DataRow dataRow in resultsSchemaRows)
                {
                    row.columnNameValuePairs.Add((string)dataRow["ColumnName"], queryResults[(string)dataRow["ColumnName"]]);
                }
                retVal.Add(row);
            }
            //close connections
            queryResults.Close();
            sqlConnection.Close();
            return retVal;
        }
        catch (Exception e)
        {
            //not sure what the preferred web exception logging method is yet.
            //writeToLogFile("performing query :", sqlString, e.Message);
            return null;
        }

    }

    
    /// <summary>
    /// Retrieves the most recent row from a table that has a timestamp field.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static Row query(string tableName)
    {
        Row retVal = new Row();

        string sqlString = "";
        try
        {
            //perform query
           
            sqlString = "Select * from dbo.[" + tableName + "] WHERE Timestamp IN (SELECT MAX(Timestamp) FROM dbo.[" + tableName + "]);";
            SqlConnection sqlConnection = new SqlConnection(databaseConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
            SqlDataReader queryResults = sqlCommand.ExecuteReader();

            //format query results                               
            queryResults.Read();
            foreach (DataRow dataRow in queryResults.GetSchemaTable().Rows)
            {
                retVal.columnNameValuePairs.Add((string)dataRow["ColumnName"], queryResults[(string)dataRow["ColumnName"]]);
            }
            
            //close connections
            queryResults.Close();
            sqlConnection.Close();
        }
        catch (Exception e)
        {
            //not sure what the preferred web exception logging method is yet.
            //writeToLogFile("performing query :", sqlString, e.Message);
            return null;
        }


        return retVal;
    }

    

    /// <summary>
    /// Stores the Data from a Row in the Database
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Key = Column Name, Value = Data
        /// </summary>
        public Dictionary<String, Object> columnNameValuePairs = new Dictionary<string, object>();
    }


}




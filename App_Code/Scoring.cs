using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Scoring
/// </summary>
public static class Scoring
{
    public class WeightedAvgHelper
    {
        public int numValues;
        public decimal averageValue;
    }

   

    public struct MatchupResults
    {
        public List<byte> player1Scores;// = new List<byte>();
        public List<byte> player2Scores;// = new List<byte>();
        public List<int> whoWonHole;// = new List<int>();//populate with 0,1,2 
        public int grossScorePlayer1;
        public int grossScorePlayer2;
        public int netScorePlayer1;
        public int netScorePlayer2;
        public decimal totalPtsPlayer1;
        public decimal totalPtsPlayer2;
        public int Team1PlayerID;
        public int Team2PlayerID; 
    }

    public struct EventStats
    {
        public Dictionary<int, int> netScores;
        public Dictionary<int, int> grossScores;
        public Dictionary<int, decimal> teamPts;
        public Dictionary<int, MatchupResults> matchupResultsA;
        public Dictionary<int, MatchupResults> matchupResultsB;
    

        public void initialize()
        {
            netScores = new Dictionary<int, int>();
            grossScores = new Dictionary<int, int>();
            teamPts = new Dictionary<int, decimal>();
            matchupResultsA = new Dictionary<int, MatchupResults>();
            matchupResultsB = new Dictionary<int, MatchupResults>();
        }
    }

    public class RoundInfo
    {
        public int GolferID;
        public int CourseID;
        public int LeagueEventID;
        
        //DateTime dateTime;
        //string EventName;
        //CourseInfo courseInfo;
    }

    public class LeagueStats
    {
        public Dictionary<RoundInfo, int> LowestGrossSingleRoundScores; //Key: EventInfo  Value: Score
        public Dictionary<RoundInfo, int> LowestNetSingleRoundScores; //Key: EventInfo  Value: Score
        public Dictionary<int, decimal> AvgScores; //Key: GolferID  Value: Avg Score(both nines)
        public Dictionary<int, int> NumberOfEventsPlayed; //Key: GolferID Value: Num Rounds 

        public Dictionary<int, int> Eagles;//Key: GolferID, Value: Num Eagles
        public Dictionary<int, int> Birdies;//Key: GolferID, Value: Num Birdies
        public Dictionary<int, int> Pars;//Key: GolferID, Value: Num Pars
        public Dictionary<int, int> Bogeys;//Key: GolferID, Value: Num Bogeys
        public Dictionary<int, int> DoubleBogeys;//Key: GolferID, Value: ....
        public Dictionary<int, int> TripleOrWorse;//Key: GolferID, Value: ....
        public Dictionary<int, int> Skins; //Key: GolferID, Value: Num Skins

        public Dictionary<int, Dictionary<int, List<int>>> SkinsByCourseID; //Key: CourseID, GolferID, Value: 9 element list representing the Num Skins this golfer has gotten on the particular hole

        public Dictionary<int, Dictionary<int, Decimal>> AverageScoreByCourse; //Key: CourseID Value: GolferID/AvgScore
        public Dictionary<int, Dictionary<int, int>> NumRoundsByGolferAndCourse; //Key: CourseID Value:GolferID/NumRounds 


        public Dictionary<int, List<byte>> LeagueRingerNineByCourse; //Key: CourseID, Value: Value:Best Score On Each Hole
        public Dictionary<int, List<Decimal>> LeagueHoleAveragesByCourse; //Key: CourseID  Value: Avg Score On Each Hole
        public Dictionary<int, int> NumberOfRoundsPlayedByCourse; //Key: CourseID  Value: Total Rounds On That Course 
        public Dictionary<int, List<byte>> LeagueHighScoreByCourse; //Key CourseID  Value: Worst Score On Each Hole 

        public Dictionary<int, WeightedAvgHelper> ParFiveScoringAvg; //Key GolferID  Value: Scoring Avg
        public Dictionary<int, WeightedAvgHelper> ParFourScoringAvg; //Key GolferID  Value: Scoring Avg 
        public Dictionary<int, WeightedAvgHelper> ParThreeScoringAvg; //Key GolferID  Value: Scoring Avg


        public Dictionary<int, Dictionary<int, List<byte>>> BestScoresByHole; //Key: GolferID, CourseID
        public Dictionary<int, Dictionary<int, List<byte>>> WorstScoresByHole; //Key: GolferID, CourseID
        public Dictionary<int, Dictionary<int, List<decimal>>> AverageScoresByHole;//Key: GolferID, CourseID
        public Dictionary<int, Dictionary<int, int>> ScoreByEvent;//Key: GolferID, LeagueEventID

        public Dictionary<int, Dictionary<int, int>> handicaps; //Key: GolferID, LeagueEventID 

        private int WorstScoreInLowestGrossSingleRoundScores;

        public void initialize()
        {
            LowestGrossSingleRoundScores = new Dictionary<RoundInfo, int>();
            LowestNetSingleRoundScores = new Dictionary<RoundInfo, int>(); 
            AvgScores = new Dictionary<int,decimal>();
            Birdies = new Dictionary<int,int>();
            Pars = new Dictionary<int,int>();
            Bogeys = new Dictionary<int,int>() ;
            DoubleBogeys = new Dictionary<int,int>();
            TripleOrWorse = new Dictionary<int,int>();
            Skins = new Dictionary<int,int>();
            NumberOfEventsPlayed = new Dictionary<int,int>();
            AverageScoreByCourse = new Dictionary<int,Dictionary<int,decimal>>();
            NumRoundsByGolferAndCourse = new Dictionary<int, Dictionary<int, int>>();

            NumberOfRoundsPlayedByCourse = new Dictionary<int, int>();
            LeagueRingerNineByCourse = new Dictionary<int, List<byte>>();
            LeagueHoleAveragesByCourse = new Dictionary<int, List<decimal>>();
            LeagueHighScoreByCourse = new Dictionary<int, List<byte>>();

            SkinsByCourseID = new Dictionary<int, Dictionary<int, List<int>>>();
            BestScoresByHole = new Dictionary<int, Dictionary<int, List<byte>>>(); //Key: GolferID, CourseID
            WorstScoresByHole = new Dictionary<int, Dictionary<int, List<byte>>>();//Key: GolferID, CourseID
            AverageScoresByHole = new Dictionary<int, Dictionary<int, List<decimal>>>();
            ScoreByEvent = new Dictionary<int, Dictionary<int, int>>();

            ParFiveScoringAvg = new Dictionary<int, WeightedAvgHelper>();
            ParFourScoringAvg = new Dictionary<int, WeightedAvgHelper>();
            ParThreeScoringAvg = new Dictionary<int, WeightedAvgHelper>();
        }

        //Dictionary<RoundInfo, int> LowestGrossSingleRoundScores, Dictionary<RoundInfo, int> LowestNetSingleRoundScores,
        public void SetupDictionaries(List<int> courseIDList, List<int> golferIDs)
        {
            foreach (int courseID in courseIDList)
            {
                NumRoundsByGolferAndCourse[courseID] = new Dictionary<int, int>();
                NumberOfRoundsPlayedByCourse.Add(courseID, 0);
                
                LeagueRingerNineByCourse.Add(courseID, new List<byte> { 99, 99, 99, 99, 99, 99, 99, 99, 99 });
                LeagueHoleAveragesByCourse.Add(courseID, new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                LeagueHighScoreByCourse.Add(courseID, new List<byte> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });

                SkinsByCourseID[courseID] = new Dictionary<int, List<int>>();
                BestScoresByHole[courseID] = new Dictionary<int, List<byte>>();
                WorstScoresByHole[courseID] = new Dictionary<int, List<byte>>();
                AverageScoresByHole[courseID] = new Dictionary<int, List<decimal>>();
                AverageScoreByCourse[courseID] = new Dictionary<int, decimal>();

                foreach (int golferID in golferIDs)
                {
                    SkinsByCourseID[courseID].Add(golferID, new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    BestScoresByHole[courseID].Add(golferID , new List<byte> { 99, 99, 99, 99, 99, 99, 99, 99, 99 });   
                    WorstScoresByHole[courseID].Add(golferID, new List<byte> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    AverageScoresByHole[courseID].Add(golferID, new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                    NumRoundsByGolferAndCourse[courseID].Add(golferID, 0);
                    AverageScoreByCourse[courseID].Add(golferID, 0);
                    ScoreByEvent[golferID] = new Dictionary<int, int>();
                }
            }

            foreach (int golferID in golferIDs)
            {
                Skins.Add(golferID, 0);
                Birdies.Add(golferID, 0);
                Pars.Add(golferID, 0);
                Bogeys.Add(golferID, 0);
                DoubleBogeys.Add(golferID, 0);
                TripleOrWorse.Add(golferID, 0);
                NumberOfEventsPlayed.Add(golferID, 0);
                AvgScores.Add(golferID, 0);
                ParFiveScoringAvg.Add(golferID, new WeightedAvgHelper());            
                ParFourScoringAvg.Add(golferID, new WeightedAvgHelper());
                ParThreeScoringAvg.Add(golferID, new WeightedAvgHelper());
            }
        }

        //private static void ModifyHoleByHoleScores(int CourseID, int GolferID, List<byte> scores,  //Inputs ONLY
        //Dictionary<int, Dictionary<int, int>> NumRoundsByGolferAndCourse, Dictionary<int, int> NumberOfRoundsPlayedByCourse,
        //Dictionary<int,List<byte>> BestScoresByHole, Dictionary<int, List<byte>> WorstScoresByHole, Dictionary<int, List<decimal>> AverageScoresByHole,
        //Dictionary<int, List<byte>> LeagueRingerNineByCourse, Dictionary<int, List<Decimal>> LeagueHoleAveragesByCourse, Dictionary<int, List<byte>> LeagueHighHoleScoresByCourse)
        public void ModifyHoleByHoleScores(int CourseID, int GolferID, List<byte> scores)
        {
            for (int i = 0; i < 9; i++)
            {
                if (BestScoresByHole[CourseID][GolferID][i] > scores[i])
                {
                    BestScoresByHole[CourseID][GolferID][i] = scores[i];
                }

                if (LeagueRingerNineByCourse[CourseID][i] > scores[i])
                {
                    LeagueRingerNineByCourse[CourseID][i] = scores[i];
                }

                if (WorstScoresByHole[CourseID][GolferID][i] < scores[i])
                {
                    WorstScoresByHole[CourseID][GolferID][i] = scores[i];
                }

                if (LeagueHighScoreByCourse[CourseID][i] < scores[i])
                {
                    LeagueHighScoreByCourse[CourseID][i] = scores[i];
                }

                AverageScoresByHole[CourseID][GolferID][i] = WeightedAverage(scores[i], AverageScoresByHole[CourseID][GolferID][i], NumRoundsByGolferAndCourse[CourseID][GolferID]);  //((AverageScoresByHole[CourseID][GolferID][i] * NumberOfRoundsPlayedByCourse[CourseID]) + scores[i]) / (NumberOfRoundsPlayedByCourse[CourseID] + 1);
                LeagueHoleAveragesByCourse[CourseID][i] = ((LeagueHoleAveragesByCourse[CourseID][i] * NumberOfRoundsPlayedByCourse[CourseID]) + scores[i]) / (NumberOfRoundsPlayedByCourse[CourseID] + 1);
            }
            NumberOfRoundsPlayedByCourse[CourseID]++;
        }

        private decimal WeightedAverage(decimal newValue, decimal prevAvg, decimal numbersOfValuesInPrevAverage)
        {
            return (newValue + (prevAvg * numbersOfValuesInPrevAverage)) / (numbersOfValuesInPrevAverage + 1); 
        }

        public int ComputeParStatsAndTotalScore(CourseInfo courseInfo, int GolferID, int leagueEventID, List<byte> scores, int handicap)
        {
            int totalScore = 0;
            for (int i = 0; i < 9; i++)
            {
                if (courseInfo.holeParRatings[i] == (scores[i] + 1))
                {
                    Birdies[GolferID]++;
                }
                else if (courseInfo.holeParRatings[i] == (scores[i]))
                {
                    Pars[GolferID]++;
                }
                else if ((courseInfo.holeParRatings[i] + 1) == scores[i])
                {
                    Bogeys[GolferID]++;
                }
                else if ((courseInfo.holeParRatings[i] + 2) == scores[i])
                {
                    DoubleBogeys[GolferID]++;
                }
                else if ((courseInfo.holeParRatings[i] + 2) < scores[i])
                {
                    TripleOrWorse[GolferID]++;
                }
                totalScore += scores[i];
                
                if (courseInfo.holeParRatings[i] == 5)
                {
                    ParFiveScoringAvg[GolferID].averageValue = (scores[i] + (ParFiveScoringAvg[GolferID].averageValue * ParFiveScoringAvg[GolferID].numValues)) / (ParFiveScoringAvg[GolferID].numValues + 1);
                    ParFiveScoringAvg[GolferID].numValues++;
                }
                else if (courseInfo.holeParRatings[i] == 4)
                {
                    ParFourScoringAvg[GolferID].averageValue = (scores[i] + (ParFourScoringAvg[GolferID].averageValue * ParFourScoringAvg[GolferID].numValues)) / (ParFourScoringAvg[GolferID].numValues + 1);
                    ParFourScoringAvg[GolferID].numValues++;
                }
                else if (courseInfo.holeParRatings[i] == 3)
                {
                    ParThreeScoringAvg[GolferID].averageValue = (scores[i] + (ParThreeScoringAvg[GolferID].averageValue * ParThreeScoringAvg[GolferID].numValues)) / (ParThreeScoringAvg[GolferID].numValues + 1);
                    ParThreeScoringAvg[GolferID].numValues++;
                }
            }
           
            AvgScores[GolferID] = (totalScore + (AvgScores[GolferID] * NumberOfEventsPlayed[GolferID])) / (NumberOfEventsPlayed[GolferID] + 1);
            NumberOfEventsPlayed[GolferID]++;
            AverageScoreByCourse[courseInfo.CourseID][GolferID] = (totalScore + (NumRoundsByGolferAndCourse[courseInfo.CourseID][GolferID] * AverageScoreByCourse[courseInfo.CourseID][GolferID])) / (NumRoundsByGolferAndCourse[courseInfo.CourseID][GolferID] + 1);
            NumRoundsByGolferAndCourse[courseInfo.CourseID][GolferID]++;
            UpdateBestRounds(totalScore, totalScore - handicap, GolferID, courseInfo.CourseID, leagueEventID, 30);
            ScoreByEvent[GolferID].Add(leagueEventID, totalScore);
            return totalScore;
        }

        private void UpdateBestRounds(int gross_score, int netScore, int GolferID, int CourseID, int LeagueEventID, int NumberOfRows)
        {
            RoundInfo newRoundInfo = new RoundInfo();
            newRoundInfo.CourseID = CourseID;
            newRoundInfo.GolferID = GolferID;
            newRoundInfo.LeagueEventID = LeagueEventID;
            if (LowestGrossSingleRoundScores.Count < NumberOfRows)
            {
                LowestGrossSingleRoundScores.Add(newRoundInfo, gross_score);
            }
            else
            {
                var items = from pair in LowestGrossSingleRoundScores
                            orderby pair.Value descending
                            select pair;

                if (items.ToList()[0].Value > gross_score)
                {
                    LowestGrossSingleRoundScores.Add(newRoundInfo, gross_score);
                    LowestGrossSingleRoundScores.Remove(items.ToList()[0].Key);
                }
            }

            if (LowestNetSingleRoundScores.Count < NumberOfRows)
            {
                LowestNetSingleRoundScores.Add(newRoundInfo, netScore);
            }
            else
            {
                var items = from pair in LowestNetSingleRoundScores
                            orderby pair.Value descending
                            select pair;

                if (items.ToList()[0].Value > netScore)
                {                    
                    LowestNetSingleRoundScores.Add(newRoundInfo, netScore);
                    LowestNetSingleRoundScores.Remove(items.ToList()[0].Key);
                }
            }  
        }
    }

    //public class PlayerStats
    //{
    //    public Dictionary<int, List<byte>> BestScoresByHole;
    //    public Dictionary<int, List<byte>> WorstScoresByHole;
    //    public Dictionary<int, List<decimal>> AverageScoresByHole;

    //    public int bestNineHoleScore;
    //    public int worstNineHoleScore;

    //    public Dictionary<int, int> ScoreByEvent; //Key:LeagueEvent, Value: Score
    //    //Dictionary<int, Decimal> HandicapByEvent;

    //    public void initialize()
    //    {
    //         BestScoresByHole = new Dictionary<int, List<byte>>();
    //         WorstScoresByHole = new Dictionary<int, List<byte>>();
    //         AverageScoresByHole = new Dictionary<int,List<decimal>>();

    //        ScoreByEvent = new Dictionary<int,int>();
    //       // Dictionary<int, Decimal> HandicapByEvent = new Dictionary<int,decimal>();
    //    }
    //}

    //public static Dictionary<int, decimal> SortDictionaryByAscendingValue(Dictionary<int, decimal> dictionary)
    //{
    //    var items = from pair in dictionary
    //                orderby pair.Value ascending
    //                select pair;

    //    Dictionary<int, decimal> retVal = new Dictionary<int, decimal>();
    //}


    public static LeagueStats GetSeasonStats(int LeagueID, int SeasonID, List<int> CourseIDs)
    {
        LeagueStats retVal = new LeagueStats();
        //playerStats = new Dictionary<int,PlayerStats>();
        retVal.initialize();

        Dictionary<int, string> golfers = DatabaseFunctions.GetGolferNamesAndIDs(LeagueID.ToString());
        var settings = DatabaseFunctions.GetLeagueSettings(LeagueID);
        bool useCurrentRound = settings.ContainsKey("UseCurrentRoundForHandicap") ? bool.Parse(settings["UseCurrentRoundForHandicap"]) : true;

        useCurrentRound = true;
        retVal.SetupDictionaries(CourseIDs, golfers.Keys.ToList());
        retVal.handicaps = GetHandicaps(LeagueID, useCurrentRound);

        //InitializeDictionaries(CourseIDs, golfers.Keys, 
        //retVal.Birdies, retVal.Pars, retVal.Bogeys, 
        //retVal.DoubleBogeys, retVal.TripleOrWorse, retVal.AvgScores, retVal.NumberOfEventsPlayed,
        //retVal.NumRoundsByGolferAndCourse, retVal.NumberOfRoundsPlayedByCourse,
        //Dictionary<int, List<byte>> LeagueRingerNineByCourse, Dictionary<int, List<Decimal>> LeagueHoleAveragesByCourse, Dictionary<int, List<byte>> LeagueHighHoleScoresByCourse)


        Dictionary<int, EventInfo> events = DatabaseFunctions.GetEventsWithScoresPosted(LeagueID, SeasonID);

        foreach (int LeagueEventID in events.Keys)
        {
            //Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(LeagueEventID);

            Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(LeagueEventID);
            CourseInfo courseInfo = DatabaseFunctions.GetCourseInfo(LeagueEventID);

            List<int> eventSkins = calculateSkins(scores);
            int holeIndex = 0;
            foreach (int GolferID in eventSkins)
            {
                if (GolferID != 0)
                {
                    retVal.Skins[GolferID]++;
                    retVal.SkinsByCourseID[courseInfo.CourseID][GolferID][holeIndex]++;
                }
                holeIndex++;
            }

            //Dictionary<int, int> handicaps = GetPlayerHandicapsForEvent(LeagueEventID, scores.Keys.ToList());

            //Dictionary<int, int> subs = DatabaseFunctions.GetSubs(LeagueEventID);
            //List<int> noShows = DatabaseFunctions.GetNoShows(LeagueEventID);
            //Dictionary<int, int> handicaps = DatabaseFunctions.GetHandicapOverrides(LeagueEventID);
            //int Team1PlayerA_ID, Team1PlayerB_ID, Team2PlayerA_ID, Team2PlayerB_ID;
            //Add No shows to Handicaps and scores
            //scores.Add(0, Scoring.GetNoShowScores(courseInfo));
            //handicaps.Add(0, 0);

            int index = 0;
            //foreach matchup
            foreach (int GolferID in scores.Keys)
            {
                //if(!playerStats.ContainsKey(GolferID))
                //{
                //    PlayerStats newPlayerStats = new PlayerStats();
                //    newPlayerStats.initialize();
                //    playerStats.Add(GolferID, newPlayerStats);
                //}
                //ModifyHoleByHoleScores(courseInfo.CourseID, GolferID, scores[GolferID], retVal.NumRoundsByGolferAndCourse, retVal.NumberOfRoundsPlayedByCourse, playerStats[GolferID].BestScoresByHole, playerStats[GolferID].WorstScoresByHole, playerStats[GolferID].AverageScoresByHole, retVal.LeagueRingerNineByCourse, retVal.LeagueHoleAveragesByCourse, retVal.LeagueHighScoreByCourse);
                retVal.ModifyHoleByHoleScores(courseInfo.CourseID, GolferID, scores[GolferID]);
                //int totalScore = ComputeParStatsAndTotalScore(courseInfo, GolferID, scores[GolferID], retVal.Birdies, retVal.Pars, retVal.Bogeys, retVal.DoubleBogeys, retVal.TripleOrWorse, retVal.AvgScores, retVal.NumberOfEventsPlayed);
                retVal.ComputeParStatsAndTotalScore(courseInfo, GolferID, LeagueEventID, scores[GolferID], retVal.handicaps[GolferID][LeagueEventID]);
                

                //if (playerStats[GolferID].bestNineHoleScore > totalScore)
                //{
                //    playerStats[GolferID].bestNineHoleScore = totalScore;
                //}

                //if (playerStats[GolferID].worstNineHoleScore > totalScore)
                //{
                //    playerStats[GolferID].worstNineHoleScore = totalScore;
                //}
            }
        }

        return retVal;

    }

    


    private static int ComputeParStatsAndTotalScore(CourseInfo courseInfo, int GolferID, List<byte> scores, //Inputs
        Dictionary<int, int> Birdies, Dictionary<int, int> Pars, Dictionary<int, int> Bogeys, Dictionary<int, int> DoubleBogeys, Dictionary<int, int> TripleOrWorse, 
        Dictionary<int, decimal> AvgScores, Dictionary<int, int> NumberOfEventsPlayed)
    {
        int totalScore = 0;
        for (int i = 0; i < 9; i++)
        {
            if (courseInfo.holeParRatings[i] == (scores[i] + 1))
            {
                if (Birdies.ContainsKey(GolferID))
                {
                    Birdies[GolferID]++;
                }
                else
                {
                    Birdies[GolferID] = 1;
                }
            }
            else if (courseInfo.holeParRatings[i] == (scores[i]))
            {
                if (Pars.ContainsKey(GolferID))
                {
                    Pars[GolferID]++;
                }
                else
                {
                    Pars[GolferID] = 1;
                }
            }
            else if ((courseInfo.holeParRatings[i] + 1) == scores[i])
            {
                if (Bogeys.ContainsKey(GolferID))
                {
                    Bogeys[GolferID]++;
                }
                else
                {
                    Bogeys[GolferID] = 1;
                }
            }
            else if ((courseInfo.holeParRatings[i] + 2) == scores[i])
            {
                if (DoubleBogeys.ContainsKey(GolferID))
                {
                    DoubleBogeys[GolferID]++;
                }
                else
                {
                    DoubleBogeys[GolferID] = 1;
                }
            }
            else if ((courseInfo.holeParRatings[i] + 2) < scores[i])
            {
                if (TripleOrWorse.ContainsKey(GolferID))
                {
                    TripleOrWorse[GolferID]++;
                }
                else
                {
                    TripleOrWorse[GolferID] = 1;
                }
            }
            totalScore += scores[i];
        }
        if (AvgScores.ContainsKey(GolferID))
        {
            AvgScores[GolferID] = totalScore + (AvgScores[GolferID] * NumberOfEventsPlayed[GolferID]) / (NumberOfEventsPlayed[GolferID] + 1);
            NumberOfEventsPlayed[GolferID]++;
        }
        else
        {
            AvgScores[GolferID] = totalScore;
            NumberOfEventsPlayed[GolferID] = 1;
        }
       
        return totalScore;
    }

     //Dictionary<RoundInfo, int> LowestGrossSingleRoundScores, Dictionary<RoundInfo, int> LowestNetSingleRoundScores,
    private static void InitializeDictionaries(List<int> courseIDList, List<int> golferIDs, 
        Dictionary<int, int> Birdies, Dictionary<int, int> Pars, Dictionary<int, int> Bogeys, 
        Dictionary<int, int> DoubleBogeys, Dictionary<int, int> TripleOrWorse, Dictionary<int, decimal> AvgScores, Dictionary<int, int> NumberOfEventsPlayed,
         Dictionary<int, Dictionary<int, int>> NumRoundsByGolferAndCourse, Dictionary<int, int> NumberOfRoundsPlayedByCourse,
        Dictionary<int, List<byte>> BestScoresByHole, Dictionary<int, List<byte>> WorstScoresByHole, Dictionary<int, List<decimal>> AverageScoresByHole,
        Dictionary<int, List<byte>> LeagueRingerNineByCourse, Dictionary<int, List<Decimal>> LeagueHoleAveragesByCourse, Dictionary<int, List<byte>> LeagueHighHoleScoresByCourse)
    {
        //Birdies = new Dictionary<int, int>();
        //Pars = new Dictionary<int, int>();
        //Bogeys = new Dictionary<int, int>(); 
        //DoubleBogeys = new Dictionary<int, int>(); 
        //TripleOrWorse = new Dictionary<int, int>(); 

        foreach (int courseID in courseIDList)
        {
            NumRoundsByGolferAndCourse[courseID] = new Dictionary<int, int>();
            NumberOfRoundsPlayedByCourse.Add(courseID, 0);
            BestScoresByHole.Add(courseID, new List<byte>{99,99,99,99,99,99,99,99,99});
            WorstScoresByHole.Add(courseID, new List<byte>{0,0,0,0,0,0,0,0,0});
            AverageScoresByHole.Add(courseID, new List<decimal>{0,0,0,0,0,0,0,0,0});
            LeagueRingerNineByCourse.Add(courseID, new List<byte>{99,99,99,99,99,99,99,99,99});
            LeagueHoleAveragesByCourse.Add(courseID, new List<decimal>{0,0,0,0,0,0,0,0,0});
            LeagueHighHoleScoresByCourse.Add(courseID, new List<byte>{0,0,0,0,0,0,0,0,0});

            foreach (int golferID in golferIDs)
            {
                NumRoundsByGolferAndCourse[courseID].Add(golferID, 0);
            }
        }

        foreach (int golferID in golferIDs)
        {
            Birdies.Add(golferID, 0);
            Pars.Add(golferID, 0);
            Bogeys.Add(golferID, 0);
            DoubleBogeys.Add(golferID, 0);
            NumberOfEventsPlayed.Add(golferID, 0);
        }
    }


    private static List<decimal> ConvertByteListToDecimal(List<byte> input)
    {
        List<decimal> retVal = new List<decimal>();
        foreach(byte b in input)
        {
            retVal.Add((decimal)b);
        }
        return retVal;
    }


    public static EventStats GetEventResults(int LeagueID, int LeagueEventID, Dictionary<int, int> handicaps)
    {
        EventStats retVal = new EventStats();
        retVal.initialize();

        Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(LeagueEventID);
        Dictionary<string, string> LeagueSettings = DatabaseFunctions.GetLeagueSettings(LeagueID);
        Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(LeagueEventID);
        CourseInfo courseInfo = DatabaseFunctions.GetCourseInfo(LeagueEventID);
        Dictionary<int, int> subs = DatabaseFunctions.GetSubs(LeagueEventID);

        if (handicaps == null)
        {
            handicaps = Scoring.GetPlayerHandicapsForEvent(LeagueID, LeagueEventID, scores.Keys.ToList());
        }

        if (LeagueSettings.ContainsKey("MaxHandicap"))
        {
            int maxhandicap = int.Parse(LeagueSettings["MaxHandicap"]);
            List<int> keys = handicaps.Keys.ToList();
            foreach (int key in keys)
            {
                if (handicaps[key] > maxhandicap)
                {
                    handicaps[key] = maxhandicap;
                }
            }
        }

        int Team1PlayerA_ID, Team1PlayerB_ID, Team2PlayerA_ID, Team2PlayerB_ID;
        //Add No shows to Handicaps and scores
        var noShowScores = Scoring.GetNoShowScores(courseInfo);
        scores.Add(0, noShowScores);
        scores.Add(-1, noShowScores);
        scores.Add(-2, noShowScores);

        if (LeagueSettings.ContainsKey("NewScoring"))
        {
            handicaps.Add(0, 7);//no show
            handicaps.Add(-1, 7);//blind1
            handicaps.Add(-2, 7);//blind2
        }
        else
        {
            handicaps.Add(0, 0);//no show
            handicaps.Add(-1, 9);//blind1
            handicaps.Add(-2, 9);//blind2
        }
       
        Dictionary<int, Team> teams = DatabaseFunctions.GetTeams(LeagueID);

        int index = 0;
        //foreach matchup
        foreach (int team1ID in matchups.Keys)
        {
            Scoring.GetGolferIDs(team1ID, LeagueEventID ,subs, handicaps, scores, out Team1PlayerA_ID, out Team1PlayerB_ID);
            Scoring.GetGolferIDs(matchups[team1ID], LeagueEventID ,subs, handicaps, scores, out Team2PlayerA_ID, out Team2PlayerB_ID);     
            Scoring.MatchupResults results_A = Scoring.GetMatchupResults(scores[Team1PlayerA_ID], scores[Team2PlayerA_ID], handicaps[Team1PlayerA_ID], handicaps[Team2PlayerA_ID], courseInfo);
            results_A.Team1PlayerID = Team1PlayerA_ID;
            results_A.Team2PlayerID = Team2PlayerA_ID;
            Scoring.MatchupResults results_B = Scoring.GetMatchupResults(scores[Team1PlayerB_ID], scores[Team2PlayerB_ID], handicaps[Team1PlayerB_ID], handicaps[Team2PlayerB_ID], courseInfo);
            results_B.Team1PlayerID = Team1PlayerB_ID;
            results_B.Team2PlayerID = Team2PlayerB_ID;
            index++;

            if (LeagueSettings.ContainsKey("SubPtsLimit"))
            {
                results_A = AdjustResultsForSubs(results_A, WasPlayerSubbing(Team1PlayerA_ID, team1ID, subs, teams), WasPlayerSubbing(Team2PlayerA_ID, matchups[team1ID], subs, teams));
                results_B = AdjustResultsForSubs(results_B, WasPlayerSubbing(Team1PlayerB_ID, team1ID, subs, teams), WasPlayerSubbing(Team2PlayerB_ID, matchups[team1ID], subs, teams));
            }

            if (LeagueSettings.ContainsKey("NewScoring"))
            {
                results_A = AdjustResultsForNoShows(results_A, Team1PlayerA_ID, Team2PlayerA_ID);
                results_B = AdjustResultsForNoShows(results_B, Team1PlayerB_ID, Team2PlayerB_ID);
            }

            if (Team1PlayerA_ID > 0)
            {
                retVal.grossScores.Add(Team1PlayerA_ID, results_A.grossScorePlayer1);
                retVal.netScores.Add(Team1PlayerA_ID, results_A.netScorePlayer1);
            }
            if (Team2PlayerA_ID > 0)
            {
                retVal.grossScores.Add(Team2PlayerA_ID, results_A.grossScorePlayer2);
                retVal.netScores.Add(Team2PlayerA_ID, results_A.netScorePlayer2);
            }
            if (Team1PlayerB_ID > 0)
            {
                retVal.netScores.Add(Team1PlayerB_ID, results_B.netScorePlayer1);
                retVal.grossScores.Add(Team1PlayerB_ID, results_B.grossScorePlayer1);
            }

            if (Team2PlayerB_ID > 0)
            {
                retVal.grossScores.Add(Team2PlayerB_ID, results_B.grossScorePlayer2);
                retVal.netScores.Add(Team2PlayerB_ID, results_B.netScorePlayer2);
            }

            int team1MedalPlayPts, team2MedalPlayPts;
            CalculateMedalPlay(out team1MedalPlayPts,out team2MedalPlayPts, results_A, results_B, LeagueSettings);

            retVal.teamPts.Add(team1ID, results_A.totalPtsPlayer1 + results_B.totalPtsPlayer1 + team1MedalPlayPts);
            retVal.teamPts.Add(matchups[team1ID], results_A.totalPtsPlayer2 + results_B.totalPtsPlayer2 + team2MedalPlayPts);
            retVal.matchupResultsA.Add(team1ID, results_A);
            retVal.matchupResultsB.Add(team1ID, results_B);
            
           
        }
        
        return retVal;
    }

   

    public static void CalculateMedalPlay(out int Team1MedalPlayPts, out int Team2MedalPlayPts, MatchupResults resultsA, MatchupResults resultsB, Dictionary<string, string> LeagueSettings)
    {
        Team1MedalPlayPts = 0;
        Team2MedalPlayPts = 0;

        var Team1NoShow = false; 
        var Team2NoShow = false; 

        if(LeagueSettings.ContainsKey("NewScoring"))
        {
            Team1NoShow = resultsA.Team1PlayerID == 0 && resultsB.Team1PlayerID == 0;
            Team2NoShow = resultsA.Team2PlayerID == 0 && resultsB.Team2PlayerID == 0;
        }


        if(Team1NoShow && !Team2NoShow)
        {
            Team2MedalPlayPts = 2;
        }
        else if(!Team1NoShow && Team2NoShow)
        {
            Team1MedalPlayPts = 2;
        }

        else if ((resultsA.netScorePlayer1 + resultsB.netScorePlayer1) < (resultsA.netScorePlayer2 + resultsB.netScorePlayer2))
        {
            Team1MedalPlayPts = 2;
        }
        else if ((resultsA.netScorePlayer1 + resultsB.netScorePlayer1) > (resultsA.netScorePlayer2 + resultsB.netScorePlayer2))
        {
            Team2MedalPlayPts = 2;
        }
        else
        {
            Team1MedalPlayPts = 1;
            Team2MedalPlayPts = 1;
        }
    }

    public static Dictionary<int, int> GetHandicapsForEventFromHandicapByGolferIDDictionary(Dictionary<int, Dictionary<int, int>> handicapsByGolferID, int leagueEventID)
    {
        Dictionary<int, int> retVal = new Dictionary<int,int>();
        foreach (int GolferID in handicapsByGolferID.Keys)
        {
            if (handicapsByGolferID[GolferID].ContainsKey(leagueEventID))
            {
                retVal.Add(GolferID, handicapsByGolferID[GolferID][leagueEventID]);
            }
        }
        return retVal;
    }

   
    private static bool WasPlayerSubbing(int playerID, int teamID, Dictionary<int, int> subs, Dictionary<int, Team> teams)
    {
        if(teamID == 65 && playerID==256)
        {
            return false;
        }
        if(subs.Values.Contains(playerID))
        {
            return teams[teamID].Golfer3ID != playerID && teams[teamID].Golfer2ID != playerID  && teams[teamID].Golfer1ID != playerID; 
        }
        return false;
    }

    private static MatchupResults AdjustResultsForSubs(MatchupResults results, bool player1WasSub, bool player2WasSub)
    {
        if(player1WasSub && results.totalPtsPlayer1 > 5)
        {
            results.totalPtsPlayer1 = 5;
            results.totalPtsPlayer2 = 4;
        }

        if(player2WasSub && results.totalPtsPlayer2 > 5)
        {
            results.totalPtsPlayer2 = 5;
            results.totalPtsPlayer1 = 4;
        }

        return results;
    }


    private static MatchupResults AdjustResultsForNoShows(MatchupResults results, int player1Id, int player2Id)
    {
        if (player1Id == 0 && results.totalPtsPlayer1 > 2)
        {
            results.totalPtsPlayer1 = 2;
        }

        if (player2Id == 0 && results.totalPtsPlayer2 > 2)
        {
            results.totalPtsPlayer2 = 2;
        }

        return results;
    }


    public static MatchupResults GetMatchupResults(List<byte> player1Scores, List<byte> player2Scores, int player1Handicap, int player2Handicap, CourseInfo courseInfo)
    {
        MatchupResults retVal = new MatchupResults();
        retVal.whoWonHole = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        retVal.grossScorePlayer1 = 0;
        retVal.grossScorePlayer2 = 0;
        retVal.netScorePlayer1 = 0;
        retVal.netScorePlayer2 = 0;
        retVal.totalPtsPlayer1 = 0;
        retVal.totalPtsPlayer2 = 0;
        retVal.player1Scores = player1Scores;
        retVal.player2Scores = player2Scores;

        int player1AdjustedHandicap = 0, player2AdjustedHandicap = 0;

        if (player1Handicap > player2Handicap)
        {
            player1AdjustedHandicap = player1Handicap - player2Handicap;
            player2AdjustedHandicap = 0;
        }
        else if(player1Handicap < player2Handicap)
        {
            player2AdjustedHandicap = player2Handicap - player1Handicap;
            player1AdjustedHandicap = 0;
        }

        List<int> player1HandicapAdjustments = GetHandicapAdjustments(courseInfo, player1AdjustedHandicap);
        List<int> player2HandicapAdjustments = GetHandicapAdjustments(courseInfo, player2AdjustedHandicap);

        for (int i = 0; i < 9; i++)
        {
            retVal.grossScorePlayer1 += player1Scores[i];
            retVal.grossScorePlayer2 += player2Scores[i];

            int player1Net = player1Scores[i] - player1HandicapAdjustments[i];
            int player2Net = player2Scores[i] - player2HandicapAdjustments[i];

            //retVal.netScorePlayer1 += player1Net;
            //retVal.netScorePlayer2 += player2Net;
         
            if (player1Net < player2Net)
            {
                retVal.totalPtsPlayer1 += 1;
                retVal.whoWonHole[i] = 1;
            }
            else if (player1Net > player2Net)
            {
                retVal.totalPtsPlayer2 += 1;
                retVal.whoWonHole[i] = 2;
            }
            else//must be equal
            {
                retVal.totalPtsPlayer1 += (decimal).5;
                retVal.totalPtsPlayer2 += (decimal).5;
                retVal.whoWonHole[i] = 0;
            }
        }

        retVal.netScorePlayer1 = retVal.grossScorePlayer1 - player1Handicap;
        retVal.netScorePlayer2 = retVal.grossScorePlayer2 - player2Handicap;

        return retVal;
    }

    
    /// <summary>
    /// Counts the current events scores in the handicap calculation
    /// </summary>
    /// <param name="LeagueID"></param>
    /// <returns></returns>
    //Golfer, LeagueEventID, Handicap
    public static Dictionary<int, Dictionary<int, int>> GetHandicaps(int LeagueID, bool useCurrentRound)
    {
        //golfer ID, event ID, Handicap For Event
        Dictionary<int, Dictionary<int, int>> retVal = new Dictionary<int,Dictionary<int,int>>();
        
        Dictionary<int, CourseInfo> courseInfos = DatabaseFunctions.GetCourses(LeagueID);
    
        Dictionary<int, List<HandicapCalculationData>> handicapCalculationDataDictionary = DatabaseFunctions.GetHandicapCalculationData(LeagueID);

        Dictionary<string, string> leagueSettings = DatabaseFunctions.GetLeagueSettings(LeagueID);

        int escMaxOverPar = leagueSettings.ContainsKey("ESCMaxOverPar") ? int.Parse(leagueSettings["ESCMaxOverPar"]) : 3;

        foreach (int GolferID in handicapCalculationDataDictionary.Keys)
        {
            retVal.Add(GolferID, new Dictionary<int, int>());
            foreach (HandicapCalculationData handicapCalculationData in handicapCalculationDataDictionary[GolferID])
            {
                CalculateHandicapDifferential(handicapCalculationData, courseInfos, escMaxOverPar);
            }

            AggregateHandicapDifferentials(retVal[GolferID], handicapCalculationDataDictionary[GolferID], useCurrentRound);
        }

        return retVal;

    }

    private static void AggregateHandicapDifferentials(Dictionary<int, int> playerHandicapsByEvent, List<HandicapCalculationData> handicapCalculationData, bool useCurrentRound)
    {
        List<double> handicapDifferentials = new List<double>();
        for (int i = 0; i < handicapCalculationData.Count; i++)
        {
            if (handicapDifferentials.Count == 0 || useCurrentRound)
            {
                handicapDifferentials.Add(handicapCalculationData[i].HandicapDifferential);
            }
            else if(i > 1)
            {
                handicapDifferentials.Add(handicapCalculationData[i - 1].HandicapDifferential);
            }

            if (handicapDifferentials.Count > 20)//Make sure we use no more than 20 rounds in the past to calculate handicap
            {
                handicapDifferentials.RemoveAt(0);
            }

            if (handicapCalculationData[i].HandicapOvveride == null)
            {
                int numberOfRoundsToUse = GolfLeagueWebsiteGlobals.GetPortableHandicapNumberOfRoundsToUse(handicapDifferentials.Count);

                //copy the list so we don't disturb its chronological ordering when sorting
                List<double> handicapDifferentialsSorted = new List<double>(handicapDifferentials);  //.Sort();
                handicapDifferentialsSorted.Sort();

                double differentialTotal = 0;
                for (int j = 0; j < numberOfRoundsToUse; j++)
                {
                    differentialTotal += handicapDifferentialsSorted[j];
                }

                if (numberOfRoundsToUse == 1)
                {
                    differentialTotal = differentialTotal * .8;
                }
                else if (numberOfRoundsToUse == 2)
                {
                    differentialTotal = differentialTotal * .9;
                }

                playerHandicapsByEvent.Add(handicapCalculationData[i].LeagueEventID, Convert.ToInt32(differentialTotal / numberOfRoundsToUse));
            }
            else
            {
                playerHandicapsByEvent.Add(handicapCalculationData[i].LeagueEventID, (int)handicapCalculationData[i].HandicapOvveride);
            }
        }
    }


    private static void CalculateHandicapDifferential(HandicapCalculationData handicapCalculationData, Dictionary<int, CourseInfo> courseInfos, int EscMaxOverParScore)
    {
        //trim out any scores greater than triple bogeys
        int totalAdjustedScore = 0;
        for (int holeIndex = 0; holeIndex < 9; holeIndex++)
        {
            byte MaxScore = (byte)(courseInfos[handicapCalculationData.CourseID].holeParRatings[holeIndex] + EscMaxOverParScore);
            if (handicapCalculationData.scores[holeIndex] > MaxScore)
            {
                totalAdjustedScore += MaxScore;
            }
            else
            {
                totalAdjustedScore += handicapCalculationData.scores[holeIndex];
            }
        }
        //calculate differential
        handicapCalculationData.HandicapDifferential = totalAdjustedScore - courseInfos[handicapCalculationData.CourseID].courseHandicapRating;
    }

   
    public static Dictionary<int, int> GetPlayerHandicapsForEvent(int LeaugeID, int LeagueEventID, List<int> GolferIDs)
    {
        Dictionary<int, int> retVal = new Dictionary<int, int>(); //DatabaseFunctions.GetHandicapOverrides(LeagueEventID);

        var settings = DatabaseFunctions.GetLeagueSettings(LeaugeID);
        bool useCurrentRound = settings.ContainsKey("UseCurrentRoundForHandicap") ? bool.Parse(settings["UseCurrentRoundForHandicap"]) : true;

        var handicaps = GetHandicaps(LeaugeID, useCurrentRound);
               
        foreach (int GolferID in GolferIDs)
        {
            if (!retVal.ContainsKey(GolferID))
            {
                retVal.Add(GolferID, handicaps[GolferID][LeagueEventID]);
            }
        }
        return retVal;
    }


    public static void GetGolferIDs(int teamID, int LeagueEventID, Dictionary<int, int> subs, Dictionary<int, int> handicaps,  Dictionary<int, List<byte>> scores, out int A_PlayerID, out int B_PlayerID)
    {
        Dictionary<int, string> golfers = DatabaseFunctions.GetGolfers(teamID);

        if(teamID == 85)
        {
            golfers.Remove(299);
        }

        //if(golfers.Count > 2)
        //{
        //    int keyToRemove = 0;
        //    foreach(int GolferID in golfers.Keys)
        //    {
        //        if(subs.ContainsKey(GolferID))
        //        {
        //            keyToRemove = GolferID;
        //        }
        //    }
        //    if(keyToRemove == 0)
        //    {
        //        foreach (int GolferID in golfers.Keys)
        //        {
        //            if (!scores.ContainsKey(GolferID))
        //            {
        //                keyToRemove = GolferID;
        //            }
        //        }
        //    }
        //    golfers.Remove(keyToRemove);
        //}

        int golferID1 = golfers.Keys.ToList()[0];
        int golferID2 = golfers.Keys.ToList()[1];
        

        if (subs.ContainsKey(golferID1))
        {
            golferID1 = subs[golferID1];
        }
        if (subs.ContainsKey(golferID2))
        {
            golferID2 = subs[golferID2];
        }

        //Check for No Shows
        if (!scores.ContainsKey(golferID1))
        {
            golferID1 = 0;
        }
        if (!scores.ContainsKey(golferID2))
        {
            golferID2 = 0;
        }

        if (golferID2 == 0 || handicaps[golferID1] < handicaps[golferID2])
        {
            A_PlayerID = golferID1;
            B_PlayerID = golferID2;
        }
        else
        {
            A_PlayerID = golferID2;
            B_PlayerID = golferID1;
        }
    }

    private static List<int> GetHandicapAdjustments(CourseInfo courseInfo, int golferHandicap)
    {
        List<int> retVal = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            retVal.Add(0);
        }

        Dictionary<int, int> holeToHandicaptoIndex = new Dictionary<int, int>();
        for (int i = 0; i < 9; i++)
        {
            holeToHandicaptoIndex.Add(courseInfo.holeHandicaps[i], i);
        }
        List<int> holeHandicapList = holeToHandicaptoIndex.Keys.ToList();
        holeHandicapList.Sort();
        //holeHandicapList.Reverse();

        if (golferHandicap > 0)
        {
            for (int i = 0; i < golferHandicap; i++)
            {
                int holeToAddStrokeTo = holeToHandicaptoIndex[holeHandicapList[i % 9]];
                retVal[holeToAddStrokeTo] += 1;
            }
        }

        return retVal;
    }

    //no show defaults to 0 handicap golfer shooting a bogey on each hole
    public static List<byte> GetNoShowScores(CourseInfo courseInfo)
    {
        List<byte> retVal = new List<byte>();
        foreach (int parRating in courseInfo.holeParRatings)
        {
            retVal.Add((byte)(parRating + 1));
        }
        return retVal;
    }

    //public static List<byte> GetBlindScores(CourseInfo courseInfo)
    //{
    //    List<byte> retVal = new List<byte>();
    //    foreach (int parRating in courseInfo.holeParRatings)
    //    {
    //        retVal.Add((byte)(parRating + 2));
    //    }
    //    return retVal;
    //}

    public static List<int> calculateSkins(Dictionary<int, List<byte>> scores)
    {
        List<int> retVal = new List<int>();

        int LowestScoreOnHole = 99;
        int GolferIDWinningSkin = 0;
        //bool skinWon = true;
        for (int i = 0; i < 9; i++)
        {
            LowestScoreOnHole = 99;
            foreach (int golferID in scores.Keys)
            {
                if (scores[golferID][i] < LowestScoreOnHole)
                {
                    LowestScoreOnHole = scores[golferID][i];
                    GolferIDWinningSkin = golferID;
                }
                else if (scores[golferID][i] == LowestScoreOnHole)
                {
                    GolferIDWinningSkin = 0;
                }
            }
            retVal.Add(GolferIDWinningSkin);
        }

        return retVal;
    }

    public static void ApplyHandicapsToScoresForHandicappedSkins(Dictionary<int, List<byte>> scores, Dictionary<int, int> handicaps, CourseInfo courseInfo)
    {
        List<int> HolesToAdjust = DatabaseFunctions.GetNonPar3HolesOrderedByHandicap(courseInfo.CourseID);
        Dictionary<int, List<byte>> retVal = new Dictionary<int, List<byte>>();

        foreach(int golferID in scores.Keys)
        {
            if (golferID != 0)
            {
                decimal golferAdjustedHandicap = handicaps[golferID];
                golferAdjustedHandicap = golferAdjustedHandicap * (decimal).7;

                foreach (int holeIndex in HolesToAdjust)
                {
                    if (golferAdjustedHandicap < 1)
                    {
                        break;
                    }
                    scores[golferID][holeIndex - 1]--;
                    golferAdjustedHandicap--;
                }
            }    
        }
    }

    //public static List<int> OrderHolesByHandicapAndRemovePar3s(CourseInfo courseInfo)
    //{
    //    List<int> retVal = new List<int>();
    //    Dictionary<int, int> holeindexAndHandicaps = new Dictionary<int, int>();

    //    for (int i = 0; i < 9; i++)
    //    {
    //        if(courseInfo.holeParRatings[i] > 3)
    //        {
    //            holeindexAndHandicaps.Add(i, courseInfo.holeHandicaps[i]);
    //        }
    //    }

    //    holeindexAndHandicaps

    //    return retVal;
    //}


}
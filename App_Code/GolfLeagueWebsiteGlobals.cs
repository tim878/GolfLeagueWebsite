using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GolfLeagueWebsiteGlobals
/// </summary>
public static class GolfLeagueWebsiteGlobals
{
    //public enum Tees {PRO, ADV, MENS, SENIOR, WOMENS};
    public static Dictionary<string, int> Tees = new Dictionary<string, int>() 
    {
        {"PRO", 0 },
        {"ADV", 1 },
        {"MENS", 2 },
        {"SENIOR", 3 },
        {"WOMENS", 4 }
    };

    public struct Matchup
    {
        public int Team1ID;
        public int Team2ID;
        public string Team1Name;
        public string Team2Name;
    }

    public static int GetPortableHandicapNumberOfRoundsToUse(int NumOfRoundsAvailable)
    {
        if (NumOfRoundsAvailable < 3)
        {
            return 1;
        }
        else if (NumOfRoundsAvailable < 5)
        {
            return 2;
        }
        else if (NumOfRoundsAvailable < 6)
        {
            return 3;
        }
        else if (NumOfRoundsAvailable < 8)
        {
            return 4;
        }
        else if (NumOfRoundsAvailable < 10)
        {
            return 5;
        }
        else if (NumOfRoundsAvailable < 11)
        {
            return 6;
        }
        else if (NumOfRoundsAvailable < 13)
        {
            return 7;
        }
        else if (NumOfRoundsAvailable < 15)
        {
            return 8;
        }
        else if (NumOfRoundsAvailable < 16)
        {
            return 9;
        }
        else if (NumOfRoundsAvailable < 18)
        {
            return 10;
        }
        else if (NumOfRoundsAvailable < 20)
        {
            return 11;
        }
        else 
        {
            return 12;
        }
    }

   

    

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string LeagueName;
    public int? LeagueID;
    public string currentUserID;



    protected void Page_Init(object sender, EventArgs e)
    {
        FindLeagueID();

        if (LeagueName != null)
        {
            GolfLeagueName.Text = "League: " + LeagueName;
        }

    }

    private void FindLeagueID()
    {
        //URL contains leagueID
        if (Request.QueryString["LeagueID"] != null)
        {
            LeagueName = DatabaseFunctions.ValidateLeagueID(Request.QueryString["LeagueID"]);
            Session["LeagueID"] = LeagueID;
            Session["LeagueName"] = LeagueName;
            return;
        }

        //Session contains League ID
        if (Session["LeagueID"] != null)
        {
            try
            {
                LeagueID = (int)Session["LeagueID"];
                LeagueName = DatabaseFunctions.ValidateLeagueID(LeagueID.ToString());
                return;
            }
            catch { }
        }

        //User logged in
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            currentUserID = Membership.GetUser().ProviderUserKey.ToString();
            try
            {
                LeagueID = DatabaseFunctions.GetLeagueIDFromLoggedInUserID(currentUserID);
                Session["LeagueID"] = LeagueID;
                LeagueName = DatabaseFunctions.ValidateLeagueID(LeagueID.ToString());
                Session["LeagueName"] = LeagueName;
                return;
                //redirect to the manage page and put LeagueID in URL
            }
            catch { }
        }

        //Redirect to No League, unless they are going to a page that supports not having a leagueID
        if (!Request.Url.ToString().Contains("NoLeague") && !Request.Url.ToString().Contains("Login") && !Request.Url.GetLeftPart(UriPartial.Path).Contains("Register"))
        {
            //Redirect to league selection page
            Response.Redirect("NoLeague.aspx", true);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}

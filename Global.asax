<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        //RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }

    public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        //routes.MapPageRoute("Default", "Leagues/{LeagueName}/{categoryName}", "~/categoriespage.aspx");
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        try
        {
            string originalPath = Request.Url.ToString();
            if (originalPath.ToLower().Contains("/league/"))
            {
                var tokens = originalPath.Split(new[] { "/league/" }, StringSplitOptions.None);
                string leagueName = tokens[1].Substring(0, tokens[1].IndexOf("/"));
                int leagueID = DatabaseFunctions.GetLeagueIDFromShortName(leagueName);
                HttpContext.Current.Items["LeagueID"] = leagueID;

                string pageName = tokens[1].Substring(tokens[1].IndexOf("/"));
                string newPath = pageName; //+ "?LeagueID=" + leagueID.ToString();

                Context.RewritePath(newPath);
            }
        }
        catch { }
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        HttpContext context = HttpContext.Current;
        if (context != null && context.Session != null)
        {
            if (HttpContext.Current.Items["LeagueID"] != null)
            {
                Session["LeagueID"] = HttpContext.Current.Items["LeagueID"];
            }
        }
    }

</script>

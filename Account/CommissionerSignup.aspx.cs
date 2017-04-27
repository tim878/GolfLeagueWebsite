using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommissionerSignup : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void AddCommissioner(object sender, CommandEventArgs e)
    {
        //TODO validate textbox input
        int GolferID = DatabaseFunctions.AddGolfer(TextBox_FirstName.Text, TextBox_LastName.Text, TextBox_NickName.Text, TextBox_EmailAddress.Text);
        

       
    }
}
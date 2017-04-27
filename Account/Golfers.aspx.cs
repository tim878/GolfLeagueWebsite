using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Golfers : System.Web.UI.Page
{
    private int leagueID;

    //private Dictionary<Button, List<TextBox>> 
    private Dictionary<string, TextBox> textBoxLookup = new Dictionary<string, TextBox>(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        List<Golfer> golfers = DatabaseFunctions.GetGolfersInfo(leagueID.ToString()).Values.ToList();
        

        var items = from golfer in golfers
                    orderby golfer.lastName ascending
                    select golfer;

        foreach (Golfer golfer in items)
        {
            TableRow tr = new TableRow();
            //TableCell tcFirstName = new TableCell();
            //TextBox textBoxFirstName = new TextBox();

            TableCell numRdsCell = CreateTableCellWithTextbox("numRds_" + golfer.golferID, golfer.numberOfRoundsPlayed.ToString(), 50);
            numRdsCell.Width = 50;
            tr.Cells.Add(numRdsCell);

            string textBox1ID = "textBoxFirstName_" + golfer.golferID.ToString();
            tr.Cells.Add(CreateTableCellWithTextbox(textBox1ID, golfer.firstName));

            string textBox2ID = "textBoxLastName_" + golfer.golferID.ToString();
            tr.Cells.Add(CreateTableCellWithTextbox(textBox2ID, golfer.lastName));

            string textBox3ID = "textBoxNickName_" + golfer.golferID.ToString();
            tr.Cells.Add(CreateTableCellWithTextbox(textBox3ID, golfer.nickName));

            string textBox4ID = "textBoxEmail_" + golfer.golferID.ToString();
            tr.Cells.Add(CreateTableCellWithTextbox(textBox4ID, golfer.emailAddress));

            Button SaveButton = new Button();
            SaveButton.Text = "Save";
            SaveButton.Width = 70;
            SaveButton.ID = "SaveButton_" + golfer.golferID.ToString();
            SaveButton.Style.Add("display", "none");
            SaveButton.Command += SaveEdit;

            HtmlButton CancelButton = new HtmlButton();
            CancelButton.ID = "CancelButton_" + golfer.golferID.ToString();
            //CancelButton.Style("width") = "2em";
            //Button CancelButton = new Button();
            //CancelButton.Text = "Cancel";
            //CancelButton.Width = 70;
            CancelButton.ID = "CancelButton_" + golfer.golferID.ToString();
            CancelButton.Style.Add("display", "none");
            CancelButton.InnerText = "Cancel";

            HtmlButton EditButton = new HtmlButton();
            EditButton.InnerText = "Edit";
            //EditButton.t = "Edit";
            //EditButton.Width = 70;
            EditButton.ID = "EditButton_" + golfer.golferID.ToString();
            EditButton.Attributes.Add("onclick", "javascript:if(!editActive){ActivateTextBox('MainContent_" + textBox1ID + "');ActivateTextBox('MainContent_" + textBox2ID + "');ActivateTextBox('MainContent_" + textBox3ID + "');ActivateTextBox('MainContent_" + textBox4ID + "');ShowControl('MainContent_" + SaveButton.ID + "');ShowControl('MainContent_" + CancelButton.ID + "');HideControl('MainContent_" + EditButton.ID + "');}editActive=true;");
            //CancelButton.PostBackUrl = "javascript:HideControl('MainContent_" + CancelButton.ID + "');HideControl('MainContent_" + SaveButton.ID + "');ShowControl('MainContent_" + EditButton.ID + ");";
            CancelButton.Attributes.Add("onclick", "javascript:HideControl('MainContent_" + CancelButton.ID + "');HideControl('MainContent_" + SaveButton.ID + "');ShowControl('MainContent_" + EditButton.ID + "');DeactivateTextBox('MainContent_" + textBox1ID + "');DeactivateTextBox('MainContent_" + textBox2ID + "');DeactivateTextBox('MainContent_" + textBox3ID + "');DeactivateTextBox('MainContent_" + textBox4ID + "');editActive=false;");

            Table ButtonTable = new Table();
            TableRow buttonRow = new TableRow();
            TableCell tcButton = new TableCell();
            TableCell tcButton1 = new TableCell();
            TableCell tcButton2 = new TableCell();
            TableCell tcButton3 = new TableCell();
            tcButton1.Controls.Add(EditButton);
            tcButton2.Controls.Add(SaveButton);
            tcButton3.Controls.Add(CancelButton);
            buttonRow.Cells.Add(tcButton1);
            buttonRow.Cells.Add(tcButton2);
            buttonRow.Cells.Add(tcButton3);
            ButtonTable.Rows.Add(buttonRow);
            tcButton.Controls.Add(ButtonTable);
            tr.Cells.Add(tcButton);

            //Table_MainContent.Rows.AddAt(1, tr);
            Table_MainContent.Rows.Add(tr);
        }
    }



    private TableCell CreateTableCellWithTextbox(string textboxID, string initialTextBoxText)
    {
 
        TableCell tc = new TableCell();
        TextBox tb = new TextBox();
        tb.ID = textboxID;
        textBoxLookup.Add(textboxID, tb);
        tb.Text = initialTextBoxText;
        tb.Enabled = false;
        tc.Controls.Add(tb);
        return tc;
    }

    private TableCell CreateTableCellWithTextbox(string textboxID, string initialTextBoxText, int textBoxWidth)
    {
        TableCell tc = new TableCell();
        TextBox tb = new TextBox();
        tb.ID = textboxID;
        tb.Text = initialTextBoxText;
        tb.Enabled = false;
        tb.Width = 50;
        tc.Controls.Add(tb);
        return tc;
    }



    private bool ValidateInput()
    {
        if (TextBox_FirstName.Text == "" || TextBox_FirstName.Text.Length > 200)
        {
            return false;
        }
        if (TextBox_LastName.Text == "" || TextBox_LastName.Text.Length > 200)
        {
            return false;
        }
        return true;
    }

    private void ClearTextBoxes()
    {
        TextBox_FirstName.Text = "";
        TextBox_LastName.Text = "";
        TextBox_NickName.Text = "";
        TextBox_EmailAddress.Text = "";
    }

    protected void SavePlayer(object sender, CommandEventArgs e)
    {
        //TODO validate textbox input
        if (!ValidateInput())
        {
            Response.Write("<script language='javascript'>alert('Golfer Not Added.  Both First and Last Names are required.');</script>");
            return;
        }
        int GolferID = DatabaseFunctions.AddGolfer(TextBox_FirstName.Text, TextBox_LastName.Text, TextBox_NickName.Text, TextBox_EmailAddress.Text);
        int SeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        DatabaseFunctions.AddLeagueAffiliation(GolferID, leagueID);
        Response.Write("<script language='javascript'>alert('Golfer Added Successfully.');</script>");
        ClearTextBoxes();
    }

    protected void SaveEdit(object sender, CommandEventArgs e)
    {
        Button button = (Button)sender;
        string golferID = button.ID.Split('_')[1];
        DatabaseFunctions.EditGolfer(int.Parse(golferID), textBoxLookup["textBoxFirstName_" + golferID].Text, textBoxLookup["textBoxLastName_" + golferID].Text, textBoxLookup["textBoxNickName_" + golferID].Text, textBoxLookup["textBoxEmail_" + golferID].Text); 
    }
}
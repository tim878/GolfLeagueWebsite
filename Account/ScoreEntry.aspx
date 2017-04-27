<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ScoreEntry.aspx.cs" Inherits="ScoreEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table cellpadding="20">
        <tr>
            <td colspan="3">
               <b style="font-size:20px;color:Black">Score Entry Page</b>
            </td>
        </tr>
        <tr>
            <td>                
                <asp:RadioButton id="radiobutton_csv" Text="Enter as CSV" Checked="false" runat="server" GroupName="EntryMode" OnCheckedChanged="ScoreEntryModeChanged" AutoPostBack="true"/>
                &nbsp
                <asp:RadioButton id="radiobutton_holebyhole" Text="Enter Hole by Hole" Checked="true" runat="server" GroupName="EntryMode" OnCheckedChanged="ScoreEntryModeChanged" AutoPostBack="true"/>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                         <td>
                            <b style="font-size:14px;color:Black">Select Event to Enter Scores For:</b>
                         </td>
                         <td>
                            <asp:DropDownList Width="440" ID="Dropdown_LeagueEvent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdown_LeagueEvent_SelectedIndexChanged" />
                         </td>
                    </tr>
                    <tr>
                       <td colspan="2">
                            <asp:Panel ID="Panel_EventDetails" runat=server Visible=false>
                                   <table>
                                        <tr>
                                            <td>
                                                <asp:Calendar ID="Calendar_EventDate" runat="server" Width="200px" /> 
                                            </td>
                                            <td>
                                                <asp:DropDownList Width="300" ID="DropDownCourseSelect" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Button_SaveEventDetails" runat="server" OnCommand="SaveEventDetails" Text="Save" style="font-size:90%;font-weight:bold;width:100px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />   
                                            </td>
                                        </tr>
                                   </table>
                            </asp:Panel> 
                       </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <asp:Panel ID="Panel_SelectMatchup" runat=server Visible=false>
                            <table>
                                <tr>
                                    <td><b style="font-size:14px;color:Black">Select Matchup to Enter Scores For:</b></td>
                                    <td>
                                        <asp:DropDownList Width="440" ID="Dropdown_MatchupSelect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdown_MatchupSelect_SelectedIndexChanged" Enabled="false"/>
                                    </td>
                                </tr>
                            </table>  
                        </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
                <td colspan="2">
                    <table id="ScoreEntryTable" cellpadding="10" visible="false" runat="server">
                        <tr>
                            <td>
                                <b style="font-size:14px;color:Black">Player</b>
                            </td>
                            <td>
                                <b style="font-size:14px;color:Black">Enter Sub</b>
                            </td>
                            <td>
                                <b style="font-size:14px;color:Black">No Show</b>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">1</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">2</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">3</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">4</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">5</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">6</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">7</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">8</b>
                                            </td>
                                             <td Width="35">
                                                <b style="font-size:14px;color:Black">9</b>
                                            </td>
                                    </tr>    
                                </table>
                               
                            </td>
                           
                            <td>
                                <b style="font-size:14px;color:Black">Total</b>
                            </td>
                            <td>
                                <b style="font-size:14px;color:Black">Handicap</b></br>
                                 <b style="font-size:14px;color:Black">Override</b></br>
                                  <b style="font-size:14px;color:Black">(Optional)</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label_Player1Name" runat="server" Visible="true"></asp:Label>
                                <asp:DropDownList Width="200" ID="DropdownPlayer1Sub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                            </td>
                            <td>
                                 <asp:Button ID="Button_Player1Sub" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                 <asp:Button ID="Button_Player1NoShow" runat="server" OnCommand="NoShowButtonClicked" Text="X" style="font-size:190%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                <asp:Table id="Table_P1" runat="server">
                                    <asp:TableRow>
                                             <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole1" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole2" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole3" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole4" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole5" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole6" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole7" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole8" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P1_Hole9" runat="server" Width="30"/>
                                            </asp:TableCell>
                                    </asp:TableRow>    
                                </asp:Table>
                                <asp:TextBox ID="TextBox_Player1_CSV" runat="server" Width="350" Visible="false"/>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_P1_Total" runat="server" Width="30"/>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_P1_HandicapOvveride" runat="server" Width="30"/>
                            </td>
                        </tr>

                         <tr>
                            <td>
                                <asp:Label ID="Label_Player2Name" runat="server"></asp:Label>
                                <asp:DropDownList Width="200" ID="DropdownPlayer2Sub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                            </td>
                             <td>
                                 <asp:Button ID="Button_Player2Sub" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                 <asp:Button ID="Button_Player2NoShow" runat="server" OnCommand="NoShowButtonClicked" Text="X" style="font-size:190%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                             <td>
                                <asp:Table id="Table_P2" runat="server">
                                    <asp:TableRow>
                                             <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole1" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole2" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole3" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole4" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole5" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole6" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole7" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole8" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P2_Hole9" runat="server" Width="30"/>
                                            </asp:TableCell>
                                    </asp:TableRow>    
                                </asp:Table>
                                <asp:TextBox ID="TextBox_Player2_CSV" runat="server" Width="350" Visible="false"/>                                
                            </td>
                            
                            <td>
                                <asp:TextBox ID="TextBox_P2_Total" runat="server" Width="30"/>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_P2_HandicapOvveride" runat="server" Width="30"/>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label_Player3Name" runat="server"></asp:Label>
                                <asp:DropDownList Width="200" ID="DropdownPlayer3Sub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                            </td>
                             <td>
                                 <asp:Button ID="Button_Player3Sub" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                 <asp:Button ID="Button_Player3NoShow" runat="server" OnCommand="NoShowButtonClicked" Text="X" style="font-size:190%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                <asp:Table id="Table_P3" runat="server">
                                    <asp:TableRow>
                                             <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole1" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole2" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole3" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole4" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole5" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole6" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole7" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole8" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P3_Hole9" runat="server" Width="30"/>
                                            </asp:TableCell>
                                    </asp:TableRow>    
                                </asp:Table>
                                <asp:TextBox ID="TextBox_Player3_CSV" runat="server" Width="350" Visible="false"/>                                
                            </td>
                            
                            <td>
                                <asp:TextBox ID="TextBox_P3_Total" runat="server" Width="30"/>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_P3_HandicapOvveride" runat="server" Width="30"/>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label_Player4Name" runat="server"></asp:Label>
                                <asp:DropDownList Width="200" ID="DropdownPlayer4Sub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                            </td>
                             <td>
                                 <asp:Button ID="Button_Player4Sub" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                 <asp:Button ID="Button_Player4NoShow" runat="server" OnCommand="NoShowButtonClicked" Text="X" style="font-size:190%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                            <td>
                                <asp:Table id="Table_P4" runat="server">
                                    <asp:TableRow>
                                             <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole1" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole2" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole3" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole4" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole5" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole6" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole7" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole8" runat="server" Width="30"/>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:TextBox ID="TextBox_P4_Hole9" runat="server" Width="30"/>
                                            </asp:TableCell>
                                    </asp:TableRow>    
                                </asp:Table>
                                <asp:TextBox ID="TextBox_Player4_CSV" runat="server" Width="350" Visible="false"/>                                
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_P4_Total" runat="server" Width="30"/>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_P4_HandicapOvveride" runat="server" Width="30"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="Update" runat="server" OnCommand="UpdateTotal" Text="Update Total" style="font-size:90%;font-weight:bold;width:100px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />   
                            </td>
                            <td colspan="3" align="center">
                                <asp:Button ID="Save" runat="server" OnCommand="SaveScores" Text="Save" style="font-size:90%;font-weight:bold;width:100px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />   
                            </td>  
                        </tr>       
                    </table>
                </td>
            </tr>
            
       
        <%--<tr>
            <td colspan="3">
                <table id="CSVTable" cellpadding="20" visible="false" runat="server">
                    <tr>
                        <td colspan=3>
                            <b style="font-size:14px;color:Black">Enter Scores In CSV Format (Without Total)</b>
                        </td>
                        <td>
                            <b style="font-size:14px;color:Black">Handicap Override (Optional)</b>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                         <asp:Label ID="Label_Player1Name_CSV" runat="server"></asp:Label>
                         <asp:DropDownList Width="200" ID="DropdownPlayer1SubCSV" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                        </td>
                         <td>
                                 <asp:Button ID="Button_Player1SubCSV" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                        <td>
                             <asp:TextBox ID="TextBox_Player1_CSV" runat="server" Width="300"/>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_P1_HandicapOvverideCSV" runat="server" Width="30"/>
                        </td>
                    </tr>
                     <tr>
                        <td>
                         <asp:Label ID="Label_Player2Name_CSV" runat="server"></asp:Label>
                         <asp:DropDownList Width="200" ID="DropdownPlayer2SubCSV" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                        </td>
                         <td>
                                 <asp:Button ID="Button_Player2SubCSV" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                        <td>
                             <asp:TextBox ID="TextBox_Player2_CSV" runat="server" Width="300"/>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_P2_HandicapOvverideCSV" runat="server" Width="30"/>
                        </td>
                    </tr>
                     <tr>
                        <td>
                         <asp:Label ID="Label_Player3Name_CSV" runat="server"></asp:Label>
                         <asp:DropDownList Width="200" ID="DropdownPlayer3SubCSV" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                        </td>
                         <td>
                                 <asp:Button ID="Button_Player3SubCSV" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                        <td>
                             <asp:TextBox ID="TextBox_Player3_CSV" runat="server" Width="300"/>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_P3_HandicapOvverideCSV" runat="server" Width="30"/>
                        </td>
                    </tr>
                     <tr>
                        <td>
                         <asp:Label ID="Label_Player4Name_CSV" runat="server"></asp:Label>
                         <asp:DropDownList Width="200" ID="DropdownPlayer4SubCSV" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SubSelected" Visible="false"  />
                        </td>
                         <td>
                                 <asp:Button ID="Button_Player4SubCSV" runat="server" OnCommand="EnterSubButtonClicked" Text="Sub" style="font-size:90%;font-weight:bold;width:50px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                            </td>
                        <td>
                             <asp:TextBox ID="TextBox_Player4_CSV" runat="server" Width="300"/>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_P4_HandicapOvverideCSV" runat="server" Width="30"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan=4 align="center">
                           <asp:Button ID="Button1" runat="server" OnCommand="SaveScoreCSV" Text="Save" style="font-size:90%;font-weight:bold;width:100px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" />
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>--%>
    </table>
</asp:Content>


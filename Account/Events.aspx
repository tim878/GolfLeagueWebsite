<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="Events" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <asp:table ID="Table_MainContent" runat="Server" style="width: 900px" cellpadding="2">
       <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="Label_ExistingEvents" Text="Scheduled Events for Current Season" Font-Bold="true" Font-Size="16" runat=server/> 
            </asp:TableCell>
        </asp:TableRow>   
     </asp:table> 
    
    <table border="solid">
        <tr>
            <td>
                <table style="width: 500px" cellpadding="20">
                    <tr>
                        <td>
                             <b style="font-size:12px;color:Black">Date</b>
                        </td>
                        <td>
                            <asp:Calendar ID="Calendar_Date" runat="server" Width="200px" /> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b style="font-size:12px;color:Black">Select Course</b>
                        </td>
                        <td>
                             <asp:DropDownList Width="240" ID="Dropdown_Course" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <b style="font-size:12px;color:Black">Select Tees</b>
                        </td>
                        <td>
                             <asp:DropDownList Width="240" ID="DropDown_Tees" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b style="font-size:12px;color:Black">Select Season</b>
                        </td>
                        <td>
                             <asp:DropDownList Width="240" ID="DropDown_Seasons" runat="server" />
                        </td>
                    </tr>
                    <tr>
                       <td>
                             <b style="font-size:12px;color:Black">Event Name (ex. Week 1)</b>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_EventName" runat="server" Width="200px" /> 
                        </td>
                        
                    </tr>
                    <tr>
                         <td>
                             <b style="font-size:12px;color:Black">Event Number (Required)</b>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_EventNum" runat="server" Width="200px" /> 
                        </td>
                    </tr>
                    
                </table>
            </td>
            <td>
                <table style="width: 500px" cellpadding="20">
                    <tr>
                        <td align="center">
                            <b style="font-size:18px;color:Black">Matchups</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 300px">
                             <asp:Panel ID="Panel_Matchups" runat="server" HorizontalAlign=Left/>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                             <asp:DropDownList ID="Dropdown_Team1" runat=server Width=200/>
                        </td>           
                    </tr>
                    <tr>
                       <td align="center">
                              <asp:DropDownList ID="Dropdown_Team2" runat=server Width=200/>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                              <asp:Button ID="AddMatchupButton" runat="server" OnCommand="AddMatchupButtonClick" Text="Add Matchup" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
                        </td>
                    </tr>
                     <%--<tr>
                        <td  align=center>
                             <asp:Button ID="Button1" runat="server" OnCommand="SaveMatchups" Text="Save Matchups" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
                        </td>  
                    </tr>  --%>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align=center>
                <asp:Button ID="Button_Save" runat="server" OnCommand="CreateLeagueEventButtonPress" Text="Create Event" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
    </table>
    
    
</asp:Content>
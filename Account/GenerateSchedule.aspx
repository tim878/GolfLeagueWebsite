<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GenerateSchedule.aspx.cs" Inherits="GenerateSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 575px" cellpadding="20">
        <tr>
            <td colspan="2">
               <b style="font-size:20px;color:Black">Use this page to create a new schedule for Your League (Make sure to add players/teams first)</b>
            </td>
        </tr>
        <tr>
            <td>
                 <b style="font-size:20px;color:Black">Enter Number of Weeks/Events</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_NumEvents" runat="server" /> 
            </td>
        </tr>
        <tr>
            <td colspan="2" align=center>
               <asp:Button ID="Button_Generate" runat="server" OnCommand="CreateSchedule" Text="Generate Schedule" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
        <tr id="tr_complete" visible="false">
            <td colspan="2">
               <b style="font-size:20px;color:Black">Schedule Finished.  Click Schedule tab to view.</b>
            </td>
        </tr>
    </table>
</asp:Content>


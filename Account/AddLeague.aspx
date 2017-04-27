<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddLeague.aspx.cs" Inherits="AddLeague" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 900px" cellpadding="20">
        <tr>
            <td>
                 <b style="font-size:12px;color:Black">League Name</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_LeagueName" runat="server" Width="200px" /> 
            </td>
            
        </tr>
        
        <tr>
            <td colspan="2" align=center>
               <asp:Button ID="Button_Save" runat="server" OnCommand="CreateLeague" Text="Create League" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
       
    </table>
</asp:Content>


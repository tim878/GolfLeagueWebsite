<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CommissionerSignup.aspx.cs" Inherits="CommissionerSignup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 900px" cellpadding="20">
        <tr>
            <td>
                 <b style="font-size:12px;color:Black">First Name</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_FirstName" runat="server" Width="200px" /> 
            </td>
            <td>
                 <b style="font-size:12px;color:Black">Last Name</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_LastName" runat="server" Width="300px" /> 
            </td>
        </tr>
        <tr>
            <td>
                 <b style="font-size:12px;color:Black">NickName (Optional)</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_NickName" runat="server" Width="200px" /> 
            </td>
            <td>
                 <b style="font-size:12px;color:Black">Email Address (Optional)</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_EmailAddress" runat="server" Width="300px" /> 
            </td>
        </tr>
        <tr>
             <td>
                 <b style="font-size:12px;color:Black">Password</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Width="300px" /> 
            </td>
            <td colspan="2" align=center>
               <asp:Button ID="Button_Save" runat="server" OnCommand="AddCommissioner" Text="Sign Up" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
       
    </table>
</asp:Content>


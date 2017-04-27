<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Golfers.aspx.cs" Inherits="Golfers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript">

    var editActive = false;

    function HideControl(controlID) 
    {
        document.getElementById(controlID).style.display = 'none';
    }

    function ShowControl(controlID) 
    {
        document.getElementById(controlID).style.display = 'block';
    }

    function ActivateTextBox(textBoxID) 
    {
        var element = document.getElementById(textBoxID);
        element.disabled = false;
    }

    function DeactivateTextBox(textBoxID) 
    {
        var element = document.getElementById(textBoxID);
        element.disabled = true;
    }

   
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:table ID="Table_MainContent" runat="Server" style="width: 900px" cellpadding="2">
       <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="Label1" Text="Add New Golfer" Font-Bold="true" Font-Size="16" runat=server/> 
            </asp:TableCell>
        </asp:TableRow>    
        <asp:TableRow>
            <asp:TableCell>
                 <b style="font-size:12px;color:Black"># of Rnds.</b>
            </asp:TableCell>
            <asp:TableCell>
                 <b style="font-size:12px;color:Black">First Name</b>
            </asp:TableCell>
            <asp:TableCell>
                 <b style="font-size:12px;color:Black">Last Name</b>
            </asp:TableCell>
            <asp:TableCell>
                 <b style="font-size:12px;color:Black">NickName (Optional)</b>
            </asp:TableCell>
            <asp:TableCell>
                 <b style="font-size:12px;color:Black">Email Address (Optional)</b>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
             <asp:TableCell>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox_FirstName" runat="server" Width="190px" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox_LastName" runat="server" Width="190px" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox_NickName" runat="server" Width="190px" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBox_EmailAddress" runat="server" Width="190px" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="Button_Save" runat="server" OnCommand="SavePlayer" Text="Add Player" style="font-size:90%;font-weight:bold;width:80px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="Label2" Text="Edit Existing Golfer" Font-Bold="true" Font-Size="16" runat=server/> 
            </asp:TableCell>
        </asp:TableRow>    
    </asp:table>
    
    
    <%--<table style="width: 900px" cellpadding="20">
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
            <td colspan="2" align=center>
               <asp:Button ID="Button_Save" runat="server" OnCommand="SavePlayer" Text="Add Player" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
       
    </table>--%>
</asp:Content>


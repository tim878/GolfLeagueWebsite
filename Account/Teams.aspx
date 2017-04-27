<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Teams.aspx.cs" Inherits="Teams" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <style type="text/css">
        .style1
        {
            width: 192px;
        }
    </style>

    <link href="http://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet"/>
<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
<script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

 <%--styling for all buttons on the page--%>
 <script type="text/javascript">
     $(function () {
         $("input[type=submit]").button();
     });
 </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager runat=server></asp:ScriptManager>
    <table style="width: 900px" border="solid">
        <tr>
            <td>
                <asp:Panel ID="Panel_Teams" runat="server" HorizontalAlign=Left/>
            </td>
        </tr>
    </table>
    <table style="width: 900px" cellpadding="5" border="solid">
        <tr>
            <td colspan="2" align="center">
                <asp:Label runat=server Text="Add Team" ForeColor="Black" Font-Size="20" Font-Bold=true/>
            </td>
        </tr>
        <tr>
            <td class="style1">
                 <b style="font-size:12px;color:Black">Enter Team Name (Optional)</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_TeamName" runat="server" Width="297px" /> 
            </td>
           
        </tr>
        <tr>
            <td class="style1">
                 <b style="font-size:12px;color:Black">Select Player 1</b>
            </td>
            <td>
                <asp:DropDownList Width="240" ID="Dropdown_Player1Select" runat="server" />
            </td>
           
        </tr>
         <tr>
            <td class="style1">
                 <b style="font-size:12px;color:Black">Select Player 2</b>
            </td>
            <td>
                <asp:DropDownList Width="240" ID="Dropdown_Player2Select" runat="server"  />
            </td>
           
        </tr>
         <tr>

            <td colspan="2">
                <asp:CheckBox ID="Checkbox_ActiveInCurrentSeason" runat="server" Text="Active In Current Season" />
            </td>
            
        </tr>
        <tr>
            
            <td colspan="2" align=center>
                <asp:UpdatePanel runat=server>
                    <ContentTemplate>
                        <asp:Button ID="Button_AddTeam" runat="server" OnCommand="AddTeam" Text="Add Team" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
           
        </tr>
       
       
    </table>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TeamsNew.aspx.cs" Inherits="TeamsNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link href="../Styles/Grid.css" rel="stylesheet" />
<link href="../Styles/Modal.css" rel="stylesheet" />
<link href="http://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet"/>
<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
 <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

 <%--styling for buttons--%>
 <script type="text/javascript">
     $(function () {
         $("input[type=submit]").button();
     });
 </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>  
<cc1:ModalPopupExtender ID="ModalPopupExtenderEdit" runat="server" PopupControlID="PanelEdit" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="PanelEdit" runat="server" CssClass="modalPopup" align="center" style="display:none">
   <table cellpadding="1px">
        <tr>
            <td><asp:Label ID="lbHide" runat="server" Font-Bold="true" Text="Hide Team for future Events" Width="250px"/> </td>
            <td><asp:Label ID="lbTeamName" runat="server" Font-Bold="true" Text="Team Name"/></td>
            <td colspan="2" align="center"><asp:Label ID="LbGolfers" runat="server" Font-Bold="true" Text="Golfers"/></td>
            <td></td>
            <td></td>
          
        </tr>    
        <tr>
            <td><asp:CheckBox ID="CheckBoxHide" runat="server"/> </td>
            <td><asp:TextBox ID="TextBoxTeamName" runat="server" Width="200"/></td>
            <td style="padding-left: 30px;"><asp:Label ID="LabelGolfer1" runat="server" Width="120"/></td>
            <td><asp:Label ID="LabelGolfer2" runat="server" Width="120"/></td>
            <td><asp:Button ID="btnSaveEdits" runat="server" Text="Save Edits" OnCommand="SaveEdits" /> </td>
            <td><asp:Button ID="btnClose" runat="server" Text="Close" /></td>    
        </tr>
   </table>
    
</asp:Panel>

<cc1:ModalPopupExtender ID="ModalPopupExtenderAdd" runat="server" PopupControlID="PanelAdd" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="PanelAdd" runat="server" CssClass="modalPopup" align="center" style="display:none">
   <table cellpadding="1px">
        <tr>
            
            <td><asp:Label ID="LabelAddTeamName" runat="server" Font-Bold="true" Text="Team Name(Required)"/></td>
            <td colspan="3" align="center"><asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Select Golfers"/></td>
            <td></td>
            <td></td>
        </tr>    
        <tr>
            <td><asp:TextBox ID="TextBoxAddTeamName" runat="server" Width="200"/></td>
            <td style="padding-left: 30px;"> <asp:DropDownList ID="DropdownGolfer1" runat="server" width="200px"/>  </td>
            <td> <asp:DropDownList ID="DropdownGolfer2" runat="server" width="200px"/> </td>
            <td><asp:DropDownList ID="DropDownGolfer3" runat="server" width="200px"/></td>
            <td><asp:Button ID="ButtonAddSubmit" runat="server" Text="Save" OnCommand="AddSubmit" /> </td>
            <td><asp:Button ID="ButtonCloseAddModal" runat="server" Text="Close" /></td>    
        </tr>
   </table>
</asp:Panel>


    <asp:Button ID="ButtonAddTeam" Text="Add New Team" runat="server" OnCommand="AddButtonClick" />
    <asp:Button ID="btnShow" runat="server" Text="" style = "display:none" />
                <asp:GridView ID="grdTeams" runat="server" 
                    CssClass="Grid"                    
                    AlternatingRowStyle-CssClass="alt"
                    PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false"
                    EmptyDataText="No Golfers Entered"
                    CellPadding="20">
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField >
                            <HeaderTemplate>
                                <asp:Label ID="lbHiddenForFuture" runat="server"  Text="Hidden for future Events" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("hidden")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbTeamName" runat="server"  Text="Team Name"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("TeamName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <asp:Label ID="lbGlf1" runat="server"  Text="Golfer 1"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Golfer1Name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbGlf2" runat="server"  Text="Golfer 2"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Golfer2Name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbGlf3" runat="server"  Text="Golfer 3"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Golfer3Name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonEdit" Text="Edit" class="btn btn-primary" runat="server" CommandName="Edit Event" CommandArgument=<%#Eval("TeamID") %> OnCommand="EditButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>                   
                    </Columns>
                </asp:GridView>
          
</asp:Content>


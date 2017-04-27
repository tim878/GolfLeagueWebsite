<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GolfersNew.aspx.cs" Inherits="GolfersNew" %>
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
<cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel1" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style="display:none">
   <table>
        <tr>
            <td><asp:Label ID="lbFirstName" runat="server" Font-Bold="true" Text="First Name" Width="150px"/> </td>
            <td><asp:Label ID="lbLastName" runat="server" Font-Bold="true" Text="Last Name"/></td>
            <td><asp:Label ID="lbNickName" runat="server" Font-Bold="true" Text="NickName"/></td>
            <td><asp:Label ID="lbEmailAddress" runat="server" Font-Bold="true" Text="Email Address"/></td>
            <td></td>
        </tr>    
        <tr>
            <td><asp:TextBox ID="TextBoxFirstName" runat="server" Width="200"/> </td>
            <td><asp:TextBox ID="TextBoxLastName" runat="server" Width="200"/></td>
            <td><asp:TextBox ID="TextBoxNickName" runat="server" Width="200"/></td>
            <td><asp:TextBox ID="TextBoxEmailAddress" runat="server" Width="200"/></td>
            <td>
                <asp:Button ID="btnClose" runat="server" Text="Close" /> 
                <asp:Button ID="btnSave" runat="server" Text="Save" OnCommand="Save" />              
            </td>
        </tr>
   </table>
    
</asp:Panel>
    <asp:Button ID="ButtonAddGolfer" Text="Add New Golfer" runat="server" OnCommand="AddButtonClick" />
    <asp:Button ID="btnShow" runat="server" Text="" style = "display:none" />
                <asp:GridView ID="grdGolfers" runat="server" 
                    CssClass="Grid"                    
                    AlternatingRowStyle-CssClass="alt"
                    PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false"
                    EmptyDataText="No Golfers Entered"
                    >
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbNumRounds" runat="server"  Text="# of Rounds" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("numberOfRoundsPlayed")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <HeaderTemplate>
                                <asp:Label ID="lbFirstName" runat="server"  Text="First Name" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("firstName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbLastName" runat="server"  Text="Last Name"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("lastName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <asp:Label ID="lbNickName" runat="server"  Text="NickName"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("nickName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbEmailAddress" runat="server"  Text="Email Address"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("emailAddress") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonEdit" Text="Edit" class="btn btn-primary" runat="server" CommandName="Edit Event" CommandArgument=<%#Eval("GolferID") %> OnCommand="EditButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>                      
                    </Columns>
                </asp:GridView>
          
</asp:Content>


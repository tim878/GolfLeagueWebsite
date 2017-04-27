<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ScoreEntry_New.aspx.cs" Inherits="ScoreEntry_New" %>
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
            <td><asp:Label ID="lbGolferHeader" runat="server" Font-Bold="true" Text="Scheduled Golfer" Width="150px"/> </td>
            <td><asp:Label ID="lbSubHeader" runat="server" Font-Bold="true" Text="Sub"/></td>
            <td><asp:Label ID="lbScores" runat="server" Font-Bold="true" Text="Enter Scores Seperated by Spaces or Commas"/></td>
            <td></td>
        </tr>    
        <tr>
            <td><asp:Label ID="lbGolferToEdit" runat="server"  Text=""/> </td>
            <td><asp:DropDownList Width="200" ID="DropdownSub" runat="server" AutoPostBack="False"/></td>
            <td><asp:TextBox ID="TextBoxScores" runat="server" Width="350"/></td>

            <td>
                <asp:Button ID="btnClose" runat="server" Text="Close" /> 
                <asp:Button ID="btnSave" runat="server" Text="Save" OnCommand="Save" />
            </td>
        </tr>
   </table>
    
</asp:Panel>
    
    
    <table cellpadding="20">
        <tr>
            <td>
               <b style="font-size:20px;color:Black">Score Entry Page</b>
                <asp:Button ID="btnShow" runat="server" Text="" style = "display:none" />
            </td>
        </tr>
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
                <asp:GridView ID="grdScores" runat="server" 
                    CssClass="Grid"                    
                    AlternatingRowStyle-CssClass="alt"
                    PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false"
                    EmptyDataText="No Matchups for this Event."
                    AllowSorting="false"
                    OnRowDataBound="Grid_RowDataBound" 
                    CellPadding="20">
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbGolfer" runat="server"  Text="Golfer" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Golfer")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <HeaderTemplate>
                                <asp:Label ID="lbSub" runat="server"  Text="Sub" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Sub")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole1" runat="server"  Text="(1)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole1") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>
                                <asp:Label ID="lbHole2" runat="server"  Text="(2)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole2") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole3" runat="server"  Text="(3)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole3") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole4" runat="server"  Text="(4)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole4") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole5" runat="server"  Text="(5)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole5") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole6" runat="server"  Text="(6)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole6") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole7" runat="server"  Text="(7)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole7") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole8" runat="server"  Text="(8)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole8") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbHole9" runat="server"  Text="(9)"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("ScoreHole9") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbTotalScore" runat="server"  Text="Total"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("TotalScore") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonEdit" Text="Edit" class="btn btn-primary" runat="server" CommandName="Edit Event" CommandArgument=<%#Eval("GolferID") %> OnCommand="EditButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonDelete" Text="Delete" class="btn btn-primary" runat="server" CommandName="Delete Event" CommandArgument=<%#Eval("GolferID") %> OnCommand="DeleteButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>                           
                    </Columns>
                </asp:GridView>
           </td>
        </tr>
        
      
       
        
    </table>
</asp:Content>


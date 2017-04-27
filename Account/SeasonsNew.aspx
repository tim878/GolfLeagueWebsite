<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SeasonsNew.aspx.cs" Inherits="SeasonsNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link href="../Styles/Grid.css" rel="stylesheet" />
<link href="../Styles/Modal.css" rel="stylesheet" />
<link href="http://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet"/>
<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
<script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

 <%--styling for all buttons on the page--%>
 <script type="text/javascript">
     $(function () {
         $("input[type=submit]").button();
     });
 </script>


 <style type="text/css">
          #TeamsToUseList, #AvailableTeamsList{
            border: 1px solid #eee;
            width: 302px;
            min-height: 20px;
            list-style-type: none;
            margin: 0;
            padding: 5px 0 0 0;
            float: left;
            margin-right: 10px;
          }
         
          #TeamsToUseList li, #AvailableTeamsList li{
            margin: 0 5px 5px 5px;
            padding: 5px;
            font-size: 1.2em;
            width: 270px;
          }
         
  </style>

  <script type="text/javascript">

      function UpdateList(event, ui) 
      {
          var getTeamList = "";

          $("#TeamsToUseList .ui-state-default").each(function (index, element) {
              getTeamList += element.getAttribute("teamID") + ',';
          });

          $("#<%= TeamsToUseValues.ClientID %>").val(getTeamList);
      }  

      $(function () {
          $("#AvailableTeamsList, #TeamsToUseList").sortable({
              connectWith: ".connectedSortable",
              stop: function(event, ui) { UpdateList(event, ui);},
              create: function(event, ui) { UpdateList(event, ui);}
//              stop: function (event, ui) {
//                  var getTeamList = "";

//                  $("#TeamsToUseList .ui-state-default").each(function (index, element) {
//                      getTeamList += element.getAttribute("teamID") + ',';
//                  });

//                  $("#<%= TeamsToUseValues.ClientID %>").val(getTeamList);
//              },
//              create: function (event, ui) {
//                  var getTeamList = "";

//                  $("#TeamsToUseList .ui-state-default").each(function (index, element) {
//                      getTeamList += element.getAttribute("teamID") + ',';
//                  });

//                  $("#<%= TeamsToUseValues.ClientID %>").val(getTeamList);
//              }
          }).disableSelection();
      });
  </script>

  <%--scripts to support draggable lists of teams --%>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>  
<cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel1" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style="display:none">
   <table>
        <tr>
            <td><asp:Label ID="SeasonName" runat="server" Font-Bold="true" Text="Season Name" /> </td>
            <td></td>
        </tr>    
        <tr>
            <td><asp:TextBox ID="TextBoxSeasonName" runat="server" Width="200"/> </td>           
            <td>
                <asp:Button ID="btnClose" runat="server" Text="Close" /> 
                <asp:Button ID="btnSave" runat="server" Text="Save" OnCommand="Save" />              
            </td>
        </tr>
   </table>   
</asp:Panel>

<cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel2" TargetControlID="btnShow2" CancelControlID="btnClose2" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="center" style="display:none">
   <table>
        <tr>
            <td colspan="2">
                <asp:Label ID="LabelAddEvents" runat="server" Font-Bold="true" Text="Generate Events (Events will be added on to currently scheduled Events.)" />
            </td>
        </tr>
        <tr>
            <td> <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Teams to Use" Width="150px"/> </td>
            <td> <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Available Teams" Width="150px"/>  </td>
        </tr>
        <tr>
            <td>
                <input type="hidden"  id="TeamsToUseValues" name="TeamsToUseValues" runat="server" />
                <ul id="TeamsToUseList" class="connectedSortable" style="float:left" runat="server" clientidmode="Static">  
                </ul>
            </td>
            <td>
                <ul id="AvailableTeamsList" class="connectedSortable" style="float:left" runat="server" clientidmode="Static">
                </ul>
            </td>
        </tr>    
        <tr>
            <td><asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Number of Weeks to Add" /></td>        
            <td><asp:TextBox ID="TextboxNumberOfWeeksToAdd" runat="server" Width="100"/> </td> 
        </tr>       
        <tr>           
            <td>
                <asp:Button ID="btnClose2" runat="server" Text="Close" /> 
                <asp:Button ID="btnAddWeeks" runat="server" Text="Add Weeks" OnCommand="GenerateEvents" />              
            </td>
        </tr>
   </table>   
</asp:Panel>



    <asp:Button ID="ButtonAddSeason" Text="Create New Season" runat="server" OnCommand="AddButtonClick" /> 
    <asp:DropDownList ID="DropdownCurrentSeason" runat="server" AutoPostBack="True" width="200px" OnSelectedIndexChanged="DropdownCurrentSeasonsSelectedIndexChanged" /> 
    <asp:Button ID="btnShow" runat="server" Text="" style = "display:none" />
    <asp:Button ID="btnShow2" runat="server" Text="" style = "display:none" />
                <asp:GridView ID="grdSeasons" runat="server" 
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
                                <asp:Label ID="lbCurrentSeason" runat="server"  Text="Is Current" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# (bool)Eval("isCurrentSeason") == true ? ">>>>>>" : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbSeasonName" runat="server"  Text="Season Name" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("SeasonName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <HeaderTemplate>
                                <asp:Label ID="lbNumEventsScheduled" runat="server"  Text="# Events Scheduled" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("NumberOfEventsScheduled")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonAutoGenerate" Text="Auto Generate Events" class="btn btn-primary" runat="server" CommandName="AutoGenerate" CommandArgument=<%#Eval("SeasonID") %> OnCommand="GenerateEventsButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonEdit" Text="Edit" class="btn btn-primary" runat="server" CommandName="EditName" CommandArgument=<%#Eval("SeasonID") %> OnCommand="EditButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>                             
                    </Columns>
                </asp:GridView>
          
</asp:Content>


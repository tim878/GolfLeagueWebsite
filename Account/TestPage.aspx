<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="TestPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <link href="http://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet"/>
    <link href="../Styles/Grid.css" rel="stylesheet" />

    <%--<link href="~/Styles/Bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap-theme.css" rel="stylesheet" />
    <link href="~/Styles/bootstrap-overrides.css" rel="stylesheet" />--%>

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

   <%-- Use JQuery Styling for Datepickers and Buttons--%>
    
    
    <script type="text/javascript">
        $(function () {
            $("input[type=submit]").button();
        });
    </script>


    <style type="text/css">
          #TeamOneList, #TeamTwoList {
            border: 1px solid #eee;
            width: 142px;
            min-height: 20px;
            list-style-type: none;
            margin: 0;
            padding: 5px 0 0 0;
            float: left;
            margin-right: 10px;
          }
          #TeamOneList li, #TeamTwoList li {
            margin: 0 5px 5px 5px;
            padding: 5px;
            font-size: 1.2em;
            width: 120px;
          }
  </style>
  <script type="text/javascript">
      $(function () {
          $("#TeamOneList, #TeamTwoList").sortable({
              connectWith: ".connectedSortable"
          }).disableSelection();
      });
  </script>

      
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

                <input type="hidden" id="TeamOneListValues" />
                <input type="hidden" id="TeamTwoListValues" />
                <asp:Panel runat="server" style="display:inline" >
                <ul id="TeamOneList" class="connectedSortable" >
                    <li class="ui-state-default">Team 1</li>
                    <li class="ui-state-default">Team 2</li>
                    <li class="ui-state-default">Team 3</li>
                </ul>

                <ul id="TeamTwoList" class="connectedSortable" >
                    <li class="ui-state-default">Team 4</li>
                    <li class="ui-state-default">Team 5</li>
                </ul>
                </asp:Panel>
                    
            

    
</asp:Content>
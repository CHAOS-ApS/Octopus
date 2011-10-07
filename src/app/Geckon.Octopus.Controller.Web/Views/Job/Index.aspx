<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Geckon.Octopus.Controller.Web.ViewModels.JobIndexViewModel>" %>
<%@ Import Namespace="Geckon.Octopus.Controller.Web.ViewModels" %>
<%@ Import Namespace="Geckon.Octopus.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JobIndex
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%">
    
        <tr>
            <td style="font-weight: bold;">Type</td>
            <td style="font-weight: bold; width: 50px;">Count</td>
            <td style="font-weight: bold; width: 50px;">Percent</td>
        </tr>
    
        <%  foreach( JobStatisticsAggregate stat in Model.JobStatisticsAggregates )
            {
                %>

        <tr>
            <td><%= stat.StatusType %></td>
            <td><%= stat.Count %></td>
            <td><%= string.Format("{0:P2}", stat.Percent)%></td>
        </tr>

                <%
            } 
        %>

    </table>

    <br />

    <%  foreach( Job unfinishedJob in Model.CurrentlyRunningJobs )
        {
            Html.RenderPartial( "JobProgress", unfinishedJob );
        } 
    %>

</asp:Content>

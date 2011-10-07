<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Geckon.Octopus.Data.Job>" %>

<table width="100%" >
    <tr>
        <td style="width: 40px; font-weight:bold;"><%= string.Format("#{0:D}", Model.ID)%></td>
        <td style="font-size: 80%; line-height: 14px;"><%= System.IO.Path.GetFileNameWithoutExtension( XDocument.Parse(Model.JobXML).Descendants("SourceFilePath").First().Value ) %></tdstyle>
        <td style="text-align: right; font-weight:bold; font-size: 80%; line-height: 14px;"><%= string.Format("{0:P4}", double.Parse( XDocument.Parse(Model.JobXML).Root.Attribute("TotalProgress" ).Value ) )%></td>
    </tr>
    <tr>
        <td colspan="3">
            <div class="JobProgressBarCanvas">
                <div class="JobProgressBar" style="width: <%= string.Format( "{0:F0}",double.Parse( XDocument.Parse(Model.JobXML).Root.Attribute("TotalProgress" ).Value ) * 100 ) %>%;">
                     	&nbsp;
                </div>
            </div>
        </td>
    </tr>
</table>



<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPreview.aspx.cs" Inherits="CelloStore.FrontEnd.Reports.ReportPreview" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager runat="server" id="scriptManager"></asp:ScriptManager>
        <rsweb:ReportViewer ID="rptReport" runat="server" Width="100%">
        </rsweb:ReportViewer>

    </div>
    </form>
</body>
</html>

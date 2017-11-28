<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SSISMigrationDoc.aspx.cs" Inherits="Sealegs.Backend.Views.Docs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <img src="../Content/Images/SSISMigrationOverview.png" alt="Planets" usemap="#planetmap" ismap="ismap" />
    <map name="planetmap">
        <area shape="rect" coords="83, 167, 282, 187" title="SqlServerInputUserInRoles" href="#">
        <area shape="rect" coords="711, 491, 905, 531" title="SqlServerLinkedService" href="#">
        <area shape="rect" coords="76, 501, 275, 518" title="MSDataManagementGateway" href="#">
        <area shape="rect" coords="381, 166, 602, 189" title="SealegsUsersInRolesPipeline" href="#">
        <area shape="rect" coords="761, 55, 951, 94" title="AzureBlobOutputUserInRoles" href="#">
        <area shape="rect" coords="761, 253, 958, 292" title="MSAzureStorageExplorer" href="#">
        <area shape="rect" coords="81, 653, 272, 692" title="InputDataset" href="#">
        <area shape="rect" coords="380, 641, 616, 707" title="AzureBatchLinkedService" href="#">
        <area shape="rect" coords="725, 667, 890, 684" title="SealegsAzureSqlLinkedService" href="#">
        <area shape="rect" coords="383, 495, 611, 522" title="OOTBOnPremSqlServerToAzureBlob" href="#">
    </map>
</body>
</html>

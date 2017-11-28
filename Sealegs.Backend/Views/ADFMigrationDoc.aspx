<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ADFMigrationDoc.aspx.cs" Inherits="Sealegs.Backend.Views.Docs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <img src="../Content/Images/ADFMigrationOverview.png" alt="Planets" usemap="#ADFMigrationOverviewMap" style="margin-right: 0px" />
    <map name="ADFMigrationOverviewMap">
                    <area shape="Rect" coords="99, 58, 289, 98" href="" id="SqlServerInputUserInRoles">
                    <area shape="Rect" coords="110, 113, 282, 135" href="" id="SqlServerLinkedService">
                    <area shape="Rect" coords="83, 151, 311, 215" href="" id="MSDataManagementGateway">
                    <area shape="Rect" coords="391, 198, 592, 218" href="" id="SealegsUsersInRolesPipeline">
                    <area shape="Rect" coords="692, 61, 893, 105" href="" id="AzureBlobOutputUserInRoles">
                    <area shape="Rect" coords="743, 117, 850, 139" href="" id="SealegsAzureStorageLinkedService">
                    <area shape="Rect" coords="676, 157, 910, 220" href="" id="MSAzureStorageExplorer" alt="Shadow for some" data-maphilight='{"fillColor":"00ff00","shadow":true,"shadowBackground":"ffffff","alwaysOn":true,"strokeWidth":2}'>
                    <area shape="Rect" coords="129, 556, 233, 595" href="" id="InputDataset">
                    <area shape="Rect" coords="722, 572, 833, 612" href="" id="OutputDataset">
                    <area shape="Rect" coords="387, 634, 571, 656" href="" id="AzureBatchLinkedService">
                    <area shape="Rect" coords="399, 672, 557, 694" href="" id="AzureStorageLinkedService">
                    <area shape="Rect" coords="679, 665, 886, 686" href="" id="SealegsAzureSqlLinkedService">
                    <area shape="Rect" coords="377, 235, 607, 314" href="" id="OOTBOnPremSqlServerToAzureBlob">
                    <area shape="Rect" coords="390, 567, 582, 615" href="" id="AzureBlobToAzureSqlCustomActivity">
                    <area shape="Rect" coords="732, 623, 825, 654" href="" id="AzureSQL">
                </map>
</body>
</html>

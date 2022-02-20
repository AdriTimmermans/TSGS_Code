<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Correction_Player_Data.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Correction_Player_Data" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="Label3" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Selecteer te corrigeer deelnemer</asp:Label>
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView1_OnRowCreated">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Speler Id" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="Speler_ID" runat="server" Text='<%#Eval("Speler_ID")%>' BackColor="Transparent"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Spelernaam">
                <ItemTemplate>
                    <%#Eval("SpelerNaam")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SelectIcon" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:ImageButton CommandName="select" runat="server" ImageUrl="~/images/unselect.jpg" Height="20px" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:Button ID="Button1" Text="done" runat="server" OnClick="Button1_Click" />
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                &nbsp;</td>
        </tr>
    </table>
    <script>
        function SelectIcon(x) {
            x.src = "images/done2.jpg";
        }

        function UnselectIcon(x) {
            x.src = "images/unselect.jpg";
        }
    </script>
</asp:Content>

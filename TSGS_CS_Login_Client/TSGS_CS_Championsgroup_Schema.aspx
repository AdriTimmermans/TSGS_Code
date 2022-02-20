<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Championsgroup_Schema.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Championsgroup_Schema" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Speler Id" Visible="false">
                <ItemTemplate>
                    <%#Eval("Speler_ID")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SpelerNaam">
                <ItemTemplate>
                    <asp:Label ID="SpelerNaam" runat="server" Text='<%#Eval("Spelernaam")%>' BackColor="Transparent"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Lotingnummer">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="tbLotingnummer" runat="server" AutoPostBack="true" Width="25"   OnTextChanged="tbLotingnummer_TextChanged" Text='<%#Eval("Lotnumber")%>' />
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



    <asp:Button ID="Button2" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

                <!-- additional footer buttons -->

                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

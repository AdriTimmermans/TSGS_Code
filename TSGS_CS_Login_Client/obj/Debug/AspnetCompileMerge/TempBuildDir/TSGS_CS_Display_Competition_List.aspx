<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_Competition_List.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_Competition_List" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Label</asp:Label>,
            <asp:Label ID="Label4" runat="server" Font-Names="Arial">Label</asp:Label>
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="PL">
                <ItemTemplate>
                    <asp:Label ID="PL" runat="server" Text='<%#Eval("PL")%>' BackColor="Transparent"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Foto">
                <ItemTemplate>
                    <asp:Image ID="imgPicture" runat="server"></asp:Image>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Speler">
                <ItemTemplate>
                    <%#Eval("SpelerNaam")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rating" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("Competition_Points", "{0:f1}")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Punten" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("Aantal_punten", "{0:f1}")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Games" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("Aantal_partijen")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Perc" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("Percentage", "{0:f1}")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="KG" Visible="false">
                <ItemTemplate>
                    <asp:TextBox ID="KG" runat="server" Text='<%#Eval("Member_Premier_Group")%>' BackColor="Transparent"></asp:TextBox>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
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
    <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="18pt" Height="44px">* betekent....</asp:Label>
    <table style="width: 100%;">
        <tr>
            <td></td>
        </tr>
        <tr>
            <td class="auto-style1">&nbsp;
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">                     
                <asp:Button ID="Button1" runat="server" Text="Upload to website" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
                <asp:Button ID="Button2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

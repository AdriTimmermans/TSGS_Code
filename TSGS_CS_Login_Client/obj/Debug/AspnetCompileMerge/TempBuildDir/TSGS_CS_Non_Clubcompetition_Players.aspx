<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Non_Clubcompetition_Players.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Non_Clubcompetition_Players" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="14pt" Height="24px" Width="700px">Ronde nummer: </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Font-Names="Arial" Font-Size="14pt" Width="700px">Aantal deelnemers:</asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button3" runat="server" Height="44px" Font-Size="18pt" Width="288px" Text="Zet iedereen op  afwezig" OnClick="Button3_Click"></asp:Button>&nbsp;&nbsp;
                    <asp:Button ID="Button1" runat="server" Height="44px" Font-Size="18pt" Text="Gereed" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <table id="Table1" border="0">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id" Visible="false">
                            <ItemTemplate>
                                <%#Eval("Speler_ID")%>
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
                        <asp:TemplateField HeaderText="Telefoonnummer">
                            <ItemTemplate>
                                <%#Eval("Telefoonnummer")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Team">
                            <ItemTemplate>
                                <%#Eval("Team")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Competition_Id" Visible="false">
                            <ItemTemplate>
                                <%#Eval("Competition_Id")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Afwezig">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbAfwezig" runat="server" AutoPostBack="true" OnCheckedChanged="OnAfwezigChanged" Text=" " />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="External">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbExtern" runat="server" AutoPostBack="true" OnCheckedChanged="OnExternChanged" Text=" " />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kroongroep">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbKroongroep" runat="server" AutoPostBack="false" OnCheckedChanged="OnKroongroepChanged" Text=" " />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Partijnr">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="InputKroongroep" runat="server" AutoPostBack="true" OnTextChanged="OnInputChanged" autocomplete="off" Text=" " />
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
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Weet je het zeker?</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button2" TabIndex="2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Link_Player.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Link_Player" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
    <table id="Table1" border="0">
        <tr>
            <td style="vertical-align:top">
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
                        <asp:TemplateField HeaderText="LinkPlayer">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbLinkPlayer" runat="server" AutoPostBack="true" OnCheckedChanged="OnLinkPlayerChanged" Text=" " />
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
            <td style="vertical-align:top">
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound">
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
                        <asp:TemplateField HeaderText="UnLinkPlayer">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbUnLinkPlayer" runat="server" AutoPostBack="true" OnCheckedChanged="OnUnLinkPlayerChanged" Text=" " />
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

    <table style="width: 100%">
        <tr>
            <td class="auto-style1">&nbsp;</td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

                <!-- additional footer buttons -->

                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>


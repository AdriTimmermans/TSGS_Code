<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Adjust_Pairing.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Adjust_Pairing" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Selecteer open te breken partij</asp:Label>

    <table>
        <tr>
            <td style="vertical-align: top">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView_RowCreated">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="PID_Wit" runat="server" Text='<%#Eval("PID_Wit")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Speler Id Zwart" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="PID_Zwart" runat="server" Text='<%#Eval("PID_Zwart")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wit">
                            <ItemTemplate>
                                <asp:Label ID="SpelerNaamWit" runat="server" Text='<%#Eval("SpelerNaamWit")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="tegen" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                -
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Zwart">
                            <ItemTemplate>
                                <asp:Label ID="SpelerNaamZwart" runat="server" Text='<%#Eval("SpelerNaamZwart")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
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

            </td>
            <td>&nbsp;&nbsp;&nbsp;
            </td>
            <td style="width: 25%; vertical-align: top">
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView2_PageIndexChanging" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" OnRowCreated="GridView_RowCreated">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ID" runat="server" Text='<%#Eval("ID")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wit">
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("Name")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
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
            </td>
            <td>&nbsp;&nbsp;&nbsp;
            </td>

            <td style="width: 25%; vertical-align: top">
                <asp:GridView ID="GridView3" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView3_PageIndexChanging" OnSelectedIndexChanged="GridView3_SelectedIndexChanged" OnRowCreated="GridView_RowCreated">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ID" runat="server" Text='<%#Eval("ID")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Zwart">
                            <ItemTemplate>
                                <asp:Label ID="Name" runat="server" Text='<%#Eval("Name")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
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
            </td>
        </tr>
    </table>
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Handindeling</asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Font-Names="Arial">-</asp:Label>
            </td>
            <td>&nbsp;&nbsp;-&nbsp;&nbsp;
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" Font-Names="Arial">-</asp:Label>
            </td>
            <td style="text-align: right">&nbsp;&nbsp;<asp:Button ID="Button2" runat="server" Text="Reset game" OnClick="Button2_Click"></asp:Button>
                <asp:Label ID="Label7" runat="server" Font-Names="Arial" Visible="false"></asp:Label>
                <asp:CheckBox ID="cb_CGGame" runat="server" Visible="false" OnCheckedChanged="CB_CGGame_Search_GameNr" />
                &nbsp;<asp:TextBox ID="CGGameNr" runat="server" Visible="false"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Game OK" OnClick="Button1_Click"></asp:Button>
                <asp:Button ID="Button3" runat="server" Text="Gereed" OnClick="Button3_Click"></asp:Button>
            </td>
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

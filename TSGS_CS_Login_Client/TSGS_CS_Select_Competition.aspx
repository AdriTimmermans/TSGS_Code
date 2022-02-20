<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Select_Competition.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Select_Competition" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" HorizontalAlign="Left" OnRowCreated="GridView_RowCreated">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="SelectIcon"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                            <asp:ImageButton CommandName="select" runat="server" ImageUrl="~/images/unselect.jpg" Height="20px" /> 
                            </ItemTemplate> 
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <EmptyDataRowStyle Font-Size="14pt" />
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
            <td class="auto-style1">&nbsp;
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button2" TabIndex="2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <script>
        function SelectIcon(x)
        {
            x.src = "images/done2.jpg";
        }

        function UnselectIcon(x)
        {
            x.src = "images/unselect.jpg";
        }
    </script>
</asp:Content>

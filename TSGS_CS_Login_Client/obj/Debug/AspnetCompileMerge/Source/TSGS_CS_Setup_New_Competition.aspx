<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Setup_New_Competition.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Setup_New_Competition" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
   <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Font-Names="Arial" Font-Size="14pt" HorizontalAlign="Left" Visible="true">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/notdone2.jpg">
                            <ControlStyle Height="20px" />
                        </asp:CommandField>
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
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound" Visible="false">
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
                        <asp:TemplateField HeaderText="Team">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tbTeam" runat="server" AutoPostBack="false" onkeydown = "return (event.keyCode!=13);" Width="10" autocomplete="off" ontextchanged="tbTeam_TextChanged" Text='<%#Eval("Team")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kroongroep">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbKroongroep" runat="server" AutoPostBack="false" OnCheckedChanged="cbKroongroep_Changed" Text=" " />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Spelendlid">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSpelend" runat="server" AutoPostBack="false" onCheckedChanged="cbSpelend_Changed" Text=" " />
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Overnemen">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbOvernemen" runat="server" AutoPostBack="true" onCheckedChanged="cbOvernemen_Changed" Text=" " />
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

<asp:Button ID="Button2" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button2_Click"></asp:Button> 


    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" >Feedback action</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

<!-- additional footer buttons -->

                <asp:Button ID="Button1" runat="server" Text="Cancel"></asp:Button>            </td>
        </tr>
    </table>
</asp:Content>

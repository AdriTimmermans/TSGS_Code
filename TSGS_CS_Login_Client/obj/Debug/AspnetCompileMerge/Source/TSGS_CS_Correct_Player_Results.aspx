<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Correct_Player_Results.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Correct_Player_Results" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
    <asp:Label ID="Label2" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" HorizontalAlign="Left">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="RN">
                            <ItemTemplate>
                                <asp:Label ID="RN" runat="server" Text='<%#Eval("Rondenr")%>' BackColor="Transparent"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="RDate" runat="server" Text='<%#Eval("Rondedatum", "{0:dd/MM/yyyy}")%>' BackColor="Transparent"></asp:Label>
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
                        <asp:TemplateField HeaderText="Result">
                            <ItemTemplate>
                                <%#Eval("Afkorting")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Color">
                            <ItemTemplate>
                                <%#Eval("Kleur")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ELOW" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%#Eval("ELOResultaat", "{0:f1}")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CPW" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%#Eval("CompetitieResultaat", "{0:f1}")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
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
            </td>
        </tr>
    </table>
    <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label8" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label9" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label10" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label11" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label12" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label13" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Result" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLR_SelectedChange"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="Kleur" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLK_SelectedChange"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="TextBox3" TabIndex="28" Font-Size="12pt" runat="server" Width="80px"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="TextBox4" TabIndex="28" Font-Size="12pt" runat="server" Width="80px"></asp:TextBox>
            </td>
            <td>
                <asp:CheckBox ID="CheckBox1" TabIndex="29" Font-Size="12pt" runat="server" Width="50px" Text=" "></asp:CheckBox>
            </td>
        </tr>
    </table>
    <asp:Button ID="Button2" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>


    <table style="width: 100%">
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

                <!-- additional footer buttons -->

                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

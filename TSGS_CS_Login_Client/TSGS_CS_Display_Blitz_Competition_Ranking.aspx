<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_Blitz_Competition_Ranking.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_Blitz_Competition_Ranking" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Najaarscompetitie 2015</asp:Label>,
            <asp:Label ID="Label4" runat="server" Font-Names="Arial">Stand na ronde x</asp:Label>

    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AllowSorting="true" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
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
                    <asp:Label ID="Picture" runat="server" Text='<%#Eval("Foto")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Speler">
                <ItemTemplate>
                    <%#Eval("Speler")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="R1" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("R1")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="R2" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("R2")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="R3" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("R3")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="R4" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("R4")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="R5" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("R5")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="R6" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("R6")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Strafpunten" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("PuntenClean")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText=" " ItemStyle-HorizontalAlign="Right" Visible="false">
                <ItemTemplate>
                    <%#Eval("PuntenRaw")%>
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
    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:UpdatePanel ID="Updatepanel1" runat="Server">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="300" DynamicLayout="true">
                            <ProgressTemplate>
                                <img src="images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button ID="Button2" runat="server" Text="Uploaden" Font-Size="12pt" OnClick="Button2_Click"></asp:Button><br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
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

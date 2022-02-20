<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_Participant_List.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_Participant_List" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Competitie ...</asp:Label>,
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Deelnemerlijst</asp:Label>

    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Foto">
                <ItemTemplate>
                    <asp:Image ID="imgPicture" runat="server"></asp:Image>
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
                    <asp:TextBox ID="tbTeam" runat="server" Enabled="false" Width="20"   Text='<%#Eval("Team")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Kroongroep">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbKroongroep" runat="server" Enabled="false" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Spelendlid">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbSpelend" runat="server" Enabled="false" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Clubrating">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Clubrating" runat="server" AutoPostBack="false" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="KNSB_Id">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="KNSB_Id" runat="server" AutoPostBack="false" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="KNSB_Rating">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="KNSB_Rating" runat="server" AutoPostBack="false" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="FIDE_Id">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="FIDE_Id" runat="server" AutoPostBack="false" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="FIDE_Rating">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="FIDE_Rating" runat="server" AutoPostBack="false" Text=" " />
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
                        <asp:Button ID="Button1" runat="server" Text="Upload" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style1"></td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

                <!-- additional footer buttons -->
                <asp:Button ID="Button2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>


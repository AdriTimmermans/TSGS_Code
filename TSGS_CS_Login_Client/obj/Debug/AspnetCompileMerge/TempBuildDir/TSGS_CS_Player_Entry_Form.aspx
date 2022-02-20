<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Player_Entry_Form.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Player_Entry_Form" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="Label18" runat="server" CssClass="mediumsize" Font-Names="Arial" Font-Size="14pt">Achternaam: </asp:Label><br />
    <asp:TextBox ID="TextBox15" TabIndex="1" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox><br />
    <asp:Button ID="Button5" TabIndex="2" runat="server" Text="Speler zoeken" Font-Names="Arial" Font-Size="12pt" OnClick="Button5_Click"></asp:Button><br />
    <br />

    <asp:Label ID="Label17" runat="server" Font-Size="14pt">Voornaam: </asp:Label><br />
    <asp:TextBox ID="TextBox14" TabIndex="3" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label19" runat="server" Font-Size="14pt">Tussenvoegsel: </asp:Label><br />
    <asp:TextBox ID="TextBox13" TabIndex="4" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label16" runat="server" Font-Size="14pt">Voorletters:</asp:Label><br />
    <asp:TextBox ID="TextBox12" TabIndex="5" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label2" runat="server" Font-Size="14pt">Titel:</asp:Label><br />
    <asp:TextBox ID="TextBox3" TabIndex="5" Font-Size="12pt" runat="server" Width="48px"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label20" runat="server" Font-Size="14pt">Rating: </asp:Label><br />
    <asp:TextBox ID="TextBox10" TabIndex="6" Font-Size="12pt" runat="server" Width="50px"></asp:TextBox><br />
    <br />


    <asp:Label ID="Label12" runat="server" Font-Size="14pt">Telefoonnummer:</asp:Label><br />
    <asp:TextBox ID="TextBox7" TabIndex="7" Font-Size="12pt" runat="server" Width="105px"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label13" runat="server" Font-Size="14pt">Foto:</asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:FileUpload ID="FileUpload1" runat="server" Font-Size="12pt" Width="360px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" TabIndex="8" runat="server" Text="Toon foto" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Image ID="Image2" TabIndex="9" runat="server" AlternateText="Afbeelding niet gevonden" ImageUrl="~/images/nofoto.png"></asp:Image><br />
    <br />
    <asp:Label ID="Label14" runat="server" Font-Size="14pt">KNSB nummer:</asp:Label><br />
    <asp:TextBox ID="TextBox11" TabIndex="10" Font-Size="12pt" runat="server" Width="105px" AutoComplete="Off"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label22" runat="server" Font-Size="14pt">FIDE nummer:</asp:Label><br />
    <asp:TextBox ID="TextBox1" TabIndex="11" Font-Size="12pt" runat="server" Width="98px" AutoComplete="Off"></asp:TextBox><br />
    <br />

    <asp:Label ID="Label47" runat="server" Font-Size="14pt">FIDE rating:</asp:Label><br />
    <asp:TextBox ID="TextBox2" TabIndex="11" Font-Size="12pt" runat="server" Width="105px" AutoComplete="Off"></asp:TextBox><br />
    <br />
    <asp:CheckBox ID="CheckBox1" TabIndex="12" runat="server" Text="Wil indeling op email" TextAlign="Left" Font-Size="14pt"></asp:CheckBox>

    <br />
    <br />
    <asp:Label ID="Label21" runat="server" Width="179px" Font-Size="14pt">Email adres:</asp:Label><br />
    <asp:TextBox ID="TextBox16" TabIndex="13" Font-Size="12pt" runat="server" AutoComplete="Off"></asp:TextBox><br />
    <br />
    <asp:Button ID="Button6" TabIndex="14" runat="server" Width="256px" Text="Deelnemer toevoegen" Font-Size="12pt" OnClick="Button6_Click"></asp:Button>

    <div class="zoekspeler">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="selecteer_Click" Visible="false">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Club Id" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Label31" runat="server" Text='<%# Bind("Club_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KNSB Id">
                    <ItemTemplate>
                        <asp:Label ID="Label32" runat="server" Text='<%# Bind("KNSB_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Achternaam">
                    <ItemTemplate>
                        <asp:Label ID="Label33" runat="server" Text='<%# Bind("Achternaam")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tussenvoegsel">
                    <ItemTemplate>
                        <asp:Label ID="Label34" runat="server" Text='<%# Bind("Voornaam")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voornaam">
                    <ItemTemplate>
                        <asp:Label ID="Label35" runat="server" Text='<%# Bind("Tussenvoegsel")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voorletters">
                    <ItemTemplate>
                        <asp:Label ID="Label37" runat="server" Text='<%# Bind("Voorletters")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rating">
                    <ItemTemplate>
                        <asp:Label ID="Label36" runat="server" Text='<%# Bind("Rating")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/notdone2.jpg">
                    <ControlStyle Height="20px" />
                </asp:CommandField>
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
    </div>
    <div class="zoekFIDEspeler">
        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowCommand="GV2_selecteer_Click" Visible="false">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Bond Id" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="Label41" runat="server" Text='<%# Bind("Bond_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FIDE nummer">
                    <ItemTemplate>
                        <asp:Label ID="Label42" runat="server" Text='<%# Bind("FIDE_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Titel">
                    <ItemTemplate>
                        <asp:Label ID="Label43" runat="server" Text='<%# Bind("Titel")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Naam">
                    <ItemTemplate>
                        <asp:Label ID="Label44" runat="server" Text='<%# Bind("Naam")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rating">
                    <ItemTemplate>
                        <asp:Label ID="Label46" runat="server" Text='<%# Bind("Rating")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/notdone2.jpg">
                    <ControlStyle Height="20px" />
                </asp:CommandField>
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
    </div>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px" Visible="false">Weet je het zeker?</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button2" TabIndex="2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

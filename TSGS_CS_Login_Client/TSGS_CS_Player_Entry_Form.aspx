<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Player_Entry_Form.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Player_Entry_Form" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="Label23" runat="server" CssClass="mediumsize"   >Snelschaak: nieuwe deelnemer(s)? </asp:Label>&nbsp;&nbsp;
    <asp:Button ID="Button3" TabIndex="2" runat="server" Text="Ja"    OnClick="Button3_Click"></asp:Button>&nbsp;&nbsp;
    <asp:Button ID="Button4" TabIndex="2" runat="server" Text="Nee"   OnClick="Button4_Click"></asp:Button><br />

    <asp:Label ID="Label18" runat="server" CssClass="mediumsize"    >Achternaam: </asp:Label><br />
    <asp:TextBox ID="TextBox15" TabIndex="1" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
    <asp:Button ID="Button5" TabIndex="2" runat="server" Text="Speler zoeken"    OnClick="Button5_Click"></asp:Button><br />

    <asp:Label ID="Label17" runat="server"  >Voornaam: </asp:Label><br />
    <asp:TextBox ID="TextBox14" TabIndex="3" runat="server" Width="150px"></asp:TextBox><br />

    <asp:Label ID="Label19" runat="server"  >Tussenvoegsel: </asp:Label><br />
    <asp:TextBox ID="TextBox13" TabIndex="4" runat="server" Width="150px"></asp:TextBox><br />

    <asp:Label ID="Label16" runat="server"  >Voorletters:</asp:Label><br />
    <asp:TextBox ID="TextBox12" TabIndex="5" runat="server" Width="150px"></asp:TextBox><br />

    <asp:Label ID="Label24" runat="server"  >Geboortedatum (mm/dd/yyyy):</asp:Label><br />
    <asp:TextBox ID="TextBox24" TabIndex="6" runat="server" Width="48px"></asp:TextBox>
    <asp:CustomValidator ID="CVTB" runat="server" ControlToValidate="TextBox24" ErrorMessage="Date was in incorrect format" OnServerValidate="CustomValidator1_ServerValidate" /><br />

    <asp:Label ID="Label2" runat="server"  >Titel:</asp:Label><br />
    <asp:TextBox ID="TextBox3" TabIndex="5" runat="server" Width="48px"></asp:TextBox><br />

    <asp:Label ID="Label20" runat="server"  >Rating: </asp:Label><br />
    <asp:TextBox ID="TextBox10" TabIndex="6" OnTextChanged="OnRatingTextBoxChanged"   runat="server" Width="50px"></asp:TextBox><br />

    <asp:Label ID="Label12" runat="server"  >Telefoonnummer:</asp:Label><br />
    <asp:TextBox ID="TextBox7" TabIndex="7" runat="server" Width="105px"></asp:TextBox><br />

    <asp:Label ID="Label13" runat="server"  >Foto:</asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:FileUpload ID="FileUpload1" runat="server"  Width="360px" OnChange="FileSelectedChange"/>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" TabIndex="8" runat="server" Text="Toon foto" style="display: none" OnClick="Button1_Click"></asp:Button>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Image ID="Image2" runat="server" AlternateText="Afbeelding niet gevonden" ImageUrl="~/images/nofoto.png"></asp:Image>
    <asp:ImageButton ID="RotateButton" ImageUrl="~/images/sub_black_rotate_cw.png" Width="40" TabIndex="9" runat="server" Text="" OnClick="RotateButton_Click"></asp:ImageButton>
    <br />
    <asp:Label ID="Label14" runat="server"  >KNSB nummer:</asp:Label><br />
    <asp:TextBox ID="TextBox11" TabIndex="10" runat="server" Width="105px"  ></asp:TextBox><br />

    <asp:Label ID="Label27" runat="server"  >KNSB rating:</asp:Label><br />
    <asp:TextBox ID="TextBox27" TabIndex="11" runat="server" Width="105px"  ></asp:TextBox><br />

    <asp:Label ID="Label22" runat="server"  >FIDE nummer:</asp:Label><br />
    <asp:TextBox ID="TextBox1" TabIndex="11" runat="server" Width="98px"  ></asp:TextBox><br />

    <asp:Label ID="Label47" runat="server"  >FIDE rating:</asp:Label><br />
    <asp:TextBox ID="TextBox2" TabIndex="11" runat="server" Width="105px"  ></asp:TextBox><br />

    <asp:Label ID="Label25" runat="server"  >Snelschaak rating:</asp:Label><br />
    <asp:TextBox ID="TextBox25" TabIndex="11" OnTextChanged="OnRatingTextBoxChanged" runat="server" Width="105px"  ></asp:TextBox><br />

    <asp:Label ID="Label26" runat="server"  >Rapid rating:</asp:Label><br />
    <asp:TextBox ID="TextBox26" TabIndex="11" OnTextChanged="OnRatingTextBoxChanged" runat="server" Width="105px"  ></asp:TextBox><br />
    
    <asp:CheckBox ID="CheckBox1" TabIndex="12" runat="server" Text="Wil indeling op email" TextAlign="Left"  ></asp:CheckBox>

    <br />
    <asp:Label ID="Label21" runat="server" Width="179px"  >Email adres:</asp:Label><br />
    <asp:TextBox ID="TextBox16" TabIndex="13" runat="server"  ></asp:TextBox><br />
    <br />

    <div id="divKNSB" class="zoekspeler" runat="server"  visible="false">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnSelectedIndexChanged="selecteer_Click" OnPageIndexChanging="GridView1_PageIndexChanging" Visible="false"  OnRowCreated="GridView_RowCreated">
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
                        <asp:TemplateField HeaderText="SelectIcon" ItemStyle-HorizontalAlign="Center">
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
    </div>
    <div id="divFIDE" class="zoekFIDEspeler" runat="server" visible="false">
        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnSelectedIndexChanged= "GV2_selecteer_Click"  OnPageIndexChanging="GridView2_PageIndexChanging" Visible="false"  OnRowCreated="GridView_RowCreated">
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
                        <asp:TemplateField HeaderText="SelectIcon" ItemStyle-HorizontalAlign="Center">
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
    </div>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="auto-style3">
    <asp:Button ID="Button6" TabIndex="14" runat="server" Width="256px" Text="Deelnemer toevoegen"  OnClick="Button6_Click"></asp:Button>

                <asp:Button ID="Button7" TabIndex="15" runat="server" Text="Nog een speler invoeren"  OnClick="Button7_Click"></asp:Button>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                &nbsp;&nbsp;
                <asp:Button ID="Button2" TabIndex="16" runat="server" Text="Gereed"  OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=Button1.ClientID %>").click();
            }
        }
        function SelectIcon(x) {
            x.src = "images/done2.jpg";
        }

        function UnselectIcon(x) {
            x.src = "images/unselect.jpg";
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Correction_Player_Data_Details.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Correction_Player_Data_Details" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style4 {
            width: 53%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divKNSB" runat="server" class="zoekspeler" visible="false">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="selecteer_Click" Visible="false" OnRowCreated="GridView_RowCreated">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Club Id" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Label131" runat="server" Text='<%# Bind("Club_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="KNSB Id">
                    <ItemTemplate>
                        <asp:Label ID="Label132" runat="server" Text='<%# Bind("KNSB_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Achternaam">
                    <ItemTemplate>
                        <asp:Label ID="Label133" runat="server" Text='<%# Bind("Achternaam")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tussenvoegsel">
                    <ItemTemplate>
                        <asp:Label ID="Label134" runat="server" Text='<%# Bind("Voornaam")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voornaam">
                    <ItemTemplate>
                        <asp:Label ID="Label135" runat="server" Text='<%# Bind("Tussenvoegsel")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voorletters">
                    <ItemTemplate>
                        <asp:Label ID="Label137" runat="server" Text='<%# Bind("Voorletters")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rating">
                    <ItemTemplate>
                        <asp:Label ID="Label136" runat="server" Text='<%# Bind("Rating")%>'></asp:Label>
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
    <div id="divFIDE" runat="server" class="zoekFIDEspeler" visible="false">
        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView2_PageIndexChanging" OnSelectedIndexChanged="GV2_selecteer_Click" Visible="false" OnRowCreated="GridView_RowCreated">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Bond Id" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="Label141" runat="server" Text='<%# Bind("Bond_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FIDE nummer">
                    <ItemTemplate>
                        <asp:Label ID="Label142" runat="server" Text='<%# Bind("FIDE_Id")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Titel">
                    <ItemTemplate>
                        <asp:Label ID="Label143" runat="server" Text='<%# Bind("Titel")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Naam">
                    <ItemTemplate>
                        <asp:Label ID="Label144" runat="server" Text='<%# Bind("Naam")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rating">
                    <ItemTemplate>
                        <asp:Label ID="Label146" runat="server" Text='<%# Bind("Rating")%>'></asp:Label>
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
    <table style="width: 100%;" border="1">
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label33" runat="server" Height="32px" Font-Names="Arial" Font-Size="14pt">Competitie</asp:Label>
                <asp:Label ID="Label33a" runat="server" Height="32px" Font-Names="Arial" Font-Size="12pt"> </asp:Label></td>
            <td style="width: 50%">
                <asp:Label ID="Label34" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Speler Id  </asp:Label><br />
                <asp:Label ID="Label34a" runat="server" Height="32px" Font-Names="Arial" Font-Size="12pt"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label3" runat="server" Height="32px" Font-Names="Arial" Font-Size="14pt">Achternaam</asp:Label><br />
                <asp:TextBox ID="TextBox3" TabIndex="1" Font-Size="12pt" runat="server" AutoPostBack="true" Width="150px" OnTextChanged="OnTextBoxMandatoryChanged" ></asp:TextBox><br />
                <asp:Button ID="Button5" TabIndex="2" runat="server" Text="Speler zoeken" Font-Names="Arial" Font-Size="12pt" OnClick="Button5_Click"></asp:Button>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label4" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Tussenvoegsel  </asp:Label><br />
                <asp:TextBox ID="TextBox4" TabIndex="3" AutoPostBack="true" OnTextChanged="OnTextBoxEmptyAllowedChanged" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label5" runat="server"  Font-Names="Arial" Font-Size="14pt" Height="32px">Voorletters </asp:Label><br />
                <asp:TextBox ID="TextBox5" TabIndex="4" AutoPostBack="true" OnTextChanged="OnTextBoxMandatoryChanged" Font-Size="12pt" runat="server" Width="301px"></asp:TextBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label6" runat="server"  Font-Names="Arial" Font-Size="14pt" Height="32px">Roepnaam </asp:Label><br />
                <asp:TextBox ID="TextBox6" TabIndex="5" AutoPostBack="true" OnTextChanged="OnTextBoxMandatoryChanged" Font-Size="12pt" runat="server" Width="301px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label35" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Titel </asp:Label><br />
                <asp:TextBox ID="TextBox17" TabIndex="6"  AutoPostBack="true" OnTextChanged="OnTextBoxEmptyAllowedChanged" Font-Size="12pt" runat="server" Width="97px"></asp:TextBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label8" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Telefoonnummer </asp:Label><br />
                <asp:TextBox ID="TextBox8" TabIndex="7" AutoPostBack="true" OnTextChanged="OnTextBoxEmptyAllowedChanged" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label7" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">KNSBnummer </asp:Label><br />
                <asp:TextBox ID="TextBox7" TabIndex="8"  AutoPostBack="true" OnTextChanged="OnTextBoxEmptyAllowedChanged" Font-Size="12pt" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label43" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">KNSBRating </asp:Label><br />
                <asp:TextBox ID="TextBox18"  Enabled="false" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label41" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">FIDEnummer </asp:Label><br />
                <asp:TextBox ID="TextBox1" TabIndex="9"  AutoPostBack="true" OnTextChanged="OnTextBoxEmptyAllowedChanged"  Font-Size="12pt" runat="server" Width="100px"></asp:TextBox><br />
                <asp:Button ID="Button4" runat="server" Text="Geen rating" Font-Names="Arial" Font-Size="12pt" OnClick="Button4_Click"></asp:Button>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label42" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">FIDERating </asp:Label><br />
                <asp:TextBox ID="TextBox2" Enabled="false" Font-Size="12pt" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label11" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px" >Rating </asp:Label><br />
                <asp:TextBox ID="TextBox11" TabIndex="10" AutoPostBack="true" OnTextChanged="OnRatingTextBoxChanged" Font-Size="12pt" runat="server" Width="80px" ></asp:TextBox>
                <br />
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label10" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px" >Startrating </asp:Label>
                <br />
                <asp:TextBox ID="TextBox10" Enabled="false" Font-Size="12pt" runat="server" Width="80px"></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label13" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Snelschaakrating </asp:Label>
                <br />
                <asp:TextBox ID="TextBox13" TabIndex="11" AutoPostBack="true" OnTextChanged="OnRatingTextBoxChanged" Font-Size="12pt" runat="server" Width="80px"></asp:TextBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label24" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px" >Rapidrating </asp:Label>
                <br />
                <asp:TextBox ID="TextBox24" TabIndex="12" AutoPostBack="true" OnTextChanged="OnRatingTextBoxChanged" Font-Size="12pt" runat="server" Width="80px"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label16" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Team </asp:Label>
                <br />
                <asp:TextBox ID="TextBox16" TabIndex="13" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Font-Size="12pt" runat="server" Width="23px"></asp:TextBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label17" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Clublid </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox17" TabIndex="14" AutoPostBack="true" Font-Size="12pt" runat="server" Width="50px" Text="  "></asp:CheckBox>
            </td>        
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label18" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Deelnemer_teruggetrokken </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox18" TabIndex="15" AutoPostBack="true" Font-Size="12pt" runat="server" Width="50px" Text=" "></asp:CheckBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label19" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Speelt_mee_sinds_ronde </asp:Label>
                <br />
                <asp:TextBox ID="TextBox19" TabIndex="16" AutoPostBack="true"  OnTextChanged="OnIntTextBoxChanged" Font-Size="12pt" runat="server" Width="31px"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label26" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Vrijgeloot </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox26" TabIndex="17" Font-Size="12pt" runat="server" Width="80px" Text=" "></asp:CheckBox>
            </td>
            <td style="width: 50%">
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label27" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Wants_Email </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox27" TabIndex="18" Font-Size="12pt" runat="server" Width="50px" Text=" "></asp:CheckBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label28" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Email_Address </asp:Label>
                <br />
                <asp:TextBox ID="TextBox28" TabIndex="19" AutoPostBack="true"  OnTextChanged="OnTextBoxEmptyAllowedChanged" Font-Size="12pt" runat="server" Width="95%"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label31" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Member_Premier_Group </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox31" TabIndex="20" Font-Size="12pt" runat="server" Width="50px" Text=" "></asp:CheckBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label32" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">ProfilePicture </asp:Label>
                <br />
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/nofoto.png" />
                <asp:FileUpload ID="FileUpload1" TabIndex="21" runat="server" Font-Size="12pt" Width="360px" />&nbsp;&nbsp;
                <asp:ImageButton ID="RotateButton" ImageUrl="~/images/sub_black_rotate_cw.png" Width="40" TabIndex="9" runat="server" Text="" OnClick="RotateButton_Click"></asp:ImageButton>
                <asp:Button ID="Button1" TabIndex="23" runat="server" Text="Toon foto" Style="display: none" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label20" visible="false" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Doet_mee_met_snelschaak </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox20" visisble="false" Font-Size="12pt" runat="server" Width="36px" Text="  "></asp:CheckBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label21" visible="false" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Speelt_blitz_sinds_ronde </asp:Label>
                <br />
                <asp:TextBox ID="TextBox21" visible="false" AutoPostBack="true" Font-Size="12pt" runat="server" Width="33px"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td style="width: 50%">
                <asp:Label ID="Label29" visible="false" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Wants_SMS </asp:Label>
                <br />
                <asp:CheckBox ID="CheckBox29" visible="false" Font-Size="12pt" runat="server" Width="50px" Text=" "></asp:CheckBox>
            </td>
            <td style="width: 50%">
                <asp:Label ID="Label30" visible="false" runat="server" Font-Names="Arial" Font-Size="14pt" Height="32px">Mobile_Number </asp:Label>
                <br />
                <asp:TextBox ID="TextBox30" visible="false" Font-Size="12pt" runat="server" Width="151px"></asp:TextBox>
            </td>
        </tr>
        </table>
    <table style="width: 100%;">
        <tr>
            <td style="width: 50%">
                <asp:Button ID="Button2" TabIndex="24" runat="server" Text="Bewaren" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
            <td style="width: 50%; text-align: right">
                <asp:Button ID="Button3" TabIndex="25" Text="Done" runat="server" Font-Size="12pt" OnClick="Button3_Click"></asp:Button>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_ZS_Tournament_Parameters.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_ZS_Tournament_Registration" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table border="1">
        <tr>
            <td>
                <asp:Label ID="Toernooi_Id" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox3" runat="server" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Aanmaak_Datum" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox4" runat="server" Width="120"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Toernooi_Naam" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox5" runat="server" Width="300"></asp:TextBox>
            </td>
        </tr>

        <tr>

            <td>
                <asp:Label ID="Hoofdsponsor" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox6" runat="server" AutoPostBack="true" Width="300"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Subsponsors" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox7" runat="server" AutoPostBack="true" Width="300"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Aantal_Ronden" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox8" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Aantal_Partijen_Per_Uitslag" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox9" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Indelings_Modus" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox10" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Aantal_Rating_Groepen" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox11" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
        </tr>

        <tr>

            <td>
                <asp:Label ID="Aantal_Invoerpunten" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox12" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>

            <td>
                <asp:Label ID="Decentrale_Invoer_Spread" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox13" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Decentrale_Invoer_Maximaal" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox14" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="KFactor" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox15" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Restrictie_Ronden" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox16" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>

            <td>
                <asp:Label ID="Restrictie_Rating_Grens" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox17" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged"  Width="400"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Laatste_ronde" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox24" runat="server" AutoPostBack="true" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                &nbsp;&nbsp;
            </td>
            <td>
                <asp:Label ID="Website_Basis" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox25" runat="server" Width="250"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Website_Template" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox18" runat="server" Width="250"></asp:TextBox>
            </td>

            <td>
                <asp:Label ID="Website_Competitie" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox19" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Client_FTP_Host" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox20" runat="server" Width="250"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Client_FTP_UN" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox21" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Client_FTP_PW" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox22" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Current_State" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox23" runat="server" Width="40" Visible="false"></asp:TextBox>
                <asp:DropDownList ID="DDLStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_SelectedChange" Height="17px" Width="201px"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Toernooi_Logo" runat="server" Font-Size="14pt">Toernooi Logo:</asp:Label><br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:FileUpload ID="FileUpload1" runat="server" Font-Size="12pt" Width="360px" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button3" runat="server" Text="Toon foto" Style="display: none" OnClick="Button3_Click"></asp:Button>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="Button3" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Image ID="Image2" TabIndex="9" runat="server" AlternateText="Afbeelding niet gevonden" width="100" ImageUrl="~/images/TSGSLogo.png"></asp:Image>
                <asp:ImageButton ID="RotateButton" ImageUrl="~/images/sub_black_rotate_cw.png" Width="40" TabIndex="9" runat="server" Text="" OnClick="RotateButton2_Click"></asp:ImageButton>
                <br />
                <br />
            </td>
            <td></td>
            <td>
                <asp:Label ID="ProfilePicture" runat="server" Font-Size="14pt">No foto picture:</asp:Label><br />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:FileUpload ID="FileUpload2" runat="server" Font-Size="12pt" Width="360px" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button4" runat="server" Text="Toon foto" Style="display: none" OnClick="Button4_Click"></asp:Button>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="Button4" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Image ID="Image1" TabIndex="9" runat="server" AlternateText="Afbeelding niet gevonden" width="100" ImageUrl="~/images/nofoto.png"></asp:Image>
                <asp:ImageButton ID="RotateButton2" ImageUrl="~/images/sub_black_rotate_cw.png" Width="40" TabIndex="9" runat="server" Text="" OnClick="RotateButton_Click"></asp:ImageButton>
                <br />
                <br />
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">
                <!-- additional footer buttons -->
                <asp:Button ID="Button2" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=Button3.ClientID %>").click();
            }
        }
        function UploadFile2(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=Button4.ClientID %>").click();
            }
        }
    </script>
</asp:Content>

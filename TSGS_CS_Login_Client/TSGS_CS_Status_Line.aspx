<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TSGS_CS_Status_Line.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Status_Line" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    	<title>TSGS_CS_Status_Line</title>
		<meta http-equiv="Refresh" content="5" />
		<link href="templates/modern.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    	<asp:Image id="Image1" style="Z-INDEX: 101; LEFT: 552px; POSITION: absolute; TOP: 8px" runat="server"
			Width="16px" Height="16px" ImageUrl="~/images/INDRED.bmp"></asp:Image>
		<asp:Image id="Image3" style="Z-INDEX: 109; LEFT: 816px; POSITION: absolute; TOP: 8px" runat="server"
			Width="16px" Height="16px" ImageUrl="~/images/WWWRED.bmp"></asp:Image>
		<asp:Label id="Label2" style="Z-INDEX: 108; LEFT: 728px; POSITION: absolute; TOP: 8px" runat="server"
			Width="80px">Gepubliceerd</asp:Label>
		<asp:Image id="Image4" style="Z-INDEX: 107; LEFT: 712px; POSITION: absolute; TOP: 8px" runat="server"
			Width="16px" Height="16px" ImageUrl="~/images/OVZRED.bmp"></asp:Image>
		<asp:Label id="Label1" style="Z-INDEX: 106; LEFT: 648px; POSITION: absolute; TOP: 8px" runat="server">Overzicht</asp:Label>
		<asp:Image id="Image2" style="Z-INDEX: 105; LEFT: 632px; POSITION: absolute; TOP: 8px" runat="server"
			Width="16px" Height="16px" ImageUrl="~/images/RESRED.bmp"></asp:Image>
		<asp:Label id="lbl_csc_Status_Line_Indeling" style="Z-INDEX: 103; LEFT: 496px; POSITION: absolute; TOP: 8px"
			runat="server" Width="56px" Height="16px" BackColor="Transparent">Indeling:</asp:Label>
		<asp:Label id="lbl_csc_Status_Line_Ronde" style="Z-INDEX: 102; LEFT: 424px; POSITION: absolute; TOP: 8px"
			runat="server" Width="64px" Height="16px" BackColor="Transparent">Ronde:</asp:Label>
		<asp:Label id="lbl_csc_Status_Line_Resultaten" style="Z-INDEX: 104; LEFT: 568px; POSITION: absolute; TOP: 8px"
			runat="server">Uitslagen</asp:Label>
		<asp:Label id="Label3" style="Z-INDEX: 110; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server"
			Height="16px" Width="408px" BackColor="Transparent">No club, no competition selected</asp:Label>
        <asp:ImageButton ID="ImageButton3" style="Z-INDEX: 108; LEFT: 900px; POSITION: absolute; TOP: 8px" runat="server"
			Width="16px" Height="16px" ImageUrl="~/images/home.jpg" OnClick="ImageButton3_Click" />


    </div>
    </form>
</body>
</html>

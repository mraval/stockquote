<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Quote Application</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    
    </head>
<body>
    <h1> Stock Advisor</h1>
    <form id="form1" runat="server">
        <div>
    
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Enter Stock Symbol : "></asp:Label>
    
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get Quote" Width="85px" />
        &nbsp;<br />
        <br />
        <asp:TextBox ID="TextBox2" runat="server" Height="246px" TextMode="MultiLine" Width="717px"></asp:TextBox>
    
    </div>
    <p></p>
    </form>
    <asp:Label ID="Advice" runat="server" ForeColor="Red" Height="18px"></asp:Label>
</body>
</html>

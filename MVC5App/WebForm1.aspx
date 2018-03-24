<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web.Services" %>
@{
    ViewBag.Title = "SurgeViewer";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
    <link href="~/Views/Shared/_Layout.cshtml" rel="stylesheet" type="text/css" />
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
<script type="text/C#" runat="server">
    [WebMethod]
    public static string SayHello(string name)
    {
        if(String.IsNullOrEmpty(name))
        {
            return "is empty String";
        }
        return "Hello " + name;
    }

    [WebMethod]
    public static string InsertRow(string rowStringInput)
    {
        var tempQueryTools = new MVC5App.Scripts.Test.QueryTools();
        if (!(String.IsNullOrEmpty(rowStringInput)))
        {
            tempQueryTools.insertString(rowStringInput);
            return "sucess";
        }
        return "fail";
    }
</script>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <script type="text/javascript" src="/scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: 'POST',
                url: 'WebForm1.aspx/sayhello',
                data: JSON.stringify({ name: 'John' }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    // Notice that msg.d is used to retrieve the result object
                    alert(msg.d);
                }
            });
        });
    </script>
</head>
<body>
    <form id="Form1" runat="server">

    </form>
</body>
</html>
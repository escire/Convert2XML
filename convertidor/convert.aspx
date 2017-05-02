<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="convert.aspx.cs" Inherits="jats.convert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Upload File</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <style>
        #divUpload, #divErrorReq{
            margin:15px;
        } 
        .panel-info>.panel-heading {
            background-image: -webkit-linear-gradient(top,#e54c34 0,#e54c34 100%);
            background-image: -o-linear-gradient(top,#e54c34 0,#e54c34 100%);
            background-image: -webkit-gradient(linear,left top,left bottom,from(#e54c34),to(#e54c34));
            background-image: linear-gradient(to bottom,#e54c34 0,#e54c34 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#e54c34', endColorstr='#e54c34', GradientType=0);
            background-repeat: repeat-x;
        }
        .panel-info {
            border-color: #e54c34;
        }
        .panel-info>.panel-heading {
            color: #fff;
            background-color: #fff;
            border-color: #fff;
        }
        .row {
            padding: 5px;
        }
        .btn-primary {
            background-image: -webkit-linear-gradient(top,#e54c34 0,#e54c34 100%);
            background-image: -o-linear-gradient(top,#e54c34 0,#e54c34 100%);
            background-image: -webkit-gradient(linear,left top,left bottom,from(#e54c34),to(#e54c34));
            background-image: linear-gradient(to bottom,#e54c34 0,#e54c34 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#e54c34', endColorstr='#e54c34', GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            background-repeat: repeat-x;
            border-color: #e54c34;
            color: #FFF;
        }
        .btn-primary:focus, .btn-primary:hover {
            background-color: #c32912;
            background-position: 0 -15px;
            filter: alpha(opacity=50); /* For IE8 and earlier */
        }
        .btn-primary:hover {
            color: #fff;
            border-color: #c32912;
        }
        .btn-primary.disabled, .btn-primary.disabled.active, .btn-primary.disabled.focus, .btn-primary.disabled:active, .btn-primary.disabled:focus, .btn-primary.disabled:hover, .btn-primary[disabled], .btn-primary[disabled].active, .btn-primary[disabled].focus, .btn-primary[disabled]:active, .btn-primary[disabled]:focus, .btn-primary[disabled]:hover, fieldset[disabled] .btn-primary, fieldset[disabled] .btn-primary.active, fieldset[disabled] .btn-primary.focus, fieldset[disabled] .btn-primary:active, fieldset[disabled] .btn-primary:focus, fieldset[disabled] .btn-primary:hover {
            background-color: #c32912;
            background-image: none;
        }
        input#subirBoton, input#inicarConversion, input#subirArchivo, #statusLabel {
            width: 70%;
            display: block;
            margin: 0 auto;
        }
        div#divCargando{
            width: 100%;
            margin: 10px;
        }
        div#divExito{
            width: 30%;
            margin: auto;
        }
        div#divCargando img, div#divExito img{
            width: 100%;
            margin: auto;
        }
        div#divErrorConversion {
            width: 100%;
            margin: 0 auto;
            text-align: center;
            padding: 15px;
        }
    </style>
    <script>
        function closeMe() {
            window.opener = self;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="divUpload" runat="server">
        <div class="panel  panel-info">
              <div class="panel-heading">Nueva Conversión</div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <div class="page-header">
                              <h4>Selecciona archivo: <small>(docx)</small></h4>
                            </div>
                        </div>
                        <div class="col-md-6">
                                <asp:FileUpload ID="subirArchivo" runat="server" CssClass="btn btn-default" AllowMultiple="false" accept="application/vnd.openxmlformats-officedocument.wordprocessingml.document"/>
                                <br />
                                <asp:Button runat="server" id="subirBoton" text="Upload" onclick="subirBoton_Click" CssClass="btn btn-primary"/>
                                <br />
                                <asp:Label runat="server" id="statusLabel" text="" />
                        </div>
                        <br />
                    </div> 
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                              <h4 runat="server" id="labelIniciar"></h4>
                        </div>
                        <div class="col-md-6">
                            <asp:Button runat="server" id="inicarConversion" text="Iniciar !" onclick="inicarConversion_Click" CssClass="btn btn-primary" Enabled="false"/>
                        </div>
                    </div> 

              <div class="panel-body">
              </div>
        </div>
    </asp:Panel>
    
    <asp:Panel  runat="server" ID="divErrorReq" Visible="false">
        <h4>No se cumplen los requerimientos</h4>
        <ul class="list-group">
            
            <li class="list-group-item">Título de la Revista</li>
            <li class="list-group-item">Abreviatura de la Revista</li>
            <li class="list-group-item">ISSN impreso de la Revista</li>
            <li class="list-group-item">Editorial/Institución de la revista</li>
            <li class="list-group-item">Número de cada artículo a convertir (volumen, número y año)</li>
            <li class="list-group-item">Páginas de cada artículo a convertir</li>
            <li class="list-group-item">Fecha de publicación de cada artículo a convertir</li>
        </ul>
    </asp:Panel>

    <asp:Panel runat="server" ID="divCargando" Visible="false">
        <br />
        <h3>Convirtiendo ..</h3>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/cargando.gif"/>
    </asp:Panel>

    <asp:Panel runat="server" ID="divErrorConversion" Visible="false">
        <h3>No fue posible realizar la conversión</h3>
        <asp:Label ID="LabelError" runat="server" Text="Label"></asp:Label>
    </asp:Panel>


    <asp:Panel runat="server" ID="divExito" Visible="false">
        <h3>Descargando...</h3>
        <img src="img/descargar.png" />
    </asp:Panel>
        
    </form>
    
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="jats.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Working!</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous"/>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <style>
        body{
            background-color: #e54c34;
        }
        .row {
            text-align: center;
            padding: 20px;
        }
        img:hover{
            opacity: 0.5;
            filter: alpha(opacity=50); /* For IE8 and earlier */
        }
        a {
            color: #FFF;
        }
        a:hover {
            color: #dAdAdA;
            text-decoration: none;
        }
        #footer {
            border-top: 1px solid #000;
            background: #222;
            color: #7B7B7B;
            font-size: 13px;
            position: relative;
            z-index: 100;
            clear: both;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
		<div class="container">
			<div class="row">
                 <div class="col-md-3"></div>
                 <div class="col-md-6">
                     <a href="https://github.com/escire/Convert2XML/">
                            <img src="http://escire.mx/wp-content/uploads/2015/06/icono1-1.png" alt="Automatización de Bibliotecas" class="img-circle aligncenter"/>
                    </a>
                    <h3 class="home-widget-title"><a href="http://escire.mx/?page_id=20">Convert2xml</a></h3>
                 </div>
            </div>
		</div>
        <footer id="footer" role="contentinfo">
		    <div class="row">
                <div>
                    <h4 class="widget-title">Contáctanos</h4>			
                    <div class="textwidget"><b>Consultoría, Tecnologías y Gestión del Conocimiento SA de CV</b>
                            <br/>
                            Tel. +52 (222) 230 5594
                            <br/>
                            <a href="mailto:contacto@escire.mx">contacto@escire.mx</a>
                            <br/>
                            Recta a Cholula #308 Casa 2, Col. Lázaro Cárdenas, San Andrés Cholula, Puebla, C.P. 72810
                    </div>
                    <br/>
                </div>
	        </div>
        </footer>
    </form>
</body>
</html>

# Convert2XML

![eScire](http://escire.net/convert2xml/logo_convert2xml.jpg)

## English

**Convert2XML** is composed of two developments required for its operation: a plugin that integrates into the system "Open Journal Systems" and a webservice that is responsible for processing the file conversion. Below are the requirements and installation instructions for each development.:

### Webservice

#### Requirements
- Windows Server
- Microsoft Office 2013 (Word)
- IIS Express
- Visual Studio 2013

#### Installation

1. Download all files inside folder "webservice".
2. Create a deployment package on the source computer by using Visual Studio 2013.
3. In IIS, create a new entry in the file "config\applicationhost.config" in "sites".
4. Start IIS.

### Plugin

1. Download all files inside folder "plugin_ojs", compress them and name the file: converter.tar.gz.
2. Log into your OJS installation as manager.
3. Go to "System modules".
4. Go to "Install a new module".
5. Install the file converter.tar.gz.
6. Within generic modules, go to the module configuration and write the data: user, password and URL of the webservice. If you want you can register in http://converter.escire.mx/admin/Registro.aspx to use our webservice for 30 days for free.
7. When installing the converter will create a user with editor permissions, do not edit the user, edit the user data in the ConverterPlugin.inc.php this file.
8. Add the hook to call in the file: "[OJS]/templates/sectionEditor/submissionEditing.tpl". Edit the file by adding the following line:<br />
{call_hook name="Converter"}<br />
it should be placed just before the line:<br />
{include file="sectionEditor/submission/layout.tpl"}

## Español

**Convert2XML** está compuesto por dos desarrollos necesarios para su funcionamiento: un plugin que se integra al sistema Open Journal Systems y un servicio web que se encarga de procesar la conversión de los archivos. A continuación se indican los requerimientos y las instrucciones de instalación para cada desarrollo:

### Servicio web

#### Requerimientos
- Windows Server
- Microsoft Office 2013 (Word)
- IIS Express
- Visual Studio 2013

#### Instalación

1. Descargar los archivos de la carpeta "webservice".
2. Crear un paquete de implementación en el equipo de origen utilizando Visual Studio 2013.
3. En IIS, crear una nueva entrada en el archivo "config\applicationhost.config" dentro de "sites".
4. Iniciar IIS.

### Plugin

1. Descarga los archivos de la carpeta "plugin_ojs", comprímelos y nombra al archivo: converter.tar.gz.
2. Ingresa a tu instalación de OJS como gestor.
3. Entra a "Módulos del Sistema".
4. Entra a "Instalar un nuevo módulo".
5. Instala el archivo converter.tar.gz.
6. Dentro de módulos genéricos, ve a la configuración del módulo y escribe los datos: usuario, contraseña y url del servicio web. Si lo deseas puedes regístrate en http://converter.escire.mx/admin/Registro.aspx para probar nuestro servicio web por 30 días gratis.
7. Al momento de instalar el convertidor se va a crear un usuario con permisos de editor, no edite el usuario, edite los datos de este usuario en el archivo ConverterPlugin.inc.php.
8. Agregue el hook de llamado en el archivo: "[OJS]/templates/sectionEditor/submissionEditing.tpl". Edite el archivo agregando la siguiente línea:<br />
{call_hook name="Converter"}<br />
deberá colocarla justo antes la línea:<br />
{include file="sectionEditor/submission/layout.tpl"}

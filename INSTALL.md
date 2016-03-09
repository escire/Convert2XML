# Convert2XML

![eScire](http://escire.net/convert2xml/logo_convert2xml.jpg)

## English


## Español

El proyecto **Convert2XML** está compuesto por dos desarrollos necesarios para su funcionamiento: un plugin que se integra al sistema Open Journal Systems y un servicio web que se encarga de procesar la conversión de los archivos. A continuación se indican los requerimientos y las instrucciones de instalación para cada desarrollo:

### Servicio web

**Requerimientos**
- Windows Server
- Microsoft Office 2013 (Word)
- IIS Express
- Visual Studio 2013

**Instalación**

1. Descargar los archivos de la carpeta "webservice".
2. Crear un paquete de implementación en el equipo de origen utilizando Visual Studio 2013.
3. En IIS, crear una nueva entrada en el archivo "config\applicationhost.config" dentro de "sites".
4. Iniciar IIS.

## Plugin

1. Descarga los archivos de la carpeta "plugin_ojs", comprímelos y nombra al archivo: converter.tar.gz.
2. Ingresa a tu revista como gestor.
3. Entra a "Módulos del Sistema".
4. Entra a "Instalar un nuevo módulo".
5. Instala  el archivo converter.tar.gz.
6. Dentro de módulos genéricos, ve a la configuración del módulo y escribe los datos: usuario, contraseña y url del servicio web. Si lo deseas puedes regístrate en http://converter.escire.mx/admin/Registro.aspx para probar nuestro servicio web por 30 días gratis.
7. Al momento de instalar el convertidor se va a crear un usuario con permisos de editor, no edite el usuario, edite los datos de este usuario en el archivo ConverterPlugin.inc.php.
8. Agregue el hook de llamado en el archivo: "[OJS]/templates/sectionEditor/submissionEditing.tpl". Edite el archivo agregando la siguiente línea:
{call_hook name="Converter"}

deberá colocarla justo antes la línea:
{include file="sectionEditor/submission/layout.tpl"}

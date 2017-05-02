# Convert2XML

![eScire](http://escire.net/convert2xml/logo_convert2xml.jpg)

## English
\([Ver en español](#español)\)

**Convert2XML** is composed of two developments required for its operation: a plugin that integrates into the system "Open Journal Systems" and a webservice that is responsible for processing the file conversion. Below are the requirements and installation instructions for each development.:

### Webservice

#### Requirements
- Windows Server
- Microsoft .NET Framework 4.6.2
- Microsoft Office 2013 (Word)
- IIS Express
- XmlPrime for the .NET Framework
- Java SE Development Kit 8

#### Installation

1. Download and unzip the "convertidor.zip" file.
2. Configure the "Web.config" file
3. In IIS, create a new entry in the file "config\applicationhost.config" in "sites".
4. Start IIS.

### Plugin

1. Download the file: convert2xml.tar.gz.
2. Log into your OJS installation as manager.
3. Go to "System modules".
4. Go to "Install a new module".
5. Install the file convert2xml.tar.gz.
6. Within generic modules, go to the module configuration and write the data: URL of the webservice. 

## Español
\([View in english](#english)\)

**Convert2XML** está compuesto por dos desarrollos necesarios para su funcionamiento: un plugin que se integra al sistema Open Journal Systems y un servicio web que se encarga de procesar la conversión de los archivos. A continuación se indican los requerimientos y las instrucciones de instalación para cada desarrollo:

### Servicio web

#### Requerimientos
- Windows Server
- Microsoft .NET Framework 4.6.2
- Microsoft Office 2013 (Word)
- IIS Express 
- XmlPrime for the .NET Framework
- Java SE Development Kit 8

#### Instalación

1. Descargar y descomprimir el archivo "convertidor.zip".
2. Configurar el archivo "Web.config"
3. En IIS, crear una nueva entrada en el archivo "config\applicationhost.config" dentro de "sites".
4. Iniciar IIS.

### Plugin

1. Descarga el archivo convert2xml.tar.gz.
2. Ingresa a tu instalación de OJS como gestor.
3. Entra a "Módulos del Sistema".
4. Entra a "Instalar un nuevo módulo".
5. Instala el archivo convert2xml.tar.gz..
6. Dentro de módulos genéricos, ve a la configuración del módulo y escribe los datos: url del servicio web. 

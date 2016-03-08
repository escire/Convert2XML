using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using System.IO;
using Microsoft.Office;
using Microsoft.Office.Interop.Word;
using Ionic.Zip;
using System.Configuration;

namespace convertidor
{
    public partial class convertir : System.Web.UI.Page
    {
        Dictionary<string, string> mensaje = new Dictionary<string, string>();

        Dictionary<string, string> parametrosXML = new Dictionary<string, string>();

        private String urlServer;
        private String rutaTemporal;

        private string GetUserIP()
        {
            return Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            urlServer = ConfigurationManager.AppSettings["site.url"];
            rutaTemporal = @ConfigurationManager.AppSettings["site.temporal"];

            string cliente = Request.QueryString["cliente"];
            string clave = Request.QueryString["clave"];
            string archivo = Request.QueryString["archivo"];
            string url = Request.QueryString["url"];
            string formato = Request.QueryString["formato"];
			
			cliente = clave = "generico";

            



            //parametros para generar XML
            parametrosXML.Add("revistaId", Request.QueryString["revistaId"]);
            parametrosXML.Add("revistaTitulo", Request.QueryString["revistaTitulo"]);
            parametrosXML.Add("onlineIssn", Request.QueryString["onlineIssn"]);
            parametrosXML.Add("printIssn", Request.QueryString["printIssn"]);
            parametrosXML.Add("abbreviation", Request.QueryString["abbreviation"]);
            parametrosXML.Add("publisher", Request.QueryString["publisher"]);
            //parametrosXML.Add("",     Request.QueryString[""]);

            bool valido = validar(cliente, clave, archivo, url, formato);

            if (valido)
            {
                bool validoXML = true;

                if (formato.Equals("xml"))
                    validoXML = validarXML();

                if (validoXML)
                {
                    new BaseDatos().log("Iniciando conversión a" + formato + " de " + url + archivo, GetUserIP(), cliente);
                    convierte(cliente, archivo, url, formato);
                }

            }
            else
            {
                new BaseDatos().log("Conversión a " + formato + " rechazada: " + url + archivo, GetUserIP(), cliente);
            }


        }



        protected void convierte(string cliente, string archivo, string url, string formato)
        {
            WebClient wc = new WebClient();
            string urlcomp = url + archivo;
            string archivoSinCar = RemoveSpecialCharacters(archivo);
            string carpetaArchivo = Path.GetFileNameWithoutExtension(archivoSinCar);
            string ruta = rutaTemporal + cliente + "\\" + carpetaArchivo + "\\";
            String archivoFinal = ruta + archivoSinCar;
            Uri uri = new Uri(urlcomp);
            
            try
            {
                bool existe = Directory.Exists(ruta);
                if (!existe)
                    Directory.CreateDirectory(ruta);

                wc.DownloadFile(uri, @archivoFinal);
            }
            catch (WebException we)
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Error al obtener el archivo, asegurese de que el archivo existe en el servidor");
                Response.Write(generaMensaje(mensaje));
                System.Console.Error.WriteLine("exception: {0}", we);
                return;
            }

            if (File.Exists(archivoFinal))
            {
                Dictionary<string, string> res = convierteArchivoA(ruta, archivoSinCar, formato, cliente);
                String conv = res["estatus"];
                if (conv.Equals("exito"))
                {
                    mensaje.Add("status", "exito");
                    mensaje.Add("message", "Archivo convertido exitosamente");
                    mensaje.Add("downloadURL", res["downloadURL"]);
                    mensaje.Add("zipFileName", res["zipFileName"]);
                    mensaje.Add("fileName", res["fileName"]);
                    mensaje.Add("imageFolder", res["imageFolder"]);
                    mensaje.Add("convertionType", res["convertionType"]);
                    Response.ContentType = "application/json";
                    Response.Write(generaMensaje(mensaje));

                }
                else
                {
                    mensaje.Add("estatus", "error");
                    String mensajeDetalle = "";
                    if ( res.ContainsKey("mensaje") )
                        mensajeDetalle = res["mensaje"];
                    mensaje.Add("mensaje", "No se puede convertir el archivo, verifique que el formato sea correcto, si el problema persiste contacte al proveedor del servicio error (999). " + mensajeDetalle);
                    Response.Write(generaMensaje(mensaje));
                }
            }
            else
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "No se pudo crear el archivo en el servidor, consulte al proveedor para obtener soporte de este error");
                Response.Write(generaMensaje(mensaje));
                return;
            }




        }


        protected Dictionary<string, string> convierteArchivoA(string ruta, string nombreArchivo, string formato, string cliente)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("estatus", "error");
            Document document = null;
            Application applicationclass = null;
            applicationclass = new Application();

            object missingType = Type.Missing;
            object readOnly = true;
            object isVisible = false;
            object rutaArchivo = ruta + nombreArchivo;
            object formatoConversion = tipoArchivo(formato);
            object rutaArchivoConver = ruta + Path.GetFileNameWithoutExtension(ruta + nombreArchivo) + "." + formato;
            string archivoNomFinal = Path.GetFileNameWithoutExtension(ruta + nombreArchivo) + "." + formato;

            if (formato.Equals("xml"))
            {
                ProcesaDocumento pd = new ProcesaDocumento(ruta + nombreArchivo, ruta, parametrosXML);
                if (pd.procesa())
                {
                    if (!pd.generaXMLV4())
                        res.Add("mensaje", pd.Mensaje);
                }
                else
                {
                    res.Add("mensaje", pd.Mensaje);
                }
            }
            else
            {
                applicationclass.Documents.Open(ref rutaArchivo,
                                                ref readOnly,
                                                ref missingType, ref missingType, ref missingType,
                                                ref missingType, ref missingType, ref missingType,
                                                ref missingType, ref missingType, ref isVisible,
                                                ref missingType, ref missingType, ref missingType,
                                                ref missingType, ref missingType);

                applicationclass.Visible = false;
                document = applicationclass.ActiveDocument;


                document.SaveAs(ref rutaArchivoConver, ref formatoConversion, ref missingType,
                    ref missingType, ref missingType, ref missingType,
                    ref missingType, ref missingType, ref missingType,
                    ref missingType, ref missingType, ref missingType,
                    ref missingType, ref missingType, ref missingType,
                    ref missingType);

                document.Close(ref missingType, ref missingType, ref missingType);
                applicationclass.Quit();
            }

            if (formato.Equals("html"))
            {
                aplicaFormato(rutaArchivoConver.ToString(), Path.GetFileNameWithoutExtension(ruta + nombreArchivo));
            }

            if (File.Exists(rutaArchivoConver.ToString()))
            {

                bool comprimido = comprime(ruta, archivoNomFinal, formato);
                if (comprimido)
                {
                    string nombreArchivoZip = Path.GetFileNameWithoutExtension(nombreArchivo) + ".zip";
                    string urlServidor = urlServer+"descargar.ashx?cliente=" + cliente + "&archivo=" + nombreArchivoZip;
                    res.Add("downloadURL", urlServidor);
                    res["estatus"] = "exito";
                    res.Add("zipFileName", nombreArchivoZip);
                    res.Add("fileName", Path.GetFileNameWithoutExtension(ruta + nombreArchivo) + "." + formato);
                    res.Add("imageFolder", "images");
                    res.Add("convertionType", formato);
                    ;
                }
                else
                {
                    res.Add("mensaje", "No se puede comprimir el archivo");
                }
            }

            return res;
        }

        protected bool validarXML()
        {
            foreach (String xx in parametrosXML.Keys)
            {
                if (xx.Equals("revistaTitulo") || xx.Equals("abbreviation") || xx.Equals("publisher"))
                {
                    if (string.IsNullOrEmpty(parametrosXML[xx]))
                    {
                        mensaje.Add("estatus", "error");
                        String temp = xx.Equals("revistaTitulo") ? "Título de la Revista" : xx;
                        mensaje.Add("mensaje", "En su revista, dentro de OJS, debe capturar: " + temp);
                        Response.Write(generaMensaje(mensaje));
                        return false;
                    }
                }
            }
            if( string.IsNullOrEmpty( parametrosXML["onlineIssn"] ) && string.IsNullOrEmpty( parametrosXML["printIssn"]) ){
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "En su revista, dentro de OJS, debe capturar: ISSN");
                Response.Write(generaMensaje(mensaje));
                return false;
            }

            return true;
        }


        protected bool validar(string cliente, string clave, string archivo, string url, string formato)
        {
            if (string.IsNullOrEmpty(cliente) || string.IsNullOrEmpty(clave))
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Configure su usuario y/o contraseña dentro de OJS");
                Response.Write(generaMensaje(mensaje));
                return false;
            }

            
            if (string.IsNullOrEmpty(archivo))
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Especifique el nombre del archivo");
                Response.Write(generaMensaje(mensaje));
                return false;
            }
           
            if (string.IsNullOrEmpty(url))
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Especifique una url");
                Response.Write(generaMensaje(mensaje));
                return false;
            }

            if (string.IsNullOrEmpty(formato))
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Especifique el formato de conversión");
                Response.Write(generaMensaje(mensaje));
                return false;
            }

            if (!(formato.Equals("html") || formato.Equals("pdf") || formato.Equals("epub") || formato.Equals("xml")))
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "So lo se permite convertir archivos con formatos: .html, .pdf, .txt, .xml");
                Response.Write(generaMensaje(mensaje));
                return false;
            }

            if (!System.IO.Directory.Exists(rutaTemporal + cliente))
            {
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "El cliente no existe");
                Response.Write(generaMensaje(mensaje));
                return false;
            }

            return true;
        }

        protected bool comprime(string ruta, string nombreArchivo, string formato)
        {
            bool comprimido = false;
            string rutaArchivo = ruta + nombreArchivo;
            string carpetaArchivos = ruta + Path.GetFileNameWithoutExtension(nombreArchivo) + "_archivos";
            string rutaZip = ruta + Path.GetFileNameWithoutExtension(nombreArchivo) + ".zip";

            using (ZipFile zip = new ZipFile())
            {
                try
                {
                    zip.AddFile(rutaArchivo, "");
                    if (formato.Equals("html") || formato.Equals("xml"))
                    {
                        if (Directory.Exists(carpetaArchivos))
                            zip.AddDirectory(carpetaArchivos, "");
                    }
                    zip.Save(rutaZip);
                    comprimido = true;
                }
                catch (Exception e)
                {
                    System.Console.Error.WriteLine("exception: {0}", e);
                }
            }

            try
            {
                File.Delete(rutaArchivo);
                Directory.Delete(carpetaArchivos, true);

            }catch(Exception er){}



            return comprimido;
        }

        protected string generaMensaje(Dictionary<string, string> mensajes)
        {
            string json = "";
            json = new JavaScriptSerializer().Serialize(mensajes);
            mensaje.Clear();
            return json;
        }

        protected static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        protected object tipoArchivo(string tipo)
        {

            if (tipo.Equals("html"))
                return 10;

            if (tipo.Equals("pdf"))
                return WdSaveFormat.wdFormatPDF;

            if (tipo.Equals("txt"))
                return WdSaveFormat.wdFormatText;

            return 10;
        }

        protected void aplicaFormato(string archivo, string nomArchivo)
        {
            StringBuilder newFile = new StringBuilder();
            string temp = "";
            string[] file = File.ReadAllLines(archivo, Encoding.Default);

            foreach (string line in file)
            {
                if (line.Contains(nomArchivo + "_archivos/"))
                {
                    temp = line.Replace(nomArchivo + "_archivos/", "");
                    newFile.Append(temp);
                    continue;
                }

                if (line.Contains("position:relative;"))
                {
                    temp = line.Replace("position:relative;", "");
                    newFile.Append(temp + "\r\n");
                    continue;
                }

                if (line.Contains("page:WordSection1;"))
                {
                    temp = line.Replace("page:WordSection1;", "page:WordSection1; -webkit-box-shadow: 3px 3px 5px 0px rgba(0,0,0,0.75); -moz-box-shadow: 3px 3px 5px 0px rgba(0,0,0,0.75); box-shadow: 3px 3px 5px 0px rgba(0,0,0,0.75); width: 80%;  margin: auto; padding: 3em; background-color: #FFF;");
                    newFile.Append(temp + "\r\n");
                    continue;
                }

                if (line.Contains("<body l"))
                {
                    temp = line.Replace("<body l", "<body style=\"background-color: #EBE8E8;\" l");
                    newFile.Append(temp + "\r\n");
                    continue;
                }


                newFile.Append(line + "\r\n");
            }
            File.WriteAllText(archivo, newFile.ToString(), Encoding.UTF8);
        }

    }
}
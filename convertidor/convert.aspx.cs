using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Word;
using System.Text;
using System.Web.Script.Serialization;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace jats
{
    public partial class convert : BasePage
    {
        
        Dictionary<string, string> mensaje = new Dictionary<string, string>();
        Dictionary<string, string> parametrosXML = new Dictionary<string, string>();
        private String urlServer;
        private String rutaTemporal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["revistaId"]) || String.IsNullOrEmpty(Request.QueryString["revistaTitulo"])
                    || String.IsNullOrEmpty(Request.QueryString["printIssn"]) || String.IsNullOrEmpty(Request.QueryString["abbreviation"])
                    || String.IsNullOrEmpty(Request.QueryString["publisher"]) || String.IsNullOrEmpty(Request.QueryString["date"])
                    || String.IsNullOrEmpty(Request.QueryString["volume"]) || String.IsNullOrEmpty(Request.QueryString["issue"])
                    || String.IsNullOrEmpty(Request.QueryString["year"]) || String.IsNullOrEmpty(Request.QueryString["page"]))
                {
                    divUpload.Visible = false;
                    divErrorReq.Visible = true;
                    
                }
                else
                {
                    Random random = new Random();
                    int idConversion = random.Next(90000);
                    Session["idConversion"] = idConversion;

                    urlServer = ConfigurationManager.AppSettings["site.url"];
                    rutaTemporal = @ConfigurationManager.AppSettings["site.temporal"];

                    statusLabel.Text = "";
                    //parametros para generar XML
                    Session["revistaId"] = Request.QueryString["revistaId"];
                    Session["revistaTitulo"] = Request.QueryString["revistaTitulo"];
                    Session["onlineIssn"] = Request.QueryString["onlineIssn"];
                    Session["printIssn"] = Request.QueryString["printIssn"];
                    Session["abbreviation"] = Request.QueryString["abbreviation"];
                    Session["publisher"] = Request.QueryString["publisher"];
                    Session["date"] = Request.QueryString["date"];
                    Session["volume"] = Request.QueryString["volume"];
                    Session["issue"] = Request.QueryString["issue"];
                    Session["year"] = Request.QueryString["year"];
                    Session["page"] = Request.QueryString["page"];
                }                                
            }
            else
            {
                if (Session["iniciaConversion"] != null && Session["iniciaConversion"].ToString() == "1")
                {
                    Session["iniciaConversion"] = null;
                    int idConversion = (int)Session["idConversion"];
                    String nombreAleatorio = Session["nombreAleatorio"] + "";
                    String rutaDocumento = ConfigurationManager.AppSettings["archos.ruta"] + "/" + idConversion + "/";
                    convierteArchivoA(rutaDocumento, nombreAleatorio, "xml");
                    
                }
                else if (Session["nombreArchivoZip"] != null && Session["ruta"] != null && Session["imageUpdate"] == null)
                {
                    divExito.Visible = true;
                    divUpload.Visible = false;
                    divCargando.Visible = false;
                    divErrorReq.Visible = false;
                    Session["imageUpdate"] = "1";
                    forzarPostBack();
                }
                else if (Session["nombreArchivoZip"] != null && Session["ruta"] != null &&
                         Session["imageUpdate"] != null && Session["imageUpdate"] .ToString() == "1")
                {
                    String nombreArchivoZip = Session["nombreArchivoZip"] + "";
                    String ruta             = Session["ruta"] + "";
                    Session["nombreArchivoZip"] = null;
                    Session["ruta"]             = null;
                    Session["imageUpdate"] = null;
                    
                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                    response.Clear();
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition", "attachment; filename=" + nombreArchivoZip);
                    response.WriteFile(ruta + nombreArchivoZip);
                    response.End();

                    Session.Clear();
                    Session.RemoveAll();
                }
                
            }
            


        }

        protected void subirBoton_Click(object sender, EventArgs e)
        {
            if (subirArchivo.HasFile)
            {
                try
                {
                    int idConversion = (int)Session["idConversion"];
                    String filename = Path.GetFileName(subirArchivo.FileName);
                    String rutaAlmacenar = ConfigurationManager.AppSettings["archos.ruta"] + "/" + idConversion + "/";
                    String nombreAleatorio = "";

                    if (!String.IsNullOrEmpty((String)Session["revistaTitulo"]))    nombreAleatorio +=       Session["revistaTitulo"];
                    if (!String.IsNullOrEmpty((String)Session["volume"]))           nombreAleatorio += "-" + Session["volume"];
                    if (!String.IsNullOrEmpty((String)Session["issue"]))            nombreAleatorio += "-" + Session["issue"];
                    if (!String.IsNullOrEmpty((String)Session["volume"]))           nombreAleatorio += "-" + Session["volume"];
                    if (!String.IsNullOrEmpty((String)Session["page"]))             nombreAleatorio += "-" + Session["page"];


                    nombreAleatorio += ".docx";


                    Session["nombreAleatorio"] = nombreAleatorio;

                    crearCarpea(rutaAlmacenar);
                    subirArchivo.SaveAs(rutaAlmacenar + nombreAleatorio);
                    statusLabel.Text = HttpContext.GetGlobalResourceObject("Languaje", "uploadedCorrectly")+""; 
                    statusLabel.CssClass = "label label-success";

                    labelIniciar.InnerHtml = HttpContext.GetGlobalResourceObject("Languaje", "startFileConversion")+"";
                    inicarConversion.Enabled = true;

                }
                catch (Exception ex)
                {
                    statusLabel.Text = HttpContext.GetGlobalResourceObject("Languaje", "errorEncountered")+"" + ex.Message;
                    statusLabel.CssClass = "label label-danger";
                }
            }
        }

        private void crearCarpea(String ruta)
        {
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
        }


        protected void inicarConversion_Click(object sender, EventArgs e)
        {
            int idConversion = (int)Session["idConversion"];
            String nombreAleatorio = Session["nombreAleatorio"]+"";

            if ( idConversion > 0)
            {
                String rutaDocumento = ConfigurationManager.AppSettings["archos.ruta"] + "/" + idConversion+"/";

                divExito.Visible = false;
                divUpload.Visible = false;
                divCargando.Visible = true;
                divErrorReq.Visible = false;

                Session["iniciaConversion"] = 1;
                forzarPostBack();
            }
            
        }

        public void forzarPostBack()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("<script language='JavaScript' type='text/javascript'>\n");
            sbScript.Append("<!--\n");
            sbScript.Append(this.GetPostBackEventReference(this, "PBArg") + ";\n");
            sbScript.Append("// -->\n");
            sbScript.Append("</script>\n");
            this.RegisterStartupScript("AutoPostBackScript", sbScript.ToString());
        }

        
        protected void convierteArchivoA(string ruta, string nombreArchivo, string formato)
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
                foreach (String llave in Session.Keys)
                    parametrosXML.Add(llave, Session[llave]+"");

                ProcesaDocumento pd = new ProcesaDocumento2016(ruta + nombreArchivo, ruta, parametrosXML);
                pd.procesa();
                res.Add("mensaje", pd.Mensaje);

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
                    divExito.Visible = true; 
                    divUpload.Visible = false;
                    divCargando.Visible = false;
                    divErrorReq.Visible = false;
                    string nombreArchivoZip = Path.GetFileNameWithoutExtension(nombreArchivo) + ".zip";
                    Session["nombreArchivoZip"] = nombreArchivoZip;
                    Session["ruta"] = ruta;
                    
                }
                else
                {
                    divErrorReq.Visible = false;
                    divUpload.Visible = false;
                    divCargando.Visible = false;
                    divErrorConversion.Visible = true;
                    LabelError.Text = HttpContext.GetGlobalResourceObject("Languaje", "tryLater") + "";
                    res.Add("mensaje", HttpContext.GetGlobalResourceObject("Languaje", "tryLater") + "");
                }
            }
            else //no se genero archivo nuevo
            {
                divErrorReq.Visible = false;
                divUpload.Visible = false;
                divErrorConversion.Visible = true;
                divCargando.Visible = false;
                dynamic stuff = JObject.Parse(generaMensaje(res));
                LabelError.Text = stuff.mensaje;
            }

            forzarPostBack();
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

            }
            catch (Exception er) { }



            return comprimido;
        }
        protected void descomprime(string archivo, string ruta)
        {
            using (ZipFile zip1 = ZipFile.Read(archivo))
            {
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(ruta, ExtractExistingFileAction.OverwriteSilently);
                }
            }
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
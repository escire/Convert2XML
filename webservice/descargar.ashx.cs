using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace convertidor
{
    /// <summary>
    /// Descripción breve de descargar
    /// </summary>
    public class descargar : IHttpHandler
    {
        Dictionary<string, string> mensaje = new Dictionary<string, string>();

        private String rutaTemporal = @ConfigurationManager.AppSettings["site.temporal"];

        public void ProcessRequest(HttpContext context)
        {
            string archivo = context.Request.QueryString["archivo"];
            string cliente = context.Request.QueryString["cliente"];
            string ruta = rutaTemporal + cliente + "\\" + Path.GetFileNameWithoutExtension(archivo) + "\\";

            if (string.IsNullOrEmpty(archivo))
            {
                context.Response.ContentType = "application/json";
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Espeficique el nombre del archivo");
                context.Response.Write(generaMensaje(mensaje));
                return;
            }

            if (string.IsNullOrEmpty(cliente))
            {
                context.Response.ContentType = "application/json";
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "Espeficique el nombre del cliente");
                context.Response.Write(generaMensaje(mensaje));
                return;
            }

            if (File.Exists(ruta + archivo))
            {

                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + archivo);
                context.Response.WriteFile(ruta + archivo);
                context.Response.End();
            }
            else
            {
                context.Response.ContentType = "application/json";
                mensaje.Add("estatus", "error");
                mensaje.Add("mensaje", "No existe el archivo");
                context.Response.Write(generaMensaje(mensaje));
                return;
 
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
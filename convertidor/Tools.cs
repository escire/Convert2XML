using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Text;
using Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic.Devices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO.Compression;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net;
using System.Globalization;
using System.Diagnostics;

namespace jats
{
    public static class Tools
    {

        public static Boolean contine(String cadena, String buscar, Boolean ignorecase)
        {
            if (String.IsNullOrEmpty(cadena) || String.IsNullOrEmpty(buscar))
                return false;

            int index = cadena.IndexOf(buscar, ignorecase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            if (index >= 0)
                return true;
            
            return false;
        }

        public static Boolean negrita(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            if (Math.Abs(parrafo.Range.Font.Bold) == 0)
                return false;
            return true;
        }

        public static Boolean cursiva(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            if (Math.Abs(parrafo.Range.Font.Italic) == 0)
                return false;
            return true;
        }

        public static int tamano(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            return (int)parrafo.Range.Font.Size;
        }

        public static Boolean tamano(Microsoft.Office.Interop.Word.Paragraph parrafo, int tam)
        {
            if (tamano(parrafo) == tam)
                return true;
            return false;
        }

        public static Boolean tipoletra(Microsoft.Office.Interop.Word.Paragraph parrafo, String tipoletra)
        {
            if (parrafo.Range.Font.NameBi.Equals(tipoletra))
                return true;
            return false;
        }

        public static String tipoletra(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            return parrafo.Range.Font.NameBi;

        }

        public static Boolean alineacion(Microsoft.Office.Interop.Word.Paragraph parrafo, String alineacion)
        {
            if (parrafo.Alignment.ToString().Equals(alineacion))
                return true;
            return false;
        }

        public static String alineacion(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            return parrafo.Alignment.ToString();

        }


        public static String texto(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            String st = parrafo.Range.Text.ToString().Trim();
            if (st.Contains("\v"))
            {
                st = st.Replace("\v", " ").Replace(@"&#11;", " ").Replace(@"&#xb;", " ").Replace(@"\u000B", " ");
                st = st + "";
            }
            st = st.Replace(@"<br>", " ").Replace(@"<br/>", " ").Replace(@"<br>", " ");
            return st;
        }

        public static Boolean vacio(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            if (texto(parrafo).Equals(""))
                return true;
            return texto(parrafo).Equals(Environment.NewLine);
        }

        public static String conviertefecha(String f)
        {
            String fecha = "";
            String[] formatStrings = { "M/y", "M/d/y", "M-d-y", "d/M/y", "d-M-y", "mm/dd/yyyy", "mm-dd-yyyy", "dd/mm/yyyy", "dd-mm-yyyy" };


            return fecha;
        }

        public static String extreaAnio(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
                return null;

            if (cadena.Any(char.IsDigit)) // ver si la cadena contiene digitos
            {
                foreach (String numero in Regex.Split(cadena, @"\D+"))
                {
                    if (numero.Length == 4)
                    {
                        return numero;
                    }
                }
            }
            return null;

        }

        
        public static String creaLinksExternos(Articulo articulo, String body)
        {
            String bodyLink = body;
            List<String> histReempla = new List<String>();

            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            
            foreach (Match m in linkParser.Matches(bodyLink))
            {
                String tipo = "";
                String ulr = m.Value;

                if (ulr.Contains("<")) ulr = ulr.Substring(0, ulr.IndexOf("<"));

                if (ulr.EndsWith(".")) ulr = ulr.Substring(0, ulr.Length - 1);

                if (!histReempla.Contains(ulr))
                {
                    try
                    {
                        var request = System.Net.WebRequest.Create(ulr);
                        request.Method = "HEAD";
                        var response = request.GetResponse();
                        tipo = response.ContentType;
                    }
                    catch (Exception e4) { }


                    foreach (string extension in Tools.getPropiedad("media.video").Split('|'))
                        if (m.Value.Trim().EndsWith("." + extension) || tipo.EndsWith(extension))
                        {
                            bodyLink = bodyLink.Replace(ulr, "<media mimetype=\"video\"  mime-subtype=\"" + extension + "\" xlink:href=\"" + ulr + "\"/>");
                            histReempla.Add(ulr);
                            goto nextLink;
                        }

                    foreach (string extension in Tools.getPropiedad("media.audio").Split('|'))
                        if (m.Value.Trim().EndsWith("." + extension) || tipo.EndsWith(extension))
                        {
                            bodyLink = bodyLink.Replace(ulr, "<media mimetype=\"audio\"  mime-subtype=\"" + extension + "\" xlink:href=\"" + ulr + "\"/>");
                            histReempla.Add(ulr);
                            goto nextLink;
                        }

                    foreach (string extension in Tools.getPropiedad("media.animaciones").Split('|'))
                        if (m.Value.Trim().EndsWith("." + extension) || tipo.EndsWith(extension))
                        {
                            bodyLink = bodyLink.Replace(ulr, "<media mimetype=\"application\"  mime-subtype=\"" + extension + "\" xlink:href=\"" + ulr + "\"/>");
                            histReempla.Add(ulr);
                            goto nextLink;
                        }

                    foreach (string extension in Tools.getPropiedad("media.application").Split('|'))
                        if (m.Value.Trim().EndsWith("." + extension) || tipo.EndsWith(extension))
                        {
                            bodyLink = bodyLink.Replace(ulr, "<supplementary-material mimetype=\"application\"  mime-subtype=\"" + extension + "\" xlink:href=\"" + ulr + "\"/>");
                            histReempla.Add(ulr);
                            goto nextLink;
                        }

                    bodyLink = bodyLink.Replace(ulr, "<ext-link ext-link-type=\"uri\" xlink:href=\"" + ulr + "\">" + ulr + "</ext-link>");
                    histReempla.Add(ulr);
                    nextLink: ;
                }

                
            }
                

            return bodyLink;
        }

        public static String enlazaReferencias(Articulo articulo, String body)
        {
            String bodyLink = body;
            int i = 1;
            foreach (LinkReferencia linkRef in articulo.linkReferencias)
            {
                if (!String.IsNullOrEmpty(linkRef.Anio) && !String.IsNullOrEmpty(linkRef.Apellido))
                {
                    /*
                    Regex regex = new Regex(@linkRef.Apellido+"(.+?)[^\(]*"+linkRef.Anio+"[^\)]*"); // Santos(.+?)[^\(]*2002[^\)]*
                    Match match = regex.Match(parrafo);
                    if (match.Success)
	                {
	                    Console.WriteLine(match.Value);
	                }
                     * */
                    string pattern = linkRef.Apellido + @"(.+?)[^\(]*" + linkRef.Anio + @"[^\)]*";
                    foreach (Match m in Regex.Matches(bodyLink, pattern, RegexOptions.Singleline ))
                    {
                        if (!m.Value.Contains("\"") && !m.Value.Contains("<") && !m.Value.Contains(">") )
                            bodyLink = bodyLink.Replace(m.Value, "<xref ref-type=\"bibr\" rid=\"B" + i + "\">" + m.Value + "</xref>");
                    }

                }
                i += 1;
            }
            return bodyLink;
        }

        public static String extraeFecha(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
                return null;

            Regex rgx = new Regex(@"\d{2}-\d{2}-\d{4}");
            Match mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();
            rgx = new Regex(@"\d{4}-\d{2}-\d{2}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();

            rgx = new Regex(@"\d{2}/\d{2}/\d{4}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();
            rgx = new Regex(@"\d{4}/\d{2}/\d{2}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();

            rgx = new Regex(@"\d{2}.\d{2}.\d{4}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString().Replace(".","-");
            rgx = new Regex(@"\d{4}.\d{2}.\d{2}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();

            return null;
        }

        public static string extraeNumero(string original)
        {
            return new string(original.Where(c => Char.IsDigit(c)).ToArray());
        }

        public static Boolean isTituloTraduccion(Paragraph i)
        {
            return (negrita(i) && !cursiva(i) && subrayado(i) && alineacion(i, "wdAlignParagraphCenter")
                                && tipoletra(i, "Times New Roman")
                                && !vacio(i) && tamano(i, 14)
                   );
        }




        public static Boolean subrayado(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            if (parrafo.Range.Font.Underline == null)
                return false;
            if (parrafo.Range.Font.Underline.ToString().Equals("wdUnderlineNone"))
                return false;

            return true;
        }

        public static string getEmail(string cadena)
        {
            if (String.IsNullOrEmpty(cadena))
                return null;

            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            MatchCollection emailMatches = emailRegex.Matches(cadena);
            foreach (Match emailMatch in emailMatches)
            {
                return emailMatch.Value;
            }
            return null;
        }

        public static String downloadJson(string url)
        {
            using (var w = new System.Net.WebClient())
            {
                var json_data = string.Empty;
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                return json_data;
            }
        }

        public static string getPropiedad(string propiedad)
        {
            return System.Configuration.ConfigurationManager.AppSettings[propiedad];
        }

        public static Boolean verificaContenido(Microsoft.Office.Interop.Word.Paragraph parrafo, String propiedad)
        {

            String valoresPropiedad = getPropiedad(propiedad);
            if (!String.IsNullOrEmpty(valoresPropiedad))
            {
                string[] words = valoresPropiedad.Split('|');
                foreach (string word in words)
                {
                    if (texto(parrafo).StartsWith(word, true, new CultureInfo("es-MX")))
                        return true;
                    //if (texto(parrafo).IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                        //return true;
                }
            }
            return false;
        }

        public static Boolean verificaContenido(String cadena, String propiedad)
        {

            String valoresPropiedad = getPropiedad(propiedad);
            if (!String.IsNullOrEmpty(valoresPropiedad))
            {
                string[] words = valoresPropiedad.Split('|');
                foreach (string word in words)
                {
                    if (cadena.StartsWith(word, true, new CultureInfo("es-MX")))
                        return true;
                }
            }
            return false;
        }

        public static String extraeMathML(Microsoft.Office.Interop.Word.Range rango, Articulo articulo, Boolean inline){
            String texto = rango.WordOpenXML;

            using (XmlReader reader2 = XmlReader.Create(new StringReader(texto)))
            {
                reader2.MoveToContent();
                reader2.ReadToFollowing("m:oMath");
                String formula = reader2.ReadInnerXml();

                if (!String.IsNullOrEmpty(formula))
                {
                    formula = "<!--[if gte msEquation 12]><m:oMath>" + formula + "</m:oMath><![endif]--><![if !msEquation]>";
                    String archivo = Path.GetRandomFileName() + ".xml";
                    String fileFormula = Tools.getPropiedad("site.temporal.formulas") + @"\" + archivo;
                    File.WriteAllText(fileFormula, formula, Encoding.UTF8);

                        
                    String fileConversion = toMathMl(fileFormula);

                    //nuevaFormula = Regex.Replace(nuevaFormula, "&amp;", "&");
                    String nuevaFormula = HttpUtility.HtmlDecode(File.ReadAllText(Tools.getPropiedad("site.temporal.formulas") + @"\" + fileConversion));
                    nuevaFormula = nuevaFormula.Replace("&amp;#65279;", "").Replace("&amp;", "&");
                    nuevaFormula = nuevaFormula.Replace("<?xml version=\"1.0\" encoding=\"US-ASCII\"?>", "");
                    nuevaFormula = nuevaFormula.Replace("<?xml-stylesheet type=\"text/xsl\" href=\"pmathml.xsl\"?>", "");
                    nuevaFormula = nuevaFormula.Replace("<html xmlns=\"http://www.w3.org/1999/xhtml\">", "");
                    nuevaFormula = nuevaFormula.Replace("<body>", "").Replace("</body>", "").Replace("</html>", "").Replace("&#239;&#187;&#191;", "");
                    nuevaFormula = nuevaFormula.Replace("xmlns:mml=\"http://www.w3.org/1998/Math/MathML\"", "").Replace("<mml:mi>&#160;</mml:mi>", "");

                    nuevaFormula = nuevaFormula.Replace("<mml:math ", "<mml:math id=\"e" + articulo.numFormulas + 1 + "\"");

                    if (inline)
                        nuevaFormula = "<inline-formula>" + nuevaFormula + "</inline-formula>";
                    else
                        nuevaFormula = "<disp-formula>" + nuevaFormula + "</disp-formula>";

                    return nuevaFormula;
                }

            }
            return "";
        }

        public static String toMathMl(String urlFormula)
        {

            string xsl = Path.Combine(HttpContext.Current.Server.MapPath("~"), @"xsl\xhtml-mathml.xsl");
            string saxon = Path.Combine(HttpContext.Current.Server.MapPath("~"), @"xsl\saxon8.jar");
            string tagsoup = Path.Combine(HttpContext.Current.Server.MapPath("~"), @"xsl\tagsoup-1.2.1.jar");

            string xmlOriginal = urlFormula;
            string xmlLimpio = getPropiedad("site.temporal.formulas") + @"\" + Path.GetFileNameWithoutExtension(xmlOriginal) + "_Limpio.xml";
            string xmlMathMl = getPropiedad("site.temporal.formulas") + @"\" + Path.GetFileNameWithoutExtension(xmlOriginal) + "_MathMl.xml";

            //String argumentos = @"-jar  " + tagsoup.Replace(@"C:\", @"\") + " --lexical --output-encoding=UTF-8 " + xmlOriginal.Replace(@"C:\", @"\") + " > " + xmlLimpio.Replace(@"C:\", @"\");
            String argumentos = @"-jar  " + tagsoup.Replace(@"C:\", @"\") + " --lexical --output-encoding=UTF-8 " + xmlOriginal.Replace(@"C:\", @"\"); ;

            argumentos = String.Format(argumentos);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            String exito = "";
            int lineCount = 0;

            p.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    lineCount++;
                    exito += ("\n[" + lineCount + "]: " + e.Data);
                }
            });
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = argumentos;
            p.Start();
            exito = p.StandardOutput.ReadToEnd();
            String error = p.StandardError.ReadToEnd();
            p.WaitForExit();

            if (!String.IsNullOrEmpty(exito))
            {
                exito = exito.Replace("´╗┐", "").Replace("Ôêé", "∂").Replace("QUOTE", " ").Replace("┬▒", "±");
                File.WriteAllText(@xmlLimpio, exito);
            }
            else
                return null;

            if (!File.Exists(xmlLimpio))
            {
                File.Delete(xmlOriginal);
                return null;
            }


            //java -jar saxon8.jar -o file.xml temp.xml xhtml-mathml.xsl
            argumentos = @"-jar " + saxon.Replace(@"C:\", @"\") + " -o " + xmlMathMl.Replace(@"C:\", @"\") + " " + xmlLimpio.Replace(@"C:\", @"\") + " " + xsl;
            p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = argumentos;
            p.Start();

            // To avoid deadlocks, always read the output stream first and then wait.
            exito = p.StandardOutput.ReadToEnd();
            error = p.StandardError.ReadToEnd();
            p.WaitForExit();

            if (File.Exists(xmlLimpio))
                File.Delete(xmlLimpio);

            if (File.Exists(xmlOriginal))
                File.Delete(xmlOriginal);

            if (File.Exists(xmlMathMl))
                return Path.GetFileName(xmlMathMl);
            return null;
        }


        public static void consultaWayta(String nombre, Articulo articulo, String id, String afiliacion, String email)
        {
            String wayta = System.Configuration.ConfigurationManager.AppSettings["wayta.url.institution"];
            String json = Tools.downloadJson(wayta + nombre);
            if (!String.IsNullOrEmpty(json))
            {
                dynamic array = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                String headJason = array.head.match;

                if ((headJason).Equals("exact"))
                {
                    dynamic instituciones = array.choices;
                    foreach (var item in instituciones)
                    {
                        String value = item["value"];
                        String city = item["city"];
                        String country = item["country"];
                        String state = item["state"];
                        String score = item["score"];
                        String iso3166 = item["iso3166"];
                        articulo.addAfiliaciones(id, afiliacion, nombre, email, country, iso3166, state, city);
                        break;

                    }
                }
                else if ((headJason).Equals("multiple"))
                {
                    dynamic instituciones = array.choices;
                    foreach (var item in instituciones)
                    {
                        String value = item["value"];
                        if (nombre.Trim().ToLower().Contains(value.Trim().ToLower()))
                        {
                            String city = item["city"];
                            String country = item["country"];
                            String state = item["state"];
                            String score = item["score"];
                            String iso3166 = item["iso3166"];
                            articulo.addAfiliaciones(id, afiliacion, nombre, email, country, iso3166, state, city);
                            break;
                        }

                    }
                    articulo.addAfiliaciones(id, afiliacion, nombre, email, null, null, null, null);
                }else
                    articulo.addAfiliaciones(id, afiliacion);

            }
        }

    }
}
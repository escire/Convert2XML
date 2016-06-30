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

namespace convertidor
{
    public class ProcesaDocumento
    {
        private String rutaDocumento;
        private object rutaContenido;
        private String mensaje;
        private String log;
        private Articulo articulo = new Articulo();
        private int parrafoinicial;
        private int parrafoFinal;
        private Dictionary<string, string> parametrosXML;
        private Boolean resumen = false;

        private Boolean anteriorDescipcionImagen = false;
        private String descipcionImagen = null;
        private Dictionary<string, string> reemplazos = new Dictionary<string,string>();


        private Boolean anteriorDescipcionTable = false;
        private String descipcionTable = null;
        private Boolean eslista = false;
        private Boolean cerrarlista = false;
        private Boolean abrirlista = false;

        public String Mensaje
        {
            get
            {
                return mensaje;
            }
        }

        public ProcesaDocumento(String ruta, String rc, Dictionary<string, string> pxml)
        {
            rutaDocumento = ruta;
            rutaContenido = rc;
            mensaje = ""; 
            log = "";
            parametrosXML = pxml;
        }

        public String Log
        {
            get
            {
                return log;
            }
        }

        public Boolean procesa()
        {

            if (System.IO.File.Exists(@rutaDocumento))
            {
                object miss = Type.Missing;
                object archivo = @rutaDocumento;
                object readOnly = true;
                object isVisible = false;

                Microsoft.Office.Interop.Word.Application word = null;
                Document doc = null;
                try
                {

                    List<Microsoft.Office.Interop.Word.Range> TablesRanges = new List<Microsoft.Office.Interop.Word.Range>();

                    word = new Microsoft.Office.Interop.Word.Application();
                    doc = word.Documents.Open(ref archivo, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);

                    for (int iCounter = 1; iCounter <= doc.Tables.Count; iCounter++)
                    {
                        Microsoft.Office.Interop.Word.Range TRange = doc.Tables[iCounter].Range;
                        TablesRanges.Add(TRange);
                    }


                    var numberOfPages = doc.ComputeStatistics(WdStatistic.wdStatisticPages, false);
                    articulo.numPaginas = numberOfPages;
                    articulo.numImagenes = 0;
                    articulo.numFormulas = 0;
                    articulo.numTablas = doc.Tables.Count;
                    int j = 1; 
                    //buscar DOI
                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && tipoletra(doc.Paragraphs[i], "Times New Roman") && alineacion(doc.Paragraphs[i], "wdAlignParagraphRight")
                                && (texto(doc.Paragraphs[i]).Contains("doi") || texto(doc.Paragraphs[i]).Contains("DOI")) && tamano(doc.Paragraphs[i], 12))
                        {
                            articulo.Doi = texto(doc.Paragraphs[i]);
                            log += "<br/>************DOI encontrado: " + articulo.Doi + "<br/>";
                            break;
                        }

                    }
                    j += 1; //log += "<br/>j:" + j;
                    if (articulo.Doi == null)
                    {
                        j = 1;
                        articulo.Doi = null;
                        log += "<br/>************DOI encontrado: No se encontró DOI<br/>";
                    }
                    //buscar Seccion
                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        if (negrita(doc.Paragraphs[i]) && cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphRight")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && !texto(doc.Paragraphs[i]).Equals("") && tamano(doc.Paragraphs[i], 12))
                        {
                            articulo.Seccion = texto(doc.Paragraphs[i]);
                            log += "<br/>*****Sección encontrada: " + articulo.Seccion + "<br/>";
                            break;
                        }


                    }
                    j += 1; //log += "<br/>j:" + j;
                    if (articulo.Seccion == null)
                    {
                        mensaje = "Error en formato de documento: No se encuetra Sección";
                        log += "<br/>" + mensaje;
                        doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                        doc = null;
                        word.Quit();
                        return false;
                    }

                    //buscar titulo
                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        if (negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                                && tipoletra(doc.Paragraphs[i], "Times New Roman")
                                && !vacio(doc.Paragraphs[i]) && tamano(doc.Paragraphs[i], 14))
                        {
                            log += "<br/>*****Título encontrado: " + doc.Paragraphs[i].Range.Text.ToString() + "<br/>";
                            articulo.Titulo = doc.Paragraphs[i].Range.Text.ToString();
                            break;
                        }
                    }
                    j += 1; //log += "<br/>j:" + j;
                    if (articulo.Titulo == null)
                    {
                        mensaje = "Error en formato de documento: No se encuetra Titulo";
                        log += "<br/>" + mensaje;
                        doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                        doc = null;
                        word.Quit();
                        return false;
                    }


                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        //buscar title
                        if (negrita(doc.Paragraphs[i]) && cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && !vacio(doc.Paragraphs[i]) && tamano(doc.Paragraphs[i], 14))
                        {
                            articulo.Title = texto(doc.Paragraphs[i]);
                            log += "*****Título traducido: " + articulo.Title;
                        }

                        //buscar autores
                        else if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) &&
                              alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft") && tipoletra(doc.Paragraphs[i], "Arial")
                                && !vacio(doc.Paragraphs[i]) && tamano(doc.Paragraphs[i], 12))
                        {
                            if (texto(doc.Paragraphs[i]).Contains("*") )
                            {
                                articulo.AutorcorresNombre = texto(doc.Paragraphs[i]).Replace("*","");
                                articulo.AutorcorresNombre = Regex.Replace(articulo.AutorcorresNombre, @"[\d-]", " ");
                            }
                            String nombre = "", apellido = "";

                            if (texto(doc.Paragraphs[i]).Split(new char[0]).Length == 1) // el autor no tiene apellidos
                            {
                                nombre = texto(doc.Paragraphs[i]);
                                apellido = "NO SE ENCONTRO APELLIDO";
                            }
                            else if (texto(doc.Paragraphs[i]).Split(new char[0]).Length >= 2)
                            {
                                if (!texto(doc.Paragraphs[i]).Contains("-"))
                                {
                                    nombre = texto(doc.Paragraphs[i]).Split(new char[0])[0];
                                    apellido = texto(doc.Paragraphs[i]).Split(new char[0])[1];
                                }
                                else
                                {
                                    foreach (String pal in texto(doc.Paragraphs[i]).Split(new char[0])) // dividir nombre en espacios
                                    {
                                        if (pal.Contains("-"))
                                            apellido += " " + pal;
                                        else nombre += " " + pal;
                                    }
                                }
                            }
                            
                            String afiliacion = System.Text.RegularExpressions.Regex.Match(apellido, @"\d+").Value;

                            if ( ! String.IsNullOrEmpty( afiliacion ))
                                apellido = apellido.Trim().Replace(afiliacion, "");

                            articulo.addAutores(nombre.Trim(), apellido.Trim(), afiliacion);
                            log += "*****Autor: " + nombre + ":::" + apellido.Replace("-"," ") + ":::" + afiliacion + "<br/>";
                        }

                        //buscar afiliaciones
                        else if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Arial") && !vacio(doc.Paragraphs[i]) && tamano(doc.Paragraphs[i], 10))
                        {
                            String afiliacion = texto(doc.Paragraphs[i]);
                            String id = new String(afiliacion.TakeWhile(Char.IsDigit).ToArray());
                            if (id != null && !id.Trim().Equals(""))
                            {
                                String nombre = "";
                                String ciudad = "";
                                if (afiliacion.Contains(",")){
                                    nombre = afiliacion.Substring(afiliacion.IndexOf(id) + id.Length, afiliacion.IndexOf(",") - 1);
                                    ciudad = afiliacion.Substring(afiliacion.IndexOf(",") + 1);
                                }
                                    
                                else
                                    nombre = afiliacion;

                                articulo.addAfiliaciones(id, nombre, ciudad, afiliacion);
                                log += "*****Afiliación: " + id + ":::" + nombre + ":::" + ciudad + "<br/>";
                                
                            }

                        }//buscar autor de correspondencia
                        else if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && !vacio(doc.Paragraphs[i]) && tamano(doc.Paragraphs[i], 12))
                        {
                            String autorcorres = texto(doc.Paragraphs[i]);
                            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
                            MatchCollection emailMatches = emailRegex.Matches(autorcorres);
                            foreach (Match emailMatch in emailMatches)
                            {
                                articulo.Autorcorres = emailMatch.Value;
                                articulo.AutorcorresNombre = autorcorres;
                                log += "*****Autorcorres: " + articulo.Autorcorres + "<br/>";
                            }

                        }//buscar Resumen:
                        else if (negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && !vacio(doc.Paragraphs[i]) && tamano(doc.Paragraphs[i], 12)
                            && (    texto(doc.Paragraphs[i]).Equals("Resumen:", StringComparison.InvariantCultureIgnoreCase)
                                    || texto(doc.Paragraphs[i]).Equals("Resumen", StringComparison.InvariantCultureIgnoreCase)
                                )
                            
                            )
                        {
                            
                            int x = i + 1;
                            //articulo.Resumen = "<p><b>" + texto(doc.Paragraphs[i]) + "</b></p>";
                            articulo.Resumen = "";
                            for (; x <= doc.Paragraphs.Count ; x++)
                            {   //buscar parrafo de resumen
                                if ( !cursiva(doc.Paragraphs[x]) && alineacion(doc.Paragraphs[x], "wdAlignParagraphJustify")
                                    && tipoletra(doc.Paragraphs[x], "Times New Roman") && !vacio(doc.Paragraphs[x]) && tamano(doc.Paragraphs[x], 12))
                                {
                                    String temp = formateaTexto(doc.Paragraphs[x], articulo, "", x+1<=doc.Paragraphs.Count? doc.Paragraphs[x+1] : null);
                                    if (!String.IsNullOrEmpty(temp))
                                    {
                                        if (temp.Contains("<bold>"))
                                        {
                                            articulo.Resumen += "<sec>";
                                            articulo.Resumen += formateaTexto(doc.Paragraphs[x], articulo, "", (x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null))
                                                .Replace("<bold>", "<title>").Replace("</bold>", "</title><p>");
                                            articulo.Resumen += "</p></sec>";
                                        }
                                        else
                                        {
                                            articulo.Resumen += "<p>" + temp + "</p>";
                                        }
                                    }
                                }
                                //aca empiezan las palabras clave
                                else if (!cursiva(doc.Paragraphs[x]) && alineacion(doc.Paragraphs[x], "wdAlignParagraphLeft")
                                    && tipoletra(doc.Paragraphs[x], "Times New Roman") && !vacio(doc.Paragraphs[x]) && tamano(doc.Paragraphs[x], 12)
                                    && (
                                           texto(doc.Paragraphs[x]).StartsWith("Palabras clave:", true, null)
                                        || texto(doc.Paragraphs[x]).StartsWith("Palabras claves:", true, null)
                                        || texto(doc.Paragraphs[x]).StartsWith("Palabra clave:", true, null)    
                                    ))
                                {
                                    log += "<br/>*****" + articulo.Resumen;
                                    x -= 1;
                                    i = x; j = x;
                                    break;
                                }
                            }

                        }  //buscar palabras clave
                        else if (!cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") &&
                            (texto(doc.Paragraphs[i]).StartsWith("Palabras clave:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Palabras claves:", true, null)) 
                            && tamano(doc.Paragraphs[i], 12))
                        {
                            String pc = texto(doc.Paragraphs[i]);
                            pc = pc.Substring( pc.IndexOf(":")+1 );
                            foreach (String pal in pc.Split(','))
                            {
                                articulo.addPalabrasclave(pal.Trim());
                            }
                            log += "<br/>******Total Palabras clave: " + articulo.Palabrasclave.Count() + "<br/>";
                            j = i;
                        }
                        else if (negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") &&
                            (
                                texto(doc.Paragraphs[i]).StartsWith("Abstract:", true, null)
                                || texto(doc.Paragraphs[i]).StartsWith("Abstract", true, null)
                                || texto(doc.Paragraphs[i]).StartsWith("Resumo:", true, null)
                                || texto(doc.Paragraphs[i]).StartsWith("Resumo", true, null)
                            )
                            && tamano(doc.Paragraphs[i], 12))
                        {
                            int x = i + 1;
                            //articulo.Abstractt = "<p><b>" + texto(doc.Paragraphs[i]) + "</b></p>";
                            articulo.Abstractt = "";
                            for (; x <= doc.Paragraphs.Count; x++)
                            {   //buscar parrafo de abstract 
                                 if ( !cursiva(doc.Paragraphs[x]) && alineacion(doc.Paragraphs[x], "wdAlignParagraphJustify")
                                    && tipoletra(doc.Paragraphs[x], "Times New Roman") && !vacio(doc.Paragraphs[x]) && tamano(doc.Paragraphs[x], 12)
                                    
                                    && !(texto(doc.Paragraphs[i]).StartsWith("Key words:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Keywords:", true, null)
                                        || texto(doc.Paragraphs[i]).StartsWith("Palavras chave:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Palavras-chave:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Palavraschave:", true, null))
                                    )
                                {
                                    String temp = formateaTexto(doc.Paragraphs[x], articulo, "", (x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null) );
                                    if (!String.IsNullOrEmpty(temp))
                                    {
                                        if (temp.Contains("<bold>"))
                                        {
                                            articulo.Abstractt += "<sec>";
                                            articulo.Abstractt += formateaTexto(doc.Paragraphs[x], articulo, "", (x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null)
                                                ).Replace("<bold>", "<title>").Replace("</bold>", "</title><p>");
                                            articulo.Abstractt += "</p></sec>";
                                        }
                                        else
                                        {
                                            articulo.Abstractt += "<p>"+temp+"</p>";
                                        }
                                    }
                                }
                                //aca empiezan las key word
                                else if (!cursiva(doc.Paragraphs[x]) && alineacion(doc.Paragraphs[x], "wdAlignParagraphLeft")
                                    && tipoletra(doc.Paragraphs[x], "Times New Roman") && !vacio(doc.Paragraphs[x]) && tamano(doc.Paragraphs[x], 12)
                                    &&  (  texto(doc.Paragraphs[x]).StartsWith("Key words:", true, null) 
                                        || texto(doc.Paragraphs[x]).StartsWith("Keywords:", true, null)
                                        || texto(doc.Paragraphs[x]).StartsWith("Palavras chave:", true, null) 
                                        || texto(doc.Paragraphs[x]).StartsWith("Palavras-chave:", true, null) 
                                        || texto(doc.Paragraphs[x]).StartsWith("Palavraschave:", true, null))
                                    )
                                {
                                    log += "<br/>*****" + articulo.Abstractt;
                                    x -= 1;
                                    i = x; j = x;
                                    break;
                                }
                            }
                        }
                        //buscar Keyword
                        else if (negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") 
                            && (texto(doc.Paragraphs[i]).StartsWith("Key words:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Keywords:", true, null)
                            || texto(doc.Paragraphs[i]).StartsWith("Palavras chave:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Palavras-chave:", true, null) || texto(doc.Paragraphs[i]).StartsWith("Palavraschave:", true, null))
                            && tamano(doc.Paragraphs[i], 12))
                        {
                            String kw = texto(doc.Paragraphs[i]);
                            kw = kw.Substring(kw.IndexOf(":") + 1);
                            foreach (String key in kw.Split(','))
                            {
                                articulo.addKeywords(key.Trim());
                            }
                            log += "<br/>*****Total Keywords: " + articulo.Keywords.Count() + "<br/>";
                            j = i;
                        }
                    }
                    j += 1;
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        log += "<br/>*Parrafo:" + i;
                        //buscar fechas
                        if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12)
                            && (
                                texto(doc.Paragraphs[i]).ToLower().Contains("received:") 
                                || texto(doc.Paragraphs[i]).ToLower().Contains("recibido:")
                                || texto(doc.Paragraphs[i]).ToLower().Contains("recepción:")
                                ))
                        {
                            String fechare = extraeFecha(texto(doc.Paragraphs[i]));
                            if (String.IsNullOrEmpty(fechare))
                                fechare = extreaAnio( texto(doc.Paragraphs[i]) );
                            articulo.Fecharecibido = fechare;
                            
                            log += "<br/>*****fecha received: " + fechare + "<br/>";
                            j = i;
                        }
                        else if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12)
                            && (
                                texto(doc.Paragraphs[i]).ToLower().Contains("revisado:") 
                                || texto(doc.Paragraphs[i]).ToLower().Contains("reviewed:")
                            || texto(doc.Paragraphs[i]).ToLower().Contains("reviewed:")
                            ))
                        {
                            String fechare = extraeFecha(texto(doc.Paragraphs[i]));
                            if (String.IsNullOrEmpty(fechare))
                                fechare = extreaAnio( texto(doc.Paragraphs[i]) );
                            articulo.Fecharevisado = fechare;
                            
                            log += "<br/>*****fecha reviewed: " + fechare + "<br/>";
                            j = i;
                        }
                        else if (!negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                                && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12)
                                && (
                                        texto(doc.Paragraphs[i]).ToLower().Contains("aceptado:") 
                                        || texto(doc.Paragraphs[i]).ToLower().Contains("accepted:")
                                        || texto(doc.Paragraphs[i]).ToLower().Contains("aceptación:")
                                ))
                        {
                            String fechare = extraeFecha(texto(doc.Paragraphs[i]));
                            if (String.IsNullOrEmpty(fechare))
                                fechare = extreaAnio(texto(doc.Paragraphs[i]));
                            articulo.Fechaaceptado = fechare;
                            log += "<br/>*****fecha Accepted: " + fechare + "<br/>";
                            j = i;
                        }
                    }

                    j += 1;
                    parrafoinicial = j;
                    parrafoFinal = j;
                    //buscar parrafo final del cuerpo del articulos
                    int numeroImagen = 1;
                    String ultimoSeccionBody = null;
                    String ultimoSubSeccionBody = null;
                    Boolean bInTable;
                    int ultimatabla = -1;

                    String rutaImagenes = rutaContenido + Path.GetFileNameWithoutExtension(rutaDocumento) + "_archivos/";
                    bool exists = Directory.Exists(rutaImagenes);

                    if (!exists)
                        Directory.CreateDirectory(rutaImagenes);


                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        String text = texto(doc.Paragraphs[i]);
                        if (
                                
                                (
                                negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                                && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12)
                                && (texto(doc.Paragraphs[i]).ToLower().Contains("acknowledgements") || text.ToLower().Contains("acknowledgments") || text.ToLower().Contains("agradecimientos") || text.ToLower().Contains("obrigado"))
                                ) 
                                || 
                                (
                                negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                                && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12)
                                && (texto(doc.Paragraphs[i]).ToLower().Contains("references") || text.ToLower().Contains("referencias") || text.ToLower().Contains("referências"))
                                )
                            )
                        {
                            parrafoFinal = i ;
                            break;
                        }
                        else // guardar secciones
                        {
                            //ver si es el titulo de una seccion
                            if ( negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 16) 
                            ){
                                articulo.SectionsBody.Add(texto(doc.Paragraphs[i]), new SeccionBody( texto(doc.Paragraphs[i]) ));
                                ultimoSeccionBody = texto(doc.Paragraphs[i]);

                                articulo.addSeccion( texto(doc.Paragraphs[i]) );

                                ultimoSubSeccionBody = null;
                            }
                            else if (negrita(doc.Paragraphs[i]) && !cursiva(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                           && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 14)
                           )
                            {
                                if (ultimoSeccionBody == null || articulo.SectionsBody.Count == 0)
                                    continue;

                                SeccionBody temp = articulo.SectionsBody[ultimoSeccionBody];
                                temp.subsecciones.Add( texto(doc.Paragraphs[i]) , new SeccionBody( texto(doc.Paragraphs[i]) ));

                                ultimoSubSeccionBody = texto(doc.Paragraphs[i]);
                            }
                            else{ // es el cuerpo de la sección

                                if (ultimoSeccionBody == null || articulo.SectionsBody.Count == 0)
                                    continue;

                                SeccionBody temp = articulo.SectionsBody[ultimoSeccionBody];

                                //String textoParrafo = texto(doc.Paragraphs[i]);
                                String textoParrafo = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));

                                if (abrirlista && !cerrarlista)
                                {
                                    textoParrafo = "<list list-type=\"order\">"+textoParrafo;
                                    cerrarlista = true;
                                }

                                if (!abrirlista && cerrarlista)
                                {
                                    textoParrafo =  "</list>"+textoParrafo;
                                    cerrarlista = false;
                                    abrirlista = false;
                                }


                                textoParrafo = textoParrafo.Replace("\v", " ").Replace(@"&#11;", " ").Replace(@"&#xb;", " ").Replace(@"\u000B", " ").Replace(((char)2) + "", "");

                                bInTable = false;
                                Microsoft.Office.Interop.Word.Range r = doc.Paragraphs[i].Range;

                                for (int xx = 0; xx < TablesRanges.Count; xx++ )
                                {
                                    Microsoft.Office.Interop.Word.Range range = TablesRanges[xx];
                                    if (r.Start >= range.Start && r.Start <= range.End)
                                    {
                                        //temp.addParrafo("In Table - Paragraph number " + i.ToString() + ":" + r.Text);
                                        bInTable = true;
                                        String numeroTabla = xx+"";
                                        
                                        if (ultimatabla == xx)
                                            break;

                                        ultimatabla = xx;
                                        String label = "Tabla" + numeroTabla + 1, title = "Tabla.";

                                        
                                        if ( !String.IsNullOrEmpty(descipcionTable) && descipcionTable.Contains(":")){
                                            descipcionTable = descipcionTable.Replace("<bold>", "").Replace("</bold>", "");
                                            label = descipcionTable.Substring(0, descipcionTable.IndexOf(":"));
                                            title = descipcionTable.Substring(descipcionTable.IndexOf(":")+1);
                                            descipcionTable = null;
                                        }


                                        String tabla = "<table-wrap id=\"t" + numeroTabla+1 + "\">"
                                                            + "<label>" + label + "</label>"
                                                            + "<caption>"
                                                                + "<title>"+title+"</title>"
                                                            + "</caption>"
                                                            + "<table>";


                                        int filas   = doc.Tables[xx+1].Rows.Count;
                                        int columnas = doc.Tables[xx + 1].Columns.Count;
                                        if (filas > 0)
                                        {
                                            tabla += "<thead><tr>";
                                            for (int ww = 1; ww <= columnas; ww++)
                                                try
                                                {
                                                    tabla += "<th>" + formateaTexto(doc.Tables[xx + 1].Cell(1, ww).Range, articulo, rutaImagenes, null) + "</th>";
                                                }
                                                catch (Exception ex) { }
                                            tabla += "</tr></thead>";
                                        }
                                        tabla += "<tbody>";
                                        for (int yy = 2; yy <= filas; yy++)
                                        {
                                            tabla += "<tr>";
                                            for (int ww = 1; ww <= columnas; ww++)
                                            {
                                                Cell celda = null;
                                                try {
                                                    celda = doc.Tables[xx + 1].Cell(yy, ww);
                                                }
                                                catch (Exception wp){}

                                                if (celda != null && celda.Range != null && celda.Range.Text != null)
                                                {
                                                    
                                                    String imagen = "";
                                                    tabla += "<td>" + formateaTexto(celda.Range, articulo, rutaImagenes, null) + " " + imagen + "</td>";
                                                }
                                                else
                                                    tabla += "<td></td>";
                                            }
                                            tabla += "</tr>";
                                                
                                            
                                        }
                                        tabla += "</tbody>";
                                            tabla += "</table>";
                                            tabla += "<attrib></attrib>"
                                                  + "</table-wrap>";


                                        String attrib = null;
                                        int contador = i + (filas * columnas);
                                        for (; contador <= doc.Paragraphs.Count; contador++ )
                                        {
                                            Microsoft.Office.Interop.Word.Range rangoTemp = doc.Paragraphs[contador].Range;
                                            if (rangoTemp.Start >= range.End)
                                            {
                                                if (alineacion(doc.Paragraphs[contador], "wdAlignParagraphCenter") && tamano(doc.Paragraphs[contador], 10) && !negrita(doc.Paragraphs[contador]))
                                                {
                                                    attrib = texto(doc.Paragraphs[contador]);
                                                    tabla = tabla.Replace("<attrib></attrib>", "<attrib>" + attrib + "</attrib>");
                                                    reemplazos.Add("<p>" + attrib + "</p>", "");
                                                    reemplazos.Add("(" + label.Trim().ToLower() + ")", "(<xref ref-type=\"table\" rid=\"t" + numeroTabla + 1 + "\">" + label.Trim().ToLower() + "</xref>)");
                                                }
                                                else
                                                    tabla = tabla.Replace("<attrib></attrib>", "");
                                                break;
                                            }
                                        }
                                   
                                        
                                       
                                        if (String.IsNullOrEmpty(ultimoSubSeccionBody))
                                            temp.addParrafo(tabla);
                                        else
                                        {
                                            temp.subsecciones[ultimoSubSeccionBody].addParrafo(tabla);
                                        }
                                        break;
                                    }

                                }

                                if (!bInTable)
                                {
                                    if (!string.IsNullOrEmpty(ultimoSeccionBody) && articulo.SectionsBody.ContainsKey(ultimoSeccionBody) && !string.IsNullOrEmpty(textoParrafo))
                                    {

                                        if (String.IsNullOrEmpty(ultimoSubSeccionBody))
                                            temp.addParrafo(textoParrafo);
                                        else
                                        {
                                            temp.subsecciones[ultimoSubSeccionBody].addParrafo(textoParrafo);
                                        }
                                    }



                                }
                                
                            }
                        }

                    }

                    //buscar referencias
                    for (int i = parrafoFinal; i <= doc.Paragraphs.Count; i++)
                    {
                        if (!negrita(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(texto(doc.Paragraphs[i])))
                        {
                            String refe = texto(doc.Paragraphs[i]);
                            Referencia referencia = new Referencia();
                            referencia.Original = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));
                            //buscar paginas
                            foreach (String tmp in refe.Split(",.".ToCharArray()))
                            {
                                if (tmp.ToLower().Contains("pp"))
                                {
                                    referencia.Pages = tmp.Replace("pp", "").Replace(".", "").Trim();
                                    log += "<br/>*****Paginas" + referencia.Pages;
                                }
                                else if (tmp.Contains("-"))
                                {
                                    String nombre = "", apellido = "";
                                    foreach (String pal in tmp.Split(new char[0])) // dividir nombre en espacios
                                    {
                                        if (pal.Contains("-")) apellido += " " + pal;
                                        else nombre += " " + pal;
                                    }
                                    referencia.addAutor(nombre, apellido);
                                }
                                else if (tmp.Any(char.IsDigit)) // ver si la cadena contiene digitos
                                {
                                    foreach (String numero in Regex.Split(tmp, @"\D+"))
                                    {
                                        if (numero.Length == 4)
                                        {
                                            referencia.Date = numero;
                                        }
                                        else if (numero.Length < 4)
                                        {
                                            referencia.Edicion = numero;
                                        }
                                    }
                                }
                                else if (tmp.Length > 20 && String.IsNullOrEmpty(referencia.Titulo))
                                {
                                    referencia.Titulo = tmp;
                                }
                            }
                            if (!String.IsNullOrEmpty(referencia.Original))
                                articulo.addReferencia(referencia);

                        }
                        else if (negrita(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(texto(doc.Paragraphs[i])))
                        {
                            
                            
                                String soyAgradecimiento = texto(doc.Paragraphs[i]);
                                //articulo.Agradecimientos += soyAgradecimiento + ".<br/>";
                            
                            
                            
                        }
                        else if (!negrita(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                            && tipoletra(doc.Paragraphs[i], "Times New Roman") && tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(texto(doc.Paragraphs[i])))
                        {


                            String soyAgradecimiento = texto(doc.Paragraphs[i]);
                            articulo.Agradecimientos += soyAgradecimiento + ".<br/>";



                        }
                    }

                    

                    log += "<br/>>>>>>>Parrafo inicial y final: " + parrafoinicial + "<<<<>>>>" + parrafoFinal + "<<<<<";

                   

                    doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                    doc = null;
                    word.Quit();
                    GC.Collect(); 


                    return true;
                }
                catch (Exception e)
                {
                    log += "<br/>Error: " + e.StackTrace;
                    System.Console.Error.WriteLine("exception: {0}", e);
                    mensaje = "Error al procesar documento: consulte a su administrador";
                    doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                    doc = null;
                    word.Quit();
                    return false;
                }
            }
            else
            {
                mensaje = "Error: Archivo no existe, verifique sus rutas";
                return false;
            }
            return false;
        }


        public String formateaTexto(Range rango, Articulo articulo, String rutaGuardar, Microsoft.Office.Interop.Word.Paragraph parrafoSiguiente)
        {
            if (rango == null || String.IsNullOrEmpty(rango.Text))
                return "";

            if (rango.Text.Trim().Equals("\r") || rango.Text.Trim().Equals("\n") || rango.Text.Trim().Equals("\r\n")
                    || String.IsNullOrEmpty(rango.Text.Trim()) || Environment.NewLine.Equals(rango.Text.Trim())
                    || rango.Text.Trim().Equals("\r\a"))
                return "";

            try
            {
                String xml = rango.XML;
                String st = "";
                Boolean justificado = false;
                Boolean tamanomenor12 = false;

                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    reader.ReadToFollowing("wx:sect");

                    XmlReader inner = reader.ReadSubtree();
                    String seccion = inner.ReadInnerXml();
                    //inner.ReadToFollowing("w:r");


                    if (xml.Contains("w:jc w:val=\"both\""))
                        justificado = true;
                    if (xml.Contains("w:listPr"))
                        eslista = true;
                   
                    if (inner.ReadToDescendant("w:r"))
                    {
                        do
                        {
                            XmlReader wt = inner.ReadSubtree();

                            //wt.ReadToFollowing("w:t");
                            String cadena = "";
                            String alineacionVertical = "";
                            Boolean bold = false;
                            Boolean italic = false;
                            Boolean underline = false;
                            Boolean footnote = false;
                            Boolean binData = false;
                            Boolean tamano12 = false;
                            Boolean center = false;
                            String nombreArchivo = "";
                            


                            while (wt.Read())
                            {
                                String type = wt.NodeType.ToString();
                                String NombreNodo = wt.Name;
                                String ValorNodo = wt.Value;

                                if (type.Equals("Text"))
                                    cadena += wt.Value;

                                if (tamano12 && bold && cadena.Trim().EndsWith(":") && !footnote && !cadena.ToLower().Contains("table") && !cadena.ToLower().Contains("tabla"))
                                {
                                    anteriorDescipcionImagen = true;
                                }


                                if (tamano12 && bold && cadena.Trim().EndsWith(":") && !footnote && (cadena.ToLower().Contains("table") || cadena.ToLower().Contains("tabla")))
                                {
                                    anteriorDescipcionTable = true;
                                }

                                

                                if (NombreNodo.Equals("w:vertAlign") && type.Equals("Element"))
                                    alineacionVertical = wt.GetAttribute("w:val");

                                if (NombreNodo.Equals("w:sz-cs") && type.Equals("Element"))
                                    if (wt.GetAttribute("w:val").Equals("24"))
                                        tamano12 = true;

                                if (NombreNodo.Equals("w:sz-cs") && type.Equals("Element"))
                                    if (wt.GetAttribute("w:val").Equals("23") || wt.GetAttribute("w:val").Equals("22") || wt.GetAttribute("w:val").Equals("21") || wt.GetAttribute("w:val").Equals("20"))
                                        tamanomenor12 = true;

                                if (NombreNodo.Equals("w:underline") && type.Equals("Element"))
                                    underline = true;

                                if (NombreNodo.Equals("w:u") && type.Equals("Element"))
                                    underline = true;

                                if (NombreNodo.Equals("w:b") && type.Equals("Element"))
                                    bold = true;

                                if (NombreNodo.Equals("w:i") && type.Equals("Element"))
                                    italic = true;

                                if (NombreNodo.Equals("w:footnote") && type.Equals("Element"))
                                    footnote = true;

                                if (NombreNodo.Equals("w:binData") && type.Equals("Element"))
                                {
                                    binData = true;
                                    nombreArchivo = DateTime.Now.Ticks +wt.GetAttribute("w:name").Replace("wordml://", "");
                                }
                            }

                            if (!String.IsNullOrEmpty(cadena))
                            {
                                if (binData && !String.IsNullOrEmpty(nombreArchivo))
                                {
                                    String pathImagen = rutaGuardar + @"\" + nombreArchivo;
                                    var bytes = Convert.FromBase64String(cadena);
                                    using (var imageFile = new FileStream(pathImagen, FileMode.Create))
                                    {
                                        imageFile.Write(bytes, 0, bytes.Length);
                                        imageFile.Flush();
                                    }

                                    if (nombreArchivo.EndsWith(".wmz"))
                                    {
                                        using (Stream inputStream = new FileStream(pathImagen, FileMode.Open))
                                        {
                                            using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                                            {
                                                Image i = Image.FromStream(gzipStream);
                                                i.Save(pathImagen.Replace(".wmz", ".png"), ImageFormat.Png);
                                                nombreArchivo = nombreArchivo.Replace(".wmz", ".png");
                                            }
                                        }
                                        File.Delete(pathImagen);
                                    }

                                    if (!String.IsNullOrEmpty(st.Trim()))
                                    { // imagen inline
                                        cadena = "<inline-graphic xlink:href=\"" + nombreArchivo + "\"/>";
                                        articulo.numImagenes += 1;
                                    }
                                    else
                                    {

                                        String attrib = null;
                                        if (parrafoSiguiente != null)
                                        {
                                            if ((alineacion(parrafoSiguiente, "wdAlignParagraphCenter") || alineacion(parrafoSiguiente, "1")) && tamano(parrafoSiguiente, 10) && !negrita(parrafoSiguiente))
                                            {
                                                attrib = texto(parrafoSiguiente);
                                                reemplazos.Add("<p>" + attrib + "</p>", "");
                                            }
                                        }


                                        String label = "Figura ESCRIBA No. FIGURA", titulo = "ESCRIBA TITULO";
                                        if (!String.IsNullOrEmpty(descipcionImagen)) // se encontro descripcion para la imagen, se asume que no es formula
                                        {
                                            articulo.numImagenes += 1;
                                            if (descipcionImagen.Contains(":"))
                                            {
                                                descipcionImagen = descipcionImagen.Replace("<bold>", "").Replace("</bold>", "");
                                                label = descipcionImagen.Substring(0, descipcionImagen.IndexOf(":"));
                                                //reemplazos.Add("(" + label.Trim().ToLower() + ")", "(<xref ref-type=\"fig\" rid=\"f" + extraeNumero(label) + "\">" + label.Trim().ToLower() + "</xref>)");
                                                reemplazos.Add("(" + label.Trim().ToLower() + ")", "(<xref ref-type=\"fig\" rid=\"f" + articulo.numImagenes + "\">" + label.Trim().ToLower() + "</xref>)");
                                                titulo = descipcionImagen.Substring(descipcionImagen.IndexOf(":") + 1);
                                            }
                                            //cadena = "\n<fig id=\"f" + extraeNumero(label) + "\">";
                                            cadena = "\n<fig id=\"f" + articulo.numImagenes + "\">";
                                            cadena += "\n<label>" + label + "</label>";
                                            cadena += "\n<caption><title>" + titulo + "</title></caption>";
                                            cadena += "\n<graphic xlink:href=\"" + nombreArchivo + "\"/>";
                                            if (!String.IsNullOrEmpty(attrib))
                                                cadena += "<attrib>" + attrib + "</attrib>";
                                            cadena += "\n</fig>";
                                            descipcionImagen = null;
                                        }
                                        else
                                        {
                                            articulo.numFormulas += 1;
                                            cadena = "<!-- ELIJA SI LA IMAGEN ES FORMULA O GRAPHIC -->";
                                            cadena += "\n<disp-formula id=\"e" + extraeNumero(label) + "" + articulo.numFormulas + "\">";
                                            cadena += "\n<graphic xlink:href=\"" + nombreArchivo + "\"/>";
                                            cadena += "\n</disp-formula>";
                                        }
                                    }
                                    nombreArchivo = "";
                                    binData = false;
                                }
                                if (!String.IsNullOrEmpty(alineacionVertical) && alineacionVertical.Equals("superscript") && footnote)
                                {
                                    if (!articulo.notas.Contains(cadena))
                                    {
                                        articulo.notas.Add(cadena);
                                        cadena = "<xref ref-type=\"fn\" rid=\"fn" + articulo.notas.Count + "\"><sup>" + articulo.notas.Count + "</sup></xref>";
                                    }
                                }

                                if (!String.IsNullOrEmpty(alineacionVertical) && alineacionVertical.Equals("superscript") && !footnote)
                                {
                                    var mg = Regex.Match(cadena, @"\((\d+)\)");
                                    if (mg.Success)
                                    {
                                        String numero = extraeNumero(cadena);
                                        cadena = "<xref ref-type=\"bibr\" rid=\"B" + numero + "\"><sup>" + numero + "</sup></xref>";
                                    }
                                    else
                                    {
                                        cadena = "<sup>" + cadena + "</sup>";
                                    }
                                    
                                }

                                if (!String.IsNullOrEmpty(alineacionVertical) && alineacionVertical.Equals("subscript"))
                                {
                                    cadena = "<sub>" + cadena + "</sub>";
                                }

                                if (bold)
                                    cadena = "<bold>" + cadena + "</bold>";
                                if (italic)
                                    cadena = "<italic>" + cadena + "</italic>";
                                if (underline)
                                    cadena = "<underline>" + cadena + "</underline>";

                                
                                st += cadena;
                            }



                        } while (inner.ReadToNextSibling("w:r"));
                    }
                }

                if (anteriorDescipcionImagen)
                {
                    descipcionImagen = st;
                    anteriorDescipcionImagen = false;
                }
                    

                if (anteriorDescipcionTable)
                {
                    descipcionTable = st;
                    anteriorDescipcionTable = false;
                }

                if (justificado && tamanomenor12)
                    st = "<disp-quote>" + st + "</disp-quote>";

                if (eslista)
                {
                    st = "<list-item><p>" + st + "</p></list-item>";
                    if (abrirlista == false && cerrarlista == false)
                        abrirlista = true;
                    eslista = false;
                }
                else
                {
                    if (abrirlista  && cerrarlista ) //debemos cerrar el tag lista
                        abrirlista = false;
                }
                
                
                return st;
            }
            catch (Exception e) { return ""; }


        }

        public String formateaTexto(Microsoft.Office.Interop.Word.Paragraph parrafo, Articulo articulo, String rutaGuardar, Microsoft.Office.Interop.Word.Paragraph parrafoSiguiente)
        {
            if (parrafo == null || parrafo.Range == null || String.IsNullOrEmpty(parrafo.Range.Text))
                return "";

            if (parrafo.Range.Text.Trim().Equals("\r") || parrafo.Range.Text.Trim().Equals("\n") || parrafo.Range.Text.Trim().Equals("\r\n") 
                    || String.IsNullOrEmpty(parrafo.Range.Text.Trim()) || Environment.NewLine.Equals(parrafo.Range.Text.Trim())
                    || parrafo.Range.Text.Trim().Equals("\r\a"))
                return "";

            return formateaTexto(parrafo.Range, articulo, rutaGuardar, parrafoSiguiente);
            
        }

        public Boolean negrita( Microsoft.Office.Interop.Word.Paragraph parrafo ) 
        {
            if (Math.Abs(parrafo.Range.Font.Bold) == 0)
                return false;
            return true;
        }

        public Boolean cursiva(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            if (Math.Abs(parrafo.Range.Font.Italic) == 0)
                return false;
            return true;
        }

        public int tamano(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            return (int)parrafo.Range.Font.Size;
        }

        public Boolean tamano(Microsoft.Office.Interop.Word.Paragraph parrafo, int tam)
        {
            if (tamano(parrafo) == tam)
                return true;
            return false;
        }

        public Boolean tipoletra(Microsoft.Office.Interop.Word.Paragraph parrafo, String tipoletra)
        {
            if (parrafo.Range.Font.NameBi.Equals(tipoletra))
                return true;
            return false;
        }

        public String tipoletra(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            return parrafo.Range.Font.NameBi;
                
        }

        public Boolean alineacion(Microsoft.Office.Interop.Word.Paragraph parrafo, String alineacion)
        {
            if (parrafo.Alignment.ToString().Equals(alineacion))
                return true;
            return false;
        }

        public String alineacion(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            return parrafo.Alignment.ToString();
                
        }


        public String texto(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            String st =  parrafo.Range.Text.ToString().Trim();
            if (st.Contains("\v"))
            { 
                st = st.Replace("\v", " ").Replace(@"&#11;", " ").Replace(@"&#xb;", " ").Replace(@"\u000B", " ");
                st = st+"";
            }
            st = st.Replace(@"<br>", " ").Replace(@"<br/>", " ").Replace(@"<br>", " ");
            return st;
        }

        public Boolean vacio(Microsoft.Office.Interop.Word.Paragraph parrafo)
        {
            if (texto(parrafo).Equals(""))
                return true;
            return texto(parrafo).Equals(Environment.NewLine);
        }

        public String conviertefecha(String f)
        {
            String fecha = "";
            String[] formatStrings = { "M/y", "M/d/y", "M-d-y", "d/M/y", "d-M-y", "mm/dd/yyyy", "mm-dd-yyyy", "dd/mm/yyyy", "dd-mm-yyyy" };
           

            return fecha;
        }

        public Boolean generaXMLV4()
        {
            if (articulo != null)
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    XmlNode docNode = xml.CreateXmlDeclaration("1.0", "utf-8", null);
                    xml.AppendChild(docNode);
                    XmlDocumentType doctype = xml.CreateDocumentType("article", "-//NLM//DTD JATS (Z39.96) Journal Publishing DTD v1.0 20120330//EN", @"http://jats.nlm.nih.gov/publishing/1.1d1/JATS-journalpublishing1.dtd", null);
                    xml.AppendChild(doctype);
                    XmlElement article = xml.CreateElement("article");
                    article.SetAttribute("dtd-version", "1.0");
                    article.SetAttribute("article-type", "research-article");
                    article.SetAttribute("xml:lang", "es");
                    article.SetAttribute("xmlns:xlink", @"http://www.w3.org/1999/xlink");
                    article.SetAttribute("xmlns:mml", @"http://www.w3.org/1998/Math/MathML");
                    article.SetAttribute("specific-use", "sps-1.3");
                    xml.AppendChild(article);

                    /*
                     * FRONT
                     */
                    XmlElement front = xml.CreateElement("front");
                    article.AppendChild(front);

                    XmlElement journalmeta = xml.CreateElement("journal-meta");
                    front.AppendChild(journalmeta);

                    XmlElement journalid1 = xml.CreateElement("journal-id");
                    journalmeta.AppendChild(journalid1);
                    journalid1.SetAttribute("journal-id-type", "nlm-ta");

                    
                    XmlElement journalid2 = xml.CreateElement("journal-id");
                    journalmeta.AppendChild(journalid2);
                    journalid2.SetAttribute("journal-id-type", "publisher-id");
                    journalid2.InnerText = "1";

                    XmlElement journaltitlegroup = xml.CreateElement("journal-title-group");
                    journalmeta.AppendChild(journaltitlegroup);

                    //revistaTitulo
                    if (!String.IsNullOrEmpty(parametrosXML["revistaTitulo"]))
                    { 
                        XmlElement journaltitle = xml.CreateElement("journal-title");
                        journaltitlegroup.AppendChild(journaltitle);
                        journaltitle.InnerText = parametrosXML["revistaTitulo"];
                        journalid1.InnerText = parametrosXML["revistaTitulo"];
                    }

                    //abbreviation
                    XmlElement abbrevjournaltitle = xml.CreateElement("abbrev-journal-title");
                    journaltitlegroup.AppendChild(abbrevjournaltitle);
                    abbrevjournaltitle.SetAttribute("abbrev-type", "publisher");
                    String abbreviation = parametrosXML["abbreviation"];
                    if (!String.IsNullOrEmpty(parametrosXML["abbreviation"]))
                    { 
                        if (abbreviation.Contains("||") && abbreviation.Length > 2)
                        {
                            abbreviation = abbreviation.Substring(0, abbreviation.Length - 2); // quitar los ultimos ||
                            abbrevjournaltitle.InnerText = abbreviation;
                        }
                        else
                        {
                            abbrevjournaltitle.InnerText = abbreviation;
                        }
                    }
                    else
                    {
                        abbrevjournaltitle.InnerText = parametrosXML["revistaTitulo"];
                    }

                    if (!String.IsNullOrEmpty(parametrosXML["printIssn"]))
                    {
                        XmlElement printIssn = xml.CreateElement("issn");
                        journalmeta.AppendChild(printIssn);
                        printIssn.SetAttribute("pub-type", "ppub");
                        printIssn.InnerText = parametrosXML["printIssn"];
                    }

                    if (!String.IsNullOrEmpty(parametrosXML["onlineIssn"]))
                    {
                        XmlElement onlineIssn = xml.CreateElement("issn");
                        journalmeta.AppendChild(onlineIssn);
                        onlineIssn.SetAttribute("pub-type", "epub");
                        onlineIssn.InnerText = parametrosXML["onlineIssn"];
                    }

                    if (!String.IsNullOrEmpty(parametrosXML["publisher"]))
                    {
                        XmlElement publisher = xml.CreateElement("publisher");
                        journalmeta.AppendChild(publisher);

                        XmlElement publishername = xml.CreateElement("publisher-name");
                        publisher.AppendChild(publishername);

                        publishername.InnerText = parametrosXML["publisher"];
                    }

                    //article-meta
                    XmlElement articlemeta = xml.CreateElement("article-meta");
                    front.AppendChild(articlemeta);

                    //Article Identifier, zero or more
                    if (!String.IsNullOrEmpty(articulo.Doi))
                    {
                        XmlElement articleid = xml.CreateElement("article-id");
                        articlemeta.AppendChild(articleid);
                        articleid.SetAttribute("pub-id-type", "doi");
                        articleid.InnerText = articulo.Doi;
                    }
                    else
                    {
                        XmlComment comentario = xml.CreateComment("No se encontro DOI en documento Word, debe agregarlo manualmente");
                        articlemeta.AppendChild(comentario);
                    }

                    //Article Grouping Data, zero or one
                    XmlElement articlecategories = xml.CreateElement("article-categories");
                    articlemeta.AppendChild(articlecategories);
                    XmlElement subjgroup = xml.CreateElement("subj-group");
                    articlecategories.AppendChild(subjgroup);
                    subjgroup.SetAttribute("subj-group-type", "heading");

                    /*
                    foreach(String tempSec in articulo.secciones ){
                        XmlElement subject = xml.CreateElement("subject");
                        subjgroup.AppendChild(subject);
                        subject.InnerText = tempSec;
                    }
                     * */
                    XmlElement subject = xml.CreateElement("subject");
                    subjgroup.AppendChild(subject);
                    subject.InnerText = articulo.Seccion;

                    //Title Group, zero or one
                    XmlElement titlegroup = xml.CreateElement("title-group");
                    articlemeta.AppendChild(titlegroup);
                    XmlElement articletitle = xml.CreateElement("article-title");
                    titlegroup.AppendChild(articletitle);
                    //articletitle.SetAttribute("lang_id", "es");
                    articletitle.InnerText = articulo.Titulo;

                    if (!string.IsNullOrEmpty(articulo.Title))
                    {
                        XmlElement transtitlegroup = xml.CreateElement("trans-title-group");
                        titlegroup.AppendChild(transtitlegroup);
                        //transtitlegroup.SetAttribute("lang_id","en");

                        XmlElement transtitle = xml.CreateElement("trans-title");
                        transtitlegroup.AppendChild(transtitle);
                        transtitle.InnerText = articulo.Title;
                    }

                    /*  Any combination of:
                        <contrib-group> Contributor Group
                        <aff> Affiliation
                        <aff-alternatives> Affiliation Alternatives
                        <x> X - Generated Text and Punctuation
                        <author-notes> Author Note Group, zero or one 
                     */
                    //Autores
                    XmlElement contribgroup = xml.CreateElement("contrib-group");
                    articlemeta.AppendChild(contribgroup);

                    int contador = 1;
                    foreach (Autor aut in articulo.Autores)
                    {
                        XmlElement contrib = xml.CreateElement("contrib");
                        contribgroup.AppendChild(contrib);
                        contrib.SetAttribute("contrib-type", "author");

                        XmlElement name = xml.CreateElement("name");
                        XmlElement surname = xml.CreateElement("surname");
                        XmlElement givennames = xml.CreateElement("given-names");

                        contrib.AppendChild(name);
                        name.AppendChild(surname);
                        name.AppendChild(givennames);
                        surname.InnerText = aut.SurName;
                        givennames.InnerText = aut.FirstName;

                        if (!String.IsNullOrEmpty(aut.Afiliacion))
                        {
                            XmlElement xref = xml.CreateElement("xref");
                            contrib.AppendChild(xref);
                            xref.SetAttribute("ref-type", "aff");
                            xref.SetAttribute("rid", "aff" + aut.Afiliacion);
                            xref.InnerText = contador + "";
                        }
                        if (!String.IsNullOrEmpty(articulo.Autorcorres) && !String.IsNullOrEmpty(articulo.AutorcorresNombre) && articulo.AutorcorresNombre.Contains(aut.FirstName) && articulo.AutorcorresNombre.Contains(aut.SurName))
                        {

                            XmlElement xref = xml.CreateElement("xref");
                            contrib.AppendChild(xref);
                            xref.SetAttribute("ref-type", "corresp");
                            xref.SetAttribute("rid", "c01");
                            xref.InnerText = "<sup>*</sup>";
                        }
                        contador += 1;

                    }
                    foreach (Afiliacion af in articulo.Afiliaciones)
                    {
                        XmlElement aff = xml.CreateElement("aff");
                        contribgroup.AppendChild(aff);
                        aff.SetAttribute("id", "aff" + af.Id);

                        XmlElement label = xml.CreateElement("label");
                        aff.AppendChild(label);
                        label.InnerText = af.Id;


                        XmlElement institution = xml.CreateElement("institution");
                        aff.AppendChild(institution);
                        institution.SetAttribute("content-type", "original");
                        institution.InnerText = af.Original.Replace(af.Id, " ");
                    }

                    if (!String.IsNullOrEmpty(articulo.Autorcorres))
                    {
                        XmlElement authornotes = xml.CreateElement("author-notes");
                        articlemeta.AppendChild(authornotes);
                        XmlElement corresp = xml.CreateElement("corresp");
                        authornotes.AppendChild(corresp);
                        corresp.SetAttribute("id", "c01");
                        corresp.InnerText = "E-mail: ";
                        XmlElement email = xml.CreateElement("email");
                        corresp.AppendChild(email);
                        email.InnerText = articulo.Autorcorres;
                    }

                    //<pub-date> Publication Date, zero or more, el validador me obliga a ponerlo
                    if (!String.IsNullOrEmpty(parametrosXML["date"]))
                    {
                        String date = parametrosXML["date"]+"";
                        XmlElement pubdate = xml.CreateElement("pub-date");
                        pubdate.SetAttribute("pub-type", "epub-ppub");
                        articlemeta.AppendChild(pubdate);
                        if (date.Length >= 9)
                        {
                            XmlElement day = xml.CreateElement("day");
                            pubdate.AppendChild(day);
                            day.InnerText = date.Substring(8, 2);
                        }
                        //month
                        if (date.Length >= 7)
                        {
                            XmlElement month = xml.CreateElement("month");
                            pubdate.AppendChild(month);
                            month.InnerText = date.Substring(5, 2);
                        }
                        //year
                        if (date.Length >= 4)
                        {
                            XmlElement year = xml.CreateElement("year");
                            pubdate.AppendChild(year);
                            year.InnerText = date.Substring(0, 4);
                        }

                        
                    }

                    // <volume> Volume Number, zero or one
                    XmlElement volume = xml.CreateElement("volume");
                    articlemeta.AppendChild(volume);
                    if (!String.IsNullOrEmpty(parametrosXML["volume"]))
                        volume.InnerText = parametrosXML["volume"]+"";
                    else
                        volume.InnerText = "0";

                    //<issue> Issue Number, zero or more
                    XmlElement issue = xml.CreateElement("issue");
                    articlemeta.AppendChild(issue);
                    if (!String.IsNullOrEmpty(parametrosXML["issue"]))
                        issue.InnerText = parametrosXML["issue"]+"";
                    else
                        issue.InnerText = "0";
                    XmlComment insertaVolume = xml.CreateComment("Insertar Volumen y Número");
                    articlemeta.AppendChild(insertaVolume);

                    // Optionally, the following sequence (in order): <fpage> First Page
                    XmlElement fpage = xml.CreateElement("fpage");
                    articlemeta.AppendChild(fpage);
                    fpage.InnerText = 1 + "";

                    // Optionally, the following sequence (in order): <lpage> Last Page, zero or one
                    XmlElement lpage = xml.CreateElement("lpage");
                    articlemeta.AppendChild(lpage);
                    lpage.InnerText = (articulo.numPaginas + 1) + "";
                    XmlComment c1 = xml.CreateComment("Verificar valor de fpage y lpage");
                    articlemeta.AppendChild(c1);

                    //<history> History: Document History, zero or one
                    if (!String.IsNullOrEmpty(articulo.Fechaaceptado) || !String.IsNullOrEmpty(articulo.Fecharecibido) || !String.IsNullOrEmpty(articulo.Fecharevisado))
                    {
                        XmlElement history = xml.CreateElement("history");
                        articlemeta.AppendChild(history);
                        if (!String.IsNullOrEmpty(articulo.Fechaaceptado))
                        {
                            XmlElement date = xml.CreateElement("date");
                            history.AppendChild(date);
                            date.SetAttribute("date-type", "accepted");

                            if (articulo.Fechaaceptado.Length > 4 && articulo.Fechaaceptado.Contains("-"))
                            {
                                String dia = articulo.Fechaaceptado.Substring(0, articulo.Fechaaceptado.IndexOf("-"));
                                articulo.Fechaaceptado = articulo.Fechaaceptado.Substring(articulo.Fechaaceptado.IndexOf("-") + 1);
                                String mes = articulo.Fechaaceptado.Substring(0, articulo.Fechaaceptado.IndexOf("-"));
                                articulo.Fechaaceptado = articulo.Fechaaceptado.Substring(articulo.Fechaaceptado.IndexOf("-") + 1);
                                String ano = articulo.Fechaaceptado;

                                XmlElement year = xml.CreateElement("year");
                                XmlElement month = xml.CreateElement("month");
                                XmlElement day = xml.CreateElement("day");
                                date.AppendChild(day);
                                date.AppendChild(month);
                                date.AppendChild(year);
                                
                                year.InnerText = ano;
                                month.InnerText = mes;
                                day.InnerText = dia;
                            }
                            else if (articulo.Fechaaceptado.Length > 4 && articulo.Fechaaceptado.Contains("/"))
                            {
                                String dia = articulo.Fechaaceptado.Substring(0, articulo.Fechaaceptado.IndexOf("/"));
                                articulo.Fechaaceptado = articulo.Fechaaceptado.Substring(articulo.Fechaaceptado.IndexOf("/") + 1);
                                String mes = articulo.Fechaaceptado.Substring(0, articulo.Fechaaceptado.IndexOf("/"));
                                articulo.Fechaaceptado = articulo.Fechaaceptado.Substring(articulo.Fechaaceptado.IndexOf("/") + 1);
                                String ano = articulo.Fechaaceptado;

                                XmlElement year = xml.CreateElement("year");
                                XmlElement month = xml.CreateElement("month");
                                XmlElement day = xml.CreateElement("day");
                                date.AppendChild(day);
                                date.AppendChild(month);
                                date.AppendChild(year);
                                year.InnerText = ano;
                                month.InnerText = mes;
                                day.InnerText = dia;
                            }
                            else
                            {
                                XmlElement year = xml.CreateElement("year");
                                date.AppendChild(year);
                                year.InnerText = articulo.Fechaaceptado;
                            }

                        }
                        if (!String.IsNullOrEmpty(articulo.Fecharecibido))
                        {

                            XmlElement date = xml.CreateElement("date");
                            history.AppendChild(date);
                            date.SetAttribute("date-type", "received");

                            if (articulo.Fecharecibido.Length > 4 && articulo.Fecharecibido.Contains("-"))
                            {
                                String dia = articulo.Fecharecibido.Substring(0, articulo.Fecharecibido.IndexOf("-"));
                                articulo.Fecharecibido = articulo.Fecharecibido.Substring(articulo.Fecharecibido.IndexOf("-") + 1);
                                String mes = articulo.Fecharecibido.Substring(0, articulo.Fecharecibido.IndexOf("-"));
                                articulo.Fecharecibido = articulo.Fecharecibido.Substring(articulo.Fecharecibido.IndexOf("-") + 1);
                                String ano = articulo.Fecharecibido;
                                
                                XmlElement day = xml.CreateElement("day");
                                XmlElement month = xml.CreateElement("month");
                                XmlElement year = xml.CreateElement("year");
                                
                                date.AppendChild(day);
                                date.AppendChild(month);
                                date.AppendChild(year);
                                
                                year.InnerText = ano;
                                month.InnerText = mes;
                                day.InnerText = dia;
                            }
                            else if (articulo.Fecharecibido.Length > 4 && articulo.Fecharecibido.Contains("/"))
                            {
                                String dia = articulo.Fecharecibido.Substring(0, articulo.Fecharecibido.IndexOf("/"));
                                articulo.Fecharecibido = articulo.Fecharecibido.Substring(articulo.Fecharecibido.IndexOf("/") + 1);
                                String mes = articulo.Fecharecibido.Substring(0, articulo.Fecharecibido.IndexOf("/"));
                                articulo.Fecharecibido = articulo.Fecharecibido.Substring(articulo.Fecharecibido.IndexOf("/") + 1);
                                String ano = articulo.Fecharecibido;

                                XmlElement year = xml.CreateElement("year");
                                XmlElement month = xml.CreateElement("month");
                                XmlElement day = xml.CreateElement("day");
                                date.AppendChild(day);
                                date.AppendChild(month);
                                date.AppendChild(year);
                                
                                
                                year.InnerText = ano;
                                month.InnerText = mes;
                                day.InnerText = dia;
                            }
                            else
                            {

                                XmlElement year = xml.CreateElement("year");
                                date.AppendChild(year);
                                year.InnerText = articulo.Fecharecibido;
                            }
                        }
                        if (!String.IsNullOrEmpty(articulo.Fecharevisado))
                        {
                            XmlElement date = xml.CreateElement("date");
                            history.AppendChild(date);
                            date.SetAttribute("date-type", "rev-recd");

                            if (articulo.Fecharevisado.Length > 4 && articulo.Fecharevisado.Contains("-"))
                            {
                                String dia = articulo.Fecharevisado.Substring(0, articulo.Fecharevisado.IndexOf("-"));
                                articulo.Fecharevisado = articulo.Fecharevisado.Substring(articulo.Fecharevisado.IndexOf("-") + 1);
                                String mes = articulo.Fecharevisado.Substring(0, articulo.Fecharevisado.IndexOf("-"));
                                articulo.Fecharevisado = articulo.Fecharevisado.Substring(articulo.Fecharevisado.IndexOf("-") + 1);
                                String ano = articulo.Fecharevisado;

                                XmlElement year = xml.CreateElement("year");
                                XmlElement month = xml.CreateElement("month");
                                XmlElement day = xml.CreateElement("day");
                                date.AppendChild(day);
                                date.AppendChild(month);
                                date.AppendChild(year);
                                
                                
                                year.InnerText = ano;
                                month.InnerText = mes;
                                day.InnerText = dia;
                            }
                            else if (articulo.Fecharevisado.Length > 4 && articulo.Fecharevisado.Contains("/"))
                            {
                                String dia = articulo.Fecharevisado.Substring(0, articulo.Fecharevisado.IndexOf("/"));
                                articulo.Fecharevisado = articulo.Fecharevisado.Substring(articulo.Fecharevisado.IndexOf("/") + 1);
                                String mes = articulo.Fecharevisado.Substring(0, articulo.Fecharevisado.IndexOf("/"));
                                articulo.Fecharevisado = articulo.Fecharevisado.Substring(articulo.Fecharevisado.IndexOf("/") + 1);
                                String ano = articulo.Fecharevisado;

                                XmlElement year = xml.CreateElement("year");
                                XmlElement month = xml.CreateElement("month");
                                XmlElement day = xml.CreateElement("day");
                                date.AppendChild(day);
                                date.AppendChild(month);
                                date.AppendChild(year);
                                
                                
                                year.InnerText = ano;
                                month.InnerText = mes;
                                day.InnerText = dia;
                            }
                            else
                            {
                                XmlElement year = xml.CreateElement("year");
                                date.AppendChild(year);
                                year.InnerText = articulo.Fecharevisado;
                            }
                        }
                    }

                    //<permissions> Permissions, zero or one
                    XmlElement permissions = xml.CreateElement("permissions");
                    articlemeta.AppendChild(permissions);
                    XmlElement license = xml.CreateElement("license");
                    permissions.AppendChild(license);
                    license.SetAttribute("license-type", "open-access");
                    license.SetAttribute("xlink:href", @"https://creativecommons.org/licenses/by-nc-nd/4.0/deed.es");
                    XmlElement licensep = xml.CreateElement("license-p");
                    license.AppendChild(licensep);
                    licensep.InnerText = "Este es un artículo publicado en acceso abierto bajo una licencia Creative Commons";

                    //<abstract> Abstract, zero or more                    
                    if (!String.IsNullOrEmpty(articulo.Resumen))
                    {
                        XmlElement abstractt = xml.CreateElement("abstract");
                        articlemeta.AppendChild( abstractt );
                        abstractt.InnerText = articulo.Resumen;
                    }

                    //<trans-abstract> Translated Abstract, zero or more
                    if (!String.IsNullOrEmpty(articulo.Abstractt))
                    {
                        XmlElement transabstract = xml.CreateElement("trans-abstract");
                        articlemeta.AppendChild(transabstract);
                        transabstract.SetAttribute("xml:lang", "en");
                        transabstract.InnerText = articulo.Abstractt;
                    }

                    //<kwd-group> Keyword Group, zero or more
                    if (articulo.Palabrasclave.Count > 0)
                    {
                        XmlElement kwdgroup = xml.CreateElement("kwd-group");
                        articlemeta.AppendChild(kwdgroup);
                        kwdgroup.SetAttribute("xml:lang", "es");

                        XmlElement title = xml.CreateElement("title");
                        kwdgroup.AppendChild(title);
                        title.InnerText = "Palabras Clave";

                        foreach( String pc in articulo.Palabrasclave ) {
                            XmlElement kwd = xml.CreateElement("kwd");
                            kwdgroup.AppendChild(kwd);
                            kwd.InnerText = pc;
                        }
                    }

                    if (articulo.Keywords.Count > 0)
                    {
                        XmlElement kwdgroup = xml.CreateElement("kwd-group");
                        articlemeta.AppendChild(kwdgroup);
                        kwdgroup.SetAttribute("xml:lang", "en");

                        XmlElement title = xml.CreateElement("title");
                        kwdgroup.AppendChild(title);
                        title.InnerText = "Keywords";

                        foreach (String pc in articulo.Keywords)
                        {
                            XmlElement kwd = xml.CreateElement("kwd");
                            kwdgroup.AppendChild(kwd);
                            kwd.InnerText = pc;
                        }
                    }

                    //<counts> Counts, zero or one
                    XmlElement counts = xml.CreateElement("counts");
                    articlemeta.AppendChild(counts);

                    XmlElement figcount = xml.CreateElement("fig-count");
                    counts.AppendChild(figcount);
                    figcount.SetAttribute("count",articulo.numImagenes+"");

                    XmlComment c3 = xml.CreateComment("Verificar valor fig-count");
                    counts.AppendChild(c3);    

                    XmlElement tablecount = xml.CreateElement("table-count");
                    counts.AppendChild(tablecount);
                    tablecount.SetAttribute("count", articulo.numTablas+"");

                    XmlElement equationcount = xml.CreateElement("equation-count");
                    counts.AppendChild(equationcount);
                    equationcount.SetAttribute("count", articulo.numFormulas + "");
                    XmlComment c2 = xml.CreateComment("Verificar valor equation-count");
                    counts.AppendChild(c2);    

                    XmlElement refcount = xml.CreateElement("ref-count");
                    counts.AppendChild(refcount);
                    refcount.SetAttribute("count", articulo.Referencias.Count+"");

                    XmlElement pagecount = xml.CreateElement("page-count");
                    counts.AppendChild(pagecount);
                    pagecount.SetAttribute("count", articulo.numPaginas+1+"");



                    
                        XmlElement body = xml.CreateElement("body");
                        article.AppendChild(body);
                        
                        
                        foreach (KeyValuePair<string, SeccionBody> tempSec in articulo.SectionsBody)
	                    {
	                        XmlElement sec = xml.CreateElement("sec");
                            if ( tempSec.Value.titulo.ToLower().Contains("intro") )
                                sec.SetAttribute("sec-type", "intro");
                            else if (tempSec.Value.titulo.ToLower().Contains("results") || tempSec.Value.titulo.ToLower().Contains("resultado") )
                                sec.SetAttribute("sec-type", "results");
                            else if (tempSec.Value.titulo.ToLower().Contains("conclusión") || tempSec.Value.titulo.ToLower().Contains("conclusion"))
                                sec.SetAttribute("sec-type", "conclusions");
                            else
                            {
                                //sec.SetAttribute("sec-type", "intro|methods|conclusions|results");
                                XmlComment comentario = xml.CreateComment("Agregar a  atributo sec-type, posibles valores: intro, methods, conclusions, results");
                                sec.AppendChild(comentario);    
                            }
                            body.AppendChild(sec);

                            XmlElement title = xml.CreateElement("title");
                            sec.AppendChild(title);
                            title.InnerText = tempSec.Value.titulo;

                            foreach( string tempp in tempSec.Value.parrafos() ){
                                XmlElement pp = xml.CreateElement("p");
                                sec.AppendChild(pp);
                                pp.InnerText = tempp;    
                            }

                            foreach (KeyValuePair<string, SeccionBody> subSecP in tempSec.Value.subsecciones)
                            {
                                XmlElement subsec = xml.CreateElement("sec");
                                sec.AppendChild(subsec);
                                XmlElement subtitle = xml.CreateElement("title");
                                subsec.AppendChild(subtitle);
                                subtitle.InnerText = subSecP.Value.titulo;

                                foreach (string tempp in subSecP.Value.parrafos())
                                {
                                    XmlElement pp = xml.CreateElement("p");
                                    subsec.AppendChild(pp);
                                    pp.InnerText = tempp;
                                }
                            }

	                    }



                        /*
                         *  BACK
                        */
                        XmlElement back = xml.CreateElement("back");
                        article.AppendChild(back);

                        //Agradecimientos
                        if (!String.IsNullOrEmpty(articulo.Agradecimientos))
                        {
                            XmlElement ack = xml.CreateElement("ack");
                            back.AppendChild(ack);

                            XmlElement titleA = xml.CreateElement("title");
                            ack.AppendChild(titleA);
                            titleA.InnerText = "Agradecimentos";

                            XmlElement pA = xml.CreateElement("p");
                            ack.AppendChild(pA);
                            pA.InnerText = articulo.Agradecimientos.Replace("||","  ").Replace("<br/>","  ");
                        }

                        XmlElement reflist = xml.CreateElement("ref-list");
                        back.AppendChild(reflist);
                        //referencias bibliograficas
                        int totalREf = articulo.Referencias.Count;

                        XmlElement titleRef = xml.CreateElement("title");
                        reflist.AppendChild(titleRef);
                        titleRef.InnerText = "Referência Bibliográfica";

                        int ss = 1;
                        foreach(Referencia refe in  articulo.Referencias ){

                            if (String.IsNullOrEmpty(refe.Original) || refe.Original.Equals("\a"))
                                continue;

                            XmlElement refer = xml.CreateElement("ref");
                            reflist.AppendChild(refer);
                            refer.SetAttribute( "id", "B"+ss );

                            XmlElement label = xml.CreateElement("label");
                            refer.AppendChild(label);
                            label.InnerText = ss+"";

                            string pattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);

                            XmlElement mixedcitation = xml.CreateElement("mixed-citation");
                            refer.AppendChild(mixedcitation);

                            if (!String.IsNullOrEmpty(r.Match(refe.Original).Value))
                            {
                                //mixedcitation.InnerText = refe.Original.Replace(r.Match(refe.Original).Value, "&lt;" + r.Match(refe.Original).Value + "/&gt;");
                                mixedcitation.InnerText = refe.Original.Replace("<","").Replace("/>","").Replace(">","");
                            }
                            else
                                mixedcitation.InnerText = refe.Original;

                            XmlElement elementcitation = xml.CreateElement("element-citation");
                            refer.AppendChild(elementcitation);
                            elementcitation.SetAttribute("publication-type", "other");
                            
                            
                            pattern = @"\b\d{4}\b";
                            r = new System.Text.RegularExpressions.Regex(pattern);
                            if (!string.IsNullOrEmpty(r.Match(refe.Original).Value ))
                            {
                                XmlElement year = xml.CreateElement("year");
                                elementcitation.AppendChild(year);
                                year.InnerText = r.Match(refe.Original).Value;
                            }

                            pattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
                            r = new System.Text.RegularExpressions.Regex(pattern);
                            if (!string.IsNullOrEmpty(r.Match(refe.Original).Value))
                            {
                                XmlElement extlink = xml.CreateElement("ext-link");
                                elementcitation.AppendChild(extlink);
                                extlink.SetAttribute("ext-link-type", "uri");
                                extlink.SetAttribute("xlink:href", r.Match(refe.Original).Value);
                                extlink.InnerText = r.Match(refe.Original).Value;
                            }
                            


                            ss += 1;
                        }

                        //notas
                        int totalNotas = articulo.notas.Count;
                        if (totalNotas > 0)
                        {
                            XmlElement fngroup = xml.CreateElement("fn-group");
                            back.AppendChild(fngroup);

                            string pattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
                            //(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);

                            for (int i = 0; i < totalNotas; i++)
                            {
                                XmlElement fn = xml.CreateElement("fn");
                                fngroup.AppendChild(fn);
                                fn.SetAttribute("fn-type","other");
                                fn.SetAttribute("id", "fn" + (i + 1));

                                XmlElement label = xml.CreateElement("label");
                                XmlElement p = xml.CreateElement("p");
                                fn.AppendChild(label);
                                fn.AppendChild(p);

                                label.InnerText = ""+(i + 1);

                                if (!string.IsNullOrEmpty(r.Match(articulo.notas[i]).Value))
                                {
                                    String link = "<ext-link ext-link-type=\"uri\" xlink:href=\"" + r.Match(articulo.notas[i]).Value + "\">" + r.Match(articulo.notas[i]).Value + "</ext-link>";
                                    articulo.notas[i] = articulo.notas[i].Replace(r.Match(articulo.notas[i]).Value, link);
                                }

                                p.InnerText = articulo.notas[i];

                            }
                        }
                    


                    try
                    {
                        //String temp = rutaContenido + "xml.xml";
                        String temp = rutaContenido + Path.GetFileNameWithoutExtension(rutaDocumento) + ".xml";
                        xml.Save(@temp);

                        //corregir un error de c#
                        var fileContents = System.IO.File.ReadAllText(temp);
                        fileContents = fileContents.Replace("<ext-link ext-link-type=\"uri\" href", "<ext-link ext-link-type=\"uri\" xlink:href");
                        fileContents = fileContents.Replace("href=\"https://creativecommons.org/licenses/by-nc-nd/4.0/deed.es\"", "xlink:href=\"https://creativecommons.org/licenses/by-nc-nd/4.0/deed.es\"");
                        
                        //de tablas
                        fileContents = fileContents.Replace("&lt;", "<").Replace("&gt;", ">");
                        fileContents = fileContents.Replace("<bold> QUOTE </bold>", "").Replace("QUOTE", "");
                        fileContents = fileContents.Replace("<p><list-item>", "<list-item>").Replace("<p><list list-type", "<list list-type")
                            .Replace("</list-item></p>", "</list-item>").Replace("<p></list>", "</list><p>");

                        foreach (KeyValuePair<string, string> r in reemplazos)
                            fileContents = fileContents.Replace(r.Key, r.Value);

                        System.IO.File.WriteAllText(temp, fileContents);
                    }
                    catch (Exception e)
                    {
                        mensaje = "Error: Error al guardar archivo XML: " + rutaContenido + Path.GetFileNameWithoutExtension(rutaDocumento) + ".xml";
                        System.Console.Error.WriteLine("exception: {0}", e);
                        return false;
                    }

                }catch(Exception e){
                     mensaje = "Error: No es posible generar estructura de archivo XML";
                    return false;
                }

            }
            else
            {
                mensaje = "Error: No es posible generar XML por que no se ha procesado documento";
                return false;
            }
            return true;
        }


        private String extreaAnio(  String cadena)
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

        private String extraeFecha(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
                return null;

            Regex rgx = new Regex(@"\d{2}-\d{2}-\d{4}");
            Match mat = rgx.Match(cadena);
            if( !String.IsNullOrEmpty(mat.ToString()) )
                return mat.ToString();

            rgx = new Regex(@"\d{2}/\d{2}/\d{4}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();

            rgx = new Regex(@"\d{2}.\d{2}.\d{4}");
            mat = rgx.Match(cadena);
            if (!String.IsNullOrEmpty(mat.ToString()))
                return mat.ToString();

            return null;
        }

        public string extraeNumero(string original)
        {
            return new string(original.Where(c => Char.IsDigit(c)).ToArray());
        }
    }


}



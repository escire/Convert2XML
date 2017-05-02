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
//using Newtonsoft.Json.Linq;

namespace jats
{
    public class ProcesaDocumento2016 : ProcesaDocumento
    {
        public ProcesaDocumento2016(String ruta, String rc, Dictionary<string, string> pxml) : base(ruta, rc, pxml)
        {
            log = "";
        }

        public override Boolean procesa()
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

                    Boolean anexos = false;
                    String ultimoAnexo = null;
                    String ultimoSubAnexo = null;
                    int parraAnexo = 0;

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

                    String paginas = parametrosXML["page"];
                    if (!String.IsNullOrEmpty(paginas))
                    {
                        paginas = paginas.Trim();
                        if (paginas.Contains("-"))
                        {
                            articulo.PaginaI = "" + paginas.Substring(0, paginas.IndexOf("-"));
                            if ((paginas.IndexOf("-") + 1) < paginas.Length)
                                articulo.PaginaF = "" + paginas.Substring(paginas.IndexOf("-") + 1);
                            else
                                articulo.PaginaF = articulo.PaginaI;
                        }
                        else
                        {
                            articulo.PaginaI = "" + paginas;
                            articulo.PaginaF = "" + paginas;
                        }
                    }
                    else
                    {
                        articulo.PaginaI = "1";
                        articulo.PaginaF = "" + articulo.numPaginas;
                    }


                    int j = 1;
                    //buscar DOI
                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];

                        if (!Tools.texto(parrafo).Equals(""))
                        {
                            if ( Tools.alineacion(parrafo, "wdAlignParagraphRight") && Tools.contine(Tools.texto(parrafo), "/", true) && Tools.tamano(parrafo, 12)
                                && !Tools.negrita(parrafo) && !Tools.cursiva(parrafo) )
                            {
                                articulo.Doi = Tools.texto(parrafo);
                                break;
                            } // se encontro otra cosa (posiblemente seccion)
                            else
                                break;
                        }
                    }
                    j += 1;
                    if (articulo.Doi == null)
                    {
                        j = 1;
                        articulo.Doi = null;
                    }
                    //buscar Seccion
                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        if (!Tools.texto(doc.Paragraphs[i]).Equals(""))
                        {
                            if (Tools.alineacion(parrafo, "wdAlignParagraphRight") && Tools.tamano(parrafo, 12) && !Tools.negrita(parrafo) && !Tools.cursiva(parrafo) )
                            {
                                articulo.Seccion = Tools.texto(parrafo);
                                j = i + 1;
                                break;
                            }//se encontro algo diferente a la seccion
                            else
                            {
                                j = i;
                                break;
                            }
                        }
                    }
                    if (articulo.Seccion == null)
                    {
                        mensaje = "Error en formato de documento Word: No se encuetra Sección de la Revista";
                        doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                        doc = null;
                        word.Quit();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(word);
                        return false;
                    }

                    //buscar titulo
                    for (int i = j; i <= doc.Paragraphs.Count; i++, j++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        if (!Tools.vacio(parrafo))
                        {
                            if (Tools.negrita(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphCenter") && Tools.tamano(parrafo, 14) && Tools.negrita(parrafo)
                                && !Tools.cursiva(parrafo))
                            {
                                //articulo.Titulo = doc.Paragraphs[i].Range.Text.ToString();
                                articulo.Titulo = formateaTexto(parrafo.Range, articulo, "", doc.Paragraphs[i + 1]);
                                j = i + 1;
                                break;
                            }
                            else // se encontro algo diferente al titulo
                            {
                                j = i;
                                break;
                            }
                        }
                    }
                    if (articulo.Titulo == null)
                    {
                        mensaje = "Error en formato de documento Word: No se encuetra Titulo principal del artículo";
                        doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                        doc = null;
                        word.Quit();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(word);
                        return false;
                    }


                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        if (!Tools.vacio(parrafo)) //buscar  traducción del título
                        {
                            if (Tools.negrita(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphCenter") && Tools.tamano(parrafo, 14) && Tools.negrita(parrafo)
                                && !Tools.cursiva(parrafo))
                            {
                                articulo.Title = Tools.texto(parrafo);
                                j = i + 1;
                                break;
                            }
                            else // se encontro algo diferente title
                            {
                                j = i;
                                break;
                            }
                        }

                    }
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        //buscar autores
                        if (!Tools.vacio(parrafo))
                        {
                            if (Regex.IsMatch(Tools.texto(parrafo), @"^\d+"))  // inicia con número, puede ser afiliación
                            {
                                j = i;
                                break;
                            }
                            if (Tools.alineacion(parrafo, "wdAlignParagraphLeft") && Tools.tamano(parrafo, 12) && !Tools.negrita(parrafo) && !Tools.cursiva(parrafo))
                            {
                                if (Tools.texto(parrafo).StartsWith("*"))
                                {
                                    articulo.Autorcorres = Tools.texto(parrafo);
                                    continue;
                                }else if (Tools.texto(parrafo).Contains("*"))
                                {
                                    articulo.AutorcorresNombre = Tools.texto(parrafo).Replace("*", "");
                                    articulo.AutorcorresNombre = Regex.Replace(articulo.AutorcorresNombre, @"[\d-]", " ");
                                }
                                
                                String nombre = "", apellido = "";

                                if (Tools.texto(parrafo).Split(new char[0]).Length == 1) // el autor no tiene apellidos
                                {
                                    nombre = Tools.texto(parrafo);
                                    apellido = "NO SE ENCONTRO APELLIDO";
                                }
                                else if (Tools.texto(parrafo).Split(new char[0]).Length >= 2)
                                {
                                    if (!Tools.texto(parrafo).Contains("-"))
                                    {
                                        nombre = Tools.texto(parrafo).Split(new char[0])[0];
                                        apellido = Tools.texto(parrafo).Split(new char[0])[1];
                                    }
                                    else
                                    {
                                        foreach (String pal in Tools.texto(parrafo).Split(new char[0])) // dividir nombre en espacios
                                        {
                                            if (pal.Contains("-"))
                                                apellido += " " + pal;
                                            else nombre += " " + pal;
                                        }
                                    }
                                }

                                String afiliacion = System.Text.RegularExpressions.Regex.Match(Tools.texto(parrafo), @"\d+").Value;
                                //era apellido

                                if (!String.IsNullOrEmpty(afiliacion))
                                    apellido = apellido.Trim().Replace(afiliacion, "");

                                articulo.addAutores(nombre.Trim(), apellido.Trim(), afiliacion);
                            }
                            else // se encontro algo diferente autores
                            {
                                j = i;
                                goto EndAutores;
                            }
                        }
                    }
                    EndAutores: ;
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];//buscar afiliaciones (inicien en número y esten en tamaño 12)
                        if (!Tools.vacio(parrafo))
                        {
                            Boolean empiezaNumero = Regex.IsMatch(Tools.texto(parrafo), @"^\d+");
                            if ( empiezaNumero && Tools.tamano(parrafo, 12) 
                                && Tools.alineacion(parrafo, "wdAlignParagraphLeft") && !Tools.negrita(parrafo) && !Tools.cursiva(parrafo))
                            {
                                String afiliacion = Tools.texto(parrafo).Replace(">", "").Replace("<", "");

                                String email = Tools.getEmail(afiliacion);

                                String id = new String(afiliacion.TakeWhile(Char.IsDigit).ToArray());
                                if (id != null && !id.Trim().Equals(""))
                                {
                                    if (afiliacion.Contains(",")) // o punto
                                    {
                                        String nombre = afiliacion.Substring(afiliacion.IndexOf(id) + id.Length, afiliacion.IndexOf(",") - 1); // o punto
                                        if (!String.IsNullOrEmpty(nombre))
                                        {
                                            Tools.consultaWayta(nombre, articulo, id, afiliacion, email);
                                        }
                                    }
                                    else
                                        articulo.addAfiliaciones(id, afiliacion);

                                }

                            }
                            else  // se encontro algo diferente afiliacion
                            {
                                j = i;
                                goto endAfiliacion;
                            }
                        }
                    }
                    endAfiliacion: ;
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        if (!Tools.vacio(parrafo))
                        {
                            //buscar autor de correspondencia
                            if (!Tools.negrita(parrafo) && !Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                                 && Tools.tamano(parrafo, 12) && !String.IsNullOrEmpty(Tools.getEmail(Tools.texto(parrafo))))
                            {
                                String autorcorres = Tools.texto(parrafo);
                                articulo.Autorcorres = Tools.getEmail(Tools.texto(parrafo));
                                articulo.AutorcorresNombre = autorcorres;

                            }
                            else  // se encontro algo diferente a autor de correspondencia
                            {
                                j = i;
                                break;
                            }
                        }
                    }
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        //buscar Resumen:
                        if (    Tools.negrita(parrafo)      && !Tools.cursiva(parrafo)      && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                                && !Tools.vacio(parrafo)    && Tools.tamano(parrafo, 12)
                                && Tools.verificaContenido(parrafo, "sinonimos.resumen")
                            )
                        {

                            int x = i + 1;
                            //articulo.Resumen = "<p><b>" + texto(doc.Paragraphs[i]) + "</b></p>";
                            articulo.Resumen = "";
                            for (; x <= doc.Paragraphs.Count; x++)
                            {   //buscar parrafo de resumen
                                if (!Tools.cursiva(doc.Paragraphs[x]) && Tools.alineacion(doc.Paragraphs[x], "wdAlignParagraphJustify")
                                    && !Tools.vacio(doc.Paragraphs[x]) && Tools.tamano(doc.Paragraphs[x], 12)
                                    && !Tools.verificaContenido(doc.Paragraphs[x], "sinonimos.palabrasclave"))
                                {
                                    String temp = formateaTexto(doc.Paragraphs[x], articulo, "", x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null);
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
                                else if (!Tools.cursiva(doc.Paragraphs[x]) && Tools.alineacion(doc.Paragraphs[x], "wdAlignParagraphLeft")
                                    && !Tools.vacio(doc.Paragraphs[x]) && Tools.tamano(doc.Paragraphs[x], 12)
                                    && Tools.verificaContenido(doc.Paragraphs[x], "sinonimos.palabrasclave") )
                                {
                                    i = x; j = x;
                                    goto endResumen;
                                }
                            }

                        }
                        else if (!Tools.vacio(doc.Paragraphs[i])) // se encontro algo diferente a resumen
                        {
                            j = i;
                            goto endResumen;
                        }
                    }
                    endResumen: ;
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        //buscar palabras clave
                        if (!Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                                    && !Tools.vacio(parrafo) && Tools.tamano(parrafo, 12)
                                    && Tools.verificaContenido(parrafo, "sinonimos.palabrasclave"))
                        {
                            String pc = Tools.texto(parrafo);
                            pc = pc.Substring(pc.IndexOf(":") + 1);
                            foreach (String pal in pc.Split(','))
                            {
                                foreach (String pal2 in pal.Trim().Split(';'))
                                    articulo.addPalabrasclave(pal2.Trim());
                            }
                            j = i;
                        }
                        else if (!Tools.vacio(parrafo)) // se encontro algo diferente a palabras clave
                        {
                            j = i;
                            break;
                        }
                    }
                    for (int i = j; i <= doc.Paragraphs.Count; i++) //buscar abstract
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        if (Tools.negrita(parrafo) && !Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                                && !Tools.vacio(parrafo) && Tools.tamano(parrafo, 12)
                                && Tools.verificaContenido(parrafo, "sinonimos.abstract")
                            )
                        {
                            int x = i + 1;
                            //articulo.Abstractt = "<p><b>" + texto(doc.Paragraphs[i]) + "</b></p>";
                            articulo.Abstractt = "";
                            for (; x <= doc.Paragraphs.Count; x++)
                            {   //buscar parrafo de abstract 
                                if (!Tools.cursiva(doc.Paragraphs[x]) && Tools.alineacion(doc.Paragraphs[x], "wdAlignParagraphJustify")
                                   && Tools.tipoletra(doc.Paragraphs[x], "Times New Roman") && !Tools.vacio(doc.Paragraphs[x]) && Tools.tamano(doc.Paragraphs[x], 12)
                                   && !(Tools.verificaContenido(parrafo, "sinonimos.keywords"))
                                   )
                                {
                                    String temp = formateaTexto(doc.Paragraphs[x], articulo, "", (x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null));
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
                                            articulo.Abstractt += "<p>" + temp + "</p>";
                                        }
                                    }
                                }
                                //aca empiezan las key word
                                //else if (!Tools.vacio(doc.Paragraphs[x]))
                                else if (!Tools.cursiva(doc.Paragraphs[x]) && Tools.alineacion(doc.Paragraphs[x], "wdAlignParagraphLeft")
                                    && !Tools.vacio(doc.Paragraphs[x]) && Tools.tamano(doc.Paragraphs[x], 12)
                                    && Tools.verificaContenido(doc.Paragraphs[x], "sinonimos.keywords")
                                    )
                                {
                                    i = x; j = x;
                                    //break;
                                    goto EndAbstract;
                                }
                            }
                        }
                        else if (!Tools.vacio(parrafo)) // se encontro algo diferente a abstract
                        {
                            j = i;
                            break;
                        }
                    }
                    EndAbstract: ;
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        //buscar Keyword
                        if (Tools.negrita(parrafo) && !Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                            && Tools.tamano(parrafo, 12)
                            && Tools.verificaContenido(parrafo, "sinonimos.keywords")
                            && Tools.tamano(doc.Paragraphs[i], 12))
                        {
                            String kw = Tools.texto(parrafo);
                            kw = kw.Substring(kw.IndexOf(":") + 1);
                            foreach (String key in kw.Split(','))
                            {
                                foreach (String key2 in key.Trim().Split(';'))
                                    articulo.addKeywords(key2.Trim());
                            }
                            j = i;
                        }
                        else if (!Tools.vacio(parrafo)) // se encontro algo diferente a Keyword
                        {
                            j = i;
                            break;
                        }
                    }
                    for (int i = j; i <= doc.Paragraphs.Count; i++)
                    {
                        Paragraph parrafo = doc.Paragraphs[i];
                        //buscar fechas
                        if (!Tools.vacio(parrafo))
                        {
                            if (!Tools.negrita(parrafo) && !Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                            && Tools.tamano(doc.Paragraphs[i], 12) && Tools.verificaContenido(parrafo, "sinonimos.fechaRecibido"))
                            {
                                String fechare = Tools.extraeFecha(Tools.texto(doc.Paragraphs[i]));

                                if (String.IsNullOrEmpty(fechare))
                                    fechare = Tools.extreaAnio(Tools.texto(doc.Paragraphs[i]));
                                articulo.Fecharecibido = fechare;

                                j = i;
                            }
                            else if ( !Tools.negrita(parrafo) && !Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                                && Tools.tamano(doc.Paragraphs[i], 12) && Tools.verificaContenido(parrafo, "sinonimos.fechaRevisado"))
                            {
                                String fechare = Tools.extraeFecha(Tools.texto(doc.Paragraphs[i]));

                                if (String.IsNullOrEmpty(fechare))
                                    fechare = Tools.extreaAnio(Tools.texto(doc.Paragraphs[i]));

                                articulo.Fecharevisado = fechare;

                                j = i;
                            }
                            else if ( !Tools.negrita(parrafo) && !Tools.cursiva(parrafo) && Tools.alineacion(parrafo, "wdAlignParagraphLeft")
                                && Tools.tamano(doc.Paragraphs[i], 12) && Tools.verificaContenido(parrafo, "sinonimos.fechaAceptado"))
                            {
                                String fechare = Tools.extraeFecha(Tools.texto(doc.Paragraphs[i]));
                                if (String.IsNullOrEmpty(fechare))
                                    fechare = Tools.extreaAnio(Tools.texto(doc.Paragraphs[i]));
                                articulo.Fechaaceptado = fechare;
                                j = i;
                            }
                            else
                            {
                                j = i;
                                goto endFechas;
                            }
                        }
                    }
                    endFechas: ;
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

                        
                        String text = Tools.texto(doc.Paragraphs[i]);
                        // si hay titulo de la seccion -> traduccion
                        if (Tools.negrita(doc.Paragraphs[i]) && Tools.cursiva(doc.Paragraphs[i]) && Tools.subrayado((doc.Paragraphs[i])) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphRight")
                             && !Tools.texto(doc.Paragraphs[i]).Equals("") && Tools.tamano(doc.Paragraphs[i], 12))
                        {
                            hayTraduccion = true;
                            articuloTraduccion = new Articulo();
                            articuloTraduccion.Seccion = Tools.texto(doc.Paragraphs[i]);
                        } // es titulo traducción
                        if (Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i])
                            && Tools.subrayado(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                            && !Tools.vacio(doc.Paragraphs[i]) && Tools.tamano(doc.Paragraphs[i], 14)
                            )
                        {
                            if (articuloTraduccion == null)
                                articuloTraduccion = new Articulo();
                            articuloTraduccion.Titulo = Tools.texto(doc.Paragraphs[i]);
                            hayTraduccion = true;
                        }
                        //resumen del subarticulo
                        if (!Tools.cursiva(doc.Paragraphs[i])
                                && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                                && !Tools.vacio(doc.Paragraphs[i])
                                && Tools.tamano(doc.Paragraphs[i], 12)
                                && hayTraduccion && Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.abstract"))
                        {
                            articuloTraduccion.Abstractt = Tools.texto(doc.Paragraphs[i]);
                            for (int x = i; x <= doc.Paragraphs.Count; x++)
                            {   //buscar parrafo de resumen
                                if (!Tools.cursiva(doc.Paragraphs[x]) && Tools.alineacion(doc.Paragraphs[x], "wdAlignParagraphJustify")
                                   && !Tools.vacio(doc.Paragraphs[x]) && Tools.tamano(doc.Paragraphs[x], 12))
                                {
                                    String temp = formateaTexto(doc.Paragraphs[x], articulo, "", x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null);
                                    if (!String.IsNullOrEmpty(temp))
                                    {
                                        if (temp.Contains("<bold>"))
                                        {
                                            articuloTraduccion.Abstractt += "<sec>";
                                            articuloTraduccion.Abstractt += formateaTexto(doc.Paragraphs[x], articulo, "", (x + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[x + 1] : null))
                                                .Replace("<bold>", "<title>").Replace("</bold>", "</title><p>");
                                            articuloTraduccion.Abstractt += "</p></sec>";
                                        }
                                        else
                                        {
                                            articuloTraduccion.Abstractt += "<p>" + temp + "</p>";
                                        }
                                    }
                                }
                                //aca empiezan las palabras clave del subarticuo
                                else if (!Tools.cursiva(doc.Paragraphs[x]) && Tools.alineacion(doc.Paragraphs[x], "wdAlignParagraphLeft")
                                    && !Tools.vacio(doc.Paragraphs[x]) && Tools.tamano(doc.Paragraphs[x], 12)
                                    && hayTraduccion && Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.keywords"))
                                {
                                    x -= 1;
                                    i = x;
                                    break;
                                }
                            }
                        }
                        if (Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft")
                            && Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.keywords")
                            && Tools.tamano(doc.Paragraphs[i], 12))
                        {
                            String kw = Tools.texto(doc.Paragraphs[i]);
                            kw = kw.Substring(kw.IndexOf(":") + 1);
                            foreach (String key in kw.Split(','))
                            {
                                foreach (String key2 in key.Trim().Split(';'))
                                {
                                    if( articuloTraduccion == null )
                                        articulo.addKeywords(key2.Trim());
                                    else
                                        articuloTraduccion.addKeywords(key2.Trim());
                                }
                                
                                    
                            }
                            j = i;
                        }
                        if (
                                /*
                                (
                                    Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphJustify")
                                    && Tools.tamano(doc.Paragraphs[i], 14)
                                    && Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.conflictos")
                                )
                                ||*/
                                (
                                    Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                                    && Tools.tamano(doc.Paragraphs[i], 12)
                                    && Tools.verificaContenido(doc.Paragraphs[i],"sinonimos.agradecimientos")
                                )
                                ||
                                (
                                    Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphJustify")
                                    && Tools.tamano(doc.Paragraphs[i], 12)
                                    && Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.referencias")
                                )
                            )
                        {
                            parrafoFinal = i;
                            goto endCuerpo;
                        }
                        else // guardar secciones
                        {
                            //ver si es el titulo de una seccion
                            if (Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                                && Tools.tamano(doc.Paragraphs[i], 16)
                            )
                            {
                                if (!hayTraduccion)
                                    articulo.SectionsBody.Add(Tools.texto(doc.Paragraphs[i]), new SeccionBody(Tools.texto(doc.Paragraphs[i])));
                                else
                                    articuloTraduccion.SectionsBody.Add(Tools.texto(doc.Paragraphs[i]), new SeccionBody(Tools.texto(doc.Paragraphs[i])));

                                ultimoSeccionBody = Tools.texto(doc.Paragraphs[i]);

                                if (!hayTraduccion)
                                    articulo.addSeccion(Tools.texto(doc.Paragraphs[i]));
                                else
                                    articuloTraduccion.addSeccion(Tools.texto(doc.Paragraphs[i]));

                                ultimoSubSeccionBody = null;
                            }
                            else if (Tools.negrita(doc.Paragraphs[i]) && !Tools.cursiva(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                                        && Tools.tamano(doc.Paragraphs[i], 14)
                           )
                            {
                                if (!hayTraduccion && (ultimoSeccionBody == null || articulo.SectionsBody.Count == 0))
                                    continue;
                                else if (hayTraduccion && (ultimoSeccionBody == null || articuloTraduccion.SectionsBody.Count == 0))
                                    continue;

                                SeccionBody temp = null;

                                if (!hayTraduccion)
                                    temp = articulo.SectionsBody[ultimoSeccionBody];
                                else
                                    temp = articuloTraduccion.SectionsBody[ultimoSeccionBody];

                                temp.subsecciones.Add(Tools.texto(doc.Paragraphs[i]), new SeccionBody(Tools.texto(doc.Paragraphs[i])));

                                ultimoSubSeccionBody = Tools.texto(doc.Paragraphs[i]);
                            }
                            else
                            { // es el cuerpo de la sección

                                if (!hayTraduccion && (ultimoSeccionBody == null || articulo.SectionsBody.Count == 0))
                                    continue;
                                else if (hayTraduccion && (ultimoSeccionBody == null || articuloTraduccion.SectionsBody.Count == 0))
                                    continue;

                                SeccionBody temp = null;
                                if (!hayTraduccion)
                                    temp = articulo.SectionsBody[ultimoSeccionBody];
                                else
                                    temp = articuloTraduccion.SectionsBody[ultimoSeccionBody];

                                //String textoParrafo = texto(doc.Paragraphs[i]);
                                String textoParrafo = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));

                                if (i + 1 < doc.Paragraphs.Count)
                                {
                                    String tmp = Tools.texto(doc.Paragraphs[i + 1]);
                                    if (textoParrafo.Contains("<list-item>") && cerrarlista && String.IsNullOrEmpty(tmp))
                                    {
                                        textoParrafo = textoParrafo + "</list>";
                                        cerrarlista = false;
                                        abrirlista = false;
                                    }
                                }

                                if (abrirlista && !cerrarlista)
                                {
                                    textoParrafo = "<list list-type=\"order\">" + textoParrafo;
                                    cerrarlista = true;
                                }

                                if (!abrirlista && cerrarlista)
                                {
                                    textoParrafo = "</list>" + textoParrafo;
                                    cerrarlista = false;
                                    abrirlista = false;
                                }


                                textoParrafo = textoParrafo.Replace("\v", " ").Replace(@"&#11;", " ").Replace(@"&#xb;", " ").Replace(@"\u000B", " ").Replace(((char)2) + "", "");

                                bInTable = false;
                                Microsoft.Office.Interop.Word.Range r = doc.Paragraphs[i].Range;

                                for (int xx = 0; xx < TablesRanges.Count; xx++)
                                {
                                    Microsoft.Office.Interop.Word.Range range = TablesRanges[xx];
                                    if (r.Start >= range.Start && r.Start <= range.End)
                                    {
                                        //temp.addParrafo("In Table - Paragraph number " + i.ToString() + ":" + r.Text);
                                        bInTable = true;
                                        String numeroTabla = xx + "";

                                        if (ultimatabla == xx)
                                            break;

                                        ultimatabla = xx;
                                        String label = "Tabla" + (numeroTabla + 1), title = "Tabla.";


                                        if (!String.IsNullOrEmpty(descipcionTable) && (descipcionTable.Contains(":") || descipcionTable.Contains(".")))
                                        {
                                            descipcionTable = descipcionTable.Replace("<bold>", "").Replace("</bold>", "");
                                            if( descipcionTable.Contains(":") )
                                                label = descipcionTable.Substring(0, descipcionTable.IndexOf(":"));
                                            else
                                                label = descipcionTable.Substring(0, descipcionTable.IndexOf("."));

                                            title = descipcionTable.Substring(descipcionTable.IndexOf(":") + 1);
                                            descipcionTable = null;
                                        }

                                        reemplazos.Add(label.ToLower() , "<xref ref-type=\"table\" rid=\"t" + numeroTabla + 1 + "\">" + label + "</xref>");

                                        String tabla = "<table-wrap id=\"t" + numeroTabla + 1 + "\">"
                                                            + "<label>" + label + "</label>"
                                                            + "<caption>"
                                                                + "<title>" + title + "</title>"
                                                            + "</caption>"
                                                            + "<table>";


                                        int filas = doc.Tables[xx + 1].Rows.Count;
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
                                                try
                                                {
                                                    celda = doc.Tables[xx + 1].Cell(yy, ww);
                                                }
                                                catch (Exception wp) { }

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
                                        for (; contador <= doc.Paragraphs.Count; contador++)
                                        {
                                            Microsoft.Office.Interop.Word.Range rangoTemp = doc.Paragraphs[contador].Range;
                                            if (rangoTemp.Start >= range.End)
                                            {
                                                if (Tools.alineacion(doc.Paragraphs[contador], "wdAlignParagraphCenter") && Tools.tamano(doc.Paragraphs[contador], 10) && !Tools.negrita(doc.Paragraphs[contador]))
                                                {
                                                    attrib = Tools.texto(doc.Paragraphs[contador]);
                                                    tabla = tabla.Replace("<attrib></attrib>", "<attrib>" + attrib + "</attrib>");
                                                    reemplazos.Add("<p>" + attrib + "</p>", "");
                                                    if (!reemplazos.Keys.Contains("(" + label.Trim().ToLower() + ")"))
                                                        reemplazos.Add(label.Trim().ToLower(), "<xref ref-type=\"table\" rid=\"t" + numeroTabla + 1 + "\">" + label.Trim().ToLower() + "</xref>");
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
                                    if (!hayTraduccion)
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
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(ultimoSeccionBody) && articuloTraduccion.SectionsBody.ContainsKey(ultimoSeccionBody) && !string.IsNullOrEmpty(textoParrafo))
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

                    }

                    endCuerpo: ;
                    int parrafoGlosario = doc.Paragraphs.Count;

                    int parrafoAgradecimiento = -1;

                    //buscar referencias
                    for (int i = parrafoFinal; i <= doc.Paragraphs.Count; i++)
                    {   //&& !Tools.cursiva(doc.Paragraphs[i])
                        if (!Tools.negrita(doc.Paragraphs[i])  && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphJustify")
                            && Tools.tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[i])) && i != parrafoAgradecimiento)
                        {
                            String refe = Tools.texto(doc.Paragraphs[i]);
                            Referencia referencia = new Referencia();
                            LinkReferencia linkref = new LinkReferencia();

                            referencia.Original = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));

                            var test = new Regex(@"\d+\.( )");
                            referencia.Original = test.Replace(referencia.Original, "", 1);

                            linkref.Apellido = referencia.Original.Substring(0, referencia.Original.IndexOf(" "));

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
                                            linkref.Anio = numero;
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
                            {
                                articulo.addReferencia(referencia);
                                articulo.addLinkReferencia(linkref);
                            }

                        }
                        else if (Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.agradecimientos") && Tools.negrita(doc.Paragraphs[i]) 
                                && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter")
                                && Tools.tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[i])))
                        {

                            for (int w = i + 1; w  <= doc.Paragraphs.Count; w++)
                            {
                                if (!String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[w])))
                                {
                                    String soyAgradecimiento = Tools.texto(doc.Paragraphs[w]);
                                    articulo.Agradecimientos += soyAgradecimiento + ".<br/>";
                                    parrafoAgradecimiento = w;
                                    break;
                                }
                            }
                        }
                        //buscar notas del autor
                        else if (Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphJustify")
                          && Tools.tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[i])))
                        {
                            if (parrafoAgradecimiento != i && !Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.referencias"))
                            {
                                String nota = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));
                                articulo.notasAutor.Add(nota);
                            }
                        }//buscar anexos, inician en tamano 16, centrado y times new
                            //anexos
                        else if (Tools.negrita(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter") 
                         && Tools.tamano(doc.Paragraphs[i], 16) && !String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[i])))
                        {
                            anexos = true;
                            parraAnexo = i + 1;
                            articulo.apendice.SectionsBody.Add("anexo", new SeccionBody(Tools.texto(doc.Paragraphs[i])));
                            ultimoAnexo = null;
                            break;
                        }
                        else if (Tools.negrita(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphLeft") && Tools.tipoletra(doc.Paragraphs[i], "Times New Roman")
                             && Tools.tamano(doc.Paragraphs[i], 16) && Tools.verificaContenido(doc.Paragraphs[i], "sinonimos.listaDefinicion"))
                        {
                            parrafoGlosario = i;
                        }
                    }

                    //buscar Anexos
                    for (int i = parraAnexo; i <= doc.Paragraphs.Count && parraAnexo != 0; i++)
                    {
                        // Seccion del anexo
                        if (Tools.negrita(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter") 
                                        && Tools.tamano(doc.Paragraphs[i], 14) && !String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[i])))
                        {
                            ultimoSubAnexo = null;
                            ultimoAnexo = Tools.texto(doc.Paragraphs[i]);
                            articulo.apendice.SectionsBody["anexo"].subsecciones.Add(ultimoAnexo, new SeccionBody(ultimoAnexo));
                        }// sub-Seccion del anexo
                        else if (Tools.negrita(doc.Paragraphs[i]) && Tools.alineacion(doc.Paragraphs[i], "wdAlignParagraphCenter") 
                                            && Tools.tamano(doc.Paragraphs[i], 13) && !String.IsNullOrEmpty(Tools.texto(doc.Paragraphs[i])))
                        {
                            ultimoSubAnexo = Tools.texto(doc.Paragraphs[i]);
                            articulo.apendice.SectionsBody["anexo"].subsecciones[ultimoAnexo].subsecciones.Add(ultimoSubAnexo, new SeccionBody(ultimoSubAnexo));
                        }    //cuerpo de la seccion
                        else
                        {
                            //else if (!negrita(doc.Paragraphs[i]) && alineacion(doc.Paragraphs[i], "wdAlignParagraphJustify") && tipoletra(doc.Paragraphs[i], "Times New Roman")
                            //  && tamano(doc.Paragraphs[i], 12) && !String.IsNullOrEmpty(texto(doc.Paragraphs[i])))
                            //{
                            String textoParrafo = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));
                            if (!String.IsNullOrEmpty(textoParrafo))
                            {
                                if( i+1 < doc.Paragraphs.Count ){
                                    String tmp = Tools.texto(doc.Paragraphs[i + 1]);
                                    if (textoParrafo.Contains("<list-item>") && cerrarlista && String.IsNullOrEmpty(tmp))
                                    {
                                        textoParrafo = textoParrafo + "</list>";
                                        cerrarlista = false;
                                        abrirlista = false;
                                    }
                                }

                                if (abrirlista && !cerrarlista)
                                {
                                    textoParrafo = "<list list-type=\"order\">" + textoParrafo;
                                    cerrarlista = true;
                                }
                                if (!abrirlista && cerrarlista)
                                {
                                    textoParrafo = "</list>" + textoParrafo;
                                    cerrarlista = false;
                                    abrirlista = false;
                                }
                                textoParrafo = textoParrafo.Replace("\v", " ").Replace(@"&#11;", " ").Replace(@"&#xb;", " ").Replace(@"\u000B", " ").Replace(((char)2) + "", "");

                                if (String.IsNullOrEmpty(ultimoAnexo) && String.IsNullOrEmpty(ultimoSubAnexo))
                                {
                                    articulo.apendice.SectionsBody["anexo"].addParrafo(textoParrafo);
                                }
                                else if (!String.IsNullOrEmpty(ultimoAnexo) && String.IsNullOrEmpty(ultimoSubAnexo))
                                {
                                    articulo.apendice.SectionsBody["anexo"].subsecciones[ultimoAnexo].addParrafo(textoParrafo);
                                }
                                else if (!String.IsNullOrEmpty(ultimoAnexo) && !String.IsNullOrEmpty(ultimoSubAnexo))
                                {
                                    articulo.apendice.SectionsBody["anexo"].subsecciones[ultimoAnexo].subsecciones[ultimoSubAnexo].addParrafo(textoParrafo);
                                }
                            }
                        }
                    }

                    //buscar glosario
                    for (int i = parrafoGlosario + 1; i <= doc.Paragraphs.Count; i++)
                    {
                        articulo.glosario.nombre = formateaTexto(doc.Paragraphs[parrafoGlosario], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));
                        String renglon = formateaTexto(doc.Paragraphs[i], articulo, rutaImagenes, (i + 1 <= doc.Paragraphs.Count ? doc.Paragraphs[i + 1] : null));
                        if (renglon.Contains("</bold>") && renglon.Contains("-"))
                        {
                            String llave = renglon.Substring(0, renglon.IndexOf("</bold>") + 7);
                            if (llave.EndsWith("-</bold>"))
                                llave = llave.Replace("-</bold>", "</bold>");

                            String men = renglon.Substring(renglon.IndexOf("</bold>") + 7);
                            articulo.glosario.contenido.Add(llave, men);
                        }

                    }



                    log += "<br/>>>>>>>Parrafo inicial y final: " + parrafoinicial + "<<<<>>>>" + parrafoFinal + "<<<<<";



                    doc.Close(WdSaveOptions.wdDoNotSaveChanges, ref miss, ref miss);
                    doc = null;
                    word.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(word);
                    GC.Collect();


                    GenerarXML generarXML = new GenerarXML(rutaDocumento, (rutaContenido + ""), parametrosXML, reemplazos, articulo, hayTraduccion, articuloTraduccion);
                    if (!generarXML.generaXMLV4())
                        mensaje = generarXML.getMensaje();

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
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(word);
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


        public override String formateaTexto(Range rango, Articulo articulo, String rutaGuardar, Microsoft.Office.Interop.Word.Paragraph parrafoSiguiente)
        {

            try
            {
                if (rango.XML == null) return "";
            }
            catch (Exception we)
            {
                return "";
            }
                


            try
            {
                
                String xml = rango.XML;
                String st = "";
                Boolean justificado = false;
                Boolean tamanomenor12 = false;
                Boolean sangria = false;
                Boolean cuadroTexto = false;

                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    reader.ReadToFollowing("wx:sect");

                    XmlReader inner = reader.ReadSubtree();
                    String seccion = inner.ReadInnerXml();


                    try
                    {
                        if (xml.Contains("wx:sect"))
                        {
                            String temporal = xml.Substring(xml.IndexOf("wx:sect"));
                            if (temporal.Contains("w:ind"))
                                sangria = true;

                            if (temporal.Contains("v:textbox"))
                                cuadroTexto = true;
                        }
                    }
                    catch (Exception er) { }



                    if (xml.Contains("w:jc w:val=\"both\""))
                        justificado = true;

                    if (xml.Contains("w:listPr"))
                    {
                        eslista = true;
                        sangria = false;
                    }

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

                                if (NombreNodo.Equals("w:vertAlign") && type.Equals("Element"))
                                    alineacionVertical = wt.GetAttribute("w:val");

                                if (NombreNodo.Equals("w:sz-cs") && type.Equals("Element"))
                                    if (wt.GetAttribute("w:val").Equals("24"))
                                        tamano12 = true;

                                if (NombreNodo.Equals("w:sz-cs") && type.Equals("Element"))
                                    if (wt.GetAttribute("w:val").Equals("23") || wt.GetAttribute("w:val").Equals("22") || wt.GetAttribute("w:val").Equals("21") || wt.GetAttribute("w:val").Equals("20"))
                                        tamanomenor12 = true;

                                if (NombreNodo.Equals("w:jc") && type.Equals("Element"))
                                    if (wt.GetAttribute("w:val").Equals("center"))
                                        center = true;

                                if (NombreNodo.Equals("w:underline") && type.Equals("Element"))
                                    underline = true;

                                if (NombreNodo.Equals("w:u") && type.Equals("Element"))
                                    underline = true;

                                if (NombreNodo.Equals("w:b") && type.Equals("Element"))
                                    bold = true;

                                if (NombreNodo.Equals("w:i") && type.Equals("Element"))
                                    italic = true;

                                
                                if (!String.IsNullOrEmpty(cadena)  && bold && (cadena.Trim().EndsWith(":") || cadena.Trim().EndsWith(".") )
                                    && !footnote && Tools.verificaContenido(cadena, "sinonimos.imagen"))
                                {
                                    if (cadena.ToLower().Contains("table")) break;
                                    if (cadena.ToLower().Contains("tabla")) break;

                                    Regex regex = new Regex(@"\ \d+(\d+)?\:");
                                    Match m = regex.Match(cadena);
                                    if (m.Success)
                                        anteriorDescipcionImagen = true;
                                    else
                                    {
                                        regex = new Regex(@"\ \d+(\d+)?\.");
                                        m = regex.Match(cadena);
                                        if (m.Success)
                                            anteriorDescipcionImagen = true;
                                    }
                                }



                                if (!String.IsNullOrEmpty(cadena) && bold && ((cadena.Trim().EndsWith(":")  || cadena.Trim().EndsWith(".") ))
                                    && !footnote && Tools.verificaContenido(cadena, "sinonimos.tabla"))
                                {
                                    Regex regex = new Regex(@"\ \d+(\d+)?\:");
                                    Match m = regex.Match(cadena);
                                    if (m.Success)
                                        anteriorDescipcionTable = true;
                                    else
                                    {
                                        regex = new Regex(@"\ \d+(\d+)?\.");
                                        m = regex.Match(cadena);
                                        if (m.Success)
                                            anteriorDescipcionTable = true;
                                    }
                                }



                                if (NombreNodo.StartsWith("w:footnote") && (type.Equals("Element") || type.Equals("EndElement")))
                                    footnote = true;

                                if (NombreNodo.Equals("w:binData") && type.Equals("Element"))
                                {
                                    binData = true;
                                    if (String.IsNullOrEmpty(descipcionImagen))
                                        nombreArchivo = Path.GetFileNameWithoutExtension(@rutaDocumento) + "-gf" +
                                        ((articulo.numImagenes + articulo.numFormulas + 1) + "").PadLeft(5, '0') + wt.GetAttribute("w:name").Replace("wordml://", "");
                                    else
                                        nombreArchivo = Path.GetFileNameWithoutExtension(@rutaDocumento) + "-i" +
                                       ((articulo.numImagenes + articulo.numFormulas + 1) + "").PadLeft(5, '0') + wt.GetAttribute("w:name").Replace("wordml://", "");
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
                                    {

                                        // validar si contiene formula
                                        String formula = Tools.extraeMathML(rango, articulo, true);

                                        if (!formula.Contains("mml:math"))
                                        {
                                            cadena = "<inline-graphic xlink:href=\"" + nombreArchivo + "\"/>";
                                            articulo.numImagenes += 1;
                                        }
                                        else
                                        {
                                            cadena = formula;
                                            articulo.numFormulas += 1;
                                        }

                                    }
                                    else
                                    {

                                        String attrib = null;
                                        if (parrafoSiguiente != null)
                                        {
                                            if ((Tools.alineacion(parrafoSiguiente, "wdAlignParagraphCenter") || Tools.alineacion(parrafoSiguiente, "1")) && Tools.tamano(parrafoSiguiente, 12) && !Tools.negrita(parrafoSiguiente))
                                            {
                                                attrib = Tools.texto(parrafoSiguiente);
                                                if (!reemplazos.ContainsKey("<p>" + attrib + "</p>"))
                                                    reemplazos.Add("<p>" + attrib + "</p>", "");
                                            }
                                        }


                                        String label = "Figura ESCRIBA No. FIGURA", titulo = "ESCRIBA TITULO";
                                        if (!String.IsNullOrEmpty(descipcionImagen)) // se encontro descripcion para la imagen, se asume que no es formula
                                        {
                                            articulo.numImagenes += 1;
                                            if (descipcionImagen.Contains(":") || descipcionImagen.Contains("."))
                                            {
                                                descipcionImagen = descipcionImagen.Replace("<bold>", "").Replace("</bold>", "");
                                                
                                                if( descipcionImagen.Contains(":") )
                                                    label = descipcionImagen.Substring(0, descipcionImagen.IndexOf(":"));
                                                else
                                                    label = descipcionImagen.Substring(0, descipcionImagen.IndexOf("."));
                                                
                                                reemplazos.Add(label.Trim().ToLower()   , "<xref ref-type=\"fig\" rid=\"f" + articulo.numImagenes + "\">" + label.Trim().ToLower() + "</xref>");
                                                reemplazos.Add(" "+label.Trim()+" "     , " <xref ref-type=\"fig\" rid=\"f" + articulo.numImagenes + "\">" + label.Trim() + "</xref> ");
                                                reemplazos.Add(" " + label.Trim() + "," , " <xref ref-type=\"fig\" rid=\"f" + articulo.numImagenes + "\">" + label.Trim().ToLower() + "</xref>,");
                                                reemplazos.Add("(" + label.Trim() + ")" , "(<xref ref-type=\"fig\" rid=\"f" + articulo.numImagenes + "\">" + label.Trim().ToLower() + "</xref>)");
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

                                            // validar si contiene formula
                                            String formula = Tools.extraeMathML(rango, articulo, false);

                                            if (!formula.Contains("mml:math"))
                                            {
                                                articulo.numImagenes += 1;
                                                cadena = "\n<graphic id=\"g" + articulo.numImagenes + "\" xlink:href=\"" + nombreArchivo + "\"/>";
                                            }
                                            else
                                            {
                                                cadena = "\n" + formula;
                                                articulo.numFormulas += 1;
                                            }
                                        }
                                    }
                                    nombreArchivo = "";
                                    binData = false;
                                }
                                if ( /*!String.IsNullOrEmpty(alineacionVertical) && alineacionVertical.Equals("superscript") &&*/ footnote)
                                {
                                    if (!articulo.notas.Contains(cadena))
                                    {
                                        articulo.notas.Add(cadena);
                                        cadena = "<xref ref-type=\"fn\" rid=\"fn" + articulo.notas.Count + "\"><sup>" + articulo.notas.Count + "</sup></xref>";
                                        footnote = false;
                                    }
                                }

                                if (!String.IsNullOrEmpty(alineacionVertical) && alineacionVertical.Equals("superscript") && !footnote)
                                {
                                    var mg = Regex.Match(cadena, @"\((\d+)\)");
                                    if (mg.Success)
                                    {
                                        String numero = Tools.extraeNumero(cadena);
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
                    if (abrirlista && cerrarlista) //debemos cerrar el tag lista
                        abrirlista = false;
                }

                if (sangria)
                    st = "<disp-quote>" + st + "</disp-quote>";

                if (cuadroTexto)
                {
                    articulo.numCuadrosTexto += 1;
                    st = "<boxed-text id=\"bx" + articulo.numCuadrosTexto + "\"><p>" + st + "</p></boxed-text>";
                }


                return st;
            }
            catch (Exception e)
            {
                return "";
            }


        }

        public override String formateaTexto(Microsoft.Office.Interop.Word.Paragraph parrafo, Articulo articulo, String rutaGuardar, Microsoft.Office.Interop.Word.Paragraph parrafoSiguiente)
        {
            if (parrafo == null || parrafo.Range == null || String.IsNullOrEmpty(parrafo.Range.Text))
                return "";

            try
            {
                if (!String.IsNullOrEmpty(parrafo.Range.XML))
                    if (parrafo.Range.XML.Contains("wx:sect"))
                    {
                        String temporal = parrafo.Range.XML.Substring(parrafo.Range.XML.IndexOf("wx:sect"));
                        if (temporal.Contains("v:textbox"))
                            return this.formateaTexto(parrafo.Range, articulo, rutaGuardar, parrafoSiguiente);

                    }
            }
            catch (Exception exp)
            {
                exp.ToString();
            }

            if (parrafo.Range.Text.Trim().Equals("\r") || parrafo.Range.Text.Trim().Equals("\n") || parrafo.Range.Text.Trim().Equals("\r\n")
                    || String.IsNullOrEmpty(parrafo.Range.Text.Trim()) || Environment.NewLine.Equals(parrafo.Range.Text.Trim())
                    || parrafo.Range.Text.Trim().Equals("\r\a"))
                return "";


            return this.formateaTexto(parrafo.Range, articulo, rutaGuardar, parrafoSiguiente);

        }

        
    
    }
}
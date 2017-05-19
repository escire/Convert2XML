using System.Windows;
using System;
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
using System.Collections.Generic;

namespace jats
{
    public class GenerarXML
    {
        Articulo articulo = null, articuloTraduccion = null;
        Dictionary<string, string> parametrosXML;
        Boolean hayTraduccion = false;
        String mensaje = "";
        String rutaDocumento, rutaContenido;
        Dictionary<string, string> reemplazos;

        public String getMensaje()
        {
            return mensaje;
        }



        public GenerarXML(String ruta, String rc, Dictionary<string, string> pxml, Dictionary<string, string> reemp, Articulo art, Boolean hayTradu, Articulo articuloTradu)
        {
            rutaDocumento = ruta;
            rutaContenido = rc;
            articulo = art;
            parametrosXML = pxml;
            reemplazos = reemp;
            hayTraduccion = hayTradu;
            articuloTraduccion = articuloTradu;
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
                    article.SetAttribute("specific-use", "sps-1.4");
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
                    journalid2.InnerText = "tl";

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

                        Boolean isCorres = (aut.SurName.Contains("*") || aut.FirstName.Contains("*"));

                        surname.InnerText = aut.SurName.Replace('*', ' ').Replace(";", " ").Trim();
                        givennames.InnerText = aut.FirstName.Replace('*', ' ').Replace(";", " ").Trim();

                        if (!String.IsNullOrEmpty(aut.Afiliacion))
                        {
                            XmlElement xref = xml.CreateElement("xref");
                            contrib.AppendChild(xref);
                            xref.SetAttribute("ref-type", "aff");
                            xref.SetAttribute("rid", "aff" + aut.Afiliacion);
                            xref.InnerText = contador + "";
                        }
                        if (!String.IsNullOrEmpty(articulo.Autorcorres) && isCorres)
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

                        if (!String.IsNullOrEmpty(af.nombre))
                        {
                            XmlElement nombre = xml.CreateElement("institution");
                            aff.AppendChild(nombre);
                            nombre.SetAttribute("content-type", "orgname");
                            nombre.InnerText = af.nombre;
                        }

                        if (!String.IsNullOrEmpty(af.estado) || !String.IsNullOrEmpty(af.ciudad))
                        {
                            XmlElement addrline = xml.CreateElement("addr-line");
                            aff.AppendChild(addrline);

                            if (!String.IsNullOrEmpty(af.ciudad))
                            {
                                XmlElement namedcontent = xml.CreateElement("named-content");
                                addrline.AppendChild(namedcontent);
                                namedcontent.SetAttribute("content-type", "city");
                                namedcontent.InnerText = af.ciudad;
                            }

                            if (!String.IsNullOrEmpty(af.estado))
                            {
                                XmlElement namedcontent = xml.CreateElement("named-content");
                                addrline.AppendChild(namedcontent);
                                namedcontent.SetAttribute("content-type", "state");
                                namedcontent.InnerText = af.estado;
                            }
                        }

                        if (!String.IsNullOrEmpty(af.pais))
                        {
                            XmlElement country = xml.CreateElement("country");
                            aff.AppendChild(country);
                            if (!String.IsNullOrEmpty(af.paisIso))
                                country.SetAttribute("country", af.paisIso);
                            country.InnerText = af.pais;
                        }

                        if (!String.IsNullOrEmpty(af.email))
                        {
                            XmlElement email = xml.CreateElement("email");
                            aff.AppendChild(email);
                            email.InnerText = af.email;
                        }

                        XmlElement institution = xml.CreateElement("institution");
                        aff.AppendChild(institution);
                        institution.SetAttribute("content-type", "original");
                        institution.InnerText = af.Original.Replace(af.Id, " ");

                    }

                    if (!String.IsNullOrEmpty(articulo.Autorcorres) || articulo.notasAutor.Count > 0)
                    {
                        XmlElement authornotes = xml.CreateElement("author-notes");
                        articlemeta.AppendChild(authornotes);
                        if (!String.IsNullOrEmpty(articulo.Autorcorres))
                        {
                            XmlElement corresp = xml.CreateElement("corresp");
                            authornotes.AppendChild(corresp);
                            corresp.SetAttribute("id", "c01");
                            corresp.InnerText = "E-mail: ";
                            XmlElement email = xml.CreateElement("email");
                            corresp.AppendChild(email);
                            email.InnerText = articulo.Autorcorres;
                        }
                        if (articulo.notasAutor.Count > 0)
                        {
                            int contadorNota = 1 + articulo.notas.Count;
                            foreach (String nota in articulo.notasAutor)
                            {
                                XmlElement fn = xml.CreateElement("fn");
                                authornotes.AppendChild(fn);
                                fn.SetAttribute("fn-type", "other");
                                fn.SetAttribute("id", "fn" + contadorNota);
                                fn.InnerText = @"<p>" + nota + "@</p>";
                                contadorNota += 1;
                            }
                        }

                    }

                    //<pub-date> Publication Date, zero or more, el validador me obliga a ponerlo
                    if (!String.IsNullOrEmpty(parametrosXML["date"]))
                    {
                        String date = parametrosXML["date"] + "";
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
                        volume.InnerText = parametrosXML["volume"] + "";
                    else
                        volume.InnerText = "0";

                    //<issue> Issue Number, zero or more
                    XmlElement issue = xml.CreateElement("issue");
                    articlemeta.AppendChild(issue);
                    if (!String.IsNullOrEmpty(parametrosXML["issue"]))
                        issue.InnerText = parametrosXML["issue"] + "";
                    else
                        issue.InnerText = "0";
                    XmlComment insertaVolume = xml.CreateComment("Insertar Volumen y Número");
                    articlemeta.AppendChild(insertaVolume);

                    // Optionally, the following sequence (in order): <fpage> First Page
                    XmlElement fpage = xml.CreateElement("fpage");
                    articlemeta.AppendChild(fpage);
                    fpage.InnerText = articulo.PaginaI;

                    // Optionally, the following sequence (in order): <lpage> Last Page, zero or one
                    XmlElement lpage = xml.CreateElement("lpage");
                    articlemeta.AppendChild(lpage);
                    lpage.InnerText = articulo.PaginaF;
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
                    license.SetAttribute("xlink:href", @"http://creativecommons.org/licenses/by-nc/4.0/");
                    license.SetAttribute("xml:lang", "es");
                    XmlElement licensep = xml.CreateElement("license-p");
                    license.AppendChild(licensep);
                    licensep.InnerText = "Este es un artículo publicado en acceso abierto bajo una licencia Creative Commons";

                    //<abstract> Abstract, zero or more                    
                    if (!String.IsNullOrEmpty(articulo.Resumen))
                    {
                        XmlElement abstractt = xml.CreateElement("abstract");
                        articlemeta.AppendChild(abstractt);
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

                        foreach (String pc in articulo.Palabrasclave)
                        {
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
                    figcount.SetAttribute("count", articulo.numImagenes + "");

                    XmlComment c3 = xml.CreateComment("Verificar valor fig-count");
                    counts.AppendChild(c3);

                    XmlElement tablecount = xml.CreateElement("table-count");
                    counts.AppendChild(tablecount);
                    tablecount.SetAttribute("count", articulo.numTablas + "");

                    XmlElement equationcount = xml.CreateElement("equation-count");
                    counts.AppendChild(equationcount);
                    equationcount.SetAttribute("count", articulo.numFormulas + "");
                    XmlComment c2 = xml.CreateComment("Verificar valor equation-count");
                    counts.AppendChild(c2);

                    XmlElement refcount = xml.CreateElement("ref-count");
                    counts.AppendChild(refcount);
                    refcount.SetAttribute("count", articulo.Referencias.Count + "");

                    XmlElement pagecount = xml.CreateElement("page-count");
                    counts.AppendChild(pagecount);

                    if (!String.IsNullOrEmpty(articulo.PaginaF) && !String.IsNullOrEmpty(articulo.PaginaI))
                    {
                        int inicio = Int32.Parse(articulo.PaginaI);
                        int fin = Int32.Parse(articulo.PaginaF);
                        pagecount.SetAttribute("count", (fin - inicio + 1) + "");
                    }
                    else
                    {
                        pagecount.SetAttribute("count", articulo.numPaginas + 1 + "");
                    }

                    XmlElement body = xml.CreateElement("body");
                    article.AppendChild(body);


                    foreach (KeyValuePair<string, SeccionBody> tempSec in articulo.SectionsBody)
                    {
                        if (String.IsNullOrEmpty(tempSec.Value.titulo))
                            break;

                        XmlElement sec = xml.CreateElement("sec");
                        if (tempSec.Value.titulo.ToLower().Contains("intro") || tempSec.Value.titulo.ToLower().Contains("introducción")
                            || tempSec.Value.titulo.ToLower().Contains("introduction") || tempSec.Value.titulo.ToLower().Contains("introdução")
                            || tempSec.Value.titulo.ToLower().Contains("einführung") || tempSec.Value.titulo.ToLower().Contains("diagnóstico")
                            || tempSec.Value.titulo.ToLower().Contains("diagnostic") || tempSec.Value.titulo.ToLower().Contains("diagnose")
                            || tempSec.Value.titulo.ToLower().Contains("diagnosis"))
                            sec.SetAttribute("sec-type", "intro");

                        else if (tempSec.Value.titulo.ToLower().Contains("method") || tempSec.Value.titulo.ToLower().Contains("método")
                            || tempSec.Value.titulo.ToLower().Contains("verfahren") || tempSec.Value.titulo.ToLower().Contains("méthode"))
                            sec.SetAttribute("sec-type", "methods");

                        else if (tempSec.Value.titulo.ToLower().Contains("supplementary") || tempSec.Value.titulo.ToLower().Contains("suplementar")
                            || tempSec.Value.titulo.ToLower().Contains("ergänzungs") || tempSec.Value.titulo.ToLower().Contains("supplémentaire")
                            || tempSec.Value.titulo.ToLower().Contains("suplementario"))
                            sec.SetAttribute("sec-type", "supplementary-material");

                        else if (tempSec.Value.titulo.ToLower().Contains("materials") || tempSec.Value.titulo.ToLower().Contains("materiales")
                            || tempSec.Value.titulo.ToLower().Contains("matériels") || tempSec.Value.titulo.ToLower().Contains("materiais"))
                            sec.SetAttribute("sec-type", "materials");

                        else if (tempSec.Value.titulo.ToLower().Contains("results") || tempSec.Value.titulo.ToLower().Contains("resultado")
                            || tempSec.Value.titulo.ToLower().Contains("résultat") || tempSec.Value.titulo.ToLower().Contains("ergebnis"))
                            sec.SetAttribute("sec-type", "results");

                        else if (tempSec.Value.titulo.ToLower().Contains("discussion") || tempSec.Value.titulo.ToLower().Contains("discusión")
                            || tempSec.Value.titulo.ToLower().Contains("discussão") || tempSec.Value.titulo.ToLower().Contains("diskussion")
                            || tempSec.Value.titulo.ToLower().Contains("discussion"))
                            sec.SetAttribute("sec-type", "discussion");

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

                        foreach (string tempp in tempSec.Value.parrafos())
                        {
                            XmlElement pp = xml.CreateElement("p");
                            sec.AppendChild(pp);
                            pp.InnerText = tempp;
                        }

                        foreach (KeyValuePair<string, SeccionBody> subSecP in tempSec.Value.subsecciones)
                        {
                            if (String.IsNullOrEmpty(subSecP.Value.titulo))
                                break;

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
                        pA.InnerText = articulo.Agradecimientos.Replace("||", "  ").Replace("<br/>", "  ");
                    }

                    XmlElement reflist = xml.CreateElement("ref-list");
                    back.AppendChild(reflist);
                    //referencias bibliograficas
                    int totalREf = articulo.Referencias.Count;

                    XmlElement titleRef = xml.CreateElement("title");
                    reflist.AppendChild(titleRef);
                    titleRef.InnerText = "Referência Bibliográfica";

                    int ss = 1;
                    foreach (Referencia refe in articulo.Referencias)
                    {

                        if (String.IsNullOrEmpty(refe.Original) || refe.Original.Equals("\a"))
                            continue;

                        XmlElement refer = xml.CreateElement("ref");
                        reflist.AppendChild(refer);
                        refer.SetAttribute("id", "B" + ss);

                        XmlElement label = xml.CreateElement("label");
                        refer.AppendChild(label);
                        label.InnerText = ss + "";

                        string pattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
                        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);

                        XmlElement mixedcitation = xml.CreateElement("mixed-citation");
                        refer.AppendChild(mixedcitation);

                        //var test = new Regex(@"^\d+([\.])?$");
                        //refe.Original = test.Replace(refe.Original, "");

                        if (!String.IsNullOrEmpty(r.Match(refe.Original).Value))
                        {
                            //mixedcitation.InnerText = refe.Original.Replace(r.Match(refe.Original).Value, "&lt;" + r.Match(refe.Original).Value + "/&gt;");
                            mixedcitation.InnerText = refe.Original.Replace("<", "").Replace("/>", "").Replace(">", "");
                        }
                        else
                            mixedcitation.InnerText = refe.Original;

                        XmlElement elementcitation = xml.CreateElement("element-citation");
                        refer.AppendChild(elementcitation);
                        elementcitation.SetAttribute("publication-type", "other");


                        pattern = @"\b\d{4}\b";
                        r = new System.Text.RegularExpressions.Regex(pattern);
                        if (!string.IsNullOrEmpty(r.Match(refe.Original).Value))
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

                    if (articulo.glosario.contenido.Count > 0)
                    {
                        XmlElement deflist = xml.CreateElement("def-list");
                        back.AppendChild(deflist);
                        deflist.SetAttribute("id", "d1");

                        XmlElement label = xml.CreateElement("label");
                        deflist.AppendChild(label);
                        label.InnerText = articulo.glosario.nombre;

                        foreach (String key in articulo.glosario.contenido.Keys)
                        {

                            XmlElement defitem = xml.CreateElement("def-item");
                            deflist.AppendChild(defitem);

                            XmlElement term = xml.CreateElement("term");
                            defitem.AppendChild(term);
                            term.InnerText = key;

                            XmlElement def = xml.CreateElement("def");
                            defitem.AppendChild(def);
                            def.InnerText = "<p>" + articulo.glosario.contenido[key] + "</p>";
                        }

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
                            fn.SetAttribute("fn-type", "other");
                            fn.SetAttribute("id", "fn" + (i + 1));

                            XmlElement label = xml.CreateElement("label");
                            XmlElement p = xml.CreateElement("p");
                            fn.AppendChild(label);
                            fn.AppendChild(p);

                            label.InnerText = "" + (i + 1);

                            if (!string.IsNullOrEmpty(r.Match(articulo.notas[i]).Value))
                            {
                                String link = "<ext-link ext-link-type=\"uri\" xlink:href=\"" + r.Match(articulo.notas[i]).Value + "\">" + r.Match(articulo.notas[i]).Value + "</ext-link>";
                                articulo.notas[i] = articulo.notas[i].Replace(r.Match(articulo.notas[i]).Value, link);
                            }

                            p.InnerText = articulo.notas[i];

                        }
                    }

                    //Anexos || apendice
                    if (articulo.apendice.SectionsBody.Count > 0)
                    {
                        //articulo.apendice.SectionsBody["anexo"].subsecciones[ultimoAnexo].subsecciones[ultimoSubAnexo].addParrafo(textoParrafo);
                        XmlElement appgroup = xml.CreateElement("app-group");
                        back.AppendChild(appgroup);

                        XmlElement app = xml.CreateElement("app");
                        appgroup.AppendChild(app);
                        app.SetAttribute("id", "app1");

                        XmlElement label = xml.CreateElement("label");
                        app.AppendChild(label);
                        label.InnerText = articulo.apendice.SectionsBody["anexo"].titulo;
                        foreach (String parrafo in articulo.apendice.SectionsBody["anexo"].parrafos())
                        {
                            XmlElement p = xml.CreateElement("p");
                            app.AppendChild(p);
                            p.InnerText = parrafo;
                        }
                        foreach (String anexo in articulo.apendice.SectionsBody["anexo"].subsecciones.Keys)
                        {
                            XmlElement sec = xml.CreateElement("sec");
                            app.AppendChild(sec);

                            XmlElement title = xml.CreateElement("title");
                            sec.AppendChild(title);
                            title.InnerText = articulo.apendice.SectionsBody["anexo"].subsecciones[anexo].titulo;

                            foreach (String parrafo in articulo.apendice.SectionsBody["anexo"].subsecciones[anexo].parrafos())
                            {
                                XmlElement p = xml.CreateElement("p");
                                sec.AppendChild(p);
                                p.InnerText = parrafo;
                            }

                            foreach (String subAnexo in articulo.apendice.SectionsBody["anexo"].subsecciones[anexo].subsecciones.Keys)
                            {
                                XmlElement subsec = xml.CreateElement("sec");
                                sec.AppendChild(subsec);

                                XmlElement subtitle = xml.CreateElement("title");
                                subsec.AppendChild(subtitle);
                                subtitle.InnerText = articulo.apendice.SectionsBody["anexo"].subsecciones[anexo].subsecciones[subAnexo].titulo;

                                foreach (String parrafo in articulo.apendice.SectionsBody["anexo"].subsecciones[anexo].subsecciones[subAnexo].parrafos())
                                {
                                    XmlElement p = xml.CreateElement("p");
                                    subsec.AppendChild(p);
                                    p.InnerText = parrafo;
                                }

                            }

                        }

                    }

                    /* Traduccion */
                    if (hayTraduccion && articuloTraduccion != null)
                    {
                        XmlElement subarticle = xml.CreateElement("sub-article");
                        article.AppendChild(subarticle);
                        subarticle.SetAttribute("article-type", "translation");
                        subarticle.SetAttribute("id", "s1");
                        subarticle.SetAttribute("xml:lang", "en");

                        XmlElement frontstub = xml.CreateElement("front-stub");
                        subarticle.AppendChild(frontstub);

                        if (!String.IsNullOrEmpty(articuloTraduccion.Seccion))
                        {
                            XmlElement article_categories = xml.CreateElement("article-categories");
                            frontstub.AppendChild(article_categories);

                            XmlElement subj_group = xml.CreateElement("subj-group");
                            article_categories.AppendChild(subj_group);

                            subj_group.SetAttribute("subj-group-type", "heading");

                            XmlElement subject_ = xml.CreateElement("subject");
                            subj_group.AppendChild(subject_);
                            subject_.InnerText = articuloTraduccion.Seccion;
                        }



                        if (!String.IsNullOrEmpty(articuloTraduccion.Titulo))
                        {
                            XmlElement titlegroup2 = xml.CreateElement("title-group");
                            frontstub.AppendChild(titlegroup2);

                            XmlElement articletitle2 = xml.CreateElement("article-title");
                            titlegroup2.AppendChild(articletitle2);
                            articletitle2.InnerText = articuloTraduccion.Titulo;
                        }

                        if (!String.IsNullOrEmpty(articuloTraduccion.Abstractt))
                        {
                            XmlElement abstract_ = xml.CreateElement("abstract");
                            frontstub.AppendChild(abstract_);

                            XmlElement title_ = xml.CreateElement("title");
                            abstract_.AppendChild(title_);
                            title_.InnerText = articuloTraduccion.Abstractt.Substring(0, articuloTraduccion.Abstractt.IndexOf("<p>"));

                            abstract_.InnerText = articuloTraduccion.Abstractt.Substring(articuloTraduccion.Abstractt.IndexOf("<p>"));
                        }

                         if ( articuloTraduccion.Keywords.Count > 0  ){
                             XmlElement kwd_group = xml.CreateElement("kwd-group");
                             frontstub.AppendChild(kwd_group);

                             XmlElement title = xml.CreateElement("title");
                             kwd_group.AppendChild(title);
                             title.InnerText = "Keywords:";
                             foreach (String kw in articuloTraduccion.Keywords)
                             {
                                 XmlElement kwd = xml.CreateElement("kwd");
                                 kwd_group.AppendChild(kwd);
                                 kwd.InnerText = kw;
                             }
                         }


                        XmlElement body2 = xml.CreateElement("body");
                        subarticle.AppendChild(body2);


                        foreach (KeyValuePair<string, SeccionBody> tempSec in articuloTraduccion.SectionsBody)
                        {
                            if (String.IsNullOrEmpty(tempSec.Value.titulo))
                                break;

                            XmlElement sec = xml.CreateElement("sec");
                            if (tempSec.Value.titulo.ToLower().Contains("intro"))
                                sec.SetAttribute("sec-type", "intro");
                            else if (tempSec.Value.titulo.ToLower().Contains("results") || tempSec.Value.titulo.ToLower().Contains("resultado"))
                                sec.SetAttribute("sec-type", "results");
                            else if (tempSec.Value.titulo.ToLower().Contains("conclusión") || tempSec.Value.titulo.ToLower().Contains("conclusion"))
                                sec.SetAttribute("sec-type", "conclusions");
                            else
                            {
                                //sec.SetAttribute("sec-type", "intro|methods|conclusions|results");
                                XmlComment comentario = xml.CreateComment("Agregar a  atributo sec-type, posibles valores: intro, methods, conclusions, results");
                                sec.AppendChild(comentario);
                            }
                            body2.AppendChild(sec);

                            XmlElement title = xml.CreateElement("title");
                            sec.AppendChild(title);
                            title.InnerText = tempSec.Value.titulo;

                            foreach (string tempp in tempSec.Value.parrafos())
                            {
                                XmlElement pp = xml.CreateElement("p");
                                sec.AppendChild(pp);
                                pp.InnerText = tempp;
                            }

                            foreach (KeyValuePair<string, SeccionBody> subSecP in tempSec.Value.subsecciones)
                            {
                                if (String.IsNullOrEmpty(subSecP.Value.titulo))
                                    break;

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

                        body2.InnerXml = Tools.enlazaReferencias(articulo, body2.InnerXml);
                        body2.InnerXml = Tools.creaLinksExternos(articulo, body2.InnerXml);

                    }

                    //enlazar referencia
                    body.InnerXml = Tools.enlazaReferencias(articulo, body.InnerXml);
                    body.InnerXml = Tools.creaLinksExternos(articulo, body.InnerXml);

                    //enlazar links externos

                    try
                    {
                        //String temp = rutaContenido + "xml.xml";
                        String temp = rutaContenido + Path.GetFileNameWithoutExtension(rutaDocumento) + ".xml";
                        xml.Save(@temp);

                        //corregir un error de c#
                        var fileContents = System.IO.File.ReadAllText(temp);
                        fileContents = fileContents.Replace("<ext-link ext-link-type=\"uri\" href", "<ext-link ext-link-type=\"uri\" xlink:href");
                        fileContents = fileContents.Replace("href=\"http://creativecommons.org/licenses/by-nc/4.0/\"", "xlink:href=\"http://creativecommons.org/licenses/by-nc/4.0/\"");

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

                }
                catch (Exception e)
                {
                    mensaje = HttpContext.GetGlobalResourceObject("Languaje", "GeneralError") + " XML";
                    return false;
                }

            }
            else
            {
                mensaje = HttpContext.GetGlobalResourceObject("Languaje", "GeneralError") + " XML";
                return false;
            }
            return true;
        }


    }
}
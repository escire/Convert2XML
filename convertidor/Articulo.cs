﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jats
{
    public class Articulo
    {

        public int numPaginas { get; set; }
        public int numTablas { get; set; }
        public int numImagenes { get; set; }

        public int numFormulas { get; set; }

        public int numCuadrosTexto { get; set; }

        public String Doi { get; set; }
        private List<Autor> autores = new List<Autor>();
        public String Seccion { get; set; }
        public String Titulo { get; set; }
        public String Title { get; set; }
        public String Editor { get; set; }
        public String Autorcorres { get; set; }
        public String AutorcorresNombre { get; set; }
        public String Fechaaceptado { get; set; }
        public String Fecharecibido { get; set; }
        public String Fecharevisado { get; set; }
        public String Volumen { get; set; }
        public String Numero { get; set; }
        public String PaginaI { get; set; }
        public String PaginaF { get; set; }
        public String Resumen { get; set; }
        public String Abstractt { get; set; }

        public String Agradecimientos { get; set; }

        private List<String> palabrasclave = new List<string>();
        private List<String> keywords = new List<string>();
        private List<Afiliacion> afiliaciones = new List<Afiliacion>();
        private List<Referencia> referencias = new List<Referencia>();
        public List<String> secciones = new List<String>();
        public List<LinkReferencia> linkReferencias = new List<LinkReferencia>();

        public List<String> notas = new List<String>();  

        public Dictionary<string, SeccionBody> SectionsBody = new Dictionary<string, SeccionBody>();  

        public String Body { get; set; }

        public Apendice apendice = new Apendice();

        public List<String> notasAutor = new List<string>();

        public Glosario glosario = new Glosario();


        public List<Referencia> Referencias
        {
            get
            {
                return referencias;
            }
            set
            {
                referencias = value;
            }
        }

        public void addLinkReferencia( LinkReferencia linkref )
        {
            linkReferencias.Add( linkref );
        }

        public void addReferencia( Referencia r )
        {
            referencias.Add(r);
        }


        public void addSeccion(String s)
        {
            secciones.Add(s);
        }

        public List<Afiliacion> Afiliaciones
        {
            get
            {
                return afiliaciones;
            }
            set
            {
                afiliaciones = value;
            }
        }

        public void addAfiliaciones(String id, String original)
        {
            afiliaciones.Add(new Afiliacion(id, original));
        }

        public void addAfiliaciones(String i, String origi, String nom, String mail, String pa, String paISO, String esta, String ciu){
            afiliaciones.Add(new Afiliacion( i,  origi,  nom,  mail,  pa,  paISO,  esta,  ciu) );
        }
  
        public void addPalabrasclave(String pc)
        {
            palabrasclave.Add(pc);
        }

        public void addKeywords(String kw){
            keywords.Add(kw);
        }

        public List<String> Palabrasclave
        {
            get
            {
                return palabrasclave;
            }
            set
            {
                palabrasclave = value;
            }
        }

        public List<String> Keywords
        {
            get
            {
                return keywords;
            }
            set
            {
                keywords = value;
            }
        }
        
        public List<Autor> Autores
        {
            get
            {
                return autores;
            }
            set
            {
                 autores = value;
            }
        }

        public Boolean addAutores( String fb, String sn, String af ){
            if (fb != null && sn != null  && !fb.Equals("") && !sn.Equals("") )
            {
                autores.Add( new Autor(fb, sn, af) );
                return true;
            }
            return false;

        }

    }

    public class Glosario
    {
        public String nombre { get; set; }

        public Dictionary<string, string> contenido = new Dictionary<string, string>();  

    }

    public class Autor
    {

        public String FirstName { get; set; }
        public String SurName { get; set; }
        public String Afiliacion { get; set; }

        public Autor(String fn, String sn, String af)
        {
            FirstName = fn;
            SurName = sn;
            Afiliacion = af;
        }   
    }

    public class LinkReferencia{
        public String Apellido { get; set; }
        public String Anio { get; set; }

        public LinkReferencia() { }
        public LinkReferencia( String apellido, String anio ){
            Apellido = apellido;
            Anio = anio;
        }
    }

    public class Afiliacion
    {
        public String Original { get; set; }
        public String Id { get; set; }
        public String Direccion { get; set; }
        public String pais { get; set; }
        public String paisIso { get; set; }
        public String estado { get; set; }
        public String ciudad { get; set; }
        public String nombre { get; set; }
        public String email { get; set; }

        public Afiliacion(String i, String origi, String nom, String mail, String pa, String paISO, String esta, String ciu)
        {
            Id = i;
            Original = origi;
            nombre = nom;
            email = mail;
            pais = pa;
            paisIso = paISO;
            estado = esta;
            ciudad = ciu;
        }

        public Afiliacion(String i, String origi)
        {
            Id = i;
            Original = origi;
        }

    }

    public class SeccionBody
    {
        public SeccionBody(string t) { titulo = t; }
        public String titulo { get; set; }

        public Dictionary<string, SeccionBody> subsecciones = new Dictionary<string, SeccionBody>();  
        
        private List<String> parrafo = new List<String>();
        public void addParrafo(String texto)
        {
            parrafo.Add(texto);
        }

        public List<String> parrafos()
        {
            return parrafo;
        }
    }

    public class Apendice
    {
        public Dictionary<string, SeccionBody> SectionsBody = new Dictionary<string, SeccionBody>();  
    }

    public class Referencia
    {

        public String Original { get; set; }
        public String Organizacion { get; set; }
        public String Titulo { get; set; }
        public String Date { get; set; }
        public String Edicion { get; set; }
        public String Pages { get; set; }

        private List<Autor> autores = new List<Autor>();

        public List<Autor> Autores{
            get{
                return autores;
            }
            set{
                autores = value;
            }
        }

        public void addAutor( String nombre, String apellido ){
            autores.Add( new Autor(nombre, apellido, null ));
        }


       
    }
}
<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:mml="http://www.w3.org/1998/Math/MathML" version="1.0" exclude-result-prefixes="xlink mml">
  <xsl:output doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd" encoding="UTF-8"/>
  <xsl:strip-space elements="*"/>
  <!-- Space is preserved in all elements allowing #PCDATA -->
  <xsl:preserve-space elements="abbrev abbrev-journal-title access-date addr-line               aff alt-text alt-title article-id article-title               attrib award-id bold chapter-title chem-struct               collab comment compound-kwd-part compound-subject-part               conf-acronym conf-date conf-loc conf-name conf-num               conf-sponsor conf-theme contrib-id copyright-holder               copyright-statement copyright-year corresp country               date-in-citation day def-head degrees disp-formula               edition elocation-id email etal ext-link fax fpage               funding-source funding-statement given-names glyph-data               gov inline-formula inline-supplementary-material               institution isbn issn-l issn issue issue-id issue-part               issue-sponsor issue-title italic journal-id               journal-subtitle journal-title kwd label license-p               long-desc lpage meta-name meta-value mixed-citation               monospace month named-content object-id on-behalf-of               overline p page-range part-title patent person-group               phone prefix preformat price principal-award-recipient               principal-investigator product pub-id publisher-loc               publisher-name related-article related-object role               roman sans-serif sc season self-uri series series-text               series-title sig sig-block size source speaker std               strike string-name styled-content std-organization               sub subject subtitle suffix sup supplement surname               target td term term-head tex-math textual-form th               time-stamp title trans-source trans-subtitle trans-title               underline uri verse-line volume volume-id volume-series               xref year                mml:annotation mml:ci mml:cn mml:csymbol mml:mi mml:mn                mml:mo mml:ms mml:mtext"/>
  <xsl:param name="transform" select="'jats-html.xsl'"/>
  <xsl:param name="report-warnings" select="'no'"/>
  <xsl:variable name="verbose" select="$report-warnings='yes'"/>
  <!-- Keys -->
  <!-- To reduce dependency on a DTD for processing, we declare
       a key to use instead of the id() function. -->
  <xsl:key name="element-by-id" match="*[@id]" use="@id"/>
  <!-- Enabling retrieval of cross-references to objects -->
  <xsl:key name="xref-by-rid" match="xref" use="@rid"/>
  <!-- ============================================================= -->
  <!--  ROOT TEMPLATE - HANDLES HTML FRAMEWORK                       -->
  <!-- ============================================================= -->
  <xsl:template match="/">
    <html>
      <!-- HTML header -->
      <xsl:call-template name="make-html-header"/>
      <body>
	    
		<!--
	    <div id="rightSidebar"></div>
		-->
		
		  
		  
        <div class="panel panel-default" id="tablaContenidos">
          <div class="panel-body">
            <xsl:apply-templates/>
          </div>
        </div>
        <style>
			img:hover { -moz-box-shadow: 0 0 10px #ccc; -webkit-box-shadow: 0 0 10px #ccc; box-shadow: 0 0 10px #ccc; } - See more at: http://www.corelangs.com/css/box/hover.html#sthash.mXk6WLcs.dpuf
		.wrapperMenu { 
		  overflow:hidden;
		}
		.wrapperMenu div {
		   padding: 10px;
			padding-top: 3px;
			padding-bottom: 3px;
		}
		#izquierda {
		  float:left; 
		  width:50%;
		}
		#derecha { 
		  overflow:hidden;
		}

		@media screen and (max-width: 400px) {
		   #izquierda { 
			float: none;
			margin-right:0;
			width:auto;
			border:0;
			border-bottom:2px solid #000;    
		  }
		}
		#sidebarUser{
			    margin-top: 60px;
		}
		#sidebar div.block{
			padding: 10px 8px 1.5em 1em;
		}
		p{
			text-align: justify;
			color: #4c4a37; font-family: 'Source Sans Pro', sans-serif; font-size: 14px; line-height: 16px; margin: 0 0 24px;
		}
		a.link { color: #ff9900; text-decoration: none; }
		a.link:hover { color: #ffcc66 }
		h1 { color: #333; font-weight: 400; line-height: 1.4; margin-bottom: 12px; }
		#main h1  { 
    		font-size: 24px;
			width: 75%;
			color: #00314c;
			font-family: 'Roboto', sans-serif;
			text-align : left;  
		}
		#main h2  { 
    		font-size: 22px;
			width: 75%;
			color: #00314c;
			font-family: 'Roboto', sans-serif;
			text-align : left;  
		}
		#main h3  { 
    		font-size: 20px;
			width: 75%;
			color: #00314c;
			font-family: 'Roboto', sans-serif;
			text-align : left;  
		}
		#main h4  { 
    		font-size: 18px;
			width: 75%;
			color: #00314c;
			font-family: 'Roboto', sans-serif;
			text-align : left;  
		}
		#main h5, .blockTitle  { 
    		font-size: 16px;
			width: 100%;
			color: #00314c;
			font-family: 'Roboto', sans-serif;
			text-align : left;  
		}	
		/*
		div#sidebarContent ul {
			border-top: 2px solid #0176c3;
		}
		*/
		select#searchField {
			width: 90%;
		}
		input#simpleQuery {
			width: 90%;
		}
		
		div#sidebarContent ul li{
			list-style: none
			color: #0176c3;
		}
		table#simpleSearchInput{
			border: 1px solid #fff;
		}
		.subheader { font-size: 26px; font-weight: 300; color: #ffcc66; margin: 0 0 24px; }
		
		p.citation{
			width: 78%;
    		font-size: 12px;
		}
		fig.panel{
			border: thin solid #CCC
		}
		body{
			text-align : justify;
		}
		table{
			border: 1px solid #9f9f9f;
			width: 100%;
			-webkit-box-sizing: content-box;
    		box-sizing: content-box;
			margin: 0 auto;
			clear: both;
			border-collapse: separate;
			border-spacing: 0;
			background-color: #fafafa;
		}
		thead {
			display: table-header-group;
			vertical-align: middle;
			border-color: inherit;
			border-collapse: separate;
    		border-spacing: 0;
		}
		tr {
			display: table-row;
			vertical-align: inherit;
			border-color: inherit;
			font-size: 14px;
		}
		th {
			white-space: nowrap;
			padding: 10px 18px;
    		border-bottom: 1px solid #111;
			font-weight: bold;
			box-sizing: content-box;
			font-size: 16px;
		}
		tbody {
			display: table-row-group;
			vertical-align: middle;
			border-color: inherit;
		}
		tr.odd {
			background-color: #f9f9f9;
		}
		
		div#content{
			/* outer shadows  (note the rgba is red, green, blue, alpha) 
			-webkit-box-shadow: 0px 0px 12px rgba(0, 0, 0, 0.4); 
			-moz-box-shadow: 0px 1px 6px rgba(23, 69, 88, .5); */
			border-color: #ff0000;
			box-shadow: 2px 2px 2px #CCCCCC;
			-webkit-border-radius: 2px;
			-moz-border-radius: 2px; 
			border: thin solid #EEE;
		}
		img.imagen {
			max-width: 90%;
			height: auto;
			margin-left: auto;
    		margin-right: auto;
			display: block;
		}
		
		.metadata-entry{
			font-size: 12px;
		}
		.section, .Section {
		    border-bottom-style: none;
		}
		.footer {
		    background-color: #FFF; 
		    background-image: none;
	    	    border-top: 1px solid #FFF; 
		}
		ul li:before {
    			font-size: 1em;
    			content: "";
		}

		#tablaContenidos {
			    border: thin solid #CCC;
		}

		#article-body{
			padding-top:20px;
			text-align: justify;
		}
		.main-title{
			padding-top:15px;
		}
		.section-title{
			padding-top:10px;
		}

		div &gt; *:first-child { margin-top:0em }

		div { margin-top: 0.5em }

		div.front, div.footer { }

		.back, .body { font-family: serif }

		div.metadata { font-family: sans-serif }
		div.centered { text-align: center }

		div.table { display: table }
		div.metadata.table { width: 100% }
		div.row2 { display: table-row }
		div.cell { display: table-cell; padding-left: 0.25em; padding-right: 0.25em }

		div.metadata div.cell {
		    vertical-align: top }

		div.two-column div.cell {
		    width: 50% }

		div.one-column div.cell.spanning { width: 100% }

		div.metadata-group { margin-top: 0.5em;
		  font-size: 75% }

		div.metadata-group &gt; p, div.metadata-group &gt; div { margin-top: 0.5em }

		div.metadata-area * { margin: 0em }

		div.metadata-chunk { margin-left: 0em }

		div.branding { text-align: center }

		div.document-title-notes {
		   text-align: center;
		   width: 60%;
		   margin-left: auto;
		   margin-right: auto
		   }

		div.footnote { font-size: 90% }

		/* rules */
		hr.part-rule {
		    border: 1px solid #dcdcdc;
		    width: 70%;
		    margin-top: 1em;
		    margin-bottom: 1em;
		    }

		hr.section-rule {
		    border: 1px dotted #dcdcdc;
		    width: 70%;
		    margin-top: 1em;
		    margin-bottom: 1em;
		    }

		/* superior numbers that are cross-references */
		.xref {
		    color: red;
		    }
		    
		/* generated text */     
		.generated { 
			font-size: 12px;
			color: #222; 
		}

		.warning, tex-math {
		    font-size:80%; font-family: sans-serif }

		.warning {
		    color: red }

		.tex-math { color: green }

		.data {
		    color: black;
		    }

		.formula {
		    font-family: sans-serif;
		    font-size: 90% }
		    
		/* --------------- Titling levels -------------------- */


		h1, h2, h3, h4, h5, h6 {
		   display: block;
		   margin-top: 0em;
		   margin-bottom: 0.5em;
		   font-weight: bold;
           font-weight: 500;
           line-height: 1.1;
		  }
		/* titling level 1: document title */
		.document-title {
		   text-align: center;
		}

		/* callout titles appear in a left column (table cell)
		   opposite what they head */
		.callout-title { 
		  margin-top: 0.5em;
		  margin-right: 1em;
		  font-size: 140% }
		  


		div.section, div.back-section, div.footer {
		  margin:15px;} 

		div.panel { background-color: white;
		  font-size: 90%;
		  border: thin solid black;
		  padding-left: 0.5em; padding-right: 0.5em;
		  padding-top: 0.5em; padding-bottom: 0.5em;
		  margin-top: 0.5em; margin-bottom: 0.5em }

		div.blockquote { font-size: 90%;
		  margin-left: 1em; margin-right: 1em;
		  margin-top: 0.5em; margin-bottom: 0.5em }

		div.caption {
		  margin-top: 0.5em; margin-bottom: 0.5em }

		div.speech {
		  margin-left: 1em; margin-right: 1em;
		  margin-top: 0.5em; margin-bottom: 0.5em }

		div.verse-group {
		  margin-left: 1em;
		  margin-top: 0.5em; margin-bottom: 0.5em }

		div.verse-group div.verse-group {
		  margin-left: 1em;
		  margin-top: 0em; margin-bottom: 0em }

		div.note { margin-top: 0em; margin-left: 1em;
		  font-size: 85% }

		.ref-label { margin-top: 0em; vertical-align: top }

		.ref-content { margin-top: 0em; padding-left: 0.25em }

		h5.label { margin-top: 0em; margin-bottom: 0em }

		p { margin-top: 0.5em; margin-bottom: 0em }

		p.first { margin-top: 0em }

		p.verse-line, p.citation { margin-top: 0em; margin-bottom: 0em; margin-left: 2em; text-indent: -2em }

		p.address-line { margin-top: 0em; margin-bottom: 0em; margin-left: 2em }

		ul, ol { margin-top: 0.5em }

		li { margin-top: 0.5em; margin-bottom: 0em }
		li &gt; p { margin-top: 0.2em; margin-bottom: 0em  }

		div.def-list { border-spacing: 0.25em }

		div.def-list div.cell { vertical-align: top;
		  border-bottom: thin solid black;
		  padding-bottom: 0.5em }

		div.def-list div.def-list-head {
		  text-align: left }

		/* text decoration */
		.label { font-weight: bold; font-family: sans-serif; font-size: 80% }

		.monospace {
			font-family: monospace;
			}

		.overline{
			text-decoration: overline;
			}
		 
		a       { text-decoration: none }
		a:hover { text-decoration: underline }
		.transition {
			-webkit-transform: scale(1.1); 
			-moz-transform: scale(1.1);
			-o-transform: scale(1.1);
			transform: scale(1.1);
		}
		img.imagen {
			-webkit-transition: all .4s ease-in-out;
			-moz-transition: all .4s ease-in-out;
			-o-transition: all .4s ease-in-out;
			-ms-transition: all .4s ease-in-out;
		}
			
			
		sup.circulo{
			background: #6789d3 none repeat scroll 0 0;
			border-radius: 10px;
			color: #fff;
			cursor: pointer;
			display: inline-block;
			font-size: 8px;
			line-height: 100%;
			margin-top: 0px;
			padding: 4px 6px 3px;
			text-align: center;
			vertical-align: top;
		}
		sup.circulo.verde{
			background: #b2dba1 none repeat scroll 0 0;
		}
		sup.circulo.verde{
			background: #9acfea none repeat scroll 0 0;
		}
			
		.descargarArchivo{
    		/*background: #0176c3;
			background-image: -webkit-linear-gradient(top, #38a0e5, #0176c3);
			background-image: linear-gradient(to bottom, #38a0e5, #0176c3);
			box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.25);
			*/
			background-color: #d9534f;
    		border-color: #d43f3a;
			border: 1px solid #d43f3a;
			color: #fff;
			text-shadow: 0 1px rgba(0, 0, 0, 0.2);
			font-size: 0.875rem;
			width: 85%;
    		max-width: 300px;
			-webkit-font-smoothing: antialiased;
			-moz-osx-font-smoothing: grayscale;
			position: relative;
			display: inline-block;
			vertical-align: middle;
			cursor: pointer;
			margin: 0;
			line-height: normal;
			border-radius: 3px;
			border: 1px solid transparent;
			padding: 0.5em 1em;
			transition: 0.15s ease;
			text-align: center;
		}
		#rightSidebar a.descargarArchivo {
			color: #FFF;
			font-weight: 700;
			font-size: 0.875rem;
			font-family: sans-serif;
    		-webkit-transition: 0.15s ease;
			margin-bottom: 5px;
		}
		.descargarArchivo:hover{
			background: #0176c3;
			background-image: -webkit-linear-gradient(top, #38a0e5, #0176c3);
			background-image: linear-gradient(to bottom, #38a0e5, #0176c3);
			box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.25);
			border: 1px solid #0176c3;
			color: #fff;
			text-shadow: 0 1px rgba(0, 0, 0, 0.2);
		}
		#sidebarUser {
			margin-top: 10px;
		}
		#sidebarContent {
			margin-top: 60px;
		}
		div.cell.author p.metadata-entry {
			font-size: 16px;
		}
		a.link sup {
			background: #789ad3 none repeat scroll 0 0;
			border-radius: 10px;
			color: #fff;
			cursor: pointer;
			/* display: inline-block; */
			font-size: 8px;
			line-height: 100%;
			margin-top: 0px;
			padding: 4px 6px 3px;
			text-align: center;
		}
	</style>
      </body>
    </html>
  </xsl:template>
  <xsl:template name="make-html-header">
    <head>
      <title>
        <xsl:variable name="authors">
          <xsl:call-template name="author-string"/>
        </xsl:variable>
        <xsl:value-of select="normalize-space(string($authors))"/>
        <xsl:if test="normalize-space(string($authors))">: </xsl:if>
        <xsl:value-of select="/article/front/article-meta/title-group/article-title[1]"/>
      </title>	    
		
		<script src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
		<script src="https://code.jquery.com/ui/1.12.0/jquery-ui.js"></script>
		<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.1/MathJax.js?config=TeX-MML-AM_CHTML"></script>
		
		<link href="https://fonts.googleapis.com/css?family=Lato|Roboto" rel="stylesheet"/>
  		
		 <script>
			  $( function() {
					
			 		
			 		$('img.imagen').hover(function() {
						$("img.imagen").addClass('transition');

					}, function() {
						$("img.imagen").removeClass('transition');
					});
			 
			 		var newURL = window.location.protocol + "//" + window.location.host + window.location.pathname;
					var epub = "/demo/toEpub/convertir.php?url=" + newURL;
					
			 		
  					var	$newdiv2 = document.createElement( "div" );
			 		$newdiv2.className = "block";
			 		$newdiv2.id = "sidebarContent";
			 
			 		var	$newSpan = document.createElement( "span" );
			 		$newSpan.className = "blockTitle";
			 		$newSpan.setAttribute("style", "margin-top:15px;");
			 
			 		var	$newUL = document.createElement( "ul" );
			 		$( "#rightSidebar" ).prepend( $newdiv2 );
			 		
			 
			 		var	$newXML = document.createElement( "a" );
			 		$newXML.className = "descargarArchivo";
			 		$newXML.title = "Descargar XML";
			 		$newXML.text = "Descargar XML";
			 		$newXML.href = newURL.replace("view", "download");
			 		$( "#sidebarContent" ).prepend( $newXML );
			 
			 		var	$newPDF = document.createElement( "a" );
			 		$newPDF.className = "descargarArchivo";
			 		$newPDF.title = "Descargar PDF";
			 		$newPDF.text = "Descargar PDF";
			 		$newPDF.href = $('meta[name=citation_pdf_url]').attr("content");
			 		if ($('meta[name=citation_pdf_url]').attr("content") != null)
			 			$( "#sidebarContent" ).prepend( $newPDF );
			 
			 		var	$newEPUB = document.createElement( "a" );
			 		$newEPUB.className = "descargarArchivo";
			 		$newEPUB.title = "Descargar EPUB";
			 		$newEPUB.text = "Descargar EPUB";
			 		$newEPUB.href = $('meta[name=citation_fulltext_html_url]').attr("content");
			 		if ($('meta[name=citation_fulltext_html_url]').attr("content") != null)
			 			$( "#sidebarContent" ).prepend( $newEPUB );
			 
			 		
			 		$( "#sidebarContent" ).append( $newSpan );
			 		$( "#sidebarContent" ).append( $newUL );
			 		$( "#sidebarContent span" ).html( 'Tabla de Contenido' );
			 
			 		$('h2').each(function() {
			 			
			 			var seccion = $(this).text();
			 			var enlace = $(this).next().prop('id')
			 			console.log( seccion + " -- " + enlace );
			 
			 			var	$newLi = document.createElement( "li" );
			 			$newLi.className = "liContenido";
			 
			 			var	$newA = document.createElement( "a" );
			 			$newA.className = "hrefContenido";
			 			$newA.title = seccion;
			 			$newA.text = seccion;
			 			$newA.href = newURL+"#"+enlace;
			 			
			 			$newLi.appendChild($newA);
			 
			 			$( "#sidebarContent ul" ).append( $newLi );
					});
			 
			  } );
		  </script>
	
		
      <!-- XXX check: any other header stuff? XXX -->
    </head>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  TOP LEVEL                                                    -->
  <!-- ============================================================= -->
  <!--
      content model for article:
         (front,body?,back?,floats-group?,(sub-article*|response*))
      
      content model for sub-article:
         ((front|front-stub),body?,back?,floats-group?,
          (sub-article*|response*))
      
      content model for response:
         ((front|front-stub),body?,back?,floats-group?) -->
  <xsl:template match="article">
    <xsl:call-template name="make-article"/>
  </xsl:template>
  <xsl:template match="sub-article | response">
    <hr class="part-rule"/>
    <xsl:call-template name="make-article"/>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  "make-article" for the document architecture                 -->
  <!-- ============================================================= -->
  <xsl:template name="make-article">
    <!-- Generates a series of (flattened) divs for contents of any
	       article, sub-article or response -->
    <!-- variable to be used in div id's to keep them unique -->
    <xsl:variable name="this-article">
      <xsl:apply-templates select="." mode="id"/>
    </xsl:variable>
    <div id="{$this-article}-front" class="front">
      <xsl:apply-templates select="front | front-stub"/>
    </div>
    <!-- body -->
    <xsl:for-each select="body">
      <div id="{$this-article}-body" class="body">
        <xsl:apply-templates/>
      </div>
    </xsl:for-each>
    <xsl:if test="back | $loose-footnotes">
      <!-- $loose-footnotes is defined below as any footnotes outside
           front matter or fn-group -->
      <div id="{$this-article}-back" class="back">
        <xsl:call-template name="make-back"/>
      </div>
    </xsl:if>
    <xsl:for-each select="floats-group | floats-wrap">
      <!-- floats-wrap is from 2.3 -->
      <div id="{$this-article}-floats" class="back">
        <xsl:call-template name="main-title">
          <xsl:with-param name="contents">
            <span class="generated">Floating objects</span>
          </xsl:with-param>
        </xsl:call-template>
        <xsl:apply-templates/>
      </div>
    </xsl:for-each>
    <!-- more metadata goes in the footer -->
    <div id="{$this-article}-footer" class="footer">
      <xsl:call-template name="footer-metadata"/>
      <xsl:call-template name="footer-branding"/>
    </div>
    <!-- sub-article or response (recursively calls
		     this template) -->
    <xsl:apply-templates select="sub-article | response"/>
  </xsl:template>
  <xsl:template match="front | front-stub">
    <!-- change context to front/article-meta (again) -->
    <xsl:for-each select="article-meta | self::front-stub">
      <div class="metadata centered">
        <xsl:apply-templates mode="metadata" select="title-group"/>
      </div>
      <hr class="part-rule"/>
      <!-- contrib-group, aff, aff-alternatives, author-notes -->
      <xsl:apply-templates mode="metadata" select="contrib-group"/>
      <!-- back in article-meta or front-stub context  autor de correspondencia -->
      <xsl:if test="aff | aff-alternatives | author-notes">
            <div class="cell">
              <div class="metadata-group">
                <xsl:apply-templates mode="metadata" select="aff | aff-alternatives | author-notes"/>
              </div>
            </div>
      </xsl:if>	
      <!-- abstract(s) -->
      <xsl:if test="abstract | trans-abstract">
        <!-- rule separates title+authors from abstract(s) -->
        <hr class="section-rule"/>
        <xsl:for-each select="abstract | trans-abstract">
          <!-- title in left column, content (paras, secs) in right -->
          <div class="metadata two-column table">
            <div class="row2">
              <div class="cell" style="text-align: justify; width: 75%">
                <h4 class="callout-title">
                  <xsl:apply-templates select="title/node()"/>
                  <xsl:if test="not(normalize-space(string(title)))">
                    <br/>
                    <h2 id="resumen">
                      <xsl:if test="self::trans-abstract">Traducción del </xsl:if>
                      <xsl:text>Resumen</xsl:text>
                    </h2>
                  </xsl:if>
                </h4>
                <xsl:apply-templates select="*[not(self::title)]"/>
              </div>
            </div>
          </div>
        </xsl:for-each>
        <!-- end of abstract or trans-abstract -->
      </xsl:if>
      <!-- end of dealing with abstracts -->
    </xsl:for-each>
    <xsl:for-each select="notes">
      <div class="metadata">
        <xsl:apply-templates mode="metadata" select="."/>
      </div>
    </xsl:for-each>
    <hr class="part-rule"/>
    <!-- end of big front-matter pull -->
  </xsl:template>
  <xsl:template name="footer-metadata">
    <!-- handles: article-categories, kwd-group, counts, 
           supplementary-material, custom-meta-group
         Plus also generates a sheet of processing warnings
         -->
    <xsl:for-each select="front/article-meta | front-stub">
      <xsl:if test="article-categories | kwd-group | counts |                     supplementary-material | custom-meta-group |                     custom-meta-wrap">
        <!-- custom-meta-wrap is from NLM 2.3 -->
        <hr class="part-rule"/>
        <div class="metadata">
          <h4 class="generated">
            <xsl:text>Información de artículo</xsl:text>
          </h4>
          <div class="metadata-group">
            <xsl:apply-templates mode="metadata" select="supplementary-material"/>
            <xsl:apply-templates mode="metadata" select="article-categories | kwd-group | counts"/>
            <xsl:apply-templates mode="metadata" select="custom-meta-group | custom-meta-wrap"/>
          </div>
        </div>
      </xsl:if>
    </xsl:for-each>
    <xsl:variable name="process-warnings">
      <xsl:call-template name="process-warnings"/>
    </xsl:variable>
    <xsl:if test="normalize-space(string($process-warnings))">
      <hr class="part-rule"/>
      <div class="metadata one-column table">
        <!--<div class="row">
          <div class="cell spanning">
            
          </div>
        </div>-->
        <div class="row2">
          <div class="cell spanning">
            <h4 class="generated">
              <xsl:text>Process warnings</xsl:text>
            </h4>
            <p>Warnings reported by the processor due to problematic markup follow:</p>
            <div class="metadata-group">
              <xsl:copy-of select="$process-warnings"/>
            </div>
          </div>
        </div>
      </div>
    </xsl:if>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  METADATA PROCESSING                                          -->
  <!-- ============================================================= -->
  <!--  Includes mode "metadata" for front matter, along with 
      "metadata-inline" for metadata elements collapsed into 
      inline sequences, plus associated named templates            -->
  <!-- WAS journal-meta content:
       journal-id+, journal-title-group*, issn+, isbn*, publisher?,
       notes? -->
  <!-- (journal-id+, journal-title-group*, (contrib-group | aff | aff-alternatives)*,
       issn+, issn-l?, isbn*, publisher?, notes*, self-uri*) -->
  <xsl:template match="journal-id" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>ID de Revista</xsl:text>
        <xsl:for-each select="@journal-id-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="journal-title-group" mode="metadata">
    <xsl:apply-templates mode="metadata"/>
  </xsl:template>
  <xsl:template match="issn" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>ISSN</xsl:text>
        <xsl:call-template name="append-pub-type"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="issn-l" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>ISSN-L</xsl:text>
        <xsl:call-template name="append-pub-type"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="isbn" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>ISBN</xsl:text>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="publisher" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Editor</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates select="publisher-name" mode="metadata-inline"/>
        <xsl:apply-templates select="publisher-loc" mode="metadata-inline"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="publisher-name" mode="metadata-inline">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="publisher-loc" mode="metadata-inline">
    <span class="generated"> (</span>
    <xsl:apply-templates/>
    <span class="generated">)</span>
  </xsl:template>
  <xsl:template match="notes" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Notas</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!-- journal-title-group content:
       (journal-title*, journal-subtitle*, trans-title-group*,
       abbrev-journal-title*) -->
  <xsl:template match="journal-title" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Título</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="journal-subtitle" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Subtítulo</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="trans-title-group" mode="metadata">
    <xsl:apply-templates mode="metadata"/>
  </xsl:template>
  <xsl:template match="abbrev-journal-title" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Título Abreviado</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!-- trans-title-group content: (trans-title, trans-subtitle*) -->
  <xsl:template match="trans-title" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text> </xsl:text>
        <xsl:for-each select="(../@xml:lang|@xml:lang)[last()]">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="trans-subtitle" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Translated Subtitle</xsl:text>
        <xsl:for-each select="(../@xml:lang|@xml:lang)[last()]">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!-- article-meta content:
    (article-id*, article-categories?, title-group,
     (contrib-group | aff)*, author-notes?, pub-date+, volume?,
     volume-id*, volume-series?, issue?, issue-id*, 
     issue-title*, issue-sponsor*, issue-part?, isbn*, 
     supplement?, ((fpage, lpage?, page-range?) | elocation-id)?, 
     (email | ext-link | uri | product | supplementary-material)*, 
     history?, permissions?, self-uri*, related-article*,
     abstract*, trans-abstract*, 
     kwd-group*, funding-group*, conference*, counts?, 
     custom-meta-group?) -->
  <!-- In order of appearance... -->
  <xsl:template match="ext-link" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>External link</xsl:text>
        <xsl:for-each select="ext-link-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates select="."/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="email" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Email</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates select="."/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="uri" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">URI</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates select="."/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="self-uri" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Self URI</xsl:text>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <a href="{@xlink:href}" class="link">
          <xsl:choose>
            <xsl:when test="normalize-space(string(.))">
              <xsl:apply-templates/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@xlink:href"/>
            </xsl:otherwise>
          </xsl:choose>
        </a>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="product" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Product Information</xsl:text>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:choose>
          <xsl:when test="normalize-space(string(@xlink:href))">
            <a class="link">
              <xsl:call-template name="assign-href"/>
              <xsl:apply-templates/>
            </a>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="permissions" mode="metadata">
    <xsl:apply-templates select="copyright-statement" mode="metadata"/>
    <xsl:if test="copyright-year | copyright-holder">
      <xsl:call-template name="metadata-labeled-entry">
        <xsl:with-param name="label">Copyright</xsl:with-param>
        <xsl:with-param name="contents">
          <xsl:for-each select="copyright-year | copyright-holder">
            <xsl:apply-templates/>
            <xsl:if test="not(position()=last())">, </xsl:if>
          </xsl:for-each>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
    <xsl:apply-templates select="license" mode="metadata"/>
  </xsl:template>
  <xsl:template match="copyright-statement" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Copyright statement</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="copyright-year" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Copyright</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="license" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">
        <xsl:text>Licencia</xsl:text>
        <xsl:if test="@license-type | @xlink:href">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="@license-type"/>
            <xsl:if test="@xlink:href">
              <xsl:if test="@license-type">, </xsl:if>
              <a class="link">
                <xsl:call-template name="assign-href"/>
                <xsl:value-of select="@xlink:href"/>
              </a>
            </xsl:if>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:if>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="history/date" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Fecha</xsl:text>
        <xsl:for-each select="@date-type">
          <xsl:choose>
            <xsl:when test=".='accepted'"> aceptado</xsl:when>
            <xsl:when test=".='received'"> recibido</xsl:when>
            <xsl:when test=".='rev-request'"> revisión solicitada</xsl:when>
            <xsl:when test=".='rev-recd'"> revisión recibida</xsl:when>
          </xsl:choose>
        </xsl:for-each>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:call-template name="format-date"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="pub-date" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text> Fecha de publicación</xsl:text>
        <xsl:call-template name="append-pub-type"/>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:call-template name="format-date"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="volume-info">
    <!-- handles volume?, volume-id*, volume-series? -->
    <xsl:if test="volume | volume-id | volume-series">
      <xsl:choose>
        <xsl:when test="not(volume-id[2]) or not(volume)">
          <!-- if there are no multiple volume-id, or no volume, we make one line only -->
          <xsl:call-template name="metadata-labeled-entry">
            <xsl:with-param name="label">Volumen</xsl:with-param>
            <xsl:with-param name="contents">
              <xsl:apply-templates select="volume | volume-series" mode="metadata-inline"/>
              <xsl:apply-templates select="volume-id" mode="metadata-inline"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="volume | volume-id | volume-series" mode="metadata"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <xsl:template match="volume | issue" mode="metadata-inline">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="volume-id | issue-id" mode="metadata-inline">
    <span class="generated">
      <xsl:text> (</xsl:text>
      <xsl:for-each select="@pub-id-type">
        <span class="data">
          <xsl:value-of select="."/>
        </span>
        <xsl:text> </xsl:text>
      </xsl:for-each>
      <xsl:text>ID: </xsl:text>
    </span>
    <xsl:apply-templates/>
    <span class="generated">)</span>
  </xsl:template>
  <xsl:template match="volume-series" mode="metadata-inline">
    <xsl:if test="preceding-sibling::volume">
      <span class="generated">,</span>
    </xsl:if>
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="volume" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Volume</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="volume-id" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Volume ID</xsl:text>
        <xsl:for-each select="@pub-id-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="volume-series" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Series</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="issue-info">
    <!-- handles issue?, issue-id*, issue-title*, issue-sponsor*, issue-part?, supplement? -->
    <xsl:variable name="issue-info" select="issue | issue-id | issue-title |       issue-sponsor | issue-part"/>
    <xsl:choose>
      <xsl:when test="$issue-info and not(issue-id[2] | issue-title[2] | issue-sponsor | issue-part)">
        <!-- if there are only one issue, issue-id and issue-title and nothing else, we make one line only -->
        <xsl:call-template name="metadata-labeled-entry">
          <xsl:with-param name="label">Número</xsl:with-param>
          <xsl:with-param name="contents">
            <xsl:apply-templates select="issue | issue-title" mode="metadata-inline"/>
            <xsl:apply-templates select="issue-id" mode="metadata-inline"/>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="$issue-info" mode="metadata"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="issue-title" mode="metadata-inline">
    <span class="generated">
      <xsl:if test="preceding-sibling::issue">,</xsl:if>
    </span>
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="issue" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Número</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="issue-id" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Issue ID</xsl:text>
        <xsl:for-each select="@pub-id-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="issue-title" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Issue title</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="issue-sponsor" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Issue sponsor</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="issue-part" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Issue part</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="page-info">
    <!-- handles (fpage, lpage?, page-range?) -->
    <xsl:if test="fpage | lpage | page-range">
      <xsl:call-template name="metadata-labeled-entry">
        <xsl:with-param name="label">
          <xsl:text>Página</xsl:text>
          <xsl:if test="normalize-space(string(lpage[not(.=../fpage)]))                    or normalize-space(string(page-range))">
            <!-- we have multiple pages if lpage exists and is not equal fpage,
               or if we have a page-range -->
            <xsl:text>s</xsl:text>
          </xsl:if>
        </xsl:with-param>
        <xsl:with-param name="contents">
          <xsl:value-of select="fpage"/>
          <xsl:if test="normalize-space(string(lpage[not(.=../fpage)]))">
            <xsl:text>-</xsl:text>
            <xsl:value-of select="lpage"/>
          </xsl:if>
          <xsl:for-each select="page-range">
            <xsl:text> (pp. </xsl:text>
            <xsl:value-of select="."/>
            <xsl:text>)</xsl:text>
          </xsl:for-each>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template match="elocation-id" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Electronic Location
      Identifier</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!-- isbn is already matched in mode 'metadata' above -->
  <xsl:template match="supplement" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Supplement Info</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="related-article | related-object" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Related </xsl:text>
        <xsl:choose>
          <xsl:when test="self::related-object">object</xsl:when>
          <xsl:otherwise>article</xsl:otherwise>
        </xsl:choose>
        <xsl:for-each select="@related-article-type | @object-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="translate(.,'-',' ')"/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:choose>
          <xsl:when test="normalize-space(string(@xlink:href))">
            <a class="link">
              <xsl:call-template name="assign-href"/>
              <xsl:apply-templates/>
            </a>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conference" mode="metadata">
    <!-- content model:
      (conf-date, 
       (conf-name | conf-acronym)+, 
       conf-num?, conf-loc?, conf-sponsor*, conf-theme?) -->
    <xsl:choose>
      <xsl:when test="not(conf-name[2] | conf-acronym[2] | conf-sponsor |                   conf-theme)">
        <!-- if there is no second name or acronym, and no sponsor
             or theme, we make one line only -->
        <xsl:call-template name="metadata-labeled-entry">
          <xsl:with-param name="label">Conference</xsl:with-param>
          <xsl:with-param name="contents">
            <xsl:apply-templates select="conf-acronym | conf-name" mode="metadata-inline"/>
            <xsl:apply-templates select="conf-num" mode="metadata-inline"/>
            <xsl:if test="conf-date | conf-loc">
              <span class="generated"> (</span>
              <xsl:for-each select="conf-date | conf-loc">
                <xsl:if test="position() = 2">, </xsl:if>
                <xsl:apply-templates select="." mode="metadata-inline"/>
              </xsl:for-each>
              <span class="generated">)</span>
            </xsl:if>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates mode="metadata"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="conf-date" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference date</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-name" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-acronym" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-num" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference number</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-loc" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference location</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-sponsor" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference sponsor</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-theme" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Conference theme</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="conf-name | conf-acronym" mode="metadata-inline">
    <!-- we only hit this template if there is at most one of each -->
    <xsl:variable name="following" select="preceding-sibling::conf-name | preceding-sibling::conf-acronym"/>
    <!-- if we come after the other, we go in parentheses -->
    <xsl:if test="$following">
      <span class="generated"> (</span>
    </xsl:if>
    <xsl:apply-templates/>
    <xsl:if test="$following">
      <span class="generated">)</span>
    </xsl:if>
  </xsl:template>
  <xsl:template match="conf-num" mode="metadata-inline">
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="conf-date | conf-loc" mode="metadata-inline">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="article-id" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:choose>
          <xsl:when test="@pub-id-type='art-access-id'">Accession ID</xsl:when>
          <xsl:when test="@pub-id-type='coden'">Coden</xsl:when>
          <xsl:when test="@pub-id-type='doi'">DOI</xsl:when>
          <xsl:when test="@pub-id-type='manuscript'">Manuscript ID</xsl:when>
          <xsl:when test="@pub-id-type='medline'">Medline ID</xsl:when>
          <xsl:when test="@pub-id-type='pii'">Publisher Item ID</xsl:when>
          <xsl:when test="@pub-id-type='pmid'">PubMed ID</xsl:when>
          <xsl:when test="@pub-id-type='publisher-id'">Publisher ID</xsl:when>
          <xsl:when test="@pub-id-type='sici'">Serial Item and Contribution ID</xsl:when>
          <xsl:when test="@pub-id-type='doaj'">DOAJ ID</xsl:when>
          <xsl:when test="@pub-id-type='arXiv'">arXiv.org ID</xsl:when>
          <xsl:otherwise>
            <xsl:text>Article Id</xsl:text>
            <xsl:for-each select="@pub-id-type">
              <xsl:text> (</xsl:text>
              <span class="data">
                <xsl:value-of select="."/>
              </span>
              <xsl:text>)</xsl:text>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="contract-num" mode="metadata">
    <!-- only in 2.3 -->
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Contract</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="contract-sponsor" mode="metadata">
    <!-- only in 2.3 -->
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Contract Sponsor</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="grant-num" mode="metadata">
    <!-- only in 2.3 -->
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Grant Number</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="grant-sponsor" mode="metadata">
    <!-- only in 2.3 -->
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Grant Sponsor</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="award-group" mode="metadata">
    <!-- includes (funding-source*, award-id*, principal-award-recipient*, principal-investigator*) -->
    <xsl:apply-templates mode="metadata"/>
  </xsl:template>
  <xsl:template match="funding-source" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Funded by</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="award-id" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Award ID</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="principal-award-recipient" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Award Recipient</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="principal-investigator" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Principal Investigator</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="funding-statement" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Funding</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="open-access" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Open Access</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="title-group" mode="metadata">
    <!-- content model:
    article-title, subtitle*, trans-title-group*, alt-title*, fn-group? -->
    <!-- trans-title and trans-subtitle included for 2.3 -->
    <br/>
    <xsl:apply-templates select="article-title | subtitle | trans-title-group |       trans-title | trans-subtitle" mode="metadata"/>
    <xsl:if test="alt-title | fn-group">
      <div class="document-title-notes metadata-group">
        <xsl:apply-templates select="alt-title | fn-group" mode="metadata"/>
      </div>
    </xsl:if>
  </xsl:template>
  <xsl:template match="title-group/article-title" mode="metadata">
	  
    <h1 class="document-title">
      <xsl:apply-templates/>
      <xsl:if test="../subtitle">:</xsl:if>
    </h1>
  </xsl:template>
  <xsl:template match="title-group/subtitle | trans-title-group/subtitle" mode="metadata">
    <h2 class="document-title">
      <xsl:apply-templates/>
    </h2>
  </xsl:template>
  <xsl:template match="title-group/trans-title-group" mode="metadata">
    <!-- content model: (trans-title, trans-subtitle*) -->
    <h2>
      <xsl:apply-templates mode="metadata"/>
    </h2>
  </xsl:template>
  <xsl:template match="title-group/alt-title" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Alternative title</xsl:text>
        <xsl:for-each select="@alt-title-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="title-group/fn-group" mode="metadata">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template mode="metadata" match="journal-meta/contrib-group">
    <xsl:for-each select="contrib">
      <xsl:variable name="contrib-identification">
        <xsl:call-template name="contrib-identify"/>
      </xsl:variable>
      <!-- placing the div only if it has content -->
      <!-- the extra call to string() makes it type-safe in a type-aware
           XSLT 2.0 engine -->
      <xsl:if test="normalize-space(string($contrib-identification))">
        <xsl:copy-of select="$contrib-identification"/>
      </xsl:if>
      <xsl:variable name="contrib-info">
        <xsl:call-template name="contrib-info"/>
      </xsl:variable>
      <!-- placing the div only if it has content -->
      <xsl:if test="normalize-space(string($contrib-info))">
        <xsl:copy-of select="$contrib-info"/>
      </xsl:if>
    </xsl:for-each>
    <xsl:if test="*[not(self::contrib | self::xref)]">
      <xsl:apply-templates mode="metadata" select="*[not(self::contrib | self::xref)]"/>
    </xsl:if>
  </xsl:template>
  <xsl:template mode="metadata" match="article-meta/contrib-group">
    <!-- content model of contrib-group:
        (contrib+, 
        (address | aff | author-comment | bio | email |
        ext-link | on-behalf-of | role | uri | xref)*) -->
    <!-- each contrib makes a row: name at left, details at right -->
    <xsl:for-each select="contrib">
      <!--  content model of contrib:
          ((contrib-id)*,
           (anonymous | collab | collab-alternatives | name | name-alternatives)*,
           (degrees)*,
           (address | aff | aff-alternatives | author-comment | bio | email |
            ext-link | on-behalf-of | role | uri | xref)*)       -->
          <div class="cell author" style="text-align: right;">
            <xsl:call-template name="contrib-identify">
              <!-- handles (contrib-id)*,
                (anonymous | collab | collab-alternatives |
                 name | name-alternatives | degrees | xref) -->
            </xsl:call-template>
          </div>
          <div class="cell" style="witdh:30;">
            <xsl:call-template name="contrib-info">
              <!-- handles
                   (address | aff | author-comment | bio | email |
                    ext-link | on-behalf-of | role | uri) -->
            </xsl:call-template>
          </div>
    </xsl:for-each>
	<xsl:for-each select="contrib">
          <div class="cell2" style="witdh:30;">
            <xsl:call-template name="contrib-info">
            </xsl:call-template>
          </div>
    </xsl:for-each>  
	  
    <!-- end of contrib -->
    <xsl:variable name="misc-contrib-data" select="*[not(self::contrib | self::xref)]"/>
    <xsl:if test="$misc-contrib-data">
      <div class="metadata two-column table">
          <div class="cell afiliacion">
            <div class="metadata-group">
              <xsl:apply-templates mode="metadata" select="$misc-contrib-data"/>
            </div>
          </div>
      </div>
    </xsl:if>
  </xsl:template>
  <xsl:template name="contrib-identify">
    <!-- Placed in a left-hand pane  -->
    <!--handles
    (anonymous | collab | collab-alternatives |
    name | name-alternatives | degrees | xref)
    and @equal-contrib -->
    <div class="metadata-group">
      <xsl:for-each select="anonymous |         collab | collab-alternatives/* | name | name-alternatives/*">
        <xsl:call-template name="metadata-entry">
          <xsl:with-param name="contents">
            <xsl:if test="position() = 1">
              <!-- a named anchor for the contrib goes with its
              first member -->
              <xsl:call-template name="named-anchor"/>
              <!-- so do any contrib-ids -->
              <xsl:apply-templates mode="metadata-inline" select="../contrib-id"/>
            </xsl:if>
            <xsl:apply-templates select="." mode="metadata-inline"/>
            <xsl:if test="position() = last()">
              <xsl:apply-templates mode="metadata-inline" select="degrees | xref"/>
              <!-- xrefs in the parent contrib-group go with the last member
              of *each* contrib in the group -->
              <xsl:apply-templates mode="metadata-inline" select="following-sibling::xref"/>
            </xsl:if>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:for-each>
      <xsl:if test="@equal-contrib='yes'">
        <xsl:call-template name="metadata-entry">
          <xsl:with-param name="contents">
            <span class="generated">(Equal contributor)</span>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:if>
    </div>
  </xsl:template>
  <xsl:template match="anonymous" mode="metadata-inline">
    <xsl:text>Anonymous</xsl:text>
  </xsl:template>
  <xsl:template match="collab |                        collab-alternatives/*" mode="metadata-inline">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="contrib/name |                        contrib/name-alternatives/*" mode="metadata-inline">
    <xsl:apply-templates select="."/>
  </xsl:template>
  <xsl:template match="degrees" mode="metadata-inline">
    <xsl:text>, </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="xref" mode="metadata-inline">
    <!-- These are not expected to appear in mixed content, so brackets are provided 
		Pintar link para mostrar afiliaciones del autor
	-->
	  <xsl:text> </xsl:text> 
	  <xsl:apply-templates select="."/>
	  <xsl:text> ;</xsl:text> 
  </xsl:template>
  <xsl:template match="contrib-id" mode="metadata-inline">
    <sup>
		<span class="generated">[</span>
    		<xsl:apply-templates select="."/>
    	<span class="generated">] </span>
	</sup>;
  </xsl:template>
  <xsl:template name="contrib-info">
    <!-- Placed in a right-hand pane -->
    <div class="metadata-group">
      <xsl:apply-templates mode="metadata" select="address | aff | author-comment | bio | email |                 ext-link | on-behalf-of | role | uri"/>
    </div>
  </xsl:template>
  <xsl:template mode="metadata" match="address[not(addr-line) or not(*[2])]">
    <!-- when we have no addr-line or a single child, we generate
         a single unlabelled line -->
    <xsl:call-template name="metadata-entry">
      <xsl:with-param name="contents">
        <xsl:call-template name="address-line"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="address" mode="metadata">
    <!-- when we have an addr-line we generate an unlabelled block -->
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="contents">
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template mode="metadata" priority="2" match="address/*">
    <!-- being sure to override other templates for these
         element types -->
    <xsl:call-template name="metadata-entry"/>
  </xsl:template>
  <xsl:template match="aff" mode="metadata">
    <xsl:call-template name="metadata-entry">
      <xsl:with-param name="contents">
        <xsl:call-template name="named-anchor"/>
        <xsl:apply-templates/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="author-comment" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Comment</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="bio" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Bio</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="on-behalf-of" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">On behalf of</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="role" mode="metadata">
    <xsl:call-template name="metadata-entry"/>
  </xsl:template>
  <xsl:template match="author-notes" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Sobre el autor</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:call-template name="named-anchor"/>
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="author-notes/corresp" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:call-template name="named-anchor"/>
        <xsl:text>Autor de correspondencia</xsl:text>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="author-notes/fn | author-notes/p" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:apply-templates select="@fn-type"/>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:call-template name="named-anchor"/>
        <xsl:apply-templates/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="supplementary-material" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Supplementary material</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="article-categories" mode="metadata">
    <xsl:apply-templates mode="metadata"/>
  </xsl:template>
  <xsl:template match="article-categories/subj-group" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Categorías</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="subj-group" mode="metadata">
    <xsl:apply-templates mode="metadata"/>
  </xsl:template>
  <xsl:template match="subj-group/subj-group" mode="metadata">
    <div class="metadata-area">
      <xsl:apply-templates mode="metadata"/>
    </div>
  </xsl:template>
  <xsl:template match="subj-group/subject" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Sección</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="series-title" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Series title</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="series-text" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">Series description</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="kwd-group" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">
		  <!--
        <xsl:apply-templates select="title|label" mode="metadata-inline"/>
			-->
        <xsl:if test="not(title|label)">Keywords</xsl:if>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="title" mode="metadata">
    <xsl:apply-templates select="."/>
  </xsl:template>
  <xsl:template match="kwd" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">    </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="nested-kwd" mode="metadata">
    <ul class="nested-kwd">
      <xsl:apply-templates mode="metadata"/>
    </ul>
  </xsl:template>
  <xsl:template match="nested-kwd/kwd" mode="metadata">
    <li class="kwd">
      <xsl:apply-templates/>
    </li>
  </xsl:template>
  <xsl:template match="compound-kwd" mode="metadata">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Compound keyword</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="compound-kwd-part" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:text>Keyword part</xsl:text>
        <xsl:for-each select="@content-type">
          <xsl:text> (</xsl:text>
          <span class="data">
            <xsl:value-of select="."/>
          </span>
          <xsl:text>)</xsl:text>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="counts" mode="metadata">
    <!-- fig-count?, table-count?, equation-count?, ref-count?,
         page-count?, word-count? --> 
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Información Adicional</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template mode="metadata" match="count | fig-count | table-count | equation-count |            ref-count | page-count | word-count">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <xsl:apply-templates select="." mode="metadata-label"/>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:value-of select="@count"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="count" mode="metadata-label">
    <xsl:text>Count</xsl:text>
    <xsl:for-each select="@count-type">
      <xsl:text> (</xsl:text>
      <span class="data">
        <xsl:value-of select="."/>
      </span>
      <xsl:text>)</xsl:text>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="fig-count" mode="metadata-label">Figuras</xsl:template>
  <xsl:template match="table-count" mode="metadata-label">Tablas</xsl:template>
  <xsl:template match="equation-count" mode="metadata-label">Equaciones</xsl:template>
  <xsl:template match="ref-count" mode="metadata-label">Referencias</xsl:template>
  <xsl:template match="page-count" mode="metadata-label">Páginas</xsl:template>
  <xsl:template match="word-count" mode="metadata-label">Palabras</xsl:template>
  <xsl:template mode="metadata" match="custom-meta-group | custom-meta-wrap">
    <xsl:call-template name="metadata-area">
      <xsl:with-param name="label">Custom metadata</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates mode="metadata"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="custom-meta" mode="metadata">
    <xsl:call-template name="metadata-labeled-entry">
      <xsl:with-param name="label">
        <span class="data">
          <xsl:apply-templates select="meta-name" mode="metadata-inline"/>
        </span>
      </xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates select="meta-value" mode="metadata-inline"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="meta-name | meta-value" mode="metadata-inline">
    <xsl:apply-templates/>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  REGULAR (DEFAULT) MODE                                       -->
  <!-- ============================================================= -->
  <xsl:template match="sec">
    <div class="section">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="title"/>
      <xsl:apply-templates select="sec-meta"/>
      <xsl:apply-templates mode="drop-title"/>
    </div>
  </xsl:template>
  <xsl:template match="*" mode="drop-title">
    <xsl:apply-templates select="."/>
  </xsl:template>
  <xsl:template match="title | sec-meta" mode="drop-title"/>
  <xsl:template match="app">
    <div class="section app">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="." mode="label"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="ref-list" name="ref-list">
    <div class="section ref-list" id="referenciaB">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="." mode="label"/>
      <xsl:apply-templates select="*[not(self::ref | self::ref-list)]"/>
      <xsl:if test="ref">
        <div class="ref-list table">
          <xsl:apply-templates select="ref"/>
        </div>
      </xsl:if>
      <xsl:apply-templates select="ref-list"/>
    </div>
  </xsl:template>
  <xsl:template match="sec-meta">
    <div class="section-metadata">
      <!-- includes contrib-group | permissions | kwd-group -->
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="sec-meta/contrib-group">
    <xsl:apply-templates mode="metadata"/>
  </xsl:template>
  <xsl:template match="sec-meta/kwd-group">
    <!-- matches only if contrib-group has only contrib children -->
    <xsl:apply-templates select="." mode="metadata"/>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Titles                                                       -->
  <!-- ============================================================= -->
  <xsl:template name="main-title" match="abstract/title | body/*/title |            back/title | back[not(title)]/*/title">
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <xsl:if test="normalize-space(string($contents))">
      <!-- coding defensively since empty titles make glitchy HTML -->
      <h2 class="main-title">
        <xsl:copy-of select="$contents"/>
      </h2>
    </xsl:if>
  </xsl:template>
  <xsl:template name="section-title" match="abstract/*/title | body/*/*/title |          back[title]/*/title | back[not(title)]/*/*/title">
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <xsl:if test="normalize-space(string($contents))">
      <!-- coding defensively since empty titles make glitchy HTML -->
      <h3 class="section-title">
        <xsl:copy-of select="$contents"/>
      </h3>
    </xsl:if>
  </xsl:template>
  <xsl:template name="subsection-title" match="abstract/*/*/title | body/*/*/*/title |          back[title]/*/*/title | back[not(title)]/*/*/*/title">
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <xsl:if test="normalize-space(string($contents))">
      <!-- coding defensively since empty titles make glitchy HTML -->
      <h4 class="subsection-title">
        <xsl:copy-of select="$contents"/>
      </h4>
    </xsl:if>
  </xsl:template>
  <xsl:template name="block-title" priority="2" match="list/title | def-list/title | boxed-text/title |            verse-group/title | glossary/title | gloss-group/title | kwd-group/title">
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <xsl:if test="normalize-space(string($contents))">
      <!-- coding defensively since empty titles make glitchy HTML -->
      <h4 class="block-title">
        <xsl:copy-of select="$contents"/>
      </h4>
    </xsl:if>
  </xsl:template>
  <!-- default: any other titles found -->
  <xsl:template match="title">
    <xsl:if test="normalize-space(string(.))">
      <h3 class="title">
        <xsl:apply-templates/>
      </h3>
    </xsl:if>
  </xsl:template>
  <xsl:template match="subtitle">
    <xsl:if test="normalize-space(string(.))">
      <h5 class="subtitle">
        <xsl:apply-templates/>
      </h5>
    </xsl:if>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Figures, lists and block-level objectS                       -->
  <!-- ============================================================= -->
  <xsl:template match="address">
    <xsl:choose>
      <!-- address appears as a sequence of inline elements if
           it has no addr-line and the parent may contain text -->
      <xsl:when test="not(addr-line) and         (parent::collab | parent::p | parent::license-p |          parent::named-content | parent::styled-content)">
        <xsl:call-template name="address-line"/>
      </xsl:when>
      <xsl:otherwise>
        <div class="address">
          <xsl:apply-templates/>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="address-line">
    <!-- emits element children in a simple comma-delimited sequence -->
    <xsl:for-each select="*">
      <xsl:if test="position() &gt; 1">, </xsl:if>
      <xsl:apply-templates/>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="address/*">
    <p class="address-line">
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="array | disp-formula-group | fig-group |     fn-group | license | long-desc | open-access | sig-block |      table-wrap-foot | table-wrap-group">
    <div class="{local-name()}">
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="attrib">
    <p class="attrib">
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="boxed-text | chem-struct-wrap | fig |                        table-wrap | chem-struct-wrapper">
    <!-- chem-struct-wrapper is from NLM 2.3 -->
    <xsl:variable name="gi">
      <xsl:choose>
        <xsl:when test="self::chem-struct-wrapper">chem-struct-wrap</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="local-name(.)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <div class="{$gi} panel">
      <xsl:if test="not(@position != 'float')">
        <!-- the test respects @position='float' as the default -->
        <xsl:attribute name="style">border: thin solid #FFF</xsl:attribute>
      </xsl:if>
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="." mode="label"/>
      <xsl:apply-templates/>
      <xsl:apply-templates mode="footnote" select="self::table-wrap//fn[not(ancestor::table-wrap-foot)]"/>
    </div>
  </xsl:template>
  <xsl:template match="caption">
    <div class="caption">
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="disp-formula | statement">
    <div class="{local-name()} panel">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="." mode="label"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="glossary | gloss-group">
    <!-- gloss-group is from 2.3 -->
    <div class="glossary">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="label | title"/>
      <xsl:if test="not(normalize-space(string(title)))">
        <xsl:call-template name="block-title">
          <xsl:with-param name="contents">
            <span class="generated">Glossary</span>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <xsl:apply-templates select="*[not(self::label|self::title)]"/>
    </div>
  </xsl:template>
  <xsl:template match="textual-form">
    <p class="textual-form">
      <span class="generated">[Textual form] </span>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="glossary/glossary | gloss-group/gloss-group">
    <!-- the same document shouldn't have both types -->
    <div class="glossary">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="graphic | inline-graphic">
    <xsl:apply-templates/>
    <img alt="{@xlink:href}" class="imagen">
      <xsl:for-each select="alt-text">
        <xsl:attribute name="alt">
          <xsl:value-of select="normalize-space(string(.))"/>
        </xsl:attribute>
      </xsl:for-each>
      <xsl:call-template name="assign-src"/>
    </img>
  </xsl:template>
  <xsl:template match="alt-text">
    <!-- handled with graphic or inline-graphic -->
  </xsl:template>
  <xsl:template match="list">
    <div class="list">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="label | title"/>
      <xsl:apply-templates select="." mode="list"/>
    </div>
  </xsl:template>
  <xsl:template priority="2" mode="list" match="list[@list-type='simple' or list-item/label]">
    <ul style="list-style-type: none">
      <xsl:apply-templates select="list-item"/>
    </ul>
  </xsl:template>
  <xsl:template match="list[@list-type='bullet' or not(@list-type)]" mode="list">
    <ul>
      <xsl:apply-templates select="list-item"/>
    </ul>
  </xsl:template>
  <xsl:template match="list" mode="list">
    <xsl:variable name="style">
      <xsl:choose>
        <xsl:when test="@list-type='alpha-lower'">lower-alpha</xsl:when>
        <xsl:when test="@list-type='alpha-upper'">upper-alpha</xsl:when>
        <xsl:when test="@list-type='roman-lower'">lower-roman</xsl:when>
        <xsl:when test="@list-type='roman-upper'">upper-roman</xsl:when>
        <xsl:otherwise>decimal</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <ol style="list-style-type: {$style}">
      <xsl:apply-templates select="list-item"/>
    </ol>
  </xsl:template>
  <xsl:template match="list-item">
    <li>
      <xsl:apply-templates/>
    </li>
  </xsl:template>
  <xsl:template match="list-item/label">
    <!-- if the next sibling is a p, the label will be called as
	       a run-in -->
    <xsl:if test="following-sibling::*[1][not(self::p)]">
      <xsl:call-template name="label"/>
    </xsl:if>
  </xsl:template>
  <xsl:template match="media">
    <a class="link">
      <xsl:call-template name="assign-id"/>
      <xsl:call-template name="assign-href"/>
      <xsl:apply-templates/>
    </a>
  </xsl:template>
  <xsl:template match="p | license-p">
    <p>
      <xsl:if test="not(preceding-sibling::*)">
        <xsl:attribute name="class">first</xsl:attribute>
      </xsl:if>
      <xsl:call-template name="assign-id"/>
      <xsl:apply-templates select="@content-type"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="@content-type">
    <!-- <span class="generated">[</span>
    <xsl:value-of select="."/>
    <span class="generated">] </span> -->
  </xsl:template>
  <xsl:template match="list-item/p[not(preceding-sibling::*[not(self::label)])]">
    <p>
      <xsl:call-template name="assign-id"/>
      <xsl:for-each select="preceding-sibling::label">
        <span class="label">
          <xsl:apply-templates/>
        </span>
        <xsl:text> </xsl:text>
      </xsl:for-each>
      <xsl:apply-templates select="@content-type"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="product">
    <p class="product">
      <xsl:call-template name="assign-id"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="permissions">
    <div class="permissions">
      <xsl:apply-templates select="copyright-statement"/>
      <xsl:if test="copyright-year | copyright-holder">
        <p class="copyright">
          <span class="generated">Copyright</span>
          <xsl:for-each select="copyright-year | copyright-holder">
            <xsl:apply-templates/>
            <xsl:if test="not(position()=last())">
              <span class="generated">, </span>
            </xsl:if>
          </xsl:for-each>
        </p>
      </xsl:if>
      <xsl:apply-templates select="license"/>
    </div>
  </xsl:template>
  <xsl:template match="copyright-statement">
    <p class="copyright">
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="def-list">
    <div class="def-list">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="label | title"/>
      <div class="def-list table">
        <xsl:if test="term-head|def-head">
          <div class="row2">
            <div class="cell def-list-head">
              <xsl:apply-templates select="term-head"/>
            </div>
            <div class="cell def-list-head">
              <xsl:apply-templates select="def-head"/>
            </div>
          </div>
        </xsl:if>
        <xsl:apply-templates select="def-item"/>
      </div>
      <xsl:apply-templates select="def-list"/>
    </div>
  </xsl:template>
  <xsl:template match="def-item">
    <div class="def-item row2">
      <xsl:call-template name="assign-id"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="term">
    <div class="def-term cell">
      <xsl:call-template name="assign-id"/>
      <p>
        <xsl:apply-templates/>
      </p>
    </div>
  </xsl:template>
  <xsl:template match="def">
    <div class="def-def cell">
      <xsl:call-template name="assign-id"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="disp-quote">
    <div class="blockquote">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="preformat">
    <pre class="preformat">
      <xsl:apply-templates/>
    </pre>
  </xsl:template>
  <xsl:template match="alternatives | name-alternatives | collab-alternatives | aff-alternatives">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="citation-alternatives" priority="2">
    <!-- priority bumped to supersede match on ref/* -->
    <!-- may appear in license-p, p, ref, td, th, title  -->
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="ref">
    <div class="row2">
      <div class="ref-label cell">
        <p class="ref-label">
          <xsl:apply-templates select="." mode="label"/>
          <xsl:text> </xsl:text>
          <!-- space forces vertical alignment of the paragraph -->
          <xsl:call-template name="named-anchor"/>
        </p>
      </div>
      <div class="ref-content cell">
        <xsl:apply-templates/>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="ref/* | ref/citation-alternatives/*" priority="0">
    <!-- should match mixed-citation, element-citation, nlm-citation,
         or citation (in 2.3); note and label should be matched below;
         citation-alternatives is handled elsewhere -->
    <p class="citation">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="ref/note" priority="2">
    <xsl:param name="label" select="''"/>
    <xsl:if test="normalize-space(string($label))       and not(preceding-sibling::*[not(self::label)])">
      <p class="label">
        <xsl:copy-of select="$label"/>
      </p>
    </xsl:if>
    <div class="note">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="app/related-article |     app-group/related-article | bio/related-article |      body/related-article | boxed-text/related-article |      disp-quote/related-article | glossary/related-article |     gloss-group/related-article |     ref-list/related-article | sec/related-article">
    <xsl:apply-templates select="." mode="metadata"/>
  </xsl:template>
  <xsl:template match="app/related-object |     app-group/related-object | bio/related-object |     body/related-object | boxed-text/related-object |      disp-quote/related-object | glossary/related-object |     gloss-group/related-article |     ref-list/related-object | sec/related-object">
    <xsl:apply-templates select="." mode="metadata"/>
  </xsl:template>
  <xsl:template match="speech">
    <div class="speech">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates mode="speech"/>
    </div>
  </xsl:template>
  <xsl:template match="speech/speaker" mode="speech"/>
  <xsl:template match="speech/p" mode="speech">
    <p>
      <xsl:apply-templates select="self::p[not(preceding-sibling::p)]/../speaker"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template match="speech/speaker">
    <b>
      <xsl:apply-templates/>
    </b>
    <span class="generated">: </span>
  </xsl:template>
  <xsl:template match="supplementary-material">
    <div class="panel">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates select="." mode="label"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="tex-math">
    <span class="tex-math">
      <span class="generated">[TeX:] </span>
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="mml:*">
    <!-- this stylesheet simply copies MathML through. If your browser
         supports it, you will get it -->
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:apply-templates/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="verse-group">
    <div class="verse">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="verse-line">
    <p class="verse-line">
      <xsl:apply-templates/>
    </p>
  </xsl:template>
	
  <xsl:template match="corresp/label | chem-struct/label |     element-citation/label | mixed-citation/label | citation/label">
	  <sup class="circulo">
    	<xsl:apply-templates/>
	  </sup>	  
  </xsl:template>
  <xsl:template match="aff/label">
	  <sup class="circulo azul">
    	<xsl:apply-templates/>
	  </sup>	  
  </xsl:template>
	
  <xsl:template match="app/label | boxed-text/label |     chem-struct-wrap/label | chem-struct-wrapper/label |     disp-formula/label | fig/label | fn/label | ref/label |     statement/label | supplementary-material/label | table-wrap/label" priority="2">
    <!-- suppressed, since acquired by their parents in mode="label" -->
  </xsl:template>
  <xsl:template match="p/label">
    <span class="label">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="label" name="label">
    <!-- other labels are displayed as blocks -->
    <h5 class="label">
      <xsl:apply-templates/>
    </h5>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  TABLES                                                       -->
  <!-- ============================================================= -->
  <!--  Tables are already in XHTML, and can simply be copied
        through                                                      -->
  <xsl:template match="table | thead | tbody | tfoot |       col | colgroup | tr | th | td">
    <xsl:copy>
      <xsl:apply-templates select="@*" mode="table-copy"/>
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="array/tbody">
    <table class="tabla">
      <xsl:copy>
        <xsl:apply-templates select="@*" mode="table-copy"/>
        <xsl:call-template name="named-anchor"/>
        <xsl:apply-templates/>
      </xsl:copy>
    </table>
  </xsl:template>
  <xsl:template match="@*" mode="table-copy">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="@content-type" mode="table-copy"/>
  <!-- ============================================================= -->
  <!--  INLINE MISCELLANEOUS                                         -->
  <!-- ============================================================= -->
  <!--  Templates strictly for formatting follow; these are templates
        to handle various inline structures -->
  <xsl:template match="abbrev">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="abbrev[normalize-space(string(@xlink:href))]">
    <a class="link">
      <xsl:call-template name="assign-href"/>
      <xsl:apply-templates/>
    </a>
  </xsl:template>
  <xsl:template match="abbrev/def">
    <xsl:text>[</xsl:text>
    <xsl:apply-templates/>
    <xsl:text>]</xsl:text>
  </xsl:template>
  <xsl:template match="p/address | license-p/address |     named-content/p | styled-content/p">
    <xsl:apply-templates mode="inline"/>
  </xsl:template>
  <xsl:template match="address/*" mode="inline">
    <xsl:if test="preceding-sibling::*">
      <span class="generated">, </span>
    </xsl:if>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="award-id">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="award-id[normalize-space(string(@rid))]">
    <a href="#{@rid}" class="link">
      <xsl:apply-templates/>
    </a>
  </xsl:template>
  <xsl:template match="break">
    <br class="br"/>
  </xsl:template>
  <xsl:template match="email">
    <xsl:text> </xsl:text>
	<xsl:text>&#60;</xsl:text>  
	<xsl:apply-templates/>
	<xsl:text>&#62;</xsl:text>
	  <a href="mailto:{.}"> 
			<xsl:attribute name="Title">
				<xsl:apply-templates/>
			</xsl:attribute>	
	  		<img src="data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTkuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeD0iMHB4IiB5PSIwcHgiIHZpZXdCb3g9IjAgMCAzOTguNDY5IDM5OC40NjkiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDM5OC40NjkgMzk4LjQ2OTsiIHhtbDpzcGFjZT0icHJlc2VydmUiIHdpZHRoPSIxNnB4IiBoZWlnaHQ9IjE2cHgiPgo8Zz4KCTxnPgoJCTxwYXRoIGQ9Ik0zOTEuNjgzLDIxOC44NzJsLTU0LjkwMy01NC45MDNjLTMuNzkyLTMuNzkxLTcuNzI5LTUuNzE0LTExLjcwMi01LjcxNGMtNS4xMzksMC04LjU0MiwyLjkxMS0xMC40MjYsNi45Mzd2LTUwLjEzOSAgICBjMC0xNS45NTYtMTIuOTgxLTI4LjkzOC0yOC45MzgtMjguOTM4SDI4LjkzOEMxMi45ODEsODYuMTE1LDAsOTkuMDk2LDAsMTE1LjA1MnY2LjA4OGMwLDAuMDAzLDAsMC4wMDUsMCwwLjAwOHYxNTQuNDA0ICAgIGMwLDE1Ljk1NiwxMi45ODEsMjguOTM4LDI4LjkzOCwyOC45MzhoMjU2Ljc3NWMxMi40MjIsMCwyMy4wMzktNy44NjksMjcuMTM0LTE4Ljg4M3YxMS4xMTljMCw3Ljc3OCwzLjc4MiwxNS42MjcsMTIuMjMsMTUuNjI4ICAgIGMzLjk3MywwLDcuOTEtMS45MjMsMTEuNzAyLTUuNzE0bDU0LjkwMy01NC45MDNjNC4zNzctNC4zNzcsNi43ODctMTAuMjEzLDYuNzg3LTE2LjQzMyAgICBDMzk4LjQ2OSwyMjkuMDg0LDM5Ni4wNiwyMjMuMjQ4LDM5MS42ODMsMjE4Ljg3MnogTTE1LDExNS4wNTJjMC03LjY4Niw2LjI1Mi0xMy45MzgsMTMuOTM4LTEzLjkzOGgyNTYuNzc1ICAgIGM3LjY4NiwwLDEzLjkzOCw2LjI1MiwxMy45MzgsMTMuOTM4djEuOTgybC0xNDIuMzI2LDkxLjA0OUwxNSwxMTcuMDM0VjExNS4wNTJ6IE0yOTkuNjUxLDEzNC44NDF2NTUuNzE2aC02MC41OCAgICBjLTkuMTc0LDAtMTcuMTIyLDUuMzQ1LTIwLjg5NywxMy4wODNsLTEzLjAzMy04LjMzOEwyOTkuNjUxLDEzNC44NDF6IE0xNSwxMzQuODQxbDk0LjUxMSw2MC40NjFMMTUsMjU1Ljc2M1YxMzQuODQxeiAgICAgTTI4NS43MTMsMjg5LjQ5SDI4LjkzOGMtNy42ODYsMC0xMy45MzgtNi4yNTItMTMuOTM4LTEzLjkzOHYtMS45ODNsMTA4LjQyOC02OS4zNjRsMjkuODU1LDE5LjEgICAgYzEuMjMyLDAuNzg4LDIuNjM3LDEuMTgzLDQuMDQyLDEuMTgzczIuODEtMC4zOTUsNC4wNDItMS4xODNsMjkuODU1LTE5LjA5OWwyNC42MSwxNS43NDJ2MzYuODY2ICAgIGMwLDEyLjgxNCwxMC40MjUsMjMuMjM5LDIzLjIzOSwyMy4yMzloNTkuODJDMjk3LjAxNCwyODUuNTM0LDI5MS44MjIsMjg5LjQ5LDI4NS43MTMsMjg5LjQ5eiBNMzgxLjA3NiwyNDEuMTNsLTUzLjIyOSw1My4yMjkgICAgdi0yMS44MDZjMC00LjE0My0zLjM1OC03LjUtNy41LTcuNWgtODEuMjc2Yy00LjU0MywwLTguMjM5LTMuNjk2LTguMjM5LTguMjM5di00My4wMmMwLTQuNTQzLDMuNjk2LTguMjM4LDguMjM5LTguMjM4aDgxLjI3NiAgICBjNC4xNDIsMCw3LjUtMy4zNTcsNy41LTcuNVYxNzYuMjVsNTMuMjI5LDUzLjIyOWMxLjU0MywxLjU0MywyLjM5NCwzLjYxMiwyLjM5NCw1LjgyNSAgICBDMzgzLjQ3LDIzNy41MTYsMzgyLjYyLDIzOS41ODYsMzgxLjA3NiwyNDEuMTN6IiBmaWxsPSIjMDA2REYwIi8+Cgk8L2c+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPC9zdmc+Cg==" />
    </a>
	<xsl:text>. </xsl:text>
  </xsl:template>
  <xsl:template match="ext-link | uri | inline-supplementary-material">
    <xsl:text>  </xsl:text>
    <a target="xrefwindow" class="link">
      <xsl:attribute name="href">
        <xsl:value-of select="."/>
      </xsl:attribute>
      <!-- if an @href is present, it overrides the href
           just attached -->
      <xsl:call-template name="assign-href"/>
      <xsl:call-template name="assign-id"/>
      <xsl:apply-templates/>
      <xsl:if test="not(normalize-space(string(.)))">
        <xsl:value-of select="@xlink:href"/>
      </xsl:if>
    </a>
  </xsl:template>
  <xsl:template match="funding-source">
    <span class="funding-source">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="hr">
    <hr class="hr"/>
  </xsl:template>
  <!-- inline-graphic is handled above, with graphic -->
  <xsl:template match="inline-formula | chem-struct">
    <span class="{local-name()}">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="chem-struct-wrap/chem-struct | chem-struct-wrapper/chem-struct">
    <div class="{local-name()}">
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="milestone-start | milestone-end">
    <span class="{substring-after(local-name(),'milestone-')}">
      <xsl:comment>
        <xsl:value-of select="@rationale"/>
      </xsl:comment>
    </span>
  </xsl:template>
  <xsl:template match="object-id">
    <span class="{local-name()}">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <!-- preformat is handled above -->
  <xsl:template match="sig">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="target">
    <xsl:call-template name="named-anchor"/>
  </xsl:template>
  <xsl:template match="styled-content">
    <span>
      <xsl:copy-of select="@style"/>
      <xsl:for-each select="@style-type">
        <xsl:attribute name="class">
          <xsl:value-of select="."/>
        </xsl:attribute>
      </xsl:for-each>
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="named-content">
    <span>
      <xsl:for-each select="@content-type">
        <xsl:attribute name="class">
          <xsl:value-of select="translate(.,' ','-')"/>
        </xsl:attribute>
      </xsl:for-each>
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="private-char">
    <span>
      <xsl:for-each select="@description">
        <xsl:attribute name="title">
          <xsl:value-of select="."/>
        </xsl:attribute>
      </xsl:for-each>
      <span class="generated">[Private character </span>
      <xsl:for-each select="@name">
        <xsl:text> </xsl:text>
        <xsl:value-of select="."/>
      </xsl:for-each>
      <span class="generated">]</span>
    </span>
  </xsl:template>
  <xsl:template match="glyph-data | glyph-ref">
    <span class="generated">(Glyph not rendered)</span>
  </xsl:template>
  <xsl:template match="related-article">
    <span class="generated">[Related article:] </span>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="related-object">
    <span class="generated">[Related object:] </span>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="xref[not(normalize-space(string(.)))]">
    <a href="#{@rid}" class="link">
      <xsl:apply-templates select="key('element-by-id',@rid)" mode="label-text">
        <xsl:with-param name="warning" select="true()"/>
      </xsl:apply-templates>
    </a>
  </xsl:template>
	<!--Pinta link de afiliaciones -->
  <xsl:template match="xref">
    <a href="#{@rid}" class="link" title="Ir...">
		<xsl:choose>
			<xsl:when test="starts-with(@rid, 'aff')">
				<xsl:attribute name="title">Afiliación</xsl:attribute>
				<img class="icon icons8-Contact-Card-Filled" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABD0lEQVQ4T62TQU7DMBREZ7xjxRGAE3AEyglQl02KlC6hRaInINygUuV9EHLY9gbkCO0Nwg3KtosMchGIJG4VKXhpf78/M98mei72vA9aly8kXIZAJLa73clkPhluDzXyAB1XoQwwZbiBNh0Ax/FNwKeEkgxbCqr4a4HE8D6KVta5FcCbLgHXFFQVrx9uR0UdoEeS6yZMQuH3agAJL7NxlFiX+9RPfYGEjZ9GQM1VE+D9p7NxtLAu9/R9AaC1xBaAxKAGMIYXd6PR77iscynAJwlzqW3BGL23LHQJrVnz3+9AzwISgmchNYI+CGTe2s85l69vA2OqfSDTOE6tcwmA8wN2ymkcZ9/5AFVlit6/8QsiJn0WezNosgAAAABJRU5ErkJggg==" width="16" height="16"/>
		  	</xsl:when>
			<xsl:when test="starts-with(@rid, 'ch')">
				<xsl:apply-templates/>	
		  	</xsl:when>
			<xsl:when test="starts-with(@rid, 'c')">
				<xsl:attribute name="title">Autor de correspondencia</xsl:attribute>
				<img class="icon icons8-Message-Filled" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABA0lEQVQ4T6WS0W3CQBBEZ7YBUgIdkA6gBDpIqIDQgVMBiZR840gJ35RgOnA6oATcwE60Z19i8UGEfV8n3c3bmd0lRh6O1IPvX/tKgoaC+Pa5L0k8DAFI+OgAqgDuboNoJXEREcL+QcKRRAFg8g+okVCQmANYZgAA1e62MdPhCqRx59LMtwDvo1APkOqeAD1LfCIx6zuR8G2Gwl07knf57RIQTlYAaonR3AQJcUDNfHrZqz6gIfHojjWJRYCkthKpcwglVGZ4lVDmmAmQ7UnYApj+WVfd3tu83TmR2LinRs5+xxjifrZrk5B0Dkge4wuA9W07kH+rTKs8TNyFGyNO3RkL+AF62oCAu2yIIAAAAABJRU5ErkJggg==" width="16" height="16"/>
		  	</xsl:when>
		  	<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="(@rid = 'fn1')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn1']/p/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'fn2')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn2']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn3')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn3']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn4')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn4']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn5')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn5']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn6')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn6']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn7')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn7']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn8')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn8']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn9')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn9']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn10')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn10']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn11')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn11']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn12')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn12']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn13')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn13']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn14')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn14']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn15')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn15']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn16')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn16']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn17')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn17']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn18')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn18']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn19')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn19']/p/node()"/></xsl:attribute><xsl:apply-templates/>		</xsl:when>
					<xsl:when test="(@rid = 'fn20')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn20']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn21')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn21']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn22')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn22']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn23')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn23']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn24')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn24']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn25')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn25']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn26')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn26']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn27')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn27']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn28')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn28']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					<xsl:when test="(@rid = 'fn29')"><xsl:attribute name="title">Nota: <xsl:value-of select="/article/back/fn-group/fn[@fn-type='other' and @id='fn29']/p/node()"/></xsl:attribute><xsl:apply-templates/>	</xsl:when>
					
					
					
					<xsl:when test="(@rid = 'B1')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B1']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B2')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B2']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B3')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B3']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B4')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B4']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B5')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B5']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B6')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B6']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B7')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B7']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B8')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B8']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B9')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B9']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B10')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B10']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B11')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B11']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B12')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B12']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B13')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B13']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B14')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B14']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B15')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B15']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B16')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B16']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B17')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B17']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B18')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B18']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B19')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B19']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B20')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B20']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B21')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B21']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B22')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B22']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B23')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B23']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B24')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B24']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B25')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B25']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B26')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B26']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B27')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B27']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B28')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B28']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B29')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B29']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B30')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B30']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B31')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B31']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B32')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B32']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B33')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B33']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B34')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B34']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B35')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B35']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B36')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B36']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B37')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B37']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B38')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B38']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B39')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B39']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B40')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B40']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B41')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B41']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B42')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B42']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B43')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B43']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B44')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B44']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B45')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B45']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B46')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B46']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B47')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B47']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B48')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B48']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B49')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B49']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when><xsl:when test="(@rid = 'B1')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B1']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B50')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B50']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B51')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B51']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B52')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B52']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B53')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B53']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B54')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B54']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B55')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B55']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B56')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B56']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B57')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B57']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B58')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B58']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					<xsl:when test="(@rid = 'B59')"><xsl:attribute name="title">Bibliografia: <xsl:value-of select="/article/back/ref-list/ref[@id='B59']/mixed-citation/node()"/></xsl:attribute><xsl:apply-templates/></xsl:when>
					
					
					
					<xsl:otherwise>
						<xsl:apply-templates/>	
					</xsl:otherwise>
				</xsl:choose>		
		  	</xsl:otherwise>
		</xsl:choose>
    </a>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Formatting elements                                          -->
  <!-- ============================================================= -->
  <xsl:template match="bold">
    <b>
      <xsl:apply-templates/>
    </b>
  </xsl:template>
  <xsl:template match="italic">
    <i>
      <xsl:apply-templates/>
    </i>
  </xsl:template>
  <xsl:template match="monospace">
    <tt>
      <xsl:apply-templates/>
    </tt>
  </xsl:template>
  <xsl:template match="overline">
    <span style="text-decoration: overline">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="price">
    <span class="price">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="roman">
    <span style="font-style: normal">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="sans-serif">
    <span style="font-family: sans-serif; font-size: 80%">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="sc">
    <span style="font-variant: small-caps">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="strike">
    <span style="text-decoration: line-through">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <xsl:template match="sub">
    <sub>
      <xsl:apply-templates/>
    </sub>
  </xsl:template>
  <xsl:template match="sup">
    <sup>
      <xsl:apply-templates/>
    </sup>
  </xsl:template>
  <xsl:template match="underline">
    <span style="text-decoration: underline">
      <xsl:apply-templates/>
    </span>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  BACK MATTER                                                  -->
  <!-- ============================================================= -->
  <xsl:variable name="loose-footnotes" select="//fn[not(ancestor::front|parent::fn-group|ancestor::table-wrap)]"/>
  <xsl:template name="make-back">
    <xsl:apply-templates select="back"/>
    <xsl:if test="$loose-footnotes and not(back)">
      <!-- autogenerating a section for footnotes only if there is no
           back element, and if footnotes exist for it -->
      <xsl:call-template name="footnotes"/>
    </xsl:if>
  </xsl:template>
  <xsl:template match="back">
    <!-- content model for back: 
          (label?, title*, 
          (ack | app-group | bio | fn-group | glossary | ref-list |
           notes | sec)*) -->
    <xsl:if test="not(fn-group) and $loose-footnotes">
      <xsl:call-template name="footnotes"/>
    </xsl:if>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template name="footnotes">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Notas</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:apply-templates select="$loose-footnotes" mode="footnote"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="ack">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Agradecimientos</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="app-group">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Apendices</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="back/bio">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Bibliografía</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="back/fn-group">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Notas</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="back/glossary | back/gloss-group">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Glosario</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="back/ref-list">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Referencias</xsl:with-param>
      <xsl:with-param name="contents">
        <xsl:call-template name="ref-list"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="back/notes">
    <xsl:call-template name="backmatter-section">
      <xsl:with-param name="generated-title">Notas</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="backmatter-section">
    <xsl:param name="generated-title"/>
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <div class="back-section">
      <xsl:call-template name="named-anchor"/>
      <xsl:if test="not(title) and $generated-title">
        <xsl:choose>
          <!-- The level of title depends on whether the back matter itself
               has a title -->
          <xsl:when test="ancestor::back/title">
            <xsl:call-template name="section-title">
              <xsl:with-param name="contents" select="$generated-title"/>
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="main-title">
              <xsl:with-param name="contents" select="$generated-title"/>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
      <xsl:copy-of select="$contents"/>
    </div>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  FOOTNOTES                                                    -->
  <!-- ============================================================= -->
  <xsl:template match="fn">
    <!-- Footnotes appearing outside fn-group
       generate cross-references to the footnote,
       which is displayed elsewhere -->
    <!-- Note the rules for displayed content: if any fn elements
       not inside an fn-group (the matched fn or any other) includes
       a label child, all footnotes are expected to have a label
       child. -->
    <xsl:variable name="id">
      <xsl:apply-templates select="." mode="id"/>
    </xsl:variable>
    <a href="#{$id}" class="link">
      <xsl:apply-templates select="." mode="label-text">
        <xsl:with-param name="warning" select="true()"/>
      </xsl:apply-templates>
    </a>
  </xsl:template>
  <xsl:template match="fn-group/fn | table-wrap-foot/fn |                        table-wrap-foot/fn-group/fn">
    <xsl:apply-templates select="." mode="footnote"/>
  </xsl:template>
  <xsl:template match="fn" mode="footnote">
    <div class="footnote">
      <xsl:call-template name="named-anchor"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="fn/p">
    <p>
      <xsl:call-template name="assign-id"/>
      <xsl:if test="not(preceding-sibling::p)">
        <!-- drop an inline label text into the first p -->
        <xsl:apply-templates select="parent::fn" mode="label-text"/>
        <xsl:text> </xsl:text>
      </xsl:if>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  MODE 'label-text'
	      Generates label text for elements and their cross-references -->
  <!-- ============================================================= -->
  <!--  This mode is to support auto-numbering and generating of
        labels for certain elements by the stylesheet.
  
        The logic is as follows: for any such element type, if a
        'label' element is ever present, it is expected always to be 
        present; automatic numbering is not performed on any elements
        of that type. For example, the presence of a 'label' element 
        in any 'fig' is taken to indicate that all figs should likewise
        be labeled, and any 'fig' without a label is supposed to be 
        unlabelled (and unnumbered). But if no 'fig' elements have 
        'label' children, labels with numbers are generated for all 
        figs in display.
        
        This logic applies to:
          app, boxed-text, chem-struct-wrap, disp-formula, fig, fn, 
          note, ref, statement, table-wrap.
        
        There is one exception in the case of fn elements, where
        the checking for labels (or for @symbol attributes in the
        case of this element) is performed only within its parent
        fn-group, or in the scope of all fn elements not in an
        fn-group, for fn elements appearing outside fn-group.
        
        In all cases, this logic can be altered simply by overwriting 
        templates in "label" mode for any of these elements.
        
        For other elements, a label is simply displayed if present,
        and auto-numbering is never performed.
        These elements include:
          (label appearing in line) aff, corresp, chem-struct, 
          element-citation, mixed-citation
          
          (label appearing as a block) abstract, ack, app-group, 
          author-notes, back, bio, def-list, disp-formula-group, 
          disp-quote, fn-group, glossary, graphic, kwd-group, 
          list, list-item, media, notes, ref-list, sec, 
          supplementary-material, table-wrap-group, 
          trans-abstract, verse-group -->
  <xsl:variable name="auto-label-app" select="false()"/>
  <xsl:variable name="auto-label-boxed-text" select="false()"/>
  <xsl:variable name="auto-label-chem-struct-wrap" select="false()"/>
  <xsl:variable name="auto-label-disp-formula" select="false()"/>
  <xsl:variable name="auto-label-fig" select="false()"/>
  <xsl:variable name="auto-label-ref" select="not(//ref[label])"/>
  <!-- ref elements are labeled unless any ref already has a label -->
  <xsl:variable name="auto-label-statement" select="false()"/>
  <xsl:variable name="auto-label-supplementary" select="false()"/>
  <xsl:variable name="auto-label-table-wrap" select="false()"/>
  <!--
  These variables assignments show how autolabeling can be 
  configured conditionally.
  For example: "label figures if no figures have labels" translates to
    "not(//fig[label])", which will resolve to Boolean true() when the set of
    all fig elements with labels is empty.
  
  <xsl:variable name="auto-label-app" select="not(//app[label])"/>
  <xsl:variable name="auto-label-boxed-text" select="not(//boxed-text[label])"/>
  <xsl:variable name="auto-label-chem-struct-wrap" select="not(//chem-struct-wrap[label])"/>
  <xsl:variable name="auto-label-disp-formula" select="not(//disp-formula[label])"/>
  <xsl:variable name="auto-label-fig" select="not(//fig[label])"/>
  <xsl:variable name="auto-label-ref" select="not(//ref[label])"/>
  <xsl:variable name="auto-label-statement" select="not(//statement[label])"/>
  <xsl:variable name="auto-label-supplementary"
    select="not(//supplementary-material[not(ancestor::front)][label])"/>
  <xsl:variable name="auto-label-table-wrap" select="not(//table-wrap[label])"/>
-->
  <xsl:template mode="label" match="*" name="block-label">
    <xsl:param name="contents">
      <xsl:apply-templates select="." mode="label-text">
        <!-- we place a warning for missing labels if this element is ever
             cross-referenced with an empty xref -->
        <xsl:with-param name="warning" select="boolean(key('xref-by-rid',@id)[not(normalize-space(string(.)))])"/>
      </xsl:apply-templates>
    </xsl:param>
    <xsl:if test="normalize-space(string($contents))">
      <!-- not to create an h5 for nothing -->
      <h5 class="label">
        <xsl:copy-of select="$contents"/>
      </h5>
    </xsl:if>
  </xsl:template>
  <xsl:template mode="label" match="ref">
    <xsl:param name="contents">
      <xsl:apply-templates select="." mode="label-text"/>
    </xsl:param>
    <xsl:if test="normalize-space(string($contents))">
      <!-- we're already in a p -->
      <span class="label">
        <xsl:copy-of select="$contents"/>
      </span>
    </xsl:if>
  </xsl:template>
  <xsl:template match="app" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-app"/>
      <xsl:with-param name="warning" select="$warning"/>
      <!--
      Pass in the desired label if a label is to be autogenerated  
      <xsl:with-param name="auto-text">
        <xsl:text>Appendix </xsl:text>
        <xsl:number format="A"/>
      </xsl:with-param>-->
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="boxed-text" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-boxed-text"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Box </xsl:text>
        <xsl:number level="any"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="disp-formula" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-disp-formula"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Formula </xsl:text>
        <xsl:number level="any"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="chem-struct-wrap | chem-struct-wrapper" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-chem-struct-wrap"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Formula (chemical) </xsl:text>
        <xsl:number level="any"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="fig" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-fig"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Figure </xsl:text>
        <xsl:number level="any"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="front//fn" mode="label-text">
    <xsl:param name="warning" select="boolean(key('xref-by-rid',@id))"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels);
         by default, we get a warning only if we need a label for
         a cross-reference -->
    <!-- auto-number-fn is true only if (1) this fn is cross-referenced, and
         (2) there exists inside the front matter any fn elements with
         cross-references, but not labels or @symbols. -->
    <xsl:param name="auto-number-fn" select="boolean(key('xref-by-rid',parent::fn/@id)) and       boolean(ancestor::front//fn[key('xref-by-rid',@id)][not(label|@symbol)])"/>
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-number-fn"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:number level="any" count="fn" from="front" format="a"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="table-wrap//fn" mode="label-text">
    <xsl:param name="warning" select="boolean(key('xref-by-rid',@id))"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels);
         by default, we get a warning only if we need a label for
         a cross-reference -->
    <xsl:param name="auto-number-fn" select="not(ancestor::table-wrap//fn/label | ancestor::table-wrap//fn/@symbol)"/>
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-number-fn"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>[</xsl:text>
        <xsl:number level="any" count="fn" from="table-wrap" format="i"/>
        <xsl:text>]</xsl:text>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="fn" mode="label-text">
    <xsl:param name="warning" select="boolean(key('xref-by-rid',@id))"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels);
         by default, we get a warning only if we need a label for
         a cross-reference -->
    <!-- autonumber all fn elements outside fn-group,
         front matter and table-wrap only if none of them 
         have labels or @symbols (to keep numbering
         orderly) -->
    <xsl:variable name="in-scope-notes" select="ancestor::article//fn[not(parent::fn-group       | ancestor::front       | ancestor::table-wrap)]"/>
    <xsl:variable name="auto-number-fn" select="not($in-scope-notes/label |       $in-scope-notes/@symbol)"/>
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-number-fn"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <sup class="circulo verde">
        	<xsl:number level="any" count="fn[not(ancestor::front)]" from="article | sub-article | response"/>
        </sup>
        <xsl:apply-templates select="@fn-type"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='abbr']" priority="2">
    <span class="generated"> Abbreviation</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='com']" priority="2">
    <span class="generated"> Communicated by</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='con']" priority="2">
    <span class="generated"> Contributed by</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='conflict']" priority="2">
    <span class="generated"> Conflicts of interest</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='corresp']" priority="2">
    <span class="generated"> Corresponding author</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='current-aff']" priority="2">
    <span class="generated"> Current affiliation</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='deceased']" priority="2">
    <span class="generated"> Deceased</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='edited-by']" priority="2">
    <span class="generated"> Edited by</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='equal']" priority="2">
    <span class="generated"> Equal contributor</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='financial-disclosure']" priority="2">
    <span class="generated"> Financial disclosure</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='on-leave']" priority="2">
    <span class="generated"> On leave</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='other']" priority="2"/>
  <xsl:template match="fn/@fn-type[.='participating-researchers']" priority="2">
    <span class="generated"> Participating researcher</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='present-address']" priority="2">
    <span class="generated"> Current address</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='presented-at']" priority="2">
    <span class="generated"> Presented at</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='presented-by']" priority="2">
    <span class="generated"> Presented by</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='previously-at']" priority="2">
    <span class="generated"> Previously at</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='study-group-members']" priority="2">
    <span class="generated"> Study group member</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='supplementary-material']" priority="2">
    <span class="generated"> Supplementary material</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type[.='supported-by']" priority="2">
    <span class="generated"> Supported by</span>
  </xsl:template>
  <xsl:template match="fn/@fn-type"/>
  <xsl:template match="ref" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-ref"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:number level="any"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="statement" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-statement"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Statement </xsl:text>
        <xsl:number level="any"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="supplementary-material" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-supplementary"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Supplementary Material </xsl:text>
        <xsl:number level="any" format="A" count="supplementary-material[not(ancestor::front)]"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="table-wrap" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="auto" select="$auto-label-table-wrap"/>
      <xsl:with-param name="warning" select="$warning"/>
      <xsl:with-param name="auto-text">
        <xsl:text>Table </xsl:text>
        <xsl:number level="any" format="I"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="*" mode="label-text">
    <xsl:param name="warning" select="true()"/>
    <!-- pass $warning in as false() if a warning string is not wanted
         (for example, if generating autonumbered labels) -->
    <xsl:call-template name="make-label-text">
      <xsl:with-param name="warning" select="$warning"/>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="label" mode="label-text">
    <xsl:apply-templates mode="inline-label-text"/>
  </xsl:template>
  <xsl:template match="text()" mode="inline-label-text">
    <!-- when displaying labels, space characters become non-breaking spaces -->
    <xsl:value-of select="translate(normalize-space(string(.)),' &#10;&#9;','   ')"/>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Writing a name                                               -->
  <!-- ============================================================= -->
  <!-- Called when displaying structured names in metadata         -->
  <xsl:template match="name">
    <xsl:apply-templates select="prefix" mode="inline-name"/>
    <xsl:apply-templates select="surname[../@name-style='eastern']" mode="inline-name"/>
    <xsl:apply-templates select="given-names" mode="inline-name"/>
    <xsl:apply-templates select="surname[not(../@name-style='eastern')]" mode="inline-name"/>
    <xsl:apply-templates select="suffix" mode="inline-name"/>
  </xsl:template>
  <xsl:template match="prefix" mode="inline-name">
    <xsl:apply-templates/>
    <xsl:if test="../surname | ../given-names | ../suffix">
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="given-names" mode="inline-name">
    <xsl:apply-templates/>
    <xsl:if test="../surname[not(../@name-style='eastern')] | ../suffix">
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="contrib/name/surname" mode="inline-name">
    <xsl:apply-templates/>
    <xsl:if test="../given-names[../@name-style='eastern'] | ../suffix">
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="surname" mode="inline-name">
    <xsl:apply-templates/>
    <xsl:if test="../given-names[../@name-style='eastern'] | ../suffix">
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="suffix" mode="inline-name">
    <xsl:apply-templates/>
  </xsl:template>
  <!-- string-name elements are written as is -->
  <xsl:template match="string-name">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="string-name/*">
    <xsl:apply-templates/>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  UTILITY TEMPLATES                                           -->
  <!-- ============================================================= -->
  <xsl:template name="append-pub-type">
    <!-- adds a value mapped for @pub-type, enclosed in parenthesis,
         to a string -->
    <xsl:for-each select="@pub-type">
      <xsl:text> (</xsl:text>
      <span class="data">
        <xsl:choose>
          <xsl:when test=".='epub'">electronico</xsl:when>
          <xsl:when test=".='ppub'">impeso</xsl:when>
          <xsl:when test=".='epub-ppub'">impeso y electronico</xsl:when>
          <xsl:when test=".='epreprint'">electronic preprint</xsl:when>
          <xsl:when test=".='ppreprint'">print preprint</xsl:when>
          <xsl:when test=".='ecorrected'">corrected, electronic</xsl:when>
          <xsl:when test=".='pcorrected'">corrected, print</xsl:when>
          <xsl:when test=".='eretracted'">retracted, electronic</xsl:when>
          <xsl:when test=".='pretracted'">retracted, print</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </span>
      <xsl:text>)</xsl:text>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="metadata-labeled-entry">
    <xsl:param name="label"/>
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <xsl:call-template name="metadata-entry">
      <xsl:with-param name="contents">
        <xsl:if test="normalize-space(string($label))">
		  <xsl:choose>
        		<xsl:when test="$label='Información Adicional'">  
					<h4 class="generated">
						<xsl:copy-of select="$label"/>
						<xsl:text>: </xsl:text>
					</h4>
				</xsl:when>	
			  	<xsl:otherwise>
					<span class="generated">
						<xsl:copy-of select="$label"/>
						<xsl:text>: </xsl:text>
					</span>
				</xsl:otherwise>
		  </xsl:choose>			
        </xsl:if>
        <xsl:copy-of select="$contents"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="metadata-entry">
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <p class="metadata-entry">
      <xsl:copy-of select="$contents"/>
    </p>
  </xsl:template>
  <xsl:template name="metadata-area">
    <xsl:param name="label"/>
    <xsl:param name="contents">
      <xsl:apply-templates/>
    </xsl:param>
    <div class="metadata-area">
      <xsl:if test="normalize-space(string($label))">
        <xsl:call-template name="metadata-labeled-entry">
          <xsl:with-param name="label">
            <xsl:copy-of select="$label"/>
          </xsl:with-param>
          <xsl:with-param name="contents"/>
        </xsl:call-template>
      </xsl:if>
      <div class="metadata-chunk">
        <xsl:copy-of select="$contents"/>
      </div>
    </div>
  </xsl:template>
  <xsl:template name="make-label-text">
    <xsl:param name="auto" select="false()"/>
    <xsl:param name="warning" select="false()"/>
    <xsl:param name="auto-text"/>
    <xsl:choose>
      <xsl:when test="$auto">
        <span class="generated">
          <xsl:copy-of select="$auto-text"/>
        </span>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates mode="label-text" select="label | @symbol"/>
        <xsl:if test="$warning and not(label|@symbol)">
          <span class="warning">
            <xsl:text>{ label</xsl:text>
            <xsl:if test="self::fn"> (or @symbol)</xsl:if>
            <xsl:text> needed for </xsl:text>
            <xsl:value-of select="local-name()"/>
            <xsl:for-each select="@id">
              <xsl:text>[@id='</xsl:text>
              <xsl:value-of select="."/>
              <xsl:text>']</xsl:text>
            </xsl:for-each>
            <xsl:text> }</xsl:text>
          </span>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="assign-id">
    <xsl:variable name="id">
      <xsl:apply-templates select="." mode="id"/>
    </xsl:variable>
    <xsl:attribute name="id">
      <xsl:value-of select="$id"/>
    </xsl:attribute>
  </xsl:template>
  <xsl:template name="assign-src">
    <xsl:for-each select="@xlink:href">
      <xsl:attribute name="src">
        <xsl:value-of select="."/>
      </xsl:attribute>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="assign-href">
    <xsl:for-each select="@xlink:href">
      <xsl:attribute name="href">
        <xsl:value-of select="."/>
      </xsl:attribute>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="named-anchor">
    <!-- generates an HTML named anchor -->
    <xsl:variable name="id">
      <xsl:choose>
        <xsl:when test="@id">
          <!-- if we have an @id, we use it -->
          <xsl:value-of select="@id"/>
        </xsl:when>
        <xsl:when test="not(preceding-sibling::*) and           (parent::alternatives | parent::name-alternatives |            parent::citation-alternatives | parent::collab-alternatives |            parent::aff-alternatives)/@id">
          <!-- if not, and we are first among our siblings inside one of
               several 'alternatives' wrappers, we use its @id if available -->
          <xsl:value-of select="(parent::alternatives | parent::name-alternatives |             parent::citation-alternatives | parent::collab-alternatives |             parent::aff-alternatives)/@id"/>
        </xsl:when>
        <xsl:otherwise>
          <!-- otherwise we simply generate an ID -->
          <xsl:value-of select="generate-id(.)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <a id="{$id}" class="link">
      <xsl:comment> named anchor </xsl:comment>
    </a>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Process warnings                                             -->
  <!-- ============================================================= -->
  <!-- Generates a list of warnings to be reported due to processing
     anomalies. -->
  <xsl:template name="process-warnings">
    <!-- Only generate the warnings if set to $verbose -->
    <xsl:if test="$verbose">
      <!-- returns an RTF containing all the warnings -->
      <xsl:variable name="xref-warnings">
        <xsl:for-each select="//xref[not(normalize-space(string(.)))]">
          <xsl:variable name="target-label">
            <xsl:apply-templates select="key('element-by-id',@rid)" mode="label-text">
              <xsl:with-param name="warning" select="false()"/>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:if test="not(normalize-space(string($target-label))) and           generate-id(.) = generate-id(key('xref-by-rid',@rid)[1])">
            <!-- if we failed to get a label with no warning, and
               this is the first cross-reference to this target
               we ask again to get the warning -->
            <li>
              <xsl:apply-templates select="key('element-by-id',@rid)" mode="label-text">
                <xsl:with-param name="warning" select="true()"/>
              </xsl:apply-templates>
            </li>
          </xsl:if>
        </xsl:for-each>
      </xsl:variable>
      <xsl:if test="normalize-space(string($xref-warnings))">
        <h4>Elements are cross-referenced without labels.</h4>
        <p>Either the element should be provided a label, or their cross-reference(s) should
        have literal text content.</p>
        <ul>
          <xsl:copy-of select="$xref-warnings"/>
        </ul>
      </xsl:if>
      <xsl:variable name="alternatives-warnings">
        <!-- for reporting any element with a @specific-use different
           from a sibling -->
        <xsl:for-each select="//*[@specific-use != ../*/@specific-use]/..">
          <li>
            <xsl:text>In </xsl:text>
            <xsl:apply-templates select="." mode="xpath"/>
            <ul>
              <xsl:for-each select="*[@specific-use != ../*/@specific-use]">
                <li>
                  <xsl:apply-templates select="." mode="pattern"/>
                </li>
              </xsl:for-each>
            </ul>
            <!--<xsl:text>: </xsl:text>
          <xsl:call-template name="list-elements">
            <xsl:with-param name="elements" select="*[@specific-use]"/>
          </xsl:call-template>-->
          </li>
        </xsl:for-each>
      </xsl:variable>
      <xsl:if test="normalize-space(string($alternatives-warnings))">
        <h4>Elements with different 'specific-use' assignments appearing together</h4>
        <ul>
          <xsl:copy-of select="$alternatives-warnings"/>
        </ul>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  id mode                                                      -->
  <!-- ============================================================= -->
  <!-- An id can be derived for any element. If an @id is given,
     it is presumed unique and copied. If not, one is generated.   -->
  <xsl:template match="*" mode="id">
    <xsl:value-of select="@id"/>
    <xsl:if test="not(@id)">
      <xsl:value-of select="generate-id(.)"/>
    </xsl:if>
  </xsl:template>
  <xsl:template match="article | sub-article | response" mode="id">
    <xsl:value-of select="@id"/>
    <xsl:if test="not(@id)">
      <xsl:value-of select="local-name()"/>
      <xsl:number from="article" level="multiple" count="article | sub-article | response" format="1-1"/>
    </xsl:if>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  "format-date"                                                -->
  <!-- ============================================================= -->
  <!-- Maps a structured date element to a string -->
  <xsl:template name="format-date">
    <!-- formats date in DD Month YYYY format -->
    <!-- context must be 'date', with content model:
         (((day?, month?) | season)?, year) -->
    <xsl:for-each select="day|month|season">
      <xsl:apply-templates select="." mode="map"/>
      <xsl:text> </xsl:text>
    </xsl:for-each>
    <xsl:apply-templates select="year" mode="map"/>
  </xsl:template>
  <xsl:template match="day | season | year" mode="map">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="month" mode="map">
    <!-- maps numeric values to English months -->
    <xsl:choose>
      <xsl:when test="number() = 1">January</xsl:when>
      <xsl:when test="number() = 2">February</xsl:when>
      <xsl:when test="number() = 3">March</xsl:when>
      <xsl:when test="number() = 4">April</xsl:when>
      <xsl:when test="number() = 5">May</xsl:when>
      <xsl:when test="number() = 6">June</xsl:when>
      <xsl:when test="number() = 7">July</xsl:when>
      <xsl:when test="number() = 8">August</xsl:when>
      <xsl:when test="number() = 9">September</xsl:when>
      <xsl:when test="number() = 10">October</xsl:when>
      <xsl:when test="number() = 11">November</xsl:when>
      <xsl:when test="number() = 12">December</xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  "author-string" writes authors' names in sequence            -->
  <!-- ============================================================= -->
  <xsl:template name="author-string">
    <xsl:variable name="all-contribs" select="/article/front/article-meta/contrib-group/contrib/name/surname |               /article/front/article-meta/contrib-group/contrib/collab"/>
    <xsl:for-each select="$all-contribs">
      <xsl:if test="count($all-contribs) &gt; 1">
        <xsl:if test="position() &gt; 1">
          <xsl:if test="count($all-contribs) &gt; 2">,</xsl:if>
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:if test="position() = count($all-contribs)">and </xsl:if>
      </xsl:if>
      <xsl:value-of select="."/>
    </xsl:for-each>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Footer branding                                              -->
  <!-- ============================================================= -->
  <xsl:template name="footer-branding">
    <hr class="part-rule"/>
    <div class="branding">
      <p>
        <xsl:text>Desarrollado por eScire -</xsl:text>
        <xsl:text> Consultoría, Tecnologías y Gestión del Conocimiento SA de CV</xsl:text>
      </p>
    </div>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  Utility templates for generating warnings and reports        -->
  <!-- ============================================================= -->
  <!--  <xsl:template name="report-warning">
    <xsl:param name="when" select="false()"/>
    <xsl:param name="msg"/>
    <xsl:if test="$verbose and $when">
      <xsl:message>
        <xsl:copy-of select="$msg"/>
      </xsl:message>
    </xsl:if>
  </xsl:template>-->
  <!--<xsl:template name="list-elements">
    <xsl:param name="elements" select="/.."/>
    <xsl:if test="$elements">
      <ol>
        <xsl:for-each select="*">
          <li>
            <xsl:apply-templates select="." mode="element-pattern"/>
          </li>
        </xsl:for-each>
      </ol>
    </xsl:if>
  </xsl:template>-->
  <xsl:template match="*" mode="pattern">
    <xsl:value-of select="name(.)"/>
    <xsl:for-each select="@*">
      <xsl:text>[@</xsl:text>
      <xsl:value-of select="name()"/>
      <xsl:text>='</xsl:text>
      <xsl:value-of select="."/>
      <xsl:text>']</xsl:text>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="node()" mode="xpath">
    <xsl:apply-templates mode="xpath" select=".."/>
    <xsl:apply-templates mode="xpath-step" select="."/>
  </xsl:template>
  <xsl:template match="/" mode="xpath"/>
  <xsl:template match="*" mode="xpath-step">
    <xsl:variable name="name" select="name(.)"/>
    <xsl:text>/</xsl:text>
    <xsl:value-of select="$name"/>
    <xsl:if test="count(../*[name(.) = $name]) &gt; 1">
      <xsl:text>[</xsl:text>
      <xsl:value-of select="count(.|preceding-sibling::*[name(.) = $name])"/>
      <xsl:text>]</xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="@*" mode="xpath-step">
    <xsl:text>/@</xsl:text>
    <xsl:value-of select="name(.)"/>
  </xsl:template>
  <xsl:template match="comment()" mode="xpath-step">
    <xsl:text>/comment()</xsl:text>
    <xsl:if test="count(../comment()) &gt; 1">
      <xsl:text>[</xsl:text>
      <xsl:value-of select="count(.|preceding-sibling::comment())"/>
      <xsl:text>]</xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="processing-instruction()" mode="xpath-step">
    <xsl:text>/processing-instruction()</xsl:text>
    <xsl:if test="count(../processing-instruction()) &gt; 1">
      <xsl:text>[</xsl:text>
      <xsl:value-of select="count(.|preceding-sibling::processing-instruction())"/>
      <xsl:text>]</xsl:text>
    </xsl:if>
  </xsl:template>
  <xsl:template match="text()" mode="xpath-step">
    <xsl:text>/text()</xsl:text>
    <xsl:if test="count(../text()) &gt; 1">
      <xsl:text>[</xsl:text>
      <xsl:value-of select="count(.|preceding-sibling::text())"/>
      <xsl:text>]</xsl:text>
    </xsl:if>
  </xsl:template>
  <!-- ============================================================= -->
  <!--  End stylesheet                                               -->
  <!-- ============================================================= -->
</xsl:stylesheet>

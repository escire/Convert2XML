<?php

import('lib.pkp.classes.plugins.GenericPlugin');
import('classes.submission.common.Action');
import('lib.pkp.classes.file.FileManager');
import('classes.user.User');
import('classes.file.ArticleFileManager');
import('lib.pkp.classes.file.FileManager');

class convert2xmlPlugin extends GenericPlugin {
	/**
	 * Called as a plugin is registered to the registry
	 * @param $category String Name of category plugin was registered to
	 * @return boolean True iff plugin initialized successfully; if false,
	 * 	the plugin will not be registered.
	 */
	function register($category, $path) {
		$success = parent::register($category, $path);
		if ($success && $this->getEnabled()) {
			HookRegistry::register('TemplateManager::display',array(&$this, 'callback'));			
		}
		return $success;
	}

	function getName() {
        return 'convert2xmlPlugin';
    }
	
	function getDisplayName() {
		return __('plugins.generic.convert2xmlPlugin.name');
	}

	function getDescription() {
		return __('plugins.generic.convert2xmlPlugin.description');
	}
	
	function metadataField($hookName, $params) {
		$smarty =& $params[1];
		$output =& $params[2];
		$output .= $smarty->fetch($this->getTemplatePath() . 'index.tpl');
		return false;
	}
	
	/**
	 * Get the filename of the ADODB schema for this plugin.
	 */
	function getInstallSchemaFile() {
		return $this->getPluginPath() . '/' . 'schema.xml';
	}

	
	function displayHeaderLink($hookName, $params) {
		$journal =& Request::getJournal();
		if (!$journal) return false;

		if ($this->getEnabled()) {
			$smarty =& $params[1];
			$output =& $params[2];
			$templateMgr = TemplateManager::getManager();
			$output .= '<li><a href="' . $templateMgr->smartyUrl(array('page'=>'convert2xmlPlugin'), $smarty) . '" target="_parent">' . $templateMgr->smartyTranslate(array('key'=>'plugins.generic.convert2xmlPlugin.name'), $smarty) . '</a></li>';
		}
		return false;
	}
	

	function handleRequest($hookName, $args) {
		$page =& $args[0];
		$op =& $args[1];
		$sourceFile =& $args[2];

		// If the request is for the log analyzer itself, handle it.
		if ($page === 'convert2xmlPlugin') {
			$this->import('StatisticsHandler');
			Registry::set('plugin', $this);
			define('HANDLER_CLASS', 'StatisticsHandler');
			return true;
		}

		return false;
	}

	function isSitePlugin() {
		return true;
	}

	function getManagementVerbs() {
		$verbs = array();
		if ($this->getEnabled()) {
			$verbs[] = array('settings', AppLocale::translate('plugins.generic.convert2xml.settings'));
		}
		return parent::getManagementVerbs($verbs);
	}

 	/*
 	 * Execute a management verb on this plugin
 	 * @param $verb string
 	 * @param $args array
	 * @param $message string Location for the plugin to put a result msg
 	 * @return boolean
 	 */
	function manage($verb, $args, &$message) {
		if (!parent::manage($verb, $args, $message)) return false;
		switch ($verb) {
			case 'settings':
				$templateMgr =& TemplateManager::getManager();
				$templateMgr->register_function('plugin_url', array(&$this, 'smartyPluginUrl'));
				$journal =& Request::getJournal();
				$this->import('convert2xmlPluginSettingsForm');
				$form = new convert2xmlPluginSettingsForm($this, $journal->getId());
				if (Request::getUserVar('save')) {
					$form->readInputData();
					if ($form->validate()) {
						$form->execute();
						return false;
					} else {
						$form->display();
					}
				} else {
					$form->initData();
					$form->display();
				}
				return true;
			case 'convert2xmlPlugin':
				Request::redirect(null, 'convert2xmlPlugin');
				return false;
			default:
				// Unknown management verb
				assert(false);
				return false;
		}
	}
	
	function callback($hookName, $args) {
		$request =& Registry::get('request');
		$smarty =& $params[1];
		$output =& $params[2];
		
		if (!is_a($request->getRouter(), 'PKPPageRouter')) return null;

		$templateManager =& $args[0];

		$page = Request::getRequestedPage();
		$baseUrl = $templateManager->get_template_vars('baseUrl');
		$additionalHeadData = $templateManager->get_template_vars('additionalHeadData');
		

		$showScript = "
		<script language=\"javascript\" type=\"text/javascript\" src=\"".$baseUrl . DIRECTORY_SEPARATOR . $this->getPluginPath() . DIRECTORY_SEPARATOR . "js" . DIRECTORY_SEPARATOR ."convert2xml.js\"></script>
		<script language=\"javascript\" type=\"text/javascript\">
			
			function splashOpen(url)
			{
				var winFeatures = \"screenX=0,screenY=0,top=0,left=0,scrollbars,width=100,height=100\";
				var winName = \"Convert2xml\";
				var win = window.open(url,winName, winFeatures); 
				var extraWidth = win.screen.availWidth - win.outerWidth;
				var extraHeight = win.screen.availHeight - win.outerHeight;
				win.resizeBy(extraWidth, extraHeight);
				return win;
			}
			
			$( document ).ready(function() {
					console.log(\"estoy vivo\");
					var url = document.location.href;
					if(url.indexOf(\"editor/submissionEditing\") != -1){
						$(\"#scheduling\").prepend(".$this->construyeLink().");
					}
			});
			
		</script>";

		$templateManager->assign('additionalHeadData', $additionalHeadData."\n".$showScript);
		
		
		//$output .= $smarty->fetch($this->getTemplatePath() . 'index.tpl');
		return false;
	}
	
	function construyeLink() {
		
		$estilos = "margin-top:5px;";
		$mostrarRequerimientos = false;
		$cadena  = "<h3>".AppLocale::translate('plugins.generic.convert2xmlPlugin.name')."</h3>";
		$currentJournal2 	=& 		Request::getJournal();
		$session 			=& 		 Request::getSession();
		$templateMgr2 		=		 &TemplateManager::getManager();
		
		if (!$currentJournal2){
			$cadena .= "<p>No fue posible obtener datos de la revista</p>";
			return '"'.$cadena.'"';
		} 
		
		$wsURL 				= 		$this->getSetting($currentJournal2->getId(), 'wsURL');
		$revistaId 			=		$currentJournal2->getJournalId(); 
		$revistaTitulo 		=		$currentJournal2->getJournalTitle();
		$onlineIssn 		=		$currentJournal2->getSetting('onlineIssn');
		$printIssn 			= 		$currentJournal2->getSetting('printIssn');
		$abbreviation 		=		$currentJournal2->getSetting('abbreviation');
		$abbreviation2 = '';
		
		foreach($abbreviation as $uno=>$dos)
		{
			$abbreviation2 = $dos;
		}
		$publisher 			=		$currentJournal2->getSetting('publisherInstitution');
		
		$articleDao 		= 		DAORegistry::getDAO('ArticleDAO');
		
		$articleId = $_SERVER['REQUEST_URI'];
		while ( strcmp(substr($articleId, -1), "/") == 0 ){
			$articleId = substr($articleId, 0, -1); 
		}
		$articleId = substr($articleId,strrpos($articleId, "/",-1)+1 );
		$article 			=& 		$articleDao->getArticle($articleId);
		$issueDao 			=& 		DAORegistry::getDAO('IssueDAO');
		$issue 				=&		$issueDao->getIssueByArticleId($articleId, $currentJournal2->getJournalId()); 
		
		$urls= $wsURL; 
		
		if( !isset($revistaTitulo) || empty($revistaTitulo) )	{	$estilos="pointer-events: none;opacity: 0.5;"; $mostrarRequerimientos=true;}
		if( !isset($printIssn)     || empty($printIssn) )		{	$estilos="pointer-events: none;opacity: 0.5;"; $mostrarRequerimientos=true;}
		if( !isset($abbreviation2) || empty($abbreviation2) )	{	$estilos="pointer-events: none;opacity: 0.5;"; $mostrarRequerimientos=true;}
		if( !isset($publisher) 	   || empty($publisher) )		{	$estilos="pointer-events: none;opacity: 0.5;"; $mostrarRequerimientos=true;}
		if( $issue === NULL )									{	$estilos="pointer-events: none;opacity: 0.5;"; $mostrarRequerimientos=true;}
		
	    if( $issue === NULL ){
		      $urls.=  "&date=0&volume=0&year=0&issue=0";
	    }else{
			if($article->getDateSubmitted() === NULL) {
				$urls.=  "&date=0" ;		
				$estilos="pointer-events: none;opacity: 0.5;";    
				$mostrarRequerimientos=true;
			}else{      
				$urls.=  "&date=".urlencode($article->getDateSubmitted()) ;    
			}
		  
			if($issue->getVolume() === NULL) {
				$urls.=  "&volume=0" ;	
				$estilos="pointer-events: none;opacity: 0.5;"; 
				$mostrarRequerimientos=true;				
			}else{      
				$urls.=  "&volume=".urlencode($issue->getVolume());    
			}  
		  
			if($issue->getYear() === NULL) {
				$urls.=  "&year=0" ;    	
				$estilos="pointer-events: none;opacity: 0.5;";
				$mostrarRequerimientos=true;
			}else{     	
				$urls.=  "&year=".urlencode($issue->getYear());    
			}    
		
			if($issue->getNumber() === NULL) {
				$urls.=  "&issue=0" ;    
				$estilos="pointer-events: none;opacity: 0.5;";
				$mostrarRequerimientos=true;
			}else{     
				$urls.=  "&issue=".urlencode($issue->getNumber()) ;    
			}    
	    }
		
		$articleDao2 =& DAORegistry::getDAO('PublishedArticleDAO'); /* @var $articleDao PublishedArticleDAO */
		$article2 =& $articleDao2->getPublishedArticleByArticleId($articleId, null, true);
		if ($article){
				if( !is_object($article2) ){
					$urls.=  "&page=1-1";
					$estilos="pointer-events: none;opacity: 0.5;";
					$mostrarRequerimientos=true;
				}else{
					$urls.=  "&page=".$article2->getPages();
				}
		}
		
		$urls.=  "&publisher=".urlencode($publisher)."&printIssn=".urlencode($printIssn)."&onlineIssn=".urlencode($onlineIssn);
		$urls.=  "&abbreviation=".urlencode($abbreviation2)."&revistaId=".urlencode($revistaId)."&revistaTitulo=".urlencode($revistaTitulo);


		if ($this->getEnabled()) {
			$templateMgr = TemplateManager::getManager();			
		}
		
		$cadena .= "<a href=\"#\" target=\"popup\"  onClick=\"window.open(\'".$urls."\',\'Convert2xml\',\'titlebar=Convert2xml,top=100,left=250,width=450,height=400\'); return false;\"  >".AppLocale::translate('plugins.generic.convert2xmlPlugin.href')."</a>";
		
		$requerimientos="";
		if( $mostrarRequerimientos ==true)
			$requerimientos = "<h4><a href=\"".$currentJournal2->getUrl ()."/manager/plugin/generic/convert2xmlPlugin/settings\">".AppLocale::translate('plugins.generic.convert2xml.requerimientosVer')."</a></h4>";
		
		$cadena .= '<br/><br/>';
		return "'<div id=\"converter\" style=\"".$estilos."\">".$cadena."</div>".$requerimientos."<br/><div class=\"separator\"></div>'";
	}
	
}

?>

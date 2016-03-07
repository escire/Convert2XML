<?php

import('lib.pkp.classes.plugins.GenericPlugin');
import('classes.submission.common.Action');
import('lib.pkp.classes.file.FileManager');
import('classes.user.User');


class ConverterPLugin extends GenericPlugin{

	function register($category, $path){
		if(parent::register($category, $path)){
            if ($this->getEnabled()) {
                HookRegistry::register('Converter', array(&$this, 'callbackConvert'));
                HookRegistry::register('PluginRegistry::loadCategory', array(&$this, 'callbackGateway'));
	            $this->addLocaleData();
            }
			return true;
		}
		return false;
	}


	function getName(){
		return 'ConverterPlugin';
	}


	function getDisplayName(){	
		return __('plugins.generic.converter.displayName');
	}

	function getDescription(){
		return __('plugins.generic.converter.description');
	}

	function setEnabled($enabled) {
		parent::setEnabled($enabled);
		$journal =& Request::getJournal();
		return false;
	}


	function callbackGateway($hookName, $args) {
		$category =& $args[0];
		$plugins =& $args[1];
		$this->import('ConverterGatewayPlugin');
		$gatewayPlugin = new ConverterGatewayPlugin($this->getName());
		$plugins[$gatewayPlugin->getSeq()][$gatewayPlugin->getPluginPath()] =& $gatewayPlugin;

		return false;
	}

	function getManagementVerbs() {
		$verbs = array();
		if ($this->getEnabled()) {
			$verbs[] = array('settings', __('plugins.generic.converter.settings'));
		}
		return parent::getManagementVerbs($verbs);
	}


	function manage($verb, $args, &$message, &$messageParams) {
		if (!parent::manage($verb, $args, $message, $messageParams)) return false;

		switch ($verb) {
			case 'settings':
				$templateMgr =& TemplateManager::getManager();
				$templateMgr->register_function('plugin_url', array(&$this, 'smartyPluginUrl'));
				$journal =& Request::getJournal();

				$this->import('ConverterPluginSettingsForm');
				$form = new ConverterPluginSettingsForm($this, $journal->getId());
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
			default:
				// Unknown management verb
				assert(false);
				return false;
		}
	}

	function unsetSession(){
		$session =& Request::getSession();
		$session->unsetSessionVar('converter_pathFileCon');
	    $session->unsetSessionVar('converter_client');
	    $session->unsetSessionVar('converter_userAgent');
	    $session->unsetSessionVar('converter_loginParams');
	    $session->unsetSessionVar('converter_completePath');
	    $session->unsetSessionVar('converter_locale');
	    $session->unsetSessionVar('converter_submissionId');
	    $session->unsetSessionVar('converter_fileId');
	    $session->unsetSessionVar('converter_revision');
	    $session->unsetSessionVar('converter_fileName');
	    $session->unsetSessionVar("jsonFile");
	    $session->unsetSessionVar('actualError');
	    $session->unsetSessionVar('converter_rutaJson');
	    $session->unsetSessionVar('idFiles');
	}

	function isAssoc($arr)
	{
	    return array_keys($arr) !== range(0, count($arr) - 1);
	}

	
	function debug_to_console( $data ) {

	    if ( is_array( $data ) ){
	    	if( $this->isAssoc( $data ) ){
	    		foreach($data as $specific=>$value) {
					$this->debug_to_console( $specific.'-'.$value );
				}
	    	}
	    	else
	        	$output = "<script>console.log( 'Debug Objects: " . implode( ',', $data) . "' );</script>";
		}
	    else
	        $output = "<script>console.log( 'Debug Objects: " . $data . "' );</script>";

	    echo $output;
	}
	
	function str_ends_with($haystack, $needle)
	{
	    return strrpos($haystack, $needle) + strlen($needle) ===
	        strlen($haystack);
	}


	function callbackConvert($hookName, $args){
		$params = &$args[0];
		$smarty = &$args[1];
		$output = &$args[2];

		$this->unsetSession();

		$session =& 		 Request::getSession();
		$fileManager = 		 new FileManager();
	    $templateMgr =		 &TemplateManager::getManager();
        $journal =& 		 Request::getJournal();
        $pageVars = 		 $smarty->_tpl_vars;
        $userName = 		 'convertidor';
		$userPassword = 	 'usuario2016_#';

        /////Verificar su existe usuario convertidor y si no ... lo crea
        $roleDao =& DAORegistry::getDAO('RoleDAO');			 
		$userDao =& DAORegistry::getDAO('UserDAO');

		$newUser = new User();

		$newUser->setFirstName('Convertidor');
		$newUser->setLastName('eScire');
		$newUser->setUsername($userName);
		$newUser->setEmail('convertidor@escire.com.mx');
		$newUser->setPassword(Validation::encryptCredentials($userName,$userPassword ));
		$locales = array();
		array_push($locales, 'es_ES' );
		$newUser->setLocales($locales);

		$role = new Role();
		$role->setRoleId(ROLE_ID_EDITOR);
		$role->setJournalId($journal->getId());
		

		array_push($newUser->roles, $role);

		$userExists = $userDao->getByUsername('convertidor', true);

		if ($userExists == null) {
			if ($userDao->insertUser($newUser)) {

				$role->setUserId($newUser->getId());
				$roleDao->insertRole($role);
				$this->debug_to_console( 'Se inserto usuario: '. $newUser->getId() );	
			}else{
				$this->debug_to_console( 'No se inserto usuario' );	
			}
		}else{
			$this->debug_to_console( 'Usuario ya existe' );	
		}
			

				 $ch=curl_init();
                curl_setopt($ch, CURLOPT_URL, "http://converter.escire.mx/");
                curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
                //curl_setopt($ch, CURLOPT_CONNECTTIMEOUT ,0);
                curl_setopt($ch, CURLOPT_TIMEOUT, 400); //timeout in seconds
                set_time_limit(0);

                $tmp=curl_exec($ch);
                curl_close($ch);

                
                $this->debug_to_console($tmp);





        //VALIDAR SI SON CORRECTOS LOS DATOS DE USUARIO/PASSWORD
        $baseUrl =& 		 $templateMgr->get_template_vars('baseUrl'); //domail.com/path
		$currentJournal =&   $templateMgr->get_template_vars('currentJournal');
		$journalPath = 		 $currentJournal->getUrl();//domain.com/index.php/journal
		
		$wsURL = $this->getSetting($currentJournal->getId(), 'wsURL');
		$userPass = $this->getSetting($currentJournal->getId(), 'userPass');
		$userClient = $this->getSetting($currentJournal->getId(), 'userClient');

				
		$userDao =& DAORegistry::getDAO('UserDAO');
		$user =& $userDao->getByUsername($userName);

		if ($user == null) { return false; } // no se muestra convertidor
		if( ! Validation::checkCredentials($userName, $userPassword) ){ return false; }

		$roleDao =& DAORegistry::getDAO('RoleDAO');
		if(! $roleDao->userHasRole($currentJournal->getJournalId(), $user->getId(), ROLE_ID_EDITOR) ) { return false; }

		//Fin de la vallidacion


        
		$archivos = array('vacio' => 'vacio');
		$submissionId = 0;
		
		$initialCopyeditFile = $pageVars["initialCopyeditFile"];
		if($initialCopyeditFile){
			$temp = strtolower($initialCopyeditFile->_data["fileName"]);
			if( $this->str_ends_with($temp, 'doc') || $this->str_ends_with($temp, 'docx') )
				$archivos[$initialCopyeditFile->_data["revision"].','.$initialCopyeditFile->_data["fileId"]] = __('plugins.generic.converter.fileversion.initialCopyeditFile').' | '. $initialCopyeditFile->_data["fileName"];

			if( !empty($initialCopyeditFile->_data["submissionId"]) )
				$submissionId = 	 $initialCopyeditFile->_data["submissionId"];
		}

		$submissionFile = $pageVars["submissionFile"];
		if($submissionFile){
			$temp = strtolower($submissionFile->_data["fileName"]);
			if( $this->str_ends_with($temp, 'doc') || $this->str_ends_with($temp, 'docx') )
			$archivos[$submissionFile->_data["revision"].','.$submissionFile->_data["fileId"]] = __('plugins.generic.converter.fileversion.submissionFile').' | '.$submissionFile->_data["fileName"];

			if( !empty($submissionFile->_data["submissionId"]) )
				$submissionId = 	 $submissionFile->_data["submissionId"];
		}

		$copyeditFile = $pageVars["copyeditFile"];
		if($copyeditFile){
			$temp = strtolower($copyeditFile->_data["fileName"]);
			if( $this->str_ends_with($temp, 'doc') || $this->str_ends_with($temp, 'docx') )
			$archivos[$copyeditFile->_data["revision"].','.$copyeditFile->_data["fileId"]] = __('plugins.generic.converter.fileversion.copyeditFile').' | '.$copyeditFile->_data["fileName"];

			if( !empty($copyeditFile->_data["submissionId"]) )
				$submissionId = 	 $copyeditFile->_data["submissionId"];
		}

		$editorAuthorCopyeditFile = $pageVars["editorAuthorCopyeditFile"];
		if($editorAuthorCopyeditFile){
			$temp = strtolower($editorAuthorCopyeditFile->_data["fileName"]);
			if( $this->str_ends_with($temp, 'doc') || $this->str_ends_with($temp, 'docx') )
			$archivos[$editorAuthorCopyeditFile->_data["revision"].','.$editorAuthorCopyeditFile->_data["fileId"]] = __('plugins.generic.converter.fileversion.editorAuthorCopyeditFile').' | '.$editorAuthorCopyeditFile->_data["fileName"];

			if( !empty($editorAuthorCopyeditFile->_data["submissionId"]) )
				$submissionId = 	 $editorAuthorCopyeditFile->_data["submissionId"];
		}

		
        $finalCopyeditFile = $pageVars["finalCopyeditFile"];
        if($finalCopyeditFile){
        	$temp = strtolower($finalCopyeditFile->_data["fileName"]);
			if( $this->str_ends_with($temp, 'doc') || $this->str_ends_with($temp, 'docx') )
			$archivos[$finalCopyeditFile->_data["revision"].','.$finalCopyeditFile->_data["fileId"]] = __('plugins.generic.converter.fileversion.finalCopyeditFile').' | '.$finalCopyeditFile->_data["fileName"];

			if( !empty($finalCopyeditFile->_data["submissionId"]) )
				$submissionId = 	 $finalCopyeditFile->_data["submissionId"];
        }

		unset($archivos['vacio']);

		
		if (!empty($archivos)) {
			
			$fileName = 		 $finalCopyeditFile->_data["fileName"];
			$revision = 		 $finalCopyeditFile->_data["revision"];
	        $fileId = 			 $finalCopyeditFile->_data["fileId"];

	 		$completePath 	= realpath('.')."/plugins/generic/converter/";
			$pathFileCon 	= $completePath."archivos/";
			$rutaJson 		= $completePath."archivos/".time().'.txt';
			$pathFileCon 	= $pathFileCon . $fileId . "/"; 


			$uploadUrl = $journalPath .'/editor/uploadLayoutFile';
			$loginUrl = $baseUrl . '/index.php/index/login/signIn';
			$userAgent = "'Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.13) Gecko/20080311 Firefox/2.0.0.13'";

			$requiredData = false;
			if(empty($userName) || empty($userPassword) || empty($userClient) || empty($userPass)  || empty($wsURL)	)
				$requiredData = true;

		    $loginParams = array(
					'username' => $userName,
					'password' => $userPassword
				);
			$session->setSessionVar('converter_pathFileCon', $pathFileCon);
		    $session->setSessionVar('converter_client', $this->getSetting($currentJournal->getId(), 'userClient'));
		    $session->setSessionVar('converter_pass', $this->getSetting($currentJournal->getId(), 'userPass'));
		    $session->setSessionVar('converter_wsURL', $this->getSetting($currentJournal->getId(), 'wsURL'));
		    $session->setSessionVar('converter_userAgent', $userAgent);
		    $session->setSessionVar('converter_loginParams', $loginParams);
		    $session->setSessionVar('converter_completePath', $completePath);
		    $session->setSessionVar('converter_locale', Locale::getLocale());
		    $session->setSessionVar('converter_submissionId', $submissionId);

		    $session->setSessionVar('converter_fileId', $fileId);
		    $session->setSessionVar('converter_revision', $revision);
		    $session->setSessionVar('converter_fileName', $fileName);
		    $session->setSessionVar('converter_rutaJson', $rutaJson);

		    
		    $idFiles = '';
		    //verificar si existe zip a descargar
		    foreach ($archivos as $clave => $valor){
		    	$idFile 	 = explode(",", $clave );
		    	$idFiles 	.= ",".$idFile[1];

				$fileNametmp2 	= explode("|", $valor );
				$fileNametmp 	= trim($fileNametmp2[1]);

				$nomZip = str_replace("-","",$fileNametmp);
		    	$nomZip = str_replace(".docx","",$nomZip);
		    	$nomZip = str_replace(".DOCX","",$nomZip);
		    	$nomZip = str_replace(".doc","",$nomZip);
		    	$nomZip = str_replace(".DOC","",$nomZip);
		    	$nomZip .= '.zip';

		    	$tempVar = explode(",", $clave );
        		$nomCarp 	= $tempVar[1];

        		$rutaBuscarHTML = $completePath."archivos/".$nomCarp."/html/".$nomZip;
        		$rutaBuscarXML = $completePath."archivos/".$nomCarp."/xml/".$nomZip;

        		if( file_exists($rutaBuscarHTML) ){
        			$templateMgr->assign('zipFileHTML', $baseUrl .'/plugins/generic/converter/archivos/'.$nomCarp."/html/".$nomZip);
        			$templateMgr->assign('archivoToolTipHTML', $valor);
        		}

        		if( file_exists($rutaBuscarXML) ){
        			$templateMgr->assign('zipFileXML', $baseUrl .'/plugins/generic/converter/archivos/'.$nomCarp."/xml/".$nomZip);
        			$templateMgr->assign('archivoToolTipXML', $valor);
        		}
        		
        		
        		
		    }
		    $session->setSessionVar('idFiles', $idFiles);

		   
		    $templateMgr->assign('finalCopyeditFile', $finalCopyeditFile);
		    $templateMgr->assign('converter_fileId', $fileId);
		    $templateMgr->assign('principalUrl', $journalPath);
		    $templateMgr->assign('requiredData', $requiredData);
		    $templateMgr->assign('archivos', $archivos);

		    $templateMgr->display($this->getTemplatePath() . 'converter.tpl');
		    
		}
		return false;

	}




}



?>

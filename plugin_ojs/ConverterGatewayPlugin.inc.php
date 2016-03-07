<?php
import('classes.plugins.GatewayPlugin');
import('lib.pkp.classes.file.FileManager');


class ConverterGatewayPlugin extends GatewayPlugin{
	
	var $client = "";
	var $userPass = "";
	var $wsURL = "";
	var $fileName = "";
	var $urlWorkPlugin = "";
	var $completePath = "";
	var $loginParams;
	var $loginUrl = "";
	var $userAgent = "";
	var $uploadUrl = "";
	var $locale = "";
	var $postGalley;
	var $parentPluginName;
	var $fileId;
	var $revision;
	var $jsonStatus;
	var $cont = 0;
	var $pathFileCon;
	var $actualError = "";
	var $jsonFile = "";
	
	function ConverterGatewayPlugin($parentPluginName) {
		parent::GatewayPlugin();
		$this->parentPluginName = $parentPluginName;
	}

	function getHideManagement() {
		return true;
	}

	function getName(){
		return 'ConverterGatewayPlugin';
	}

	function getDisplayName(){
		return __('plugins.generic.converter.gateway.displayName');
	}

	function getDescription(){
		return __('plugins.generic.converter.gateway.description');
	}

	function &getConverterGatewayPlugin() {
		$plugin =& PluginRegistry::getPlugin('generic', $this->parentPluginName);
		return $plugin;
	}

	function getPluginPath() {
		$plugin =& $this->getConverterGatewayPlugin();
		return $plugin->getPluginPath();
	}

	function getEnabled() {
		$plugin =& $this->getConverterGatewayPlugin();
		return $plugin->getEnabled();
	}

	function getManagementVerbs() {
		return array();
	}

	function getMetricTypes(){
		return array();	
	}

	function getFormFieldNames(){
		return array();	
	}

	function getPubIdMetadataFile(){
		return array();	
	}

	function getPubIdType(){
		$str = "";
		return $str;	
	}

	function getPubId(){

	}

	function getExcludeFormFieldName(){
		return array();	
	}

	function ping($url)
	{
	   	$handle = curl_init($url);
		curl_setopt($handle,  CURLOPT_RETURNTRANSFER, TRUE);

		/* Get the HTML or whatever is linked in $url. */
		$response = curl_exec($handle);

		/* Check for 404 (file not found). */
		$httpCode = curl_getinfo($handle, CURLINFO_HTTP_CODE);
		if($httpCode == 404) {
		    return FALSE;
		}
		return TRUE;
	}


	function fetch($args, $request) {
		if(empty($_SESSION)){
			$journal =& Request::getJournal();
			$path = $journal->getData("path");
			Request::redirect($path);
		}

		if(!$this->getEnabled()){
			return false;
		}

		$journal =& $request->getJournal();
		if(!isset($journal))
			$this->showError();

		$journalId = $journal->getId();
		$operator = array_shift($args);

		switch ($operator) {
			case 'processConvert':
				$this->processConvert();
				break;
			
			case 'status':
				$this->getStatus();
			break;

			default:
				$this->showError();
				
		}
		return true;

	}

	function closeOutput(){
		set_time_limit(0);
	    ignore_user_abort(true);
	    header("Connection: close\r\n");
	    header("Content-Encoding: none\r\n");
	    ob_start();
	    echo "iniciando...";
	    $size = ob_get_length();
	    header("Content-Length: $size",TRUE);
	    ob_end_flush();
	    ob_flush();
	    flush();
	}

	
	function processConvert(){
		
		$fileSelected 	= $_POST['fileSelected'];
		
		$fileNametmp	= $_POST['fileName'];
		$fileNametmp2 	= explode("|", $fileNametmp );
		$fileName2 		= trim($fileNametmp2[1]);

		$convertionType = $_POST['convertionType'];
		$fileSelectedA 	= explode(",", $fileSelected );
        $revision1 		= $fileSelectedA[0];
        $fileId1 		= $fileSelectedA[1];
        
        	

        //subcarpeta para almacenar cada tipo de archivo
        $rutaSubcarpeta = '';
        if (strcmp($convertionType, 'xml') == 0) {
        	$rutaSubcarpeta = 'xml/';
        }else if (strcmp($convertionType, 'html') == 0) {
        	$rutaSubcarpeta = 'html/';
        }

		$this->closeOutput();

		import('classes.file.ArticleFileManager');
		import('lib.pkp.classes.file.FileManager');

        $session =& 				Request::getSession();
	    $templateMgr = 				&TemplateManager::getManager();
	    $this->submissionId = 		$session->getSessionVar('converter_submissionId');
		$articleFileManager = new 	ArticleFileManager($this->submissionId);
		$fileManager = new 			FileManager();

		$session->setSessionVar('converter_convertionType', $convertionType);
		$currentJournal 		=& 	$templateMgr->get_template_vars('currentJournal');
		$baseUrl 				=&	$templateMgr->get_template_vars('baseUrl');
        $this->client 			= 	$session->getSessionVar('converter_client');
        $this->userPass 		= 	$session->getSessionVar('converter_pass');
        $this->wsURL 			= 	$session->getSessionVar('converter_wsURL');
        $this->urlWorkPlugin 	= 	$baseUrl . '/plugins/generic/converter/archivos/';
        $this->loginParams 		= 	$session->getSessionVar('converter_loginParams');
        $this->completePath 	= 	$session->getSessionVar('converter_completePath');
        $idFiles				= $session->getSessionVar('idFiles');


        $this->pathFileCon 		=$this->completePath."archivos/".$fileId1."/"; 
        $pathFileConCarpeta 	=$this->pathFileCon;
        $this->pathFileCon 		=$this->pathFileCon.$convertionType."/";
        
        $this->fileId 		= $fileId1;
        $this->revision 	= $revision1;
        $this->fileName 	= $fileName2;

        $this->loginUrl =		$baseUrl . '/index.php/index/login/signIn';
        $this->userAgent =		$session->getSessionVar('converter_userAgent');
        $this->locale =			$session->getSessionVar('converter_locale');
        $journalPath = 		 	$currentJournal->getUrl();//domain.com/index.php/journal
		$this->uploadUrl = 		$journalPath .'/editor/uploadLayoutFile';

		//Validar server is running
		$url = str_replace("http://", "", $this->wsURL);
		$url = str_replace("/convertir.aspx?", "", $this->wsURL);
		$tempPing = $this->ping($url); 
		if($tempPing == FALSE){
			$actualError = '{"status": "error", "actual": "0", "total": "100", "message": "' . __("plugins.generic.converter.error.conection") . '"}';
			$session->setSessionVar('actualError', $actualError);
			return false;
		}

		
		//CREA CARPETA DEL ARCHIVO Y TIPO  A CONVERTIR 
		if(!$fileManager->fileExists($pathFileConCarpeta, 'dir')){
			if(!$fileManager->mkdir($pathFileConCarpeta)){
				$actualError = '{"status": "error", "actual": "0", "total": "100", "message": "' . __("plugins.generic.converter.error.permission") . '"}';
				$session->setSessionVar('actualError', $actualError);
			return false;
			}
		}

		//CREA CARPETA DEL ARCHIVO Y TIPO  A CONVERTIR 
		if(!$fileManager->fileExists($this->pathFileCon, 'dir')){
			if(!$fileManager->mkdir($this->pathFileCon)){
				$actualError = '{"status": "error", "actual": "0", "total": "100", "message": "' . __("plugins.generic.converter.error.permission") . '"}';
				$session->setSessionVar('actualError', $actualError);
			return false;
			}
		}

		//borrar zips anteriores
		$arrayTemp = explode(",", $idFiles);
		$mensaje = '';
		foreach ($arrayTemp as $valor){
			if( !empty($valor) ){
				$rutaLimpiar = $session->getSessionVar('converter_completePath')."archivos/".$valor."/".$convertionType.'/';
				$filesTMP = glob($rutaLimpiar.'*');
				foreach($filesTMP as $file){
				   if(is_file($file)){
				       unlink($file);
				   }
				}
				$mensaje .= "---".$rutaLimpiar;
			}
		}
		
		$mensaje = "mensaje->".$mensaje;
        $jsonStatus = array(
			"actual" => 1,
			"total" => 20,
			"message" => $mensaje
		);


		

		//EXTRAER ARCHIVO DE OJS Y COPIARLO A LA CARPETA ARCHIVOS DEL PLUGIN
		$file = $articleFileManager->readFile($this->fileId, $this->revision);
		$jsonFile = $session->getSessionVar('converter_rutaJson');
		$contStr = "";
		$fileManager->writeFile($jsonFile, $contStr);

		$writed = $fileManager->writeFile($this->pathFileCon . $this->fileName, $file);

		$jsonStatus = array(
			"actual" => 1,
			"total" => 20,
			"message" => __('plugins.generic.converter.gateway.uploadingFile')
		);
		file_put_contents($jsonFile, json_encode($jsonStatus));

		$json = $this->getFileFromServer(  $convertionType );
		if(property_exists($json, 'estatus')){
			$jsonStatus = array(
				"actual" => 0,
				"total" => 100,
				"message" => $json->error
			);
			file_put_contents($jsonFile, json_encode($jsonStatus));
			return false;
		}

		if (!extension_loaded('zip')) {
			$jsonStatus = array(
				"actual" => 0,
				"total" => 100,
				"message" => __('plugins.generic.converter.gateway.errorZip')
			);
			file_put_contents($jsonFile, json_encode($jsonStatus));

			return false;
		}

		$unzipped = $this->unzip($this->pathFileCon, $json->zipFileName);
		$convertedFileName = $this->pathFileCon . $json->fileName;

		$this->postGalley = array('from' => 'submissionEditing',
			'articleId'=> $this->submissionId,
			'layoutFileType' => 'galley',
			'layoutFile' =>	'@'.$convertedFileName);

		if($json->convertionType == "html" or $json->convertionType == "xml" ){
			$jsonStatus = array(
				"actual" => 1,
				"total" => 40,
				"message" => __('plugins.generic.converter.gateway.importingDocument')
			);
			file_put_contents($jsonFile, json_encode($jsonStatus));


			$galleyUrl = $this->insertGalleyFile();


			$imageGalleyUrl = str_replace('editGalley', 'saveGalley', $galleyUrl);
			$proofGalleyUrl = str_replace('editGalley', 'proofGalley', $galleyUrl);

			$this->insertGalleyImage($this->pathFileCon, $imageGalleyUrl, $json, $proofGalleyUrl, $json->convertionType);
		}

	}

	function insertGalleyFile(){
		$cookie = "";

		$ch = curl_init(); 
		curl_setopt($ch, CURLOPT_POST, true);
     	curl_setopt($ch, CURLOPT_URL, $this->loginUrl);
		curl_setopt($ch, CURLOPT_POSTFIELDS, $this->loginParams);
		curl_setopt($ch, CURLOPT_COOKIEJAR, $cookie);
		curl_setopt($ch, CURLOPT_USERAGENT, $this->userAgent);
		curl_exec($ch);

        curl_setopt($ch, CURLOPT_POST, 1);
        curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
        curl_setopt($ch, CURLOPT_VERBOSE,true);
        curl_setopt($ch, CURLOPT_HEADER, 1);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-type: multipart/form-data;') );
        curl_setopt($ch, CURLOPT_POSTFIELDS, $this->postGalley);
        curl_setopt($ch, CURLOPT_URL,$this->uploadUrl);
        curl_exec($ch);
        $chInfo = curl_getinfo($ch);
        curl_close($ch);
        $galleyUrl = $chInfo["url"];

        print_r($chInfo);
        return $galleyUrl;
	}

	function insertGalleyImage($imageFolder, $imageGalleyUrl, $json, $proofGalleyUrl, $convertionType="html"){
		$session =& Request::getSession();
		$convertionType = $session->getSessionVar('converter_convertionType');
        $images = glob($imageFolder."{*.jpg,*.gif,*.png}", GLOB_BRACE);
		$totalImages = count($images);
		$jsonFile = $session->getSessionVar('converter_rutaJson');
		$cnt = 0;
        foreach($images as $image){

            $imagePost = array('label' => strtoupper($convertionType),
            			  'galleyLocale'=> $this->locale,
            			  'imageFile' => '@'.$image,
            			  'uploadImage' => 'Subir'
            );
			
			$cookie = "";
            $ch = curl_init();
			curl_setopt($ch, CURLOPT_POST, true);
	     	curl_setopt($ch, CURLOPT_URL, $this->loginUrl);
			curl_setopt($ch, CURLOPT_POSTFIELDS, $this->loginParams);
			curl_setopt($ch, CURLOPT_COOKIEJAR, $cookie);
			curl_setopt($ch, CURLOPT_USERAGENT, $this->userAgent);
            curl_exec($ch);

            curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-type: multipart/form-data;') );
            curl_setopt($ch, CURLOPT_POSTFIELDS, $imagePost);
            curl_setopt($ch, CURLOPT_URL, $imageGalleyUrl);
            curl_exec($ch);
            curl_close($ch);

			++$cnt;
			$jsonStatus = array(
				"actual" => $cnt,
				"total" => ($cnt / count($images)) * 100,
				"message" => __('plugins.generic.converter.gateway.importingImage') . ': (<b>'.basename($image).'</b>).',
				"proofGalleyUrl" => $proofGalleyUrl,
				"zipDownloadURL" => $this->urlWorkPlugin . $this->fileId . "/".$convertionType."/" . $json->zipFileName
			);
			file_put_contents($jsonFile, json_encode($jsonStatus));
        }

        if($totalImages == 0){
			$jsonStatus = array(
				"actual" => 0,
				"total" => 100,
				"message" => __('plugins.generic.converter.gateway.successImport'),
				"proofGalleyUrl" => $proofGalleyUrl,
				"zipDownloadURL" => $this->urlWorkPlugin . $this->fileId . "/".$convertionType."/" . $json->zipFileName
			);
			file_put_contents($jsonFile, json_encode($jsonStatus));

		}

	}

	function unzip($completePath, $zipFile){
		$zip = new ZipArchive;
		$path = $completePath.$zipFile;
		$res = $zip->open($path);
		if ($res === TRUE) {
			$zip->extractTo($completePath);
			$zip->close();
			return true;
		} else {
			return false;
		}
	}

	function getFileFromServer( $formato = "html" ){
		
		
		//Codigo extra
		$session =& 		Request::getSession();
		$templateMgr2 = 	&TemplateManager::getManager();
		$currentJournal2 =&	$templateMgr2->get_template_vars('currentJournal');
		
		$revistaId =		$currentJournal2->getJournalId(); 
		$revistaTitulo =	$currentJournal2->getJournalTitle();

		$onlineIssn =		$currentJournal2->getSetting('onlineIssn');
		$printIssn = 		$currentJournal2->getSetting('printIssn');
		$abbreviation =		$currentJournal2->getSetting('abbreviation');

		$publisher =		$currentJournal2->getSetting('publisherInstitution');
		
		$abbreviation2 = '';
		foreach($abbreviation as $uno=>$dos)
		{
			$abbreviation2 = $abbreviation2.$dos."||";
		}
		
		$issue2 = 			$templateMgr2->get_template_vars('issue');

		
        $session =& Request::getSession();
        $actualError = $session->getSessionVar('actualError');

        
		$json = @file_get_contents($this->wsURL."cliente=" . $this->client. "&clave=" . urlencode($this->userPass) . "&archivo=" . $this->fileName . "&url=" . $this->urlWorkPlugin . $this->fileId . "/".$formato."/" . "&formato=".$formato
			."&publisher=".urlencode($publisher)."&printIssn=".urlencode($printIssn)."&onlineIssn=".urlencode($onlineIssnonlineIssn)
			."&abbreviation=".urlencode($abbreviation2)."&revistaId=".urlencode($revistaId)."&revistaTitulo=".urlencode($revistaTitulo)
			);
		
		if(empty($json)){
			$actualError = '{"status": "error", "actual": "0", "total": "100", "message": "' . __("plugins.generic.converter.gateway.convertedFileError") . '"}';
			$session->setSessionVar('actualError', $actualError);
			exit(0);
		}
		$json = str_replace("\u0026", "&", $json );
		$json = json_decode($json);
		print_r($json);
		if(property_exists($json, 'estatus')){
			if($json->estatus == 'error'){
				$actualError = '{"status": "error", "actual": "0", "total": "100", "message": "' . $json->mensaje. '"}';
				$session->setSessionVar('actualError', $actualError);
				exit(0);
			}
		}

		$zipFile = file_get_contents(urldecode($json->downloadURL));
		file_put_contents($this->pathFileCon . $json->zipFileName, $zipFile);

		return $json;
		

	}

	function showError(){
		header("HTTP/1.0 500 Internal Server Error");
		echo "";
		exit;
	}

	
	function getStatus(){
        $session 		=& Request::getSession();
        $actualError 	= $session->getSessionVar('actualError');
        $fileId 		= $session->getSessionVar('converter_fileId');
        $pathFileCon 	= $session->getSessionVar('converter_pathFileCon');
        $convertionType = $session->getSessionVar('converter_convertionType');
         
		$jsonFile = $session->getSessionVar('converter_rutaJson');

		if(!empty($actualError)){
			$files = glob($pathFileCon.$convertionType."/"."{*.jpg,*.gif,*.png,*.txt,*.html,*.docx,*.doc,*.xml}", GLOB_BRACE);
			foreach($files as $file){
				unlink($file);
			}

			echo $actualError;
			return;
		}

		if( file_exists($jsonFile) )
			$fileContent = file_get_contents($jsonFile); 
		else{
		    $fileContent='{"actual":1,"total":5 ,"message":"Iniciando..."}';
		}	
		
		
		$json = json_decode($fileContent);

		if($json->total == 100){
			$files = glob($pathFileCon.$convertionType."/"."{*.jpg,*.gif,*.png,*.txt,*.html,*.docx,*.doc,*.xml}", GLOB_BRACE);
			foreach($files as $file){
				unlink($file);
			}
		}	
		

		echo $fileContent;
	}

	function debug_to_console( $data ) {

	    if ( is_array( $data ) )
	        $output = "<script>console.log( 'Debug Objects: " . implode( ',', $data) . "' );</script>";
	    else
	        $output = "<script>console.log( 'Debug Objects: " . $data . "' );</script>";

	    echo $output;
	}

	function getBlockContext() {
		return '';
	}

	function getSupportedContexts() {
		return '';
	}


}



?>

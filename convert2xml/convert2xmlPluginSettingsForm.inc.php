<?php

import('lib.pkp.classes.form.Form');

class convert2xmlPluginSettingsForm extends Form {

	var $journalId;
	var $plugin;

	function convert2xmlPluginSettingsForm(&$plugin, $journalId) {
		$this->journalId = $journalId;
		$this->plugin =& $plugin;

		parent::Form($plugin->getTemplatePath() . 'settingsForm.tpl');

		$this->addCheck(new FormValidator($this, 'wsURL', 		'required', 'plugins.generic.convert2xml.settings.wsURLRequired'));
	}


	function initData() {
		$journalId = $this->journalId;
		$plugin =& $this->plugin;
		$this->setData('wsURL', 		$plugin->getSetting($journalId, 'wsURL'));
		
		
		$currentJournal 	=& 		Request::getJournal();
		$templateMgr 		=		 &TemplateManager::getManager();
		$revistaId 			=		$currentJournal->getJournalId(); 
		$revistaTitulo 		=		$currentJournal->getJournalTitle();
		
		if( isset($revistaTitulo) && !empty($revistaTitulo) ){
			$this->setData('revistaTitulo', 		'plugins.generic.convert2xml.requerimientos.statusOk');
			$this->setData('colorTitulo', 			'color:green;');
		}else{
			$this->setData('revistaTitulo', 		'plugins.generic.convert2xml.requerimientos.statusBad');
			$this->setData('colorTitulo', 			'color:red;');
		}
		
		
		$abbreviation 		=		$currentJournal->getSetting('abbreviation');
		$abbreviation2 = '';
		foreach($abbreviation as $uno=>$dos)
		{
			$abbreviation2 = $dos;
		}
		if( isset($abbreviation2) && !empty($abbreviation2) ){
			$this->setData('abbreviation', 			'plugins.generic.convert2xml.requerimientos.statusOk');
			$this->setData('colorabbreviation', 	'color:green;');
		}else{
			$this->setData('abbreviation', 			'plugins.generic.convert2xml.requerimientos.statusBad');
			$this->setData('colorabbreviation', 	'color:red;');
		}
		
		$printIssn 			= 		$currentJournal->getSetting('printIssn');
		if( isset($printIssn) && !empty($printIssn) ){
			$this->setData('printIssn', 			'plugins.generic.convert2xml.requerimientos.statusOk');
			$this->setData('colorprintIssn', 		'color:green;');
		}else{
			$this->setData('printIssn', 			'plugins.generic.convert2xml.requerimientos.statusBad');
			$this->setData('colorprintIssn', 		'color:red;');
		}
		
		$publisher 			=		$currentJournal->getSetting('publisherInstitution');
		if( isset($publisher) && !empty($publisher) ){
			$this->setData('publisher', 			'plugins.generic.convert2xml.requerimientos.statusOk');
			$this->setData('colorpublisher', 		'color:green;');
		}else{
			$this->setData('publisher', 			'plugins.generic.convert2xml.requerimientos.statusBad');
			$this->setData('colorpublisher', 		'color:red;');
		}
		
		$this->setData('sugerenciaURL', 			$currentJournal->getUrl ());
	}

	function readInputData() {

		$this->readUserVars(array('wsURL'));
	}

	function execute() {
		$plugin =& $this->plugin;
		$journalId = $this->journalId;
		$plugin->updateSetting($journalId, 'wsURL', 		$this->getData('wsURL'), 'string');
	}
}

?>

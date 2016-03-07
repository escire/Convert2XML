<?php

import('lib.pkp.classes.form.Form');

class ConverterPluginSettingsForm extends Form {

	var $journalId;
	var $plugin;

	function ConverterPluginSettingsForm(&$plugin, $journalId) {
		$this->journalId = $journalId;
		$this->plugin =& $plugin;

		parent::Form($plugin->getTemplatePath() . 'settingsForm.tpl');

		$this->addCheck(new FormValidator($this, 'userClient', 	'required', 'plugins.generic.converter.settings.userClientRequired'));
		$this->addCheck(new FormValidator($this, 'userPass', 	'required', 'plugins.generic.converter.settings.userPassRequired'));
		$this->addCheck(new FormValidator($this, 'wsURL', 		'required', 'plugins.generic.converter.settings.wsURLRequired'));
	}


	function initData() {
		$journalId = $this->journalId;
		$plugin =& $this->plugin;

		$this->setData('userClient', 	$plugin->getSetting($journalId, 'userClient'));
		$this->setData('userPass', 		$plugin->getSetting($journalId, 'userPass'));
		$this->setData('wsURL', 		$plugin->getSetting($journalId, 'wsURL'));
	}

	function readInputData() {
		$this->readUserVars(array('userClient'));
		$this->readUserVars(array('userPass'));
		$this->readUserVars(array('wsURL'));
	}

	function execute() {
		$plugin =& $this->plugin;
		$journalId = $this->journalId;
		$plugin->updateSetting($journalId, 'userClient', 	$this->getData('userClient'), 'string');
		$plugin->updateSetting($journalId, 'userPass', 		$this->getData('userPass'), 'string');
		$plugin->updateSetting($journalId, 'wsURL', 		$this->getData('wsURL'), 'string');
	}
}

?>

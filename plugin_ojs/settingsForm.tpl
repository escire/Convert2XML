{**
 * plugins/generic/converter/settingsForm.tpl
 *
 *
 * Converter plugin settings
 *
 *}
{strip}
{assign var="pageTitle" value="plugins.generic.converter.displayName"}
{include file="common/header.tpl"}
{/strip}
<div id="ConverterSettings">
<div id="description">{translate key="plugins.generic.converter.description"}</div>

<div class="separator">&nbsp;</div>

<h3>{translate key="plugins.generic.converter.settings"}</h3>

<form method="post" action="{plugin_url path="settings"}">
{include file="common/formErrors.tpl"}

<table width="100%" class="data">
	<tr valign="top"> 
		<td width="30%" align="right" class="label">{fieldLabel name="userClient" required="true" key="plugins.generic.converter.settings.userClient"}</td>
		<td width="70%" class="value">
			<input type="text" name="userClient" id="userClient" value="{$userClient}" class="textField" data-tool="{translate key="plugins.generic.converter.settings.userClientToolTip"}"/>
																										<span class="fieldtip">{translate key="plugins.generic.converter.settings.userClientToolTip"}</span>
		</td>
	</tr>
	<tr valign="top"> 
		<td width="30%" align="right" class="label">{fieldLabel name="userPass" required="true" key="plugins.generic.converter.settings.userPass"}</td>
		<td width="70%" class="value">
			<input type="password" name="userPass" id="userPass" value="{$userPass}" class="textField" data-tool="{translate key="plugins.generic.converter.settings.userPassToolTip"}"/>
																										<span class="fieldtip">{translate key="plugins.generic.converter.settings.userPassToolTip"}</span>
		</td>
	</tr>
	<tr valign="top"> 
		<td width="30%" align="right" class="label">{fieldLabel name="wsURL" required="true" key="plugins.generic.converter.settings.wsURL"}</td>
		<td width="70%" class="value">
			<input type="text" name="wsURL" id="wsURL" value="{$wsURL}" class="textField" data-tool="{translate key="plugins.generic.converter.settings.wsURLToolTip"}"/>
																										<span class="fieldtip">{translate key="plugins.generic.converter.settings.wsURLToolTip"}</span>
		</td>
	</tr>
</table>

<br/>

<input type="submit" name="save" class="button defaultButton" value="{translate key="common.save"}"/> <input type="button" class="button" value="{translate key="common.cancel"}" onclick="history.go(-1)"/>
</form>
<p><span class="formRequired">{translate key="common.requiredField"}</span></p>
<p><a target="_blanck" style="text-decoration: none;" href="http://converter.escire.mx/admin/Registro.aspx">{translate key="plugins.generic.converter.register"}</a> {translate key="plugins.generic.converter.register2"}</p>
<p>{translate key="plugins.generic.converter.leyenda"}</p>
<p>&copy; <a target="_blanck" style="text-decoration: none;" href="http://escire.mx/">eScire</a> - Consultoría, Tecnologías y Gestión del Conocimiento SA de CV</p>
</div>

{literal}
<style type="text/css">
	/* input field tooltips */
	input + .fieldtip {
	  visibility: hidden;
	  position: relative;
	  bottom: 0;
	  left: 15px;
	  opacity: 0;
	  content: attr(data-tool);
	  height: auto;
	  min-width: 100px;
	  padding: 5px 8px;
	  z-index: 9999;
	  color: #fff;
	  font-size: 10px;
	  font-weight: bold;
	  text-decoration: none;
	  text-align: center;
	  background: #666;
	  -webkit-border-radius: 5px;
	  -moz-border-radius: 5px;
	  border-radius: 5px;
	  -webkit-transition: all 0.2s ease-in-out;
	  -moz-transition: all 0.2s ease-in-out;
	  -ms-transition: all 0.2s ease-in-out;
	  -o-transition: all 0.2s ease-in-out;
	  transition: all 0.2s ease-in-out;
	}

	input + .fieldtip:after {
	  display: block;
	  position: absolute;
	  visibility: hidden;
	  content:'';
	  width: 0;
	  height: 0;
	  top: 8px;
	  left: -8px;
	  border-style: solid;
	  border-width: 4px 8px 4px 0;
	  border-color: transparent rgba(0,0,0,0.75) transparent transparent;
	  -webkit-transition: all 0.2s ease-in-out;
	  -moz-transition: all 0.2s ease-in-out;
	  -ms-transition: all 0.2s ease-in-out;
	  -o-transition: all 0.2s ease-in-out;
	  transition: all 0.2s ease-in-out;
	}

	input:focus + .fieldtip, input:focus + .fieldtip:after {
	  visibility: visible;
	  opacity: 1;
	}

</style>
{/literal}

{include file="common/footer.tpl"}


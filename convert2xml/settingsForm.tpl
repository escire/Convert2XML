{**
 * plugins/generic/convert2xml/settingsForm.tpl
 *
 *
 * convert2xml plugin settings
 *
 *}
{strip}
{assign var="pageTitle" value="plugins.generic.convert2xmlPlugin.name"}
{include file="common/header.tpl"}
{/strip}
<div id="ConverterSettings">
<div id="description">{translate key="plugins.generic.convert2xml.description"}</div>

<div class="separator">&nbsp;</div>

<h3>{translate key="plugins.generic.convert2xml.settings"}</h3>

<form method="post" action="{plugin_url path="settings"}">
{include file="common/formErrors.tpl"}

<table width="100%" class="data">
	<tr valign="top"> 
		<td width="20%" align="left" class="label">{fieldLabel name="wsURL" required="true" key="plugins.generic.convert2xml.settings.wsURL"}</td>
		<td width="70%" class="value">
			<input type="text" name="wsURL" id="wsURL" value="{$wsURL}" class="textField" data-tool="{translate key="plugins.generic.convert2xml.settings.wsURLToolTip"}"/>
																										<span class="fieldtip">{translate key="plugins.generic.convert2xml.settings.wsURLToolTip"}</span>
		</td>
	</tr>
</table>

<br/>

<input type="submit" name="save" class="button defaultButton" value="{translate key="common.save"}"/> <input type="button" class="button" value="{translate key="common.cancel"}" onclick="history.go(-1)"/>
</form>
<p><span class="formRequired">{translate key="common.requiredField"}</span></p>

<h3>{translate key="plugins.generic.convert2xml.requerimientos"}</h3> 
<table width="95%" class="data" border="1" id="requisitosConverter">
	<tr>
		<th width="20%"  class="label">{translate key="plugins.generic.convert2xml.requerimientos.header1"}</th>
		<th width="30%"  class="label">	{translate key="plugins.generic.convert2xml.requerimientos.header2"}</th>
		<th width="40%"  class="label">	{translate key="plugins.generic.convert2xml.requerimientos.header3"}</th>
	</tr>
	<tr>
		<td width="20%"  class="label" style="{$colorTitulo}">{translate key=$revistaTitulo}</td>
		<td width="30%"  class="label">{translate key="plugins.generic.convert2xml.requerimientos1"}</td>
		<td width="40%"  class="label"><a href="{$sugerenciaURL}/manager/setup/1">{translate key="plugins.generic.convert2xml.requerimientos.paso1"}</a></td>
	</tr>
	<tr>
		<td width="20%"  class="label" style="{$colorabbreviation}">{translate key=$abbreviation}</td>
		<td width="30%"  class="label">{translate key="plugins.generic.convert2xml.requerimientos2"}</td>
		<td width="40%"  class="label"><a href="{$sugerenciaURL}/manager/setup/1">{translate key="plugins.generic.convert2xml.requerimientos.paso1"}</a></td>
	</tr>
	<tr>
		<td width="20%"  class="label" style="{$colorprintIssn}">{translate key=$printIssn}</td>
		<td width="30%"  class="label">{translate key="plugins.generic.convert2xml.requerimientos3"}</td>
		<td width="40%"  class="label"><a href="{$sugerenciaURL}/manager/setup/1">{translate key="plugins.generic.convert2xml.requerimientos.paso1"}</a></td>
	</tr>
	<tr>
		<td width="20%"  class="label" style="{$colorpublisher}">{translate key=$publisher}</td>
		<td width="30%"  class="label">{translate key="plugins.generic.convert2xml.requerimientos4"}</td>
		<td width="40%"  class="label"><a href="{$sugerenciaURL}/manager/setup/1">{translate key="plugins.generic.convert2xml.requerimientos.paso1"}</a></td>
	</tr>
</table>
<br/>
<ul>
  <li style="list-style: disc;margin-left: 10px;">{translate key="plugins.generic.convert2xml.requerimientos5"}</li>
  <li style="list-style: disc;margin-left: 10px;">{translate key="plugins.generic.convert2xml.requerimientos6"}</li>
  <li style="list-style: disc;margin-left: 10px;">{translate key="plugins.generic.convert2xml.requerimientos7"}</li>
</ul> 
<br/>
<p>&copy; <a target="_blanck" style="text-decoration: none;" href="http://escire.mx/">eScire</a> - Consultoría, Tecnologías y Gestión del Conocimiento SA de CV</p>
</div>

{literal}
<style type="text/css">

table#requisitosConverter {
    border-collapse: collapse;
}
#requisitosConverter td, #requisitosConverter th{ 
    padding:5px;
	border: 1px solid #d1d1d1;
	text-align: justify;
}

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


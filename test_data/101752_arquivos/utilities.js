 function comparaData (data1, data2)
 {
     if ( parseInt( data2.split( "/" )[2].toString() + data2.split( "/" )[1].toString() + data2.split( "/" )[0].toString() ) >= parseInt( data1.split( "/" )[2].toString() + data1.split( "/" )[1].toString() + data1.split( "/" )[0].toString() ) ) {
         return false;
     } else {
         return true;
     }
 }

/**
* Cria um recurso de Heart Bit para evitar o timeOut de página
*/
function keepAlive(){
	new Ajax.Request( '/informacoes/keepAlive' , { onSuccess:openDialog } );
// 	window.setTimeout(keepAlive, 15000);
}
/**
* Carrega um select com a url passada
*
* @param string value 		Valor selecionado no campo que vai carregar o select
* @param string name 		Nome do campo que vai ser gerado
* @param string divId 		Id da div que vai receber o select gerado
* @param string url 		Endereco que vai ser renderizado
* @param string selected	Valor que vira selecionado
*/
function carregarSelect( value , name , divId , url , selected , obrigatorio )
{
	if( value )
	{
		params = "value=" + value + "&name=" + name + "&selected=" + selected + "&obrigatorio=" + obrigatorio;
		new Ajax.Updater( divId , url , { parameters:params , evalScripts:true } );
	}
}

/**
* Carrega um select com a url passada atraves de um retorno json
*
* @param string value 		Valor selecionado no campo que vai carregar o select
* @param string id 			ID do campo que vai ser carregado
* @param string chave		nome do campo de chave no Json
* @param string texto		nome do campo de texto no Json
* @param string url 		Endereco que vai ser renderizado
* @param string selected	Valor selecionado da select gerada
*/
function carregarSelectPorJson( value , id , chave , texto , url , selected )
{
	if( value )
	{
		params = "value=" + value + "&name=" + id + "&selected=" + selected;
		new Ajax.Request( url ,
			{
				asynchronous: true ,
				method: 'post',
				parameters: params ,
				onSuccess: function( response )
				{
					var objResult = eval( "(" + response.responseText + ")");
					
					$(id).options.length = 0;
					
					objResult.each( 
						function( objEntidade )
						{
							newOp = new Option();
							newOp.text = eval( "objEntidade." + texto ) ;
							newOp.value = eval( "objEntidade." + chave ) ;
							newOp.setAttribute( "onMouseOver" , "SuperTitleOn( this , this.text )" );
							newOp.setAttribute( "onMouseOut" , "SuperTitleOff(this) " );
							$(id).options[$(id).length] = newOp;
						}
					)
					loadTips();
				}
			}
		);
		
		
		
		//new Ajax.Updater( divId , url , { parameters:params , evalScripts:true } );
	}
}

/**
* Carrega uma div com a url passada
*
* @param string value 	Valor selecionado no campo que vai carregar o select
* @param string divId 	Id da div que vai receber o select gerado
* @param string url 	Endereco que vai ser renderizado
*/
function carregarDiv( value , divId , url )
{
	if( value )
	{
		params = "value=" + value;
		new Ajax.Updater( divId , url , { parameters:params , evalScripts:true } );
	}
}


var inputEl;
var checkMsg;

function verifyExists( el , url , msg , id )
{
	if( !id )
	{
		inputEl = el;
		checkMsg = msg;
		if( el.value )
		{
			params = "value=" + el.value;
			new Ajax.Request( url , { parameters:params , onSuccess:checkExists } );
		}
	}
}

function checkExists( response )
{
	if( parseInt( response.responseText ) == 1 )
	{
		alert( checkMsg );
		inputEl.value = "";
	}
	else if( parseInt( response.responseText ) == 2 )
	{
		window.location.reload();
	}
}

/**
* Abre um popup( div ) com o conteudo da url passada
*
* @param url
*/
function popup( url )
{
	new Ajax.Request( url , { onSuccess:openDialog } );
}

/**
* Abre um Modal (2ºNivel de popup / div ) com o conteudo da url passada
* 
* Obs: implementação provisória destinada a dar solução rápida para dois níveis de popup. A variável Modal é um duplicata de Dialog. 
* Implementação deve ser melhorada ou substituída no futuro
*
* @param url
*/
function modal( url )
{
	new Ajax.Request( url , { onSuccess:openModal } );
}

function popupClosePrevious( url ){
	new Ajax.Request( url , { onSuccess: function(response){
		Dialog.closeInfo();
		openDialog(response);
	} } );
}

/**
* Salva as informações abertas no popup
*
* @param string  url 		 Endereço que sera salva as informacoes
* @param string  formId	 Id do formulario que sera salvo
* @param string  divId		 Id da div que receberá a resposta
* @param boolean isValidate Flag se o formulario sera validado
* @param function fnOnSuccess Função que será executada quando completar a execução
*/
function saveDialog( url , formId , divId , isValidate , fnOnSuccess )
{
	if( isValidate )
	{
		if( !oValidate.verify( $( formId ) ) ) return false;
	}
	new Ajax.Updater( divId , url , { evalScripts:true , parameters:Form.serialize( formId ) , onSuccess: fnOnSuccess , onComplete: closeDialog} );
}

/**
* Salva as informações abertas no Modal (2ºNivel de popup)
*
* @param string  url 		 Endereço que sera salva as informacoes
* @param string  formId	 Id do formulario que sera salvo
* @param string  divId		 Id da div que receberá a resposta
* @param boolean isValidate Flag se o formulario sera validado
* @param function fnOnSuccess Função que será executada quando completar a execução
*/
function saveModal( url , formId , divId , isValidate , fnOnSuccess )
{
	if( isValidate )
	{
		if( !oValidate.verify( $( formId ) ) ) return false;
	}
	new Ajax.Updater( divId , url , { evalScripts:true , parameters:Form.serialize( formId ) , onSuccess: fnOnSuccess , onComplete: closeModal} );
}

function saveModalAndCloseDialog( url , formId , divId , isValidate , fnOnSuccess )
{
	if( isValidate )
	{
		if( !oValidate.verify( $( formId ) ) ) return false;
	}
//	closeDialog();
//	closeModal();
//	openDialog(request);
	//fnOnSuccess
	new Ajax.Updater( divId , url , { evalScripts:true , parameters:Form.serialize( formId ) , onSuccess: function(response){
		closeDialog();
		openDialog(response);
	}, onComplete: closeModal} );
}

/**
* Abre o popup com a resposta do ajax
*
* @param object XMLHTTPRequest
*/
function openDialog( response )
{
	setTimeout(function() {response.responseText.evalScripts()}, 10);
	
	showDialog( response.responseText ); 
}

/**
* Mostra o popup com um conteúdo HTML
*
* @param string Conteúdo HTML
*/
function showDialog( htmlContents )
{
	Dialog.info( htmlContents, {windowParameters: {className:'alphacube', recenterAuto: false, resizable: true, width: 700, scrollable: false, draggable: true, showEffect:Element.show , hideEffect:Element.hide}});
	
}


/**
* Abre o Modal (2ºNivel de popup) com a resposta do ajax
*
* @param object XMLHTTPRequest
*/
function openModal( response )
{
	setTimeout(function() {response.responseText.evalScripts()}, 10);
	
	showModal( response.responseText ); 
}

/**
* Mostra o Modal (2ºNivel de popup) com um conteúdo HTML
*
* @param string Conteúdo HTML
*/
function showModal( htmlContents )
{
	Modal.info( htmlContents, {windowParameters: {className:'alphacube', recenterAuto: false, resizable: true, width: 700, scrollable: false, draggable: true, showEffect:Element.show , hideEffect:Element.hide}});
	
}


/**
* Fecha um dialago aberto
*/
function closeDialog()
{
	Dialog.closeInfo();
}

/**
* Fecha um Modal (2ºNivel de popup) aberto
*/
function closeModal()
{
	Modal.closeInfo();
}



function setRadioValue( hiddenId , el )
{
	$(hiddenId).value = el.value;
}

function showHidden( id , button )
{
	if( $(id).style.display == "none" )
	{
		$(id).style.display = "";
		$(button).src = $(button).src.replace('plus.jpg', 'minus.jpg');
		$(button).alt = "Ocultar Bolsistas";
	}
	else
	{
		$(id).style.display = "none";
		$(button).src = $(button).src.replace('minus.jpg', 'plus.jpg');
		$(button).alt = "Mostrar Bolsistas";
	}
}

function diffMeses(dataIni, dataFim)
{
	var a = dataIni.getTime();
	var b = dataFim.getTime();
	if(a > b) {
		return Math.round((a - b)/ (86400000 * 30));
	}else{
		return Math.round((b - a)/ (86400000 * 30));
	}
}

/**
* Objeto em javascript
* [id] identificador
* [cd] código
*/
function registroIdCd( stId , stCd )
{
	this.Id=stId || '';
	this.Cd=stCd || '';
}

/**
* Esta função preenche um componente com o valor de outro
* [ob] objeto html preenchedor
* [ob] objeto html a ser preenchido
* [ar] array de dados correlativo (opcional)
*/
function preencheCampo(Preenchedor, Preenchido, arDados)
{
	arDados = arDados || '';
	if(arDados == ''){
		Preenchido.value = '';
		Preenchido.value = Preenchedor.value;
		return;
	}else{
		Preenchido.value = '';
		if(Preenchedor.type == 'select-one'){
			for(dado = 0; dado < arDados.length; dado++){
				if(Preenchedor.value == arDados[dado].Id){
					Preenchido.value = arDados[dado].Cd;
					return;
				}
			}
		}else{
			for(dado = 0; dado < arDados.length; dado++){
				if(Preenchedor.value == arDados[dado].Cd){
					Preenchido.value = arDados[dado].Id;
					return;
				}
			}
		}
	}
}

/**
 * Formata número
 */
function formataApenasNumero(inputTexto, x) {

  if (x)
      inputTexto.value = inputTexto.value.replace(/[^0-9,x,X]/g,'');
  else
      inputTexto.value = inputTexto.value.replace(/[^0-9]/g,'');
}

/**
 * Esta função formata um valor em real
 * [st] string valor // ex: 9999.00 // 9999
 */
function formataReal( valor )
{
	var i = 0;
	var j = 0;
	var l = 0;
	var string = "";

	valor = new String( valor );
	l = valor.length;
	
	for( i = l; i > 0; i-- )
	{
		if( j % 3 == 0 && j != 0)
		{
			string = "." + string;
		}
		string = valor.charAt(i-1) + string;
		j++;
		
	}
	string = string + ",00";

	return string;
	
}

function helperCheckAll( valor, form )
{
	/*
	for( i = 1; i < form.elements.length; i++ ){
 		check = form.elements[i];

		if( check && check.type == 'checkbox' ) check.checked = valor;
	}
	*/
	

	J("input[type='checkbox']").each(
		function(){
			this.checked = valor;
		}
	);
}

function helperCheckAllGroup( valor, form, stringExpression )
{
	try{
		var re = new RegExp( stringExpression );
		
	 	/*
	 	for( i = 1; i < form.elements.length; i++ ){
	 		check = form.elements[i];
	 		
			if( check && check.type == 'checkbox' && re.test(check.name) )
			{
				check.checked = valor;						
			}
		}
		*/

		J("input[type='checkbox']").each(
			function(){
				if( re.test(this.name) )
				{
					this.checked = valor;
				}
			}
		);
	}catch(e){
		alert(e.message);
	}
}

function carregando( display )
{
	try{
		if( display )
		{
			var pageSize = WindowUtilities.getPageSize();

			$('carregando').style.top			= '0';
			$('carregando').style.height		= pageSize.pageHeight + 'px';
			$('carregando').style.paddingTop	= '25%';
			$('carregando').style.display		= '';
			document.body.style.cursor			= 'wait';
		}else{
			$('carregando').style.display		= 'none';
			document.body.style.cursor			= 'auto';
		}
	}catch(e){
		alert(e.message);
	}
}

function barCarregando( display )
{
	try{
		if( display )
		{
			if( $('carregando').style.display == '') return 0;
			$('carregando').style.top			= document.body.scrollTop;
			$('carregando').style.height		= '80px';
			$('carregando').style.paddingTop	= '8px';
			$('carregando').style.display		= '';
			document.body.style.cursor			= 'wait';
		}else{
			$('carregando').style.display		= 'none';
			document.body.style.cursor			= 'auto';
		}
	}catch(e){
		alert(e);
	}
}
//Serve para formatar um number em formato dinheiro
Number.prototype.formatMoney = function(c, d, t){ 
var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t,
i = parseInt(n = (+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
return (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t)
+ (c ? d + (n - i).toFixed(c).slice(2) : "");
};
//fim da função...

String.prototype.lpad = function(pSize, pCharPad)
{
	var str = this;
	var dif = pSize - str.length;
	var ch = String(pCharPad).charAt(0);
	for (; dif>0; dif--) str = ch + str;
	return (str);
} //String.lpad

/**
 * Função responsável por redirecionar o navegador para uma determinada URL
 * 
 * @param URL string
 * 
 */
function carregarUrl( url )
{
	window.location.href = url; 
} 
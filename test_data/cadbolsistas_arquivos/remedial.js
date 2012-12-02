

/**
 *@link http://www.crockford.com/javascript/remedial.html
 *@date 01/11/2005
 *@version 2.0
 * with changes
 */

/**
 * Confere se o parametro ? externo do javascript
 * Check is the parameter is alien of the javascript
 *
 *@param  objElement
 *@return bool
 */
function isAlien( objElement )
{
	return isObject( objElement ) && typeof objElement.constructor != 'function';
}

/**
 * Confere se o parametro enviado ? um array
 * Check if the parameter sended is one array
 *
 @param mixElement
 @return bool
 */
function isArray( objMix )
{
	return isObject( objMix ) && objMix.constructor == Array;
}

/**
 * Confere se o objeto enviado ? um boleano
 * Check if the parameter sended is one bool
 *
 @param mixElement
 @return bool
 */
function isBoolean( objMix )
{
	return typeof objMix == 'boolean';
}

/**
 * Confere se o objeto enviado ? vazio
 * Check if the object sended is empty
 *
 @param objElement
 @return bool
 */
function isEmpty( objElement )
{
	var strProperty, mixValue;
	if ( isObject( objElement ) )
	{
		for ( strProperty in objElement )
		{
			mixValue = o[ strProperty ];
			if ( isUndefined( mixValue ) && isFunction( mixValue ) )
			{
				return false;
			}
		}
	}
	return true;
}

/**
 * Confere se o parametro enviado ? uma fun??o
 * Check if the parameter sended is one function
 *
 @param mixElement
 @return bool
 */
function isFunction( mixElement )
{
	return typeof mixElement == 'function';
}


function existFunction( strNameFunction )
{
	if ( window[ strNameFunction ] != undefined )
	{
		return isFunction( window[ strNameFunction ] );
	}
	else
	{
		return false;
	}
}

/**
 * Confere se o parametro enviado ? nulo
 * Check if the parameter sended is null
 *
 @param mixElement
 @return bool
 */
function isNull( mixElement )
{
	return typeof mixElement == 'object' && !mixElement;
}

/**
 * Confere se o parametro enviado ? um n?mero
 * Check if the parameter sended is one number
 *
 @param mixElement
 @return bool
 */
function isNumber( mixElement )
{
	return typeof mixElement == 'number' && isFinite( mixElement );
}

/**
 * Confere se o elemento enviado ? um objeto
 * Check if the element sended is one object
 *
 @param mixElement
 @return bool
 */
function isObject( mixElement )
{
	return ( mixElement && typeof mixElement == 'object' ) || isFunction( mixElement );
}

/**
 * Confere se o elemento enviado ? um texto
 * Check if the element sended is one string
 *
 @param mixElement
 @return bool
 */
function isString( mixElement )
{
	return typeof mixElement == 'string';
}

/**
 * Confere se o elemento enviado ? indefinido
 * Check if the element sended is undefined
 *
 @param mixElement
 @return bool
 */
function isUndefined( mixElement )
{
	return typeof mixElement == 'undefined';
}

/**
 * Muda alguns caracters para o modo html
 * Change some chars to html mode
 *
 @param string strText
 @return string
 */
function entityify( strText )
{
	if (isUndefined(strText))
	{
		return "undefined";
	}

	strText = strText + "";

	strText = strText
	.replace( /&/g , "&amp;" )
	.replace( /</g , "&lt;" )
	.replace( />/g , "&gt;" );

	strText = strText;
	strText = replaceAll( strText, "\n", "<br/>\n" );
	strText = replaceAll( strText, " ", "&nbsp;" );

	return strText;
};

function quote( strText )
{
	var c, i, l = strText.length, o = '"';
	for (i = 0; i < l; i += 1)
	{
		c = strText.charAt(i);
		if (c >= ' ')
		{
			if (c == '\\' || c == " \"" )
			{
				o += '\\';
			}
		o += c;
		}
		else
		{
			switch (c)
			{
				case '\b':
					o += '\\b';
					break;
				case '\f':
					o += '\\f';
					break;
				case '\n':
					o += '\\n';
					break;
				case '\r':
					o += '\\r';
					break;
				case '\t':
					o += '\\t';
					break;
				default:
					c = c.charCodeAt();
					o += '\\u00' + Math.floor(c / 16).toString(16) +
						(c % 16).toString(16);
			}
		}
	}
	return o + '"';
};

/**
 * Remove os espacos em branco dos extremos de um string
 * Remove the white spaces os the limits of some string
 *
 @param string strText
 @return string
 */
function trim( strText )
{
	if (! strText)
	{
		return "";
	}
	return strText.replace(/^\s*(\S*(\s+\S+)*)\s*$/, "$1");
};


/**
 * remove as ocorrencias de um texto nos extremos do outro
 *@param string strText
 *@param sttring strPeace
 *@param bool boolLeft
 *@param bool boolRigth
 *@return string
 */
function trimString ( strText, strPeace, boolLeft, boolRight )
{
	strText += "";

	if (boolLeft == undefined)
	{
		boolLeft = true;
	}

	if (boolRight == undefined)
	{
		boolRight = false;
	}

	intPos = strText.indexOf( strPeace ) ;

	if ( boolLeft )
	while ( intPos == 0 )
	{
		strText = strText.substring( strPeace.length );
		intPos = strText.indexOf( strPeace ) ;
	}

	if ( boolRight )
	while ( intPos == strText.length - strPeace.length )
	{
		strText = strText.substring( 0 , intPos ) ;
		intPos = strText.indexOf( strPeace ) ;
	}

	return strText;
}

/**
 * adiciona texto ao final do corpo do html
 * add text into end of the body
 *@param string strText
 *@param object Div objDivPlace
 *@return void
 */
function print( strText , objDivPlace)
{
	if (objDivPlace == undefined)
		objDivPlace == document.body;

	objDivPlace.innerHTML += strText;
}


/**
 * substitui todas as ocorrencias de um string por outro
 * replace all the ocurrence of some string to another
 *
 *@param  string strText
 *@param  string strFinder
 *@param  string strReplacer
 *@return bool
 */
function replaceAll (strText , strFinder, strReplacer)
{
	strText += "";
	var strSpecials = /(\.|\*|\^|\?|\&|\$|\+|\-|\#|\!|\(|\)|\[|\]|\{|\}|\|)/gi; // :D
	strFinder = strFinder.replace(strSpecials, "\\$1")

	var objRe = new RegExp(strFinder, "gi");
	return strText.replace(objRe, strReplacer);
}

function strCount( strSearch, strText )
{
	strText += "";
	var intCount = 0;

	var intPos = strText.indexOf( strSearch );

	while ( intPos != -1 )
	{
		intCount++;
		strText	= strText.substr( intPos + 1 );
		intPos = strText.indexOf( strSearch );
	}

	return intCount;
}

/**
 * concatena todos os itens de um array ligando-os por um string
 *
 * join all the array elements concatened with one string
 *@param array
 *@param string $str
 *@return string
 */
function implode( strGlue, arrPieces)
{
	if ( arrPieces == undefined )
		return "undefined";
	var strResult = "";
	for(var i = 0; i < arrPieces.length; ++i)
	{
		if (strResult != "")
		{
			strResult += strGlue;
		}

		strResult += arrPieces[i];
	}
	return strResult;
}

function implode_complex( strGlue, arrPieces , intLineCount )
{
	if ( arrPieces == undefined )
		return "undefined";
	if ( intLineCount == undefined )
	{
		var intLineCount = 0;
	}

	var intLineGlue = strCount( "\n" , strGlue );
	var strResult = "";
	for(var i = 0; i < arrPieces.length; ++i)
	{
		if (strResult != "")
		{
			var strPeace = strGlue;
			strPeace = replaceAll( strPeace , "%line" , intLineCount );
			strPeace = replaceAll( strPeace , "%count" , i );
			strPeace = replaceAll( strPeace , "%row" , str_slice_line( arrPieces[i] ) );
			strResult += strPeace;
		}

		intLineCount += strCount( "\n" , arrPieces[i] );
		intLineCount += intLineGlue;

		strResult += arrPieces[i];
	}
	return strResult;
}


function explode( strSeparator, strText )
{

	strText += "";
	strSeparator + "";
	var arrResult = Array();
	var intSeparatorPos = strText.indexOf( strSeparator );

	while( intSeparatorPos != -1 )
	{
		var strBefore 	= strText.substr( 0 , intSeparatorPos );
		var strAfter	= strText.substr( intSeparatorPos + 1 );

		arrResult.push( strBefore );
		strText = strAfter;

		var intSeparatorPos = strText.indexOf( strSeparator );

	}
	arrResult.push( strText );
	return arrResult;
}
/**
 * Serarch for a item in the array and return his position if founded or -1
 * in other case
 *@date 2005-11-06 17:35
 *@param mixObject mixElement
 *@param array arrConteiner
 *@return integer
 */
function array_search( mixElement, arrConteiner )
{
	if (!arrConteiner)
		return -1;

	for(var i = 0; i < arrConteiner.length; ++i)
	{
		if (arrConteiner[i] == mixElement)
			return i;
	}
	return -1;
}

/**
 * Retorna o Html da Tag ( incluindo a mesma )
 * Return the outer Html of the Tag
 *
 */
function getHtmlString( objDomHtml )
{
	if (!objDomHtml)
		return "";

	if ( isString(objDomHtml) || isNumber(objDomHtml) )
	{
		return objDomHtml;
	}

	if ( objDomHtml.outerHTML )
	{
		return( objDomHtml.outerHTML );
	}

	var parentDiv = document.createElement("div");
	parentDiv.appendChild( objDomHtml );
	return parentDiv.innerHTML;
}

/**
 * Replace a mask in a strHtml for the element
 *@param string strHTML
 *@param string Key
 *@param mix objReplacer
 *@param string Key
 *@param mix objReplacer
 *@param string Key
 *@param mix objReplacer
 *@param ...
 *@return string
 */
function createHtmlByElements( strHTML )
{
	arrArguments = createHtmlByElements.arguments;
	if (arrArguments.length == 0)
	{
		return strHTML;
	}

	if (arrArguments.length % 2 == 0)
	{
		debuggerAlert( 'invalid paramaters numbers ' );
		return strHTML;
	}

	for (var i = 1; i < arrArguments.length; i += 2)
	{
		strSearch = arrArguments[ i ];
		mixReplace = arrArguments[ i + 1 ];
		if ( !isString(mixReplace) && !isNumber(mixReplace) )
		{
			mixReplace = getHtmlString( mixReplace );
		}

		strHTML = replaceAll( strHTML, strSearch, mixReplace );
	}

	return strHTML;
}


var autoId = 0;

/**
 * Set a unique and valid Id for some object
 *@param object objElement
 *@return string strHeader
 */
function setId( objElement, strHeader )
{
	if ( (objElement.id == undefined) || (objElement.id == "") )
	{
		objElement.id = strHeader + autoId++;
	}
	return objElement.id;
}

IE = document.all ? true : false;
var mouseX = 0;
var mouseY = 0;

/**
 * Active the function to get the mouse position
 *@return void
 */
function activeMouseGetPos()
{
	if (!IE) document.captureEvents(Event.MOUSEMOVE);

	// Set-up to use getMouseXY function onMouseMove
	document.onmousemove = getMouseXY;

	// Temporary variables to hold mouse x-y pos.s

	// Main function to retrieve mouse x-y pos.s
}

/**
 * Called function when the mouse move
 *@version 1.0
 *@param intX
 *@param intY
 *@return bool
 */
function mouseMove( intX , intY)
{
	return true;
}

function getMouseXY(e)
{
	if (IE)
	{
		mouseX = event.clientX + document.body.scrollLeft;
		mouseY = event.clientY + document.body.scrollTop;
	}
	else
	{
		mouseX = e.pageX;
		mouseY = e.pageY;
	}
	if (mouseX < 0){mouseX = 0}
	if (mouseY < 0){mouseY = 0}

	document.MouseX = new Object();
	document.MouseY = new Object();
	document.MouseX.value = mouseX;
	document.MouseY.value = mouseY;

	return mouseMove( mouseX, mouseY);
}

function str_repeat( strText, intRepeat )
{
	strReturn = "";
	for ( var i = 0; i < intRepeat; ++i)
	{
		strReturn += strText;
	}
	return strReturn;
}

function str_slice_line( strText )
{
	var strReturn = strText;
	strReturn = replaceAll( strText, "\"" , "\\\'\\'" );
//	strReturn = replaceAll( strReturn, "\'" , "\\\'" );
//	strReturn = replaceAll( strReturn, "\n" , "\\n " );
	strReturn = replaceAll( strReturn, "\n" , " " );
	return strReturn;
}

function str_slice( strText )
{
	var strReturn = strText;
	strReturn = replaceAll( strText, "\"" , "\\\\\"" );
	strReturn = replaceAll( strReturn, "\'" , "\\\'" );
	return strReturn;
}

function debuggerOutFunction()
{
}

function debuggerEnterFunction( strFunction )
{
}

function debuggerAlert()
{
}

function debuggerFunction( funcEnter )
{
	return funcEnter;
}

function showHide( object )
{
  if (object.style.display == "none")
  {
    object.style.display = "block";
    return true;
  }
  else
  {
    object.style.display = "none";
    return false;
  }
}


function removeChildNodes( object )
{
	if( object )
	{
		while ( object.childNodes.length > 0 )
		{
			object.removeChild( object.childNodes[0] , true );
		}
	}
}

function strToDate( strDate )
{
    var arrDate = explode( "/" , strDate  );
    var objDate = new Date( arrDate[2] , arrDate[1] - 1 , arrDate[0] );
    return objDate;
}

// Funcao que realiza um cast array, ou seja, converte um objeto em array
function parseArray( objElement )
{
    // Criando array
    var arrNew = new Array();

    // Caso seja uma colecao
    if( objElement.length )
    {
        for( var i = 0; i < objElement.length; ++i )
        {
            arrNew[ i ] = objElement[ i ];
        }
    }
    // Caso nao seja uma colecao
    else
    {
        for( strAttribute in objElement )
        {
            if( isFunction( objElement[ strAttribute ] ) )
            {
                arrNew[ arrNew.length ] = objElement[ strAttribute ];
            }
        }
    }
    // Retorno da funcao
    return( arrNew );
}

// Funcao que realiza um cast array, ou seja, converte um objeto em array
function parseArrayRecursive( objElement , intDepthOfRecursive )
{
	// Profundidade da recursividade
	if( intDepthOfRecursive == undefined )
	{
		intDepthOfRecursive = -1;
	}
	if( intDepthOfRecursive > 0)
	{
		--intDepthOfRecursive;
	}
	else
	{
		if( intDepthOfRecursive == 0 )
		{
			return objElement;
		}
		if( intDepthOfRecursive < 0 )
		{
			return null;
		}
	}

    // Criando array
    var arrNew = new Array();

    if( objElement == undefined )
    {
        // elemento vazio
    }
    else if( isString( objElement) || isNumber( objElement ) || isBoolean( objElement ) )
    {
       arrNew[ arrNew.length ] = objElement;
    }
    // Caso seja uma colecao
    else if( objElement.length )
    {
        for( var i = 0; i < objElement.length; ++i )
        {
            arrNew[ i ] = parseArrayRecursive( objElement[ i ] , intDepthOfRecursive );
        }
    }
    // Caso seja uma colecao
    else if( objElement.innerHTML )
    {
        arrNew = objElement.innerHTML;
    }
    // Caso nao seja uma colecao
    else if( isObject( objElement ) )
    {
        for( strAttribute in objElement )
        {
            if( ! isFunction( objElement[ strAttribute ] ) )
            {
                arrNew[ arrNew.length ] = parseArrayRecursive( objElement[ strAttribute ] , intDepthOfRecursive ) ;
            }
        }
    }
    else
    {
    	arrNew = null;
    }
    // Retorno da funcao
    return( arrNew );
}


function truncate( strText , intLength )
{
	var etc = "...";
	var strReturn;
	if ( strText.length  > intLength ) {
        intLength -= etc.length ;

        regex = /\s+?(\S+)?$/;

        strText.replace( regex , strText.substring( 0 , intLength + 1 ) );

         var strReturn = strText.substring( 0 , intLength) + etc ;
    } else {
        strReturn = strText;
    }
    return( strReturn );
}

/**
 * @param objeto form (delimitador inicial)
 * @param qual tag sera procurada
 * @param qual o atributo sera procurado
 * @param qual o valor do atributo a ser procurado
 **/
function getElementsByAttribute(oElm, mixTagName, strAttributeName, strAttributeValue){
	if( typeof( mixTagName ) == "string" )
	{
    	var arrElements = (mixTagName == "*" && oElm.all)? oElm.all : oElm.getElementsByTagName(mixTagName);
	}
	else if( typeof( mixTagName ) == "object" )
	{
		var arrElements = new Array();
		var arrOneOfElements;
		for( var i = 0 ; i < mixTagName.length; ++i )
		{
			arrOneOfElements = oElm.getElementsByTagName( mixTagName[ i ] );
			arrElements.concat( arrOneOfElements );
		}
	}
	else
	{
		var arrElements = oElm.all;
	}

    var arrReturnElements = new Array();

    var oAttributeValue = (typeof strAttributeValue != "undefined")? new RegExp("(^|\\s)" + strAttributeValue + "(\\s|$)") : null;
    var oCurrent;
    var oAttribute;
    for(var i=0; i<arrElements.length; i++)
    {
        oCurrent = arrElements[i];
        oAttribute = oCurrent.getAttribute && oCurrent.getAttribute(strAttributeName);
        if(typeof oAttribute == "string" && oAttribute.length > 0)
        {
            if(typeof strAttributeValue == "undefined" || (oAttributeValue && oAttributeValue.test(oAttribute)))
            {
                arrReturnElements.push(oCurrent);
            }
        }
    }
    return arrReturnElements;
}


function checkChildrenCheckbox( objFatherCheckbox )
{
	var arrChildrenCheckbox = getElementsByAttribute( document.body , "input" , "father" , objFatherCheckbox.id );
	for( var i in arrChildrenCheckbox )
	{
		arrChildrenCheckbox[ i ].checked = objFatherCheckbox.checked;
	}
}

function hideObject( objElement )
{
	if( isObject( objElement ) )
	{
		$( objElement ).style.display = "none";
	}
}

function showObject( objElement )
{
	if( isObject( objElement ) )
	{
		$( objElement ).style.display = "block";
	}
}

function addSlashes( strText )
{
	strText = replaceAll( strText , "\'" , "\\'");
	strText = replaceAll( strText , '"' , '\"');

	return( strText );
}

function safePlic( strText )
{
	strText = replaceAll( strText , "'" , "&#39;");
	strText = replaceAll( strText , '"' , "&#34;");

	return( strText );
}

function urlencode( strText )
{
	strText = replaceAll( strText , "%" , "%25" );
	strText = replaceAll( strText , "&" , "%26" );
	strText = replaceAll( strText , "#" , "%23" );
	strText = replaceAll( strText , " " , "%20" );
	return( strText );
}
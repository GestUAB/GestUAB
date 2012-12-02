/**
 * Classe para validação dinâmica de formulários
 * Esta classe depende dos arquivos base.js e enumerable.js do Prototype( http://prototype.conio.net )
 * 
 * 
 * Em sua página de formulários crie o Objeto Validate e adicione as validações:
 * 
 * var objValidate = new Validate();
 * objValidate.addValidate( 'id_do_campo' , funcao_de_validação );
 * 
 * Ex:
 * 
 * var objValidate = new Validate();
 * 
 * objValidate.addValidate( 'nuCPF'  , objValidate.cpf    , 'CPF invalido' );
 * objValidate.addValidate( 'nuCPF'  , objValidate.isnull , 'Campo de preenchimento obrigatorio' , false );
 * objValidate.addValidate( 'nuCNPJ' , objValidate.cnpj   , 'CNPJ invalido' );
 * 
 * E na hora de submeter o formulário chamar objValidate.verify() 
 * 
 * @author Abdala Cerqueira - abdala.cerqueira@gmail.com
 */

// The Validate Class.
var Validate = function()
{
	this.arrValidate = [];
  
	this.verify = function( oForm )
	{
  		var n = this.arrValidate.length;
  		var boolValidate = true;
  		var arrPendency = [];
  	
  		for ( var i = 0 ; i < n ; i++ ) 
		{
			
			if( this.verifyForm( oForm , this.arrValidate[i].id ) )
			{
				boolValidate = this.arrValidate[i].func( this.arrValidate[i].id , this.arrValidate[i].value );
				if( !boolValidate )
				{
					carregando(false);
					alert( this.arrValidate[i].msg );
					setTimeout( "document.getElementById( '" + this.arrValidate[i].id + "' ).focus()" , 10 );
					return false;
				}
			}
		}
		
		for ( var i = 0 ; i < n ; i++ ) 
		{
			if( this.verifyForm( oForm , this.arrValidate[i].id ) )
			{
				var TRIM = document.getElementById( this.arrValidate[i].id ).value.replace(/^\s*|\s*$/g , "" );
				document.getElementById( this.arrValidate[i].id ).value = TRIM;
				this.clearMask( this.arrValidate[i].id , this.arrValidate[i].func );
			}
		}
		
		return true;
	}
	
	this.addValidate = function( id , func , msg , value ) 
	{
  		var o = {};
  	
  		o.id 	 = id;
  		o.func 	 = func; 
  		o.msg 	 = msg;
  		o.value  = value;
  	
  		this.arrValidate.push( o );
	}
	
	this.clearMask = function( id , func )
	{
		var CLEAR;
		var TRIM = document.getElementById( id ).value.replace(/^\s*|\s*$/g , "" );
		
		if(  ( func != this.isnull ) && ( func != this.maxlength ) && ( func != this.upperCase ) && ( func != this.type ) )
		{
			if( func == this.data )
			{
				if( TRIM )
				{
					a = TRIM.split( "/" );
					CLEAR = a[2] + "-" + a[1] + "-" + a[0];
				} 
				else
				{
					CLEAR = null;
				}
			}
			else if(  func == this.dinheiro )
			{
				CLEAR = TRIM.replace( "." , "" ).replace( "." , "" ).replace( "." , "" ).replace( "," , "." );
			}
			else if(  func == this.url )
			{
				CLEAR = null;
			}
			else if(  func == this.email )
			{
				CLEAR = null;
			}
			else if(  func == this.descricao )
			{
				CLEAR = null;
			}
			else if(  func == this.dateDependency )
			{
				CLEAR = TRIM;
			}
			else
			{
				CLEAR = TRIM.replace( /[\.\-\/]/g , "" );
			}	
			
			if( CLEAR == null )
			{
				//document.getElementById( id ).disabled = "disabled";
			}
			else
			{
				document.getElementById( id ).value = CLEAR;
			}
		}
	}
  
  	this.clearValidate = function( n ) 
  	{
  		for ( var i = 0 ; i < n ; i++ ) 
		{
			document.getElementById( this.arrValidate[i].id ).style.border="";
		}
	}
  
  	this.removeValidate = function( id )
  	{
		for ( var i = 0 ; i < this.arrValidate.length ; i++ ) 
		{
			if( this.arrValidate[i].id == id )
			{
				this.arrValidate.splice( i , 1 );
			}
		}
 	}
  
  	this.verifyForm = function( oForm , id )
  	{
 	
	  	var n = oForm.elements.length;
	  	for ( var i = 0 ; i < n ; i++ ) 
		{
			if( oForm.elements[i].id == id )
				return true;
		}
		return false;
  	}
  
  	this.maxlength = function( id , value )
  	{
	  	var MAXLENGTH = document.getElementById( id ).value;
	  	if( MAXLENGTH.length > value )
		{
			return false;
		}
		return true;
 	}
 	
 	this.maxvalue = function( id , value )
  	{
	  	var maxvalue = document.getElementById( id ).value;
	  	
	  	if( maxvalue > value )
		{
			return false;			
		}
		return true;
 	}
 	
 	this.minvalue = function( id , value )
  	{
	  	var minvalue = document.getElementById( id ).value;
	  	
	  	if( minvalue < value )
		{
			return false;			
		}
		return true;
 	}
  
  	this.isnull = function( id , value )
  	{
	  	var ISNULL = document.getElementById( id ).value.replace(/^\s*|\s*$/g , "");
	  	if( ISNULL == "" && value == false )
		{
			return false;
		}
		return true;
  	}
  
  	this.upperCase = function( id )
  	{
  		document.getElementById( id ).value = document.getElementById( id ).value.toUpperCase();
  		return true;
  	}
  
  	this.type = function( id , value )
  	{
  		var TYPE = document.getElementById( id ).value;
  		TYPE = TYPE.replace( /[\.\-\/]/g , "" );
  		if (value == 'float')
  		{
  			var typeFloat = TYPE.replace( /\,/g , "." );
  			if (isNaN(typeFloat) || parseFloat(typeFloat) <=0){
  				return false;
  			}
  		}
  		else if( ( isNaN( TYPE ) && value == "int" ) || parseInt( TYPE , 10 ) <= 0 )
			{
				return false;
			}
			return true;
  	}
  
  	this.nomeCompleto = function( id )
  	{
		var NOME = document.getElementById( id ).value;
		var exp = /^(.){1,}\ (.){1,}/;

		if( ( NOME != '' ) && ( !( exp.test( NOME ) ) ) ) 
			return false;
		
		return true;
	}
	
	this.nome = function( id )
  	{

  		var NOME = document.getElementById( id ).value;

  		if (NOME.length > 0 && NOME.length <= 2) return false;
		
		return true;
	}
	
  	//nao permite digitar apenas numeros
  	this.bairro = function ( id )
  	{
  		var exp = /\d{1,}/;
		var BAIRRO = document.getElementById( id ).value;
		
		if( BAIRRO == '' ) return true; 

		if( BAIRRO.length < 2 || exp.test( BAIRRO ) )
			return false;

		return true;
  	}	
  
  	this.descricao = function( id )
  	{
  		var DESC = document.getElementById( id ).value;
  		
		if (DESC.length > 0 && DESC.length <= 2) return false;

		return true;
  	}
  	
  	this.campoLivre = function( id )
  	{
  		return true;
  	}
  
  	this.integer = function( id )
  	{
		var INTEGER = document.getElementById( id ).value;
		
		if( INTEGER == '' ) return true;
		
		var regex = INTEGER.match(/^\d.*$/);
		if ( regex == null ) return false;

		if( INTEGER <= 0 )
			return false;

		return true;
  	}
  
  	this.cpf = function( id )
  	{
		var CPF = document.getElementById( id ).value;
		if( CPF == "" ) return true;
		var sum = 0;
		
		CPF = CPF.replace(".","").replace(".","").replace("-","");
	
		if (CPF.length != 11 || CPF == "00000000000" || CPF == "11111111111" || CPF == "22222222222" ||	CPF == "33333333333" || CPF == "44444444444" || CPF == "55555555555" || CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" || CPF == "99999999999") error = false;
		for (y=0; y < 9; y ++)
			sum += parseInt(CPF.charAt(y)) * (10 - y);
		remain = 11 - (sum % 11);
		if (remain == 10 || remain == 11)remain = 0;
		if (remain != parseInt(CPF.charAt(9))) return false; sum = 0;
		for (y = 0; y < 10; y ++)
			sum += parseInt(CPF.charAt(y)) * (11 - y);
		remain = 11 - (sum % 11);
		if (remain == 10 || remain == 11) remain = 0;
		if (remain != parseInt(CPF.charAt(10))) return false;
	
		return true;
  	}
  	
  	this.cnpj = function( id )
  	{
  		var CNPJ = document.getElementById( id ).value;
  		if( CNPJ == "" ) return true;
  		
		CNPJ = CNPJ.replace(".","").replace(".","").replace("/","").replace("-","");
		
		if ( CNPJ == "00000000000000" )
		{
			return false;
		}
		else
		{
			var n = [];
			n.push( 0 );
			
			for( var i = 0; i < CNPJ.length; i++  )
				n.push( parseInt( CNPJ.substr( i , 1 ) ) );
				
			var soma = ( n[1] * 5 ) + ( n[2] * 4 ) + ( n[3] * 3 ) + ( n[4] * 2 ) + ( n[5] * 9 ) + ( n[6] * 8 ) + ( n[7] * 7 ) +	( n[8] * 6 ) + ( n[9] * 5 ) + ( n[10] * 4 ) + ( n[11] * 3 ) + ( n[12] * 2 );
			
			soma = soma - ( 11 * ( parseInt( soma / 11 ) ) );
			
			if( soma == 0 || soma == 1 )
				resultado1 = 0;
			else
				resultado1 = 11 - soma;
			
			if( resultado1 == n[13] )
			{
				soma = n[1] * 6 + n[2] * 5 + n[3] * 4 + n[4] * 3 + n[5] * 2 + n[6] * 9 + n[7] * 8 + n[8] * 7 + n[9] * 6 + n[10] * 5 + n[11] * 4 + n[12] * 3 + n[13] * 2;
				soma = soma - ( 11 * ( parseInt( soma/11 ) ) );
				
				if ( soma == 0 || soma == 1 )
					resultado2 = 0;
				else
					resultado2 = 11 - soma;
				
				if( resultado2 == n[14] )
					return true;
				else
					return false;
			}
			else
				return false;
		}
  	}
  
  	this.email = function( id )
  	{
	  	var EMAIL = document.getElementById( id ).value;
		
		if( EMAIL == "" ) return true;
			
		prim = EMAIL.indexOf("@")
		if(prim < 2) return false;

		if(EMAIL.indexOf("@",prim + 1) != -1) return false;

		if(EMAIL.lastIndexOf(".") == EMAIL.length-1) return false;

		if(EMAIL.lastIndexOf(".") < prim) return false;

		if(EMAIL.indexOf(".") < 1) return false;

		if(EMAIL.indexOf(" ") != -1) return false;

		if(EMAIL.indexOf(".@") > 0) return false;

		if(EMAIL.indexOf("@.") > 0) return false;

		if(EMAIL.indexOf("..") > 0) return false;

		var regex = EMAIL.match(/(\w+)@(.+)\.(\w+)$/);
		if ( regex == null ) return false;
		
		return true;
  	}
  
  	this.data = function( id )
  	{
	  	var DATE = document.getElementById( id ).value;
	  	
	  	if( DATE == "" ) return true;
	  	
	  	var regex = DATE.match( /^(([0-2]\d|[3][0-1])\/([0]\d|[1][0-2])\/[1-2][0-9]\d{2})$/ );
		if ( regex == null ) return false;
			
		var arrDataValidate = DATE.split("-");
		if( arrDataValidate.length > 2 ) return true;
		
		var arrData = DATE.split("/");
		
		if( arrData[2] < 1800 )
			return false	
		if( arrData[1] == 0 )
			return false
		if( arrData[1] == 2 && arrData[0] > 29 )
			return false
		if( arrData[1] == 2 && arrData[0] == 29 && ( arrData[2] % 4 ) )
			return false
		if ( ( arrData[1] == 2 || arrData[1] == 4 || arrData[1] == 6 || arrData[1] == 9 || arrData[1] == 11 )  && ( arrData[0] == 31 ) ) 
        	return false
	
		return true;
  	}
  
  	this.telefone = function( id )
  	{
  		var FONE = document.getElementById( id ).value;
	  	if( FONE == "" ) return true;
	  	var regex = FONE.match(/^\d{4}\-\d{4}$/);
	  	re = /^[^01].*/;	  	
	  	if ( regex == null ) return false;
		
		if ( re.exec(FONE) == null ) return false;
		if (FONE == '1111-1111') return false;
		if (FONE == '2222-2222') return false;
		if (FONE == '3333-3333') return false;
		if (FONE == '4444-4444') return false;
		if (FONE == '5555-5555') return false;
		if (FONE == '6666-6666') return false;
		if (FONE == '7777-7777') return false;
		if (FONE == '8888-8888') return false;
		if (FONE == '9999-9999') return false;
		if (FONE == '0000-0000') return false;
	  	
		return true;
  	}
  
  	this.cep = function( id )
 	{
	  	var CEP = document.getElementById( id ).value;
	  	if( CEP == "" ) return true;
	  	CEP = CEP.replace("-","");
	  	
	  	var regex = CEP.match(/^\d{5}\d{3}$/);
		if ( regex == null ) return false;
		
		return true;
  	}
  
  	this.dinheiro = function( id )
  	{
	  	var DINHEIRO = document.getElementById( id ).value;
	  	var sValue = DINHEIRO.replace( "." , "" ).replace( "." , "" ).replace( "." , "" ).replace( "," , "." );
	  	
	  	if( isNaN( parseFloat( sValue ) ) && parseFloat( sValue ) <= 0 )
	  		return false
	  	
	  	return true;
  	}
  
  	this.ddd = function( id )
  	{
		var DDD = document.getElementById(id).value;
		
		if( DDD == "" ) return true;
		
		if ( DDD.length < 2 )
			return false;
			
		if( parseInt( DDD ) < 11 )
			return false;
			
		return true;
  	}
  	
  	this.url =  function( id )
  	{
  		var URL = new String( document.getElementById(id).value );
  		
  		var aval = /^[http://].*/;
  		
  		if( URL == "" || URL == "HTTP://" ) return true;
  		
  		if (!aval.exec(URL)) return false;
  		
  		if (URL.indexOf('.') == -1) return false;
  		
  		return true;
  	}
  
  	this.dateDependency = function( id  , value )
  	{

		if(  document.getElementById( id ).value && document.getElementById( value ).value )
		{
			var arrInicial = document.getElementById( id ).value.split("/");
			var arrFinal   = document.getElementById( value ).value.split("/");
		
			var dtinicial = parseInt( arrInicial[2]  + "" + arrInicial[1] + "" + arrInicial[0] ); 
			var dtfinal   = parseInt( arrFinal[2] + "" + arrFinal[1] + "" + arrFinal[0] );
			
			if( dtfinal < dtinicial )
			{
				return false
			}
		}
		return true;
  	}
  	

}

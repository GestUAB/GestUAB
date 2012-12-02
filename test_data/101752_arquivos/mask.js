/**
 * Classe para mascara dinamica de formularios
 * 
 * @author Abdala Cerqueira <abdala.cerqueira@gmail.com>
 * @author Jose Roberto de Oliveira <bertono@gmail.com>
 * 
 */
var Mask = function() {
	var strMask;
	var target;
	var randomId;
	var maxLength;
	var e;

	var is_ie = (/msie/i.test(navigator.userAgent) && !/opera/i
			.test(navigator.userAgent));

	/**
	 * Recupera o evento independente do browser
	 * 
	 * @param event
	 *            e
	 */
	this.getEvent = function(e) {
		
		e = is_ie ? window.event : e;
		
		(e = e || event).preventDefault || (e.preventDefault = function() {
			e.returnValue = false;
		});
		e.stopPropagation || (e.stopPropagation = function() {
			e.cancelBubble = true;
		});
		e.target || (e.target = e.srcElement || null);
		e.key = (e.which + 1 || e.keyCode + 1) - 1 || 0;
		return e;
	}

	/**
	 * Setar os atributos mascara e campo que esta sendo aplicada a mascara
	 * Adiciona a função apply no evento de keypress
	 * 
	 * @param mixed
	 *            mixTargetId Id do campo que vai receber a mascara
	 * @param object
	 *            objMask objeto que será usado para aplicar a mascara
	 * @param string
	 *            strMaskSet mascara
	 * @param mixed
	 *            mixId ID de outro campo que queira ter interacao
	 * 
	 */
	this.init = function(mixTargetId, objMask, strMaskSet, mixId) {
		if ( mixTargetId )
		strMask = strMaskSet;
		target = $(mixTargetId);
		randomId = mixId;
		
		if (randomId)
			maxLength = $(randomId).value;

		if (strMaskSet == "DC") {
			target["onkeyup"] = function(event) {
				objMask.reduce();
			};
		} else {
			target["onkeypress"] = function(event) {
				e = objMask.getEvent(event);

				if( e.target.readOnly == true ) return false;
				
				if (strMaskSet == "INT") {
					objMask.integer();
				} else if (strMaskSet == "DS") {
					objMask.description();
				} else if (strMaskSet == "MONEY") {
					objMask.money();
				} else {
					objMask.apply();
				}
			};
		}
	}

	/**
	 * Aplica a mascara setada no initialize
	 */
	this.apply = function() {
		var i;

		var strValue = target.value;

		target.maxLength = strMask.length;

		if (e.key == 8 || e.key == 9)
			return;

		if (!((e.key > 47 && e.key < 58) || e.key == 8 || e.key == 127
				|| e.key == 0 || e.key == 9 || e.key == 13)) {
			e.key > 30 && e.preventDefault();
			return false;
		}

		if (strValue.length == 0) {
			if (strMask.charAt(0) != 0) {
				strValue += strMask.charAt(0) + strValue;
			}
		}

		for (i = 0; i < strValue.length; i++) {
			if (strMask.charAt(i + 1) != 0) {
				if ((strValue.length - 1) == i) {
					target.value += strMask.charAt(i + 1);
				}
			}
		}
	}

	/**
	 * Aplica marcara de somente numeros
	 */
	this.integer = function() {
		if (!((e.key > 47 && e.key < 58) || e.key == 8 || e.key == 127
				|| e.key == 0 || e.key == 9 || e.key == 13)) {
			e.key > 30 && e.preventDefault();
		}
	}

	/**
	 * Reduz o numero que esta no decrementador do textarea
	 */
	this.reduce = function() {
		var reduzido = parseInt(maxLength) - target.value.length;

		if (reduzido > -1) {
			$(randomId).value = reduzido;
		} else {
			target.value = target.value.substring(0, maxLength);
		}
		return;
	}

	/**
	 * Aplica a mascara a um nome
	 */
	this.description = function() {
		// Permiti espaço, enter e backspace
		if ((e.key == 8) || (e.key == 32) || (e.key == 13))
			return;

		// Permiti os acentos
		if (e.key > 193 && e.key < 251)
			return;

		// Permiti todas as letras e bloqueia alguns caracteres
		if ((e.key > 64 && e.key < 123) && !(e.key > 90 && e.key < 97)) {
			;
		} else {
			e.key > 30 && e.preventDefault();
		}
		return;
	}

	/**
	 * Aplica a mascara de dinheiro
	 */
	this.money = function() {
		var o = target;
		if (!o.readonly) {
			if (e.key > 47 && e.key < 58) {
				var n = 2;
				var s = (target.value.replace(/^0+/g, "") + String
						.fromCharCode(e.key)).replace(/\D/g, "");
				var l = s.length;

				if (o.maxLength + 1 && l >= o.maxLength)
					return false;
				if (o.maxLength == o.value.length)
					return false;

				l <= (n = 2) && (s = new Array(n - l + 2).join("0") + s);
				for ( var i = (l = (s = s.split("")).length) - n; (i -= 3) > 0; s[i - 1] += ".")
					;
				n && n < l && (s[l - ++n] += ",");
				o.value = s.join("");
			}
			e.key > 30 && e.preventDefault();
		}
	}
}


var Transfer = {

	dirSimples: function( selectEsq, selectDir )
	{
		for( i = 0; i < selectEsq.length; i++ ){
			if( selectEsq.options[i].selected == true ){
				//selectDir.options[selectDir.length] = new Option( selectEsq.options[i].text, selectEsq.options[i].value );
				newOp = new Option();
				newOp.text = selectEsq.options[i].text;
				newOp.value = selectEsq.options[i].value;
				newOp.onmouseover = selectEsq.options[i].onmouseover;
				newOp.onmouseout = selectEsq.options[i].onmouseout;
				selectDir.options[selectDir.length] = newOp;
				selectEsq.options[i] = null;
				i--;
			}
		}
	} ,

	esqSimples: function( selectEsq, selectDir )
	{
		for( i = 0; i < selectDir.length; i++ ){
			if( selectDir.options[i].selected == true ){
				//selectEsq.options[selectEsq.length] = new Option( selectDir.options[i].text, selectDir.options[i].value);
				newOp = new Option();
				newOp.text = selectDir.options[i].text;
				newOp.value = selectDir.options[i].value;
				newOp.onmouseover = selectDir.options[i].onmouseover;
				newOp.onmouseout = selectDir.options[i].onmouseout;
				selectEsq.options[selectEsq.length] = newOp;
				selectDir.options[i] = null;
				i--;
			}
		}
	} ,

	dirDupla: function( selectEsq, selectDir )
	{
		while( selectEsq.options[0] != null ){
			//selectDir.options[selectDir.length] = new Option( selectEsq.options[0].text, selectEsq.options[0].value );
			newOp = new Option();
			newOp.text = selectEsq.options[0].text;
			newOp.value = selectEsq.options[0].value;
			newOp.onmouseover = selectEsq.options[0].onmouseover;
			newOp.onmouseout = selectEsq.options[0].onmouseout;
			selectDir.options[selectDir.length] = newOp;
			selectEsq.options[0] = null;
		}
	} ,

	esqDupla: function( selectEsq, selectDir )
	{
		while( selectDir.options[0] != null ){
			//selectEsq.options[selectEsq.length] = new Option( selectDir.options[0].text, selectDir.options[0].value );
			newOp = new Option();
			newOp.text = selectDir.options[0].text;
			newOp.value = selectDir.options[0].value;
			newOp.onmouseover = selectDir.options[0].onmouseover;
			newOp.onmouseout = selectDir.options[0].onmouseout;
			selectEsq.options[selectEsq.length] = newOp;
			selectDir.options[0] = null;
		}
	} ,

	markTransfer: function( objTransfer )
	{
		for( var i = 0 ; i < objTransfer.options.length ; ++i )
		{
			objTransfer.options[ i ].selected = true;
		}
	} , 
	
	cleanTransfer: function( selectEsq , selectDir )
	{
		$(selectEsq).options.length = 0;
		$(selectDir).options.length = 0;
		
		
	}
}

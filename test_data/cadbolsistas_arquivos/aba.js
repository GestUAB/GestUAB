
Aba = Class.create();

Aba.prototype = {
	
	elements: [], 
	
	initialize: function( aElement )
	{
		elements = aElement;
	},
	
	select: function( id )
	{
		for( var i = 0; i < elements.length; i++ )
		{
			$( elements[i] ).className = "inativo";
			$( "li_" + elements[i] ).className = "inativo";
			
			if( elements[i] == id )
			{
				$( elements[i] ).className = "ativo";
				$( "li_" + elements[i] ).className = "ativo";
			}
		}
	}
};
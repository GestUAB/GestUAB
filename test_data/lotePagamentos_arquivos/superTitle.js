	/**
	 *
	 @date 2006-01-17
	 */
	FADE_IN_FINISHED = "fade in finished";

	/**
	 *
	 @date 2006-01-17
	 */
	FADE_OUT_FINISHED = "fade out finished";

	/**
	 *
	 @date 2006-01-17
	 */
	mouseX = 0;

	/**
	 *
	 @date 2006-01-17
	 */
	mouseY = 0;

	/**
	 *
	 @date 2006-01-17
	 */
	globalBoolOnFade = false;

	/**
	 *
	 @date 2006-01-17
	 */
	globalIntFadeDirection = 0;

	/**
	 *
	 @date 2006-01-17
	 */
	globalIntFadeSpeed		= 10;

	/**
	 *
	 @date 2006-01-17
	 */
	globalIntFadeValue		= 5;

	/**
	 *
	 @date 2006-01-17
	 */
	globalIntFadeCount		= 0;

	/**
	 *
	 @date 2006-01-17
	 */
	globalIntFadeMax		= 100;

	/**
	 *
	 @date 2006-01-17
	 */
	globalIntFadeMin		= 0;

	/**
	 *
	 @date 2006-01-17
	 */
	globalStrIdTitleBox = "TitleBoxId";

	/**
	 *
	 @date 2006-01-17
	 */
	globalObjSelected	= null;

	/**
	 *
	 @date 2006-01-19
	 */
	globalIntMouseXAdd	= 10;

	/**
	 *
	 @date 2006-01-19
	 */
	globalIntMouseYAdd	= 10;

	/**
	 *
	 @date 2006-01-17
	 */
	function singletonFade()
	{
		if ( globalBoolOnFade == true )
		{
			return;
		}
		globalBoolOnFade = true;
		loopFade();
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function loopFade()
	{

		var objDivTitleBox = document.getElementById( globalStrIdTitleBox );

		globalIntFadeCount += globalIntFadeValue ;

		if ( globalIntFadeCount > globalIntFadeMax )
		{
			globalIntFadeCount = globalIntFadeMax;
			globalBoolOnFade = false;
			return FADE_IN_FINISHED ;
		}
		if ( globalIntFadeCount < globalIntFadeMin )
		{
			globalIntFadeCount = globalIntFadeMin;
			globalBoolOnFade = false;
			RemoveSuperTitle();
			return FADE_OUT_FINISHED;
		}

		Alpha( objDivTitleBox , globalIntFadeCount );

		setTimeout( "loopFade()" , globalIntFadeSpeed );

		return false;
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function FadeIn()
	{
		if ( !IE )
		{
			globalIntFadeValue = +5;
		}
		else
		{
			globalIntFadeValue = +30;
		}
		singletonFade();
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function FadeOut()
	{
		if ( !IE )
		{
			globalIntFadeValue = -5;
		}
		else
		{
			globalIntFadeValue = -30;
		}
		singletonFade();
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function SuperTitleOff(me)
	{

		var objDivTitleBox = document.getElementById( globalStrIdTitleBox );

		if (objDivTitleBox == false)
		{
			// there is not a super title to be removed //
			return false;
		}

		FadeOut();
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function RemoveSuperTitle()
	{

		var objDivTitleBox = document.getElementById( globalStrIdTitleBox );

		if ( objDivTitleBox )
		{
		    objParent = objDivTitleBox.parentNode;
			objDivTitleBox.style.display = 'none';
			objParent.removeChild( objDivTitleBox );
		}
	}

	function CreateSuperTitle( strValue )
	{
		if ( strValue == "" )
		{
			RemoveSuperTitle();
			return;
		}

		var objChildBox						= document.createElement( 'div' );
		objChildBox.className 				=	"TitleClass";
		objChildBox.innerHTML 				=	strValue;
		objChildBox.style.backgroundColor	=	"#FFFFFF";

	    objDivTitleBox 						=	document.createElement('div');

		objDivTitleBox.appendChild( objChildBox );

		objDivTitleBox.id					=	globalStrIdTitleBox;
		objDivTitleBox.style.position		=	"absolute";
		objDivTitleBox.style.top			=	( mouseY + globalIntMouseYAdd ) + "px";
		objDivTitleBox.style.left			=	( mouseX + globalIntMouseXAdd ) + "px";
		objDivTitleBox.style.borderStyle	=	"solid";
		objDivTitleBox.style.borderWidth	=	"1px";
		objDivTitleBox.style.padding		=	"0px 2px 1px 2px";
		objDivTitleBox.style.zIndex			=	"200";
		objDivTitleBox.style.fontSize		=	"small";
		objDivTitleBox.onmouseout			=	SuperTitleOff;

		document.body.appendChild(objDivTitleBox);

		return objDivTitleBox;
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function SuperTitleOn( objSelected , strValue )
	{
		objDivTitleBox = document.getElementById( globalStrIdTitleBox );

		if ( objDivTitleBox && ( strValue != "" ) )
	    {
	    	if ( objSelected != globalObjSelected )
			{
				if ( globalObjSelected )
				{
					if (objSelected.onmouseover != globalObjSelected.onmouseover )

					var arrChilds = objSelected.getElementsByTagName("*");
					var intPos	= array_search( globalObjSelected, arrChilds );
					if ( intPos != -1 )
					{
						// o novo elemento de title ? pai do elemento de title anterior //
						strValue = objDivTitleBox.getElementsByTagName("div")[0].innerHTML;
					}
				}
				globalObjSelected = objSelected;

				objDivTitleBox.style.top 		= ( mouseY + globalIntMouseYAdd ) + "px";
				objDivTitleBox.style.left 		= ( mouseX + globalIntMouseXAdd ) + "px";
				objDivTitleBox.style.display 	= "block";
				objDivTitleBox.getElementsByTagName("div")[0].innerHTML = strValue;
			}
		}
		else
		{
			CreateSuperTitle( strValue );
		}
		FadeIn();
		return false;
	}

	/**
	 *
	 @date 2006-01-17
	 */
	function Alpha( objField , Percents)
	    {
	    if ( objField )
	    	{
	    		try
	    		{
			   	objField.style.filter		= "alpha(opacity="+Percents+")";
				objField.style.MozOpacity	= ""+Percents/100+"";
				}
				catch( e )
				{

				}
			}
		}

	/**
	 *
	 @date 2006-01-17
	 */
	function ShowUp(Percents,Passo)
	    {
		if (!(self.TitleBox))
			return false;
		Percents+=Passo;
		if (Percents > 100)
		    {
	        Making = false;
		    return false;
		    }
		if (Percents < 0)
		    {
	        Alpha(TitleBox,0);
	        Making = false;
		    return false;
		    }
		Alpha(TitleBox,Percents);
		setTimeout("ShowUp("+Percents+","+Passo+")",40);
	    }




// $Id: $
/**
 * @author Joshua Chan
 * @link http://nihilex.com/droplist-filter
 */
/**
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU General Public License as published by
 *    the Free Software Foundation, either version 3 of the License, or
 *    (at your option) any later version.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *    GNU General Public License for more details.
 *
 *    You should have received a copy of the GNU General Public License
 *    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

(function($){
  /**
   * This method adds a search filter widget to one or more SELECT lists
   * selected by jQuery.
   * @param minSize integer
   *    (Optional) A list must have at least this many items or else no
   *    widget will be added to it.
   * @param maxInstances integer
   *    (Optiona) No more widgets will be added if this number of widgets
   *    already exist on the page.
   */
  $.fn.droplistFilter = function(minSize, maxInstances) {
    return this.each(function(){
      if (this.tagName == 'SELECT') {
        // Verifica se a combo já foi trabalhada
    	var idFind = this.id + 'dropFind';
    	if( $('#'+idFind).attr('id') )
    	{
          return;
    	}
    	
        // Check for minimum list size
        if (minSize && this.options.length < minSize) {
          return;
        }
        // Check for maximum widget instances
        if (!$.fn.droplistFilter.prototype.instances) {
          $.fn.droplistFilter.prototype.instances = 0;
        }
        if (maxInstances && ($.fn.droplistFilter.prototype.instances >= maxInstances)) {
          return;
        }
        $.fn.droplistFilter.prototype.instances += 1;
      
        // Create a browserID string to enable CSS fixes for browser suckage.
        // No JavaScript logic uses this value. It is only applied as a CSS
        // class name for the <NOBR> element.
        if (!$.fn.droplistFilter.prototype.browserID) {
          if (navigator.userAgent.match('Safari')) {
              $.fn.droplistFilter.prototype.browserID = 'Safari';
          } else if (navigator.userAgent.match('MSIE')) {
              $.fn.droplistFilter.prototype.browserID = 'MSIE';
          } else if (navigator.userAgent.match('Firefox/3')) {
              $.fn.droplistFilter.prototype.browserID = 'Firefox3';
          } else if (navigator.userAgent.match('Firefox/2')) {
              $.fn.droplistFilter.prototype.browserID = 'Firefox2';
          } else if (navigator.userAgent.match('Gecko')) {
              $.fn.droplistFilter.prototype.browserID = 'Gecko';
          } else if (navigator.userAgent.match('Opera')) {
              $.fn.droplistFilter.prototype.browserID = 'Opera';
          } else {
              $.fn.droplistFilter.prototype.browserID = 'Other';
          }
        }
        
        // Create semi-private namespace within the node
        this._df = new Object();
        var i; var j; var k;
        
        // Save original list items
        this._df.Options = new Array();
        var hasDash = 0;
        for (i=0; i<this.options.length; i++) {
          this._df.Options[i] = {
            "text": this.options[i].text,
            "value": this.options[i].value,
            "title": this.options[i].title,
            "style": this.options[i].style
          };
          if (this.options[i].text == '-') {
            hasDash = 1;
          }
        }
        this._df.Selected = this.selectedIndex;
        if (!hasDash) this._df.Selected += 1;
        this._df.Width = this.offsetWidth;
        
        //// Create the HTML elements
        // Create the components
        this._df.Text = $('<input>').addClass('dfText');
        this._df.OK = $('<div>').addClass('dfOK');
        this._df.Reset = $('<div>').addClass('dfReset');
        this._df.Activate = $('<div>').addClass('dfActivate').append($('<div>'));
        this._df.Container = $('<div>').addClass('dfContainer').css('display', 'none');
        
        this._df.Activate.attr( 'id', idFind );
        
        // Wrap the list with a NOBR tag
        $(this).wrap( $('<nobr></nobr>').addClass($.fn.droplistFilter.prototype.browserID) );

        // Establish the width of the search box
        var boxWidth = 238;
        var thisWidth = this._df.Width + 50;
        if (thisWidth > boxWidth) {
          boxWidth = thisWidth;
        }

        // Create the hidden search box
        $(this).after((this._df.Container)
                 .width(boxWidth)
                 .append(this._df.OK)
                 .append(this._df.Reset)
                 // 3-layered text box
                 .append($('<div>').addClass('dfLeft')
                                   .append($('<div>').addClass('dfRight')
                                                     .append(this._df.Text)
                                   )
                 )
        );

        // Insert the activation button
        $(this).after(this._df.Activate);
        
        // Create alias of "this" for use in event functions
        var thisDF = this;
        

        /**
         * Fetch a copy of a given list option from the original list.
         * @param i integer
         *    The index of the option to fetch.
         * @return Option
         */
        this._df.GetOption = function(i) {
          var opt = new Option();
          for (j in thisDF._df.Options[i]) {
            try {
              if (j == 'style') {
                // Deep copy CSS attributes
                for (k in thisDF._df.Options[i].style) {
                  opt.style[k] = thisDF._df.Options[i].style[k];
                }
              } else {
                // Copy other normal attributes
                opt[j] = thisDF._df.Options[i][j];
              }
            } catch(e) { }
          }
          return opt;
        };
        
        /**
         * Repopulate the list options based on the filter string.
         * @param mixed
         *    Either -1 to reset the list, or the search string.
         */
        this._df.DoFilter = function(filterString){
          // Convert search string to lowercase if it is in fact a string
          if (filterString.toLowerCase) {
            filterString = filterString.toLowerCase();
          }
          if (filterString == -1) {
            var doReset = true;
          } else {
            var doReset = false;
          }
          // Clear the list
          thisDF.length = 0;
          // Add list header
          if (doReset) {
            //thisDF.options[0] = new Option('-', '');
          } else {
            thisDF.options[0] = new Option('-- ' + filterString + ' --', '');
          }
          // Add matching items
          for (i=0; i<thisDF._df.Options.length; i++) {
            if (doReset || thisDF._df.Options[i].text.toLowerCase().match(filterString)) {
              if (thisDF._df.Options[i].value != '-') {
                thisDF.options[thisDF.length] = thisDF._df.GetOption(i);
              }
            }
          }
          // Maintain the original width
          $(thisDF).width(thisDF._df.Width);
          // Restore the original selection on reset
          if (doReset) {
            thisDF.selectedIndex = thisDF._df.Selected;
          }
        };


        //// Event handling
        // Helper toggle function
        this._df.Toggle = function(){
          thisDF._df.Container.toggle();
          thisDF._df.Activate.toggle();
          $(thisDF).toggle();
        };
        // Activation button
        this._df.Activate.find('div').click(function(){
          thisDF._df.Toggle();
          thisDF._df.Text.select();
          thisDF._df.Text.focus();
        });
        // Reset button
        this._df.Reset.click(function(){
          thisDF._df.DoFilter(-1);
          thisDF._df.Toggle();
        });
        // Search button
        this._df.OK.click(function(){
          thisDF._df.DoFilter(thisDF._df.Text.val());
          thisDF._df.Toggle();
        });
        // Keypress
        this._df.Text.keypress(function(e){
          // Cross-browser initialization
          if (!e) var e = window.event;
          if (e.keyCode) keynum = e.keyCode;
          else if (e.which) keynum = e.which;
          // 'Enter' key
          if (keynum == 13 || keynum == 10 || keynum == 3) {
            thisDF._df.OK.click();
            // Prevent the key from doing anything else
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
            return false;
          } 
          // 'Escape' key
          else if (keynum == 27) {
            thisDF._df.Reset.click();
            // Prevent the key from doing anything else
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
            return false;
          }
          return true;
        });
        // Blur
        /*
        this._df.Text.blur(function(){
            thisDF._df.OK.click();
          });
       */

      }
    });
  };
})(jQuery);
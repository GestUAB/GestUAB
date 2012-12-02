function confirma(pergunta,url) {
   var answer = confirm(pergunta)
   if (answer){
      window.location = url;
   }
   else{
      return false;
   }
}
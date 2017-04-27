function get_base_path(url){
        var urls = url.split("/");
        var base = "";
        for(i = 0; i < urls.length; i++){
               if(urls[i] == "index.php"){
                break;
               }else{
                    base += urls[i] + "/";
               }
        }
        return base;
    }
	
function hola(){
	alert("hola");
}	
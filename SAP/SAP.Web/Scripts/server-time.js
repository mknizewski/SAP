$(document).ready(function () {
   
    if (!!window.EventSource) {
        var source = new EventSource('/api/servertime');

        source.onmessage = function (event) {
            document.getElementById("time").innerHTML = event.data;
        };

        source.addEventListener('open', function (e) {
            console.log("Zaczynam zliczać czas!");
        }, false);

        source.addEventListener('error', function (e) {
            if (e.readyState == EventSource.CLOSED) {
                console.log("error!");
            }
            else {
                console.log("error!");
            }
        }, false);

    } else {
        // not supported!               
    }

});
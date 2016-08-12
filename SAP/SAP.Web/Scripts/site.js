var loadingAlertIsDisplayed = false;

function displayLoadingAlert(flag) {
    var alert = document.getElementById("alertScreen");

    if (flag)
        alert.style.display = "inline";
    else
        alert.style.display = "none";
}

$(document).ready(function () {

    if (loadingAlertIsDisplayed)
        displayLoadingAlert(false);

});
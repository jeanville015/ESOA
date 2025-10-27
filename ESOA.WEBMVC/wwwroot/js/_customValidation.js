
var isFormValidated;


$(document).ready(function () {
    isFormValidated = false;
});

function validateForm(_formInputs) {
    isFormValidated = true 
    var indexer = 1;
    do {

        if (($('.required-' + indexer.toString()).val() != null)) {
            if (!($('.required-' + indexer.toString()).val().length > 0)) {
                //alert('val < 0');
                isFormValidated = false;
            }
        } else {
            isFormValidated = false;
        }
       
        indexer++;
    }
    while (indexer <= _formInputs);
}


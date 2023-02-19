// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function isNumber(e) {
    console.log(e)
    const charCode = e.which || e.charCode;
    const separator = 44;
    const alreadyHaveSeparator = e.target.value.toString().indexOf(String.fromCharCode(separator)) != -1;
    if (charCode == separator && alreadyHaveSeparator) {
        e.preventDefault();
    }


    if (charCode != separator && charCode > 31
        && (charCode < 48 || charCode > 57)) {
        e.preventDefault();
    }
}

function addApertureInput() {
    var container = document.querySelector("#ElementsContainer");
    
    var widthInput = '<input type="text" id="widthInput" name="widthInput1" value="0 мм" />'
    var heightInput = '<input type="text" id="heightInput" name="heightInput1" value="0 мм" />'

    container.appendChild(createInput())
    container.appendChild(heightInput)
}

function createInput(type, text, placeholder) {
    var FN = document.createElement("input");
    FN.setAttribute("type", type);
    FN.setAttribute("name", text);
    FN.setAttribute("id", text);
    FN.setAttribute("placeholder", placeholder);
}

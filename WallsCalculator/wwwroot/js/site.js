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
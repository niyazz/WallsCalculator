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
    const container = document.querySelector("#apertures-container");
    const className = "aperture"
    const apertures = document.getElementsByClassName(className);
    const lastApertureIdx = apertures.length;
    const clones = [...apertures[lastApertureIdx - 1].children].map(x => x.cloneNode(true))
    const attributesToRewrite = ["for", "name", "id", "data-valmsg-for"]
    var result = clones.map(formGroup => {
        [...formGroup.children].map(child => {
            for (const atrw of attributesToRewrite) 
                if (child.getAttribute(atrw)) 
                    child.setAttribute(atrw, child.getAttribute(atrw).replace(/\d/, lastApertureIdx))

            return child;
        })

 
        return formGroup;
    })

        var aperture = document.createElement("div")
        aperture.className = className

        result.forEach(x => aperture.appendChild(x))
        container.appendChild(aperture)
}

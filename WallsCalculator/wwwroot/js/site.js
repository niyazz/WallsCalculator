// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// console.log('today we'll talk about dot net framework')
// Write your JavaScript code.


function isNumber(e) {
    const charCode = e.which || e.charCode;
    const separator = 44;
    const alreadyHaveSeparator = e.target.value.toString().indexOf(String.fromCharCode(separator)) !== -1;
    if (charCode === separator && alreadyHaveSeparator) {
        e.preventDefault();
    }
    if (charCode !== separator && charCode > 31
        && (charCode < 48 || charCode > 57)) {
        e.preventDefault();
    }
}

function addApertureInput() {
    const apertures_container = document.querySelector("#apertures-container");
    const apertureClassName = "aperture"
    const apertures = document.getElementsByClassName(apertureClassName);
    const lastApertureIdx = apertures.length;
    const clones = [...apertures[lastApertureIdx - 1].children].map(x => x.cloneNode(true))
    const attributesToRewrite = ["for", "name", "id", "data-valmsg-for"]
    const whInputs = clones.map(formGroup => {
        [...formGroup.children].map(child => {
            for (const atrw of attributesToRewrite) 
                if (child.getAttribute(atrw)) 
                    child.setAttribute(atrw, child.getAttribute(atrw).replace(/\d/, lastApertureIdx.toString()))

            return child;
        })

 
        return formGroup;
    })

        const apertureDiv = document.createElement("div")
        apertureDiv.className = apertureClassName

        whInputs.forEach(x => apertureDiv.appendChild(x))
        apertures_container.appendChild(apertureDiv)
}

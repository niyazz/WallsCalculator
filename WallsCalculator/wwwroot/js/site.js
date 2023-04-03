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

function addApertureInput(containerName, formGroupClassName) {
    const container = document.querySelector(containerName);
    const elements = document.getElementsByClassName(formGroupClassName);
    const lastApertureIdx = elements.length;
    const clones = [...elements[lastApertureIdx - 1].children].map(x => x.cloneNode(true))
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
        apertureDiv.className = formGroupClassName

        whInputs.forEach(x => apertureDiv.appendChild(x))
        container.appendChild(apertureDiv)
}


// auto-scroll to result block after calculate
$(function () {
    $("html, body").delay(500).animate({ scrollTop: $('#outputResult').offset().top }, 500);
});


//кнопка Подробнее и скрыть
let moreBtns = document.getElementsByClassName("myBtn");
let dots = document.getElementsByClassName("dots");
let moreText = document.getElementsByClassName("more");

for (let i = 0; i < moreBtns.length; i++) {
    moreBtns[i].addEventListener('click', () => {
        if (dots[i].style.display === "none") {
            dots[i].style.display = "inline";
            moreBtns[i].innerHTML = `Подробнее
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-down" viewBox="0 0 16 16">
                <path d="M3.204 5h9.592L8 10.481 3.204 5zm-.753.659 4.796 5.48a1 1 0 0 0 1.506 0l4.796-5.48c.566-.647.106-1.659-.753-1.659H3.204a1 1 0 0 0-.753 1.659z"/>
            </svg>`;
            moreText[i].style.display = "none";
        } else {
            dots[i].style.display = "none";
            moreBtns[i].innerHTML = `Скрыть
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-up" viewBox="0 0 16 16">
              <path d="M3.204 11h9.592L8 5.519 3.204 11zm-.753-.659 4.796-5.48a1 1 0 0 1 1.506 0l4.796 5.48c.566.647.106 1.659-.753 1.659H3.204a1 1 0 0 1-.753-1.659z"/>
            </svg>`;
            moreText[i].style.display = "inline";
        }
    });
}
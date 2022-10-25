let myForm = document.getElementById('main-form');

myForm?.addEventListener('submit', () => {
    let allInputs = myForm.getElementsByTagName('input');

    for (var i = 0; i < allInputs.length; i++) {
        let input = allInputs[i];

        if (input.name && !input.value) {
            input.name = '';
        }
    }
});

let navbar = document.getElementById('login-nav');

const getUserLoginPreviewUrl = `${window.location.origin}/Account/NavigationUserPreview`;

fetch(getUserLoginPreviewUrl)
    .then(function (response) {
        return response.text();
    }).then(function (result) {
        navbar.innerHTML = result;
    });

function expandTextarea() {
    let allTextAreas = document.getElementsByTagName('textarea');

    for (var i = 0; i < allTextAreas?.length; i++) {
        allTextAreas[i]?.addEventListener('keyup', () => {
            this.style.overflow = 'hidden';
            this.style.height = 0;
            this.style.height = this.scrollHeight + 'px';
        }, false);
    }
}

expandTextarea();
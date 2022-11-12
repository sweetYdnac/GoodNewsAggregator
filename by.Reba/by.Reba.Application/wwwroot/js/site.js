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

function resizeTextBox(element) {
    element.style.height = "1px";
    element.style.height = (1 + element.scrollHeight) + "px";
}


let btnActivity = document.getElementById('btn-activity');
let activity = document.getElementById('user-activity');

let history = document.getElementById('user-history');
let btnHistory = document.getElementById('btn-history');

if (btnActivity) {
    btnActivity.onclick = () => {
        btnActivity.classList.add("active");
        btnHistory.classList.remove("active");

        activity.classList.remove("d-none");
        history.classList.add("d-none");
    };
}

if (btnHistory) {
    btnHistory.onclick = () => {
        btnHistory.classList.add("active");
        btnActivity.classList.remove("active");

        history.classList.remove("d-none");
        activity.classList.add("d-none");
    }
}
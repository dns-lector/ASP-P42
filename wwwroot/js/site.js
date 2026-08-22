document.addEventListener('submit', e => {
    const form = e.target;
    if (form.id == 'auth-form') {
        // зупиняємо автоматичне надсилання форми
        e.preventDefault();
        // вилучаємо дані, що передаються
        const formData = new FormData(form);
        const login = formData.get("auth-login");
        const password = formData.get("auth-password");
        // здійснюємо попередню перевірку (на порожність)
        let errorMessage = "";
        if (login.trim().length === 0) {
            errorMessage += "Логін не може бути порожнім.\n";
        }
        if (password.trim().length === 0) {
            errorMessage += "Пароль не може бути порожнім.\n";
        }
        const err = document.getElementById("auth-modal-error");
        if (errorMessage.length > 0) {
            err.innerText = errorMessage;
            err.style.visibility = "visible";
        }
        else {
            err.innerText = "";
            err.style.visibility = "hidden";
        }
        console.log(login, password);
    }
});

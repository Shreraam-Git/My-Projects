//common Error Message
function CommonErrormsg(successorerror, ErrorMsg) {
    if (ErrorMsg != "") {
        Swal.fire({
            icon: successorerror,
            title: ErrorMsg
        })
    }
}

//Checking the Credential to login
function CheckCredential() {
    if (document.getElementById("LoginUserName").value != "" && document.getElementById("LoginPassword").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1LogIn.aspx/CheckCredential",
            data: "{'LoginUserName':'" + document.getElementById("LoginUserName").value + "', 'LoginPassword':'" + document.getElementById("LoginPassword").value + "'}",
            dataType: "json",
            success: function (response) {

                var data = JSON.parse(response.d);

                if (data.Message == "Success") {
                    localStorage.setItem("UserName", data.UserID);
                    localStorage.setItem("OriginalUserName", document.getElementById("LoginUserName").value);

                    setTimeout(function () {
                        document.getElementById("cover-spin").style.display = "none";
                        window.location = "../Formula1/F1Dashboard.aspx";
                    }, 2000);
                }
                else {
                    CommonErrormsg('error', data.Message);
                    document.getElementById("cover-spin").style.display = "none";
                }
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Kindly Enter Your Credentials.');
        return;
    }
}

//Signing up JS
function AddCredentialSignUp() {
    if (document.getElementById("SignUpUserName").value != "" && document.getElementById("SignUpEmailId").value != "" && document.getElementById("SignUpPassword").value != "") {
        if (document.getElementById("password-strength-text").innerText != "Password strength: Strong") {
            CommonErrormsg('error', 'Make the Password Strong.');
            return;
        }

        if (!validateEmail(document.getElementById("SignUpEmailId").value)) {
            CommonErrormsg('error', 'Invalid email address.');
            return;
        }

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1LogIn.aspx/AddCredentialSignUp",
            data: "{'SignUpUserName':'" + document.getElementById("SignUpUserName").value + "', 'SignUpEmailId':'" + document.getElementById("SignUpEmailId").value + "', 'SignUpPassword':'" + document.getElementById("SignUpPassword").value + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "Success") {
                    CommonErrormsg('success', 'Thank you for signing up! Please log in to get started');
                }
                else {
                    CommonErrormsg('error', data.d);
                }

                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Kindly Enter Your Credentials.');
        return;
    }
}

//Change Password js
function ChangeCredentialSignUp() {
    if (document.getElementById("CPUserName").value != "" && document.getElementById("CPPassword").value != "" && document.getElementById("CPNewPassword").value != "") {

        if (document.getElementById("password-strength-text1").innerText != "Password strength: Strong") {
            CommonErrormsg('error', 'Make the Password Strong.');
            return;
        }
        if (document.getElementById("CPPassword").value == document.getElementById("CPNewPassword").value) {
            CommonErrormsg('error', 'Old & New Password is Same.');
            return;
        }

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1LogIn.aspx/ChangeCredentialSignUp",
            data: "{'CPUserName':'" + document.getElementById("CPUserName").value + "', 'CPPassword':'" + document.getElementById("CPPassword").value + "', 'CPNewPassword':'" + document.getElementById("CPNewPassword").value + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "Success") {
                    CommonErrormsg('success', 'Password Updated Successfully.');
                }
                else {
                    CommonErrormsg('error', data.d);
                }

                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Kindly Enter Your Credentials.');
        return;
    }
}

//Forgot Password
function ForgotCredentialSignUp() {
    if (document.getElementById("ForgotUserName").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1LogIn.aspx/ForgotCredentialSignUp",
            data: "{'ForgotUserName':'" + document.getElementById("ForgotUserName").value + "'}",
            dataType: "json",
            success: function (response) {

                var data = JSON.parse(response.d);

                if (data.Message == "DataSent") {
                    CommonErrormsg('success', 'Password Sent Successfully to ' + data.MailValue);
                }
                else {
                    CommonErrormsg('error', data.Message);
                }

                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Kindly Enter Your Credentials.');
        return;
    }
}

//Other Stuffs
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('LoginPassword').setAttribute('autocomplete', 'new-password');
    document.getElementById('SignUpPassword').setAttribute('autocomplete', 'new-password');
});

function HideandShow(Value) {
    if (Value == "Register") {
        document.getElementById('SignUpDiv').style.display = 'block';
        document.getElementById('LoginDiv').style.display = 'none';
    }
    if (Value == "Login") {
        window.location.reload();
    }
    if (Value == "ChangePassword") {
        document.getElementById('ChangePasswordDiv').style.display = 'block';
        document.getElementById('LoginDiv').style.display = 'none';
    }
    if (Value == "ForgotPassword") {
        document.getElementById('ForgotPasswordDiv').style.display = 'block';
        document.getElementById('LoginDiv').style.display = 'none';
    }
}

function ChangeType(e) {
    if (e == "VisibleEye") {
        document.getElementById(e).style.display = "none";
        document.getElementById("NoneVisibleEye").style.display = "block";
        document.getElementById('SignUpPassword').type = "text";
    }
    else if (e == "NoneVisibleEye") {
        document.getElementById(e).style.display = "none";
        document.getElementById("VisibleEye").style.display = "block";
        document.getElementById('SignUpPassword').type = "password";
    }

    if (e == "VisibleEye1") {
        document.getElementById(e).style.display = "none";
        document.getElementById("NoneVisibleEye1").style.display = "block";
        document.getElementById('LoginPassword').type = "text";
    }
    else if (e == "NoneVisibleEye1") {
        document.getElementById(e).style.display = "none";
        document.getElementById("VisibleEye1").style.display = "block";
        document.getElementById('LoginPassword').type = "password";
    }

    if (e == "VisibleEye2") {
        document.getElementById(e).style.display = "none";
        document.getElementById("NoneVisibleEye2").style.display = "block";
        document.getElementById('CPPassword').type = "text";
    }
    else if (e == "NoneVisibleEye2") {
        document.getElementById(e).style.display = "none";
        document.getElementById("VisibleEye2").style.display = "block";
        document.getElementById('CPPassword').type = "password";
    }

    if (e == "VisibleEye3") {
        document.getElementById(e).style.display = "none";
        document.getElementById("NoneVisibleEye3").style.display = "block";
        document.getElementById('CPNewPassword').type = "text";
    }
    else if (e == "NoneVisibleEye3") {
        document.getElementById(e).style.display = "none";
        document.getElementById("VisibleEye3").style.display = "block";
        document.getElementById('CPNewPassword').type = "password";
    }
}

const passwordInput = document.getElementById('SignUpPassword');
const passwordStrengthText = document.getElementById('password-strength-text');

passwordInput.addEventListener('input', function () {
    const password = passwordInput.value;
    if (password == "") {
        document.getElementById("PasswordCheck").style.display = "none";
    }
    else {
        document.getElementById("PasswordCheck").style.display = "block";
    }
    const strength = calculatePasswordStrength(password);

    if (password === '') {
        passwordStrengthText.textContent = 'Password strength: ';
    } else {
        passwordStrengthText.textContent = "Password strength: " + strength;
    }

    // Update color based on strength
    passwordStrengthText.className = '';
    if (strength === 'Weak') {
        passwordStrengthText.classList.add('weak');
    } else if (strength === 'Medium') {
        passwordStrengthText.classList.add('medium');
    } else if (strength === 'Strong') {
        passwordStrengthText.classList.add('strong');
    }
});

const passwordInput1 = document.getElementById('CPNewPassword');
const passwordStrengthText1 = document.getElementById('password-strength-text1');

passwordInput1.addEventListener('input', function () {
    const password = passwordInput1.value;
    if (password == "") {
        document.getElementById("PasswordCheck1").style.display = "none";
    }
    else {
        document.getElementById("PasswordCheck1").style.display = "block";
    }
    const strength = calculatePasswordStrength(password);

    if (password === '') {
        passwordStrengthText1.textContent = 'Password strength: ';
    } else {
        passwordStrengthText1.textContent = "Password strength: " + strength;
    }

    // Update color based on strength
    passwordStrengthText1.className = '';
    if (strength === 'Weak') {
        passwordStrengthText1.classList.add('weak');
    } else if (strength === 'Medium') {
        passwordStrengthText1.classList.add('medium');
    } else if (strength === 'Strong') {
        passwordStrengthText1.classList.add('strong');
    }
});

function calculatePasswordStrength(password) {
    let strength = 'Weak';
    if (password.length >= 8) {
        strength = 'Medium';
    }
    if (password.length >= 12) {
        strength = 'Strong';
    }
    return strength;
}

document.getElementById('googleSignUpButton').addEventListener('click', function () {
    gapi.load('auth2', function () {
        var auth2 = gapi.auth2.init({
            client_id: '859693609337-1auodfdkuek1i3hb17pnlkfftqiiooi5.apps.googleusercontent.com', // Replace with your actual client ID
        });
        auth2.signIn().then(onSignIn).catch(function (error) {
            CommonErrormsg('error', 'Error Sign in : ' + JSON.stringify(error));
        });
    });
});

function onSignIn(googleUser) {
    var id_token = googleUser.getAuthResponse().id_token;
    alert(id_token); // Check if the token is retrieved
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1LogIn.aspx/SignInGoogle",
        data: JSON.stringify({ id_token: id_token }),
        dataType: "json",
        success: function (data) {
            if (data.d === "Success") {
                alert(data.d);
            } else {
                CommonErrormsg('error', 'Error in Webmethod : ' + data.d);
            }
            $('#cover-spin').hide();
        },
        error: function (result) {
            CommonErrormsg('error', 'Error in Ajax');
            $('#cover-spin').hide();
        }
    });
}

function validateEmail(email) {
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return regex.test(email);
}
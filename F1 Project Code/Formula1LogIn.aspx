<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Formula1LogIn.aspx.cs" Inherits="Formula1LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="google-signin-client_id" content="859693609337-1auodfdkuek1i3hb17pnlkfftqiiooi5.apps.googleusercontent.com" />
    <link rel="icon" href="../assets/images/Formula1/LoginImage/logo.png" type="image/x-icon" />
    <title>Log In</title>
    <style>
        @import url("https://fonts.googleapis.com/css2?family=Open+Sans:wght@200;300;400;500;600;700&display=swap");

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: "Open Sans", sans-serif;
        }

        body {
            display: flex;
            align-items: center;
            justify-content: left;
            min-height: 100vh;
            width: 100%;
            padding: 0;
            position: relative;
        }

            body::before {
                content: "";
                position: absolute;
                width: 100%;
                height: 100%;
                background: url("../assets/images/Formula1/LoginImage/F2.jpeg"), #000;
                background-position: center;
                background-size: cover;
            }

        .wrapper {
            width: 400px;
            border-radius: 8px;
            padding: 30px;
            text-align: center;
            border: 1px solid rgba(255, 255, 255, 0.5);
            backdrop-filter: blur(29px);
            -webkit-backdrop-filter: blur(29px);
            margin-left: 150px;
        }

        .wrapperModified {
            width: 400px;
            border-radius: 8px;
            padding: 30px;
            text-align: center;
            border: 1px solid rgba(255, 255, 255, 0.5);
            backdrop-filter: blur(29px);
            -webkit-backdrop-filter: blur(29px);
        }

        .classForm {
            display: flex;
            flex-direction: column;
        }

        h2 {
            font-size: 2rem;
            margin-bottom: 20px;
            color: #fff;
        }

        h3 {
            margin-bottom: 10px;
            color: #fff;
        }

        .forget1 {
            display: inline;
            text-align: left;
            justify-content: space-between;
            margin: 15px 0 35px 0;
            color: #fff;
        }

        .wrapper a {
            color: #efefef;
            text-decoration: none;
        }

            .wrapper a:hover {
                text-decoration: underline;
            }

        .buttonclass {
            background: #fff;
            color: #000;
            font-weight: 600;
            border: none;
            padding: 12px 20px;
            cursor: pointer;
            border-radius: 3px;
            font-size: 16px;
            border: 2px solid transparent;
            transition: 0.3s ease;
            width: 340px;
            margin-top: 20px;
        }

            .buttonclass:hover {
                color: #fff;
                border-color: #fff;
                background: rgba(255, 255, 255, 0.15);
            }

        .buttonclass1 {
            display: inline-flex;
            background: #fff;
            color: #000;
            font-weight: 600;
            border: none;
            padding: 10px 18px;
            cursor: pointer;
            border-radius: 3px;
            font-size: 16px;
            border: 2px solid transparent;
            transition: 0.3s ease;
            width: 340px;
            margin-top: 20px;
            height: 45px;
        }

        .register {
            text-align: center;
            margin-top: 30px;
            color: #fff;
        }
    </style>
    <style>
        .password-strength {
            margin-top: 20px;
        }

            .password-strength span {
                display: inline-block;
                padding: 5px 10px;
                border-radius: 3px;
                color: #fff;
                font-size: 14px;
                font-weight: bold;
            }

        .weak {
            background-color: #dc3545; /* Red */
        }

        .medium {
            background-color: #ffc107; /* Yellow */
        }

        .strong {
            background-color: #28a745; /* Green */
        }
    </style>
    <style>
        .input-field {
            max-width: 380px;
            width: 100%;
            background-color: #f0f0f0;
            margin: 10px 0;
            height: 55px;
            border-radius: 5px;
            display: flex;
            grid-template-columns: 15% 85%;
            padding: 0 0.4rem;
            position: relative;
            align-items: center;
            justify-content: start;
        }

            .input-field svg {
                text-align: center;
                margin-right: 10px;
            }

            .input-field input {
                background: none;
                outline: none;
                border: none;
                line-height: 1;
                font-weight: 600;
                font-size: 1.1rem;
                color: #333;
            }

                .input-field input::placeholder {
                    color: #aaa;
                    font-weight: 500;
                }
    </style>
    <style>
        /*Page Loading Css*/
        #cover-spin {
            position: fixed;
            width: 100%;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5);
            z-index: 9999;
            display: none;
        }

        @-webkit-keyframes spin {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        #cover-spin::after {
            content: '';
            display: block;
            position: absolute;
            left: 48%;
            top: 40%;
            width: 40px;
            height: 40px;
            border-style: solid;
            border-color: white;
            border-top-color: transparent;
            border-width: 4px;
            border-radius: 50%;
            -webkit-animation: spin .8s linear infinite;
            animation: spin .8s linear infinite;
        }
    </style>
</head>
<body>
    <div id="cover-spin"></div>
    <div class="wrapper">
        <div class="classForm">
            <img height="110" width="110" style="margin-left: 115px;" src="assets/images/Formula1/LoginImage/logo.png" />
            <h3>ULTRA F1 FANTASY LEAGUE</h3>
            <div id="LoginDiv">
                <h2>Login</h2>
                <div class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-person-fill" viewBox="0 0 16 16">
                        <path d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6" />
                    </svg>
                    <input id="LoginUserName" type="text" placeholder="Username" maxlength="100" />
                </div>
                <div class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-shield-lock-fill" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M8 0c-.69 0-1.843.265-2.928.56-1.11.3-2.229.655-2.887.87a1.54 1.54 0 0 0-1.044 1.262c-.596 4.477.787 7.795 2.465 9.99a11.8 11.8 0 0 0 2.517 2.453c.386.273.744.482 1.048.625.28.132.581.24.829.24s.548-.108.829-.24a7 7 0 0 0 1.048-.625 11.8 11.8 0 0 0 2.517-2.453c1.678-2.195 3.061-5.513 2.465-9.99a1.54 1.54 0 0 0-1.044-1.263 63 63 0 0 0-2.887-.87C9.843.266 8.69 0 8 0m0 5a1.5 1.5 0 0 1 .5 2.915l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99A1.5 1.5 0 0 1 8 5" />
                    </svg>
                    <input id="LoginPassword" type="password" placeholder="Password" />
                    <svg id="VisibleEye1" onclick="ChangeType(this.id)" style="margin-left: 50px; cursor: pointer;" xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                    </svg>
                    <svg id="NoneVisibleEye1" onclick="ChangeType(this.id)" xmlns="http://www.w3.org/2000/svg" style="display: none; cursor: pointer; margin-left: 50px;" width="20" height="20" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                        <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z" />
                        <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z" />
                    </svg>
                </div>
                <div class="forget1">
                    <a onclick="HideandShow('ChangePassword')" href="#">Change password?</a>
                    <a onclick="HideandShow('ForgotPassword')" style="margin-left: 60px;" href="#">Forgot password?</a>
                </div>
                <button class="buttonclass" onclick="CheckCredential()" type="button">Log In</button>
                <div class="register">
                    <p>Don't have an account? &nbsp; <a onclick="HideandShow('Register')" href="#">Register</a></p>
                </div>
            </div>
            <div style="display: none;" id="SignUpDiv">
                <h2>Sign Up</h2>
                <div class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-person-fill" viewBox="0 0 16 16">
                        <path d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6" />
                    </svg>
                    <input id="SignUpUserName" type="text" required="required" placeholder="UserName" maxlength="100" />
                </div>
                <div style="margin-top: 19px;" class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-envelope-at-fill" viewBox="0 0 16 16">
                        <path d="M2 2A2 2 0 0 0 .05 3.555L8 8.414l7.95-4.859A2 2 0 0 0 14 2zm-2 9.8V4.698l5.803 3.546zm6.761-2.97-6.57 4.026A2 2 0 0 0 2 14h6.256A4.5 4.5 0 0 1 8 12.5a4.49 4.49 0 0 1 1.606-3.446l-.367-.225L8 9.586zM16 9.671V4.697l-5.803 3.546.338.208A4.5 4.5 0 0 1 12.5 8c1.414 0 2.675.652 3.5 1.671" />
                        <path d="M15.834 12.244c0 1.168-.577 2.025-1.587 2.025-.503 0-1.002-.228-1.12-.648h-.043c-.118.416-.543.643-1.015.643-.77 0-1.259-.542-1.259-1.434v-.529c0-.844.481-1.4 1.26-1.4.585 0 .87.333.953.63h.03v-.568h.905v2.19c0 .272.18.42.411.42.315 0 .639-.415.639-1.39v-.118c0-1.277-.95-2.326-2.484-2.326h-.04c-1.582 0-2.64 1.067-2.64 2.724v.157c0 1.867 1.237 2.654 2.57 2.654h.045c.507 0 .935-.07 1.18-.18v.731c-.219.1-.643.175-1.237.175h-.044C10.438 16 9 14.82 9 12.646v-.214C9 10.36 10.421 9 12.485 9h.035c2.12 0 3.314 1.43 3.314 3.034zm-4.04.21v.227c0 .586.227.8.581.8.31 0 .564-.17.564-.743v-.367c0-.516-.275-.708-.572-.708-.346 0-.573.245-.573.791" />
                    </svg>
                    <input id="SignUpEmailId" type="text" required="required" placeholder="Email Id" />
                </div>
                <div style="margin-top: 19px;" class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-shield-lock-fill" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M8 0c-.69 0-1.843.265-2.928.56-1.11.3-2.229.655-2.887.87a1.54 1.54 0 0 0-1.044 1.262c-.596 4.477.787 7.795 2.465 9.99a11.8 11.8 0 0 0 2.517 2.453c.386.273.744.482 1.048.625.28.132.581.24.829.24s.548-.108.829-.24a7 7 0 0 0 1.048-.625 11.8 11.8 0 0 0 2.517-2.453c1.678-2.195 3.061-5.513 2.465-9.99a1.54 1.54 0 0 0-1.044-1.263 63 63 0 0 0-2.887-.87C9.843.266 8.69 0 8 0m0 5a1.5 1.5 0 0 1 .5 2.915l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99A1.5 1.5 0 0 1 8 5" />
                    </svg>
                    <input id="SignUpPassword" type="password" required="required" placeholder="Password" />
                    <svg id="VisibleEye" onclick="ChangeType(this.id)" style="margin-left: 50px; cursor: pointer;" xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                    </svg>
                    <svg id="NoneVisibleEye" onclick="ChangeType(this.id)" xmlns="http://www.w3.org/2000/svg" style="display: none; cursor: pointer; margin-left: 50px;" width="20" height="20" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                        <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z" />
                        <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z" />
                    </svg>
                </div>
                <div id="PasswordCheck" style="display: none; text-align: left; margin-top: -10px;">
                    <div class="password-strength">
                        <span id="password-strength-text">Password strength:                             
                        </span>
                    </div>
                </div>
                <button onclick="AddCredentialSignUp()" id="SignUpBtn" class="buttonclass" type="button">Sign Up</button>
                <p style="color: white; margin-top: 12px;">Or Sign up with social platforms</p>
                <button id="googleSignUpButton" class="buttonclass1" type="button">
                    <img style="margin-top: -8px; margin-left: 45px;" width="38" height="38" src="https://img.icons8.com/fluency/48/google-logo.png" alt="google-logo" />
                    &nbsp;
                    Sign Up With Google
                </button>
                <div class="register">
                    <p>Already have an account? &nbsp; <a onclick="HideandShow('Login')" href="#">Log In</a></p>
                </div>
            </div>
            <div style="display: none;" id="ChangePasswordDiv">
                <h2>Change Password</h2>
                <div class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-person-fill" viewBox="0 0 16 16">
                        <path d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6" />
                    </svg>
                    <input id="CPUserName" type="text" required="required" placeholder="UserName" maxlength="100" />
                </div>
                <div style="margin-top: 19px;" class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-shield-lock-fill" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M8 0c-.69 0-1.843.265-2.928.56-1.11.3-2.229.655-2.887.87a1.54 1.54 0 0 0-1.044 1.262c-.596 4.477.787 7.795 2.465 9.99a11.8 11.8 0 0 0 2.517 2.453c.386.273.744.482 1.048.625.28.132.581.24.829.24s.548-.108.829-.24a7 7 0 0 0 1.048-.625 11.8 11.8 0 0 0 2.517-2.453c1.678-2.195 3.061-5.513 2.465-9.99a1.54 1.54 0 0 0-1.044-1.263 63 63 0 0 0-2.887-.87C9.843.266 8.69 0 8 0m0 5a1.5 1.5 0 0 1 .5 2.915l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99A1.5 1.5 0 0 1 8 5" />
                    </svg>
                    <input id="CPPassword" type="password" required="required" placeholder="Old Password" />
                    <svg id="VisibleEye2" onclick="ChangeType(this.id)" style="margin-left: 50px; cursor: pointer;" xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                    </svg>
                    <svg id="NoneVisibleEye2" onclick="ChangeType(this.id)" xmlns="http://www.w3.org/2000/svg" style="display: none; cursor: pointer; margin-left: 50px;" width="20" height="20" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                        <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z" />
                        <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z" />
                    </svg>
                </div>
                <div style="margin-top: 19px;" class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-shield-lock-fill" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M8 0c-.69 0-1.843.265-2.928.56-1.11.3-2.229.655-2.887.87a1.54 1.54 0 0 0-1.044 1.262c-.596 4.477.787 7.795 2.465 9.99a11.8 11.8 0 0 0 2.517 2.453c.386.273.744.482 1.048.625.28.132.581.24.829.24s.548-.108.829-.24a7 7 0 0 0 1.048-.625 11.8 11.8 0 0 0 2.517-2.453c1.678-2.195 3.061-5.513 2.465-9.99a1.54 1.54 0 0 0-1.044-1.263 63 63 0 0 0-2.887-.87C9.843.266 8.69 0 8 0m0 5a1.5 1.5 0 0 1 .5 2.915l.385 1.99a.5.5 0 0 1-.491.595h-.788a.5.5 0 0 1-.49-.595l.384-1.99A1.5 1.5 0 0 1 8 5" />
                    </svg>
                    <input id="CPNewPassword" type="password" required="required" placeholder="New Password" />
                    <svg id="VisibleEye3" onclick="ChangeType(this.id)" style="margin-left: 50px; cursor: pointer;" xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                        <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                        <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                    </svg>
                    <svg id="NoneVisibleEye3" onclick="ChangeType(this.id)" xmlns="http://www.w3.org/2000/svg" style="display: none; cursor: pointer; margin-left: 50px;" width="20" height="20" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
                        <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z" />
                        <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z" />
                    </svg>
                </div>
                <div id="PasswordCheck1" style="display: none; text-align: left; margin-top: -10px;">
                    <div class="password-strength">
                        <span id="password-strength-text1">Password strength:                             
                        </span>
                    </div>
                </div>
                <button onclick="ChangeCredentialSignUp()" id="ChangePasswordbtn" class="buttonclass" type="button">Change Password</button>
                <div class="register">
                    <a onclick="HideandShow('Login')" href="#">Log In</a>
                </div>
            </div>
            <div style="display: none;" id="ForgotPasswordDiv">
                <h2>Forgot Password</h2>
                <h3 style="color: white;">Input your UserName, we will send your password to your email Id.</h3>
                <div class="input-field">
                    <svg xmlns="http://www.w3.org/2000/svg" width="26" height="26" fill="currentColor" class="bi bi-person-fill" viewBox="0 0 16 16">
                        <path d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6" />
                    </svg>
                    <input id="ForgotUserName" type="text" required="required" placeholder="UserName" maxlength="100" />
                </div>
                <button onclick="ForgotCredentialSignUp()" id="ForgotPasswordbtn" class="buttonclass" type="button">Send</button>
                <div class="register">
                    <a onclick="HideandShow('Login')" href="#">Log In</a>
                </div>
            </div>
        </div>
    </div>
    <script src="../Ajax/Ajax.js"></script>
    <script src="../Ajax/jquery.min.js"></script>
    <script src="../assets/vendors/sweetalert2/sweetalert2.all.min.js"></script>
    <script src="../Formula1JS/Formula1Login.js"></script>
    <script src="https://apis.google.com/js/platform.js" async="async" defer="defer"></script>
    <script>
        document.addEventListener('DOMContentLoaded', () => {
            sessionStorage.clear();
            localStorage.clear();
        });
    </script>
</body>
</html>

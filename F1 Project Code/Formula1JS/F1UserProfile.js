let SessionUserName = localStorage.getItem("UserName");

function GettingUsersData() {

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/F1UserProfile.aspx/GettingUsersDataFromDB",
        data: "{'UserID':'" + SessionUserName + "'}",
        dataType: "json",
        success: function (response) {
            var data = JSON.parse(response.d);

            if (data.Message == "Success") {
                document.getElementById("UserName").value = data.UserName;
                document.getElementById("EmailId").value = data.MailId;
                document.getElementById("phone").value = data.PhoneNo;
                document.getElementById("birthday").value = data.DateofBirth;
                document.getElementById("gender").value = data.Gender;
                document.getElementById("CountryofOrigin").value = data.CountryofOrigin;
                document.getElementById("CountryCode").value = data.CountryCode;
                document.getElementById("ImageId").innerHTML = "<img src='" + data.ImageUrl + "' style='width: 200px; height: 200px;' id='UsersImage' alt='Avatar'><input type='file' id='UserImageFile' disabled='disabled' onchange='SetImageInput(" + '"' + "UsersImage" + '"' + ", event);' accept='image/*' />";
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

function GetValuesFromDb(evalue, appendid) {
    
    if (evalue != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/F1UserProfile.aspx/GettingCountryDataFromDB",
            data: "{'Value':'" + evalue + "', 'appendid':'" + appendid + "'}",
            dataType: "json",
            success: function (response) {
                var data = JSON.parse(response.d);

                if (data.Message == "Success") {
                    document.getElementById(appendid).value = data.OutputValue;
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
}

function SetImageInput(eid, event) {
    const file = event.target.files[0];
    const uploadedImage = document.getElementById(eid);
    const reader = new FileReader();
    reader.onload = function (e) {
        uploadedImage.src = e.target.result;
    };
    reader.readAsDataURL(file);
}

function EditUserProfile() {
    document.getElementById("UserImageFile").removeAttribute("disabled");
    document.getElementById("CountryCode").removeAttribute("disabled");
    document.getElementById("phone").removeAttribute("disabled");
    document.getElementById("birthday").removeAttribute("disabled");
    document.getElementById("gender").removeAttribute("disabled");
    document.getElementById("CountryofOrigin").removeAttribute("disabled");
    localStorage.setItem("Edited", "true");
}

function FormulaOneDataSave() {
    var Editedornot = localStorage.getItem("Edited");

    if (Editedornot) {
        var PhoneNumber = document.getElementById("phone").value;
        var DateofBirth = document.getElementById("birthday").value;
        var Gender = document.getElementById("gender").value;
        var CountryofOrigin = document.getElementById("CountryofOrigin").value;
        var CountryofOriginCode = document.getElementById("CountryCode").value;
        var UserImageUrl = "";

        if (PhoneNumber == "" || DateofBirth == "" || Gender == "" || CountryofOrigin == "") {
            CommonErrormsg('error', 'Kindly Fill All The Mandatory Fields * !!!');
            return;
        }

        const file = document.getElementById("UserImageFile").files[0];
        var formData = new FormData();

        if (file) {
            $('#cover-spin').show(0);

            //Getting Currrnt Date And Time
            var CurrentDateTime = getCurrentDateTime();
            var regex = /^([\s\S]+)\.([a-zA-Z0-9]+)$/;

            formData.append("files", file);
            formData.append("FormName", "UserProfile");
            formData.append("DateTime", CurrentDateTime);

            var match = file.name.match(regex);

            if (match) {
                var FileNameWithoutExtension = match[1]; // File name
                var ExtensionWithoutFileName = match[2]; // File extension
            }

            var ActualUserProfileName = FileNameWithoutExtension.replaceAll(/[`~!@#$%^&*()_=+\[\]{};:'",<.>?/|\\]/g, '').replaceAll(" ", "") + "-" + CurrentDateTime + "." + ExtensionWithoutFileName;

            const xhr = new XMLHttpRequest();
            xhr.open('POST', '../Handler/Formula1MultiFileHandler.ashx', true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    UserImageUrl = "../assets/images/Formula1/UserImage/" + ActualUserProfileName + "";
                    document.getElementById("cover-spin").style.display = "none";
                    SavewithoutFileFormulaone();
                }
                else {
                    CommonErrormsg('error', 'An error occurred while uploading files.');
                    document.getElementById("cover-spin").style.display = "none";
                }
            };
            xhr.send(formData);
        }
        else {
            SavewithoutFileFormulaone();
        }

        function SavewithoutFileFormulaone() {

            var DataList = new Array();
            var UserProfileData = {};
            UserProfileData.UserPhoneNo = PhoneNumber;
            UserProfileData.UserDateofBirth = DateofBirth;
            UserProfileData.UserGender = Gender;
            UserProfileData.UserContryofOrigin = CountryofOrigin;
            UserProfileData.UserContryofOriginCode = CountryofOriginCode;
            UserProfileData.UserImageUrl = UserImageUrl;
            UserProfileData.UserName = SessionUserName;
            DataList.push(UserProfileData);

            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../Formula1/F1UserProfile.aspx/FormulaOneInsertData",
                data: JSON.stringify({
                    DataList: DataList
                }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Saved") {
                        CommonErrormsg('success', 'Updated Successfully.');
                        document.getElementById("cover-spin").style.display = "none";

                        setTimeout(function () {
                            window.location = "../Formula1/F1Dashboard.aspx";
                        }, 2000);
                    } else {
                        CommonErrormsg('error', data.d);
                    }
                    document.getElementById("cover-spin").style.display = "none";
                },
                error: function (result) {
                    CommonErrormsg('error', 'Error in Ajax');
                    document.getElementById("cover-spin").style.display = "none";
                    return;
                }
            });
        }
    }
}

//CurrentTime
function getCurrentDateTime() {
    const currentDate = new Date();
    const [day, month, year] = [currentDate.getDate(), currentDate.getMonth() + 1, currentDate.getFullYear()].map(num => num.toString().padStart(2, '0'));
    let [hours, minutes, seconds] = [currentDate.getHours() % 12 || 12, currentDate.getMinutes(), currentDate.getSeconds()].map(num => num.toString().padStart(2, '0'));
    const ampm = currentDate.getHours() >= 12 ? 'PM' : 'AM';

    return `${day}-${month}-${year}-${hours}-${minutes}-${seconds}-${ampm}`;
}
//CurrentTime
let SessionUserName = localStorage.getItem("UserName");

//Loading Drop Down Values While Loading Page
function TeamAddDropDownValues() {

    document.getElementById("TeamMember2").innerHTML = "";

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/F1TeamUp.aspx/DropDownValueMethod",
        data: "{'SessionUserName':'" + SessionUserName + "'}",
        dataType: "json",
        success: function (data) {
            $("#TeamMember2").append($("<option value='Select'></option>").html("Select"));
            $("#TeamMember3").append($("<option value='Select'></option>").html("Select"));
            $("#TeamMember4").append($("<option value='Select'></option>").html("Select"));
            $.each(data.d, function (key, ListValue) {
                if (ListValue.ddname == "User Name") {
                    $("#TeamMember2").append($("<option value='" + ListValue.ddvalue + "'></option>").html(ListValue.ddvalue));
                    $("#TeamMember3").append($("<option value='" + ListValue.ddvalue + "'></option>").html(ListValue.ddvalue));
                    $("#TeamMember4").append($("<option value='" + ListValue.ddvalue + "'></option>").html(ListValue.ddvalue));
                }
            });
        },
        error: function (result) {
            alert("Error" + result.responseText);
        }
    });
}
//Loading Drop Down Values While Loading Page

function GettingUsersData() {

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/F1TeamUp.aspx/GettingUsersDataFromDB",
        data: "{'UserID':'" + localStorage.getItem("OriginalUserName") + "'}",
        dataType: "json",
        success: function (response) {
            var data = JSON.parse(response.d);

            if (data.Message == "Success") {                
                document.getElementById("TeamName").value = data.TeamName;
                document.getElementById("TeamSlogan").value = data.TeamSlogan;
                document.getElementById("ContentPlaceHolder1_TeamMember1").value = data.TeamMember1;

                var optionTeamMember2 = document.createElement("option"); var optionTeamMember3 = document.createElement("option"); var optionTeamMember4 = document.createElement("option");                                
                optionTeamMember2.text = data.TeamMember2; optionTeamMember3.text = data.TeamMember3; optionTeamMember4.text = data.TeamMember4;
                optionTeamMember2.value = data.TeamMember2; optionTeamMember3.value = data.TeamMember3; optionTeamMember4.value = data.TeamMember4;

                document.getElementById("TeamMember2").add(optionTeamMember2);
                document.getElementById("TeamMember3").add(optionTeamMember3);
                document.getElementById("TeamMember4").add(optionTeamMember4);

                if (data.ApprovalType == "False") { document.getElementById("ApprovalType").checked = false; }
                if (data.ApprovalType == "True") { document.getElementById("ApprovalType").checked = true; }
                document.getElementById("ImageId").innerHTML = "<img src='" + data.ImageUrl + "' style='width: 200px; height: 200px;' id='UsersImage' alt='Avatar' /><input type='file' disabled='disabled' id='UserImageFile' onchange='SetImageInput(" + '"' + "UsersImage" + '"' + ", event);' accept='image/*' />";

                document.getElementById("TeamName").disabled = true;
                document.getElementById("TeamSlogan").disabled = true;
                document.getElementById("ContentPlaceHolder1_TeamMember1").disabled = true;
                document.getElementById("TeamMember2").disabled = true;
                document.getElementById("TeamMember3").disabled = true;
                document.getElementById("TeamMember4").disabled = true;
                document.getElementById("ApprovalType").disabled = true;

                document.getElementById("TeamMember2").value = data.TeamMember2;
                document.getElementById("TeamMember3").value = data.TeamMember3;
                document.getElementById("TeamMember4").value = data.TeamMember4;

                $("#AppendButtons").html(data.TopButtonValue);
                $("#ApprovalsPlaceHolder").html(data.ApprovalTableStr);
                $("#SaveorDeleteButton").html(data.SaveorDeletebtn);
            }
            else if (data.Message == "NotSuccess") {
                $("#SaveorDeleteButton").html(data.SaveorDeletebtn);
                document.getElementById("ImageId").innerHTML = "<img src='" + data.ImageUrl + "' style='width: 200px; height: 200px;' id='UsersImage' alt='Avatar'><input type='file' id='UserImageFile' onchange='SetImageInput(" + '"' + "UsersImage" + '"' + ", event);' accept='image/*' />";
            }
            else if (data.Message == "TeamMember") {
                document.getElementById("TeamNameFromDB").innerText = data.TeamName;
                $("#AppendButtons").html(data.TopButtonValue);
            }
            else if (data.Message == "NewTeamMember") {
                document.getElementById("ImageId").innerHTML = "<img src='" + data.ImageUrl + "' style='width: 200px; height: 200px;' id='UsersImage' alt='Avatar'><input type='file' id='UserImageFile' onchange='SetImageInput(" + '"' + "UsersImage" + '"' + ", event);' accept='image/*' />";
                $("#AppendButtons").html(data.TopButtonValue);
                $("#SaveorDeleteButton").html(data.SaveorDeletebtn);
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

function EditUserProfile() {
    document.getElementById("UserImageFile").disabled = false;
    document.getElementById("TeamName").disabled = false;
    document.getElementById("TeamSlogan").disabled = false;    
    document.getElementById("TeamMember2").disabled = false;    
    document.getElementById("TeamMember3").disabled = false;    
    document.getElementById("TeamMember4").disabled = false;
    document.getElementById("ApprovalType").disabled = false;    
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

function FormulaOneDataSave(e) {
    var TeamName = document.getElementById("TeamName").value;
    var TeamSlogan = document.getElementById("TeamSlogan").value;
    var TeamMember1 = localStorage.getItem("OriginalUserName");
    var TeamMember2 = document.getElementById("TeamMember2").value;
    if (TeamMember2 == "Select") { TeamMember2 = ""; }
    var TeamMember3 = document.getElementById("TeamMember3").value;
    if (TeamMember3 == "Select") { TeamMember3 = ""; }
    var TeamMember4 = document.getElementById("TeamMember4").value;
    if (TeamMember4 == "Select") { TeamMember4 = ""; }
    var ApprovalType = document.getElementById("ApprovalType").checked;
    var UserImageUrl = "../assets/images/Formula1/UserImage/NoImage.jpg";

    if (TeamName == "") {
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
        formData.append("FormName", "TeamImage");
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
                UserImageUrl = "../assets/images/Formula1/TeamImage/" + ActualUserProfileName + "";
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
        UserProfileData.TeamName = TeamName;
        UserProfileData.TeamSlogan = TeamSlogan;
        UserProfileData.TeamMember1 = TeamMember1;
        UserProfileData.TeamMember2 = TeamMember2;
        UserProfileData.TeamMember3 = TeamMember3;
        UserProfileData.TeamMember4 = TeamMember4;
        UserProfileData.ApprovalType = ApprovalType;
        UserProfileData.UserImageUrl = UserImageUrl;
        UserProfileData.EType = e;
        UserProfileData.UserName = SessionUserName;
        DataList.push(UserProfileData);

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/F1TeamUp.aspx/FormulaOneInsertData",
            data: JSON.stringify({
                DataList: DataList
            }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    window.location = "../Formula1/F1TeamUp.aspx";
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

//CurrentTime
function getCurrentDateTime() {
    const currentDate = new Date();
    const [day, month, year] = [currentDate.getDate(), currentDate.getMonth() + 1, currentDate.getFullYear()].map(num => num.toString().padStart(2, '0'));
    let [hours, minutes, seconds] = [currentDate.getHours() % 12 || 12, currentDate.getMinutes(), currentDate.getSeconds()].map(num => num.toString().padStart(2, '0'));
    const ampm = currentDate.getHours() >= 12 ? 'PM' : 'AM';

    return `${day}-${month}-${year}-${hours}-${minutes}-${seconds}-${ampm}`;
}
//CurrentTime

function DeleteUserProfile() {

    if (document.getElementById("TeamName").value == "") { return; }

    var Confirmation = confirm("Are you sure you want to delete this team?");

    if (Confirmation) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/F1TeamUp.aspx/DeleteUserProfile",
            data: "{'UserID':'" + SessionUserName + "', 'TeamName':'" + document.getElementById("TeamName").value + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "Deleted") {
                    window.location = "../Formula1/F1TeamUp.aspx";
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

function GetTeamInfo(TableId) {

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/F1TeamUp.aspx/GetTeamInfo",
        data: "{'TableId':'" + TableId + "'}",
        dataType: "json",
        success: function (response) {
            var data = JSON.parse(response.d);

            if (data.Message == "Success") {
                $("#SecDivPlaceHolder").html(data.TeamInfo);
            }
            else {
                CommonErrormsg('error', data.Message);
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

function Jointheteam(id, Type, Admin, Mail) {
    let OriginalSessionUserName = localStorage.getItem("OriginalUserName");
    $('#cover-spin').show(0);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/F1TeamUp.aspx/Jointheteam",
        data: "{'id':'" + id + "', 'Type':'" + Type + "', 'OriginalSessionUserName':'" + OriginalSessionUserName + "', 'SessionUserName':'" + SessionUserName + "', 'Admin':'" + Admin + "', 'MailtoSend':'" + Mail + "'}",
        dataType: "json",
        success: function (response) {
            var data = JSON.parse(response.d);

            if (data.Message == "Success") {
                CommonErrormsg('success', data.OriginalMessage);
                setTimeout(function () { window.location.reload(); }, 2000);
            }
            else {
                CommonErrormsg('error', data.Message);
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

function Exittheteam() {
    var id = document.getElementById("ExitfromTeamId").name;
    let OriginalSessionUserName = localStorage.getItem("OriginalUserName");
    $('#cover-spin').show(0);

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/F1TeamUp.aspx/Exittheteam",
        data: "{'id':'" + id + "', 'OriginalSessionUserName':'" + OriginalSessionUserName + "'}",
        dataType: "json",
        success: function (response) {
            var data = JSON.parse(response.d);

            if (data.Message == "Success") {
                CommonErrormsg('success', data.OriginalMessage);
                setTimeout(function () { window.location.reload(); }, 2000);
            }
            else {
                CommonErrormsg('error', data.Message);
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

function ApproveorReject(UpdateId, Type, RequestedBy, id) {
    var confirmation = confirm("Do you wise to " + Type);

    if (confirmation) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/F1TeamUp.aspx/ApproveorReject",
            data: "{'UpdateId':'" + UpdateId + "', 'Type':'" + Type + "', 'RequestedBy':'" + RequestedBy + "', 'id':'" + id + "'}",
            dataType: "json",
            success: function (response) {
                alert(response.d);
                window.location.reload();
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                return;
            }
        });
    }
}
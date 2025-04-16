let SessionUserName = localStorage.getItem("UserName");

function GettingConstructorsValues() {

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/ConstructorDetails.aspx/GettingConstructorsValuesFromAPI",
        data: "{'UserID':'" + SessionUserName + "', 'Year':'" + document.getElementById("ContentPlaceHolder1_YearValue").value + "', 'Race':'" + document.getElementById("ContentPlaceHolder1_RaceValue").value + "'}",
        dataType: "json",
        success: function (data) {
            if (data.d == "Success") {
                CommonErrormsg('success', 'Extraction Success.');
                GettingConstructorDataFromDB();
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

function GettingConstructorDataFromDB() {
    if (document.getElementById("ContentPlaceHolder1_YearValue").value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/ConstructorDetails.aspx/GettingConstructorDataFromDB",
            data: "{'Year':'" + document.getElementById("ContentPlaceHolder1_YearValue").value + "'}",
            dataType: "json",
            success: function (data) {
                $("#ConstructorsdataAppend").html(data.d);

                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Kindly Add Year.');
    }
}

function handleImageUpload(ImageID, event, uploadedImageID, uploadIconID) {

    const file = event.target.files[0];
    const uploadedImage = document.getElementById(uploadedImageID);
    const uploadIcon = document.getElementById(uploadIconID);
    var formData = new FormData();

    if (file) {
        $('#cover-spin').show(0);

        //Getting Currrnt Date And Time
        var CurrentDateTime = getCurrentDateTime();
        var regex = /^([\s\S]+)\.([a-zA-Z0-9]+)$/;

        formData.append("files", file);
        formData.append("FormName", "ConstructorsDetails");
        formData.append("DateTime", CurrentDateTime);

        var match = file.name.match(regex);

        if (match) {
            var FileNameWithoutExtension = match[1]; // File name
            var ExtensionWithoutFileName = match[2]; // File extension
        }

        var ActualDriverName = FileNameWithoutExtension.replaceAll(/[`~!@#$%^&*()_=+\[\]{};:'",<.>?/|\\]/g, '').replaceAll(" ", "") + "-" + CurrentDateTime + "." + ExtensionWithoutFileName;

        const xhr = new XMLHttpRequest();
        xhr.open('POST', '../Handler/Formula1MultiFileHandler.ashx', true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                if (ActualDriverName != "" && ImageID != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../Formula1/ConstructorDetails.aspx/UpdateDriverName",
                        data: "{'ActualDriverName':'" + ActualDriverName + "', 'ImageID':'" + ImageID + "', 'DeleteCode':'NotDelete'}",
                        dataType: "json",
                        success: function (data) {
                            if (data.d == "Updated") {
                                const reader = new FileReader();
                                reader.onload = function (e) {
                                    uploadedImage.src = e.target.result;
                                    uploadedImage.style.display = 'block';
                                    uploadIcon.style.display = 'none';
                                };
                                reader.readAsDataURL(file);
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
            }
            else {
                CommonErrormsg('error', 'An error occurred while uploading files.');
                document.getElementById("cover-spin").style.display = "none";
            }
        };
        xhr.send(formData);
    }
}

function RemoveImageFromDBandFiles(ImageID, ImgTagId, UploadIcon, DeleteorNot) {
    if (DeleteorNot == "Delete") {
        var Confirmation = confirm("Do you wish to delete this record from the database?");
        if (!Confirmation) {
            return;
        }
    }

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/ConstructorDetails.aspx/UpdateDriverName",
        data: "{'ActualDriverName':'', 'ImageID':'" + ImageID + "', 'DeleteCode':'" + DeleteorNot + "'}",
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                document.getElementById(ImgTagId).style.display = "none";
                document.getElementById(UploadIcon).style.display = "block";

                if (DeleteorNot == "Delete") {
                    GettingConstructorDataFromDB();
                }
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

//CurrentTime
function getCurrentDateTime() {
    const currentDate = new Date();
    const [day, month, year] = [currentDate.getDate(), currentDate.getMonth() + 1, currentDate.getFullYear()].map(num => num.toString().padStart(2, '0'));
    let [hours, minutes, seconds] = [currentDate.getHours() % 12 || 12, currentDate.getMinutes(), currentDate.getSeconds()].map(num => num.toString().padStart(2, '0'));
    const ampm = currentDate.getHours() >= 12 ? 'PM' : 'AM';

    return `${day}-${month}-${year}-${hours}-${minutes}-${seconds}-${ampm}`;
}
//CurrentTime


function DriverSave() {

    var Year = document.getElementById("ContentPlaceHolder1_Year").value;
    var DriverCode = document.getElementById("ContentPlaceHolder1_DriverCode").value;
    var DriverBioUrl = document.getElementById("ContentPlaceHolder1_DriverBioUrl").value;
    var FirstName = document.getElementById("ContentPlaceHolder1_FirstName").value;
    var Nation = document.getElementById("ContentPlaceHolder1_Nationality").value;

    if (Year == "" || DriverCode == "" || DriverBioUrl == "" || FirstName == "" || Nation == "") {
        document.getElementById("ContentPlaceHolder1_ErrorMsg").innerHTML = "Kindly Fill All The Mandatory Fields * !!!"
        return;
    }
    else {
        document.getElementById("ContentPlaceHolder1_ErrorMsg").innerHTML = "";
    }

    var Confirmation = confirm("Do you want to Save this Record!!!");

    if (Confirmation) {
        $('#cover-spin').show(0);

        var DataList = new Array();        
        var DriverManualAddTableView = {};
        DriverManualAddTableView.Year = Year;
        DriverManualAddTableView.DriverCode = DriverCode;
        DriverManualAddTableView.DriverBioUrl = DriverBioUrl;
        DriverManualAddTableView.FirstName = FirstName;
        DriverManualAddTableView.Nationality = Nation;
        DriverManualAddTableView.UserName = SessionUserName;
        DataList.push(DriverManualAddTableView);

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/ConstructorDetails.aspx/DriverInsertData",
            data: JSON.stringify({
                DataList: DataList
            }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    CommonErrormsg('success', 'Saved Successfully.');                  
                    GettingConstructorDataFromDB();
                } else {
                    CommonErrormsg('error', data.d);
                }
                document.getElementById("cover-spin").style.display = "none";

                Modalcloseandclear();
            },
            error: function (result) {
                Modalcloseandclear();
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
                return;
            }
        });
    }
}

function Modalcloseandclear() {
    document.getElementById("ContentPlaceHolder1_Year").value = "";
    document.getElementById("ContentPlaceHolder1_DriverCode").value = "";
    document.getElementById("ContentPlaceHolder1_DriverBioUrl").value = "";
    document.getElementById("ContentPlaceHolder1_FirstName").value = "";
    document.getElementById("ContentPlaceHolder1_Nationality").value = "";    

    document.getElementById("SideModelClose").click();
}

function MaptheDriver(CostructorID) { localStorage.setItem("CostructorID", CostructorID); }

function MapDriverAddManualTable() {
    var table = document.getElementById("MapDriverAdd");
    for (var j = 1; j < table.rows.length; j++) {
        var TableValue = table.rows[j].cells[0].innerHTML;
        if (TableValue == document.getElementById("ContentPlaceHolder1_DriverName").value) {
            return;
        }
    }

    if (document.getElementById("ContentPlaceHolder1_DriverName").value != "") {
        var rowcount = table.rows.length;
        var row = table.insertRow(rowcount);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);

        cell1.innerHTML = document.getElementById("ContentPlaceHolder1_DriverName").value;
        cell2.innerHTML = "<button type='button' onclick='MapDriverdeleteRow(this)' id='Delete' class='badge bg-danger'><i class='bi bi-trash'></i></button>";

        document.getElementById("ContentPlaceHolder1_DriverName").value = "";
    }
}
function MapDriverdeleteRow(btn) {
    var row = btn.parentNode.parentNode;
    row.parentNode.removeChild(row);
}

function UpdateMapDriverSave() {
    var table = document.getElementById("MapDriverAdd"), DriversName = "", CostructorID = localStorage.getItem("CostructorID");
    for (var j = 1; j < table.rows.length; j++) {
        DriversName += table.rows[j].cells[0].innerHTML + ",";
    }

    if (DriversName != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/ConstructorDetails.aspx/UpdateMapDriverSave",
            data: "{'DriversName':'" + DriversName + "', 'CostructorID':'" + CostructorID + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    window.location.reload();
                }
                else {
                    setTimeout(function () { window.location.reload(); }, 3000);
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
        CommonErrormsg("error", "Kindly Add Before Save.");
    }
}
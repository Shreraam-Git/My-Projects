let SessionUserName = localStorage.getItem("UserName");

function GettingCircuitsValues() {

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/CircuitsDetails.aspx/GettingCircuitsValuesFromAPI",
        data: "{'UserID':'" + SessionUserName + "', 'Year':'" + document.getElementById("ContentPlaceHolder1_YearValue").value + "', 'Race':'" + document.getElementById("ContentPlaceHolder1_RaceValue").value + "'}",
        dataType: "json",
        success: function (data) {
            if (data.d == "Success") {
                CommonErrormsg('success', 'Extraction Success.');
                GettingCircuitsDataFromDB();
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

function GettingCircuitsDataFromDB() {
    if (document.getElementById("ContentPlaceHolder1_YearValue").value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/CircuitsDetails.aspx/GettingCircuitDataFromDB",
            data: "{'Year':'" + document.getElementById("ContentPlaceHolder1_YearValue").value + "'}",
            dataType: "json",
            success: function (data) {
                $("#CircuitsdataAppend").html(data.d);

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

function handleImageUpload(RoundDiv, ImageID, event, uploadedImageID, uploadIconID) {
    
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
        formData.append("FormName", "CircuitsDetails");
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
                        url: "../Formula1/CircuitsDetails.aspx/UpdateCircuitsName",
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

                                document.getElementById(RoundDiv).setAttribute("onmouseover", "onmouseoverFunc(this)");
                                document.getElementById(RoundDiv).setAttribute("onmouseout", "onmouseoutFunc(this)");
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

//CurrentTime
function getCurrentDateTime() {
    const currentDate = new Date();
    const [day, month, year] = [currentDate.getDate(), currentDate.getMonth() + 1, currentDate.getFullYear()].map(num => num.toString().padStart(2, '0'));
    let [hours, minutes, seconds] = [currentDate.getHours() % 12 || 12, currentDate.getMinutes(), currentDate.getSeconds()].map(num => num.toString().padStart(2, '0'));
    const ampm = currentDate.getHours() >= 12 ? 'PM' : 'AM';

    return `${day}-${month}-${year}-${hours}-${minutes}-${seconds}-${ampm}`;
}
//CurrentTime

function RemoveImageFromDBandFiles(RoundDiv, ImageID, ImgTagId, UploadIcon, DeleteorNot) {
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
        url: "../Formula1/CircuitsDetails.aspx/UpdateCircuitsName",
        data: "{'ActualDriverName':'', 'ImageID':'" + ImageID + "', 'DeleteCode':'" + DeleteorNot + "'}",
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                document.getElementById(ImgTagId).style.display = "none";
                document.getElementById(UploadIcon).style.display = "block";

                if (DeleteorNot == "Delete") {
                    GettingCircuitsDataFromDB();
                }
                else {
                    document.getElementById(RoundDiv).removeAttribute("onmouseover");
                    document.getElementById(RoundDiv).removeAttribute("onmouseout");
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

function Modalcloseandclear() {
    document.getElementById("ContentPlaceHolder1_CircuitCode").value = "";
    document.getElementById("ContentPlaceHolder1_CircuitBioUrl").value = "";
    document.getElementById("ContentPlaceHolder1_CircuitName").value = "";
    document.getElementById("ContentPlaceHolder1_CircuitLatitude").value = "";
    document.getElementById("ContentPlaceHolder1_CircuitLongitude").value = "";
    document.getElementById("ContentPlaceHolder1_Locality").value = "";
    document.getElementById("ContentPlaceHolder1_Country").value = "";

    document.getElementById("SideModelClose").click();    
}

function onmouseoverFunc(Event, e) {
    Event.preventDefault();
    e.classList.add('zoomed');
}

function onmouseoutFunc(e) {
    e.classList.remove('zoomed');
}

function CircuitsSave() {

    var Year = document.getElementById("ContentPlaceHolder1_Year").value;
    var CircuitCode = document.getElementById("ContentPlaceHolder1_CircuitCode").value;
    var CircuitBioUrl = document.getElementById("ContentPlaceHolder1_CircuitBioUrl").value;
    var CircuitName = document.getElementById("ContentPlaceHolder1_CircuitName").value;
    var CircuitLatitude = document.getElementById("ContentPlaceHolder1_CircuitLatitude").value;
    var CircuitLongitude = document.getElementById("ContentPlaceHolder1_CircuitLongitude").value;
    var Locality = document.getElementById("ContentPlaceHolder1_Locality").value;
    var Country = document.getElementById("ContentPlaceHolder1_Country").value;

    if (Year == "" || CircuitCode == "" || CircuitBioUrl == "" || CircuitName == "" || CircuitLatitude == "" || CircuitLongitude == "" || Locality == "" || Country == "") {
        document.getElementById("ContentPlaceHolder1_ErrorMsg").innerText = "Kindly Fill All The Mandatory Fields * !!!";
        return;
    }
    else {
        document.getElementById("ContentPlaceHolder1_ErrorMsg").innerText = "";
    }

    var Confirmation = confirm("Do you want to Save this Record!!!");

    if (Confirmation) {
        $('#cover-spin').show(0);

        var DataList = new Array();
        var DriverManualAddTableView = {};
        DriverManualAddTableView.Year = Year;
        DriverManualAddTableView.CircuitCode = CircuitCode;
        DriverManualAddTableView.CircuitBioUrl = CircuitBioUrl;
        DriverManualAddTableView.CircuitName = CircuitName;
        DriverManualAddTableView.CircuitLatitude = CircuitLatitude;
        DriverManualAddTableView.CircuitLongitude = CircuitLongitude;
        DriverManualAddTableView.Locality = Locality;
        DriverManualAddTableView.Country = Country;
        DriverManualAddTableView.UserName = SessionUserName;
        DataList.push(DriverManualAddTableView);

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/CircuitsDetails.aspx/CircuitsInsertData",
            data: JSON.stringify({
                DataList: DataList
            }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    CommonErrormsg('success', 'Saved Successfully.');
                    GettingCircuitsDataFromDB();
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
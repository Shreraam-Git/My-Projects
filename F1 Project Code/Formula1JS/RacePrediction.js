let SessionUserName = localStorage.getItem("UserName");

function GettingRaceName(e, RaceList) {
    
    var Year = document.getElementById(e.id).value;
    document.getElementById(RaceList).innerHTML = "";

    if (Year != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/RacePrediction.aspx/GettingRaceName",
            data: "{'Year':'" + Year + "'}",
            dataType: "json",
            success: function (data) {
                $("#" + RaceList).html(data.d);
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

function CalculatingtheresultPoints() {

    document.getElementById("DriverConstructorSection").style.display = "none";    

    var Year = document.getElementById("ContentPlaceHolder1_YearValue").value;
    var Race = document.getElementById("ContentPlaceHolder1_RaceValue").value;
    var UserName = document.getElementById("UserName").value;
    
    if (Year != "" && Race != "" && UserName != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/RacePrediction.aspx/CalculatingtheresultPoints",
            data: "{'Year':'" + Year + "', 'Race':'" + Race + "', 'UserName':'" + UserName + "'}",
            dataType: "json",
            success: function (response) {                                                
                var data = JSON.parse(response.d);

                if (data.Message == "Success") {                    
                    $("#RacePredictionDriverDetailsTable").html(data.DriverTable);
                    document.getElementById("DriverRandomOrder").innerText = data.DriverRandomOrder;                    

                    $("#ConstructorPredictionDriverDetailsTable").html(data.ConstructorTable);
                    document.getElementById("ConstructorRandomOrder").innerText = data.ConstructorRandomOrder;

                    $("#PolePositionPredictionDriverDetailsTable").html(data.PolePositionTable);
                    
                    $("#ConstraintsPredictionDriverDetailsTable").html(data.ConstraintsTable);
                    
                    $("#SprintEntryPredictionDriverDetailsTable").html(data.SprintEntryTable);

                    document.getElementById("DriverConstructorSection").style.display = "block";
                }
                else {
                    CommonErrormsg('error', data.Message);
                }   

                ChangetheTableColors(document.getElementById("toggle-dark").checked);
                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
                return;
            }
        });
    }
    else {
        CommonErrormsg("error", "Kindly Fill all the Fields");
    }
}

function ChangetheTableColors(Values) {
    if (Values == true) {        
        const headers = document.getElementsByTagName('th');
        for (let i = 0; i < headers.length; i++) {
            headers[i].style.borderBottom = '1px solid white';
            headers[i].style.backgroundColor = '#2D2D40';
            headers[i].style.color = 'white';
        }

        const Subheaders = document.getElementsByTagName('td');
        for (let i = 0; i < Subheaders.length; i++) {
            Subheaders[i].style.backgroundColor = '#2D2D40';
            Subheaders[i].style.color = 'white';
        }        
    }
    else {        
        const headers = document.getElementsByTagName('th');
        for (let i = 0; i < headers.length; i++) {
            headers[i].style.borderBottom = '1px solid black';
            headers[i].style.backgroundColor = 'transparent';
            headers[i].style.color = 'black';
        }

        const Subheaders = document.getElementsByTagName('td');
        for (let i = 0; i < Subheaders.length; i++) {
            Subheaders[i].style.backgroundColor = 'transparent';
            Subheaders[i].style.color = 'black';
        }
    }
}
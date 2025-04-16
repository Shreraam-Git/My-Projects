let SessionUserName = localStorage.getItem("UserName");

//Getting the Info while draging the object
function dragFunction(event, Details) {
    event.dataTransfer.setData('text/plain', event.target.src);
    event.dataTransfer.setData('info', event.target.getAttribute('data-info'));
    event.dataTransfer.setData('Confirmation', "FormtheSame");
    event.dataTransfer.setData('Details', Details);
}

//Allowing the drop option
function allowDrop(event) {
    event.preventDefault();
}

//Pasting the draged information to the relevent object
function dropFunction(event, eid, eptag, DRorCR) {
    const Confirming = event.dataTransfer.getData('Confirmation');
    if (Confirming != "FormtheSame") {
        return;
    }

    const Details = event.dataTransfer.getData('Details');
    if (DRorCR == "DR" && Details == "Drivers") { }
    else if (DRorCR == "CR" && Details == "Constructors") { }
    else { return; }

    const imageSrc = event.dataTransfer.getData('text/plain');

    if (event.target.name == "FourthGrid") {
        if (Details == "Drivers") {
            var SprintDriver1 = document.getElementById("SprintDriveroneImage").src, SprintDriver2 = document.getElementById("SprintDrivertwoImage").src,
                SprintDriver3 = document.getElementById("SprintDriverthreeImage").src;

            if (SprintDriver1 == imageSrc || SprintDriver2 == imageSrc || SprintDriver3 == imageSrc) {
                return;
            }
        }
    }
    else if (event.target.name != "ThirdGrid") {
        if (Details == "Drivers") {
            var Driver1 = document.getElementById("DriveroneImage").src, Driver2 = document.getElementById("DrivertwoImage").src,
                Driver3 = document.getElementById("DriverthreeImage").src, Driver4 = document.getElementById("DriverfourImage").src,
                Driver5 = document.getElementById("DriverfiveImage").src, Driver6 = document.getElementById("DriversixImage").src;

            if (Driver1 == imageSrc || Driver2 == imageSrc || Driver3 == imageSrc || Driver4 == imageSrc || Driver5 == imageSrc || Driver6 == imageSrc) {
                return;
            }
        }
        else if (Details == "Constructors") {
            var Constructor1 = document.getElementById("ConstructoroneImg").src, Constructor2 = document.getElementById("Constructortwoimg").src,
                Constructor3 = document.getElementById("Constructorthreeimg").src;

            if (Constructor1 == imageSrc || Constructor2 == imageSrc || Constructor3 == imageSrc) {
                return;
            }
        }
    }

    event.preventDefault();
    event.target.title = event.dataTransfer.getData('info');
    document.getElementById(eptag).innerText = event.dataTransfer.getData('info');
    const droppedImage = document.getElementById(eid);
    droppedImage.src = imageSrc;
}

//Removing All Race Entry Data
function RemoveAllData() {
    document.getElementById("DriveroneImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("DrivertwoImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("DriverthreeImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("DriverfourImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("DriverfiveImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("DriversixImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("SprintDriveroneImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("SprintDrivertwoImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("SprintDriverthreeImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("ConstructoroneImg").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("Constructortwoimg").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("Constructorthreeimg").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("FastestLapImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("PolePositionImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";
    document.getElementById("MostPlacesGainedImage").src = "../assets/images/Formula1/Dashboard/PlusImage.png";

    document.getElementById("Driveroneptag").innerHTML = "Driver 1";
    document.getElementById("Drivertwoptag").innerHTML = "Driver 2";
    document.getElementById("Driverthreeptag").innerHTML = "Driver 3";
    document.getElementById("Driverfourptag").innerHTML = "Driver 4";
    document.getElementById("Driverfiveptag").innerHTML = "Driver 5";
    document.getElementById("Driversixptag").innerHTML = "Driver 6";
    document.getElementById("SprintDriveroneptag").innerHTML = "Driver 1";
    document.getElementById("SprintDrivertwoptag").innerHTML = "Driver 2";
    document.getElementById("SprintDriverthreeptag").innerHTML = "Driver 3";
    document.getElementById("Constructoroneptag").innerHTML = "Constructor 1";
    document.getElementById("Constructortwoptag").innerHTML = "Constructor 2";
    document.getElementById("Constructorthreeptag").innerHTML = "Constructor 3";
    document.getElementById("FastestLapptag").innerHTML = "";
    document.getElementById("PolePositionptag").innerHTML = "";
    document.getElementById("MostPlacesGainedptag").innerHTML = "";
}

//Saving all the race entry data
function RaceEntrySave() {

    var yearandround = document.getElementById("YearRoundDataValue").innerText;
    var Spliting = yearandround.split('/');

    var Driver1 = document.getElementById("Driveroneptag").innerText, Driver2 = document.getElementById("Drivertwoptag").innerText,
        Driver3 = document.getElementById("Driverthreeptag").innerText, Driver4 = document.getElementById("Driverfourptag").innerText,
        Driver5 = document.getElementById("Driverfiveptag").innerText, Driver6 = document.getElementById("Driversixptag").innerText,
        SprintDriver1 = document.getElementById("SprintDriveroneptag").innerText, SprintDriver2 = document.getElementById("SprintDrivertwoptag").innerText,
        SprintDriver3 = document.getElementById("SprintDriverthreeptag").innerText, SprintDriveroneImage = document.getElementById("SprintDriveroneImage").src,
        SprintDrivertwoImage = document.getElementById("SprintDrivertwoImage").src, SprintDriverthreeImage = document.getElementById("SprintDriverthreeImage").src,
        Constructor1 = document.getElementById("Constructoroneptag").innerText, Constructor2 = document.getElementById("Constructortwoptag").innerText,
        Constructor3 = document.getElementById("Constructorthreeptag").innerText, FastestLap = document.getElementById("FastestLapptag").innerText,
        PolePosition = document.getElementById("PolePositionptag").innerText, MostPlacedGained = document.getElementById("MostPlacesGainedptag").innerText,
        Year = Spliting[0].trim(), Round = Spliting[1].trim();

    if (Year == "" || Round == "") {
        CommonErrormsg("error", "Can't Make Race Entry Without Year & Round Data.");
        return;
    }

    if (Driver1 == "Driver 1" || Driver2 == "Driver 2" || Driver3 == "Driver 3" || Driver4 == "Driver 4" || Driver5 == "Driver 5" || Driver6 == "Driver 6" ||
        Constructor1 == "Constructor 1" || Constructor2 == "Constructor 2" || Constructor3 == "Constructor 3" ||
        FastestLap == "" || PolePosition == "" || MostPlacedGained == "") {
        CommonErrormsg("error", "Kindly Fill all the Fields.");
        return;
    }

    if (document.getElementById("ContentPlaceHolder1_Dummyvalue").value == "Sprint Entry") {
        if (SprintDriver1 == "Driver 1" || SprintDriver2 == "Driver 2" || SprintDriver3 == "Driver 3") {
            CommonErrormsg("error", "Kindly Fill all the Fields.");
            return;
        }
    }
    else {
        SprintDriver1 = ""; SprintDriver2 = ""; SprintDriver3 = "";
        SprintDriveroneImage = ""; SprintDrivertwoImage = ""; SprintDriverthreeImage = "";
    }

    var Confirmation = confirm("Do you want to Save this Record!!!");

    if (Confirmation) {
        $('#cover-spin').show(0);

        var DataList = new Array();
        var RaceEntryFields = {};
        RaceEntryFields.DataYear = Year;
        RaceEntryFields.DataRound = Round;
        RaceEntryFields.DataDriver1 = Driver1;
        RaceEntryFields.DataDriver2 = Driver2;
        RaceEntryFields.DataDriver3 = Driver3;
        RaceEntryFields.DataDriver4 = Driver4;
        RaceEntryFields.DataDriver5 = Driver5;
        RaceEntryFields.DataDriver6 = Driver6;
        RaceEntryFields.DataSprintDriver1 = SprintDriver1;
        RaceEntryFields.DataSprintDriver2 = SprintDriver2;
        RaceEntryFields.DataSprintDriver3 = SprintDriver3;
        RaceEntryFields.DataConstructor1 = Constructor1;
        RaceEntryFields.DataConstructor2 = Constructor2;
        RaceEntryFields.DataConstructor3 = Constructor3;
        RaceEntryFields.DataFastestLap = FastestLap;
        RaceEntryFields.DataPolePosition = PolePosition;
        RaceEntryFields.DataMostPlacedGained = MostPlacedGained;

        RaceEntryFields.DataDriver1Img = document.getElementById("DriveroneImage").src;
        RaceEntryFields.DataDriver2Img = document.getElementById("DrivertwoImage").src;
        RaceEntryFields.DataDriver3Img = document.getElementById("DriverthreeImage").src;
        RaceEntryFields.DataDriver4Img = document.getElementById("DriverfourImage").src;
        RaceEntryFields.DataDriver5Img = document.getElementById("DriverfiveImage").src;
        RaceEntryFields.DataDriver6Img = document.getElementById("DriversixImage").src;
        RaceEntryFields.DataSprintDriver1Img = SprintDriveroneImage;
        RaceEntryFields.DataSprintDriver2Img = SprintDrivertwoImage;
        RaceEntryFields.DataSprintDriver3Img = SprintDriverthreeImage;
        RaceEntryFields.DataConstructor1Img = document.getElementById("ConstructoroneImg").src;
        RaceEntryFields.DataConstructor2Img = document.getElementById("Constructortwoimg").src;
        RaceEntryFields.DataConstructor3Img = document.getElementById("Constructorthreeimg").src;
        RaceEntryFields.DataFastestLapImg = document.getElementById("FastestLapImage").src;
        RaceEntryFields.DataPolePositionImg = document.getElementById("PolePositionImage").src;
        RaceEntryFields.DataMostPlacedGainedImg = document.getElementById("MostPlacesGainedImage").src;

        RaceEntryFields.DataUserName = SessionUserName;
        DataList.push(RaceEntryFields);

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/F1RaceEntry.aspx/RaceEntryInsertData",
            data: JSON.stringify({
                DataList: DataList
            }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    CommonErrormsg('success', 'Saved Successfully.');

                    setTimeout(function () {
                        window.location.reload();
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

function EnablingDiv() {
    $('#cover-spin').show(0);

    document.getElementById("FirstDiv").style.removeProperty("pointer-events");
    document.getElementById("SecondDiv").style.removeProperty("pointer-events");
    document.getElementById("ThirdDiv").style.removeProperty("pointer-events");
    document.getElementById("FourthDiv").style.removeProperty("pointer-events");

    setTimeout(function () {
        document.getElementById("cover-spin").style.display = "none";
    }, 1000);
}

function CheckingtheRaceEntry() {

    var yearandround = document.getElementById("YearRoundDataValue").innerText;
    var Spliting = yearandround.split('/');
    var Year = Spliting[0].trim(), Round = Spliting[1].trim();

    if (Year != "" && Round != "" && SessionUserName != "" && Year != null && Round != null && SessionUserName != null) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/F1RaceEntry.aspx/CheckingtheRaceEntry",
            data: "{'Year':'" + Year + "', 'Round':'" + Round + "', 'SessionUserName':'" + SessionUserName + "'}",
            dataType: "json",
            success: function (data) {
                $.each(data.d, function (key, ListValue) {

                    document.getElementById("FirstDiv").style.pointerEvents = "none";
                    document.getElementById("SecondDiv").style.pointerEvents = "none";
                    document.getElementById("ThirdDiv").style.pointerEvents = "none";
                    document.getElementById("FourthDiv").style.pointerEvents = "none";

                    document.getElementById("DriveroneImage").src = ListValue.DataDriver1Img;
                    document.getElementById("DrivertwoImage").src = ListValue.DataDriver2Img;
                    document.getElementById("DriverthreeImage").src = ListValue.DataDriver3Img;
                    document.getElementById("DriverfourImage").src = ListValue.DataDriver4Img;
                    document.getElementById("DriverfiveImage").src = ListValue.DataDriver5Img;
                    document.getElementById("DriversixImage").src = ListValue.DataDriver6Img;
                    document.getElementById("SprintDriveroneImage").src = ListValue.DataSprintDriver1Img;
                    document.getElementById("SprintDrivertwoImage").src = ListValue.DataSprintDriver2Img;
                    document.getElementById("SprintDriverthreeImage").src = ListValue.DataSprintDriver3Img;
                    document.getElementById("ConstructoroneImg").src = ListValue.DataConstructor1Img;
                    document.getElementById("Constructortwoimg").src = ListValue.DataConstructor2Img;
                    document.getElementById("Constructorthreeimg").src = ListValue.DataConstructor3Img;
                    document.getElementById("FastestLapImage").src = ListValue.DataFastestLapImg;
                    document.getElementById("PolePositionImage").src = ListValue.DataPolePositionImg;
                    document.getElementById("MostPlacesGainedImage").src = ListValue.DataMostPlacedGainedImg;

                    document.getElementById("Driveroneptag").innerHTML = ListValue.DataDriver1;
                    document.getElementById("Drivertwoptag").innerHTML = ListValue.DataDriver2;
                    document.getElementById("Driverthreeptag").innerHTML = ListValue.DataDriver3;
                    document.getElementById("Driverfourptag").innerHTML = ListValue.DataDriver4;
                    document.getElementById("Driverfiveptag").innerHTML = ListValue.DataDriver5;
                    document.getElementById("Driversixptag").innerHTML = ListValue.DataDriver6;
                    document.getElementById("SprintDriveroneptag").innerHTML = ListValue.DataSprintDriver1;
                    document.getElementById("SprintDrivertwoptag").innerHTML = ListValue.DataSprintDriver2;
                    document.getElementById("SprintDriverthreeptag").innerHTML = ListValue.DataSprintDriver3;
                    document.getElementById("Constructoroneptag").innerHTML = ListValue.DataConstructor1;
                    document.getElementById("Constructortwoptag").innerHTML = ListValue.DataConstructor2;
                    document.getElementById("Constructorthreeptag").innerHTML = ListValue.DataConstructor3;
                    document.getElementById("FastestLapptag").innerHTML = ListValue.DataFastestLap;
                    document.getElementById("PolePositionptag").innerHTML = ListValue.DataPolePosition;
                    document.getElementById("MostPlacesGainedptag").innerHTML = ListValue.DataMostPlacedGained;
                });
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                return;
            }
        });
    }
    else {
        CommonErrormsg("error", "Network Error");
        return;
    }
}
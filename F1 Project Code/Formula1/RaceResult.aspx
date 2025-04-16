<%@ Page Title="Race Result Master" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="RaceResult.aspx.cs" Inherits="Formula1_RaceResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Race Result</h5>
        </div>
        <div class="page-content">
            <section class="row">
                <div class="col-12 col-lg-12">
                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="input-group">
                                        <div class="col-md-4">
                                            <div class="form-group row align-items-center">
                                                <div class="col-lg-4 col-5">
                                                    <label for="basicInput"><b>Year / Race Name</b></label>
                                                </div>
                                                <div class="col-lg-8 col-7">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="YearValue" onblur="GettingRaceName(this, 'RoundandRaceNameList')" runat="server" TextMode="Number" AutoCompleteType="Disabled" data-MaxLength="4" oninput="javascript: if(this.value < 0){this.value = 0;}; this.value=this.value.slice(0,this.dataset.maxlength);" class="form-control" placeholder="Set Year"></asp:TextBox>
                                                        <asp:TextBox ID="RaceValue" onblur="DrpDwnFreeTextCheckFormulaone(this); GettingSprintEntry(this.id, 'ContentPlaceHolder1_YearValue');" list="RoundandRaceNameList" title="Race Count / Race Name of the Year" runat="server" AutoCompleteType="Disabled" class="form-control" placeholder="Select"></asp:TextBox>
                                                        <datalist id="RoundandRaceNameList">
                                                            <asp:PlaceHolder runat="server" ID="RoundandRaceNameListDD"></asp:PlaceHolder>
                                                        </datalist>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <div class="buttons">
                                        <button class="btn btn-primary" type="button" onclick="CheckingtheRaceResult();">Search</button>

                                        <div class="modal-success me-1 mb-1 d-inline-block">
                                            <!-- Button trigger for Success theme modal -->
                                            <button type="button" style="background-color: red;" class="btn btn-danger" id="ExtractFromAPI"
                                                data-bs-toggle="modal" data-bs-target="#success1">
                                                Extract From API
                                            </button>
                                        </div>
                                        <div class="modal-success me-1 mb-1 d-inline-block">
                                            <!-- Button trigger for Success theme modal -->
                                            <button type="button" style="background-color: green;" class="btn btn-success" onclick="AddResultManually();">
                                                Add or Edit Manually
                                            </button>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section class="row">
                <div class="col-12 col-lg-12">
                    <div class="row">
                        <div class="col-6">
                            <div class="card" style="height: 600px;">
                                <div style="overflow: scroll;" class="card-body">
                                    <%--pointer-events: none;--%>

                                    <div class="header" style="flex-direction: row; display: flex; gap: 20px; align-items: center;">
                                        <h5>Race Result for the Year / Round</h5>
                                        <div class="buttons" id="ThreeButtons" style="margin-top: 10px; display: none;">
                                            <button class="btn btn-danger" style="margin-top: -10px;" type="button" title="Remove all Data" onclick="RemoveAllData()"><i class="bi bi-trash3-fill"></i></button>
                                            <button class="btn btn-primary" style="margin-top: -10px;" type="button" title="Edit Race Entry" onclick="EnablingDiv();"><i class="bi bi-pencil-fill"></i></button>
                                            <button class="btn btn-success" style="margin-top: -10px;" title="Save Race Entry" onclick="RaceResultSave()" type="button"><i class="bi bi-floppy2-fill"></i></button>
                                        </div>
                                    </div>
                                    <hr />

                                    <div id="FirstDiv" style="flex-direction: row; display: flex; align-items: center; margin: 10px 20px 0 0;">
                                        <h6 style="margin-right: 35px;">Drivers</h6>

                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'DriveroneImage', 'Driveroneptag', 'DR')">
                                            <p style="margin: 0;">1st</p>
                                            <img id="DriveroneImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Driveroneptag" style="margin: 5px 0 0 0;">Driver 1</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'DrivertwoImage', 'Drivertwoptag', 'DR')">
                                            <p style="margin: 0;">2nd</p>
                                            <img id="DrivertwoImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Drivertwoptag" style="margin: 5px 0 0 0;">Driver 2</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'DriverthreeImage', 'Driverthreeptag', 'DR')">
                                            <p style="margin: 0;">3rd</p>
                                            <img id="DriverthreeImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Driverthreeptag" style="margin: 5px 0 0 0;">Driver 3</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'DriverfourImage', 'Driverfourptag', 'DR')">
                                            <p style="margin: 0;">4th</p>
                                            <img id="DriverfourImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Driverfourptag" style="margin: 5px 0 0 0;">Driver 4</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'DriverfiveImage', 'Driverfiveptag', 'DR')">
                                            <p style="margin: 0;">5th</p>
                                            <img id="DriverfiveImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Driverfiveptag" style="margin: 5px 0 0 0;">Driver 5</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'DriversixImage', 'Driversixptag', 'DR')">
                                            <p style="margin: 0;">6th</p>
                                            <img id="DriversixImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Driversixptag" style="margin: 5px 0 0 0;">Driver 6</p>
                                        </div>
                                    </div>
                                    <hr />

                                    <%--Dummy--%>
                                    <asp:TextBox runat="server" ID="Dummyvalue" Style="display: none;"></asp:TextBox>
                                    <%--Dummy--%>
                                    <div id="SprintEntryDiv" style="display: none;">
                                        <div id="FourthDiv" style="flex-direction: row; display: flex; align-items: center; margin: 10px 20px 0 0; gap: 30px;">
                                            <h6 style="margin-right: 35px;">Sprint Entry</h6>

                                            <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'SprintDriveroneImage', 'SprintDriveroneptag', 'DR')">
                                                <p style="margin: 0;">1st</p>
                                                <img name="FourthGrid" id="SprintDriveroneImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                                <p id="SprintDriveroneptag" style="margin: 5px 0 0 0;">Driver 1</p>
                                            </div>
                                            <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'SprintDrivertwoImage', 'SprintDrivertwoptag', 'DR')">
                                                <p style="margin: 0;">2nd</p>
                                                <img name="FourthGrid" id="SprintDrivertwoImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                                <p id="SprintDrivertwoptag" style="margin: 5px 0 0 0;">Driver 2</p>
                                            </div>
                                            <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'SprintDriverthreeImage', 'SprintDriverthreeptag', 'DR')">
                                                <p style="margin: 0;">3rd</p>
                                                <img name="FourthGrid" id="SprintDriverthreeImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                                <p id="SprintDriverthreeptag" style="margin: 5px 0 0 0;">Driver 3</p>
                                            </div>
                                        </div>
                                        <hr />
                                    </div>

                                    <div id="SecondDiv" style="flex-direction: row; display: flex; align-items: center; margin: 10px 20px 0 0; gap: 30px;">
                                        <h6 style="margin-right: 50px;">Constructors</h6>

                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'ConstructoroneImg', 'Constructoroneptag', 'CR')">
                                            <p style="margin: 0;">1st</p>
                                            <img id="ConstructoroneImg" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Constructoroneptag" style="margin: 5px 0 0 0;">Constructor 1</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'Constructortwoimg', 'Constructortwoptag', 'CR')">
                                            <p style="margin: 0;">2nd</p>
                                            <img id="Constructortwoimg" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Constructortwoptag" style="margin: 5px 0 0 0;">Constructor 2</p>
                                        </div>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'Constructorthreeimg', 'Constructorthreeptag', 'CR')">
                                            <p style="margin: 0;">3rd</p>
                                            <img id="Constructorthreeimg" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="Constructorthreeptag" style="margin: 5px 0 0 0;">Constructor 3</p>
                                        </div>
                                    </div>
                                    <hr />

                                    <div id="ThirdDiv" style="flex-direction: row; display: flex; align-items: center; margin: 20px 20px 0 0;">
                                        <h6 style="margin-right: 20px; white-space: nowrap;">Fastest Lap</h6>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'FastestLapImage', 'FastestLapptag', 'DR')">
                                            <img name="ThirdGrid" id="FastestLapImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="FastestLapptag" style="margin: 5px 0 0 0;"></p>
                                        </div>

                                        <h6 style="margin-right: 20px; white-space: nowrap;">Pole position</h6>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'PolePositionImage', 'PolePositionptag', 'DR')">
                                            <img name="ThirdGrid" id="PolePositionImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="PolePositionptag" style="margin: 5px 0 0 0;"></p>
                                        </div>

                                        <h6 style="margin-right: 20px; white-space: nowrap;">Most Places Gained</h6>
                                        <div style="cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;" class="avatar avatar-xl me-5" ondragover="allowDrop(event)" ondrop="dropFunction(event, 'MostPlacesGainedImage', 'MostPlacesGainedptag', 'DR')">
                                            <img name="ThirdGrid" id="MostPlacesGainedImage" src="../assets/images/Formula1/Dashboard/PlusImage.png" alt="" srcset="">
                                            <p id="MostPlacesGainedptag" style="margin: 5px 0 0 0;"></p>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div id="RightsideDiv" style="display: none;" class="col-6">
                            <div class="card" style="height: 600px;">
                                <div style="overflow: scroll;" class="card-body">

                                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                                        <li class="nav-item" role="presentation">
                                            <a class="nav-link active" id="home-tab" data-bs-toggle="tab" href="#home"
                                                role="tab" aria-controls="home" aria-selected="true"><b>Drivers</b></a>
                                        </li>
                                        <li class="nav-item" role="presentation">
                                            <a class="nav-link" id="profile-tab" data-bs-toggle="tab" href="#profile"
                                                role="tab" aria-controls="profile" aria-selected="false"><b>Constructors</b></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content" id="myTabContent">
                                        <div class="tab-pane fade show active" id="home" role="tabpanel"
                                            aria-labelledby="home-tab">
                                            <br />
                                            <div style='display: flex; flex-wrap: wrap;' class='card-content pb-4'>
                                                <asp:PlaceHolder runat="server" ID="DriversMasterDetails"></asp:PlaceHolder>
                                            </div>
                                        </div>
                                        <div class="tab-pane fade" id="profile" role="tabpanel"
                                            aria-labelledby="profile-tab">
                                            <br />
                                            <div style='display: flex; flex-wrap: wrap;' class='card-content pb-4'>
                                                <asp:PlaceHolder runat="server" ID="ConstructorsMasterDetails"></asp:PlaceHolder>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <%--Extract From API--%>
    <div class="modal fade text-left" id="success1" tabindex="-1" style="z-index: 9999;"
        role="dialog" aria-labelledby="myModalLabel1101"
        aria-hidden="true" data-bs-backdrop="false">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg"
            role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger">
                    <h5 class="modal-title white" id="myModalLabel1101">Extract Race Result List From API to Database(Year Wise)
                    </h5>
                    <button type="button" class="close"
                        data-bs-dismiss="modal" aria-label="Close">
                        <i data-feather="x"></i>
                    </button>
                </div>
                <div class="modal-body" style="font-weight: bold;">
                    <b>Note : Proceeding with this action will delete existing records from the database and replace them with the newly extracted record.</b>
                </div>
                <div class="modal-footer">
                    <button type="button"
                        class="btn btn-light-secondary"
                        data-bs-dismiss="modal">
                        <i class="bx bx-x d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Close</span>
                    </button>

                    <button type="button" class="btn btn-danger ml-1"
                        data-bs-dismiss="modal" onclick="GettingRaceResultValues()">
                        <i class="bx bx-check d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Accept</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <%--Extract From API--%>
    <script>                
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

        function GettingRaceName(e, RaceList) {

            var Year = document.getElementById(e.id).value;
            document.getElementById(RaceList).innerHTML = "";

            if (Year != "") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../Formula1/RaceResult.aspx/GettingRaceName",
                    data: "{'Year':'" + Year + "'}",
                    dataType: "json",
                    success: function (data) {
                        $("#" + RaceList).html(data.d);
                    },
                    error: function (result) {
                        CommonErrormsg('error', 'Error in Ajax');
                        return;
                    }
                });
            }
        }

        function GettingSprintEntry(RaceId, YearId) {

            var Year = document.getElementById(YearId).value;
            var Round = document.getElementById(RaceId).value;

            if (Year != "") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../Formula1/RaceResult.aspx/GettingSprintEntry",
                    data: "{'Year':'" + Year + "', 'Round':'" + Round + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "Sprint Entry") {
                            document.getElementById("SprintEntryDiv").style.display = "block";
                            document.getElementById("ContentPlaceHolder1_Dummyvalue").value = "Sprint";
                        }
                        else {
                            document.getElementById("SprintEntryDiv").style.display = "none";
                        }
                    },
                    error: function (result) {
                        CommonErrormsg('error', 'Error in Ajax');
                        return;
                    }
                });
            }
        }

        function AddResultManually() {
            $('#cover-spin').show(0);

            document.getElementById('RightsideDiv').style.display = 'block';
            document.getElementById('ThreeButtons').style.display = 'block';

            setTimeout(function () {
                document.getElementById("cover-spin").style.display = "none";
            }, 1000);
        }

        //Saving all the race entry data
        function RaceResultSave() {
            let SessionUserName = localStorage.getItem("UserName");

            var Driver1 = document.getElementById("Driveroneptag").innerText, Driver2 = document.getElementById("Drivertwoptag").innerText,
                Driver3 = document.getElementById("Driverthreeptag").innerText, Driver4 = document.getElementById("Driverfourptag").innerText,
                Driver5 = document.getElementById("Driverfiveptag").innerText, Driver6 = document.getElementById("Driversixptag").innerText,
                SprintDriver1 = document.getElementById("SprintDriveroneptag").innerText, SprintDriver2 = document.getElementById("SprintDrivertwoptag").innerText,
                SprintDriver3 = document.getElementById("SprintDriverthreeptag").innerText,
                Constructor1 = document.getElementById("Constructoroneptag").innerText, Constructor2 = document.getElementById("Constructortwoptag").innerText,
                Constructor3 = document.getElementById("Constructorthreeptag").innerText, FastestLap = document.getElementById("FastestLapptag").innerText,
                PolePosition = document.getElementById("PolePositionptag").innerText, MostPlacedGained = document.getElementById("MostPlacesGainedptag").innerText,
                Year = document.getElementById("ContentPlaceHolder1_YearValue").value, Round = document.getElementById("ContentPlaceHolder1_RaceValue").value;

            if (Year == "" || Round == "") {
                CommonErrormsg("error", "Kindly Fill the Year & Race Name.");
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
                RaceEntryFields.DataSprintDriver1Img = document.getElementById("SprintDriveroneImage").src;
                RaceEntryFields.DataSprintDriver2Img = document.getElementById("SprintDrivertwoImage").src;
                RaceEntryFields.DataSprintDriver3Img = document.getElementById("SprintDriverthreeImage").src;
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
                    url: "../Formula1/RaceResult.aspx/RaceResultInsertData",
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

        function CheckingtheRaceResult() {

            var Year = document.getElementById("ContentPlaceHolder1_YearValue").value, Round = document.getElementById("ContentPlaceHolder1_RaceValue").value;

            if (Year != "" && Round != "" && Year != null && Round != null) {
                $('#cover-spin').show(0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../Formula1/RaceResult.aspx/CheckingtheRaceResult",
                    data: "{'Year':'" + Year + "', 'Round':'" + Round + "'}",
                    dataType: "json",
                    success: function (data) {
                        $.each(data.d, function (key, ListValue) {
                            if (ListValue.ErrorMessage != "" && ListValue.ErrorMessage != null) {
                                CommonErrormsg('error', ListValue.ErrorMessage);
                            }
                            else {
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
                            }
                        });

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
                CommonErrormsg("error", "Kindly Fill the Year & Race Name.");
                return;
            }
        }

        function GettingRaceResultValues() {
            var Year = document.getElementById("ContentPlaceHolder1_YearValue").value, Round = document.getElementById("ContentPlaceHolder1_RaceValue").value,
                Sprint = document.getElementById("ContentPlaceHolder1_Dummyvalue").value;
            let SessionUserName = localStorage.getItem("UserName");

            if (Year != "" && Round != "") {
                $('#cover-spin').show(0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../Formula1/RaceResult.aspx/GettingRaceResultValuesFromAPI",
                    data: "{'UserID':'" + SessionUserName + "', 'Year':'" + Year + "', 'Race':'" + Round + "', 'Sprint':'" + Sprint + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "Success") {
                            CommonErrormsg('success', 'Extraction Success.');
                            CheckingtheRaceResult();
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
                CommonErrormsg("error", "Kindly Fill the Year & Race Name.");
            }
        }
    </script>
</asp:Content>


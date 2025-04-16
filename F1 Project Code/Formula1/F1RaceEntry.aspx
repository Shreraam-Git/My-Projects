<%@ Page Title="Race Entry" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="F1RaceEntry.aspx.cs" Inherits="Formula1_F1RaceEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Race Entry</h5>
        </div>
        <div class="page-content">
            <section class="row">
                <div class="col-12 col-lg-12">
                    <div class="row">
                        <div class="col-6">
                            <div class="card" style="height: 600px;">
                                <div style="overflow: scroll;" class="card-body">
                                    <%--pointer-events: none;--%>

                                    <div class="header" style="flex-direction: row; display: flex; gap: 20px; align-items: center;">
                                        <h5>Your Race Entry for the Year / Round - <span contenteditable="true" onblur="TestingNewFunction(this.innerText)" id="YearRoundDataValue"></span></h5>
                                        <div id="RaceEntryButtonsDiv"><asp:PlaceHolder runat="server" ID="ButtonsPlaceholder"></asp:PlaceHolder></div>
                                    </div>

                                    <script>
                                        //Dummy
                                        function TestingNewFunction(e) {

                                            var Spliting = e.split('/');
                                            var Year = Spliting[0].trim(), Round = Spliting[1].trim();
                                            
                                            if (parseInt(Round) > 19) {
                                                alert("Please enter race data for the previous round to test the logic");
                                                document.getElementById("YearRoundDataValue").innerText = "2024 / 19";
                                                return;
                                            }

                                            $.ajax({
                                                type: "POST",
                                                contentType: "application/json; charset=utf-8",
                                                url: "../Formula1/F1RaceEntry.aspx/TestingNewFunction",
                                                data: "{'Year':'" + Year + "', 'Round':'" + Round + "'}",
                                                dataType: "json",
                                                success: function (data) {
                                                    document.getElementById("ContentPlaceHolder1_Dummyvalue").value = data.d;

                                                    if (data.d == "Sprint Entry") { document.getElementById("ContentPlaceHolder1_SprintEntryDiv").style.display = "block"; }
                                                    else { document.getElementById("ContentPlaceHolder1_SprintEntryDiv").style.display = "none"; }
                                                },
                                                error: function (result) {
                                                    CommonErrormsg('error', 'Error in Ajax');
                                                    return;
                                                }
                                            });
                                        }
                                    </script>

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
                                    <div id="SprintEntryDiv" runat="server" style="display: none;">
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

                        <div class="col-6">
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
                                        <li class="nav-item" role="presentation">
                                            <a class="nav-link" id="Help-tab" title="Info" data-bs-toggle="tab" href="#Help"
                                                role="tab" aria-controls="Help" aria-selected="false"><b><i class="bi bi-info-circle-fill"></i></b></a>
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
                                        <div class="tab-pane fade" id="Help" role="tabpanel"
                                            aria-labelledby="Help-tab">
                                            <br />
                                            <video width="700" height="350" controls="controls" autoplay="autoplay" muted="muted">
                                                <source src="../assets/images/Formula1/Information/How to Make Race Entry .mp4" type="video/mp4">
                                                Your browser does not support HTML video.
                                            </video>
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
    <script src="../Formula1JS/RaceEntryDetails.js"></script>
    <script>        
        document.addEventListener('DOMContentLoaded', () => {
            setTimeout(function () {
                CheckingtheRaceEntry();
            }, 1000);
        });
    </script>
</asp:Content>


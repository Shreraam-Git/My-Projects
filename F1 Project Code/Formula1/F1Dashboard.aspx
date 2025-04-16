<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="F1Dashboard.aspx.cs" Inherits="Formula1_F1Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .table-container {
            overflow-y: auto;
            overflow-x: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            padding: 6px 10px 0 10px;
            text-align: center;
            color: black;
            white-space: nowrap;
        }

        td {
            background-color: transparent;
            font-size: 17px;
        }

        th {
            border-bottom: 1px solid black;
            background-color: transparent;
            font-weight: bold;
            color: black;
            text-align: center !important;
            font-size: 18px;
        }

        tr {
            background-color: #f9f9f9;
        }

        .multi-line {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin: 0 10px;
        }

        .Same-line {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
        }
    </style>
    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-content">
            <section class="row">
                <div class="col-12 col-lg-12">
                    <div class="row">
                        <%--Common For All--%>
                        <div id="RaceResultDiv" class="col-7">
                            <div class="card">
                                <div class="card-body">
                                    <div class="header">
                                        <h5>Race Results for Year / Previous Round <span id="RaceCount"></span></h5>
                                    </div>
                                    <hr />
                                    <div id="FirstDiv">
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
                                                <a class="nav-link" id="PolePosition-tab" data-bs-toggle="tab" href="#PolePosition"
                                                    role="tab" aria-controls="PolePosition" aria-selected="false"><b>Pole Position</b></a>
                                            </li>
                                            <li class="nav-item" role="presentation">
                                                <a class="nav-link" id="FLMPG-tab" data-bs-toggle="tab" href="#FLMPG"
                                                    role="tab" aria-controls="FLMPG" aria-selected="false"><b>FL & MPG</b></a>
                                            </li>
                                            <li class="nav-item" role="presentation">
                                                <a class="nav-link" id="SprintEntry-tab" data-bs-toggle="tab" href="#SprintEntry"
                                                    role="tab" aria-controls="SprintEntry" aria-selected="false"><b>Sprint Entry</b></a>
                                            </li>
                                            <li class="nav-item" role="presentation">
                                                <a class="nav-link" id="LadderChallenge-tab" data-bs-toggle="tab" href="#LadderChallenge"
                                                    role="tab" aria-controls="LadderChallenge" aria-selected="false"><b>Ladder Challenge</b></a>
                                            </li>
                                        </ul>
                                        <div class="tab-content" id="myTabContent">
                                            <div class="tab-pane fade show active" id="home" role="tabpanel"
                                                aria-labelledby="home-tab">
                                                <br />
                                                <div class='card-content pb-4'>
                                                    <div class="table-container">
                                                        <table id="RacePredictionDetailsTable">
                                                            <thead>
                                                                <tr>
                                                                    <th>Order</th>
                                                                    <th>Your Entry</th>
                                                                    <th>Race Result</th>
                                                                    <th>Status</th>
                                                                    <th>Points</th>
                                                                    <th>Against the Grain</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="RacePredictionDriverDetailsTable">
                                                            </tbody>
                                                        </table>
                                                        <hr />
                                                        <h4 style="margin-top: 40px; text-align: center;">Random drivers prediction out of top 6 drivers : <span style="background-color: green; color: white; height: 25px; width: 25px; border-radius: 50%; display: inline-block;" id="DriverRandomOrder"></span></h4>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane fade" id="profile" role="tabpanel"
                                                aria-labelledby="profile-tab">
                                                <br />
                                                <div class='card-content pb-4'>
                                                    <div class="table-container">
                                                        <table id="ConstructorPredictionDetailsTable">
                                                            <thead>
                                                                <tr>
                                                                    <th>Order</th>
                                                                    <th>Your Entry</th>
                                                                    <th>Race Result</th>
                                                                    <th>Status</th>
                                                                    <th>Points</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="ConstructorPredictionDriverDetailsTable">
                                                            </tbody>
                                                        </table>
                                                        <hr />
                                                        <h4 style="margin-top: 40px; text-align: center;">Random constructors prediction out of top 3 constructors : <span style="background-color: green; color: white; height: 25px; width: 25px; border-radius: 50%; display: inline-block;" id="ConstructorRandomOrder"></span></h4>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane fade" id="PolePosition" role="tabpanel"
                                                aria-labelledby="PolePosition-tab">
                                                <br />
                                                <div class='card-content pb-4'>
                                                    <div class="table-container">
                                                        <table id="PolePositionPredictionDetailsTable">
                                                            <thead>
                                                                <tr>
                                                                    <th>Pole Position</th>
                                                                    <th>Your Entry</th>
                                                                    <th>Race Result</th>
                                                                    <th>Status</th>
                                                                    <th>Points</th>
                                                                    <th>Against the Grain</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="PolePositionPredictionDriverDetailsTable">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane fade" id="FLMPG" role="tabpanel"
                                                aria-labelledby="FLMPG-tab">
                                                <br />
                                                <div class='card-content pb-4'>
                                                    <div class="table-container">
                                                        <table id="ConstraintsPredictionDetailsTable">
                                                            <thead>
                                                                <tr>
                                                                    <th>Constraints</th>
                                                                    <th>Your Entry</th>
                                                                    <th>Race Result</th>
                                                                    <th>Status</th>
                                                                    <th>Points</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="ConstraintsPredictionDriverDetailsTable">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane fade" id="SprintEntry" role="tabpanel"
                                                aria-labelledby="SprintEntry-tab">
                                                <br />
                                                <div class='card-content pb-4'>
                                                    <div class="table-container">
                                                        <table id="SprintEntryPredictionDetailsTable">
                                                            <thead>
                                                                <tr>
                                                                    <th>Sprint Entry</th>
                                                                    <th>Your Entry</th>
                                                                    <th>Race Result</th>
                                                                    <th>Status</th>
                                                                    <th>Points</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="SprintEntryPredictionDriverDetailsTable">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane fade" id="LadderChallenge" role="tabpanel"
                                                aria-labelledby="LadderChallenge-tab">
                                                <br />
                                                <div class='card-content pb-4'>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="text-align: center; display: none;" id="SecondDiv">
                                        <img style="height: 400px; width: 600px;" src="../assets/images/Formula1/Information/49e58d5922019b8ec4642a2e2b9291c2.png" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--Common For All--%>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <script>        
        function CalculatingtheresultPoints() {

            var UserName = localStorage.getItem("OriginalUserName");

            if (UserName != "") {

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../Formula1/F1Dashboard.aspx/CalculatingtheresultPoints",
                    data: "{'UserName':'" + UserName + "'}",
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

                            if (data.SprintEntryTable != "") {
                                $("#SprintEntryPredictionDriverDetailsTable").html(data.SprintEntryTable);
                            }
                            else {
                                document.getElementById("SprintEntry-tab").style.display = "none";
                            }

                            if (data.LadderChallengeEntryTable != "") {

                            }
                            else {
                                document.getElementById("LadderChallenge-tab").style.display = "none";
                            }

                            document.getElementById("RaceCount").innerText = data.DbTitle;
                            document.getElementById("FirstDiv").style.display = "block";
                            document.getElementById("SecondDiv").style.display = "none";
                        }
                        else {
                            if (data.Message == "Not given any race entries") {
                                document.getElementById("FirstDiv").style.display = "none";
                                document.getElementById("SecondDiv").style.display = "block";
                            } else {                                
                                CommonErrormsg('error', data.Message);                                
                            }                            
                        }

                        ChangetheTableColors(document.getElementById("toggle-dark").checked);
                    },
                    error: function (result) {
                        CommonErrormsg('error', 'Error in Ajax');
                        return;
                    }
                });
            }
            else {
                CommonErrormsg("error", "Kindly Fill all the Fields");
            }
        }
    </script>
</asp:Content>

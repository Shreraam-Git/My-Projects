<%@ Page Title="Ladder Challenge" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="LadderChallenge.aspx.cs" Inherits="Formula1_LadderChallenge" %>

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
            padding: 6px 1px 0 6px;
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
        <div class="page-heading">
            <h5 style="color: white;">Ladder Matches</h5>
        </div>
        <div class="page-content">
            <div id="DriverConstructorSection">
                <section class="section">
                    <div class="row">
                        <div class="col-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-body">
                                    <div style="height: 1000px !important;" class="table-container">
                                        <h4 style="text-align: right;">No of Challenges Left : <span style="background-color: green; color: white; height: 25px; width: 25px; text-align: center; border-radius: 50%; display: inline-block;" id="NoofChallangesLeft">0</span></h4>
                                        <hr />
                                        <table id="ladderChallangeDetailsTable">
                                            <thead>
                                                <tr>
                                                    <th>#Rank</th>
                                                    <th>Name</th>
                                                    <th>Team</th>
                                                    <th>Points</th>
                                                    <th>Challenges</th>
                                                </tr>
                                            </thead>
                                            <tbody id="ladderChallangeDataDetailsTable">
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-6 col-lg-6">
                            <div class="card" style="height: 500px !important;">
                                <div class="card-body" style="overflow: scroll;">
                                    <h4 style="text-align: left;">Ladder History</h4>
                                    <hr />
                                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                                        <li class="nav-item" role="presentation">
                                            <a class="nav-link active" id="home-tab" data-bs-toggle="tab" href="#home"
                                                role="tab" aria-controls="home" aria-selected="true"><b>Challenged</b></a>
                                        </li>
                                        <li class="nav-item" role="presentation">
                                            <a class="nav-link" id="profile-tab" data-bs-toggle="tab" href="#profile"
                                                role="tab" aria-controls="profile" aria-selected="false"><b>Challenged By</b></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content" id="myTabContent">
                                        <div class="tab-pane fade show active" id="home" role="tabpanel"
                                            aria-labelledby="home-tab">
                                            <br />
                                            <div class="table-container">
                                                <table id="ChallangedDetailsTable">
                                                    <thead>
                                                        <tr>
                                                            <th>Challanged</th>
                                                            <th>Race</th>
                                                            <th>Result</th>
                                                            <th>Points</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="ChallangedDataDetailsTable">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="tab-pane fade" id="profile" role="tabpanel"
                                            aria-labelledby="profile-tab">
                                            <br />
                                            <div class="table-container">
                                                <table id="ChallangedByDetailsTable">
                                                    <thead>
                                                        <tr>
                                                            <th>Challanged By</th>
                                                            <th>Race</th>
                                                            <th>Result</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="ChallangedByDataDetailsTable">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card" style="height: 510px;">
                                <div class="card-body">
                                    <h4 style="text-align: left;">All Challenges</h4>
                                    <hr />
                                    <div class="table-container">
                                        <table id="AllChallangeDetailsTable">
                                            <thead>
                                                <tr>
                                                    <th>User Name</th>
                                                    <th>Challenged</th>
                                                    <th>Result</th>
                                                </tr>
                                            </thead>
                                            <tbody id="AllChallangeDataDetailsTable">
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
    <script src="../Formula1JS/RacePrediction.js"></script>
</asp:Content>


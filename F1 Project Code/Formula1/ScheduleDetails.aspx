<%@ Page Title="Schedule Master" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="ScheduleDetails.aspx.cs" Inherits="Formula1_ScheduleDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .round-div {
            width: 290px; /* Adjust as needed */
            height: 200px; /* Adjust as needed */
            border-radius: 0; /* Remove rounded corners */
            overflow: hidden;
            display: flex;
            justify-content: center;
            align-items: center;
            border: 2px solid #DDE3E9; /* Optional: border to see the rectangle div */
            position: relative;
            max-width: 100%;
        }

            .round-div img {
                width: 100%;
                height: auto;
                object-fit: cover;
            }

            .round-div input[type="file"] {
                opacity: 0;
                position: absolute;
                width: 100%;
                height: 100%;
                cursor: pointer;
            }

        .upload-icon {
            font-size: 20px;
            color: #555;
            text-align: center;
            z-index: 1;
        }

        .zoomed {
            transform: scale(2.5);
            background-color: Background;
            z-index: 9999;
        }
    </style>
    <style>
        .divcontainer {
            display: flex;
            align-items: center;
        }

        .divdate {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-right: 130px;
        }

        .divday {
            font-size: 1.5rem;
            font-weight: bold;
        }

        .divmonth {
            background-color: black;
            color: white;
            padding: 2px 8px;
            border-radius: 5px;
            font-size: 1rem;
        }

        .flag img {
            width: 50px;
            height: 35px;
            border-radius: 3px;
        }
    </style>
    <style>
        .countdown-container {
            display: flex;
            align-items: center;
            padding: 20px;
            background-color: #1b5e20;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            justify-content: center;
        }

        .countdown-header {
            font-size: 18px;
            color: white;
        }

        .countdown-timer {
            display: flex;
            align-items: center;
            margin-right: 20px;
        }

        .time-box {
            text-align: center;
            margin: 0 10px;
        }

        .countdowntime {
            display: block;
            font-size: 36px;
            color: white;
        }

        .label {
            font-size: 14px;
            color: white;
        }
    </style>
    <style>
        .clock {
            width: 10rem;
            height: 10rem;
            border: 1px solid gold;
            border-radius: 50%;
            position: relative;
            padding: 0.7rem;
            -webkit-box-shadow: 0 20px 30px rgba(104,75,106,0.65);
            -moz-box-shadow: 0 20px 30px rgba(104,75,106,0.65);
            box-shadow: 0 20px 30px rgba(104,75,106,0.65);
            background: #545271;
            background-color: gold;
        }

        .outer-clock-face {
            position: relative;
            width: 100%;
            height: 100%;
            border-radius: 100%;
            background: #fefefc;
            -webkit-box-shadow: 0 20px 10px rgba(62,47,63,0.45);
            -moz-box-shadow: 0 20px 10px rgba(62,47,63,0.45);
            box-shadow: 0 20px 10px rgba(62,47,63,0.45);
            overflow: hidden;
        }

            .outer-clock-face::after {
                -webkit-transform: rotate(90deg);
                -moz-transform: rotate(90deg);
                transform: rotate(90deg)
            }

            .outer-clock-face::before,
            .outer-clock-face::after,
            .outer-clock-face .marking {
                content: '';
                position: absolute;
                width: 6px;
                height: 100%;
                background: #b8b8c5;
                z-index: 0;
                left: 49%;
            }

            .outer-clock-face .marking {
                background: #bdbdcb;
                width: 3px;
            }

                .outer-clock-face .marking.marking-one {
                    -webkit-transform: rotate(30deg);
                    -moz-transform: rotate(30deg);
                    transform: rotate(30deg)
                }

                .outer-clock-face .marking.marking-two {
                    -webkit-transform: rotate(60deg);
                    -moz-transform: rotate(60deg);
                    transform: rotate(60deg)
                }

                .outer-clock-face .marking.marking-three {
                    -webkit-transform: rotate(120deg);
                    -moz-transform: rotate(120deg);
                    transform: rotate(120deg)
                }

                .outer-clock-face .marking.marking-four {
                    -webkit-transform: rotate(150deg);
                    -moz-transform: rotate(150deg);
                    transform: rotate(150deg)
                }

        .inner-clock-face {
            position: absolute;
            top: 10%;
            left: 10%;
            width: 80%;
            height: 80%;
            background: #fefefc;
            -webkit-border-radius: 100%;
            -moz-border-radius: 100%;
            border-radius: 100%;
            z-index: 1;
        }

            .inner-clock-face::before {
                content: '';
                position: absolute;
                top: 50%;
                left: 50%;
                width: 12px;
                height: 12px;
                border-radius: 18px;
                margin-left: -6px;
                margin-top: -6px;
                background: #4d4b63;
                z-index: 11;
            }

        .hand {
            width: 50%;
            right: 50%;
            height: 6px;
            background: #61afff;
            position: absolute;
            top: 50%;
            border-radius: 6px;
            transform-origin: 100%;
            transform: rotate(90deg);
            transition-timing-function: cubic-bezier(0.1, 2.7, 0.58, 1);
        }

            .hand.hour-hand {
                width: 25%;
                height: 6px;
                z-index: 3;
            }

            .hand.min-hand {
                height: 6px;
                z-index: 10;
                width: 40%;
            }

            .hand.second-hand {
                background: #ff5e5e;
                height: 3px;
                width: 42%;
            }
    </style>
    <style>
        .tabs {
            display: flex;
            justify-content: flex-start;
            padding: 10px;
            background-color: #fff;
            border-bottom: 1px solid #ddd;
        }

        .tab {
            margin-right: 20px;
            padding: 10px;
            cursor: pointer;
            font-weight: bold;
        }

            .tab.active {
                color: red;
                border-bottom: 2px solid red;
            }

        .schedule {
            padding: 10px;
            margin-bottom: -50px;
        }

        .event {
            display: flex;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            margin-bottom: 15px;
            padding: 15px;
        }

        .tabdate {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            margin-right: 20px;
            font-weight: bold;
        }

        .tabday {
            font-size: 24px;
        }

        .tabmonth {
            background-color: black;
            border-radius: 8px;
            padding: 5px 10px;
            margin-top: 5px;
            color: white;
        }

        .details {
            display: flex;
            flex-direction: column;
            justify-content: center;
        }

        .title {
            font-size: 20px;
            margin-bottom: 5px;
        }

        .tabtime {
            color: #666;
            font-size: 18px;
        }
    </style>

    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Schedule Master</h5>
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
                                                    <label for="basicInput"><b>Year / Race</b></label>
                                                </div>
                                                <div class="col-lg-8 col-7">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="YearValue" runat="server" TextMode="Number" AutoCompleteType="Disabled" data-MaxLength="4" oninput="javascript: if(this.value < 0){this.value = 0;}; this.value=this.value.slice(0,this.dataset.maxlength);" class="form-control" placeholder="Set Year"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                        
                                        <div class="buttons">

                                            <button class="btn btn-primary" type="button" onclick="GettingScheduleDataFromDB()">Search</button>

                                            <div class="modal-success me-1 mb-1 d-inline-block">
                                                <!-- Button trigger for Success theme modal -->
                                                <button type="button" style="background-color: red;" class="btn btn-danger" id="ExtractFromAPI"
                                                    data-bs-toggle="modal" data-bs-target="#success1">
                                                    Extract From API
                                                </button>
                                            </div>
                                            <%--<div class="modal-success me-1 mb-1 d-inline-block">
                                                <!-- Button trigger for Success theme modal -->
                                                <button type="button" style="background-color: green;" class="btn btn-success"
                                                    data-bs-toggle="modal" data-bs-target="#SummaryDetailsPopup">
                                                    Add Manually
                                                </button>
                                            </div>--%>
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
                    <div class="row" id="ScheduledataAppend">
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
                    <h5 class="modal-title white" id="myModalLabel1101">Extract Schedule List From API to Database(Year Wise)
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
                        data-bs-dismiss="modal" onclick="GettingScheduleValues()">
                        <i class="bx bx-check d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Accept</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <%--Extract From API--%>

    <%--Add Manually--%>
    <div class="container demo" style="z-index: 9999;">
        <!-- Modal -->
        <div class="modal right fade" id="SummaryDetailsPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" data-bs-backdrop="false" aria-hidden="true" data-keyboard="false">
            <div class="modal-dialog custom-modal-dialog modal-dialog-scrollable" role="document">
                <div class="modal-content">

                    <div class="modal-header" style="background-color: green;">
                        <h4 class="modal-title" id="myModalLabel2" style="color: white;">Race Information</h4>
                        <button type="button" id="SideModelClose" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close">
                        </button>
                    </div>
                   
                    <section class="section">
                        <div class="card">
                            <div class="card-content">
                                <div class="card-body">
                                    <div class="form form-vertical">
                                        <div class="form-body">
                                            <div class="row" id="RaceTimingDataAppend">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
    </div>
    <%--Add Manually--%>

    <script src="../Formula1JS/ScheduleDetails.js"></script>
    <script>
        function SaveDeadLineTime(UpdateId, DocId) {

            var DateTimeValue = document.getElementById(DocId).value;

            if (DateTimeValue != "") {
                $('#cover-spin').show(0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../Formula1/ScheduleDetails.aspx/SaveDeadLineTime",
                    data: "{'UpdateId':'" + UpdateId + "', 'DateTimeValue':'" + DateTimeValue + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "Success") {
                            CommonErrormsg('success', 'Updated Successfully.');                           
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
    </script>
</asp:Content>


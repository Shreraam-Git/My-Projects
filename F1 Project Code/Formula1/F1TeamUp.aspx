<%@ Page Title="Team" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="F1TeamUp.aspx.cs" Inherits="Formula1_F1TeamUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .table-container {
            overflow-y: auto;
            overflow-x: auto;
            height: 500px !important;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            padding: 6px 1px 0 6px;
            text-align: left;
            color: black;
            white-space: nowrap;
        }

        td {
            background-color: #E6EBF2;
            font-size: 17px;
            padding: 10px;
        }

        th {
            border-bottom: 1px solid black;
            background-color: #E6EBF2;
            font-weight: bold;
            color: black;
            text-align: left !important;
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
    <style>
        .round-div input[type="file"] {
            opacity: 0;
            position: absolute;
            width: 100%;
            height: 100%;
            cursor: pointer;
        }

        .round-div img {
            width: 100%;
            height: auto;
            object-fit: cover;
        }

        .round-div {
            width: 200px; /* Adjust as needed */
            height: 200px; /* Adjust as needed */
            border-radius: 50%;
            overflow: hidden;
            display: flex;
            justify-content: center;
            align-items: center;
            border: 2px solid #DDE3E9; /* Optional: border to see the round div */
            position: relative;
        }
    </style>
    <style>
        .MemberClass {
            font-weight: bolder;
            padding: 1px 7px 1px 7px;
            height: fit-content;
            width: fit-content;
            border: 1px solid black;
        }
    </style>
    <style>
        .approval-method-section {
            margin-top: 20px;
        }

            .approval-method-section h3 {
                font-size: 1.2em;
            }

        .toggle-section {
            display: flex;
            gap: 20px;
            align-items: center;
            margin: 10px 0;
        }

        .switch {
            position: relative;
            display: inline-block;
            width: 40px;
            height: 20px;
        }

            .switch input {
                display: none;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            transition: 0.4s;
            border-radius: 15px;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 16px;
                width: 16px;
                left: 2px;
                bottom: 2px;
                background-color: white;
                transition: 0.4s;
                border-radius: 50%;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

            input:checked + .slider:before {
                transform: translateX(20px);
            }
    </style>
    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Team</h5>
        </div>
        <div class="page-content">
            <div id="DriverConstructorSection">
                <section class="section">
                    <div class="row">
                        <div class="col-12 col-md-6 col-lg-8">
                            <div class="card">
                                <div class="card-body">
                                    <div id="AppendButtons" class="buttons">
                                    </div>
                                    <div class="table-container">
                                        <table id="CreatedTeamsDetailsTable">
                                            <thead>
                                                <tr>
                                                    <th>Select</th>
                                                    <th>Team</th>
                                                    <th>Members</th>
                                                    <th>Approval Method</th>
                                                </tr>
                                            </thead>
                                            <tbody id="CreatedTeamsDetailsTableBody">
                                                <asp:PlaceHolder runat="server" ID="CreatedTeamsTableBodyPH"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="SecDivPlaceHolder" class="col-12 col-md-6 col-lg-4">
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>

    <!--primary theme Modal -->
    <div class="modal fade text-left" id="primary" tabindex="-1" style="z-index: 9999"
        role="dialog" aria-labelledby="myModalLabel160"
        aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-full"
            role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title white" id="myModalLabel160">Approvals
                    </h5>
                    <button type="button" class="close"
                        data-bs-dismiss="modal" aria-label="Close">
                        <i data-feather="x"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="table-container">
                        <table>
                            <thead>
                                <tr>
                                    <th>Requested By</th>
                                    <th>Mail Id</th>
                                    <th>Date of Birth</th>
                                    <th>Mobile No</th>
                                    <th>Gender</th>
                                    <th>Country</th>
                                    <th>Request Raised On</th>
                                    <th>Approve</th>
                                    <th>Reject</th>
                                </tr>
                            </thead>
                            <tbody id="ApprovalsPlaceHolder">
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button"
                        class="btn btn-light-secondary"
                        data-bs-dismiss="modal">
                        <i class="bx bx-x d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Close</span>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade text-left" id="success1" tabindex="-1" style="z-index: 9999;"
        role="dialog" aria-labelledby="myModalLabel1101"
        aria-hidden="true" data-bs-backdrop="false">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg"
            role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger">
                    <h5 class="modal-title white" id="myModalLabel1101">Exit From [<span id="TeamNameFromDB"></span>] Team
                    </h5>
                    <button type="button" class="close"
                        data-bs-dismiss="modal" aria-label="Close">
                        <i data-feather="x"></i>
                    </button>
                </div>
                <div class="modal-body" style="font-weight: bold;">
                    <b>Are you certain that you want to step away from the team?</b>
                </div>
                <div class="modal-footer">
                    <button type="button"
                        class="btn btn-light-secondary"
                        data-bs-dismiss="modal">
                        <i class="bx bx-x d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Close</span>
                    </button>

                    <button type="button" class="btn btn-danger ml-1"
                        data-bs-dismiss="modal" onclick="Exittheteam()">
                        <i class="bx bx-check d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Accept</span>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%--Create Team Details--%>
    <div class="modal fade text-left" id="CreateTeamPreview" tabindex="-1" style="z-index: 9999;"
        role="dialog" aria-labelledby="myModalLabel1101234"
        aria-hidden="true" data-bs-backdrop="false">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg"
            role="document">
            <div class="modal-content" style='width: 100%; height: 100%;'>
                <div class="modal-header bg-success">
                    <h5 class="modal-title white" id="myModalLabel1101234">Build Your Team
                    </h5>
                    <button type="button" class="close"
                        data-bs-dismiss="modal" aria-label="Close">
                        <i data-feather="x"></i>
                    </button>
                </div>
                <div style="align-content: center;" class="modal-body">
                    <div style="margin-bottom: 70px;" class="d-flex justify-content-center align-items-center flex-column">
                        <div class="round-div" id="ImageId">
                        </div>
                    </div>
                    <div class="form">
                        <div class="row">
                            <div class="col-md-6 col-12">
                                <div class="form-group mandatory">
                                    <label for="first-name-column" class="form-label">
                                        <b>Team Name</b></label>
                                    <input type="text" name="Name" maxlength="40" id="TeamName" class="form-control" placeholder="Between 1-30 characters" data-parsley-required="true" />
                                </div>
                            </div>
                            <div class="col-md-6 col-12">
                                <div class="form-group">
                                    <label for="first-name-column" class="form-label">
                                        <b>Slogan</b></label>
                                    <input type="text" name="solgan" id="TeamSlogan" class="form-control" placeholder="Welcome!" data-parsley-required="true" />
                                </div>
                            </div>
                            <div class="col-md-6 col-12">
                                <div class="form-group">
                                    <label for="first-name-column" class="form-label">
                                        <b>Admin / Member 1</b></label>
                                    <input type="text" disabled="disabled" runat="server" id="TeamMember1" class="form-control" value="" data-parsley-required="true" />
                                </div>
                            </div>
                            <%--Dummy--%>
                            <div style="display: none;" class="col-md-6 col-12">
                                <div class="form-group">
                                    <label for="last-name-column" class="form-label">
                                        <b>Member 2</b></label>
                                    <input list="UserNameList" onblur="DrpDwnFreeTextCheckFormulaone(this)" type="text" class="form-control" placeholder="Name" data-parsley-required="true" />
                                    <datalist id="UserNameList">
                                    </datalist>
                                </div>
                            </div>
                            <%--Dummy--%>
                            <div class="col-md-6 col-12">
                                <div class="form-group">
                                    <label for="last-name-column" class="form-label">
                                        <b>Member 2</b></label>                                    
                                    <select class="form-select" id="TeamMember2">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6 col-12">
                                <div class="form-group">
                                    <label for="last-name-column" class="form-label">
                                        <b>Member 3</b></label>                                    
                                    <select class="form-select" id="TeamMember3">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6 col-12">
                                <div class="form-group">
                                    <label for="last-name-column" class="form-label">
                                        <b>Member 4</b></label>                                    
                                    <select class="form-select" id="TeamMember4">
                                    </select>
                                </div>
                            </div>
                            <div class="approval-method-section">
                                <h3>APPROVAL METHOD <span style="margin-left: 7px; cursor: pointer;" title="If Auto Approval is enabled, users can join your team instantly. However, if Auto Approval is disabled, users must submit a request to join the team."><i class="bi bi-info-circle-fill"></i></span></h3>
                                <div class="toggle-section">
                                    <span>AUTO APPROVAL</span>
                                    <label class="switch">
                                        <input id="ApprovalType" type="checkbox" checked>
                                        <span class="slider"></span>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="SaveorDeleteButton" class="col-12 d-flex justify-content-end">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="ModelCloseforPreview1"
                        class="btn btn-light-secondary"
                        data-bs-dismiss="modal">
                        <i class="bx bx-x d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Close</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <%--Create Team Details--%>

    <script src="../Formula1JS/F1TeamUp.js"></script>
    <script>document.getElementById("ContentPlaceHolder1_TeamMember1").value = localStorage.getItem("OriginalUserName");</script>
</asp:Content>

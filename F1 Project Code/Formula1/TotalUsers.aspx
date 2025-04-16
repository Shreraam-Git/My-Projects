<%@ Page Title="Users" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="TotalUsers.aspx.cs" Inherits="Formula1_TotalUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .table-container {
            overflow-y: auto;
            overflow-x: auto;
            height: 600px !important;
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
    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Competitors</h5>
        </div>
        <div class="page-content">
            <div id="DriverConstructorSection">
                <section class="section">
                    <div class="row">
                        <div class="col-12 col-md-6 col-lg-12">
                            <div class="card">
                                <div class="card-body">                                    
                                    <div class="table-container">
                                        <table id="CompetitorsDetailsTable">
                                            <thead>
                                                <tr>
                                                    <th>S.No</th>
                                                    <th>User Name</th>
                                                    <th>Team Name</th>
                                                    <th>Email ID</th>
                                                    <th>Date of Birth</th>
                                                    <th>Phone Number</th>
                                                    <th>Gender</th>
                                                    <th>Country</th>
                                                    <th>Subscription Status</th>
                                                    <th>Amount</th>
                                                    <th>Payment Date</th>
                                                    <th>Delete</th>
                                                </tr>
                                            </thead>
                                            <tbody id="CompetitorsDetailsTableBody">
                                                <asp:PlaceHolder runat="server" ID="CompetitorsTableBodyPH"></asp:PlaceHolder>
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

</asp:Content>

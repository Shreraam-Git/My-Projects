<%@ Page Title="Drivers Master" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="DriverDetails.aspx.cs" Inherits="Formula1_DriverDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
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
    </style>

    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Drivers Master</h5>
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
                                                        <asp:TextBox ID="RaceValue" style="display:none;" title="Race Count of the Year" runat="server" TextMode="Number" AutoCompleteType="Disabled" data-MaxLength="4" oninput="javascript: if(this.value < 0){this.value = 0;}; this.value=this.value.slice(0,this.dataset.maxlength);" class="form-control" placeholder="Race Count"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div class="buttons">
                                            <button class="btn btn-primary" type="button" onclick="GettingDriverDataFromDB()">Search</button>

                                            <div class="modal-success me-1 mb-1 d-inline-block">
                                                <!-- Button trigger for Success theme modal -->
                                                <button type="button" style="background-color: red;" class="btn btn-danger" id="ExtractFromAPI"
                                                    data-bs-toggle="modal" data-bs-target="#success1">
                                                    Extract From API
                                                </button>
                                            </div>
                                            <div class="modal-success me-1 mb-1 d-inline-block">
                                                <!-- Button trigger for Success theme modal -->
                                                <button type="button" style="background-color: green;" class="btn btn-success"
                                                    data-bs-toggle="modal" data-bs-target="#SummaryDetailsPopup">
                                                    Add Manually
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
                    <div class="row" id="DriverdataAppend">
                    </div>
                </div>
            </section>
        </div>
    </div>

    <%--Add Manually--%>
    <div class="container demo" style="z-index: 9999;">
        <!-- Modal -->
        <div class="modal right fade" id="SummaryDetailsPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" data-bs-backdrop="false" aria-hidden="true" data-keyboard="false">
            <div class="modal-dialog custom-modal-dialog modal-dialog-scrollable" role="document">
                <div class="modal-content">

                    <div class="modal-header" style="background-color: green;">
                        <h4 class="modal-title" id="myModalLabel2" style="color:white;">Add Drivers Manually</h4>
                        <button type="button" id="SideModelClose" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close">
                        </button>
                    </div>

                    <section class="section">
                        <div class="card">
                            <div class="card-content">
                                <div class="card-body">
                                    <div class="form form-vertical">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="first-name-icon"><b>Year <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox ID="Year" runat="server" TextMode="Number" AutoCompleteType="Disabled" data-MaxLength="4" oninput="javascript: if(this.value < 0){this.value = 0;}; this.value=this.value.slice(0,this.dataset.maxlength);" class="form-control" placeholder="Set Year"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-calendar-month"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="email-id-icon"><b>Driver Code <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" MaxLength="50" AutoCompleteType="Disabled" ID="DriverCode" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-alphabet-uppercase"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="mobile-id-icon"><b>Driver Bio Url <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="DriverBioUrl" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-link"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Driver Number <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="number" class="form-control" MaxLength="10" AutoCompleteType="Disabled" ID="DriverNumber" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-2-circle"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>First Name <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="FirstName" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-person"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Last Name <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="LastName" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-person"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Date Of Birth <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="date" class="form-control" AutoCompleteType="Disabled" ID="DOB" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-cake2"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Nationality <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="Nationality" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-globe"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-12 d-flex justify-content-end">
                                                    <asp:Label runat="server" style="margin-top:5px;" ID="ErrorMsg" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <button type="button" onclick="DriverSave()"
                                                        class="btn btn-primary me-1 mb-1">
                                                        Submit</button>
                                                    <%--<button type="reset"
                                                        class="btn btn-light-secondary me-1 mb-1">
                                                        Reset</button>--%>
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
        </div>
    </div>
    <%--Add Manually--%>

    <%--Extract From API--%>
    <div class="modal fade text-left" id="success1" tabindex="-1" style="z-index: 9999;"
        role="dialog" aria-labelledby="myModalLabel1101"
        aria-hidden="true" data-bs-backdrop="false">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg"
            role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger">
                    <h5 class="modal-title white" id="myModalLabel1101">Extract Drivers List From API to Database(Year Wise)
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
                        data-bs-dismiss="modal" onclick="GettingDriverValues()">
                        <i class="bx bx-check d-block d-sm-none"></i>
                        <span class="d-none d-sm-block">Accept</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <%--Extract From API--%>   

    <script src="../Formula1JS/DriverDetails.js"></script>
</asp:Content>

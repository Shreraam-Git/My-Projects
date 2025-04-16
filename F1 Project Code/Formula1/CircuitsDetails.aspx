<%@ Page Title="Circuits Details" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="CircuitsDetails.aspx.cs" Inherits="Formula1_CircuitsDetails" %>

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

    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Circuits Master</h5>
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
                                            <button class="btn btn-primary" type="button" onclick="GettingCircuitsDataFromDB()">Search</button>

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
                    <div class="row" id="CircuitsdataAppend">
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
                    <h5 class="modal-title white" id="myModalLabel1101">Extract Circuits List From API to Database(Year Wise)
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
                        data-bs-dismiss="modal" onclick="GettingCircuitsValues()">
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
                        <h4 class="modal-title" id="myModalLabel2" style="color: white;">Add Circuits Manually</h4>
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
                                                        <label for="email-id-icon"><b>Circuit Code <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="CircuitCode" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-alphabet-uppercase"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="mobile-id-icon"><b>Circuit Bio Url <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="CircuitBioUrl" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-link"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Circuit Name <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="CircuitName" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-diagram-3"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Circuit Latitude <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="CircuitLatitude" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-geo-fill"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Circuit Longitude <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="CircuitLongitude" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-pin-map-fill"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Locality <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="Locality" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-map"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <div class="form-group has-icon-left">
                                                        <label for="password-id-icon"><b>Country <font color="red">*</font></b></label>
                                                        <div class="position-relative">
                                                            <asp:TextBox type="text" class="form-control" AutoCompleteType="Disabled" ID="Country" placeholder="" runat="server"></asp:TextBox>
                                                            <div class="form-control-icon">
                                                                <i class="bi bi-globe-americas"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-12 d-flex justify-content-end">
                                                    <asp:Label runat="server" Style="margin-top: 5px;" ID="ErrorMsg" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <button type="button" onclick="CircuitsSave()"
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

    <script src="../Formula1JS/CircuitsDetails.js"></script>
</asp:Content>


<%@ Page Title="Race Prediction" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="RacePrediction.aspx.cs" Inherits="Formula1_RacePrediction" %>

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
                                        <div class="col-md-7">
                                            <div class="form-group row align-items-center">
                                                <div class="col-lg-4 col-5">
                                                    <label for="basicInput"><b>Year & Race Name / User Name</b></label>
                                                </div>
                                                <div class="col-lg-8 col-7">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="YearValue" onblur="GettingRaceName(this, 'RoundandRaceNameList')" runat="server" TextMode="Number" AutoCompleteType="Disabled" data-MaxLength="4" oninput="javascript: if(this.value < 0){this.value = 0;}; this.value=this.value.slice(0,this.dataset.maxlength);" class="form-control" placeholder="Set Year"></asp:TextBox>

                                                        <asp:TextBox ID="RaceValue" onblur="DrpDwnFreeTextCheckFormulaone(this);" list="RoundandRaceNameList" title="Race Count / Race Name of the Year" runat="server" AutoCompleteType="Disabled" class="form-control" placeholder="Select Race Name"></asp:TextBox>
                                                        <datalist id="RoundandRaceNameList">
                                                            <asp:PlaceHolder runat="server" ID="RoundandRaceNameListDD"></asp:PlaceHolder>
                                                        </datalist>

                                                        <select class="form-select" id="UserName">                                                            
                                                            <asp:PlaceHolder runat="server" ID="UserNameListDD"></asp:PlaceHolder>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div class="buttons">
                                            <button class="btn btn-primary" type="button" onclick="CalculatingtheresultPoints();">Search</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <div id="DriverConstructorSection" style="display: none;">
                <section class="section">
                    <div class="row">
                        <div class="col-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-body">
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
                            <div class="card">
                                <div class="card-body">
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
                        </div>
                        <div class="col-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-body">
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
                            <div class="card">
                                <div class="card-body">
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
                            <div class="card">
                                <div class="card-body">
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
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
    <script src="../Formula1JS/RacePrediction.js"></script>
</asp:Content>

<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="F1UserProfile.aspx.cs" Inherits="Formula1_F1UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            <section class="section">
                <div class="row">
                    <div class="col-12 col-lg-12" style="width: 50%; margin: auto; position: center; left: 0; right: 0;">
                        <div class="card">
                            <div class="card-content">
                                <div class="card-body">
                                    <h3>User Profile</h3>
                                    <div style="margin-bottom: 30px;" class="d-flex justify-content-center align-items-center flex-column">
                                        <div class="round-div" id="ImageId">
                                        </div>
                                    </div>
                                    <div class="form">
                                        <div class="row">
                                            <div class="col-md-6 col-12">
                                                <div class="form-group">
                                                    <label for="first-name-column" class="form-label">
                                                        <b>User Name</b></label>
                                                    <input type="text" disabled="disabled" name="Name" id="UserName" class="form-control" placeholder="User Name" data-parsley-required="true" />
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-12">
                                                <div class="form-group">
                                                    <label for="first-name-column" class="form-label">
                                                        <b>Email Id</b></label>
                                                    <input type="email" disabled="disabled" name="Mail" id="EmailId" class="form-control" placeholder="Mail Id" data-parsley-required="true" />
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-12">
                                                <div class="form-group mandatory">
                                                    <label for="first-name-column" class="form-label">
                                                        <b>Country Code / Phone</b></label>
                                                    <div class="input-group" style="gap:10px;">
                                                        <div class="col-lg-4 col-5">
                                                            <input type="text" list="CountryCodeList" onblur="DrpDwnFreeTextCheckFormulaone(this); GetValuesFromDb(this.value, 'CountryofOrigin');" disabled="disabled" name="phone" id="CountryCode" maxlength="5" class="form-control" placeholder="" value="" data-parsley-required="true" />
                                                            <datalist id="CountryCodeList">
                                                                <asp:PlaceHolder runat="server" ID="CountryCodeListDD"></asp:PlaceHolder>
                                                            </datalist>
                                                        </div>
                                                        <input type="text" disabled="disabled" name="phone" id="phone" maxlength="10" class="form-control" placeholder="083xxxxxxxxx" value="" data-parsley-required="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-12">
                                                <div class="form-group mandatory">
                                                    <label for="last-name-column" class="form-label">
                                                        <b>Date of Birth</b></label>
                                                    <input type="date" disabled="disabled" name="birthday" id="birthday" class="form-control" placeholder="Your Birthday" data-parsley-required="true" />
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-12">
                                                <div class="form-group mandatory">
                                                    <label for="city-column" class="form-label"><b>Gender</b></label>
                                                    <input list="GenderList" disabled="disabled" name="gender" onblur="DrpDwnFreeTextCheckFormulaone(this)" maxlength="6" id="gender" class="form-control" />
                                                    <datalist id="GenderList">
                                                        <option value="Male">Male</option>
                                                        <option value="Female">Female</option>
                                                    </datalist>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-12">
                                                <div class="form-group mandatory">
                                                    <label for="last-name-column" class="form-label">
                                                        <b>Country of Origin</b></label>
                                                    <input list="CountryofOriginList" type="text" disabled="disabled" id="CountryofOrigin" onblur="DrpDwnFreeTextCheckFormulaone(this); GetValuesFromDb(this.value, 'CountryCode');" class="form-control" placeholder="Select" data-parsley-required="true" />
                                                    <datalist id="CountryofOriginList">
                                                        <asp:PlaceHolder runat="server" ID="OverallCountryList"></asp:PlaceHolder>
                                                    </datalist>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 d-flex justify-content-end">
                                                <button type="button" class="btn btn-primary me-1 mb-1" onclick="FormulaOneDataSave()" title="Save">
                                                    <i style="font-size: 20px;" class="bi bi-floppy2-fill"></i>
                                                </button>
                                                <button
                                                    type="button" title="Edit" onclick="EditUserProfile()"
                                                    class="btn btn-light-secondary me-1 mb-1">
                                                    <i style="font-size: 20px;" class="bi bi-pencil-square"></i>
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
        </div>
    </div>
    <script>                
        function OnloadFunction() {
            GettingUsersData();
            localStorage.removeItem("Edited");
        }
    </script>
    <script src="../Formula1JS/F1UserProfile.js"></script>
</asp:Content>

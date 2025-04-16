<%@ Page Title="Points Details" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="PointsDetails.aspx.cs" Inherits="Formula1_PointsDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Style for the HTML Table--%>    
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
            padding: 6px;
            text-align: center;
            color: black;
            
        }

        td {
            background-color: #E6EBF2;
            font-size: 17px;
        }

        th {
            border-bottom: 1px solid black;
            background-color: #E6EBF2;
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

    <%--Style for the flip clock--%>
    <div class="content-wrapper container" style="opacity: 0.9;">
        <div class="page-heading">
            <h5 style="color: white;">Points Master</h5>
        </div>
        <div class="page-content">
            <section class="row">
                <div class="col-12 col-lg-12">
                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="table-container">
                                        <table id="PointsDetailsDetailsTable">
                                            <thead>
                                                <tr>
                                                    <th>Criteria</th>
                                                    <th>Description</th>
                                                    <th>Points</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="PointsDetailsTablePlaceHolder"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
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
        //Function for Editing the points
        function TableEditbtn(btn, Saveid) {
            document.getElementById(Saveid).style.display = "block";
            document.getElementById(btn.id).style.display = "none";

            var row = btn.closest('tr');
            var inputs = row.querySelectorAll('td:nth-child(3) input');
            inputs.forEach(function (input) {
                input.style.backgroundColor = "#A4A5A5";
                input.removeAttribute('disabled');
            });
        }

        //Function for Saving the points
        function TableSavebtn(e) {
            let SessionUserName = localStorage.getItem("UserName");

            var row = e.closest('tr');

            var SpanValues = row.querySelectorAll('td:nth-child(3) span');
            var inputs = row.querySelectorAll('td:nth-child(3) input');
            var s = 1, i = 1, TableId = "";
            const regex = /(\D+)(\d+)/;
            const match = e.id.match(regex);
            if (match) {
                TableId = match[2];
            }
            //Declaring the array
            var DataList1 = new Array();

            SpanValues.forEach(function (SpanValue) {
                var ColumnName = SpanValue.innerText;
                if (ColumnName == "1st") { ColumnName = "POINTS_1ST"; }
                if (ColumnName == "2st") { ColumnName = "POINTS_2ND"; }
                if (ColumnName == "3st") { ColumnName = "POINTS_3RD"; }
                if (ColumnName == "4st") { ColumnName = "POINTS_4TH"; }
                if (ColumnName == "5st") { ColumnName = "POINTS_5TH"; }
                if (ColumnName == "6st") { ColumnName = "POINTS_6TH"; }
                i = 1;
                inputs.forEach(function (input) {
                    if (s == i) {
                        var RowValue = input.value;
                        var DataFields1 = {};
                        DataFields1.COLUMN_NAME = ColumnName.toUpperCase();
                        DataFields1.ROW_VALUE = RowValue;
                        DataFields1.USER_NAME = SessionUserName;
                        DataFields1.KEY_COLUMN = TableId.trim();
                        DataList1.push(DataFields1);
                    } i++;
                });
                s++;
            });

            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../Formula1/PointsDetails.aspx/TableSavebtnUpdatedata",
                data: JSON.stringify({ DataList1: DataList1 }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Saved") {
                        CommonErrormsg('success', 'Updated Successfully ✅');

                        setTimeout(function () { location.reload(); }, 3000);
                    }
                    else {
                        CommonErrormsg('error', data.d);
                    }

                    document.getElementById("cover-spin").style.display = "none";
                },
                error: function (result) {
                    alert("Error" + result.responseText);
                    document.getElementById("cover-spin").style.display = "none";
                }
            });
        }

        ////Change the dashboard flip clock color based on the light and dark mode
        //function ChangetheFlipClockColor(Values) {
        //    if (Values == true) {
        //        document.getElementById('flipdown').classList.toggle('modification');

        //        const headers = document.getElementsByTagName('th');
        //        for (let i = 0; i < headers.length; i++) {
        //            headers[i].style.border = '1px solid #ddd';
        //        }

        //        const Subheaders = document.getElementsByTagName('td');
        //        for (let i = 0; i < Subheaders.length; i++) {
        //            Subheaders[i].style.border = '1px solid #ddd';
        //        }
        //    }
        //    else {
        //        document.getElementById('flipdown').classList.remove('modification');
        //    }
        //}
    </script>
</asp:Content>

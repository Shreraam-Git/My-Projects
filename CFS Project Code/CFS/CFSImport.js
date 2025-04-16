UserAccessValidation();
function PageLoad() {
    document.getElementById("Year").value = new Date().getFullYear();//Newly Added
    document.getElementById("Month").value = new Date().getMonth() + 1;//Newly Added
    GetDataStructure();//Newly Added
    document.getElementById("DateRangeId").value = "This Month";
    DateRange(document.getElementById("DateRangeId"));
    var url_string = window.location.href;
    var url = new URL(url_string);
    var MainJobNo = url.searchParams.get("MainJobNo");
    if (MainJobNo != null) {
        document.getElementById("MainJobNo").value = MainJobNo;
        HideShowSectionContent('FormSection', 'ReportSection');
        MainTableSearchData();
    }
    var ViewId = url.searchParams.get("ViewId");
    if (ViewId != null) {
        HideShowSectionContent('ViewSection', 'ReportSection');
    }
    var OpenMode = url.searchParams.get("OpenMode");
    if (OpenMode != null) {
        HideShowSectionContent('FormSection', 'ReportSection');
    }
    PageLoadAddingDropDownValues();
    LoadTableColumns();
    EmptyTruckorContainerTableSearchData('');
    LoadedTruckTableSearchData();
}

function DateRange(e) {
    let sdt = "", edt = "", today = "";
    if (e.value == "Today") {
        sdt = new Date().toISOString().slice(0, 10);
        edt = new Date().toISOString().slice(0, 10);
    }
    else if (e.value == "Yesterday") {
        today = new Date();
        sdt = new Date(today.getFullYear(), today.getMonth(), today.getDate()).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), today.getMonth(), today.getDate()).toISOString().slice(0, 10);
    }
    else if (e.value == "This Week") {
        today = new Date();
        sdt = new Date(today.getFullYear(), today.getMonth(), today.getDate() - today.getDay() + 1).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), today.getMonth(), today.getDate() + (7 - today.getDay())).toISOString().slice(0, 10);
    }
    else if (e.value == "Last Week") {
        today = new Date();
        sdt = new Date(today.getFullYear(), today.getMonth(), today.getDate() - today.getDay() - 6).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), today.getMonth(), today.getDate() - today.getDay()).toISOString().slice(0, 10);
    }
    else if (e.value == "This Month") {
        today = new Date();
        sdt = new Date(today.getFullYear(), today.getMonth(), 2).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), today.getMonth() + 1, 1).toISOString().slice(0, 10);

    }
    else if (e.value == "Last Month") {
        today = new Date();
        sdt = new Date(today.getFullYear(), today.getMonth() - 1, 2).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), today.getMonth(), 1).toISOString().slice(0, 10);
    }
    else if (e.value == "This Calendar Year") {
        today = new Date();
        sdt = new Date(today.getFullYear(), 0, 2).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), 12, 1).toISOString().slice(0, 10);
    }
    else if (e.value == "Last Calendar Year") {
        today = new Date();
        sdt = new Date(today.getFullYear() - 1, 0, 2).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear() - 1, 12, 1).toISOString().slice(0, 10);
    }
    else if (e.value == "This Fiscal Year") {
        today = new Date();
        sdt = new Date(today.getFullYear(), 3, 2).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear() + 1, 3, 1).toISOString().slice(0, 10);
    }
    else if (e.value == "Last Fiscal Year") {
        today = new Date();
        sdt = new Date(today.getFullYear() - 1, 3, 2).toISOString().slice(0, 10);
        edt = new Date(today.getFullYear(), 3, 1).toISOString().slice(0, 10);
    }
    else if (e.value == "Custom") {
        return;
    }
    else { return; }

    document.getElementById("StartDate").value = sdt;
    document.getElementById("EndDate").value = edt;
}

function TabBGColorChange(e) {
    var ColorCode = e.value;
    const tab = document.getElementsByClassName("tab")
    for (let i = 0; i < tab.length; i++) {
        tab[i].style.backgroundColor = ColorCode;
    }

    const tabbtn = document.getElementsByClassName("tablinks");
    for (let i = 0; i < tabbtn.length; i++) {
        tabbtn[i].style.backgroundColor = ColorCode; // "#00008B";
    }
}

function TabActiveColorChange(e) {
    var ColorCode = e.value;

    const tabbtn = document.getElementsByClassName("tablinks");
    for (let i = 0; i < tabbtn.length; i++) {
        if (tabbtn[i] == active) {
            tabbtn[i].style.backgroundColor = ColorCode;
        }
    }
}

function TabFontColorChange(e) {
    var ColorCode = e.value;

    const btn = document.getElementsByClassName("tablinks");
    for (let i = 0; i < btn.length; i++) {
        btn[i].style.color = ColorCode; // "#00008B";
    }
}

function BtnBGColorChange(e) {
    var ColorCode = e.value;
    const btn = document.getElementsByClassName("btn btn-success");
    for (let i = 0; i < btn.length; i++) {
        btn[i].style.backgroundColor = ColorCode; // "#00008B";
    }
}

function BtnFontColorChange(e) {
    var ColorCode = e.value;
    const btn = document.getElementsByClassName("btn btn-success");
    for (let i = 0; i < btn.length; i++) {
        btn[i].style.color = ColorCode; // "#00008B";
    }
}

function HTMLTableHeaderColorChange(e) {
    var ColorCode = e.value;
    const col = document.getElementsByTagName("th");
    for (let i = 0; i < col.length; i++) {
        col[i].style.backgroundColor = ColorCode; // "#00008B";
    }
}

function HTMLTableHeaderFontColorChange(e) {
    var ColorCode = e.value;
    const col = document.getElementsByTagName("th");
    for (let i = 0; i < col.length; i++) {
        col[i].style.color = ColorCode; // "#00008B";
    }
}

function HideShowTabContent(type, e) {
    const tabcontent = document.getElementsByClassName("tabcontent");
    for (let i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    const btn = document.getElementsByClassName("tablinks");
    for (let i = 0; i < btn.length; i++) {
        btn[i].style.backgroundColor = document.getElementById("TabBGColorCode").value; // "#00008B";
    }

    document.getElementById(type).style.display = "block";
    document.getElementById(e.id).style.backgroundColor = document.getElementById("TabActiveColorCode").value; // #4682B4";
    document.getElementById("SeaImportMenu").style.backgroundColor = "#4682B4";

    //document.getElementsByClassName("tab")[0].style.backgroundColor = "#4682B4";
    //document.getElementsByClassName("navbar")[0].style.backgroundColor = "#4682B4";
    //document.getElementsByClassName("table-bordered").style.backgroundColor = "#4682B4";
}

function DIVHideOrShowJSFunction(DivId) {
    if (document.getElementById(DivId).style.display == "none") { document.getElementById(DivId).style.display = "block"; }
    else { document.getElementById(DivId).style.display = "none"; }
}

function enableDragAndDrop() {
    var rows = document.querySelectorAll("#SelectedColumns thead tr");

    rows.forEach(function (row) {
        // Make the row draggable if it's not already
        row.setAttribute("draggable", "true");

        // Drag start event
        row.addEventListener('dragstart', function (e) {
            e.dataTransfer.setData("text", row.id); // Store the row's id
            row.style.opacity = "0.4"; // Reduce opacity to indicate dragging
        });

        // Drag over event
        row.addEventListener('dragover', function (e) {
            e.preventDefault(); // Allow dropping
            row.style.border = "2px solid #00f"; // Highlight the row being hovered over
        });

        // Drop event
        row.addEventListener('drop', function (e) {
            e.preventDefault();
            var draggedRowId = e.dataTransfer.getData("text");
            var draggedRow = document.getElementById(draggedRowId);
            var targetRow = row;

            // Move the dragged row to the new position
            if (draggedRow !== targetRow) {
                var rows = document.querySelectorAll("#SelectedColumns thead tr");
                var targetIndex = Array.from(rows).indexOf(targetRow);
                var draggedIndex = Array.from(rows).indexOf(draggedRow);

                // Ensure to insert the dragged row above or below the target row
                if (targetIndex > draggedIndex) {
                    targetRow.parentNode.insertBefore(draggedRow, targetRow.nextSibling);
                } else {
                    targetRow.parentNode.insertBefore(draggedRow, targetRow);
                }
            }

            row.style.border = "none"; // Reset the border
            draggedRow.style.opacity = "1"; // Reset opacity of the dragged row
        });

        // Drag leave event (reset border on leaving the target row)
        row.addEventListener('dragleave', function () {
            row.style.border = "none";
        });
    });
}

// Function to perform the search
function searchTablesInputValues(tableid, textboxid) {
    // Get the input value and convert it to lowercase for case-insensitive search
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById(textboxid);
    filter = input.value.toLowerCase();
    table = document.getElementById(tableid);
    tr = table.getElementsByTagName("tr");
    // Loop through all table rows and hide those that do not match the search query
    for (i = 1; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td");
        var found = false;
        for (var j = 0; j < td.length; j++) {
            if (td[j].getElementsByTagName("input").length > 0) {
                var inputValue = td[j].getElementsByTagName("input")[0].value.toLowerCase();
                if (inputValue.indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            } else {
                txtValue = td[j].textContent || td[j].innerText;
                if (txtValue.toLowerCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }
        }
        if (found) {
            tr[i].style.display = "";
        } else {
            tr[i].style.display = "none";
        }
    }
}

function HideShowDivContent(ShowDivId, HideDivId) {
    document.getElementById(ShowDivId).style.display = "block";
    document.getElementById(HideDivId).style.display = "none";
}
function HideShowSectionContent(ShowSectionId, HideSectionId) {
    document.getElementById(ShowSectionId).style.display = "block";
    document.getElementById(HideSectionId).style.display = "none";
}

function DropDownFreeTextCheck(e) {
    if (e.value != "") {
        var options = e.list.options;
        var k = 0;
        for (var i = 0; i < options.length; i++) {
            if (e.value == options[i].value) {
                e.style.borderColor = "";
                k = 1;
                return;
            }
        }
        if (k == 0) {
            e.value = "";
            e.style.borderColor = "red";
        }
    }
    else if (e.value == "") {
        e.style.borderColor = "";
    }
}
//HTML Table Export Function for xlsx
function TableExcelExport(HTMLTableName, ExcelWorkBookName, ExcelWorkSheetName) {
    var table = document.getElementById(HTMLTableName);

    // Clone the table to preserve the original
    var cloneTable = table.cloneNode(true);

    // Get all input elements in the cloned table
    var inputs = cloneTable.querySelectorAll("input[class='textbox']");

    // Replace textboxes with their values in the cloned table
    inputs.forEach(input => {
        var cell = input.parentNode; // Get the parent cell of the input
        cell.textContent = input.value; // Replace the input element with its value
    });

    var ws = XLSX.utils.table_to_sheet(cloneTable);

    var wb = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(wb, ws, ExcelWorkSheetName);

    XLSX.writeFile(wb, ExcelWorkBookName);
}

function showErrorPopup(message, bgColor) {
    if (message == "No Data Found!!!" && bgColor == "red") { return; }

    const popup = document.getElementById('error-popup');
    const messageElement = document.getElementById('popup-message');

    // Set the message and background color
    messageElement.innerHTML = message;
    messageElement.style.fontWeight = "bold";
    popup.style.backgroundColor = bgColor;

    // Show the popup
    popup.classList.add('show');

    // Automatically close the popup after 10 seconds
    if (bgColor == "green") {
        setTimeout(() => {
            closeErrorPopup();
        }, 3000);
    }
}

function closeErrorPopup() {
    const popup = document.getElementById('error-popup');
    popup.classList.remove('show');
    setTimeout(() => {
        document.getElementById('popup-message').textContent = "";
    }, 1000);
}
//Uploading Files to Handler (Single or Multiple - suitable for both)
function UploadFilestoHandler(TBid, FileName) {
    if (TBid == "DocUploadUploadFile") { if (document.getElementById("DocUploadFileName").value == "") { showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return; } }
    else { document.getElementById("DocUploadFileName").value = FileName; }

    if (document.getElementById("MainJobNo").value == "") {
        showErrorPopup('Kindly Upload the Document against the Jobno', 'red');
        return;
    }

    var FilesFromTB = document.getElementById(TBid).files, // Getting Files from Textbox
        FNtoShowinFrontEnd = document.getElementById("DocUploadFileName").value, // Getting the User Typed File Name to show in front end (Changes Based on Requirment)
        JobValue = document.getElementById("MainJobNo").value, // Getting Job no (Changes Based on Requirment) Export - MainJobNo
        //Local Path
        //FilesSavePath = "CFS/CFS Files/Import/" + JobValue.replaceAll(/[`~!@#$%^&*()_=+\[\]{};:'",<.>?/|\\]/g, '-') + "/", // Setting the path to save the file (Changes Based on Requirment)
        //Server Path
        FilesSavePath = "Logisticsa/CFS/CFS Files/Import/" + JobValue.replaceAll(/[`~!@#$%^&*()_=+\[\]{};:'",<.>?/|\\]/g, '-') + "/", // Setting the path to save the file (Changes Based on Requirment)
        formData = new FormData(), DocumentUploadTableDataList = new Array();

    const DomainName = `${window.location.protocol}//${window.location.hostname}/`; // Getting the current url domain name

    //Files To Handler    
    if (FilesFromTB.length != 0) {
        $('#cover-spin').show(0);
        var regex = /^([\s\S]+)\.([a-zA-Z0-9]+)$/;

        for (var i = 0; i < FilesFromTB.length; i++) {

            //Getting Currrnt Date And Time
            var CurrentDateTime = getCurrentDateTime();
            var match = FilesFromTB.item(i).name.match(regex);

            if (match) { // Splitting the file name & extenstion
                var FileNameWithoutExtension = match[1]; // File name
                var ExtensionWithoutFileName = match[2]; // File extension
            }

            //Recreating the filename, if the special characters exists & also Adding current datetime for uniqueness
            var RecreatedFileName = FileNameWithoutExtension.replaceAll(/[`~!@#$%^&*()_=+\[\]{};:'",<.>?/|\\]/g, '').replaceAll(" ", "") + "-" + CurrentDateTime + "." + ExtensionWithoutFileName;

            //Setting the datas to save to db
            var DataFields2 = {};
            DataFields2.JobNo = JobValue;
            if (i == 0) { DataFields2.DummyFileName = FNtoShowinFrontEnd; }
            else { DataFields2.DummyFileName = FNtoShowinFrontEnd + " (" + i + ")"; }
            DataFields2.ActFileName = RecreatedFileName;
            DataFields2.FileLink = DomainName + FilesSavePath + RecreatedFileName;
            DocumentUploadTableDataList.push(DataFields2);

            //Setting the datas to upload the files
            formData.append("files", FilesFromTB[i]);
            formData.append("FileName", RecreatedFileName);
            formData.append("FileSavingPath", "~/" + FilesSavePath);
        }

        const xhr = new XMLHttpRequest();
        xhr.open('POST', '../CommonHandler/MultiFileHandler.ashx', true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../CFS/CFSImport.aspx/DocumentUploadTableInsertData",
                    data: JSON.stringify({ DocumentUploadTableDataList: DocumentUploadTableDataList }),
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "Saved") {
                            DocumentUploadTableSearchData(JobValue);
                            showErrorPopup('Files Uploaded.', 'green');
                            document.getElementById(TBid).value = "";
                        }
                        else {
                            showErrorPopup('File Upload Success, but Cannot save data - Error : ' + data.d, 'red');
                        }

                        setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    },
                    error: function (result) {
                        showErrorPopup(result.responseText, 'red');
                        setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    }
                });
            }
            else {
                showErrorPopup('An error occurred while uploading files.', 'red');
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        };
        xhr.send(formData);
    }
    else {
        showErrorPopup('No Files Found.', 'red');
        return;
    }
}

//Uploading Files to Handler

//CurrentTime
function getCurrentDateTime() {
    const currentDate = new Date();
    const [day, month, year] = [currentDate.getDate(), currentDate.getMonth() + 1, currentDate.getFullYear()].map(num => num.toString().padStart(2, '0'));
    let [hours, minutes, seconds] = [currentDate.getHours() % 12 || 12, currentDate.getMinutes(), currentDate.getSeconds()].map(num => num.toString().padStart(2, '0'));
    const ampm = currentDate.getHours() >= 12 ? 'PM' : 'AM';

    return `${day}-${month}-${year}-${hours}-${minutes}-${seconds}-${ampm}`;
}
//CurrentTime

function PageLoadAddingDropDownValues() {
    try {

        document.getElementById('ReportJobNoList').innerHTML = '';
        document.getElementById('ContainerTypeList').innerHTML = '';
        document.getElementById('ReportTemplateNameList').innerHTML = '';
        document.getElementById('MainIGMNoList').innerHTML = '';
        document.getElementById('ReportIGMNoList').innerHTML = '';
        document.getElementById('MainPOAList').innerHTML = '';
        document.getElementById('ReportPortofArrivalList').innerHTML = '';
        document.getElementById('GeneralPODList').innerHTML = '';
        document.getElementById('GeneralServicesList').innerHTML = '';
        document.getElementById('GeneralPOLCountryList').innerHTML = '';
        document.getElementById('GeneralCommodityList').innerHTML = '';
        document.getElementById('ContainerISOCodeList').innerHTML = '';
        document.getElementById('ContainerCargoNatureList').innerHTML = '';
        document.getElementById('ContainerHoldAgencyList').innerHTML = '';
        document.getElementById('LoadContContainerNoList').innerHTML = '';
        document.getElementById('LoadContVehicleNoList').innerHTML = '';
        document.getElementById('TransportVehicleNoList').innerHTML = '';
        document.getElementById('EmptyTorCTruckNoList').innerHTML = '';
        document.getElementById('MainVesselNameList').innerHTML = '';
        document.getElementById('GeneralConsolerList').innerHTML = '';
        document.getElementById('GeneralLineNameList').innerHTML = '';
        document.getElementById('GeneralAccountHolderList').innerHTML = '';
        document.getElementById('GeneralForwarderList').innerHTML = '';
        document.getElementById('GeneralShippingLineList').innerHTML = '';
        document.getElementById('LinerImporterNameList').innerHTML = '';
        document.getElementById('GeneralCHANameList').innerHTML = '';
        document.getElementById('LinerLinerAgentList').innerHTML = '';
        document.getElementById('GeneralNominatedCustomerList').innerHTML = '';
        document.getElementById('TransportNameList').innerHTML = '';
        document.getElementById("WorkOrderEquipment").innerHTML = "";
        document.getElementById("CompanyNameList").innerHTML = "";
        document.getElementById("BranchNameList").innerHTML = "";
        document.getElementById("ContainerScanLocationList").innerHTML = "";
        document.getElementById("ScopeIdList").innerHTML = "";
        document.getElementById("BLNumberList").innerHTML = "";

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/DropDownValueMethod",
            data: "{'columnvalue':'" + document.getElementById("MainJobNo").value + "'}",
            dataType: "json",
            success: function (data) {
                var Equipment = "<label><input type='checkbox' onclick='SelectAllCheckBoxDD(this, " + '"' + "WorkOrderEquipment" + '"' + ")' value='Select All' />Select All</label><hr/>", EquipmentExists = false, i = 1;
                var Vendor = "<label><input type='checkbox' onclick='SelectAllCheckBoxDD(this, " + '"' + "WorkOrderVendor" + '"' + ")' value='Select All' />Select All</label><hr/>", VendorExists = false;
                $.each(data.d, function (key, value) {
                    if (value.DropDownColumnName == "Vessel Name") { $("#MainVesselNameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Consoler Name") { $("#GeneralConsolerList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Line Name") { $("#GeneralLineNameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Access Company") { $("#CompanyNameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Access Branch") { $("#BranchNameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Customer Name") {
                        $("#GeneralAccountHolderList").append($("<option></option>").html(value.DefaultValue));
                        $("#GeneralShippingLineList").append($("<option></option>").html(value.DefaultValue));
                        $("#GeneralNominatedCustomerList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "CFS-Scan Location") {
                        $("#ContainerScanLocationList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "Forwarder") { $("#GeneralForwarderList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Equipment Name") { EquipmentExists = true; Equipment += "<label><input id='EqpInp" + i + "' name='Equipment' type='checkbox' onclick='GetVendorNameonblur(this)' value='" + value.DefaultValue + "' /><span id='EqSpan" + i + "'>" + value.DefaultValue + "</span></label>"; i++; }
                    if (value.DropDownColumnName == "Vendor Name") { VendorExists = true; Vendor += "<label><input id='VenInp" + i + "' name='Vendor' type='checkbox' onclick='GetVendorNameonblur(this)' value='" + value.DefaultValue + "' /><span id='VenSpan" + i + "'>" + value.DefaultValue + "</span></label>"; i++; }
                    if (value.DropDownColumnName == "Importer Name") { $("#LinerImporterNameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "CHA Name") { $("#GeneralCHANameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Liner Code") { $("#LinerLinerAgentList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "Transport Name") { $("#TransportNameList").append($("<option></option>").html(value.DefaultValue)); }
                    if (value.DropDownColumnName == "JobNo") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ReportJobNoList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "BLNumber") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#BLNumberList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "Import CFS Charge Head") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ScopeIdList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "ContType") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ContainerTypeList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "TemplateName") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ReportTemplateNameList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "MainIGMNo") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#MainIGMNoList").append($("<option></option>").html(value.DefaultValue));
                        $("#ReportIGMNoList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "PortName") {
                        if (value.DefaultValue != "" && value.DefaultValue != null) {
                            $("#MainPOAList").append($("<option></option>").html(value.DefaultValue));
                            $("#ReportPortofArrivalList").append($("<option></option>").html(value.DefaultValue));
                            $("#GeneralPODList").append($("<option></option>").html(value.DefaultValue));
                        }
                    }
                    if (value.DropDownColumnName == "CFS-Services") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#GeneralServicesList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "Country") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#GeneralPOLCountryList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "CFS-Commodity") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#GeneralCommodityList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "ContainerISOCode") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ContainerISOCodeList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "CFS-CargoNature") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ContainerCargoNatureList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "CFS-HoldAgency") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#ContainerHoldAgencyList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "ContainerNo") {
                        if (value.DefaultValue != "" && value.DefaultValue != null)
                            $("#LoadContContainerNoList").append($("<option></option>").html(value.DefaultValue));
                    }
                    if (value.DropDownColumnName == "TransportVehicleNo") {
                        if (value.DefaultValue != "" && value.DefaultValue != null) {
                            $("#LoadContVehicleNoList").append($("<option></option>").html(value.DefaultValue));
                            $("#TransportVehicleNoList").append($("<option></option>").html(value.DefaultValue));
                            $("#EmptyTorCTruckNoList").append($("<option></option>").html(value.DefaultValue));
                        }
                    }
                });
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                if (EquipmentExists) { document.getElementById("WorkOrderEquipment").innerHTML = Equipment; }
                if (VendorExists) { document.getElementById("WorkOrderVendor").innerHTML = Vendor; }
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    catch (Error) {
        showErrorPopup(Error.message, 'red');
        return;
    }
}

function ReportSearch() {

    if (document.getElementById("ReportTemplateName").value != "") {
        TemplateSearchData(document.getElementById("ReportTemplateName").value);
    }

    var ReportSearchDataList = new Array();
    var DataFields = {};
    var table = document.getElementById("SelectedColumns");
    var ColumnName = "";
    for (var i = 1; i < table.rows.length; i++) {
        var ColName = table.rows[i].cells.item(1).innerText;

        var inputElement = table.rows[i].cells.item(2).querySelector("input");
        if (inputElement) {
            if (inputElement.value != "") {
                ColName = ColName + " As [" + inputElement.value + "]";
            }
        }

        if (ColumnName == "") { ColumnName = ColName; }
        else { ColumnName = ColumnName + "," + ColName; }
    }
    DataFields.ReportColumnName = ColumnName;
    DataFields.ReportCompanyName = document.getElementById("ReportCompanyName").value;
    DataFields.ReportBranchName = document.getElementById("ReportBranchName").value;
    DataFields.ReportColumnTabName = document.getElementById("TableName").value;
    DataFields.ReportJobNo = document.getElementById("ReportJobNo").value;
    DataFields.ReportStartDate = document.getElementById("StartDate").value;
    DataFields.ReportEndDate = document.getElementById("EndDate").value;
    DataFields.ReportIGMNo = document.getElementById("ReportIGMNo").value;
    DataFields.ReportVesselName = document.getElementById("ReportVesselName").value;
    DataFields.ReportPortofArrival = document.getElementById("ReportPortofArrival").value;
    DataFields.ReportStatus = document.getElementById("ReportStatus").value;
    DataFields.ReportTemplateName = document.getElementById("ReportTemplateName").value;
    ReportSearchDataList.push(DataFields);
    if (document.getElementById("StartDate").value != "" && document.getElementById("EndDate").value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ReportSearch",
            data: JSON.stringify({ ReportSearchDataList: ReportSearchDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "No Record Found") { showErrorPopup(data.d, 'red'); }
                else { $("#ReportTable").html(data.d); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    } else { showErrorPopup('Date Range is Mandatory !!', 'red'); return; }
    HideShowTabContent('HTMLOutput', 'OutputTabBtn');
}


function MainTableInsertData() {
    var MainTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.MainJobDate = document.getElementById("MainJobDate").value;
    DataFields.MainIGMNo = document.getElementById("MainIGMNo").value;
    DataFields.MainItemNo = document.getElementById("MainItemNo").value;
    DataFields.MainIGMDate = document.getElementById("MainIGMDate").value;
    DataFields.MainBookingNo = document.getElementById("MainBookingNo").value;
    DataFields.MainBookingDate = document.getElementById("MainBookingDate").value;
    DataFields.MainVesselName = document.getElementById("MainVesselName").value;
    DataFields.MainPOA = document.getElementById("MainPOA").value;
    DataFields.MainVIANo = document.getElementById("MainVIANo").value;
    DataFields.MainVoyNo = document.getElementById("MainVoyNo").value;
    DataFields.MainJobOwner = document.getElementById("MainJobOwner").value;
    DataFields.MainIGMUpload = document.getElementById("MainIGMUpload").value;
    DataFields.GeneralDOA = document.getElementById("GeneralDOA").value;
    DataFields.GeneralServices = document.getElementById("GeneralServices").value;
    DataFields.GeneralTPNo = document.getElementById("GeneralTPNo").value;
    DataFields.GeneralTPDate = document.getElementById("GeneralTPDate").value;
    DataFields.GeneralConsoler = document.getElementById("GeneralConsoler").value;
    DataFields.GeneralBerthingDate = document.getElementById("GeneralBerthingDate").value;
    DataFields.GeneralDateofDeparture = document.getElementById("GeneralDateofDeparture").value;
    DataFields.GeneralCutoffDate = document.getElementById("GeneralCutoffDate").value;
    DataFields.GeneralRotationNo = document.getElementById("GeneralRotationNo").value;
    DataFields.GeneralGateOpenDate = document.getElementById("GeneralGateOpenDate").value;
    DataFields.GeneralScanlistRecvDate = document.getElementById("GeneralScanlistRecvDate").value;
    DataFields.GeneralBOENo = document.getElementById("GeneralBOENo").value;
    DataFields.GeneralBOEDate = document.getElementById("GeneralBOEDate").value;
    DataFields.GeneralLineName = document.getElementById("GeneralLineName").value;
    DataFields.GeneralEnblock = document.getElementById("GeneralEnblock").value;
    DataFields.GeneralGCRMSStatus = document.getElementById("GeneralGCRMSStatus").value;
    DataFields.GeneralPOLCountry = document.getElementById("GeneralPOLCountry").value;
    DataFields.GeneralPOL = document.getElementById("GeneralPOL").value;
    DataFields.GeneralPOLCode = document.getElementById("GeneralPOLCode").value;
    DataFields.GeneralPOD = document.getElementById("GeneralPOD").value;
    DataFields.GeneralAccountHolder = document.getElementById("GeneralAccountHolder").value;
    DataFields.GeneralSplitItem = document.getElementById("GeneralSplitItem").value;
    DataFields.GeneralConsoleValidUpto = document.getElementById("GeneralConsoleValidUpto").value;
    DataFields.GeneralDONo = document.getElementById("GeneralDONo").value;
    DataFields.GeneralDODate = document.getElementById("GeneralDODate").value;
    DataFields.GeneralDOValidDate = document.getElementById("GeneralDOValidDate").value;
    DataFields.GeneralSMTP = document.getElementById("GeneralSMTP").value;
    DataFields.GeneralCustomChallanNo = document.getElementById("GeneralCustomChallanNo").value;
    DataFields.GeneralDutyValue = document.getElementById("GeneralDutyValue").value;
    DataFields.GeneralVolume = document.getElementById("GeneralVolume").value;
    DataFields.GeneralAssessableValue = document.getElementById("GeneralAssessableValue").value;
    DataFields.GeneralCommodity = document.getElementById("GeneralCommodity").value;
    DataFields.GeneralDefaultCustomExamPerc = document.getElementById("GeneralDefaultCustomExamPerc").value;
    DataFields.GeneralFromLocation = document.getElementById("GeneralFromLocation").value;
    DataFields.GeneralPackageType = document.getElementById("GeneralPackageType").value;
    DataFields.GeneralForwarder = document.getElementById("GeneralForwarder").value;
    DataFields.GeneralNominatedCustomer = document.getElementById("GeneralNominatedCustomer").value;
    DataFields.GeneralBacktoPort = document.getElementById("GeneralBacktoPort").value;
    DataFields.GeneralBacktoPortRemarks = document.getElementById("GeneralBacktoPortRemarks").value;
    DataFields.GeneralTransportation = document.getElementById("GeneralTransportation").value;
    DataFields.GeneralGateInType = document.getElementById("GeneralGateInType").value;
    DataFields.GeneralShippingLine = document.getElementById("GeneralShippingLine").value;
    DataFields.MainCompanyName = document.getElementById("MainCompanyName").value;
    DataFields.MainBranchName = document.getElementById("MainBranchName").value;
    MainTableDataList.push(DataFields);

    if (document.getElementById("MainIGMNo").value != "" && document.getElementById("MainItemNo").value != "" && document.getElementById("MainIGMDate").value != "" &&
        document.getElementById("MainVesselName").value != "" && document.getElementById("MainPOA").value != "" && document.getElementById("MainVIANo").value != "" &&
        document.getElementById("MainVoyNo").value != "" && document.getElementById("GeneralDOA").value != "" && document.getElementById("GeneralBerthingDate").value != "" &&
        document.getElementById("GeneralLineName").value != "" && document.getElementById("GeneralDutyValue").value != "" && document.getElementById("GeneralAssessableValue").value != "" &&
        document.getElementById("MainCompanyName").value != "" && document.getElementById("MainBranchName").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/MainTableInsertData",
            data: JSON.stringify({ MainTableDataList: MainTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    showErrorPopup("Saved", "green");
                    location.reload(true);
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}


function MainTableUpdateData() {
    var MainTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.MainJobDate = document.getElementById("MainJobDate").value;
    DataFields.MainIGMNo = document.getElementById("MainIGMNo").value;
    DataFields.MainItemNo = document.getElementById("MainItemNo").value;
    DataFields.MainIGMDate = document.getElementById("MainIGMDate").value;
    DataFields.MainBookingNo = document.getElementById("MainBookingNo").value;
    DataFields.MainBookingDate = document.getElementById("MainBookingDate").value;
    DataFields.MainVesselName = document.getElementById("MainVesselName").value;
    DataFields.MainPOA = document.getElementById("MainPOA").value;
    DataFields.MainVIANo = document.getElementById("MainVIANo").value;
    DataFields.MainVoyNo = document.getElementById("MainVoyNo").value;
    DataFields.MainJobOwner = document.getElementById("MainJobOwner").value;
    DataFields.MainIGMUpload = document.getElementById("MainIGMUpload").value;
    DataFields.GeneralDOA = document.getElementById("GeneralDOA").value;
    DataFields.GeneralServices = document.getElementById("GeneralServices").value;
    DataFields.GeneralTPNo = document.getElementById("GeneralTPNo").value;
    DataFields.GeneralTPDate = document.getElementById("GeneralTPDate").value;
    DataFields.GeneralConsoler = document.getElementById("GeneralConsoler").value;
    DataFields.GeneralBerthingDate = document.getElementById("GeneralBerthingDate").value;
    DataFields.GeneralDateofDeparture = document.getElementById("GeneralDateofDeparture").value;
    DataFields.GeneralCutoffDate = document.getElementById("GeneralCutoffDate").value;
    DataFields.GeneralRotationNo = document.getElementById("GeneralRotationNo").value;
    DataFields.GeneralGateOpenDate = document.getElementById("GeneralGateOpenDate").value;
    DataFields.GeneralScanlistRecvDate = document.getElementById("GeneralScanlistRecvDate").value;
    DataFields.GeneralBOENo = document.getElementById("GeneralBOENo").value;
    DataFields.GeneralBOEDate = document.getElementById("GeneralBOEDate").value;
    DataFields.GeneralLineName = document.getElementById("GeneralLineName").value;
    DataFields.GeneralEnblock = document.getElementById("GeneralEnblock").value;
    DataFields.GeneralGCRMSStatus = document.getElementById("GeneralGCRMSStatus").value;
    DataFields.GeneralPOLCountry = document.getElementById("GeneralPOLCountry").value;
    DataFields.GeneralPOL = document.getElementById("GeneralPOL").value;
    DataFields.GeneralPOLCode = document.getElementById("GeneralPOLCode").value;
    DataFields.GeneralPOD = document.getElementById("GeneralPOD").value;
    DataFields.GeneralAccountHolder = document.getElementById("GeneralAccountHolder").value;
    DataFields.GeneralSplitItem = document.getElementById("GeneralSplitItem").value;
    DataFields.GeneralConsoleValidUpto = document.getElementById("GeneralConsoleValidUpto").value;
    DataFields.GeneralDONo = document.getElementById("GeneralDONo").value;
    DataFields.GeneralDODate = document.getElementById("GeneralDODate").value;
    DataFields.GeneralDOValidDate = document.getElementById("GeneralDOValidDate").value;
    DataFields.GeneralSMTP = document.getElementById("GeneralSMTP").value;
    DataFields.GeneralCustomChallanNo = document.getElementById("GeneralCustomChallanNo").value;
    DataFields.GeneralDutyValue = document.getElementById("GeneralDutyValue").value;
    DataFields.GeneralVolume = document.getElementById("GeneralVolume").value;
    DataFields.GeneralAssessableValue = document.getElementById("GeneralAssessableValue").value;
    DataFields.GeneralCommodity = document.getElementById("GeneralCommodity").value;
    DataFields.GeneralDefaultCustomExamPerc = document.getElementById("GeneralDefaultCustomExamPerc").value;
    DataFields.GeneralFromLocation = document.getElementById("GeneralFromLocation").value;
    DataFields.GeneralPackageType = document.getElementById("GeneralPackageType").value;
    DataFields.GeneralForwarder = document.getElementById("GeneralForwarder").value;
    DataFields.GeneralNominatedCustomer = document.getElementById("GeneralNominatedCustomer").value;
    DataFields.GeneralBacktoPort = document.getElementById("GeneralBacktoPort").value;
    DataFields.GeneralBacktoPortRemarks = document.getElementById("GeneralBacktoPortRemarks").value;
    DataFields.GeneralTransportation = document.getElementById("GeneralTransportation").value;
    DataFields.GeneralGateInType = document.getElementById("GeneralGateInType").value;
    DataFields.GeneralShippingLine = document.getElementById("GeneralShippingLine").value;
    DataFields.MainCompanyName = document.getElementById("MainCompanyName").value;
    DataFields.MainBranchName = document.getElementById("MainBranchName").value;
    MainTableDataList.push(DataFields);

    if (document.getElementById("MainIGMNo").value != "" && document.getElementById("MainItemNo").value != "" && document.getElementById("MainIGMDate").value != "" &&
        document.getElementById("MainVesselName").value != "" && document.getElementById("MainPOA").value != "" && document.getElementById("MainVIANo").value != "" &&
        document.getElementById("MainVoyNo").value != "" && document.getElementById("GeneralDOA").value != "" && document.getElementById("GeneralBerthingDate").value != "" &&
        document.getElementById("GeneralLineName").value != "" && document.getElementById("GeneralDutyValue").value != "" && document.getElementById("GeneralAssessableValue").value != "" &&
        document.getElementById("MainCompanyName").value != "" && document.getElementById("MainBranchName").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/MainTableUpdateData",
            data: JSON.stringify({ MainTableDataList: MainTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    showErrorPopup("Updated", "green");
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function MainTableCancelData() {
    var MainTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    MainTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "") {
        var CancelConfirm = confirm("Do You want to cancel this Record.");

        if (CancelConfirm) {
            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../CFS/CFSImport.aspx/MainTableCancelData",
                data: JSON.stringify({ MainTableDataList: MainTableDataList }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Cancelled") {
                        showErrorPopup("Cancelled", "green");
                    }
                    else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                    else { showErrorPopup(data.d, 'red'); }
                    setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                },
                error: function (result) {
                    showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                }
            });
        }
    }
    else {
        showErrorPopup('Job No is required.', 'red'); return;
    }
}


function MainTableSearchData(PassingValueId = "MainJobNo") {
    if (PassingValueId == "MainJobNo") {
        if (document.getElementById('MainJobNo').value == "") {
            showErrorPopup("JobNo is mandatory!!!", "red");
            return;
        }

        document.getElementById('FormSaveBtn').disabled = true;
        document.getElementById('FormSaveBtn').style.backgroundColor = "#808080";
    }
    else if (PassingValueId == "MainIGMNo") {
        if (document.getElementById("MainIGMNo").value == "") {
            showErrorPopup("IGM No is mandatory!!!", "red");
            return;
        }
    }

    var MainTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById(PassingValueId).value;
    DataFields.MainPassingType = PassingValueId;
    MainTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/MainTableSearchData",
        data: JSON.stringify({ MainTableDataList: MainTableDataList }),
        dataType: "json",
        success: function (data) {
            $.each(data.d, function (key, value) {
                if (PassingValueId == "MainJobNo") {
                    document.getElementById("MainJobNo").value = value.MainJobNo;
                    document.getElementById("MainJobDate").value = value.MainJobDate;
                    document.getElementById("MainItemNo").value = value.MainItemNo;
                    document.getElementById("MainBookingNo").value = value.MainBookingNo;
                    document.getElementById("MainBookingDate").value = value.MainBookingDate;
                }
                document.getElementById("MainIGMNo").value = value.MainIGMNo;
                document.getElementById("MainIGMDate").value = value.MainIGMDate;
                document.getElementById("MainVesselName").value = value.MainVesselName;
                document.getElementById("MainPOA").value = value.MainPOA;
                document.getElementById("MainVIANo").value = value.MainVIANo;
                document.getElementById("MainVoyNo").value = value.MainVoyNo;
                document.getElementById("MainJobOwner").value = value.MainJobOwner;
                document.getElementById("MainIGMUpload").value = value.MainIGMUpload;
                document.getElementById("GeneralDOA").value = value.GeneralDOA;
                document.getElementById("GeneralServices").value = value.GeneralServices;
                document.getElementById("GeneralTPNo").value = value.GeneralTPNo;
                document.getElementById("GeneralTPDate").value = value.GeneralTPDate;
                document.getElementById("GeneralConsoler").value = value.GeneralConsoler;
                document.getElementById("GeneralBerthingDate").value = value.GeneralBerthingDate;
                document.getElementById("GeneralDateofDeparture").value = value.GeneralDateofDeparture;
                document.getElementById("GeneralCutoffDate").value = value.GeneralCutoffDate;
                document.getElementById("GeneralRotationNo").value = value.GeneralRotationNo;
                document.getElementById("GeneralGateOpenDate").value = value.GeneralGateOpenDate;
                document.getElementById("GeneralScanlistRecvDate").value = value.GeneralScanlistRecvDate;
                document.getElementById("GeneralBOENo").value = value.GeneralBOENo;
                document.getElementById("GeneralBOEDate").value = value.GeneralBOEDate;
                document.getElementById("GeneralLineName").value = value.GeneralLineName;
                document.getElementById("GeneralEnblock").value = value.GeneralEnblock;
                document.getElementById("GeneralGCRMSStatus").value = value.GeneralGCRMSStatus;
                document.getElementById("GeneralPOLCountry").value = value.GeneralPOLCountry;
                document.getElementById("GeneralPOL").value = value.GeneralPOL;
                document.getElementById("GeneralPOLCode").value = value.GeneralPOLCode;
                document.getElementById("GeneralPOD").value = value.GeneralPOD;
                document.getElementById("GeneralAccountHolder").value = value.GeneralAccountHolder;
                document.getElementById("GeneralSplitItem").value = value.GeneralSplitItem;
                document.getElementById("GeneralConsoleValidUpto").value = value.GeneralConsoleValidUpto;
                document.getElementById("GeneralDONo").value = value.GeneralDONo;
                document.getElementById("GeneralDODate").value = value.GeneralDODate;
                document.getElementById("GeneralDOValidDate").value = value.GeneralDOValidDate;
                document.getElementById("GeneralSMTP").value = value.GeneralSMTP;
                document.getElementById("GeneralCustomChallanNo").value = value.GeneralCustomChallanNo;
                document.getElementById("GeneralDutyValue").value = value.GeneralDutyValue;
                document.getElementById("GeneralVolume").value = value.GeneralVolume;
                document.getElementById("GeneralAssessableValue").value = value.GeneralAssessableValue;
                document.getElementById("GeneralCommodity").value = value.GeneralCommodity;
                document.getElementById("GeneralDefaultCustomExamPerc").value = value.GeneralDefaultCustomExamPerc;
                document.getElementById("GeneralFromLocation").value = value.GeneralFromLocation;
                document.getElementById("GeneralPackageType").value = value.GeneralPackageType;
                document.getElementById("GeneralForwarder").value = value.GeneralForwarder;
                document.getElementById("GeneralNominatedCustomer").value = value.GeneralNominatedCustomer;
                document.getElementById("GeneralBacktoPort").value = value.GeneralBacktoPort;
                document.getElementById("GeneralBacktoPortRemarks").value = value.GeneralBacktoPortRemarks;
                document.getElementById("GeneralTransportation").value = value.GeneralTransportation;
                document.getElementById("GeneralGateInType").value = value.GeneralGateInType;
                document.getElementById("GeneralShippingLine").value = value.GeneralShippingLine;
                document.getElementById("MainCompanyName").value = value.MainCompanyName;
                document.getElementById("MainBranchName").value = value.MainBranchName;

                LoadDDWhileSearch(value.MainIGMNo, value.MainItemNo, value.MainJobNo, value.GeneralPOLCountry, value.GeneralPOL);
                if (PassingValueId != "MainJobNo") { DocumentUploadTableSearchData(value.MainJobNo); }
            });
            if (PassingValueId == "MainJobNo") {
                LinerTableSearchData();
                ContainerTableSearchData();
                TransportTableSearchData();
                TSADocumentUploadTableSearchData();
                WorkOrderTableSearchData();
                LoadedContainerTableSearchData();
                SealCuttingTableSearchData();
                ExaminationTableSearchData();
                StuffingTableSearchData();
                DeStuffingTableSearchData();
                LoadedContainerOutTableSearchData();
                EmptyContainerOutTableSearchData();
                FCLCargoOutTableSearchData();
                ScopeTableSearchData();
                //RevenueTableSearchData();
                //CostTableSearchData();
                LoadedTruckTableSearchData();
                //TrackingTableSearchData();
                //NotesTableSearchData();
                LogTableSearchData();
                FillItemNo(); //Added Manually            
                JobNoWrite(); //Added Manually    
            }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function LinerTableInsertData() {
    FillItemNo();
    var LinerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.LinerItemNo = document.getElementById("LinerItemNo").value;
    DataFields.LinerImporterName = document.getElementById("LinerImporterName").value;
    DataFields.LinerLinerAgent = document.getElementById("LinerLinerAgent").value;
    DataFields.LinerBLNo = document.getElementById("LinerBLNo").value;
    DataFields.LinerBLDate = document.getElementById("LinerBLDate").value;
    DataFields.LinerIMDG = document.getElementById("LinerIMDG").value;
    DataFields.LinerWeightKg = document.getElementById("LinerWeightKg").value;
    DataFields.LinerPKG = document.getElementById("LinerPKG").value;
    DataFields.LinerCargoDetails = document.getElementById("LinerCargoDetails").value;
    DataFields.LinerTSANo = document.getElementById("LinerTSANo").value;
    DataFields.LinerTSADate = document.getElementById("LinerTSADate").value;
    DataFields.LinerGeneralCHAName = document.getElementById("GeneralCHAName").value;
    LinerTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("LinerItemNo").value != "" && document.getElementById("LinerImporterName").value != "" &&
        document.getElementById("LinerLinerAgent").value != "" && document.getElementById("LinerBLNo").value != "" && document.getElementById("LinerWeightKg").value != "" &&
        document.getElementById("LinerPKG").value != "") {// && document.getElementById("LinerWeightKg").value != "0" && document.getElementById("LinerPKG").value != "0") 

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LinerTableInsertData",
            data: JSON.stringify({ LinerTableDataList: LinerTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    LinerTableSearchData();
                    document.getElementById("LinerImporterName").value = "";
                    document.getElementById("LinerLinerAgent").value = "";
                    document.getElementById("LinerBLNo").value = "";
                    document.getElementById("LinerBLDate").value = "";
                    document.getElementById("LinerIMDG").value = "";
                    document.getElementById("LinerWeightKg").value = "";
                    document.getElementById("LinerPKG").value = "";
                    document.getElementById("LinerCargoDetails").value = "";
                    document.getElementById("LinerTSANo").value = "";
                    document.getElementById("LinerTSADate").value = "";
                    document.getElementById("GeneralCHAName").value = "";
                    showErrorPopup('Saved', 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function LinerTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("LinerItemNo").value = row.cells.item(0).innerText;
    document.getElementById("LinerImporterName").value = row.cells.item(1).innerText;
    document.getElementById("LinerLinerAgent").value = row.cells.item(2).innerText;
    document.getElementById("LinerBLNo").value = row.cells.item(3).innerText;
    document.getElementById("LinerBLDate").value = row.cells.item(4).innerText;
    document.getElementById("LinerIMDG").value = row.cells.item(5).innerText;
    document.getElementById("LinerWeightKg").value = row.cells.item(6).innerText;
    document.getElementById("LinerPKG").value = row.cells.item(7).innerText;
    document.getElementById("LinerCargoDetails").value = row.cells.item(8).innerText;
    document.getElementById("LinerTSANo").value = row.cells.item(9).innerText;
    document.getElementById("LinerTSADate").value = row.cells.item(10).innerText;
    document.getElementById("GeneralCHAName").value = row.cells.item(11).innerText;
    document.getElementById("LinerItemNo").disabled = true;
    document.getElementById("LinerAddBtn").style.backgroundColor = "#808080"; //28A745
    document.getElementById("LinerAddBtn").disabled = true;
    row.parentNode.removeChild(row);
}

function LinerTableUpdateData() {
    FillItemNo();
    var LinerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.LinerItemNo = document.getElementById("LinerItemNo").value;
    DataFields.LinerImporterName = document.getElementById("LinerImporterName").value;
    DataFields.LinerLinerAgent = document.getElementById("LinerLinerAgent").value;
    DataFields.LinerBLNo = document.getElementById("LinerBLNo").value;
    DataFields.LinerBLDate = document.getElementById("LinerBLDate").value;
    DataFields.LinerIMDG = document.getElementById("LinerIMDG").value;
    DataFields.LinerWeightKg = document.getElementById("LinerWeightKg").value;
    DataFields.LinerPKG = document.getElementById("LinerPKG").value;
    DataFields.LinerCargoDetails = document.getElementById("LinerCargoDetails").value;
    DataFields.LinerTSANo = document.getElementById("LinerTSANo").value;
    DataFields.LinerTSADate = document.getElementById("LinerTSADate").value;
    DataFields.LinerGeneralCHAName = document.getElementById("GeneralCHAName").value;
    LinerTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("LinerItemNo").value != "" && document.getElementById("LinerImporterName").value != "" &&
        document.getElementById("LinerLinerAgent").value != "" && document.getElementById("LinerBLNo").value != "" && document.getElementById("LinerWeightKg").value != "" &&
        document.getElementById("LinerPKG").value != "") {// && document.getElementById("LinerWeightKg").value != "0" && document.getElementById("LinerPKG").value != "0") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LinerTableUpdateData",
            data: JSON.stringify({ LinerTableDataList: LinerTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    LinerTableSearchData();
                    document.getElementById("LinerImporterName").value = "";
                    document.getElementById("LinerLinerAgent").value = "";
                    document.getElementById("LinerBLNo").value = "";
                    document.getElementById("LinerBLDate").value = "";
                    document.getElementById("LinerIMDG").value = "";
                    document.getElementById("LinerWeightKg").value = "";
                    document.getElementById("LinerPKG").value = "";
                    document.getElementById("LinerCargoDetails").value = "";
                    document.getElementById("LinerTSANo").value = "";
                    document.getElementById("LinerTSADate").value = "";
                    document.getElementById("GeneralCHAName").value = "";
                    document.getElementById("LinerAddBtn").style.backgroundColor = "#28A745";
                    document.getElementById("LinerAddBtn").disabled = false;
                    showErrorPopup('Updated', 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function LinerTableCancelData() {
    var LinerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.LinerItemNo = document.getElementById("LinerItemNo").value;
    LinerTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("LinerItemNo").value != "") {
        var CancelConfirm = confirm("Do You want to cancel this Record.");
        if (CancelConfirm) {
            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../CFS/CFSImport.aspx/LinerTableCancelData",
                data: JSON.stringify({ LinerTableDataList: LinerTableDataList }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Cancelled") {
                        LinerTableSearchData();
                        document.getElementById("LinerImporterName").value = "";
                        document.getElementById("LinerLinerAgent").value = "";
                        document.getElementById("LinerBLNo").value = "";
                        document.getElementById("LinerBLDate").value = "";
                        document.getElementById("LinerIMDG").value = "";
                        document.getElementById("LinerWeightKg").value = "";
                        document.getElementById("LinerPKG").value = "";
                        document.getElementById("LinerCargoDetails").value = "";
                        document.getElementById("LinerTSANo").value = "";
                        document.getElementById("LinerTSADate").value = "";
                        document.getElementById("GeneralCHAName").value = "";
                        showErrorPopup('Cancelled', 'green');
                    }
                    else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                    else { showErrorPopup(data.d, 'red'); }
                    setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                },
                error: function (result) {
                    showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                }
            });
        }
    }
    else {
        showErrorPopup('Kindly Enter Job & Item No.', 'red');
        return;
    }
}

function LinerTableSearchData() {
    var LinerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    LinerTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/LinerTableSearchData",
        data: JSON.stringify({ LinerTableDataList: LinerTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("LinerTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("LinerTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='LinerTableEditData(this)' style='color=blue;'><b><u>" + value.LinerItemNo + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.LinerImporterName;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.LinerLinerAgent;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.LinerBLNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.LinerBLDate;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.LinerIMDG;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.LinerWeightKg;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.LinerPKG;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.LinerCargoDetails;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.LinerTSANo;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.LinerTSADate;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.LinerGeneralCHAName;
                document.getElementById("LinerAddBtn").disabled = true;
                document.getElementById("LinerAddBtn").style.backgroundColor = "#808080";

                const Tariffs = value.TariffValues.split(",");
                for (let i = 0; i < Tariffs.length; i++) {
                    $("#ScopeIdList").append($("<option></option>").html(Tariffs[i].trim()));
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function ContainerTableInsertData() {
    FillItemNo();
    var ContTable = document.getElementById("ContainerTable");
    var ContainerTableDataList = new Array(), Exists = false;

    for (var c = 1; c < ContTable.rows.length; c++) {
        if (document.getElementById("ContCheckBox" + c).checked) {
            if (document.getElementById("MainJobNo").value != "" && document.getElementById("ContainerItemNo" + c).value != "" && document.getElementById("ContainerNo" + c).value != "" &&
                document.getElementById("ContainerISOCode" + c).value != "" &&
                document.getElementById("ContainerSealNo" + c).value != "" && document.getElementById("ContainerWeightKg" + c).value != "" &&
                document.getElementById("ContainerNoofPackage" + c).value != "" && document.getElementById("ContainerFCLLCL" + c).value != "") {

                var DataFields = {};
                DataFields.MainJobNo = document.getElementById("MainJobNo").value;
                DataFields.ContainerItemNo = document.getElementById("ContainerItemNo" + c).value;
                DataFields.ContainerNo = document.getElementById("ContainerNo" + c).value;
                DataFields.ContainerISOCode = document.getElementById("ContainerISOCode" + c).value;
                DataFields.ContainerSize = document.getElementById("ContainerSize" + c).value;
                DataFields.ContainerType = document.getElementById("ContainerType" + c).value;
                DataFields.ContainerSealNo = document.getElementById("ContainerSealNo" + c).value;
                DataFields.ContainerTareWeight = document.getElementById("ContainerTareWeight" + c).value;
                DataFields.ContainerWeightKg = document.getElementById("ContainerWeightKg" + c).value;
                DataFields.ContainerCargoWeightKg = document.getElementById("ContainerCargoWeightKg" + c).value;
                DataFields.ContainerCargoNature = document.getElementById("ContainerCargoNature" + c).value;
                DataFields.ContainerNoofPackage = document.getElementById("ContainerNoofPackage" + c).value;
                DataFields.ContainerFCLLCL = document.getElementById("ContainerFCLLCL" + c).value;
                DataFields.ContainerPrimarySecondary = document.getElementById("ContainerPrimarySecondary" + c).value;
                DataFields.ContainerGroupCode = document.getElementById("ContainerGroupCode" + c).value;
                DataFields.ContainerIMOCode = document.getElementById("ContainerIMOCode" + c).value;
                DataFields.ContainerUNNo = document.getElementById("ContainerUNNo" + c).value;
                DataFields.ContainerScanType = document.getElementById("ContainerScanType" + c).value;
                DataFields.ContainerScanLocation = document.getElementById("ContainerScanLocation" + c).value;
                DataFields.ContainerDeliveryMode = document.getElementById("ContainerDeliveryMode" + c).value;
                DataFields.ContainerHold = document.getElementById("ContainerHold" + c).value;
                DataFields.ContainerHoldRemarks = document.getElementById("ContainerHoldRemarks" + c).value;
                DataFields.ContainerHoldAgency = document.getElementById("ContainerHoldAgency" + c).value;
                DataFields.ContainerHoldDate = document.getElementById("ContainerHoldDate" + c).value;
                DataFields.ContainerReleaseDate = document.getElementById("ContainerReleaseDate" + c).value;
                DataFields.ContainerReleaseRemarks = document.getElementById("ContainerReleaseRemarks" + c).value;
                DataFields.ContainerClaimDetails = document.getElementById("ContainerClaimDetails" + c).value;
                DataFields.ContainerClaimAmount = document.getElementById("ContainerClaimAmount" + c).value;
                DataFields.ContainerPaymentDate = document.getElementById("ContainerPaymentDate" + c).value;
                DataFields.ContainerRemarks = document.getElementById("ContainerRemarks" + c).value;
                DataFields.ContainerWHLoc = document.getElementById("ContainerWHLoc" + c).value;
                DataFields.ContainerPriority = document.getElementById("ContainerPriority" + c).value;
                ContainerTableDataList.push(DataFields);

                Exists = true;
            }
            else {
                showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
            }
        }
    }

    if (Exists) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ContainerTableInsertData",
            data: JSON.stringify({ ContainerTableDataList: ContainerTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    ContainerTableSearchData();
                    showErrorPopup('Saved', 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Select the new container to add the record.', 'red'); return;
    }
}

function ContainerTableUpdateData() {
    FillItemNo();
    var ContTable = document.getElementById("ContainerTable");
    var ContainerTableDataList = new Array(), Exists = false;

    for (var c = 1; c < ContTable.rows.length; c++) {
        if (document.getElementById("ContCheckBox" + c).checked) {
            if (document.getElementById("MainJobNo").value != "" && document.getElementById("ContainerItemNo" + c).value != "" && document.getElementById("ContainerNo" + c).value != "" &&
                document.getElementById("ContainerISOCode" + c).value != "" &&
                document.getElementById("ContainerSealNo" + c).value != "" && document.getElementById("ContainerWeightKg" + c).value != "" &&
                document.getElementById("ContainerNoofPackage" + c).value != "" && document.getElementById("ContainerFCLLCL" + c).value != "") {

                var DataFields = {};
                DataFields.MainJobNo = document.getElementById("MainJobNo").value;
                DataFields.ContainerItemNo = document.getElementById("ContainerItemNo" + c).value;
                DataFields.ContainerNo = document.getElementById("ContainerNo" + c).value;
                DataFields.ContainerISOCode = document.getElementById("ContainerISOCode" + c).value;
                DataFields.ContainerSize = document.getElementById("ContainerSize" + c).value;
                DataFields.ContainerType = document.getElementById("ContainerType" + c).value;
                DataFields.ContainerSealNo = document.getElementById("ContainerSealNo" + c).value;
                DataFields.ContainerTareWeight = document.getElementById("ContainerTareWeight" + c).value;
                DataFields.ContainerWeightKg = document.getElementById("ContainerWeightKg" + c).value;
                DataFields.ContainerCargoWeightKg = document.getElementById("ContainerCargoWeightKg" + c).value;
                DataFields.ContainerCargoNature = document.getElementById("ContainerCargoNature" + c).value;
                DataFields.ContainerNoofPackage = document.getElementById("ContainerNoofPackage" + c).value;
                DataFields.ContainerFCLLCL = document.getElementById("ContainerFCLLCL" + c).value;
                DataFields.ContainerPrimarySecondary = document.getElementById("ContainerPrimarySecondary" + c).value;
                DataFields.ContainerGroupCode = document.getElementById("ContainerGroupCode" + c).value;
                DataFields.ContainerIMOCode = document.getElementById("ContainerIMOCode" + c).value;
                DataFields.ContainerUNNo = document.getElementById("ContainerUNNo" + c).value;
                DataFields.ContainerScanType = document.getElementById("ContainerScanType" + c).value;
                DataFields.ContainerScanLocation = document.getElementById("ContainerScanLocation" + c).value;
                DataFields.ContainerDeliveryMode = document.getElementById("ContainerDeliveryMode" + c).value;
                DataFields.ContainerHold = document.getElementById("ContainerHold" + c).value;
                DataFields.ContainerHoldRemarks = document.getElementById("ContainerHoldRemarks" + c).value;
                DataFields.ContainerHoldAgency = document.getElementById("ContainerHoldAgency" + c).value;
                DataFields.ContainerHoldDate = document.getElementById("ContainerHoldDate" + c).value;
                DataFields.ContainerReleaseDate = document.getElementById("ContainerReleaseDate" + c).value;
                DataFields.ContainerReleaseRemarks = document.getElementById("ContainerReleaseRemarks" + c).value;
                DataFields.ContainerClaimDetails = document.getElementById("ContainerClaimDetails" + c).value;
                DataFields.ContainerClaimAmount = document.getElementById("ContainerClaimAmount" + c).value;
                DataFields.ContainerPaymentDate = document.getElementById("ContainerPaymentDate" + c).value;
                DataFields.ContainerRemarks = document.getElementById("ContainerRemarks" + c).value;
                DataFields.ContainerWHLoc = document.getElementById("ContainerWHLoc" + c).value;
                DataFields.ContainerPriority = document.getElementById("ContainerPriority" + c).value;
                ContainerTableDataList.push(DataFields);

                Exists = true;
            }
            else {
                showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
            }
        }
    }

    if (Exists) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ContainerTableUpdateData",
            data: JSON.stringify({ ContainerTableDataList: ContainerTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    ContainerTableSearchData();
                    showErrorPopup('Updated', 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Select the container to update the record.', 'red'); return;
    }
}

function ContainerTableCancelData() {
    FillItemNo();
    var ContainerTableDataList = new Array(), Exists = false;
    var ContTable = document.getElementById("ContainerTable");

    for (var c = 1; c < ContTable.rows.length; c++) {
        if (document.getElementById("ContCheckBox" + c).checked == true && document.getElementById("ContainerItemNo" + c).value != "" && document.getElementById("ContainerNo" + c).value != "") {
            Exists = true;
            var DataFields = {};
            DataFields.MainJobNo = document.getElementById("MainJobNo").value;
            DataFields.ContainerItemNo = document.getElementById("ContainerItemNo" + c).value;
            DataFields.ContainerNo = document.getElementById("ContainerNo" + c).value;
            ContainerTableDataList.push(DataFields);
        }
    }

    if (Exists) {
        var CancelConfirm = confirm("Do You want to cancel this Record.");

        if (CancelConfirm) {
            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../CFS/CFSImport.aspx/ContainerTableCancelData",
                data: JSON.stringify({ ContainerTableDataList: ContainerTableDataList }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Cancelled") {
                        ContainerTableSearchData();
                        showErrorPopup('Cancelled', 'green');
                    }
                    else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                    else { showErrorPopup(data.d, 'red'); }
                    setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                },
                error: function (result) {
                    showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                }
            });
        }
    }
    else {
        showErrorPopup('Select the Containers to cancel the record.', 'red'); return;
    }
}

function Addemptyrowincontainer() {
    var table = document.getElementById("ContainerTable");
    var rowcount = table.rows.length;
    var c = rowcount;
    var row = table.insertRow(rowcount);
    var cell0 = row.insertCell(0); cell0.innerHTML = "<input type='checkbox' id='ContCheckBox" + c + "' />";
    var cell1 = row.insertCell(1); cell1.innerHTML = "<input disabled='disabled' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerItemNo" + c + "' />";
    var cell2 = row.insertCell(2); cell2.innerHTML = "<input maxlength='50' class='textbox' oninput='this.value = this.value.toUpperCase();' autocomplete='off' type='text' id='ContainerNo" + c + "' value='' list='ContainerNoList' />";
    var cell3 = row.insertCell(3); cell3.innerHTML = "<input maxlength='50' list='ContainerISOCodeList' id='ContainerISOCode" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);LoadDropDownValuesonblur(this);' ondblclick='this.value = '';' />";
    var cell4 = row.insertCell(4); cell4.innerHTML = "<input maxlength='10' list='ContainerSizeList' id='ContainerSize" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />";
    var cell5 = row.insertCell(5); cell5.innerHTML = "<input maxlength='50' list='ContainerTypeList' id='ContainerType" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell6 = row.insertCell(6); cell6.innerHTML = "<input maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerSealNo" + c + "' value='' />"
    var cell7 = row.insertCell(7); cell7.innerHTML = "<input maxlength='' class='textbox' autocomplete='off' oninput='document.getElementById(" + '"' + "ContainerCargoWeightKg" + c + "" + '"' + ").value = parseFloat(document.getElementById(" + '"' + "ContainerWeightKg" + c + "" + '"' + ").value) - parseFloat(document.getElementById(" + '"' + "ContainerTareWeight" + c + "" + '"' + ").value);' type='number' id='ContainerTareWeight" + c + "' />"
    var cell8 = row.insertCell(8); cell8.innerHTML = "<input maxlength='' class='textbox' autocomplete='off' type='number' oninput='document.getElementById(" + '"' + "ContainerCargoWeightKg" + c + "" + '"' + ").value = parseFloat(document.getElementById(" + '"' + "ContainerWeightKg" + c + "" + '"' + ").value) - parseFloat(document.getElementById(" + '"' + "ContainerTareWeight" + c + "" + '"' + ").value);' id='ContainerWeightKg" + c + "' />"
    var cell9 = row.insertCell(9); cell9.innerHTML = "<input maxlength='' disabled='disabled' class='textbox' autocomplete='off' type='number' id='ContainerCargoWeightKg" + c + "' />"
    var cell10 = row.insertCell(10); cell10.innerHTML = "<input maxlength='50' list='ContainerCargoNatureList' id='ContainerCargoNature" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell11 = row.insertCell(11); cell11.innerHTML = "<input maxlength='' class='textbox' autocomplete='off' type='number' id='ContainerNoofPackage" + c + "' />"
    var cell12 = row.insertCell(12); cell12.innerHTML = "<input maxlength='10' list='ContainerFCLLCLList' value='FCL' id='ContainerFCLLCL" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell13 = row.insertCell(13); cell13.innerHTML = "<input maxlength='20' list='ContainerPrimarySecondaryList' value='Primary' id='ContainerPrimarySecondary" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell14 = row.insertCell(14); cell14.innerHTML = "<input maxlength='20' list='ContainerGroupCodeList' id='ContainerGroupCode" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell15 = row.insertCell(15); cell15.innerHTML = "<input maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerIMOCode" + c + "' value='' />"
    var cell16 = row.insertCell(16); cell16.innerHTML = "<input maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerUNNo" + c + "' value='' />"
    var cell17 = row.insertCell(17); cell17.innerHTML = "<input maxlength='50' list='ContainerScanTypeList' id='ContainerScanType" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell18 = row.insertCell(18); cell18.innerHTML = "<input maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerScanLocation" + c + "' value='' onblur='DropDownFreeTextCheck(this);' list='ContainerScanLocationList' />"
    var cell19 = row.insertCell(19); cell19.innerHTML = "<input maxlength='50' list='ContainerDeliveryModeList' id='ContainerDeliveryMode" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell20 = row.insertCell(20); cell20.innerHTML = "<input maxlength='10' list='ContainerHoldList' id='ContainerHold" + c + "' placeholder='Select' class='textbox' onblur='EnableDisableTB(this.value, this.id);DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell21 = row.insertCell(21); cell21.innerHTML = "<input disabled='disabled' maxlength='100' class='textbox' autocomplete='off' type='text' id='ContainerHoldRemarks" + c + "' value='' />"
    var cell22 = row.insertCell(22); cell22.innerHTML = "<input disabled='disabled' maxlength='50' list='ContainerHoldAgencyList' id='ContainerHoldAgency" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell23 = row.insertCell(23); cell23.innerHTML = "<input disabled='disabled' class='textbox' autocomplete='off' type='date' id='ContainerHoldDate" + c + "' value='' />"
    var cell24 = row.insertCell(24); cell24.innerHTML = "<input class='textbox' autocomplete='off' type='date' id='ContainerReleaseDate" + c + "' value='' />"
    var cell25 = row.insertCell(25); cell25.innerHTML = "<input maxlength='100' class='textbox' autocomplete='off' type='text' id='ContainerReleaseRemarks" + c + "' value='' />"
    var cell26 = row.insertCell(26); cell26.innerHTML = "<input maxlength='10' list='ContainerClaimDetailsList' id='ContainerClaimDetails" + c + "' placeholder='Select' class='textbox' onblur='EnableDisableTB(this.value, this.id);DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
    var cell27 = row.insertCell(27); cell27.innerHTML = "<input disabled='disabled' maxlength='' class='textbox' autocomplete='off' type='number' id='ContainerClaimAmount" + c + "' />"
    var cell28 = row.insertCell(28); cell28.innerHTML = "<input disabled='disabled' class='textbox' autocomplete='off' type='date' id='ContainerPaymentDate" + c + "' value='' />"
    var cell29 = row.insertCell(29); cell29.innerHTML = "<input maxlength='100' class='textbox' autocomplete='off' type='text' id='ContainerRemarks" + c + "' value='' />"
    var cell30 = row.insertCell(30); cell30.innerHTML = "<input maxlength='50' list='ContainerWHLocList' id='ContainerWHLoc" + c + "' placeholder='' class='textbox' />"
    var cell31 = row.insertCell(31); cell31.innerHTML = "<input maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerPriority" + c + "' value='' />"

    FillItemNo();
}

function ContainerTableSearchData() {
    var ContainerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    ContainerTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/ContainerTableSearchData",
        data: JSON.stringify({ ContainerTableDataList: ContainerTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("ContainerTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            var c = 1, Haz = "", Holded = "";
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("ContainerTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<input type='checkbox' id='ContCheckBox" + c + "' />";
                var cell1 = row.insertCell(1); cell1.innerHTML = "<input title='" + value.ContainerItemNo + "' value='" + value.ContainerItemNo + "' disabled='disabled' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerItemNo" + c + "' />";
                var cell2 = row.insertCell(2); cell2.innerHTML = "<input title='" + value.ContainerNo + "' value='" + value.ContainerNo + "' maxlength='50' class='textbox' oninput='this.value = this.value.toUpperCase();' autocomplete='off' type='text' id='ContainerNo" + c + "' value='' list='ContainerNoList' />";
                var cell3 = row.insertCell(3); cell3.innerHTML = "<input title='" + value.ContainerISOCode + "' value='" + value.ContainerISOCode + "' maxlength='50' list='ContainerISOCodeList' id='ContainerISOCode" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);LoadDropDownValuesonblur(this);' ondblclick='this.value = '';' />";
                var cell4 = row.insertCell(4); cell4.innerHTML = "<input title='" + value.ContainerSize + "' value='" + value.ContainerSize + "' maxlength='10' list='ContainerSizeList' id='ContainerSize" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />";
                var cell5 = row.insertCell(5); cell5.innerHTML = "<input title='" + value.ContainerType + "' value='" + value.ContainerType + "' maxlength='50' list='ContainerTypeList' id='ContainerType" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell6 = row.insertCell(6); cell6.innerHTML = "<input title='" + value.ContainerSealNo + "' value='" + value.ContainerSealNo + "' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerSealNo" + c + "' value='' />"
                var cell7 = row.insertCell(7); cell7.innerHTML = "<input title='" + value.ContainerTareWeight + "' value='" + value.ContainerTareWeight + "' maxlength='' class='textbox' autocomplete='off' oninput='document.getElementById(" + '"' + "ContainerCargoWeightKg" + c + "" + '"' + ").value = parseFloat(document.getElementById(" + '"' + "ContainerWeightKg" + c + "" + '"' + ").value) - parseFloat(document.getElementById(" + '"' + "ContainerTareWeight" + c + "" + '"' + ").value);' type='number' id='ContainerTareWeight" + c + "' />"
                var cell8 = row.insertCell(8); cell8.innerHTML = "<input title='" + value.ContainerWeightKg + "' value='" + value.ContainerWeightKg + "' maxlength='' class='textbox' autocomplete='off' type='number' oninput='document.getElementById(" + '"' + "ContainerCargoWeightKg" + c + "" + '"' + ").value = parseFloat(document.getElementById(" + '"' + "ContainerWeightKg" + c + "" + '"' + ").value) - parseFloat(document.getElementById(" + '"' + "ContainerTareWeight" + c + "" + '"' + ").value);' id='ContainerWeightKg" + c + "' />"
                var cell9 = row.insertCell(9); cell9.innerHTML = "<input title='" + value.ContainerCargoWeightKg + "' value='" + value.ContainerCargoWeightKg + "' maxlength='' disabled='disabled' class='textbox' autocomplete='off' type='number' id='ContainerCargoWeightKg" + c + "' />"
                var cell10 = row.insertCell(10); cell10.innerHTML = "<input title='" + value.ContainerCargoNature + "' value='" + value.ContainerCargoNature + "' maxlength='50' list='ContainerCargoNatureList' id='ContainerCargoNature" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell11 = row.insertCell(11); cell11.innerHTML = "<input title='" + value.ContainerNoofPackage + "' value='" + value.ContainerNoofPackage + "' maxlength='' class='textbox' autocomplete='off' type='number' id='ContainerNoofPackage" + c + "' />"
                var cell12 = row.insertCell(12); cell12.innerHTML = "<input title='" + value.ContainerFCLLCL + "' value='" + value.ContainerFCLLCL + "' maxlength='10' list='ContainerFCLLCLList' value='FCL' id='ContainerFCLLCL" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell13 = row.insertCell(13); cell13.innerHTML = "<input title='" + value.ContainerPrimarySecondary + "' value='" + value.ContainerPrimarySecondary + "' maxlength='20' list='ContainerPrimarySecondaryList' value='Primary' id='ContainerPrimarySecondary" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell14 = row.insertCell(14); cell14.innerHTML = "<input title='" + value.ContainerGroupCode + "' value='" + value.ContainerGroupCode + "' maxlength='20' list='ContainerGroupCodeList' id='ContainerGroupCode" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell15 = row.insertCell(15); cell15.innerHTML = "<input title='" + value.ContainerIMOCode + "' value='" + value.ContainerIMOCode + "' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerIMOCode" + c + "' value='' />"
                var cell16 = row.insertCell(16); cell16.innerHTML = "<input title='" + value.ContainerUNNo + "' value='" + value.ContainerUNNo + "' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerUNNo" + c + "' value='' />"
                var cell17 = row.insertCell(17); cell17.innerHTML = "<input title='" + value.ContainerScanType + "' value='" + value.ContainerScanType + "' maxlength='50' list='ContainerScanTypeList' id='ContainerScanType" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell18 = row.insertCell(18); cell18.innerHTML = "<input title='" + value.ContainerScanLocation + "' value='" + value.ContainerScanLocation + "' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerScanLocation" + c + "' value='' onblur='DropDownFreeTextCheck(this);' list='ContainerScanLocationList' />"
                var cell19 = row.insertCell(19); cell19.innerHTML = "<input title='" + value.ContainerDeliveryMode + "' value='" + value.ContainerDeliveryMode + "' maxlength='50' list='ContainerDeliveryModeList' id='ContainerDeliveryMode" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell20 = row.insertCell(20); cell20.innerHTML = "<input title='" + value.ContainerHold + "' value='" + value.ContainerHold + "' maxlength='10' list='ContainerHoldList' id='ContainerHold" + c + "' placeholder='Select' class='textbox' onblur='EnableDisableTB(this.value, this.id);DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell21 = row.insertCell(21); cell21.innerHTML = "<input title='" + value.ContainerHoldRemarks + "' value='" + value.ContainerHoldRemarks + "' disabled='disabled' maxlength='100' class='textbox' autocomplete='off' type='text' id='ContainerHoldRemarks" + c + "' value='' />"
                var cell22 = row.insertCell(22); cell22.innerHTML = "<input title='" + value.ContainerHoldAgency + "' value='" + value.ContainerHoldAgency + "' disabled='disabled' maxlength='50' list='ContainerHoldAgencyList' id='ContainerHoldAgency" + c + "' placeholder='Select' class='textbox' onblur='DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell23 = row.insertCell(23); cell23.innerHTML = "<input title='" + value.ContainerHoldDate + "' value='" + value.ContainerHoldDate + "' disabled='disabled' class='textbox' autocomplete='off' type='date' id='ContainerHoldDate" + c + "' value='' />"
                var cell24 = row.insertCell(24); cell24.innerHTML = "<input title='" + value.ContainerReleaseDate + "' value='" + value.ContainerReleaseDate + "' class='textbox' autocomplete='off' type='date' id='ContainerReleaseDate" + c + "' value='' />"
                var cell25 = row.insertCell(25); cell25.innerHTML = "<input title='" + value.ContainerReleaseRemarks + "' value='" + value.ContainerReleaseRemarks + "' maxlength='100' class='textbox' autocomplete='off' type='text' id='ContainerReleaseRemarks" + c + "' value='' />"
                var cell26 = row.insertCell(26); cell26.innerHTML = "<input title='" + value.ContainerClaimDetails + "' value='" + value.ContainerClaimDetails + "' maxlength='10' list='ContainerClaimDetailsList' id='ContainerClaimDetails" + c + "' placeholder='Select' class='textbox' onblur='EnableDisableTB(this.value, this.id);DropDownFreeTextCheck(this);' ondblclick='this.value = '';' />"
                var cell27 = row.insertCell(27); cell27.innerHTML = "<input title='" + value.ContainerClaimAmount + "' value='" + value.ContainerClaimAmount + "' disabled='disabled' maxlength='' class='textbox' autocomplete='off' type='number' id='ContainerClaimAmount" + c + "' />"
                var cell28 = row.insertCell(28); cell28.innerHTML = "<input title='" + value.ContainerPaymentDate + "' value='" + value.ContainerPaymentDate + "' disabled='disabled' class='textbox' autocomplete='off' type='date' id='ContainerPaymentDate" + c + "' value='' />"
                var cell29 = row.insertCell(29); cell29.innerHTML = "<input title='" + value.ContainerRemarks + "' value='" + value.ContainerRemarks + "' maxlength='100' class='textbox' autocomplete='off' type='text' id='ContainerRemarks" + c + "' value='' />"
                var cell30 = row.insertCell(30); cell30.innerHTML = "<input title='" + value.ContainerWHLoc + "' value='" + value.ContainerWHLoc + "' maxlength='50' list='ContainerWHLocList' id='ContainerWHLoc" + c + "' placeholder='' class='textbox' />"
                var cell31 = row.insertCell(31); cell31.innerHTML = "<input title='" + value.ContainerPriority + "' value='" + value.ContainerPriority + "' maxlength='50' class='textbox' autocomplete='off' type='text' id='ContainerPriority" + c + "' value='' />"
                if (value.ContainerIMOCode != "") {
                    Haz += value.ContainerNo + ",";
                }
                if (value.ContainerHold == "Yes") {
                    if (value.ContainerReleaseDate == "" || value.ContainerReleaseDate == null) {
                        Holded += value.ContainerNo + ",";
                    }
                }
                c++;
            });

            if (Haz != "") {
                var ErrorMessage = "The following containers contain hazardous materials: " + Haz.slice(0, -1) + ". Please handle them with caution.";
                showErrorPopup(ErrorMessage, 'red');
            }
            if (Holded != "") {
                var ErrorMessage = "The following containers are in Hold: " + Holded.slice(0, -1) + ".";
                showErrorPopup(ErrorMessage, 'red');
            }

            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function TSADocumentUploadTableSearchData() {
    var MainIGMNo = document.getElementById("MainIGMNo").value;

    if (MainIGMNo != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/TSADocumentUploadTableSearchData",
            data: JSON.stringify({ MainIGMNo: MainIGMNo }),
            dataType: "json",
            success: function (data) {

                var TableNo1 = data.d.Item1;
                var TableNo2 = data.d.Item2;

                var RemoveTableRows1 = document.getElementById("TSAUploadtable");
                for (var j = 1; j < RemoveTableRows1.rows.length; j++) { RemoveTableRows1.deleteRow(j); j--; }
                var RemoveTableRows2 = document.getElementById("DRFUploadTable");
                for (var k = 2; k < RemoveTableRows2.rows.length; k++) { RemoveTableRows2.deleteRow(k); k--; }
                var l = 1;
                $.each(TableNo1, function (key, value) {
                    if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                    var table = document.getElementById("TSAUploadtable");
                    var rowcount = table.rows.length;
                    var row = table.insertRow(rowcount);
                    var cell0 = row.insertCell(0); cell0.innerHTML = value.ItemNo;
                    var cell1 = row.insertCell(1); cell1.innerHTML = value.LinerCode;
                    var cell2 = row.insertCell(2); cell2.innerHTML = "<input class='textbox' id='TSANo" + l + "' style='width:100%;' type='text' value='" + value.TSANumber + "' title='" + value.TSANumber + "' />";
                    var cell3 = row.insertCell(3); cell3.innerHTML = "<input class='textbox' id='TSADate" + l + "' style='width:100%;' type='date' value='" + value.TSADate + "' title='" + value.TSADate + "' />";
                    var cell4 = row.insertCell(4); cell4.innerHTML = value.LinerJobNo; cell4.style.display = "none";
                    l++;
                });

                $.each(TableNo2, function (key, value) {
                    if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                    var table = document.getElementById("DRFUploadTable");
                    var rowcount = table.rows.length;
                    var row = table.insertRow(rowcount);
                    var cell0 = row.insertCell(0); cell0.innerHTML = value.DRFNo;
                    var cell1 = row.insertCell(1); cell1.innerHTML = value.DRFIssuedDate;
                    var cell2 = row.insertCell(2); cell2.innerHTML = value.TransportName;
                    var cell3 = row.insertCell(3); cell3.innerHTML = value.ContainerNumber;
                    var cell4 = row.insertCell(4); cell4.innerHTML = value.ContSize;
                    var cell5 = row.insertCell(5); cell5.innerHTML = value.ContScantype;
                    var cell6 = row.insertCell(6); cell6.innerHTML = value.ContScanLoc;
                });
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Select the IGM No.', 'red'); return;
    }
}

function DocumentUploadTableCancelData() {
    var table = document.getElementById("DocUpload");
    var Exists = false;
    for (var i = 1; i < table.rows.length; i++) {
        if (document.getElementById("radiobtn" + i).checked == true) {
            var DocumentUploadTableDataList = new Array();
            var DataFields = {};
            DataFields.JobNo = document.getElementById("MainJobNo").value;
            DataFields.ID = document.getElementById("radiobtn" + i).getAttribute("data-custom-attribute1");
            DataFields.DummyFileName = document.getElementById("radiobtn" + i).getAttribute("data-custom-attribute2");
            DataFields.FilePath = document.getElementById("href" + i).getAttribute("href");
            DocumentUploadTableDataList.push(DataFields);
            Exists = true;
        }
    }
    if (Exists && document.getElementById("MainJobNo").value != "") {
        var CancelConfirm = confirm("Do You want to cancel this Record.");

        if (CancelConfirm) {
            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../CFS/CFSImport.aspx/DocumentUploadTableCancelData",
                data: JSON.stringify({ DocumentUploadTableDataList: DocumentUploadTableDataList }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Cancelled") {
                        DocumentUploadTableSearchData("");
                        showErrorPopup('Cancelled Successfully', 'green');
                    }
                    else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                    else { showErrorPopup(data.d, 'red'); }
                    setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                },
                error: function (result) {
                    showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                }
            });
        }
    }
    else {
        showErrorPopup('Kindly Select the file to delete.', 'red');
    }
}


function DocumentUploadTableSearchData(JobNo) {
    var DocumentUploadTableDataList = new Array();
    var DataFields = {};
    if (JobNo == "") { DataFields.JobNo = document.getElementById("MainJobNo").value; }
    else { DataFields.JobNo = JobNo; }
    DocumentUploadTableDataList.push(DataFields);

    if (DataFields.JobNo != "" && DataFields.JobNo != "undefined") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/DocumentUploadTableSearchData",
            data: JSON.stringify({ DocumentUploadTableDataList: DocumentUploadTableDataList }),
            dataType: "json",
            success: function (data) {
                var table = document.getElementById("DocUpload"), i = 1;
                for (var j = 1; j < table.rows.length; j++) {
                    table.deleteRow(j);
                    j--;
                }
                $.each(data.d, function (key, value) {
                    if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                    else if (value.ReturnedValue != "" && value.ReturnedValue != null) { showErrorPopup(value.ReturnedValue, 'red'); return; }
                    var table = document.getElementById("DocUpload");
                    var rowcount = table.rows.length;
                    var row = table.insertRow(rowcount);
                    var cell0 = row.insertCell(0); cell0.innerHTML = "<input name='radio' type='radio' id='radiobtn" + i + "' data-custom-attribute1='" + value.ID + "' data-custom-attribute2='" + value.DummyFileName + "' />";
                    var cell1 = row.insertCell(1); cell1.innerHTML = "<a id='href" + i + "' href='" + value.FileLink + "' target='_blank'>" + value.DummyFileName + "</a>";
                    var cell2 = row.insertCell(2); cell2.innerHTML = "<span style='display: flex; align-items: center; gap: 4px; cursor: pointer;'><svg onclick='copyLink(" + '"' + "href" + i + "" + '"' + ")' xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='currentColor' class='bi bi-copy' viewBox='0 0 16 16'><path fill-rule='evenodd' d='M4 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2zm2-1a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 5a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1v-1h1v1a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h1v1z'/></svg>-<span>Copy</span> | <svg onclick='ShareLink(" + '"' + "href" + i + "" + '"' + ")' xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='currentColor' class='bi bi-share' viewBox='0 0 16 16'><path d='M13.5 1a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3M11 2.5a2.5 2.5 0 1 1 .603 1.628l-6.718 3.12a2.5 2.5 0 0 1 0 1.504l6.718 3.12a2.5 2.5 0 1 1-.488.876l-6.718-3.12a2.5 2.5 0 1 1 0-3.256l6.718-3.12A2.5 2.5 0 0 1 11 2.5m-8.5 4a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3m11 5.5a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3'/></svg>-<span>Share</span></span>";
                    i++;
                });
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red');
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}

function copyLink(id) {
    const link = document.getElementById(id).href; // Get the link URL

    navigator.clipboard.writeText(link)
        .then(() => {
            showErrorPopup('Link copied to clipboard!', 'green');
        })
        .catch(err => {
            showErrorPopup('Failed to copy the link: ' + err, 'red');
        });
}
function ShareLink(id) {
    const link = document.getElementById(id).href; // Get the link URL

    const shareData = {
        title: "Check this out!",
        text: "Here's a cool link to share:",
        url: link
    };

    if (navigator.share) {
        navigator.share(shareData)
            .then(() => showErrorPopup("Link shared successfully", "green"))
            .catch(err => showErrorPopup("Error sharing the link:", err, "red"));
    } else {
        showErrorPopup("Sharing is not supported in this browser. You can copy the link: " + shareData.url, "red");
    }
}

function TransportTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("TransportContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("TransportVehicleNo").value = row.cells.item(1).innerText;
    document.getElementById("TransportDRFNo").value = row.cells.item(0).innerText;
    document.getElementById("TransportDRFIssuedDate").value = row.cells.item(3).innerText;
    document.getElementById("TransportTransportName").value = row.cells.item(4).innerText;
    document.getElementById("TransportTruckDeployDate").value = row.cells.item(5).innerText;
    document.getElementById("TransportDriverName").value = row.cells.item(6).innerText;
    document.getElementById("TransportDriverMobileNo").value = row.cells.item(7).innerText;
    document.getElementById("TransportTerminalGateIn").value = row.cells.item(8).innerText;
    document.getElementById("TransportTerminalGateOut").value = row.cells.item(9).innerText;
    document.getElementById("TransportScanStatus").value = row.cells.item(10).innerText;
    document.getElementById("TransportCustomsGateOut").value = row.cells.item(11).innerText;
    row.parentNode.removeChild(row);

    if (row.cells.item(3).innerText == "" || row.cells.item(4).innerText == "") {
        LoadDropDownValuesonblur(document.getElementById("TransportDRFNo"));
    }
}

function TransportTableUpdateData() {
    var TransportTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ContainerNo = document.getElementById("TransportContainerNo").value;
    DataFields.TransportVehicleNo = document.getElementById("TransportVehicleNo").value;
    DataFields.TransportDRFNo = document.getElementById("TransportDRFNo").value;
    DataFields.TransportDRFIssuedDate = document.getElementById("TransportDRFIssuedDate").value;
    DataFields.TransportTransportName = document.getElementById("TransportTransportName").value;
    DataFields.TransportTruckDeployDate = document.getElementById("TransportTruckDeployDate").value;
    DataFields.TransportDriverName = document.getElementById("TransportDriverName").value;
    DataFields.TransportDriverMobileNo = document.getElementById("TransportDriverMobileNo").value;
    DataFields.TransportTerminalGateIn = document.getElementById("TransportTerminalGateIn").value;
    DataFields.TransportTerminalGateOut = document.getElementById("TransportTerminalGateOut").value;
    DataFields.TransportScanStatus = document.getElementById("TransportScanStatus").value;
    DataFields.TransportCustomsGateOut = document.getElementById("TransportCustomsGateOut").value;
    TransportTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("TransportContainerNo").value != "" && document.getElementById("TransportVehicleNo").value != "" &&
        document.getElementById("TransportDRFNo").value != "" && document.getElementById("TransportDRFIssuedDate").value != "" && document.getElementById("TransportTransportName").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/TransportTableUpdateData",
            data: JSON.stringify({ TransportTableDataList: TransportTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    TransportTableSearchData();
                    document.getElementById("TransportContainerNo").value = "";
                    document.getElementById("TransportVehicleNo").value = "";
                    document.getElementById("TransportDRFNo").value = "";
                    document.getElementById("TransportDRFIssuedDate").value = "";
                    document.getElementById("TransportTransportName").value = "";
                    document.getElementById("TransportTruckDeployDate").value = "";
                    document.getElementById("TransportDriverName").value = "";
                    document.getElementById("TransportDriverMobileNo").value = "";
                    document.getElementById("TransportTerminalGateIn").value = "";
                    document.getElementById("TransportTerminalGateOut").value = "";
                    document.getElementById("TransportScanStatus").value = "";
                    document.getElementById("TransportCustomsGateOut").value = "";
                    showErrorPopup('Updated', 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function TransportTableSearchData() {
    var TransportTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    TransportTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/TransportTableSearchData",
        data: JSON.stringify({ TransportTableDataList: TransportTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("TransportTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("TransportTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='TransportTableEditData(this)' style='color=blue;'><b><u>" + value.TransportDRFNo + "</u></b></>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.TransportVehicleNo;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.ContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.TransportDRFIssuedDate;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.TransportTransportName;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.TransportTruckDeployDate;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.TransportDriverName;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.TransportDriverMobileNo;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.TransportTerminalGateIn;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.TransportTerminalGateOut;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.TransportScanStatus;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.TransportCustomsGateOut;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function LoadedContainerTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("LoadContContainerNo").value = row.cells.item(0).innerText;
    document.getElementById("LoadContVehicleNo").value = row.cells.item(1).innerText;
    document.getElementById("LoadContGateInPassNo").value = row.cells.item(2).innerText;
    document.getElementById("LoadContGateInPassDate").value = row.cells.item(3).innerText;
    document.getElementById("LoadContAgentSealNo").value = row.cells.item(4).innerText;
    document.getElementById("LoadContScanStatus").value = row.cells.item(5).innerText;
    document.getElementById("LoadContScanType").value = row.cells.item(6).innerText;
    document.getElementById("LoadContScanLocation").value = row.cells.item(7).innerText;
    document.getElementById("LoadContStatusType").value = row.cells.item(8).innerText;
    document.getElementById("LoadContPluginRequired").value = row.cells.item(9).innerText;
    document.getElementById("LoadContAdditionAgentSealNo").value = row.cells.item(10).innerText;
    document.getElementById("LoadContODC").value = row.cells.item(11).innerText;
    document.getElementById("LoadContODCDimension").value = row.cells.item(12).innerText;
    document.getElementById("LoadContContainerTag").value = row.cells.item(13).innerText;
    document.getElementById("LoadContDamageDetails").value = row.cells.item(14).innerText;
    document.getElementById("LoadContPortorCustomSealNo").value = row.cells.item(15).innerText;
    document.getElementById("LoadContAdditionPortorCustomSealNo").value = row.cells.item(16).innerText;
    document.getElementById("LoadContModeofArrival").value = row.cells.item(17).innerText;
    document.getElementById("LoadContTransportType").value = row.cells.item(18).innerText;
    document.getElementById("LoadContEIRNo").value = row.cells.item(19).innerText;
    document.getElementById("LoadContVehicleType").value = row.cells.item(20).innerText;
    document.getElementById("LoadContContainerHold").value = row.cells.item(21).innerText;
    document.getElementById("LoadContContainerHoldRemarks").value = row.cells.item(22).innerText;
    document.getElementById("LoadContContainerHoldAgency").value = row.cells.item(23).innerText;
    document.getElementById("LoadContContainerHoldDate").value = row.cells.item(24).innerText;
    document.getElementById("LoadContTruckTag").value = row.cells.item(25).innerText;
    document.getElementById("LoadContContainerCondition").value = row.cells.item(26).innerText;
    document.getElementById("LoadContContainerNo").disabled = true;
    row.parentNode.removeChild(row);
}

function LoadedContainerTableUpdateData() {
    var LoadedContainerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.LoadContItemNo = document.getElementById("MainItemNo").value;
    DataFields.LoadContContainerNo = document.getElementById("LoadContContainerNo").value;
    DataFields.LoadContVehicleNo = document.getElementById("LoadContVehicleNo").value;
    DataFields.LoadContGateInPassNo = document.getElementById("LoadContGateInPassNo").value;
    DataFields.LoadContGateInPassDate = document.getElementById("LoadContGateInPassDate").value;
    DataFields.LoadContAgentSealNo = document.getElementById("LoadContAgentSealNo").value;
    DataFields.LoadContScanStatus = document.getElementById("LoadContScanStatus").value;
    DataFields.LoadContScanType = document.getElementById("LoadContScanType").value;
    DataFields.LoadContScanLocation = document.getElementById("LoadContScanLocation").value;
    DataFields.LoadContStatusType = document.getElementById("LoadContStatusType").value;
    DataFields.LoadContPluginRequired = document.getElementById("LoadContPluginRequired").value;
    DataFields.LoadContAdditionAgentSealNo = document.getElementById("LoadContAdditionAgentSealNo").value;
    DataFields.LoadContODC = document.getElementById("LoadContODC").value;
    DataFields.LoadContODCDimension = document.getElementById("LoadContODCDimension").value;
    DataFields.LoadContContainerTag = document.getElementById("LoadContContainerTag").value;
    DataFields.LoadContDamageDetails = document.getElementById("LoadContDamageDetails").value;
    DataFields.LoadContPortorCustomSealNo = document.getElementById("LoadContPortorCustomSealNo").value;
    DataFields.LoadContAdditionPortorCustomSealNo = document.getElementById("LoadContAdditionPortorCustomSealNo").value;
    DataFields.LoadContModeofArrival = document.getElementById("LoadContModeofArrival").value;
    DataFields.LoadContTransportType = document.getElementById("LoadContTransportType").value;
    DataFields.LoadContEIRNo = document.getElementById("LoadContEIRNo").value;
    DataFields.LoadContVehicleType = document.getElementById("LoadContVehicleType").value;
    DataFields.LoadContContainerHold = document.getElementById("LoadContContainerHold").value;
    DataFields.LoadContContainerHoldRemarks = document.getElementById("LoadContContainerHoldRemarks").value;
    DataFields.LoadContContainerHoldAgency = document.getElementById("LoadContContainerHoldAgency").value;
    DataFields.LoadContContainerHoldDate = document.getElementById("LoadContContainerHoldDate").value;
    DataFields.LoadContTruckTag = document.getElementById("LoadContTruckTag").value;
    DataFields.LoadContContainerCondition = document.getElementById("LoadContContainerCondition").value;
    LoadedContainerTableDataList.push(DataFields);

    if (document.getElementById("LoadContContainerNo").value != "" && document.getElementById("LoadContVehicleNo").value != "" && document.getElementById("LoadContStatusType").value != "" &&
        document.getElementById("LoadContODC").value != "" && document.getElementById("LoadContContainerTag").value != "" && document.getElementById("LoadContTransportType").value != "" &&
        document.getElementById("LoadContEIRNo").value != "" && document.getElementById("LoadContVehicleType").value != "" && document.getElementById("LoadContScanStatus").value != "") {

        UploadFilestoHandler('LoadContFiles', 'Gate In - ' + document.getElementById("LoadContContainerNo").value);

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LoadedContainerTableUpdateData",
            data: JSON.stringify({ LoadedContainerTableDataList: LoadedContainerTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    LoadedContainerTableSearchData();
                    document.getElementById("LoadContVehicleNo").value = "";
                    document.getElementById("LoadContGateInPassNo").value = "";
                    document.getElementById("LoadContGateInPassDate").value = "";
                    document.getElementById("LoadContAgentSealNo").value = "";
                    document.getElementById("LoadContScanStatus").value = "";
                    document.getElementById("LoadContScanType").value = "";
                    document.getElementById("LoadContScanLocation").value = "";
                    document.getElementById("LoadContStatusType").value = "";
                    document.getElementById("LoadContPluginRequired").value = "No";
                    document.getElementById("LoadContAdditionAgentSealNo").value = "";
                    document.getElementById("LoadContODC").value = "No";
                    document.getElementById("LoadContODCDimension").value = "";
                    document.getElementById("LoadContODCDimension").disabled = true;
                    document.getElementById("LoadContContainerTag").value = "";
                    document.getElementById("LoadContDamageDetails").value = "";
                    document.getElementById("LoadContDamageDetails").disabled = true;
                    document.getElementById("LoadContPortorCustomSealNo").value = "";
                    document.getElementById("LoadContAdditionPortorCustomSealNo").value = "";
                    document.getElementById("LoadContModeofArrival").value = "";
                    document.getElementById("LoadContTransportType").value = "";
                    document.getElementById("LoadContEIRNo").value = "";
                    document.getElementById("LoadContVehicleType").value = "";
                    document.getElementById("LoadContContainerHold").value = "No";
                    document.getElementById("LoadContContainerHoldRemarks").value = "";
                    document.getElementById("LoadContContainerHoldRemarks").disabled = true;
                    document.getElementById("LoadContContainerHoldAgency").value = "";
                    document.getElementById("LoadContContainerHoldAgency").disabled = true;
                    document.getElementById("LoadContContainerHoldDate").value = "";
                    document.getElementById("LoadContContainerHoldDate").disabled = true;
                    document.getElementById("LoadContTruckTag").value = "";
                    document.getElementById("LoadContContainerCondition").value = "";
                    document.getElementById("LoadContContainerNo").disabled = false;
                    document.getElementById("LoadContContainerNo").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function LoadedContainerTableSearchData() {
    var LoadedContainerTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.LoadContItemNo = document.getElementById("MainItemNo").value;
    DataFields.LoadContContainerNo = document.getElementById("LoadContContainerNo").value;
    LoadedContainerTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/LoadedContainerTableSearchData",
        data: JSON.stringify({ LoadedContainerTableDataList: LoadedContainerTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("LoadedContainerTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("LoadedContainerTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='LoadedContainerTableEditData(this)' style='color=blue;'><b><u>" + value.LoadContContainerNo + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.LoadContVehicleNo;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.LoadContGateInPassNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.LoadContGateInPassDate;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.LoadContAgentSealNo;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.LoadContScanStatus;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.LoadContScanType;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.LoadContScanLocation;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.LoadContStatusType;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.LoadContPluginRequired;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.LoadContAdditionAgentSealNo;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.LoadContODC;
                var cell12 = row.insertCell(12); cell12.innerHTML = value.LoadContODCDimension;
                var cell13 = row.insertCell(13); cell13.innerHTML = value.LoadContContainerTag;
                var cell14 = row.insertCell(14); cell14.innerHTML = value.LoadContDamageDetails;
                var cell15 = row.insertCell(15); cell15.innerHTML = value.LoadContPortorCustomSealNo;
                var cell16 = row.insertCell(16); cell16.innerHTML = value.LoadContAdditionPortorCustomSealNo;
                var cell17 = row.insertCell(17); cell17.innerHTML = value.LoadContModeofArrival;
                var cell18 = row.insertCell(18); cell18.innerHTML = value.LoadContTransportType;
                var cell19 = row.insertCell(19); cell19.innerHTML = value.LoadContEIRNo;
                var cell20 = row.insertCell(20); cell20.innerHTML = value.LoadContVehicleType;
                var cell21 = row.insertCell(21); cell21.innerHTML = value.LoadContContainerHold;
                var cell22 = row.insertCell(22); cell22.innerHTML = value.LoadContContainerHoldRemarks;
                var cell23 = row.insertCell(23); cell23.innerHTML = value.LoadContContainerHoldAgency;
                var cell24 = row.insertCell(24); cell24.innerHTML = value.LoadContContainerHoldDate;
                var cell25 = row.insertCell(25); cell25.innerHTML = value.LoadContTruckTag;
                var cell26 = row.insertCell(26); cell26.innerHTML = value.LoadContContainerCondition;
                if (value.LoadContGateInPassNo != "" && value.LoadContGateInPassNo != null) {
                    var cell27 = row.insertCell(27); cell27.innerHTML = "<a href='../CFS/GateInPass.aspx?GP=" + value.LoadContGateInPassNo + "&Type=LC' target='_blank'>Print</a>";
                }
                else { var cell27 = row.insertCell(27); cell27.innerHTML = "<span onclick='showErrorPopup(" + '"' + "The Container has not gated in" + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function EmptyTruckorContainerTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("EmptyTorCWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("EmptyTorCWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("EmptyTorCGateInPassNo").value = row.cells.item(2).innerText;
    document.getElementById("EmptyTorCGateInPassDate").value = row.cells.item(3).innerText;
    document.getElementById("EmptyTorCGateInMode").value = row.cells.item(4).innerText;
    document.getElementById("EmptyTorCCHAName").value = row.cells.item(5).innerText;
    document.getElementById("EmptyTorCTruckNo").value = row.cells.item(6).innerText;
    document.getElementById("EmptyTorCContainerNo").value = row.cells.item(7).innerText;
    document.getElementById("EmptyTorCDriverName").value = row.cells.item(8).innerText;
    document.getElementById("EmptyTorCDriverLicenseNo").value = row.cells.item(9).innerText;
    document.getElementById("EmptyTorCRemarks").value = row.cells.item(10).innerText;
    document.getElementById("EmptyTorCIdForUpadate").value = row.cells.item(11).innerText;
    row.parentNode.removeChild(row);
}

function EmptyTruckorContainerTableUpdateData() {
    var EmptyTruckorContainerTableDataList = new Array();
    var DataFields = {};
    DataFields.EmptyTorCWorkOrderNo = document.getElementById("EmptyTorCWorkOrderNo").value;
    DataFields.EmptyTorCWorkOrderDate = document.getElementById("EmptyTorCWorkOrderDate").value;
    DataFields.EmptyTorCGateInPassNo = document.getElementById("EmptyTorCGateInPassNo").value;
    DataFields.EmptyTorCGateInPassDate = document.getElementById("EmptyTorCGateInPassDate").value;
    DataFields.EmptyTorCGateInMode = document.getElementById("EmptyTorCGateInMode").value;
    DataFields.EmptyTorCCHAName = document.getElementById("EmptyTorCCHAName").value;
    DataFields.EmptyTorCTruckNo = document.getElementById("EmptyTorCTruckNo").value;
    DataFields.EmptyTorCContainerNo = document.getElementById("EmptyTorCContainerNo").value;
    DataFields.EmptyTorCDriverName = document.getElementById("EmptyTorCDriverName").value;
    DataFields.EmptyTorCDriverLicenseNo = document.getElementById("EmptyTorCDriverLicenseNo").value;
    DataFields.EmptyTorCRemarks = document.getElementById("EmptyTorCRemarks").value;
    DataFields.EmptyTruckorContainerId = document.getElementById("EmptyTorCIdForUpadate").value;
    EmptyTruckorContainerTableDataList.push(DataFields);

    if (document.getElementById("EmptyTorCTruckNo").value != "" && document.getElementById("EmptyTorCDriverName").value != "" && document.getElementById("EmptyTorCGateInMode").value != "" &&
        document.getElementById("EmptyTorCWorkOrderNo").value != "" && document.getElementById("EmptyTorCWorkOrderDate").value != "") {

        if (document.getElementById("EmptyTorCGateInMode").value == "Empty Container In") {
            if (document.getElementById("EmptyTorCContainerNo").value == "") { showErrorPopup('Container No is Required.', 'red'); return; }
        }

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/EmptyTruckorContainerTableUpdateData",
            data: JSON.stringify({ EmptyTruckorContainerTableDataList: EmptyTruckorContainerTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    showErrorPopup(data.d, 'green');
                    EmptyTruckorContainerTableSearchData(document.getElementById("EmptyTorCIdForUpadate").value);
                    document.getElementById("EmptyTorCWorkOrderNo").value = "";
                    document.getElementById("EmptyTorCWorkOrderDate").value = "";
                    document.getElementById("EmptyTorCGateInPassNo").value = "";
                    document.getElementById("EmptyTorCGateInPassDate").value = "";
                    document.getElementById("EmptyTorCGateInMode").value = "";
                    document.getElementById("EmptyTorCCHAName").value = "";
                    document.getElementById("EmptyTorCTruckNo").value = "";
                    document.getElementById("EmptyTorCContainerNo").value = "";
                    document.getElementById("EmptyTorCDriverName").value = "";
                    document.getElementById("EmptyTorCDriverLicenseNo").value = "";
                    document.getElementById("EmptyTorCRemarks").value = "";
                    document.getElementById("EmptyTorCIdForUpadate").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function EmptyTruckorContainerTableSearchData(CategoryValue) {
    var EmptyTruckorContainerTableDataList = new Array();
    var DataFields = {};
    DataFields.Category = CategoryValue;
    EmptyTruckorContainerTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/EmptyTruckorContainerTableSearchData",
        data: JSON.stringify({ EmptyTruckorContainerTableDataList: EmptyTruckorContainerTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("EmptyTruckorContainerTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("EmptyTruckorContainerTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='EmptyTruckorContainerTableEditData(this)' style='color=blue;'><b><u>" + value.EmptyTorCWorkOrderNo + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.EmptyTorCWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.EmptyTorCGateInPassNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.EmptyTorCGateInPassDate;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.EmptyTorCGateInMode;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.EmptyTorCCHAName;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.EmptyTorCTruckNo;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.EmptyTorCContainerNo;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.EmptyTorCDriverName;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.EmptyTorCDriverLicenseNo;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.EmptyTorCRemarks;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.EmptyTruckorContainerId; cell11.style.display = "none";
                if (value.EmptyTorCGateInPassNo != "" && value.EmptyTorCGateInPassNo != null) {
                    if (value.EmptyTorCGateInMode == "Empty Container In") { var cell12 = row.insertCell(12); cell12.innerHTML = "<a href='../CFS/GateInPass.aspx?GP=" + value.EmptyTorCGateInPassNo + "&Type=EC' target='_blank'>Print</a>"; }
                    else {
                        var cell12 = row.insertCell(12); cell12.innerHTML = "<a href='../CFS/GateInPass.aspx?GP=" + value.EmptyTorCGateInPassNo + "&Type=ET' target='_blank'>Print</a>";
                    }
                }
                else { var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "The Container has not gated in" + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function GetVendorNameonblur(e) {

    if (e.value != "") {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/GetVendorNameonblur",
            data: JSON.stringify({ Value: e.value, Name: e.name }),
            dataType: "json",
            success: function (data) {
                $.each(data.d, function (key, ListValue) {
                    if (ListValue.ErrorValue != "") {
                        showErrorPopup(ListValue.ErrorValue, 'red'); //Error Message
                        return;
                    }
                    else {
                        if (ListValue.ColumnName != "" && ListValue.ColumnName != null) {
                            if (e.name == "Equipment") {
                                var EqupExists = false, i = 1;
                                while (!EqupExists) {
                                    if (document.getElementById("VenSpan" + i)) {
                                        if (document.getElementById("VenSpan" + i).innerText == ListValue.ColumnName) {
                                            EqupExists = true;
                                            if (document.getElementById("VenInp" + i).checked) { document.getElementById("VenInp" + i).checked = false; }
                                            else if (document.getElementById("VenInp" + i).checked == false) { document.getElementById("VenInp" + i).checked = true; }
                                        }
                                    }
                                    i++;
                                }
                            }
                            if (e.name == "Vendor") {
                                var VendExists = false, j = 1;
                                while (!VendExists) {
                                    if (document.getElementById("EqSpan" + j)) {
                                        if (document.getElementById("EqSpan" + j).innerText == ListValue.ColumnName) {
                                            VendExists = true;
                                            if (document.getElementById("EqpInp" + j).checked) { document.getElementById("EqpInp" + j).checked = false; }
                                            else if (document.getElementById("EqpInp" + j).checked == false) { document.getElementById("EqpInp" + j).checked = true; }
                                        }
                                    }
                                    j++;
                                }
                            }
                        }
                    }
                });
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red');
            }
        });
    }
}

function GetContainerNoonblur(Type, btn) {
    var Jobno = document.getElementById("MainJobNo").value;
    document.getElementById("WorkOrderContainerNo").innerHTML = "";
    var Exists = false;

    if (Type != "") {
        if (Jobno != "") {
            if (Type != "Empty Truck In" && Type != "Empty Truck Out" && Type != "Empty Container In") {
                Exists = true;
                $('#cover-spin').show(0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../CFS/CFSImport.aspx/GetContainerNoonblur",
                    data: JSON.stringify({ Type: Type, Jobno: Jobno }),
                    dataType: "json",
                    success: function (data) {
                        var MultiContainerno = "<label><input type='checkbox' onclick='SelectAllCheckBoxDD(this, " + '"' + "WorkOrderContainerNo" + '"' + ")' value='Select All' />Select All</label><hr/>";
                        var i = 1;
                        $.each(data.d, function (key, ListValue) {
                            if (ListValue.ErrorValue != "") {
                                showErrorPopup(ListValue.ErrorValue, 'red'); //Error Message
                            }
                            else {
                                MultiContainerno += "<label><input type='checkbox' value='" + ListValue.ColumnName + "' />" + ListValue.ColumnName + "</label>"; i++;
                            }
                        });
                        document.getElementById("WorkOrderContainerNo").innerHTML = MultiContainerno;
                        if (i > 1) {
                            setTimeout(() => {
                                TriggerEdit(btn);
                            }, 1000);
                        };
                        setTimeout(() => {
                            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                        }, 1000);
                    },
                    error: function (result) {
                        showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    }
                });
            }
            if (!Exists) {
                setTimeout(() => {
                    TriggerEdit(btn);
                }, 1000);
            }
        }
        else {
            showErrorPopup('Job No is Required.', 'red'); return;
        }
    }
}

function WorkOrderTableInsertData() {
    var WOType = document.getElementById("WorkOrderType").value, WOTruckNo = document.getElementById("WorkOrderTruckNo").value;
    if (WOType == "Empty Truck In" || WOType == "Empty Truck Out") {
        if (WOTruckNo == "") { showErrorPopup('Vehicle No is Required.', 'red'); return; }
    }
    else if (WOType == "Empty Container In") {
        if (WOTruckNo == "") { showErrorPopup('Vehicle No is Required.', 'red'); return; }
        if (document.getElementById("WorkOrderEmptyContNo").value == "") { showErrorPopup('Container No is Required.', 'red'); return; }
    }

    let WOContcheckboxes = document.querySelectorAll('#WorkOrderContainerNo input[type="checkbox"]');
    let WOEqpcheckboxes = document.querySelectorAll('#WorkOrderEquipment input[type="checkbox"]');
    let WOVendorcheckboxes = document.querySelectorAll('#WorkOrderVendor input[type="checkbox"]');
    var WorkOrderTableDataList = new Array(), ContExists = false, ConEquipment = "", ConVendor = "";

    if (WOType != "Empty Container In" && WOType != "Empty Truck In" && WOType != "Empty Truck Out") {
        WOContcheckboxes.forEach(function (checkbox) {
            ContExists = true; ConEquipment = ""; ConVendor = "";
            if (checkbox.value != "Select All") {
                if (checkbox.checked) {
                    var DataFields = {};
                    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
                    DataFields.WorkOrderType = WOType;
                    DataFields.WorkOrderWorkOrderNo = document.getElementById("WorkOrderWorkOrderNo").value;
                    DataFields.WorkOrderWorkOrderDate = document.getElementById("WorkOrderWorkOrderDate").value;
                    DataFields.WorkOrderContainerNo = checkbox.value;
                    DataFields.WorkOrderStuffingContainerNo = document.getElementById("WorkOrderSection49ContNo").value;
                    DataFields.WorkOrderTruckNo = WOTruckNo;
                    DataFields.WorkOrderEquipmentType = document.getElementById("WorkOrderEquipmentType").value;
                    WOEqpcheckboxes.forEach(function (Echeckbox) {
                        if (Echeckbox.value != "Select All") {
                            if (Echeckbox.checked) {
                                ConEquipment += Echeckbox.value.replaceAll("|", "][") + "|";
                            }
                        }
                    });
                    WOVendorcheckboxes.forEach(function (vcheckbox) {
                        if (vcheckbox.value != "Select All") {
                            if (vcheckbox.checked) {
                                ConVendor += vcheckbox.value.replaceAll("|", "][") + "|";
                            }
                        }
                    });
                    DataFields.WorkOrderEquipment = ConEquipment.slice(0, -1);
                    DataFields.WorkOrderVendor = ConVendor.slice(0, -1);
                    WorkOrderTableDataList.push(DataFields);
                }
            }
        });
    }
    else {
        var DataFields = {};
        DataFields.MainJobNo = document.getElementById("MainJobNo").value;
        DataFields.WorkOrderType = WOType;
        DataFields.WorkOrderWorkOrderNo = document.getElementById("WorkOrderWorkOrderNo").value;
        DataFields.WorkOrderWorkOrderDate = document.getElementById("WorkOrderWorkOrderDate").value;
        DataFields.WorkOrderContainerNo = document.getElementById("WorkOrderEmptyContNo").value;
        DataFields.WorkOrderStuffingContainerNo = document.getElementById("WorkOrderSection49ContNo").value;
        DataFields.WorkOrderTruckNo = WOTruckNo;
        DataFields.WorkOrderEquipmentType = document.getElementById("WorkOrderEquipmentType").value;
        WOEqpcheckboxes.forEach(function (Echeckbox) {
            if (Echeckbox.value != "Select All") {
                if (Echeckbox.checked) {
                    ConEquipment += Echeckbox.value.replaceAll("|", "][") + "|";
                }
            }
        });
        WOVendorcheckboxes.forEach(function (vcheckbox) {
            if (vcheckbox.value != "Select All") {
                if (vcheckbox.checked) {
                    ConVendor += vcheckbox.value.replaceAll("|", "][") + "|";
                }
            }
        });
        DataFields.WorkOrderEquipment = ConEquipment.slice(0, -1);
        DataFields.WorkOrderVendor = ConVendor.slice(0, -1);
        WorkOrderTableDataList.push(DataFields);
    }

    if ((WOType == "Seal Cutting" || WOType == "Section-49") && !ContExists) {
        showErrorPopup('Container No is Required.', 'red'); return;
    }
    if ((WOType == "Examination" || WOType == "De-Stuffing") && !ContExists) {
        showErrorPopup('Container No is Required.', 'red'); return;
    }
    if ((WOType == "Examination" || WOType == "De-Stuffing") && document.getElementById("WorkOrderEquipmentType").value == "") {
        showErrorPopup('Equipment Type is Required.', 'red'); return;
    }
    if (WOType == "Loaded Container Out" && !ContExists) {
        showErrorPopup('Container No is Required.', 'red'); return;
    }
    if (WOType == "Loaded Container Out" && WOTruckNo == "") {
        showErrorPopup('Vehicle No is Required.', 'red'); return;
    }
    if (WOType == "Empty Container Out" && !ContExists) {
        showErrorPopup('Container No is Required.', 'red'); return;
    }
    if (WOType == "Empty Container Out" && WOTruckNo == "") {
        showErrorPopup('Vehicle No is Required.', 'red'); return;
    }
    if (WOType == "FCL Cargo Out" && !ContExists) {
        showErrorPopup('Container No is Required.', 'red'); return;
    }
    if (WOType == "FCL Cargo Out" && WOTruckNo == "") {
        showErrorPopup('Vehicle No is Required.', 'red'); return;
    }

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/WorkOrderTableInsertData",
        data: JSON.stringify({ WorkOrderTableDataList: WorkOrderTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Saved") {
                WorkOrderTableSearchData();
                document.getElementById("WorkOrderType").value = "";
                document.getElementById("WorkOrderWorkOrderNo").value = "";
                document.getElementById("WorkOrderWorkOrderDate").value = "";
                document.getElementById("WorkOrderEmptyContNo").value = "";
                document.getElementById("WorkOrderTruckNo").value = "";
                document.getElementById("WorkOrderEquipmentType").value = "";

                WOContcheckboxes.forEach(function (checkbox) {
                    checkbox.disabled = false;
                    checkbox.checked = false;
                });
                WOEqpcheckboxes.forEach(function (checkbox) {
                    checkbox.disabled = false;
                    checkbox.checked = false;
                });
                WOVendorcheckboxes.forEach(function (checkbox) {
                    checkbox.disabled = false;
                    checkbox.checked = false;
                });

                document.getElementById("WorkOrderType").disabled = false;
                showErrorPopup(data.d, 'green');
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function WorkOrderTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    GetContainerNoonblur(row.cells.item(0).innerText, btn);
}
function TriggerEdit(btn) {
    var row = btn.parentNode.parentNode;

    document.getElementById("WorkOrderType").value = row.cells.item(0).innerText;
    document.getElementById("WorkOrderWorkOrderNo").value = row.cells.item(1).innerText;
    document.getElementById("WorkOrderWorkOrderDate").value = row.cells.item(2).innerText;

    let WOContcheckboxes = document.querySelectorAll('#WorkOrderContainerNo input[type="checkbox"]');
    let WOEqpcheckboxes = document.querySelectorAll('#WorkOrderEquipment input[type="checkbox"]');
    let WOVendorcheckboxes = document.querySelectorAll('#WorkOrderVendor input[type="checkbox"]');
    WOContcheckboxes.forEach(function (checkbox) {
        checkbox.disabled = true;
        if (checkbox.value != row.cells.item(3).innerText) {
            checkbox.checked = false;
        }
        else { checkbox.checked = true; }
    });
    WOEqpcheckboxes.forEach(function (checkbox) {
        var equipments = row.cells.item(6).innerText.split("|");
        equipments.forEach(equipment => {
            if (checkbox.value.trim() != equipment.trim() && checkbox.checked == false) {
                checkbox.checked = false;
            }
            else { checkbox.checked = true; }
        });
    });
    WOVendorcheckboxes.forEach(function (checkbox) {
        var vendors = row.cells.item(7).innerText.split("|");
        vendors.forEach(vendor => {
            if (checkbox.value.trim() != vendor.trim() && checkbox.checked == false) {
                checkbox.checked = false;
            }
            else { checkbox.checked = true; }
        });
    });

    document.getElementById("WorkOrderEmptyContNo").value = row.cells.item(3).innerText;
    document.getElementById("WorkOrderTruckNo").value = row.cells.item(4).innerText;
    document.getElementById("WorkOrderEquipmentType").value = row.cells.item(5).innerText;
    document.getElementById("WorkOrderType").disabled = true;
    row.parentNode.removeChild(row);

    EnableDisableTB(document.getElementById("WorkOrderType").value, 'WorkOrderType');
}

function WorkOrderTableCancelData() {
    var WorkOrderTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.WorkOrderWorkOrderNo = document.getElementById("WorkOrderWorkOrderNo").value;
    WorkOrderTableDataList.push(DataFields);

    let WOContcheckboxes = document.querySelectorAll('#WorkOrderContainerNo input[type="checkbox"]');
    let WOEqpcheckboxes = document.querySelectorAll('#WorkOrderEquipment input[type="checkbox"]');
    let WOVendorcheckboxes = document.querySelectorAll('#WorkOrderVendor input[type="checkbox"]');
    if (document.getElementById("MainJobNo").value != "" && document.getElementById("WorkOrderWorkOrderNo").value != "") {
        var CancelConfirm = confirm("Do You want to cancel this Record.");
        if (CancelConfirm) {
            $('#cover-spin').show(0);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../CFS/CFSImport.aspx/WorkOrderTableCancelData",
                data: JSON.stringify({ WorkOrderTableDataList: WorkOrderTableDataList }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "Cancelled") {
                        WorkOrderTableSearchData();
                        document.getElementById("WorkOrderType").value = "";
                        document.getElementById("WorkOrderWorkOrderNo").value = "";
                        document.getElementById("WorkOrderWorkOrderDate").value = "";
                        document.getElementById("WorkOrderEmptyContNo").value = "";
                        document.getElementById("WorkOrderTruckNo").value = "";
                        document.getElementById("WorkOrderEquipmentType").value = "";

                        WOContcheckboxes.forEach(function (checkbox) {
                            checkbox.disabled = false;
                            checkbox.checked = false;
                        });
                        WOEqpcheckboxes.forEach(function (checkbox) {
                            checkbox.disabled = false;
                            checkbox.checked = false;
                        });
                        WOVendorcheckboxes.forEach(function (checkbox) {
                            checkbox.disabled = false;
                            checkbox.checked = false;
                        });

                        document.getElementById("WorkOrderType").disabled = false;
                        showErrorPopup(data.d, 'green');
                    }
                    else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                    else { showErrorPopup(data.d, 'red'); }
                    setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                },
                error: function (result) {
                    showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                }
            });
        }
    }
    else {
        showErrorPopup('Kindly Enter Job & Work Order No.', 'red');
        return;
    }
}

function WorkOrderTableSearchData() {
    var WorkOrderTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    WorkOrderTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/WorkOrderTableSearchData",
        data: JSON.stringify({ WorkOrderTableDataList: WorkOrderTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("WorkOrderTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("WorkOrderTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='WorkOrderTableEditData(this)' style='color=blue;'><b><u>" + value.WorkOrderType + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.WorkOrderWorkOrderNo;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.WorkOrderWorkOrderDate;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.WorkOrderContainerNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.WorkOrderTruckNo;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.WorkOrderEquipmentType;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.WorkOrderEquipment;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.WorkOrderVendor;
                if (value.WorkOrderWorkOrderNo != "" && value.WorkOrderWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.WorkOrderType == "Empty Truck Out") {
                        if (IGM_No != "" && value.WorkOrderWorkOrderNo != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.WorkOrderWorkOrderNo.trim() + "&Type=ETO' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "Seal Cutting") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=SC' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "Examination") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=EX' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "Section-49") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=S49' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "De-Stuffing") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=DST' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "Loaded Container Out") {
                        if (IGM_No != "" && value.WorkOrderWorkOrderNo != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.WorkOrderWorkOrderNo.trim() + "&Type=LCO' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "Empty Container Out") {
                        if (IGM_No != "" && value.WorkOrderWorkOrderNo != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.WorkOrderWorkOrderNo.trim() + "&Type=EPCO' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else if (value.WorkOrderType == "FCL Cargo Out") {
                        if (IGM_No != "" && value.WorkOrderWorkOrderNo != "") {
                            var cell8 = row.insertCell(8); cell8.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.WorkOrderWorkOrderNo.trim() + "&Type=FCL' target='_blank'>Print</a>";
                        }
                        else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "No Data Found." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                }
                else { var cell8 = row.insertCell(8); cell8.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order has not been created." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function SealCuttingTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("SealCutWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("SealCutWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("SealCutContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("SealCutCFSSealNo").value = row.cells.item(3).innerText;
    document.getElementById("SealCutVendor").value = row.cells.item(4).innerText;
    document.getElementById("SealCutWorkOrderStatus").value = row.cells.item(5).innerText;
    row.parentNode.removeChild(row);
}

function SealCuttingTableUpdateData() {
    var SealCuttingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.SealCutWorkOrderNo = document.getElementById("SealCutWorkOrderNo").value;
    DataFields.SealCutWorkOrderDate = document.getElementById("SealCutWorkOrderDate").value;
    DataFields.SealCutContainerNo = document.getElementById("SealCutContainerNo").value;
    DataFields.SealCutCFSSealNo = document.getElementById("SealCutCFSSealNo").value;
    DataFields.SealCutVendor = document.getElementById("SealCutVendor").value;
    DataFields.SealCutWorkOrderStatus = document.getElementById("SealCutWorkOrderStatus").value;
    SealCuttingTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("SealCutWorkOrderNo").value != "" && document.getElementById("SealCutWorkOrderDate").value != "" &&
        document.getElementById("SealCutContainerNo").value != "" && document.getElementById("SealCutCFSSealNo").value != "" && document.getElementById("SealCutWorkOrderStatus").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/SealCuttingTableUpdateData",
            data: JSON.stringify({ SealCuttingTableDataList: SealCuttingTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    SealCuttingTableSearchData();
                    document.getElementById("SealCutWorkOrderNo").value = "";
                    document.getElementById("SealCutWorkOrderDate").value = "";
                    document.getElementById("SealCutContainerNo").value = "";
                    document.getElementById("SealCutCFSSealNo").value = "";
                    document.getElementById("SealCutVendor").value = "";
                    document.getElementById("SealCutWorkOrderStatus").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function SealCuttingTableSearchData() {
    var SealCuttingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    SealCuttingTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/SealCuttingTableSearchData",
        data: JSON.stringify({ SealCuttingTableDataList: SealCuttingTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("SealCuttingTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("SealCuttingTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.SealCutWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.SealCutWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='SealCuttingTableEditData(this)' style='color=blue;'><b><u>" + value.SealCutWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.SealCutWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.SealCutContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.SealCutCFSSealNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.SealCutVendor;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.SealCutWorkOrderStatus;

                if (value.SealCutWorkOrderNo != "" && value.SealCutWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.SealCutWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell6 = row.insertCell(6); cell6.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=SC' target='_blank'>Print</a>";
                        }
                        else { var cell6 = row.insertCell(6); cell6.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell6 = row.insertCell(6); cell6.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function ExaminationTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("ExamWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("ExamWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("ExamContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("ExamCFSSealNo").value = row.cells.item(3).innerText;
    document.getElementById("ExamScrapLabour").value = row.cells.item(4).innerText;
    document.getElementById("ExamExamedPkgs").value = row.cells.item(5).innerText;
    document.getElementById("ExamExamedPerc").value = row.cells.item(6).innerText;
    document.getElementById("ExamStartDateTime").value = row.cells.item(7).innerText;
    document.getElementById("ExamEndDateTime").value = row.cells.item(8).innerText;
    document.getElementById("ExamEquipment").value = row.cells.item(9).innerText;
    document.getElementById("ExamVendor").value = row.cells.item(10).innerText;
    document.getElementById("ExamWorkOrderStatus").value = row.cells.item(11).innerText;
    row.parentNode.removeChild(row);
}

function ExaminationTableUpdateData() {
    var ExaminationTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ExamWorkOrderNo = document.getElementById("ExamWorkOrderNo").value;
    DataFields.ExamWorkOrderDate = document.getElementById("ExamWorkOrderDate").value;
    DataFields.ExamContainerNo = document.getElementById("ExamContainerNo").value;
    DataFields.ExamCFSSealNo = document.getElementById("ExamCFSSealNo").value;
    DataFields.ExamScrapLabour = document.getElementById("ExamScrapLabour").value;
    DataFields.ExamExamedPkgs = document.getElementById("ExamExamedPkgs").value;
    DataFields.ExamExamedPerc = document.getElementById("ExamExamedPerc").value;
    DataFields.ExamStartDateTime = document.getElementById("ExamStartDateTime").value;
    DataFields.ExamEndDateTime = document.getElementById("ExamEndDateTime").value;
    DataFields.ExamEquipment = document.getElementById("ExamEquipment").value;
    DataFields.ExamVendor = document.getElementById("ExamVendor").value;
    DataFields.ExamWorkOrderStatus = document.getElementById("ExamWorkOrderStatus").value;
    ExaminationTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("ExamWorkOrderNo").value != "" && document.getElementById("ExamWorkOrderDate").value != "" &&
        document.getElementById("ExamContainerNo").value != "" && document.getElementById("ExamCFSSealNo").value != "" && document.getElementById("ExamExamedPerc").value != "" &&
        document.getElementById("ExamExamedPkgs").value != "" && document.getElementById("ExamStartDateTime").value != "" && document.getElementById("ExamEndDateTime").value != "" &&
        document.getElementById("ExamWorkOrderStatus").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ExaminationTableUpdateData",
            data: JSON.stringify({ ExaminationTableDataList: ExaminationTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    ExaminationTableSearchData();
                    document.getElementById("ExamWorkOrderNo").value = "";
                    document.getElementById("ExamWorkOrderDate").value = "";
                    document.getElementById("ExamContainerNo").value = "";
                    document.getElementById("ExamCFSSealNo").value = "";
                    document.getElementById("ExamScrapLabour").value = "";
                    document.getElementById("ExamExamedPkgs").value = "";
                    document.getElementById("ExamExamedPerc").value = "";
                    document.getElementById("ExamStartDateTime").value = "";
                    document.getElementById("ExamEndDateTime").value = "";
                    document.getElementById("ExamEquipment").value = "";
                    document.getElementById("ExamVendor").value = "";
                    document.getElementById("ExamWorkOrderStatus").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function ExaminationTableSearchData() {
    var ExaminationTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    ExaminationTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/ExaminationTableSearchData",
        data: JSON.stringify({ ExaminationTableDataList: ExaminationTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("ExaminationTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("ExaminationTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.ExamWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.ExamWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='ExaminationTableEditData(this)' style='color=blue;'><b><u>" + value.ExamWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.ExamWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.ExamContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.ExamCFSSealNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.ExamScrapLabour;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.ExamExamedPkgs;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.ExamExamedPerc;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.ExamStartDateTime;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.ExamEndDateTime;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.ExamEquipment;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.ExamVendor;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.ExamWorkOrderStatus;

                if (value.ExamWorkOrderNo != "" && value.ExamWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.ExamWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell12 = row.insertCell(12); cell12.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=EX' target='_blank'>Print</a>";
                        }
                        else { var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function StuffingTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("StuffWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("StuffWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("StuffDeStuffingContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("StuffStuffingContainerNo").value = row.cells.item(3).innerText;
    document.getElementById("StuffDeclaredPkgs").value = row.cells.item(4).innerText;
    document.getElementById("StuffDeclaredWeight").value = row.cells.item(5).innerText;
    document.getElementById("StuffStuffedFrom").value = row.cells.item(6).innerText;
    document.getElementById("StuffStuffedTo").value = row.cells.item(7).innerText;
    document.getElementById("StuffStuffedWeight").value = row.cells.item(8).innerText;
    document.getElementById("StuffStuffedPkgs").value = row.cells.item(9).innerText;
    document.getElementById("StuffRemarks").value = row.cells.item(10).innerText;
    document.getElementById("StuffWorkOrderStatus").value = row.cells.item(11).innerText;
    row.parentNode.removeChild(row);
}

//// Update weight based on package input
//function updateWeight(PkgId, WeightId, DeclWeight, DeclPkgs, BalPkg, BalWeight, Tabid, Contid) {
//    var packageInput = document.getElementById(PkgId).value, TotWeight = 0;

//    var StuffTable = document.getElementById(Tabid);
//    for (var i = 1; i < StuffTable.rows.length; i++) {
//        if (StuffTable.rows[i].cells.item(2).innerText.trim() == document.getElementById(Contid).value.trim()) {
//            if (StuffTable.rows[i].cells.item(9).innerText.trim() != "" && StuffTable.rows[i].cells.item(9).innerText.trim() != "0" && StuffTable.rows[i].cells.item(9).innerText.trim() != null) {
//                packageInput = parseFloat(packageInput) + parseFloat(StuffTable.rows[i].cells.item(9).innerText.trim());
//                TotWeight = parseFloat(TotWeight) + parseFloat(StuffTable.rows[i].cells.item(8).innerText.trim());
//            }
//        }
//    }

//    const weightInput = document.getElementById(WeightId);
//    const baseWeight = parseFloat(document.getElementById(DeclWeight).value);

//    if (packageInput > parseFloat(document.getElementById(DeclPkgs).value)) {
//        document.getElementById(PkgId).value = 0;
//        showErrorPopup('The number of entered packages exceeds the declared number of packages.', 'red');
//    }

//    // Calculate the new weight
//    const newWeight = baseWeight * document.getElementById(PkgId).value;
//    // Update the weight field
//    weightInput.value = newWeight.toFixed(2); // To limit to 2 decimals if needed

//    document.getElementById(BalPkg).value = (parseFloat(document.getElementById(DeclPkgs).value) - parseFloat(packageInput)).toFixed(2);
//    document.getElementById(BalWeight).value = (parseFloat(document.getElementById(DeclWeight).value) - (parseFloat(weightInput.value) + parseFloat(TotWeight))).toFixed(2);

//    if (packageInput == "0") {
//        document.getElementById(BalPkg).value = "0";
//    }
//    if (weightInput.value == "0") {
//        document.getElementById(BalWeight).value = "0";
//    }
//}

//// Update package based on weight input
//function updatePackage(PkgId, WeightId, DeclWeight, DeclPkgs, BalPkg, BalWeight, Tabid, Contid) {
//    var weightInput = document.getElementById(WeightId).value, TotPkgs = 0;

//    var StuffTable = document.getElementById(Tabid);
//    for (var i = 1; i < StuffTable.rows.length; i++) {
//        if (StuffTable.rows[i].cells.item(2).innerText.trim() == document.getElementById(Contid).value.trim()) {
//            if (StuffTable.rows[i].cells.item(8).innerText.trim() != "" && StuffTable.rows[i].cells.item(8).innerText.trim() != "0" && StuffTable.rows[i].cells.item(8).innerText.trim() != null) {
//                weightInput = parseFloat(weightInput) + parseFloat(StuffTable.rows[i].cells.item(8).innerText.trim());
//                TotPkgs = parseFloat(TotPkgs) + parseFloat(StuffTable.rows[i].cells.item(9).innerText.trim());
//            }
//        }
//    }

//    const packageInput = document.getElementById(PkgId);
//    const baseWeight = parseFloat(document.getElementById(DeclWeight).value);

//    if (weightInput > parseFloat(document.getElementById(DeclWeight).value)) {
//        document.getElementById(WeightId).value = 0;
//        showErrorPopup('The entered weight exceeds the declared weight.', 'red');
//    }

//    // Calculate the new package count
//    const newPackage = document.getElementById(WeightId).value / baseWeight;
//    // Update the package field
//    packageInput.value = newPackage.toFixed(2); // To limit to 2 decimals if needed

//    document.getElementById(BalPkg).value = (parseFloat(document.getElementById(DeclPkgs).value) - (parseFloat(packageInput.value) + parseFloat(TotPkgs))).toFixed(2);
//    document.getElementById(BalWeight).value = (parseFloat(document.getElementById(DeclWeight).value) - parseFloat(weightInput)).toFixed(2);

//    if (packageInput.value == "0") {
//        document.getElementById(BalPkg).value = "0";
//    }
//    if (weightInput == "0") {
//        document.getElementById(BalWeight).value = "0";
//    }
//}

function CalculateValues(e) {
    function formatValue(value) {
        return parseFloat(value).toFixed(2) === "0.00" ? "0" : parseFloat(value).toFixed(2);
    }

    if (e.id == "DeStuffDeStuffedPkgs") {
        const DecWeight = parseFloat(document.getElementById("DeStuffDeclaredWeight").value),
            DecPkgs = parseFloat(document.getElementById("DeStuffDeclaredPkgs").value);

        var WeightPerQty = DecWeight / DecPkgs;

        document.getElementById("DeStuffBalancePkgs").value = formatValue(DecPkgs - parseFloat(e.value));
        document.getElementById("DeStuffBalanceWeight").value = formatValue(DecWeight - (parseFloat(e.value) * WeightPerQty));
        document.getElementById("DeStuffDeStuffedWeight").value = formatValue(parseFloat(e.value) * WeightPerQty);
    }

    if (e.id == "StuffStuffedPkgs") {
        const DecWeight = parseFloat(document.getElementById("StuffDeclaredWeight").value),
            DecPkgs = parseFloat(document.getElementById("StuffDeclaredPkgs").value);

        var WeightPerQty = DecWeight / DecPkgs;

        document.getElementById("StuffBalancePkgs").value = formatValue(DecPkgs - parseFloat(e.value));
        document.getElementById("StuffBalanceWeight").value = formatValue(DecWeight - (parseFloat(e.value) * WeightPerQty));
        document.getElementById("StuffStuffedWeight").value = formatValue(parseFloat(e.value) * WeightPerQty);
    }
}

function StuffingTableUpdateData() {
    var StuffingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.StuffWorkOrderNo = document.getElementById("StuffWorkOrderNo").value;
    DataFields.StuffWorkOrderDate = document.getElementById("StuffWorkOrderDate").value;
    DataFields.StuffDeStuffingContainerNo = document.getElementById("StuffDeStuffingContainerNo").value;
    DataFields.StuffStuffingContainerNo = document.getElementById("StuffStuffingContainerNo").value;
    DataFields.StuffDeclaredPkgs = document.getElementById("StuffDeclaredPkgs").value;
    DataFields.StuffDeclaredWeight = document.getElementById("StuffDeclaredWeight").value;
    DataFields.StuffStuffedFrom = document.getElementById("StuffStuffedFrom").value;
    DataFields.StuffStuffedTo = document.getElementById("StuffStuffedTo").value;
    DataFields.StuffStuffedWeight = document.getElementById("StuffStuffedWeight").value;
    DataFields.StuffStuffedPkgs = document.getElementById("StuffStuffedPkgs").value;
    DataFields.StuffRemarks = document.getElementById("StuffRemarks").value;
    DataFields.StuffWorkOrderStatus = document.getElementById("StuffWorkOrderStatus").value;
    DataFields.StuffBalancePkgs = document.getElementById("StuffBalancePkgs").value;
    DataFields.StuffBalanceWeight = document.getElementById("StuffBalanceWeight").value;
    StuffingTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("StuffWorkOrderNo").value != "" && document.getElementById("StuffWorkOrderDate").value != "" &&
        document.getElementById("StuffDeStuffingContainerNo").value != "" && document.getElementById("StuffStuffingContainerNo").value != "" && document.getElementById("StuffStuffedFrom").value != "" &&
        document.getElementById("StuffStuffedTo").value != "" && document.getElementById("StuffStuffedWeight").value != "" && document.getElementById("StuffStuffedPkgs").value != "" &&
        document.getElementById("StuffWorkOrderStatus").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/StuffingTableUpdateData",
            data: JSON.stringify({ StuffingTableDataList: StuffingTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    StuffingTableSearchData();
                    document.getElementById("StuffWorkOrderNo").value = "";
                    document.getElementById("StuffWorkOrderDate").value = "";
                    document.getElementById("StuffDeStuffingContainerNo").value = "";
                    document.getElementById("StuffStuffingContainerNo").value = "";
                    document.getElementById("StuffDeclaredPkgs").value = "";
                    document.getElementById("StuffDeclaredWeight").value = "";
                    document.getElementById("StuffStuffedFrom").value = "";
                    document.getElementById("StuffStuffedTo").value = "";
                    document.getElementById("StuffStuffedWeight").value = "";
                    document.getElementById("StuffStuffedPkgs").value = "";
                    document.getElementById("StuffRemarks").value = "";
                    document.getElementById("StuffWorkOrderStatus").value = "";
                    document.getElementById("StuffBalancePkgs").value = "";
                    document.getElementById("StuffBalanceWeight").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function StuffingTableSearchData() {
    var StuffingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    StuffingTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/StuffingTableSearchData",
        data: JSON.stringify({ StuffingTableDataList: StuffingTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("StuffingTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("StuffingTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.StuffWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.StuffWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='StuffingTableEditData(this)' style='color=blue;'><b><u>" + value.StuffWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.StuffWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.StuffDeStuffingContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.StuffStuffingContainerNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.StuffDeclaredPkgs;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.StuffDeclaredWeight;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.StuffStuffedFrom;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.StuffStuffedTo;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.StuffStuffedWeight;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.StuffStuffedPkgs;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.StuffRemarks;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.StuffWorkOrderStatus;

                if (value.StuffWorkOrderNo != "" && value.StuffWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.StuffWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell12 = row.insertCell(12); cell12.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=S49' target='_blank'>Print</a>";
                        }
                        else { var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function DeStuffingTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("DeStuffWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("DeStuffWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("DeStuffContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("DeStuffFromDate").value = row.cells.item(3).innerText;
    document.getElementById("DeStuffToDate").value = row.cells.item(4).innerText;
    document.getElementById("DeStuffContainerCondition").value = row.cells.item(5).innerText;
    document.getElementById("DeStuffDeclaredWeight").value = row.cells.item(6).innerText;
    document.getElementById("DeStuffDeclaredPkgs").value = row.cells.item(7).innerText;
    document.getElementById("DeStuffDeStuffedWeight").value = row.cells.item(8).innerText;
    document.getElementById("DeStuffDeStuffedPkgs").value = row.cells.item(9).innerText;
    document.getElementById("DeStuffDestuffMarkNo").value = row.cells.item(10).innerText;
    document.getElementById("DeStuffDestuffLocation").value = row.cells.item(11).innerText;
    document.getElementById("DeStuffAreainSqmt").value = row.cells.item(12).innerText;
    document.getElementById("DeStuffVolume").value = row.cells.item(13).innerText;
    document.getElementById("DeStuffMode").value = row.cells.item(14).innerText;
    document.getElementById("DeStuffShort").value = row.cells.item(15).innerText;
    document.getElementById("DeStuffExcess").value = row.cells.item(16).innerText;
    document.getElementById("DeStuffNoofGrids").value = row.cells.item(17).innerText;
    document.getElementById("DeStuffDelayDueTo").value = row.cells.item(18).innerText;
    document.getElementById("DeStuffDelayRemarks").value = row.cells.item(19).innerText;
    document.getElementById("DeStuffContractor").value = row.cells.item(20).innerText;
    document.getElementById("DeStuffSupervisor").value = row.cells.item(21).innerText;
    document.getElementById("DeStuffMarksNo").value = row.cells.item(22).innerText;
    document.getElementById("DeStuffMovementType").value = row.cells.item(23).innerText;
    document.getElementById("DeStuffRemarks").value = row.cells.item(24).innerText;
    document.getElementById("DeStuffWorkOrderStatus").value = row.cells.item(25).innerText;
    row.parentNode.removeChild(row);
}

function DeStuffingTableUpdateData() {
    var DeStuffingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.DeStuffWorkOrderNo = document.getElementById("DeStuffWorkOrderNo").value;
    DataFields.DeStuffWorkOrderDate = document.getElementById("DeStuffWorkOrderDate").value;
    DataFields.DeStuffContainerNo = document.getElementById("DeStuffContainerNo").value;
    //DataFields.DeStuffVehicleNo = document.getElementById("DeStuffVehicleNo").value;
    DataFields.DeStuffFromDate = document.getElementById("DeStuffFromDate").value;
    DataFields.DeStuffToDate = document.getElementById("DeStuffToDate").value;
    DataFields.DeStuffContainerCondition = document.getElementById("DeStuffContainerCondition").value;
    DataFields.DeStuffDeclaredWeight = document.getElementById("DeStuffDeclaredWeight").value;
    DataFields.DeStuffDeclaredPkgs = document.getElementById("DeStuffDeclaredPkgs").value;
    DataFields.DeStuffDeStuffedWeight = document.getElementById("DeStuffDeStuffedWeight").value;
    DataFields.DeStuffDeStuffedPkgs = document.getElementById("DeStuffDeStuffedPkgs").value;
    DataFields.DeStuffDestuffMarkNo = document.getElementById("DeStuffDestuffMarkNo").value;
    DataFields.DeStuffDestuffLocation = document.getElementById("DeStuffDestuffLocation").value;
    DataFields.DeStuffAreainSqmt = document.getElementById("DeStuffAreainSqmt").value;
    DataFields.DeStuffVolume = document.getElementById("DeStuffVolume").value;
    DataFields.DeStuffMode = document.getElementById("DeStuffMode").value;
    DataFields.DeStuffShort = document.getElementById("DeStuffShort").value;
    DataFields.DeStuffExcess = document.getElementById("DeStuffExcess").value;
    DataFields.DeStuffNoofGrids = document.getElementById("DeStuffNoofGrids").value;
    DataFields.DeStuffDelayDueTo = document.getElementById("DeStuffDelayDueTo").value;
    DataFields.DeStuffDelayRemarks = document.getElementById("DeStuffDelayRemarks").value;
    DataFields.DeStuffContractor = document.getElementById("DeStuffContractor").value;
    DataFields.DeStuffSupervisor = document.getElementById("DeStuffSupervisor").value;
    DataFields.DeStuffMarksNo = document.getElementById("DeStuffMarksNo").value;
    DataFields.DeStuffMovementType = document.getElementById("DeStuffMovementType").value;
    DataFields.DeStuffRemarks = document.getElementById("DeStuffRemarks").value;
    DataFields.DeStuffWorkOrderStatus = document.getElementById("DeStuffWorkOrderStatus").value;
    DataFields.DeStuffBalancePkgs = document.getElementById("DeStuffBalancePkgs").value;
    DataFields.DeStuffBalanceWeight = document.getElementById("DeStuffBalanceWeight").value;
    DeStuffingTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("DeStuffWorkOrderNo").value != "" && document.getElementById("DeStuffWorkOrderDate").value != "" &&
        document.getElementById("DeStuffContainerNo").value != "" && document.getElementById("DeStuffFromDate").value != "" && document.getElementById("DeStuffToDate").value != "" &&
        document.getElementById("DeStuffDeclaredWeight").value != "" && document.getElementById("DeStuffDeclaredPkgs").value != "" && document.getElementById("DeStuffDeStuffedWeight").value != "" &&
        document.getElementById("DeStuffDeStuffedPkgs").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/DeStuffingTableUpdateData",
            data: JSON.stringify({ DeStuffingTableDataList: DeStuffingTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    DeStuffingTableSearchData();
                    document.getElementById("DeStuffWorkOrderNo").value = "";
                    document.getElementById("DeStuffWorkOrderDate").value = "";
                    document.getElementById("DeStuffContainerNo").value = "";
                    //document.getElementById("DeStuffVehicleNo").value = "";
                    document.getElementById("DeStuffFromDate").value = "";
                    document.getElementById("DeStuffToDate").value = "";
                    document.getElementById("DeStuffContainerCondition").value = "";
                    document.getElementById("DeStuffDeclaredWeight").value = "";
                    document.getElementById("DeStuffDeclaredPkgs").value = "";
                    document.getElementById("DeStuffDeStuffedWeight").value = "";
                    document.getElementById("DeStuffDeStuffedPkgs").value = "";
                    document.getElementById("DeStuffDestuffMarkNo").value = "";
                    document.getElementById("DeStuffDestuffLocation").value = "";
                    document.getElementById("DeStuffAreainSqmt").value = "";
                    document.getElementById("DeStuffVolume").value = "";
                    document.getElementById("DeStuffMode").value = "";
                    document.getElementById("DeStuffShort").value = "";
                    document.getElementById("DeStuffExcess").value = "";
                    document.getElementById("DeStuffNoofGrids").value = "";
                    document.getElementById("DeStuffDelayDueTo").value = "";
                    document.getElementById("DeStuffDelayRemarks").value = "";
                    document.getElementById("DeStuffContractor").value = "";
                    document.getElementById("DeStuffSupervisor").value = "";
                    document.getElementById("DeStuffMarksNo").value = "";
                    document.getElementById("DeStuffMovementType").value = "";
                    document.getElementById("DeStuffRemarks").value = "";
                    document.getElementById("DeStuffWorkOrderStatus").value = "";
                    document.getElementById("DeStuffBalancePkgs").value = "";
                    document.getElementById("DeStuffBalanceWeight").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function DeStuffingTableSearchData() {
    var DeStuffingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DeStuffingTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/DeStuffingTableSearchData",
        data: JSON.stringify({ DeStuffingTableDataList: DeStuffingTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("DeStuffingTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("DeStuffingTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.DeStuffWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.DeStuffWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='DeStuffingTableEditData(this)' style='color=blue;'><b><u>" + value.DeStuffWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.DeStuffWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.DeStuffContainerNo;
                //var cell3 = row.insertCell(3); cell3.innerHTML = value.DeStuffVehicleNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.DeStuffFromDate;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.DeStuffToDate;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.DeStuffContainerCondition;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.DeStuffDeclaredWeight;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.DeStuffDeclaredPkgs;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.DeStuffDeStuffedWeight;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.DeStuffDeStuffedPkgs;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.DeStuffDestuffMarkNo;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.DeStuffDestuffLocation;
                var cell12 = row.insertCell(12); cell12.innerHTML = value.DeStuffAreainSqmt;
                var cell13 = row.insertCell(13); cell13.innerHTML = value.DeStuffVolume;
                var cell14 = row.insertCell(14); cell14.innerHTML = value.DeStuffMode;
                var cell15 = row.insertCell(15); cell15.innerHTML = value.DeStuffShort;
                var cell16 = row.insertCell(16); cell16.innerHTML = value.DeStuffExcess;
                var cell17 = row.insertCell(17); cell17.innerHTML = value.DeStuffNoofGrids;
                var cell18 = row.insertCell(18); cell18.innerHTML = value.DeStuffDelayDueTo;
                var cell19 = row.insertCell(19); cell19.innerHTML = value.DeStuffDelayRemarks;
                var cell20 = row.insertCell(20); cell20.innerHTML = value.DeStuffContractor;
                var cell21 = row.insertCell(21); cell21.innerHTML = value.DeStuffSupervisor;
                var cell22 = row.insertCell(22); cell22.innerHTML = value.DeStuffMarksNo;
                var cell23 = row.insertCell(23); cell23.innerHTML = value.DeStuffMovementType;
                var cell24 = row.insertCell(24); cell24.innerHTML = value.DeStuffRemarks;
                var cell25 = row.insertCell(25); cell25.innerHTML = value.DeStuffWorkOrderStatus;

                if (value.DeStuffWorkOrderNo != "" && value.DeStuffWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.DeStuffWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && Item_No != "") {
                            var cell26 = row.insertCell(26); cell26.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + Item_No.trim() + "&Type=DST' target='_blank'>Print</a>";
                        }
                        else { var cell26 = row.insertCell(26); cell26.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Item No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell26 = row.insertCell(26); cell26.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function LoadedContainerOutTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("LoadOutWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("LoadOutWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("LoadOutContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("LoadOutVehicleNo").value = row.cells.item(3).innerText;
    document.getElementById("LoadOutCustDutyValue").value = row.cells.item(4).innerText;
    document.getElementById("LoadOutStampDutyValue").value = row.cells.item(5).innerText;
    document.getElementById("LoadOutOpenOrderDate").value = row.cells.item(6).innerText;
    document.getElementById("LoadOutOutofChargeDate").value = row.cells.item(7).innerText;
    document.getElementById("LoadOutCustOutofChargeNo").value = row.cells.item(8).innerText;
    document.getElementById("LoadOutCondition").value = row.cells.item(9).innerText;
    document.getElementById("LoadOutRemarks").value = row.cells.item(10).innerText;
    document.getElementById("LoadOutWorkOrderStatus").value = row.cells.item(11).innerText;
    row.parentNode.removeChild(row);
}

function LoadedContainerOutTableUpdateData() {
    var LoadedContainerOutTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.LoadOutWorkOrderNo = document.getElementById("LoadOutWorkOrderNo").value;
    DataFields.LoadOutWorkOrderDate = document.getElementById("LoadOutWorkOrderDate").value;
    DataFields.LoadOutContainerNo = document.getElementById("LoadOutContainerNo").value;
    DataFields.LoadOutVehicleNo = document.getElementById("LoadOutVehicleNo").value;
    DataFields.LoadOutCustDutyValue = document.getElementById("LoadOutCustDutyValue").value;
    DataFields.LoadOutStampDutyValue = document.getElementById("LoadOutStampDutyValue").value;
    DataFields.LoadOutOpenOrderDate = document.getElementById("LoadOutOpenOrderDate").value;
    DataFields.LoadOutOutofChargeDate = document.getElementById("LoadOutOutofChargeDate").value;
    DataFields.LoadOutCustOutofChargeNo = document.getElementById("LoadOutCustOutofChargeNo").value;
    DataFields.LoadOutCondition = document.getElementById("LoadOutCondition").value;
    DataFields.LoadOutRemarks = document.getElementById("LoadOutRemarks").value;
    DataFields.LoadOutWorkOrderStatus = document.getElementById("LoadOutWorkOrderStatus").value;
    LoadedContainerOutTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("LoadOutWorkOrderNo").value != "" && document.getElementById("LoadOutWorkOrderDate").value != "" &&
        document.getElementById("LoadOutContainerNo").value != "" && document.getElementById("LoadOutVehicleNo").value != "" && document.getElementById("LoadOutOpenOrderDate").value != "" &&
        document.getElementById("LoadOutOutofChargeDate").value != "" && document.getElementById("LoadOutCustOutofChargeNo").value != "" && document.getElementById("LoadOutWorkOrderStatus").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LoadedContainerOutTableUpdateData",
            data: JSON.stringify({ LoadedContainerOutTableDataList: LoadedContainerOutTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    LoadedContainerOutTableSearchData();
                    document.getElementById("LoadOutWorkOrderNo").value = "";
                    document.getElementById("LoadOutWorkOrderDate").value = "";
                    document.getElementById("LoadOutContainerNo").value = "";
                    document.getElementById("LoadOutVehicleNo").value = "";
                    document.getElementById("LoadOutCustDutyValue").value = "";
                    document.getElementById("LoadOutStampDutyValue").value = "";
                    document.getElementById("LoadOutOpenOrderDate").value = "";
                    document.getElementById("LoadOutOutofChargeDate").value = "";
                    document.getElementById("LoadOutCustOutofChargeNo").value = "";
                    document.getElementById("LoadOutCondition").value = "";
                    document.getElementById("LoadOutRemarks").value = "";
                    document.getElementById("LoadOutWorkOrderStatus").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function LoadedContainerOutTableSearchData() {
    var LoadedContainerOutTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    LoadedContainerOutTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/LoadedContainerOutTableSearchData",
        data: JSON.stringify({ LoadedContainerOutTableDataList: LoadedContainerOutTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("LoadedContainerOutTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("LoadedContainerOutTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.LoadOutWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.LoadOutWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='LoadedContainerOutTableEditData(this)' style='color=blue;'><b><u>" + value.LoadOutWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.LoadOutWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.LoadOutContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.LoadOutVehicleNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.LoadOutCustDutyValue;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.LoadOutStampDutyValue;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.LoadOutOpenOrderDate;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.LoadOutOutofChargeDate;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.LoadOutCustOutofChargeNo;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.LoadOutCondition;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.LoadOutRemarks;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.LoadOutWorkOrderStatus;

                if (value.LoadOutWorkOrderNo != "" && value.LoadOutWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.LoadOutWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && value.LoadOutWorkOrderNo != "") {
                            var cell12 = row.insertCell(12); cell12.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.LoadOutWorkOrderNo.trim() + "&Type=LCO' target='_blank'>Print</a>";
                        }
                        else { var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function EmptyContainerOutTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("MtyOutWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("MtyOutWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("MtyOutContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("MtyOutVehicleNo").value = row.cells.item(3).innerText;
    document.getElementById("MtyOutModeofGateOut").value = row.cells.item(4).innerText;
    document.getElementById("MtyOutCycle").value = row.cells.item(5).innerText;
    document.getElementById("MtyOutDriverName").value = row.cells.item(6).innerText;
    document.getElementById("MtyOutEquipmentCondition").value = row.cells.item(7).innerText;
    document.getElementById("MtyOutContainerTag").value = row.cells.item(8).innerText;
    document.getElementById("MtyOutRemarks").value = row.cells.item(9).innerText;
    document.getElementById("MtyOutWorkOrderStatus").value = row.cells.item(10).innerText;
    document.getElementById("MtyMovementBy").value = row.cells.item(11).innerText;
    row.parentNode.removeChild(row);
}

function EmptyContainerOutTableUpdateData() {
    var EmptyContainerOutTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.MtyOutWorkOrderNo = document.getElementById("MtyOutWorkOrderNo").value;
    DataFields.MtyOutWorkOrderDate = document.getElementById("MtyOutWorkOrderDate").value;
    DataFields.MtyOutContainerNo = document.getElementById("MtyOutContainerNo").value;
    DataFields.MtyOutVehicleNo = document.getElementById("MtyOutVehicleNo").value;
    DataFields.MtyOutModeofGateOut = document.getElementById("MtyOutModeofGateOut").value;
    DataFields.MtyOutCycle = document.getElementById("MtyOutCycle").value;
    DataFields.MtyOutDriverName = document.getElementById("MtyOutDriverName").value;
    DataFields.MtyOutEquipmentCondition = document.getElementById("MtyOutEquipmentCondition").value;
    DataFields.MtyOutContainerTag = document.getElementById("MtyOutContainerTag").value;
    DataFields.MtyOutRemarks = document.getElementById("MtyOutRemarks").value;
    DataFields.MtyOutWorkOrderStatus = document.getElementById("MtyOutWorkOrderStatus").value;
    DataFields.MtyMovementBy = document.getElementById("MtyMovementBy").value;
    EmptyContainerOutTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("MtyOutWorkOrderNo").value != "" && document.getElementById("MtyOutWorkOrderDate").value != "" &&
        document.getElementById("MtyOutContainerNo").value != "" && document.getElementById("MtyOutVehicleNo").value != "" && document.getElementById("MtyOutWorkOrderStatus").value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/EmptyContainerOutTableUpdateData",
            data: JSON.stringify({ EmptyContainerOutTableDataList: EmptyContainerOutTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    EmptyContainerOutTableSearchData();
                    document.getElementById("MtyOutWorkOrderNo").value = "";
                    document.getElementById("MtyOutWorkOrderDate").value = "";
                    document.getElementById("MtyOutContainerNo").value = "";
                    document.getElementById("MtyOutVehicleNo").value = "";
                    document.getElementById("MtyOutModeofGateOut").value = "";
                    document.getElementById("MtyOutCycle").value = "";
                    document.getElementById("MtyOutDriverName").value = "";
                    document.getElementById("MtyOutEquipmentCondition").value = "";
                    document.getElementById("MtyOutContainerTag").value = "";
                    document.getElementById("MtyOutRemarks").value = "";
                    document.getElementById("MtyOutWorkOrderStatus").value = "";
                    document.getElementById("MtyMovementBy").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function EmptyContainerOutTableSearchData() {
    var EmptyContainerOutTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    EmptyContainerOutTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/EmptyContainerOutTableSearchData",
        data: JSON.stringify({ EmptyContainerOutTableDataList: EmptyContainerOutTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("EmptyContainerOutTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("EmptyContainerOutTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.MtyOutWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.MtyOutWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='EmptyContainerOutTableEditData(this)' style='color=blue;'><b><u>" + value.MtyOutWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.MtyOutWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.MtyOutContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.MtyOutVehicleNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.MtyOutModeofGateOut;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.MtyOutCycle;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.MtyOutDriverName;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.MtyOutEquipmentCondition;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.MtyOutContainerTag;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.MtyOutRemarks;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.MtyOutWorkOrderStatus;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.MtyMovementBy;

                if (value.MtyOutWorkOrderNo != "" && value.MtyOutWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.MtyOutWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && value.MtyOutWorkOrderNo != "") {
                            var cell12 = row.insertCell(12); cell12.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.MtyOutWorkOrderNo.trim() + "&Type=EPCO' target='_blank'>Print</a>";
                        }
                        else { var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell12 = row.insertCell(12); cell12.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function FCLCargoOutTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("FCLOutWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("FCLOutWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("FCLOutContainerNo").value = row.cells.item(2).innerText;
    document.getElementById("FCLOutVehicleNo").value = row.cells.item(3).innerText;
    document.getElementById("FCLOutManifestPackages").value = row.cells.item(4).innerText;
    document.getElementById("FCLOutManifestWeight").value = row.cells.item(5).innerText;
    document.getElementById("FCLOutBalancePackages").value = row.cells.item(6).innerText;
    document.getElementById("FCLOutBalanceWeight").value = row.cells.item(7).innerText;
    document.getElementById("FCLOutDestuffedWeight").value = row.cells.item(8).innerText; // FCLOutDestuffedWeight
    document.getElementById("FCLOutDestuffedPkgs").value = row.cells.item(9).innerText; // FCLOutDestuffedPkgs
    document.getElementById("FCLOutDestuffedFrom").value = row.cells.item(10).innerText; // FCLOutDestuffedFrom
    document.getElementById("FCLOutDestuffedTo").value = row.cells.item(11).innerText; // FCLOutDestuffedTo
    document.getElementById("FCLOutCustDutyValue").value = row.cells.item(12).innerText;
    document.getElementById("FCLOutStampDutyValue").value = row.cells.item(13).innerText;
    document.getElementById("FCLOutOOCNo").value = row.cells.item(14).innerText;
    document.getElementById("FCLOutOOCDate").value = row.cells.item(15).innerText;
    document.getElementById("FCLOutTallyDetails").value = row.cells.item(16).innerText;
    document.getElementById("FCLOutEquipment").value = row.cells.item(17).innerText;
    document.getElementById("FCLOutVendor").value = row.cells.item(18).innerText;
    document.getElementById("FCLOutRemarks").value = row.cells.item(19).innerText;
    document.getElementById("FCLOutWorkOrderStatus").value = row.cells.item(20).innerText;
    document.getElementById("WorkOrderNo2").value = row.cells.item(21).innerText;
    row.parentNode.removeChild(row);

    updateWeight('FCLOutDestuffedPkgs', 'FCLOutDestuffedWeight', 'FCLOutManifestWeight', 'FCLOutManifestPackages', 'FCLOutBalancePackages', 'FCLOutBalanceWeight', 'FCLCargoOutTable', 'FCLOutContainerNo');
}

function FCLCargoOutTableUpdateData() {
    var FCLCargoOutTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.FCLOutWorkOrderNo = document.getElementById("FCLOutWorkOrderNo").value;
    DataFields.FCLOutWorkOrderDate = document.getElementById("FCLOutWorkOrderDate").value;
    DataFields.FCLOutContainerNo = document.getElementById("FCLOutContainerNo").value;
    DataFields.FCLOutVehicleNo = document.getElementById("FCLOutVehicleNo").value;
    DataFields.FCLOutManifestPackages = document.getElementById("FCLOutManifestPackages").value;
    DataFields.FCLOutManifestWeight = document.getElementById("FCLOutManifestWeight").value;
    DataFields.FCLOutBalancePackages = document.getElementById("FCLOutBalancePackages").value;
    DataFields.FCLOutBalanceWeight = document.getElementById("FCLOutBalanceWeight").value;
    DataFields.FCLOutDestuffedFrom = document.getElementById("FCLOutDestuffedFrom").value;
    DataFields.FCLOutDestuffedTo = document.getElementById("FCLOutDestuffedTo").value;
    DataFields.FCLOutDestuffedPkgs = document.getElementById("FCLOutDestuffedPkgs").value;
    DataFields.FCLOutDestuffedWeight = document.getElementById("FCLOutDestuffedWeight").value;
    DataFields.FCLOutCustDutyValue = document.getElementById("FCLOutCustDutyValue").value;
    DataFields.FCLOutStampDutyValue = document.getElementById("FCLOutStampDutyValue").value;
    DataFields.FCLOutOOCNo = document.getElementById("FCLOutOOCNo").value;
    DataFields.FCLOutOOCDate = document.getElementById("FCLOutOOCDate").value;
    DataFields.FCLOutTallyDetails = document.getElementById("FCLOutTallyDetails").value;
    DataFields.FCLOutEquipment = document.getElementById("FCLOutEquipment").value;
    DataFields.FCLOutVendor = document.getElementById("FCLOutVendor").value;
    DataFields.FCLOutRemarks = document.getElementById("FCLOutRemarks").value;
    DataFields.FCLOutWorkOrderStatus = document.getElementById("FCLOutWorkOrderStatus").value;
    DataFields.DeStuffWorkOrderNo = document.getElementById("WorkOrderNo2").value;
    FCLCargoOutTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("FCLOutWorkOrderNo").value != "" && document.getElementById("FCLOutWorkOrderDate").value != "" &&
        document.getElementById("FCLOutContainerNo").value != "" && document.getElementById("FCLOutVehicleNo").value != "" && document.getElementById("FCLOutDestuffedFrom").value != "" &&
        document.getElementById("FCLOutDestuffedTo").value != "" && document.getElementById("FCLOutDestuffedPkgs").value != "" && document.getElementById("FCLOutDestuffedWeight").value != "" &&
        document.getElementById("FCLOutWorkOrderStatus").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/FCLCargoOutTableUpdateData",
            data: JSON.stringify({ FCLCargoOutTableDataList: FCLCargoOutTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    FCLCargoOutTableSearchData();
                    document.getElementById("FCLOutWorkOrderNo").value = "";
                    document.getElementById("FCLOutWorkOrderDate").value = "";
                    document.getElementById("FCLOutContainerNo").value = "";
                    document.getElementById("FCLOutVehicleNo").value = "";
                    document.getElementById("FCLOutManifestPackages").value = "";
                    document.getElementById("FCLOutManifestWeight").value = "";
                    document.getElementById("FCLOutBalancePackages").value = "";
                    document.getElementById("FCLOutBalanceWeight").value = "";
                    document.getElementById("FCLOutDestuffedFrom").value = "";
                    document.getElementById("FCLOutDestuffedTo").value = "";
                    document.getElementById("FCLOutDestuffedPkgs").value = "";
                    document.getElementById("FCLOutDestuffedWeight").value = "";
                    document.getElementById("FCLOutCustDutyValue").value = "";
                    document.getElementById("FCLOutStampDutyValue").value = "";
                    document.getElementById("FCLOutOOCNo").value = "";
                    document.getElementById("FCLOutOOCDate").value = "";
                    document.getElementById("FCLOutTallyDetails").value = "";
                    document.getElementById("FCLOutEquipment").value = "";
                    document.getElementById("FCLOutVendor").value = "";
                    document.getElementById("FCLOutRemarks").value = "";
                    document.getElementById("FCLOutWorkOrderStatus").value = "";
                    document.getElementById("WorkOrderNo2").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function FCLCargoOutTableSearchData() {
    var FCLCargoOutTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    FCLCargoOutTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/FCLCargoOutTableSearchData",
        data: JSON.stringify({ FCLCargoOutTableDataList: FCLCargoOutTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("FCLCargoOutTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("FCLCargoOutTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                if (value.FCLOutWorkOrderStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = value.FCLOutWorkOrderNo; }
                else { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='FCLCargoOutTableEditData(this)' style='color=blue;'><b><u>" + value.FCLOutWorkOrderNo + "</u></b></span>"; }
                var cell1 = row.insertCell(1); cell1.innerHTML = value.FCLOutWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.FCLOutContainerNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.FCLOutVehicleNo;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.FCLOutManifestPackages;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.FCLOutManifestWeight;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.FCLOutBalancePackages; cell6.style.display = "none";
                var cell7 = row.insertCell(7); cell7.innerHTML = value.FCLOutBalanceWeight; cell7.style.display = "none";
                var cell8 = row.insertCell(8); cell8.innerHTML = value.FCLOutDestuffedWeight; // FCLOutDestuffedWeight
                var cell9 = row.insertCell(9); cell9.innerHTML = value.FCLOutDestuffedPkgs; // FCLOutDestuffedPkgs
                var cell10 = row.insertCell(10); cell10.innerHTML = value.FCLOutDestuffedFrom; // FCLOutDestuffedFrom
                var cell11 = row.insertCell(11); cell11.innerHTML = value.FCLOutDestuffedTo; // FCLOutDestuffedTo
                var cell12 = row.insertCell(12); cell12.innerHTML = value.FCLOutCustDutyValue;
                var cell13 = row.insertCell(13); cell13.innerHTML = value.FCLOutStampDutyValue;
                var cell14 = row.insertCell(14); cell14.innerHTML = value.FCLOutOOCNo;
                var cell15 = row.insertCell(15); cell15.innerHTML = value.FCLOutOOCDate;
                var cell16 = row.insertCell(16); cell16.innerHTML = value.FCLOutTallyDetails;
                var cell17 = row.insertCell(17); cell17.innerHTML = value.FCLOutEquipment;
                var cell18 = row.insertCell(18); cell18.innerHTML = value.FCLOutVendor;
                var cell19 = row.insertCell(19); cell19.innerHTML = value.FCLOutRemarks;
                var cell20 = row.insertCell(20); cell20.innerHTML = value.FCLOutWorkOrderStatus;
                var cell21 = row.insertCell(21); cell21.innerHTML = value.DeStuffWorkOrderNo; cell21.style.display = "none";

                if (value.FCLOutWorkOrderNo != "" && value.FCLOutWorkOrderNo != null) {
                    var IGM_No = document.getElementById("MainIGMNo").value, Item_No = document.getElementById("MainItemNo").value;

                    if (value.FCLOutWorkOrderStatus == "Completed") {
                        if (IGM_No != "" && value.FCLOutWorkOrderNo != "") {
                            var cell22 = row.insertCell(22); cell22.innerHTML = "<a href='../CFS/WorkOrderPrint.aspx?IGM=" + IGM_No.trim() + "&Item=" + value.FCLOutWorkOrderNo.trim() + "&Type=FCL' target='_blank'>Print</a>";
                        }
                        else { var cell22 = row.insertCell(22); cell22.innerHTML = "<span onclick='showErrorPopup(" + '"' + "IGM & Work Order No are Mandatory." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>"; }
                    }
                    else {
                        var cell22 = row.insertCell(22); cell22.innerHTML = "<span onclick='showErrorPopup(" + '"' + "Work Order Status is pending." + '"' + ", " + '"' + "red" + '"' + ");'><u>Print</u></span>";
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function ScopeTableInsertData() {
    var ScopeTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ScopeWorkOrderNo = document.getElementById("ScopeWorkOrderNo").value;
    DataFields.ScopeWorkOrderDate = document.getElementById("ScopeWorkOrderDate").value;
    DataFields.ScopeId = document.getElementById("ScopeId").value;
    DataFields.BLNo = document.getElementById("BLNo").value;
    DataFields.ScopeDescription = document.getElementById("ScopeDescription").value;
    DataFields.ScopeAgreedCostwithCustomer = document.getElementById("ScopeAgreedCostwithCustomer").value;
    DataFields.ScopeActualCost = document.getElementById("ScopeActualCost").value;
    DataFields.ScopeStatus = document.getElementById("ScopeStatus").value;
    DataFields.ScopeRemarks = document.getElementById("ScopeRemarks").value;
    ScopeTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("ScopeId").value != "" && document.getElementById("ScopeAgreedCostwithCustomer").value != "" &&
        document.getElementById("ScopeActualCost").value != "" && document.getElementById("ScopeStatus").value != "" && document.getElementById("BLNo").value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ScopeTableInsertData",
            data: JSON.stringify({ ScopeTableDataList: ScopeTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Saved") {
                    ScopeTableSearchData();
                    document.getElementById("ScpId").value = "";
                    document.getElementById("ScopeWorkOrderNo").value = "";
                    document.getElementById("ScopeWorkOrderDate").value = "";
                    document.getElementById("ScopeId").value = "";
                    document.getElementById("BLNo").value = "";
                    document.getElementById("ScopeDescription").value = "";
                    document.getElementById("ScopeAgreedCostwithCustomer").value = "";
                    document.getElementById("ScopeActualCost").value = "";
                    document.getElementById("ScopeStatus").value = "Pending";
                    document.getElementById("ScopeStatus").disabled = true;
                    document.getElementById("ScopeRemarks").value = "";

                    showErrorPopup(data.d, "green");
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup("Kindly fill the mandatory fields.", "red");
        return;
    }
}

function ScopeTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("ScpId").value = row.cells.item(0).innerText;
    document.getElementById("ScopeWorkOrderNo").value = row.cells.item(1).innerText;
    document.getElementById("ScopeWorkOrderDate").value = row.cells.item(2).innerText;
    document.getElementById("ScopeId").value = row.cells.item(3).innerText;
    document.getElementById("BLNo").value = row.cells.item(4).innerText;
    document.getElementById("ScopeDescription").value = row.cells.item(5).innerText;
    document.getElementById("ScopeAgreedCostwithCustomer").value = row.cells.item(6).innerText;
    document.getElementById("ScopeActualCost").value = row.cells.item(7).innerText;
    document.getElementById("ScopeStatus").value = row.cells.item(8).innerText;
    document.getElementById("ScopeStatus").disabled = false;
    document.getElementById("ScopeRemarks").value = row.cells.item(9).innerText;
    document.getElementById("ScopeAddBtn").style.backgroundColor = "#808080";
    document.getElementById("ScopeAddBtn").disabled = true;
    row.parentNode.removeChild(row);
}

function ScopeTableUpdateData() {
    var ScopeTableDataList = new Array();
    var DataFields = {};
    DataFields.ScpId = document.getElementById("ScpId").value;
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ScopeWorkOrderNo = document.getElementById("ScopeWorkOrderNo").value;
    DataFields.ScopeWorkOrderDate = document.getElementById("ScopeWorkOrderDate").value;
    DataFields.ScopeId = document.getElementById("ScopeId").value;
    DataFields.BLNo = document.getElementById("BLNo").value;
    DataFields.ScopeDescription = document.getElementById("ScopeDescription").value;
    DataFields.ScopeAgreedCostwithCustomer = document.getElementById("ScopeAgreedCostwithCustomer").value;
    DataFields.ScopeActualCost = document.getElementById("ScopeActualCost").value;
    DataFields.ScopeStatus = document.getElementById("ScopeStatus").value;
    DataFields.ScopeRemarks = document.getElementById("ScopeRemarks").value;
    ScopeTableDataList.push(DataFields);

    if (document.getElementById("MainJobNo").value != "" && document.getElementById("ScopeId").value != "" && document.getElementById("ScopeAgreedCostwithCustomer").value != "" &&
        document.getElementById("ScopeActualCost").value != "" && document.getElementById("ScopeStatus").value != "" && document.getElementById("BLNo").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ScopeTableUpdateData",
            data: JSON.stringify({ ScopeTableDataList: ScopeTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    ScopeTableSearchData();
                    document.getElementById("ScpId").value = "";
                    document.getElementById("ScopeWorkOrderNo").value = "";
                    document.getElementById("ScopeWorkOrderDate").value = "";
                    document.getElementById("ScopeId").value = "";
                    document.getElementById("BLNo").value = "";
                    document.getElementById("ScopeDescription").value = "";
                    document.getElementById("ScopeAgreedCostwithCustomer").value = "";
                    document.getElementById("ScopeActualCost").value = "";
                    document.getElementById("ScopeStatus").value = "Pending";
                    document.getElementById("ScopeStatus").disabled = true;
                    document.getElementById("ScopeRemarks").value = "";

                    showErrorPopup(data.d, "green");

                    document.getElementById("ScopeAddBtn").style.backgroundColor = "#28A745";
                    document.getElementById("ScopeAddBtn").disabled = true;
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup("Kindly fill the mandatory fields.", "red");
        return;
    }
}


function ScopeTableCancelData() {
    var ScopeTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ScopeWorkOrderNo = document.getElementById("ScopeWorkOrderNo").value;
    DataFields.ScpId = document.getElementById("ScpId").value;
    ScopeTableDataList.push(DataFields);
    var CancelConfirm = confirm("Do You want to cancel this Record.");

    if (CancelConfirm) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/ScopeTableCancelData",
            data: JSON.stringify({ ScopeTableDataList: ScopeTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Cancelled") {
                    ScopeTableSearchData();
                    document.getElementById("ScpId").value = "";
                    document.getElementById("ScopeWorkOrderNo").value = "";
                    document.getElementById("ScopeWorkOrderDate").value = "";
                    document.getElementById("ScopeId").value = "";
                    document.getElementById("BLNo").value = "";
                    document.getElementById("ScopeDescription").value = "";
                    document.getElementById("ScopeAgreedCostwithCustomer").value = "";
                    document.getElementById("ScopeActualCost").value = "";
                    document.getElementById("ScopeStatus").value = "Pending";
                    document.getElementById("ScopeStatus").disabled = true;
                    document.getElementById("ScopeRemarks").value = "";

                    showErrorPopup(data.d, "green");

                    document.getElementById("ScopeAddBtn").style.backgroundColor = "#28A745";
                    document.getElementById("ScopeAddBtn").disabled = true;
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}

function ScopeTableSearchData() {
    var ScopeTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    ScopeTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/ScopeTableSearchData",
        data: JSON.stringify({ ScopeTableDataList: ScopeTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("Scope");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("Scope");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);

                if (value.ScopeStatus == "Pending") { var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='ScopeTableEditData(this)' style='color=blue;'><b><u>" + value.ScpId + "</u></b></span>"; }
                else if (value.ScopeStatus == "Completed") { var cell0 = row.insertCell(0); cell0.innerHTML = "<span style='color=blue;'><b><u>" + value.ScpId + "</u></b></span>"; }                
                var cell1 = row.insertCell(1); cell1.innerHTML = value.ScopeWorkOrderNo;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.ScopeWorkOrderDate;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.ScopeId;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.BLNo;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.ScopeDescription;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.ScopeAgreedCostwithCustomer;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.ScopeActualCost;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.ScopeStatus;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.ScopeRemarks;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function RevenueTableInsertData() {
    var RevenueTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.RevenueChargeHead = document.getElementById("RevenueChargeHead").value;
    DataFields.RevenueChargeDescription = document.getElementById("RevenueChargeDescription").value;
    DataFields.RevenueCostCenter = document.getElementById("RevenueCostCenter").value;
    DataFields.RevenueCustomerCurrency = document.getElementById("RevenueCustomerCurrency").value;
    DataFields.RevenueCompanyCurrency = document.getElementById("RevenueCompanyCurrency").value;
    DataFields.RevenueExchangeRate = document.getElementById("RevenueExchangeRate").value;
    DataFields.RevenueHSNOrSACCode = document.getElementById("RevenueHSNOrSACCode").value;
    DataFields.RevenueQty = document.getElementById("RevenueQty").value;
    DataFields.RevenueUOM = document.getElementById("RevenueUOM").value;
    DataFields.RevenueUnitPrice = document.getElementById("RevenueUnitPrice").value;
    DataFields.RevenueGSTPercentage = document.getElementById("RevenueGSTPercentage").value;
    DataFields.RevenueGSTAmt = document.getElementById("RevenueGSTAmt").value;
    DataFields.RevenueTotalPrice = document.getElementById("RevenueTotalPrice").value;
    DataFields.RevenueReceivableType = document.getElementById("RevenueReceivableType").value;
    DataFields.RevenueInvoiceNo = document.getElementById("RevenueInvoiceNo").value;
    DataFields.RevenueInvoiceDate = document.getElementById("RevenueInvoiceDate").value;
    DataFields.RevenueDueDate = document.getElementById("RevenueDueDate").value;
    RevenueTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/RevenueTableInsertData",
        data: JSON.stringify({ RevenueTableDataList: RevenueTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Saved") {
                RevenueTableSearchData();
                document.getElementById("RevenueChargeHead").value = "";
                document.getElementById("RevenueChargeDescription").value = "";
                document.getElementById("RevenueCostCenter").value = "";
                document.getElementById("RevenueCustomerCurrency").value = "";
                document.getElementById("RevenueCompanyCurrency").value = "";
                document.getElementById("RevenueExchangeRate").value = "";
                document.getElementById("RevenueHSNOrSACCode").value = "";
                document.getElementById("RevenueQty").value = "";
                document.getElementById("RevenueUOM").value = "";
                document.getElementById("RevenueUnitPrice").value = "";
                document.getElementById("RevenueGSTPercentage").value = "";
                document.getElementById("RevenueGSTAmt").value = "";
                document.getElementById("RevenueTotalPrice").value = "";
                document.getElementById("RevenueReceivableType").value = "";
                document.getElementById("RevenueInvoiceNo").value = "";
                document.getElementById("RevenueInvoiceDate").value = "";
                document.getElementById("RevenueDueDate").value = "";
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function RevenueTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("RevenueChargeHead").value = row.cells.item(0).innerText;
    document.getElementById("RevenueChargeDescription").value = row.cells.item(1).innerText;
    document.getElementById("RevenueCostCenter").value = row.cells.item(2).innerText;
    document.getElementById("RevenueCustomerCurrency").value = row.cells.item(3).innerText;
    document.getElementById("RevenueCompanyCurrency").value = row.cells.item(4).innerText;
    document.getElementById("RevenueExchangeRate").value = row.cells.item(5).innerText;
    document.getElementById("RevenueHSNOrSACCode").value = row.cells.item(6).innerText;
    document.getElementById("RevenueQty").value = row.cells.item(7).innerText;
    document.getElementById("RevenueUOM").value = row.cells.item(8).innerText;
    document.getElementById("RevenueUnitPrice").value = row.cells.item(9).innerText;
    document.getElementById("RevenueGSTPercentage").value = row.cells.item(10).innerText;
    document.getElementById("RevenueGSTAmt").value = row.cells.item(11).innerText;
    document.getElementById("RevenueTotalPrice").value = row.cells.item(12).innerText;
    document.getElementById("RevenueReceivableType").value = row.cells.item(13).innerText;
    document.getElementById("RevenueInvoiceNo").value = row.cells.item(14).innerText;
    document.getElementById("RevenueInvoiceDate").value = row.cells.item(15).innerText;
    document.getElementById("RevenueDueDate").value = row.cells.item(16).innerText;
    document.getElementById("RevenueChargeHead").disabled = true;
    document.getElementById("RevenueTableAddBtn").style.backgroundColor = "#808080";
    document.getElementById("RevenueTableAddBtn").disabled = true;
    row.parentNode.removeChild(row);
}


function RevenueTableUpdateData() {
    var RevenueTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.RevenueChargeHead = document.getElementById("RevenueChargeHead").value;
    DataFields.RevenueChargeDescription = document.getElementById("RevenueChargeDescription").value;
    DataFields.RevenueCostCenter = document.getElementById("RevenueCostCenter").value;
    DataFields.RevenueCustomerCurrency = document.getElementById("RevenueCustomerCurrency").value;
    DataFields.RevenueCompanyCurrency = document.getElementById("RevenueCompanyCurrency").value;
    DataFields.RevenueExchangeRate = document.getElementById("RevenueExchangeRate").value;
    DataFields.RevenueHSNOrSACCode = document.getElementById("RevenueHSNOrSACCode").value;
    DataFields.RevenueQty = document.getElementById("RevenueQty").value;
    DataFields.RevenueUOM = document.getElementById("RevenueUOM").value;
    DataFields.RevenueUnitPrice = document.getElementById("RevenueUnitPrice").value;
    DataFields.RevenueGSTPercentage = document.getElementById("RevenueGSTPercentage").value;
    DataFields.RevenueGSTAmt = document.getElementById("RevenueGSTAmt").value;
    DataFields.RevenueTotalPrice = document.getElementById("RevenueTotalPrice").value;
    DataFields.RevenueReceivableType = document.getElementById("RevenueReceivableType").value;
    DataFields.RevenueInvoiceNo = document.getElementById("RevenueInvoiceNo").value;
    DataFields.RevenueInvoiceDate = document.getElementById("RevenueInvoiceDate").value;
    DataFields.RevenueDueDate = document.getElementById("RevenueDueDate").value;
    RevenueTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/RevenueTableUpdateData",
        data: JSON.stringify({ RevenueTableDataList: RevenueTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                RevenueTableSearchData();
                document.getElementById("RevenueChargeHead").value = "";
                document.getElementById("RevenueChargeDescription").value = "";
                document.getElementById("RevenueCostCenter").value = "";
                document.getElementById("RevenueCustomerCurrency").value = "";
                document.getElementById("RevenueCompanyCurrency").value = "";
                document.getElementById("RevenueExchangeRate").value = "";
                document.getElementById("RevenueHSNOrSACCode").value = "";
                document.getElementById("RevenueQty").value = "";
                document.getElementById("RevenueUOM").value = "";
                document.getElementById("RevenueUnitPrice").value = "";
                document.getElementById("RevenueGSTPercentage").value = "";
                document.getElementById("RevenueGSTAmt").value = "";
                document.getElementById("RevenueTotalPrice").value = "";
                document.getElementById("RevenueReceivableType").value = "";
                document.getElementById("RevenueInvoiceNo").value = "";
                document.getElementById("RevenueInvoiceDate").value = "";
                document.getElementById("RevenueDueDate").value = "";
                document.getElementById("RevenueChargeHead").disabled = false;
                document.getElementById("RevenueTableAddBtn").style.backgroundColor = "#28A745";
                document.getElementById("RevenueTableAddBtn").disabled = false;
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function RevenueTableCancelData() {
    var RevenueTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.RevenueChargeHead = document.getElementById("RevenueChargeHead").value;
    DataFields.RevenueChargeDescription = document.getElementById("RevenueChargeDescription").value;
    DataFields.RevenueCostCenter = document.getElementById("RevenueCostCenter").value;
    DataFields.RevenueCustomerCurrency = document.getElementById("RevenueCustomerCurrency").value;
    DataFields.RevenueCompanyCurrency = document.getElementById("RevenueCompanyCurrency").value;
    DataFields.RevenueExchangeRate = document.getElementById("RevenueExchangeRate").value;
    DataFields.RevenueHSNOrSACCode = document.getElementById("RevenueHSNOrSACCode").value;
    DataFields.RevenueQty = document.getElementById("RevenueQty").value;
    DataFields.RevenueUOM = document.getElementById("RevenueUOM").value;
    DataFields.RevenueUnitPrice = document.getElementById("RevenueUnitPrice").value;
    DataFields.RevenueGSTPercentage = document.getElementById("RevenueGSTPercentage").value;
    DataFields.RevenueGSTAmt = document.getElementById("RevenueGSTAmt").value;
    DataFields.RevenueTotalPrice = document.getElementById("RevenueTotalPrice").value;
    DataFields.RevenueReceivableType = document.getElementById("RevenueReceivableType").value;
    DataFields.RevenueInvoiceNo = document.getElementById("RevenueInvoiceNo").value;
    DataFields.RevenueInvoiceDate = document.getElementById("RevenueInvoiceDate").value;
    DataFields.RevenueDueDate = document.getElementById("RevenueDueDate").value;
    RevenueTableDataList.push(DataFields);
    var CancelConfirm = confirm("Do You want to cancel this Record.");

    if (CancelConfirm) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/RevenueTableCancelData",
            data: JSON.stringify({ RevenueTableDataList: RevenueTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Cancelled") {
                    document.getElementById("RevenueChargeHead").value = "";
                    document.getElementById("RevenueChargeDescription").value = "";
                    document.getElementById("RevenueCostCenter").value = "";
                    document.getElementById("RevenueCustomerCurrency").value = "";
                    document.getElementById("RevenueCompanyCurrency").value = "";
                    document.getElementById("RevenueExchangeRate").value = "";
                    document.getElementById("RevenueHSNOrSACCode").value = "";
                    document.getElementById("RevenueQty").value = "";
                    document.getElementById("RevenueUOM").value = "";
                    document.getElementById("RevenueUnitPrice").value = "";
                    document.getElementById("RevenueGSTPercentage").value = "";
                    document.getElementById("RevenueGSTAmt").value = "";
                    document.getElementById("RevenueTotalPrice").value = "";
                    document.getElementById("RevenueReceivableType").value = "";
                    document.getElementById("RevenueInvoiceNo").value = "";
                    document.getElementById("RevenueInvoiceDate").value = "";
                    document.getElementById("RevenueDueDate").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}

function RevenueTableSearchData() {
    var RevenueTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    RevenueTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/RevenueTableSearchData",
        data: JSON.stringify({ RevenueTableDataList: RevenueTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("RevenueTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("RevenueTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='RevenueTableEditData(this)' style='color=blue;'><b><u>" + value.RevenueChargeHead + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.RevenueChargeDescription;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.RevenueCostCenter;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.RevenueCustomerCurrency;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.RevenueCompanyCurrency;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.RevenueExchangeRate;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.RevenueHSNOrSACCode;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.RevenueQty;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.RevenueUOM;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.RevenueUnitPrice;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.RevenueGSTPercentage;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.RevenueGSTAmt;
                var cell12 = row.insertCell(12); cell12.innerHTML = value.RevenueTotalPrice;
                var cell13 = row.insertCell(13); cell13.innerHTML = value.RevenueReceivableType;
                var cell14 = row.insertCell(14); cell14.innerHTML = value.RevenueInvoiceNo;
                var cell15 = row.insertCell(15); cell15.innerHTML = value.RevenueInvoiceDate;
                var cell16 = row.insertCell(16); cell16.innerHTML = value.RevenueDueDate;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function CostTableInsertData() {
    var CostTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.CostChargeHead = document.getElementById("CostChargeHead").value;
    DataFields.CostChargeDescription = document.getElementById("CostChargeDescription").value;
    DataFields.CostCostCenter = document.getElementById("CostCostCenter").value;
    DataFields.CostPayableCurrency = document.getElementById("CostPayableCurrency").value;
    DataFields.CostCompanyCurrency = document.getElementById("CostCompanyCurrency").value;
    DataFields.CostExchangeRate = document.getElementById("CostExchangeRate").value;
    DataFields.CostHSNOrSACCode = document.getElementById("CostHSNOrSACCode").value;
    DataFields.CostQty = document.getElementById("CostQty").value;
    DataFields.CostUOM = document.getElementById("CostUOM").value;
    DataFields.CostUnitPrice = document.getElementById("CostUnitPrice").value;
    DataFields.CostGSTPercentage = document.getElementById("CostGSTPercentage").value;
    DataFields.CostGSTAmt = document.getElementById("CostGSTAmt").value;
    DataFields.CostTotalPrice = document.getElementById("CostTotalPrice").value;
    DataFields.CostPayableType = document.getElementById("CostPayableType").value;
    DataFields.CostEmployeeName = document.getElementById("CostEmployeeName").value;
    DataFields.CostVendorName = document.getElementById("CostVendorName").value;
    DataFields.CostVendorBranch = document.getElementById("CostVendorBranch").value;
    DataFields.CostInvoiceNo = document.getElementById("CostInvoiceNo").value;
    DataFields.CostInvoiceDate = document.getElementById("CostInvoiceDate").value;
    DataFields.CostDueDate = document.getElementById("CostDueDate").value;
    CostTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/CostTableInsertData",
        data: JSON.stringify({ CostTableDataList: CostTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Saved") {
                CostTableSearchData();
                document.getElementById("CostChargeHead").value = "";
                document.getElementById("CostChargeDescription").value = "";
                document.getElementById("CostCostCenter").value = "";
                document.getElementById("CostPayableCurrency").value = "";
                document.getElementById("CostCompanyCurrency").value = "";
                document.getElementById("CostExchangeRate").value = "";
                document.getElementById("CostHSNOrSACCode").value = "";
                document.getElementById("CostQty").value = "";
                document.getElementById("CostUOM").value = "";
                document.getElementById("CostUnitPrice").value = "";
                document.getElementById("CostGSTPercentage").value = "";
                document.getElementById("CostGSTAmt").value = "";
                document.getElementById("CostTotalPrice").value = "";
                document.getElementById("CostPayableType").value = "";
                document.getElementById("CostEmployeeName").value = "";
                document.getElementById("CostVendorName").value = "";
                document.getElementById("CostVendorBranch").value = "";
                document.getElementById("CostInvoiceNo").value = "";
                document.getElementById("CostInvoiceDate").value = "";
                document.getElementById("CostDueDate").value = "";
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function CostTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("CostChargeHead").value = row.cells.item(0).innerText;
    document.getElementById("CostChargeDescription").value = row.cells.item(1).innerText;
    document.getElementById("CostCostCenter").value = row.cells.item(2).innerText;
    document.getElementById("CostPayableCurrency").value = row.cells.item(3).innerText;
    document.getElementById("CostCompanyCurrency").value = row.cells.item(4).innerText;
    document.getElementById("CostExchangeRate").value = row.cells.item(5).innerText;
    document.getElementById("CostHSNOrSACCode").value = row.cells.item(6).innerText;
    document.getElementById("CostQty").value = row.cells.item(7).innerText;
    document.getElementById("CostUOM").value = row.cells.item(8).innerText;
    document.getElementById("CostUnitPrice").value = row.cells.item(9).innerText;
    document.getElementById("CostGSTPercentage").value = row.cells.item(10).innerText;
    document.getElementById("CostGSTAmt").value = row.cells.item(11).innerText;
    document.getElementById("CostTotalPrice").value = row.cells.item(12).innerText;
    document.getElementById("CostPayableType").value = row.cells.item(13).innerText;
    document.getElementById("CostEmployeeName").value = row.cells.item(14).innerText;
    document.getElementById("CostVendorName").value = row.cells.item(15).innerText;
    document.getElementById("CostVendorBranch").value = row.cells.item(16).innerText;
    document.getElementById("CostInvoiceNo").value = row.cells.item(17).innerText;
    document.getElementById("CostInvoiceDate").value = row.cells.item(18).innerText;
    document.getElementById("CostDueDate").value = row.cells.item(19).innerText;
    document.getElementById("CostChargeHead").disabled = true;
    document.getElementById("CostTableAddBtn").style.backgroundColor = "#808080";
    document.getElementById("CostTableAddBtn").disabled = true;
    row.parentNode.removeChild(row);
}


function CostTableUpdateData() {
    var CostTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.CostChargeHead = document.getElementById("CostChargeHead").value;
    DataFields.CostChargeDescription = document.getElementById("CostChargeDescription").value;
    DataFields.CostCostCenter = document.getElementById("CostCostCenter").value;
    DataFields.CostPayableCurrency = document.getElementById("CostPayableCurrency").value;
    DataFields.CostCompanyCurrency = document.getElementById("CostCompanyCurrency").value;
    DataFields.CostExchangeRate = document.getElementById("CostExchangeRate").value;
    DataFields.CostHSNOrSACCode = document.getElementById("CostHSNOrSACCode").value;
    DataFields.CostQty = document.getElementById("CostQty").value;
    DataFields.CostUOM = document.getElementById("CostUOM").value;
    DataFields.CostUnitPrice = document.getElementById("CostUnitPrice").value;
    DataFields.CostGSTPercentage = document.getElementById("CostGSTPercentage").value;
    DataFields.CostGSTAmt = document.getElementById("CostGSTAmt").value;
    DataFields.CostTotalPrice = document.getElementById("CostTotalPrice").value;
    DataFields.CostPayableType = document.getElementById("CostPayableType").value;
    DataFields.CostEmployeeName = document.getElementById("CostEmployeeName").value;
    DataFields.CostVendorName = document.getElementById("CostVendorName").value;
    DataFields.CostVendorBranch = document.getElementById("CostVendorBranch").value;
    DataFields.CostInvoiceNo = document.getElementById("CostInvoiceNo").value;
    DataFields.CostInvoiceDate = document.getElementById("CostInvoiceDate").value;
    DataFields.CostDueDate = document.getElementById("CostDueDate").value;
    CostTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/CostTableUpdateData",
        data: JSON.stringify({ CostTableDataList: CostTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                CostTableSearchData();
                document.getElementById("CostChargeHead").value = "";
                document.getElementById("CostChargeDescription").value = "";
                document.getElementById("CostCostCenter").value = "";
                document.getElementById("CostPayableCurrency").value = "";
                document.getElementById("CostCompanyCurrency").value = "";
                document.getElementById("CostExchangeRate").value = "";
                document.getElementById("CostHSNOrSACCode").value = "";
                document.getElementById("CostQty").value = "";
                document.getElementById("CostUOM").value = "";
                document.getElementById("CostUnitPrice").value = "";
                document.getElementById("CostGSTPercentage").value = "";
                document.getElementById("CostGSTAmt").value = "";
                document.getElementById("CostTotalPrice").value = "";
                document.getElementById("CostPayableType").value = "";
                document.getElementById("CostEmployeeName").value = "";
                document.getElementById("CostVendorName").value = "";
                document.getElementById("CostVendorBranch").value = "";
                document.getElementById("CostInvoiceNo").value = "";
                document.getElementById("CostInvoiceDate").value = "";
                document.getElementById("CostDueDate").value = "";
                document.getElementById("CostChargeHead").disabled = false;
                document.getElementById("CostTableAddBtn").style.backgroundColor = "#28A745";
                document.getElementById("CostTableAddBtn").disabled = false;
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function CostTableCancelData() {
    var CostTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.CostChargeHead = document.getElementById("CostChargeHead").value;
    DataFields.CostChargeDescription = document.getElementById("CostChargeDescription").value;
    DataFields.CostCostCenter = document.getElementById("CostCostCenter").value;
    DataFields.CostPayableCurrency = document.getElementById("CostPayableCurrency").value;
    DataFields.CostCompanyCurrency = document.getElementById("CostCompanyCurrency").value;
    DataFields.CostExchangeRate = document.getElementById("CostExchangeRate").value;
    DataFields.CostHSNOrSACCode = document.getElementById("CostHSNOrSACCode").value;
    DataFields.CostQty = document.getElementById("CostQty").value;
    DataFields.CostUOM = document.getElementById("CostUOM").value;
    DataFields.CostUnitPrice = document.getElementById("CostUnitPrice").value;
    DataFields.CostGSTPercentage = document.getElementById("CostGSTPercentage").value;
    DataFields.CostGSTAmt = document.getElementById("CostGSTAmt").value;
    DataFields.CostTotalPrice = document.getElementById("CostTotalPrice").value;
    DataFields.CostPayableType = document.getElementById("CostPayableType").value;
    DataFields.CostEmployeeName = document.getElementById("CostEmployeeName").value;
    DataFields.CostVendorName = document.getElementById("CostVendorName").value;
    DataFields.CostVendorBranch = document.getElementById("CostVendorBranch").value;
    DataFields.CostInvoiceNo = document.getElementById("CostInvoiceNo").value;
    DataFields.CostInvoiceDate = document.getElementById("CostInvoiceDate").value;
    DataFields.CostDueDate = document.getElementById("CostDueDate").value;
    CostTableDataList.push(DataFields);
    var CancelConfirm = confirm("Do You want to cancel this Record.");

    if (CancelConfirm) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/CostTableCancelData",
            data: JSON.stringify({ CostTableDataList: CostTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Cancelled") {
                    document.getElementById("CostChargeHead").value = "";
                    document.getElementById("CostChargeDescription").value = "";
                    document.getElementById("CostCostCenter").value = "";
                    document.getElementById("CostPayableCurrency").value = "";
                    document.getElementById("CostCompanyCurrency").value = "";
                    document.getElementById("CostExchangeRate").value = "";
                    document.getElementById("CostHSNOrSACCode").value = "";
                    document.getElementById("CostQty").value = "";
                    document.getElementById("CostUOM").value = "";
                    document.getElementById("CostUnitPrice").value = "";
                    document.getElementById("CostGSTPercentage").value = "";
                    document.getElementById("CostGSTAmt").value = "";
                    document.getElementById("CostTotalPrice").value = "";
                    document.getElementById("CostPayableType").value = "";
                    document.getElementById("CostEmployeeName").value = "";
                    document.getElementById("CostVendorName").value = "";
                    document.getElementById("CostVendorBranch").value = "";
                    document.getElementById("CostInvoiceNo").value = "";
                    document.getElementById("CostInvoiceDate").value = "";
                    document.getElementById("CostDueDate").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}


function CostTableSearchData() {
    var CostTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    CostTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/CostTableSearchData",
        data: JSON.stringify({ CostTableDataList: CostTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("CostTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("CostTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='CostTableEditData(this)' style='color=blue;'><b><u>" + value.CostChargeHead + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.CostChargeDescription;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.CostCostCenter;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.CostPayableCurrency;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.CostCompanyCurrency;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.CostExchangeRate;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.CostHSNOrSACCode;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.CostQty;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.CostUOM;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.CostUnitPrice;
                var cell10 = row.insertCell(10); cell10.innerHTML = value.CostGSTPercentage;
                var cell11 = row.insertCell(11); cell11.innerHTML = value.CostGSTAmt;
                var cell12 = row.insertCell(12); cell12.innerHTML = value.CostTotalPrice;
                var cell13 = row.insertCell(13); cell13.innerHTML = value.CostPayableType;
                var cell14 = row.insertCell(14); cell14.innerHTML = value.CostEmployeeName;
                var cell15 = row.insertCell(15); cell15.innerHTML = value.CostVendorName;
                var cell16 = row.insertCell(16); cell16.innerHTML = value.CostVendorBranch;
                var cell17 = row.insertCell(17); cell17.innerHTML = value.CostInvoiceNo;
                var cell18 = row.insertCell(18); cell18.innerHTML = value.CostInvoiceDate;
                var cell19 = row.insertCell(19); cell19.innerHTML = value.CostDueDate;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function LoadedTruckTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("LoadTruckWorkOrderNo").value = row.cells.item(0).innerText;
    document.getElementById("LoadTruckWorkOrderDate").value = row.cells.item(1).innerText;
    document.getElementById("LoadTruckGateOutPassNo").value = row.cells.item(2).innerText;
    document.getElementById("LoadTruckGateOutPassDate").value = row.cells.item(3).innerText;
    document.getElementById("LoadTruckModeofGateOut").value = row.cells.item(4).innerText;
    document.getElementById("LoadTruckContainerNo").value = row.cells.item(5).innerText;
    document.getElementById("LoadTruckTruckNo").value = row.cells.item(6).innerText;
    document.getElementById("LoadTruckPkgsorWeight").value = row.cells.item(7).innerText;
    document.getElementById("LoadTruckRemarks").value = row.cells.item(8).innerText;
    document.getElementById("LoadTruckMainJobNo").value = row.cells.item(9).innerText;
    row.parentNode.removeChild(row);
}

function LoadedTruckTableUpdateData() {
    var LoadedTruckTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("LoadTruckMainJobNo").value;
    DataFields.LoadTruckWorkOrderNo = document.getElementById("LoadTruckWorkOrderNo").value;
    DataFields.LoadTruckWorkOrderDate = document.getElementById("LoadTruckWorkOrderDate").value;
    DataFields.LoadTruckGateOutPassNo = document.getElementById("LoadTruckGateOutPassNo").value;
    DataFields.LoadTruckGateOutPassDate = document.getElementById("LoadTruckGateOutPassDate").value;
    DataFields.LoadTruckModeofGateOut = document.getElementById("LoadTruckModeofGateOut").value;
    DataFields.LoadTruckContainerNo = document.getElementById("LoadTruckContainerNo").value;
    DataFields.LoadTruckTruckNo = document.getElementById("LoadTruckTruckNo").value;
    DataFields.LoadTruckPkgsorWeight = document.getElementById("LoadTruckPkgsorWeight").value;
    DataFields.LoadTruckRemarks = document.getElementById("LoadTruckRemarks").value;
    LoadedTruckTableDataList.push(DataFields);

    if (document.getElementById("LoadTruckMainJobNo").value != "" && document.getElementById("LoadTruckWorkOrderNo").value != "" && //document.getElementById("LoadTruckWorkOrderDate").value != "" &&
        document.getElementById("LoadTruckTruckNo").value != "") {

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LoadedTruckTableUpdateData",
            data: JSON.stringify({ LoadedTruckTableDataList: LoadedTruckTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated" || data.d == "Inserted") {
                    LoadedTruckTableSearchData();
                    document.getElementById("LoadTruckWorkOrderNo").value = "";
                    document.getElementById("LoadTruckWorkOrderDate").value = "";
                    document.getElementById("LoadTruckGateOutPassNo").value = "";
                    document.getElementById("LoadTruckGateOutPassDate").value = "";
                    document.getElementById("LoadTruckModeofGateOut").value = "";
                    document.getElementById("LoadTruckContainerNo").value = "";
                    document.getElementById("LoadTruckTruckNo").value = "";
                    document.getElementById("LoadTruckPkgsorWeight").value = "";
                    document.getElementById("LoadTruckRemarks").value = "";
                    showErrorPopup(data.d, 'green');
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
    }
}

function LoadedTruckTableSearchData() {
    var LoadedTruckTableDataList = new Array();
    var DataFields = {};
    DataFields.LoadTruckStatus = document.getElementById("LoadTruckStatus").value;
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    LoadedTruckTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/LoadedTruckTableSearchData",
        data: JSON.stringify({ LoadedTruckTableDataList: LoadedTruckTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("LoadedTruckTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("LoadedTruckTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='LoadedTruckTableEditData(this)' style='color=blue;'><b><u>" + value.LoadTruckWorkOrderNo + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.LoadTruckWorkOrderDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.LoadTruckGateOutPassNo;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.LoadTruckGateOutPassDate;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.LoadTruckModeofGateOut;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.LoadTruckContainerNo;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.LoadTruckTruckNo;
                var cell7 = row.insertCell(7); cell7.innerHTML = value.LoadTruckPkgsorWeight;
                var cell8 = row.insertCell(8); cell8.innerHTML = value.LoadTruckRemarks;
                var cell9 = row.insertCell(9); cell9.innerHTML = value.MainJobNo;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function TrackingTableInsertData() {
    var TrackingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.TrackingMileStone = document.getElementById("TrackingMileStone").value;
    DataFields.TrackingPlanDate = document.getElementById("TrackingPlanDate").value;
    DataFields.TrackingActualDate = document.getElementById("TrackingActualDate").value;
    DataFields.TrackingRemarks = document.getElementById("TrackingRemarks").value;
    TrackingTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/TrackingTableInsertData",
        data: JSON.stringify({ TrackingTableDataList: TrackingTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Saved") {
                TrackingTableSearchData();
                document.getElementById("TrackingMileStone").value = "";
                document.getElementById("TrackingPlanDate").value = "";
                document.getElementById("TrackingActualDate").value = "";
                document.getElementById("TrackingRemarks").value = "";
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function TrackingTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("TrackingMileStone").value = row.cells.item(0).innerText;
    document.getElementById("TrackingPlanDate").value = row.cells.item(1).innerText;
    document.getElementById("TrackingActualDate").value = row.cells.item(2).innerText;
    document.getElementById("TrackingRemarks").value = row.cells.item(3).innerText;
    document.getElementById("TrackingMileStone").disabled = true;
    document.getElementById("TrackingTableAddBtn").style.backgroundColor = "#808080";
    document.getElementById("TrackingTableAddBtn").disabled = true;
    row.parentNode.removeChild(row);
}


function TrackingTableUpdateData() {
    var TrackingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.TrackingMileStone = document.getElementById("TrackingMileStone").value;
    DataFields.TrackingPlanDate = document.getElementById("TrackingPlanDate").value;
    DataFields.TrackingActualDate = document.getElementById("TrackingActualDate").value;
    DataFields.TrackingRemarks = document.getElementById("TrackingRemarks").value;
    TrackingTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/TrackingTableUpdateData",
        data: JSON.stringify({ TrackingTableDataList: TrackingTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                TrackingTableSearchData();
                document.getElementById("TrackingMileStone").value = "";
                document.getElementById("TrackingPlanDate").value = "";
                document.getElementById("TrackingActualDate").value = "";
                document.getElementById("TrackingRemarks").value = "";
                document.getElementById("TrackingMileStone").disabled = false;
                document.getElementById("TrackingTableAddBtn").style.backgroundColor = "#28A745";
                document.getElementById("TrackingTableAddBtn").disabled = false;
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function TrackingTableCancelData() {
    var TrackingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.TrackingMileStone = document.getElementById("TrackingMileStone").value;
    DataFields.TrackingPlanDate = document.getElementById("TrackingPlanDate").value;
    DataFields.TrackingActualDate = document.getElementById("TrackingActualDate").value;
    DataFields.TrackingRemarks = document.getElementById("TrackingRemarks").value;
    TrackingTableDataList.push(DataFields);
    var CancelConfirm = confirm("Do You want to cancel this Record.");

    if (CancelConfirm) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/TrackingTableCancelData",
            data: JSON.stringify({ TrackingTableDataList: TrackingTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Cancelled") {
                    document.getElementById("TrackingMileStone").value = "";
                    document.getElementById("TrackingPlanDate").value = "";
                    document.getElementById("TrackingActualDate").value = "";
                    document.getElementById("TrackingRemarks").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}


function TrackingTableSearchData() {
    var TrackingTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    TrackingTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/TrackingTableSearchData",
        data: JSON.stringify({ TrackingTableDataList: TrackingTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("TrackingTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("TrackingTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='TrackingTableEditData(this)' style='color=blue;'><b><u>" + value.TrackingMileStone + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.TrackingPlanDate;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.TrackingActualDate;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.TrackingRemarks;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function NotesTableInsertData() {
    var NotesTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ImportNotes = document.getElementById("ImportNotes").value;
    DataFields.Reminder = document.getElementById("Reminder").value;
    DataFields.NotesRemarks = document.getElementById("NotesRemarks").value;
    NotesTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/NotesTableInsertData",
        data: JSON.stringify({ NotesTableDataList: NotesTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Saved") {
                NotesTableSearchData();
                document.getElementById("ImportNotes").value = "";
                document.getElementById("Reminder").value = "";
                document.getElementById("NotesRemarks").value = "";
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function NotesTableEditData(btn) {
    var row = btn.parentNode.parentNode;
    document.getElementById("ImportNotes").value = row.cells.item(0).innerText;
    document.getElementById("Reminder").value = row.cells.item(1).innerText;
    document.getElementById("NotesRemarks").value = row.cells.item(2).innerText;
    document.getElementById("ImportNotes").disabled = true;
    document.getElementById("NotesTableAddBtn").style.backgroundColor = "#808080";
    document.getElementById("NotesTableAddBtn").disabled = true;
    row.parentNode.removeChild(row);
}


function NotesTableUpdateData() {
    var NotesTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ImportNotes = document.getElementById("ImportNotes").value;
    DataFields.Reminder = document.getElementById("Reminder").value;
    DataFields.NotesRemarks = document.getElementById("NotesRemarks").value;
    NotesTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/NotesTableUpdateData",
        data: JSON.stringify({ NotesTableDataList: NotesTableDataList }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                NotesTableSearchData();
                document.getElementById("ImportNotes").value = "";
                document.getElementById("Reminder").value = "";
                document.getElementById("NotesRemarks").value = "";
                document.getElementById("ImportNotes").disabled = false;
                document.getElementById("NotesTableAddBtn").style.backgroundColor = "#28A745";
                document.getElementById("NotesTableAddBtn").disabled = false;
            }
            else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
            else { showErrorPopup(data.d, 'red'); }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function NotesTableCancelData() {
    var NotesTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    DataFields.ImportNotes = document.getElementById("ImportNotes").value;
    DataFields.Reminder = document.getElementById("Reminder").value;
    DataFields.NotesRemarks = document.getElementById("NotesRemarks").value;
    NotesTableDataList.push(DataFields);
    var CancelConfirm = confirm("Do You want to cancel this Record.");

    if (CancelConfirm) {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/NotesTableCancelData",
            data: JSON.stringify({ NotesTableDataList: NotesTableDataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Cancelled") {
                    document.getElementById("ImportNotes").value = "";
                    document.getElementById("Reminder").value = "";
                    document.getElementById("NotesRemarks").value = "";
                }
                else if (data.d == "SessionTimeout") { window.location = "./Login.aspx"; }
                else { showErrorPopup(data.d, 'red'); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}


function NotesTableSearchData() {
    var NotesTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    NotesTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/NotesTableSearchData",
        data: JSON.stringify({ NotesTableDataList: NotesTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("NotesTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("NotesTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='NotesTableEditData(this)' style='color=blue;'><b><u>" + value.ImportNotes + "</u></b></span>";
                var cell1 = row.insertCell(1); cell1.innerHTML = value.Reminder;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.NotesRemarks;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function LogTableSearchData() {
    var LogTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    LogTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/LogTableSearchData",
        data: JSON.stringify({ LogTableDataList: LogTableDataList }),
        dataType: "json",
        success: function (data) {
            var RemoveTableRows = document.getElementById("LogTable");
            for (var j = 1; j < RemoveTableRows.rows.length; j++) { RemoveTableRows.deleteRow(j); j--; }
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                var table = document.getElementById("LogTable");
                var rowcount = table.rows.length;
                var row = table.insertRow(rowcount);
                var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='LogTableEditData(this)' style='color=blue;'><b><u>" + value.TableKeyColumn + "</u></b></span>";
                //var cell1 = row.insertCell(1); cell1.innerHTML = value.TableName;
                var cell1 = row.insertCell(1); cell1.innerHTML = value.UpdatedBy;
                var cell2 = row.insertCell(2); cell2.innerHTML = value.Comments;
                var cell3 = row.insertCell(3); cell3.innerHTML = value.ColumnName;
                var cell4 = row.insertCell(4); cell4.innerHTML = value.OldValue;
                var cell5 = row.insertCell(5); cell5.innerHTML = value.NewValue;
                var cell6 = row.insertCell(6); cell6.innerHTML = value.UpdatedOn;
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}
function UserAccessValidation() {
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/UserAccessValidation",
        data: JSON.stringify({ Dummy: "" }),
        dataType: "json",
        success: function (data) {
            var AccessExists = false;
            $.each(data.d, function (key, value) {
                if (value.ReturnedValue == "No Data Found!!!") { showErrorPopup("Access Denied!!!", "red"); window.location = "/AccessDenied.html"; return; }
                else if (value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, "red"); return; }

                if (value.AccessRole == "CFS General Documentation") {
                    document.getElementById("TransportTabBtn").style.display = "none";
                    document.getElementById("WorkOrderTabBtn").style.display = "none";
                    document.getElementById("GateInTabBtn").style.display = "none";
                    document.getElementById("WorkOrderCloseTabBtn").style.display = "none";
                    document.getElementById("ScopeTabBtn").style.display = "none";
                    document.getElementById("RevenueTabBtn").style.display = "none";
                    document.getElementById("CostTabBtn").style.display = "none";
                    document.getElementById("GateOutTabBtn").style.display = "none";
                    AccessExists = true;
                }
                else if (value.AccessRole == "CFS PNR Operations") {
                    document.getElementById("WorkOrderTabBtn").style.display = "none";
                    document.getElementById("GateInTabBtn").style.display = "none";
                    document.getElementById("WorkOrderCloseTabBtn").style.display = "none";
                    document.getElementById("ScopeTabBtn").style.display = "none";
                    document.getElementById("RevenueTabBtn").style.display = "none";
                    document.getElementById("CostTabBtn").style.display = "none";
                    document.getElementById("GateOutTabBtn").style.display = "none";
                    document.getElementById("GateOutTabBtn").style.display = "none";
                    AccessExists = true;
                }
                else if (value.AccessRole == "CFS Gate Documentation") {
                    document.getElementById("GeneralTabBtn").style.display = "none";
                    document.getElementById("LinerTabBtn").style.display = "none";
                    document.getElementById("ContainerTabBtn").style.display = "none";
                    document.getElementById("TSAandDRFBtn").style.display = "none";
                    document.getElementById("TransportTabBtn").style.display = "none";
                    document.getElementById("WorkOrderTabBtn").style.display = "none";
                    document.getElementById("WorkOrderCloseTabBtn").style.display = "none";
                    document.getElementById("ScopeTabBtn").style.display = "none";
                    document.getElementById("RevenueTabBtn").style.display = "none";
                    document.getElementById("CostTabBtn").style.display = "none";
                    AccessExists = true;
                }
                else if (value.AccessRole == "CFS Control Documentation") {
                    document.getElementById("TransportTabBtn").style.display = "none";
                    document.getElementById("GateInTabBtn").style.display = "none";
                    document.getElementById("GateOutTabBtn").style.display = "none";
                    document.getElementById("ScopeTabBtn").style.display = "none";
                    document.getElementById("RevenueTabBtn").style.display = "none";
                    document.getElementById("CostTabBtn").style.display = "none";
                    AccessExists = true;
                }
                else if (value.AccessRole == "Finance") {
                    document.getElementById("GeneralTabBtn").style.display = "none";
                    document.getElementById("LinerTabBtn").style.display = "none";
                    document.getElementById("ContainerTabBtn").style.display = "none";
                    document.getElementById("TSAandDRFBtn").style.display = "none";
                    document.getElementById("DocumentUploadTabBtn").style.display = "none";
                    document.getElementById("TransportTabBtn").style.display = "none";
                    document.getElementById("WorkOrderTabBtn").style.display = "none";
                    document.getElementById("GateInTabBtn").style.display = "none";
                    document.getElementById("WorkOrderCloseTabBtn").style.display = "none";
                    document.getElementById("GateOutTabBtn").style.display = "none";
                    document.getElementById("GateOutTabBtn").style.display = "none";
                    AccessExists = true;
                }
                else if (value.AccessRole == "Admin") { AccessExists = true; }

                //Common For All
                //document.getElementById("TrackingTabBtn").style.display = "none";
                //document.getElementById("NotesTabBtn").style.display = "none";
                //document.getElementById("LogTabBtn").style.display = "none";
                //document.getElementById("SettingsTabBtn").style.display = "none";
            });
            if (!AccessExists) { showErrorPopup("Access Denied!!!", "red"); window.location = "/AccessDenied.html"; return; }
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}


function PreviousDataSearch() {
    var MainTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    MainTableDataList.push(DataFields);
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/MainTablePreviousDataSearch",
        data: JSON.stringify({ MainTableDataList: MainTableDataList }),
        dataType: "json",
        success: function (data) {
            $.each(data.d, function (key, value) {
                if (value.MainJobNo == "" || value.MainJobNo == null) { showErrorPopup('No Record Found.', 'red'); }
                else {
                    // Append a query parameter to the URL
                    const newParam = "MainJobNo=" + value.MainJobNo.trim();
                    const currentUrl = window.location.href.split('?')[0]; // Remove existing query parameters
                    const newUrl = `${currentUrl}?${newParam}`;

                    // Update the URL without reloading the page
                    history.pushState(null, '', newUrl);

                    document.getElementById("MainJobNo").value = value.MainJobNo.trim();
                    MainTableSearchData();
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function NextDataSearch() {
    if (document.getElementById("MainJobNo").value == "") { return; }
    var MainTableDataList = new Array();
    var DataFields = {};
    DataFields.MainJobNo = document.getElementById("MainJobNo").value;
    MainTableDataList.push(DataFields); $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/MainTableNextDataSearch",
        data: JSON.stringify({ MainTableDataList: MainTableDataList }),
        dataType: "json",
        success: function (data) {
            $.each(data.d, function (key, value) {
                if (value.MainJobNo == "" || value.MainJobNo == null) { showErrorPopup('No Record Found.', 'red'); }
                else {
                    // Append a query parameter to the URL
                    const newParam = "MainJobNo=" + value.MainJobNo.trim();
                    const currentUrl = window.location.href.split('?')[0]; // Remove existing query parameters
                    const newUrl = `${currentUrl}?${newParam}`;

                    // Update the URL without reloading the page
                    history.pushState(null, '', newUrl);

                    document.getElementById("MainJobNo").value = value.MainJobNo.trim();
                    MainTableSearchData();
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

//--------------------------Added Manually--------------------------------------
function FillItemNo() {
    document.getElementById("LinerItemNo").value = document.getElementById("MainItemNo").value;
    var table = document.getElementById("ContainerTable");
    for (var j = 1; j < table.rows.length; j++) {
        document.getElementById("ContainerItemNo" + j).value = document.getElementById("MainItemNo").value;
    }
}

function EnableDisableTB(evalue, eid) {
    document.getElementById("WOContainerNoId").style.display = "block";
    document.getElementById("WorkOrderEmptyContNo").style.display = "none";
    document.getElementById("WOContainerNoId").classList.remove("Contdisabled");
    document.getElementById("WorkOrderTruckNo").disabled = true;
    document.getElementById("WorkOrderSection49ContNo").disabled = true;
    //document.getElementById("WOEquipmentId").classList.remove("Contdisabled");
    //document.getElementById("WOVendorId").classList.remove("Contdisabled");
    document.getElementById("WorkOrderEquipmentType").disabled = true;

    if (evalue == "Yes" && eid == "ContainerHold" + eid.charAt(eid.length - 1)) {
        document.getElementById("ContainerHoldRemarks" + eid.charAt(eid.length - 1)).disabled = false;
        document.getElementById("ContainerHoldAgency" + eid.charAt(eid.length - 1)).disabled = false;
        document.getElementById("ContainerHoldDate" + eid.charAt(eid.length - 1)).disabled = false;
        return;
    }
    else if (eid == "ContainerHold" + eid.charAt(eid.length - 1)) {
        document.getElementById("ContainerHoldRemarks" + eid.charAt(eid.length - 1)).disabled = true;
        document.getElementById("ContainerHoldAgency" + eid.charAt(eid.length - 1)).disabled = true;
        document.getElementById("ContainerHoldDate" + eid.charAt(eid.length - 1)).disabled = true;
        return;
    }

    if (evalue == "Yes" && eid == "LoadContContainerHold") {
        document.getElementById("LoadContContainerHoldRemarks").disabled = false;
        document.getElementById("LoadContContainerHoldAgency").disabled = false;
        document.getElementById("LoadContContainerHoldDate").disabled = false;
        return;
    }
    else if (eid == "LoadContContainerHold") {
        document.getElementById("LoadContContainerHoldRemarks").disabled = true;
        document.getElementById("LoadContContainerHoldAgency").disabled = true;
        document.getElementById("LoadContContainerHoldDate").disabled = true;
        return;
    }

    if (evalue == "Yes" && eid == "ContainerClaimDetails" + eid.charAt(eid.length - 1)) {
        document.getElementById("ContainerClaimAmount" + eid.charAt(eid.length - 1)).disabled = false;
        document.getElementById("ContainerPaymentDate" + eid.charAt(eid.length - 1)).disabled = false;
        return;
    }
    else if (eid == "ContainerClaimDetails" + eid.charAt(eid.length - 1)) {
        document.getElementById("ContainerClaimAmount" + eid.charAt(eid.length - 1)).disabled = true;
        document.getElementById("ContainerPaymentDate" + eid.charAt(eid.length - 1)).disabled = true;
        return;
    }

    if (evalue == "Seal Cutting") {
        //document.getElementById("WOEquipmentId").classList.add("Contdisabled");
        //document.getElementById("WOVendorId").classList.remove("Contdisabled");
    }
    else if (evalue == "Examination" || evalue == "De-Stuffing") {
        document.getElementById("WorkOrderEquipmentType").disabled = false;
    }
    else if (evalue == "Empty Container Out") {
        //document.getElementById("WOEquipmentId").classList.add("Contdisabled");
        //document.getElementById("WOVendorId").classList.add("Contdisabled");
        document.getElementById("WorkOrderTruckNo").disabled = false;
    }
    else if (evalue == "FCL Cargo Out") {
        document.getElementById("WorkOrderTruckNo").disabled = false;
    }
    else if (evalue == "Loaded Container Out") {
        //document.getElementById("WOEquipmentId").classList.add("Contdisabled");
        //document.getElementById("WOVendorId").classList.add("Contdisabled");
        document.getElementById("WorkOrderTruckNo").disabled = false;
    }
    else if (evalue == "Empty Truck In" || evalue == "Empty Truck Out") {
        document.getElementById("WorkOrderTruckNo").disabled = false;
        document.getElementById("WOContainerNoId").classList.add("Contdisabled");
    }
    else if (evalue == "Empty Container In") {
        document.getElementById("WorkOrderTruckNo").disabled = false;
        document.getElementById("WorkOrderEmptyContNo").style.display = "block";
        document.getElementById("WOContainerNoId").style.display = "none";
        document.getElementById("WOContainerNoId").classList.remove("Contdisabled");
    }

    if (evalue == "Section-49") {
        document.getElementById("ContLableNo").innerHTML = "De-stuffing Container No<span style='color: red;'>&nbsp;*</span>";
        document.getElementById("WorkOrderSection49ContNo").disabled = false;
    }
    else { document.getElementById("ContLableNo").innerHTML = "Container No<span style='color: red;'>&nbsp;*</span>"; }
}

function LoadDropDownValuesonblur(e) {

    if (e.id == "MainIGMNo") { document.getElementById("MainItemNoList").innerHTML = ""; }
    if (e.id == "MainItemNo") { document.getElementById("ContainerNoList").innerHTML = ""; }
    if (e.id == "GeneralPOLCountry") { document.getElementById("GeneralPOLList").innerHTML = ""; }
    if (e.id == "GeneralPOL") { document.getElementById("GeneralPOLCode").value = ""; }
    if (e.id == "ReportCompanyName" || e.id == "MainCompanyName") { document.getElementById("BranchNameList").innerHTML = ""; }
    if (e.id == "TransportDRFNo") { document.getElementById("TransportDRFIssuedDate").value = ""; document.getElementById("TransportTransportName").value = ""; }
    if (e.id == "ContainerISOCode") { document.getElementById("ContainerSize").value = ""; document.getElementById("ContainerType").value = ""; document.getElementById("ContainerTareWeight").value = ""; }

    if (e.value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LoadDropDownValuesonblur",
            data: "{'TextBoxValue':'" + e.value + "', 'IGMNo':'" + document.getElementById("MainIGMNo").value + "', 'JobNo':'" + document.getElementById("MainJobNo").value + "', 'Country':'" + document.getElementById("GeneralPOLCountry").value + "'}",
            dataType: "json",
            success: function (data) {
                var MultiContainerno = "<label><input type='checkbox' onclick='SelectAllCheckBoxDD(this)' value='Select All' />Select All</label><hr/>", ContExists = false;

                $.each(data.d, function (key, ListValue) {
                    if (ListValue.ReturnedValue != "" && ListValue.ReturnedValue != null) { showErrorPopup(ListValue.ReturnedValue, 'red'); return; }

                    if (ListValue.ddname == "Branch Name" && (e.id == "ReportCompanyName" || e.id == "MainCompanyName")) {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null) {
                            $("#BranchNameList").append($("<option></option>").html(ListValue.ddvalue));
                        }
                    }

                    if (ListValue.ddname == "Item No" && e.id == "MainIGMNo") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            $("#MainItemNoList").append($("<option></option>").html(ListValue.ddvalue));
                    }
                    if (ListValue.ddname == "Port Name" && e.id == "GeneralPOLCountry") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            $("#GeneralPOLList").append($("<option></option>").html(ListValue.ddvalue));
                    }
                    if (ListValue.ddname == "Port Code" && e.id == "GeneralPOL") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            document.getElementById("GeneralPOLCode").value = ListValue.ddvalue;
                    }
                    if (ListValue.ddname == "Container No" && e.id == "MainItemNo") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            $("#ContainerNoList").append($("<option></option>").html(ListValue.ddvalue));
                    }
                    if (ListValue.ddname == "Only Gated In Container No" && e.id == "MainItemNo") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null) {
                            MultiContainerno += "<label><input type='checkbox' value='" + ListValue.ddvalue + "' />" + ListValue.ddvalue + "</label>";
                            ContExists = true;
                        }
                    }
                    if (ListValue.ddname == "DRF Date" && e.id == "TransportDRFNo") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            document.getElementById("TransportDRFIssuedDate").value = ListValue.ddvalue;
                    }
                    if (ListValue.ddname == "DRF TransportName" && e.id == "TransportDRFNo") {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            document.getElementById("TransportTransportName").value = ListValue.ddvalue;
                    }
                    var EndDigit = e.id.substring(e.id.length - 1, e.id.length);
                    if (ListValue.ddname == "Container Size" && e.id == "ContainerISOCode" + EndDigit) {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            document.getElementById("ContainerSize" + EndDigit).value = ListValue.ddvalue;
                    }
                    if (ListValue.ddname == "Container Type" && e.id == "ContainerISOCode" + EndDigit) {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            document.getElementById("ContainerType" + EndDigit).value = ListValue.ddvalue;
                    }
                    if (ListValue.ddname == "Container TareWeight" && e.id == "ContainerISOCode" + EndDigit) {
                        if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                            document.getElementById("ContainerTareWeight" + EndDigit).value = ListValue.ddvalue;
                    }
                });

                if (ContExists) { document.getElementById("WorkOrderContainerNo").innerHTML = MultiContainerno; }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red');
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}

function LoadDDWhileSearch(IGM, Item, JobNo, Country, PortName) {

    document.getElementById("MainItemNoList").innerHTML = "";
    document.getElementById("ContainerNoList").innerHTML = "";
    document.getElementById("GeneralPOLList").innerHTML = "";
    document.getElementById("GeneralPOLCode").value = "";
    document.getElementById("TransportDRFNoList").value = "";
    document.getElementById("StuffStuffingContainerNoList").value = "";

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/LoadDDWhileSearch",
        data: "{'IGM':'" + IGM + "', 'Item':'" + Item + "', 'JobNo':'" + JobNo + "', 'Country':'" + Country + "', 'PortName':'" + PortName + "'}",
        dataType: "json",
        success: function (data) {
            $.each(data.d, function (key, ListValue) {
                if (ListValue.ReturnedValue != "" && ListValue.ReturnedValue != null) { showErrorPopup(ListValue.ReturnedValue, 'red'); return; }

                if (ListValue.ddname == "DRF No") {
                    if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                        $("#TransportDRFNoList").append($("<option></option>").html(ListValue.ddvalue));
                }
                if (ListValue.ddname == "Item No") {
                    if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                        $("#MainItemNoList").append($("<option></option>").html(ListValue.ddvalue));
                }
                if (ListValue.ddname == "Port Name") {
                    if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                        $("#GeneralPOLList").append($("<option></option>").html(ListValue.ddvalue));
                }
                if (ListValue.ddname == "Port Code") {
                    if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                        document.getElementById("GeneralPOLCode").value = ListValue.ddvalue;
                }
                if (ListValue.ddname == "Container No") {
                    if (ListValue.ddvalue != "" && ListValue.ddvalue != null)
                        $("#ContainerNoList").append($("<option></option>").html(ListValue.ddvalue));
                }
                if (ListValue.ddname == "Empty Container No") {
                    if (ListValue.ddvalue != "" && ListValue.ddvalue != null) {
                        $("#StuffStuffingContainerNoList").append($("<option></option>").html(ListValue.ddvalue));
                    }
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red');
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}
function CheckEmpty1() {
    if (document.getElementById("MainJobNo").value != "") {
        JobNoWrite();
        document.getElementById("FormDownloadBtn").click();
    }
    else {
        showErrorPopup('Job No is Required.', 'red');
        return;
    }
}
function CheckEmpty() {
    if (document.getElementById("MainJobNo").value != "" && document.getElementById("MainIGMNo").value != "" && document.getElementById("MainItemNo").value != "") {
        document.getElementById("DupJobNoNumber").value = document.getElementById("MainIGMNo").value;
        document.getElementById("TSAExcelFormat").click();
    }
    else {
        showErrorPopup('Please upload the details for the IGM. Start by searching for the IGM first', 'red');
        return;
    }
}

function TSADataUpdate() {
    if (document.getElementById("MainJobNo").value != "" && document.getElementById("MainIGMNo").value != "") {

        var table = document.getElementById("TSAUploadtable");
        var TSADataList = new Array();
        for (var t = 1; t < table.rows.length; t++) {
            var row = table.rows[t];
            var DataFields = {};
            DataFields.MainJobNo = row.cells.item(4).innerText;
            DataFields.MainItemNo = row.cells.item(0).innerText;
            DataFields.MainLineName = row.cells.item(1).innerText;
            DataFields.MainTSANo = document.getElementById("TSANo" + t).value;
            DataFields.MainTSADate = document.getElementById("TSADate" + t).value;
            TSADataList.push(DataFields);
        }

        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/TSADataUpdate",
            data: JSON.stringify({ TSADataList: TSADataList }),
            dataType: "json",
            success: function (data) {
                if (data.d == "Updated") {
                    TSADocumentUploadTableSearchData();
                }
                else {
                    showErrorPopup(data.d, 'red');
                }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red');
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        showErrorPopup('Please update the details against the IGM. Start by searching for the IGM first', 'red');
        return;
    }
}

function DRFandScanListUpload(TBId) {
    try {
        const excel_file = document.getElementById(TBId);

        if (excel_file.value != "" && document.getElementById("MainJobNo").value != "" && document.getElementById("MainIGMNo").value != "" && document.getElementById("MainItemNo").value != "") {
            if (!['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.ms-excel'].includes(excel_file.files[0].type)) {
                showErrorPopup('Only .xlsx or .xls file formats are allowed', 'red');
                excel_file.value = '';
                return false;
            }

            var reader = new FileReader();
            reader.readAsArrayBuffer(excel_file.files[0]);
            reader.onload = function (event) {
                var data = new Uint8Array(reader.result);
                var work_book = XLSX.read(data, { type: 'array' });
                var sheet_name = work_book.SheetNames;
                var sheet_data1 = XLSX.utils.sheet_to_json(work_book.Sheets[sheet_name[0]], { header: 1 }); //DRF Details

                if (sheet_data1.length > 0) {
                    var SheetData1List = new Array();
                    for (var row = 3; row < sheet_data1.length; row++) {
                        if (row > 2) {
                            var SheetData1Fields = {};
                            SheetData1Fields.ExcelIGMNo = document.getElementById("MainIGMNo").value;
                            SheetData1Fields.ExcelContainerNo = sheet_data1[row][0];
                            SheetData1Fields.ExcelScanType = sheet_data1[row][8];
                            SheetData1Fields.ExcelScanLocation = sheet_data1[row][9];
                            SheetData1Fields.ExcelDRFNo = sheet_data1[row][5];
                            SheetData1Fields.ExcelDRFIssuedDate = excelSerialToDateTime(sheet_data1[row][6], true);
                            SheetData1Fields.ExcelTransportName = sheet_data1[row][7];
                            SheetData1List.push(SheetData1Fields);

                            if (sheet_data1[row][8] != "" && typeof sheet_data1[row][8] != "undefined") {
                                if (document.getElementById("ContainerScanTypeList").options.length > 0) {
                                    var ScantypeTest = Findthevalueinlist("ContainerScanTypeList", sheet_data1[row][8].toString().trim());
                                    if (!ScantypeTest) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the scan type field doesn’t contains this value ' + "[" + sheet_data1[row][8] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }
                            if (sheet_data1[row][9] != "" && typeof sheet_data1[row][9] != "undefined") {
                                if (document.getElementById("ContainerScanLocationList").options.length > 0) {
                                    var ScanLocationTest = Findthevalueinlist("ContainerScanLocationList", sheet_data1[row][9].toString().trim());
                                    if (!ScanLocationTest) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Scan Location field doesn’t contains this value ' + "[" + sheet_data1[row][9] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }
                            if (sheet_data1[row][7] != "" && typeof sheet_data1[row][7] != "undefined") {
                                if (document.getElementById("TransportNameList").options.length > 0) {
                                    var TransportNameTest = Findthevalueinlist("TransportNameList", sheet_data1[row][7].toString().trim());
                                    if (!TransportNameTest) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the TransportName field doesn’t contains this value ' + "[" + sheet_data1[row][7] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    showErrorPopup('Uploaded Excel is Empty !!!', 'red');
                    return;
                }

                $('#cover-spin').show(0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../CFS/CFSImport.aspx/DRFandScanListUpload",
                    data: JSON.stringify({ SheetData1List: SheetData1List }),
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "Saved") {
                            TSADocumentUploadTableSearchData();
                        }
                        else {
                            showErrorPopup(data.d, 'red');
                        }
                        setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    },
                    error: function (result) {
                        showErrorPopup(result.responseText, 'red');
                        setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    }
                });
            };
        }
        else {
            showErrorPopup('Please upload the details for the IGM. Start by searching for the IGM first', 'red');
            return;
        }
    }
    catch (Error) {
        showErrorPopup(Error.message, 'red');
        return;
    }
}

function Findthevalueinlist(ListId, SearchText) {

    var dataList = document.getElementById(ListId); // Get the datalist element 
    var options = dataList.options; // Get all options inside the datalist
    var found = false; // Flag to check if the text is found
    // Loop through the options to find a match
    for (var i = 0; i < options.length; i++) {
        if (options[i].value === SearchText) {
            found = true;
            break;
        }
    }
    return found;
}

function SelectAllCheckBoxDD(e, TBid) {
    let checkboxes = document.querySelectorAll('#' + TBid + ' input[type="checkbox"]');
    checkboxes.forEach(function (checkbox) {
        if (e.checked == true) {
            checkbox.checked = true;
        }
        else {
            checkbox.checked = false;
        }
    });
}

let expanded = false;
function showCheckboxes(TBid) {
    let checkboxes = document.querySelector('#' + TBid + '.checkboxes');
    if (!expanded) {
        checkboxes.style.display = "block";
        expanded = true;
    } else {
        checkboxes.style.display = "none";
        expanded = false;
    }
}

// Hide checkboxes when clicking outside the multiselect
document.addEventListener('click', function (event) {
    let multiselects = document.querySelectorAll('.multiselect');
    let checkboxes = document.querySelectorAll(".checkboxes");

    checkboxes.forEach(function (checkbox, index) {
        // Check if the click is outside the multiselect
        if (!multiselects[index].contains(event.target)) {
            if (checkbox.style.display === "block") {
                checkbox.style.display = "none";
                expanded = false;
            }
        }
    });
});

//----------------------------------------New Report js----------------------------------------

function LoadTableColumns() {
    const selectElement = document.getElementById('TableName'); // Getting Values from the [Tab Name] Textbox
    const selectedOption = selectElement.options[selectElement.selectedIndex];

    var TableName = selectedOption.value; // Getting the sql TableName that have been choosed by the user
    var ColumnstoExclude = selectedOption.getAttribute("data-except-columns"); // Getting the Exceptional columns list 
    document.getElementById("ColumnNameList").innerHTML = ""; //Clearing the column name Datalist

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/getcolumnsfromtable",
        data: JSON.stringify({ TableName: TableName, ColumnstoExclude: ColumnstoExclude }),
        dataType: "json",
        success: function (data) {
            for (var j = 1; j < document.getElementById("AvailableColumns").rows.length; j++) { document.getElementById("AvailableColumns").deleteRow(j); j--; } //Removing the table rows to append new

            $.each(data.d, function (key, ListValue) {
                if (ListValue.ErrorValue != "") {
                    showErrorPopup(ListValue.ErrorValue, 'red'); //Error Message
                }
                else {
                    //Inserting the column names to the table
                    var row = document.getElementById("AvailableColumns").insertRow(document.getElementById("AvailableColumns").rows.length);
                    var cell0 = row.insertCell(0); cell0.innerHTML = "<input data-table-name='" + TableName + "' data-column='" + ListValue.ColumnName + "' type='checkbox' style='width: 15px; height: 15px' />";
                    var cell1 = row.insertCell(1); cell1.innerHTML = ListValue.ColumnName;
                    //Loading the column names to the Condition tab - Column Name Field
                    $("#ColumnNameList").append($("<option></option>").html(ListValue.ColumnName));
                }
            });
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red');
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function SelectAllcb(tableid, tbid) {
    // Get the header checkbox element that triggered the event
    const headerCheckbox = document.getElementById(tbid);

    // Select all checkboxes in the table's tbody
    const checkboxes = document.querySelectorAll("#" + tableid + " thead input[type='checkbox']");

    // Set each checkbox's checked property to match the header checkbox
    checkboxes.forEach(checkbox => {
        checkbox.checked = headerCheckbox.checked;
    });
}

function MovetoOtherTable(FromTab, ToTab) {
    var TargetTable = document.getElementById(ToTab); //Getting the Target table from the id
    var ColumnTableName = document.getElementById("TableName").value;

    // Select all checkboxes in the table's tbody
    const Fromcheckboxes = document.querySelectorAll("#" + FromTab + " thead input[type='checkbox']");
    // Set each checkbox's checked property to match the header checkbox
    Fromcheckboxes.forEach(checkbox => {
        if (checkbox.checked == true) {
            if (checkbox.id != "AcSelectall") {
                if (checkbox.id != "ScSelectall") {
                    if (checkbox.getAttribute("data-table-name") == ColumnTableName) {
                        var row = TargetTable.insertRow(TargetTable.rows.length);
                        var i = TargetTable.rows.length;
                        row.setAttribute("draggable", "true"); // Make row draggable
                        row.setAttribute("id", "row" + i); // Unique id for each row
                        var cell0 = row.insertCell(0); cell0.innerHTML = "<input data-table-name='" + ColumnTableName + "' data-column='" + checkbox.getAttribute("data-column") + "' type='checkbox' style='width: 15px; height: 15px' />";
                        var cell1 = row.insertCell(1); cell1.innerHTML = checkbox.getAttribute("data-column");
                        if (ToTab == "SelectedColumns") { var cell2 = row.insertCell(2); cell2.innerHTML = "<input style='width:100% !important;' value='' title='Set a display name for the column.' type='text' class='textbox' />"; }
                    }

                    checkbox.closest('tr').remove();
                    enableDragAndDrop();
                }
            }
        }
    });
}

function GetDatatypeandValue(evalue) {
    var ColumnTableName = document.getElementById("TableName").value;
    document.getElementById("ColumnValueList").innerHTML = "";

    if (evalue != "" && ColumnTableName != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/GetDatatype",
            data: JSON.stringify({ evalue: evalue, ColumnTableName: ColumnTableName }),
            dataType: "json",
            success: function (data) {
                var o = 1;
                $.each(data.d, function (key, ListValue) {
                    if (ListValue.ErrorValue != "") {
                        showErrorPopup(ListValue.ErrorValue, 'red');
                    }
                    else {
                        $("#ColumnValueList").append($("<option></option>").html(ListValue.ColumnValues));
                        if (o == 1) {
                            document.getElementById("DataType").value = ListValue.DataType;
                            LoadOperatorsBasedondatatype(ListValue.DataType);
                        }
                        o++;
                    }
                });
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red');
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else {
        document.getElementById("OperatorList").innerHTML = "";
        document.getElementById("DataType").value = "";
    }
}

function LoadOperatorsBasedondatatype(DataType) {
    document.getElementById("OperatorList").innerHTML = "";
    document.getElementById("Operator").disabled = false;

    let data = [];
    [
        ["varchar", [["Equal To", "="], ["Not Equal To", "<>"], ["Add the values using comma (Ex: 1,2,3)", "In"], ["Filters based on textual criteria", "Like"]]],
        ["nvarchar", [["Equal To", "="], ["Not Equal To", "<>"], ["Add the values using comma (Ex: 1,2,3)", "In"], ["Filters based on textual criteria", "Like"]]],
        ["int", [["Less than or Equal To", "<="], ["Greater than or Equal To", ">="], ["Less than", "<"], ["Greater than", ">"]]],
        ["float", [["Less than or Equal To", "<="], ["Greater than or Equal To", ">="], ["Less than", "<"], ["Greater than", ">"]]],
        ["bigint", [["Less than or Equal To", "<="], ["Greater than or Equal To", ">="], ["Less than", "<"], ["Greater than", ">"]]],
        ["real", [["Less than or Equal To", "<="], ["Greater than or Equal To", ">="], ["Less than", "<"], ["Greater than", ">"]]]
    ].forEach(([type, comps]) => comps.forEach(([op, sym]) => data.push([type, op, sym])));

    for (var l = 0; l < data.length; l++) {
        if (DataType == "bigint" || DataType == "float" || DataType == "int" || DataType == "real") {
            if (l == 0) { document.getElementById("ColumnCondition").type = "number"; }
            if (DataType == data[l][0]) {
                $("#OperatorList").append($("<option value='" + data[l][2] + "'></option>").html(data[l][1]));
            }
        }
        if (DataType == "nvarchar" || DataType == "varchar") {
            if (l == 0) { document.getElementById("ColumnCondition").type = "text"; }
            if (DataType == data[l][0]) {
                $("#OperatorList").append($("<option value='" + data[l][2] + "'></option>").html(data[l][1]));
            }
        }
    }

    if (DataType == "datetime" || DataType == "date") {
        document.getElementById("ColumnCondition").type = "text";
        document.getElementById("Operator").disabled = true;
        let listvalues = ["Today", "Yesterday", "This Week", "Last Week", "This Month", "Last Month", "This Calendar Year", "Last Calendar Year", "This Fiscal Year", "Last Fiscal Year"];
        listvalues.forEach(value => {
            $("#ColumnValueList").append($("<option></option>").text(value).val(value));
        });
    }
}

function TemplateSaveBtnJSFunction(SaveorUpdate) {
    var TemplateData = new Array();
    //Need to work on in manual based requirements
    var CombinedColumns = "CFSImportContainerWiseData.MainJobNo,", //Set the key column common for all the tables to display as first column in select qry
        JoinedTables = "CFSImportContainerWiseData "; //Set the Main table name to join it as first table while left join

    var TemplateCondtionData = new Array();
    const pathname = new URL(window.location.href).pathname;

    var table1 = document.getElementById("ReportConditionTable");
    var table2 = document.getElementById("SelectedColumns");
    for (var i = 1; i < table1.rows.length; i++) {
        var row = table1.rows[i];
        var Condtableview = {};
        Condtableview.ColumnNames = row.cells.item(0).innerText;
        Condtableview.DataTypes = row.cells.item(1).innerText;
        Condtableview.Operators = row.cells.item(2).innerText;
        Condtableview.Values = row.cells.item(3).innerText;
        Condtableview.TabNames = row.cells.item(4).innerText;
        Condtableview.SaveorUpdate = SaveorUpdate;
        TemplateCondtionData.push(Condtableview);
    }

    if (table2.rows.length > 1) {
        var tableview = {};
        tableview.ModuleName = "Operations";
        tableview.FormName = pathname.substring(pathname.lastIndexOf('/') + 1);
        tableview.Templatename = document.getElementById("TemplatenametoSave").value;
        tableview.SaveorUpdate = SaveorUpdate;
        if (document.getElementById("TemplatenametoSave").value == "") {
            showErrorPopup('Kindly Enter the Template Name.', 'red');
            return;
        }

        const checkboxes = document.querySelectorAll("#SelectedColumns thead input[type='checkbox']"); var f = 1;
        let TableNames = [];
        checkboxes.forEach(checkbox => {
            var tablename = checkbox.getAttribute("data-table-name"), columnname = checkbox.getAttribute("data-column");
            if ((tablename != "" && tablename != null) && (columnname != "" && columnname != null)) {
                if (columnname != "MainJobNo") {//Need to work on in manual based requirements

                    const closestTextInput = checkbox.closest('tr')?.querySelector("input[type='text']");
                    var ValueExists = false;
                    if (closestTextInput) {
                        const inputValue = closestTextInput.value;
                        if (inputValue != "" && inputValue != null) {
                            CombinedColumns += tablename + "." + columnname + " As [" + inputValue + "],"; //Gethering Columns
                            ValueExists = true;
                        }
                    }
                    if (!ValueExists) {
                        CombinedColumns += tablename + "." + columnname + ","; //Gethering Columns
                    }
                }

                if (!TableNames.includes(tablename)) {
                    TableNames.push(tablename);

                    if (tablename != "CFSImportContainerWiseData") {//Need to work on in manual based requirements                        
                        JoinedTables += "Left Join " + tablename + " On " + tablename + ".RecordStatus = CFSImportContainerWiseData.RecordStatus And " + tablename + ".MainJobNo = CFSImportContainerWiseData.MainJobNo ";//Need to work on in manual based requirements
                    }
                }
            }
        });

        tableview.Query = "Select Distinct " + CombinedColumns.slice(0, -1) + " From " + JoinedTables;
        TemplateData.push(tableview);
    }
    else {
        showErrorPopup('No columns has been selected.', 'red');
        return;
    }

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/TemplateCreate",
        data: JSON.stringify({ TemplateData: TemplateData, TemplateCondtionData: TemplateCondtionData }),
        dataType: "json",
        success: function (data) {
            if (data.d == "Saved") {
                showErrorPopup(data.d, 'green');
                document.getElementById("TemplatenametoSave").disabled = false;
            }
            else { showErrorPopup(data.d, 'red'); }

            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red');
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}

function ConditionAddBtnJSFunction() {

    // Select all checkboxes in the table's tbody
    const checkboxes = document.querySelectorAll("#SelectedColumns thead input[type='checkbox']"); var TableExists = false;
    // Set each checkbox's checked property to match the header checkbox
    checkboxes.forEach(checkbox => {
        if (checkbox.getAttribute("data-table-name") == document.getElementById('TableName').value) {
            TableExists = true;
        }
    });

    if (TableExists) {
        if (document.getElementById('ColumnName').value != "" && document.getElementById('ColumnCondition').value != "") {
            if (document.getElementById('Operator').disabled == false && document.getElementById('Operator').value == "") {
                showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
            }
            var table = document.getElementById("ReportConditionTable");
            var rowcount = table.rows.length;
            var row = table.insertRow(rowcount);
            var cell1 = row.insertCell(0); cell1.innerHTML = "<span onclick='ConditionEditJSFunction(this)' style='color=blue;'><b><u>" + document.getElementById('ColumnName').value + "</u></b></span>";
            var cell2 = row.insertCell(1); cell2.innerHTML = document.getElementById('DataType').value;
            var cell3 = row.insertCell(2); cell3.innerHTML = document.getElementById('Operator').value;
            var cell4 = row.insertCell(3); cell4.innerHTML = document.getElementById('ColumnCondition').value;
            var cell5 = row.insertCell(4); cell5.innerHTML = document.getElementById('TableName').value;

            document.getElementById('ColumnName').value = "";
            document.getElementById('DataType').value = "";
            document.getElementById('Operator').value = "";
            document.getElementById('ColumnCondition').value = "";
            document.getElementById('ColumnCondition').type = "text";

            document.getElementById('OperatorList').innerHTML = "";
            document.getElementById('ColumnValueList').innerHTML = "";
        }
        else {
            showErrorPopup('Kindly Enter all the required fields (*).', 'red'); return;
        }
    }
    else {
        if (document.getElementById("SelectedColumns").rows.length > 1) { showErrorPopup('The condition column table doesn’t match the selected column table.', 'red'); }
        else { showErrorPopup('Kindly Select the columns first.', 'red'); }
        return;
    }
}
function ConditionCancelBtnJSFunction() {
    document.getElementById('ColumnName').value = "";
    document.getElementById('DataType').value = "";
    document.getElementById('Operator').value = "";
    document.getElementById('ColumnCondition').value = "";
    document.getElementById('ColumnCondition').type = "text";

    document.getElementById('OperatorList').innerHTML = "";
    document.getElementById('ColumnValueList').innerHTML = "";
}
function ConditionEditJSFunction(btn) {
    var row = btn.parentNode.parentNode;

    document.getElementById('ColumnName').value = row.cells.item(0).innerText;
    document.getElementById('DataType').value = row.cells.item(1).innerText;
    document.getElementById('Operator').value = row.cells.item(2).innerText;
    document.getElementById('ColumnCondition').value = row.cells.item(3).innerText;

    row.parentNode.removeChild(row);
}

function LoadDataBasedonCont(evalue) {
    if (evalue != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/LoadDataBasedonCont",
            data: JSON.stringify({ evalue: evalue }),
            dataType: "json",
            success: function (data) {
                if (data.d.startsWith("ErrorMessage : ")) { showErrorPopup(data.d, 'red'); }
                else {
                    // Append a query parameter to the URL
                    const newParam = "MainJobNo=" + data.d.trim();
                    const currentUrl = window.location.href.split('?')[0]; // Remove existing query parameters
                    const newUrl = `${currentUrl}?${newParam}`;

                    // Update the URL without reloading the page
                    history.pushState(null, '', newUrl);

                    document.getElementById("MainJobNo").value = data.d.trim();
                    MainTableSearchData();
                }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}

function TemplateSearchData(TemName) {
    if (TemName != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/TemplateSearchData",
            data: JSON.stringify({ TemName: TemName }),
            dataType: "json",
            success: function (data) {
                document.getElementById("TemplatenametoSave").value = TemName;
                document.getElementById("TemplatenametoSave").disabled = true;
                var TableNo1 = data.d.Item1;
                var TableNo2 = data.d.Item2;

                var RemoveTableRows1 = document.getElementById("ReportConditionTable");
                for (var j = 1; j < RemoveTableRows1.rows.length; j++) { RemoveTableRows1.deleteRow(j); j--; }
                var RemoveTableRows2 = document.getElementById("SelectedColumns");
                for (var k = 2; k < RemoveTableRows2.rows.length; k++) { RemoveTableRows2.deleteRow(k); k--; }
                $.each(TableNo1, function (key, value) {
                    if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }
                    var table = document.getElementById("SelectedColumns");
                    var rowcount = table.rows.length;
                    var row = table.insertRow(rowcount);
                    var cell0 = row.insertCell(0); cell0.innerHTML = "<input data-table-name='" + value.tabname + "' data-column='" + value.colsname + "' type='checkbox' style='width: 15px; height: 15px' />";;
                    var cell1 = row.insertCell(1); cell1.innerHTML = value.colsname;
                    var cell2 = row.insertCell(2); cell2.innerHTML = "<input style='width:100% !important;' value='" + value.RenameValue + "' title='Set a display name for the column.' type='text' class='textbox' />";
                });

                $.each(TableNo2, function (key, value) {
                    if (value.ReturnedValue != null && value.ReturnedValue != "") { showErrorPopup(value.ReturnedValue, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000); return; }

                    var table = document.getElementById("ReportConditionTable");
                    var rowcount = table.rows.length;
                    var row = table.insertRow(rowcount);
                    var cell0 = row.insertCell(0); cell0.innerHTML = "<span onclick='ConditionEditJSFunction(this)' style='color: blue;'><b><u>" + value.ColumnName + "</u></b></span>";
                    var cell1 = row.insertCell(1); cell1.innerHTML = value.DataType;
                    var cell2 = row.insertCell(2); cell2.innerHTML = value.Operator;
                    var cell3 = row.insertCell(3); cell3.innerHTML = value.ColumnValue;
                    var cell4 = row.insertCell(4); cell4.innerHTML = value.TableName;
                });

                if (data.d == "No Record Found") { showErrorPopup(data.d, 'red'); }
                else { $("#ReportTable").html(data.d); }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red'); setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
}

function CheckDownload() {
    document.getElementById("MainIGMUploadTb").value = document.getElementById("MainIGMNo").value;
    document.getElementById("MainIGMUploadBt").click();
}

//----------------------------------------New Report js----------------------------------------

//Extracting values from the excel and loading into a HTML Table
function ReadingDataFromExcel(TBId) {
    try {
        const excel_file = document.getElementById(TBId);

        if (excel_file.value != "") {
            if (!['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.ms-excel'].includes(excel_file.files[0].type)) {
                showErrorPopup('Only .xlsx or .xls file formats are allowed', 'red');
                excel_file.value = '';
                return false;
            }

            var reader = new FileReader();
            reader.readAsArrayBuffer(excel_file.files[0]);
            reader.onload = function (event) {
                var data = new Uint8Array(reader.result);
                var work_book = XLSX.read(data, { type: 'array' });
                var sheet_name = work_book.SheetNames;
                var sheet_data = XLSX.utils.sheet_to_json(work_book.Sheets[sheet_name[0]], { header: 1 }); //Basic Info                               

                if (sheet_data.length == 2) {
                    var SheetDataList = new Array();
                    var SheetDataFields = {};
                    SheetDataFields.IGMNo = sheet_data[1][0];
                    SheetDataFields.IGMDate = excelSerialToDateTime(sheet_data[1][1]);
                    SheetDataFields.Vessel = sheet_data[1][2];
                    SheetDataFields.VIANo = sheet_data[1][3];
                    SheetDataFields.VoyNo = sheet_data[1][4];
                    SheetDataFields.POA = sheet_data[1][5];
                    SheetDataFields.GeneralPortofDischarge = sheet_data[1][5];
                    //SheetDataFields.GeneralCHAName = sheet_data[1][6];
                    SheetDataFields.GeneralAccountHolderName = sheet_data[1][6];
                    SheetDataFields.JobOwner = sheet_data[1][7];
                    SheetDataList.push(SheetDataFields);

                    // if (sheet_data[1][2] != "" && typeof sheet_data[1][2] != "undefined") { //Vessel
                    // if (document.getElementById("MainVesselNameList").options.length > 0) {
                    // if (!Findthevalueinlist("MainVesselNameList", sheet_data[1][2].toString().trim())) {
                    // showErrorPopup('The dropdown values in the Vessel Name field doesn’t contains this value ' + "[" + sheet_data[1][2] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                    // return;
                    // }
                    // }
                    // }
                    if (sheet_data[1][5] != "" && typeof sheet_data[1][5] != "undefined") { //POA
                        if (document.getElementById("MainPOAList").options.length > 0) {
                            if (!Findthevalueinlist("MainPOAList", sheet_data[1][5].toString().trim())) {
                                showErrorPopup('The dropdown values in the POA field doesn’t contains this value ' + "[" + sheet_data[1][5] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                return;
                            }
                        }
                    }
                    if (sheet_data[1][6] != "" && typeof sheet_data[1][6] != "undefined") { //GeneralAccountHolderName
                        if (document.getElementById("GeneralAccountHolderList").options.length > 0) {
                            if (!Findthevalueinlist("GeneralAccountHolderList", sheet_data[1][6].toString().trim())) {
                                showErrorPopup('The dropdown values in the Account Holder field doesn’t contains this value ' + "[" + sheet_data[1][6] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                return;
                            }
                        }
                    }
                }
                else {
                    showErrorPopup('Kindly input the data into the General Sheet or ensure that all data is placed in one row in Excel.', 'red');
                    return;
                }

                var sheet_data1 = XLSX.utils.sheet_to_json(work_book.Sheets[sheet_name[2]], { header: 1 }); //Liner Details

                if (sheet_data1.length > 0) {
                    var SheetData1List = new Array();
                    for (var row = 0; row < sheet_data1.length; row++) {
                        if (row > 0) {
                            var SheetData1Fields = {};
                            SheetData1Fields.LinerItemNo = sheet_data1[row][0];
                            SheetData1Fields.LinerImporterName = sheet_data1[row][1];
                            SheetData1Fields.LinerCHAName = sheet_data1[row][2];
                            SheetData1Fields.LinerLinerAgent = sheet_data1[row][3];
                            SheetData1Fields.LinerBLNumber = sheet_data1[row][4];
                            SheetData1Fields.LinerBLDate = excelSerialToDateTime(sheet_data1[row][5]);
                            SheetData1Fields.LinerIMDG = sheet_data1[row][6];
                            SheetData1Fields.LinerWeightKgs = sheet_data1[row][7];
                            SheetData1Fields.LinerPackages = sheet_data1[row][8];
                            SheetData1Fields.LinerCargoDetails = sheet_data1[row][9];
                            SheetData1Fields.LinerTSANumber = sheet_data1[row][10];
                            SheetData1Fields.LinerTSADate = excelSerialToDateTime(sheet_data1[row][11]);
                            SheetData1Fields.LinerPANNumber = sheet_data1[row][12];
                            SheetData1List.push(SheetData1Fields);

                            // if (sheet_data1[row][1] != "" && typeof sheet_data1[row][1] != "undefined") { //LinerImporterName
                            // if (document.getElementById("LinerImporterNameList").options.length > 0) {
                            // if (!Findthevalueinlist("LinerImporterNameList", sheet_data1[row][1].toString().trim())) {
                            // showErrorPopup('Error - [Row - '+row+'], The dropdown values in the Importer Name field doesn’t contains this value ' + "[" + sheet_data1[row][1] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                            // return;
                            // }
                            // }
                            // }
                            // if (sheet_data1[row][2] != "" && typeof sheet_data1[row][2] != "undefined") { //LinerCHAName
                            // if (document.getElementById("GeneralCHANameList").options.length > 0) {
                            // if (!Findthevalueinlist("GeneralCHANameList", sheet_data1[row][2].toString().trim())) {
                            // showErrorPopup('Error - [Row - '+row+'], The dropdown values in the CHA Name field doesn’t contains this value ' + "[" + sheet_data1[row][2] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                            // return;
                            // }
                            // }
                            // }
                            // if (sheet_data1[row][3] != "" && typeof sheet_data1[row][3] != "undefined") { //LinerLinerAgent
                            // if (document.getElementById("LinerLinerAgentList").options.length > 0) {
                            // if (!Findthevalueinlist("LinerLinerAgentList", sheet_data1[row][3].toString().trim())) {
                            // showErrorPopup('Error - [Row - '+row+'], The dropdown values in the Liner Agent field doesn’t contains this value ' + "[" + sheet_data1[row][3] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                            // return;
                            // }
                            // }
                            // }
                        }
                    }
                }
                else {
                    showErrorPopup('Kindly Enter the data in Liner Sheet on Excel.', 'red');
                    return;
                }

                var sheet_data2 = XLSX.utils.sheet_to_json(work_book.Sheets[sheet_name[4]], { header: 1 }); //Container Details

                if (sheet_data2.length > 0) {
                    var SheetData2List = new Array();
                    for (var row = 0; row < sheet_data2.length; row++) {
                        if (row > 0) {
                            var SheetData2Fields = {};
                            SheetData2Fields.ContainerItemNo = sheet_data2[row][0];
                            SheetData2Fields.ContainerContNo = sheet_data2[row][1];
                            SheetData2Fields.ContainerISOCode = sheet_data2[row][2];
                            SheetData2Fields.ContainerSize = sheet_data2[row][3];
                            SheetData2Fields.ContainerType = sheet_data2[row][4];
                            SheetData2Fields.ContainerSealNo = sheet_data2[row][5];
                            SheetData2Fields.ContainerTareWeight = sheet_data2[row][6];
                            SheetData2Fields.ContainerWeightKgs = sheet_data2[row][7];
                            SheetData2Fields.ContainerCargoWeightKgs = sheet_data2[row][8];
                            SheetData2Fields.CargoNature = sheet_data2[row][9];
                            SheetData2Fields.ContainerNoofPackage = sheet_data2[row][10];
                            SheetData2Fields.ContainerFCLLCL = sheet_data2[row][11];
                            SheetData2Fields.ContainerPrimSec = sheet_data2[row][12];
                            SheetData2Fields.ContainerGroupCode = sheet_data2[row][13];
                            SheetData2Fields.ContainerIMOCode = sheet_data2[row][14];
                            SheetData2Fields.ContainerUNNo = sheet_data2[row][15];
                            SheetData2Fields.ContainerScanType = sheet_data2[row][16];
                            SheetData2Fields.ContainerScanLocation = sheet_data2[row][17];
                            SheetData2Fields.ContainerDeliveryMode = sheet_data2[row][18];
                            SheetData2Fields.ContainerHold = sheet_data2[row][19];
                            SheetData2Fields.ContainerHoldRemarks = sheet_data2[row][20];
                            SheetData2Fields.ContainerHoldAgency = sheet_data2[row][21];
                            SheetData2Fields.ContainerHoldDate = excelSerialToDateTime(sheet_data2[row][22]);
                            SheetData2Fields.ContainerReleaseDate = excelSerialToDateTime(sheet_data2[row][23]);
                            SheetData2Fields.ContainerReleaseRemarks = sheet_data2[row][24];
                            SheetData2Fields.ContainerClaimDetails = sheet_data2[row][25];
                            SheetData2Fields.ContainerClaimAmount = sheet_data2[row][26];
                            SheetData2Fields.ContainerPaymentDate = excelSerialToDateTime(sheet_data2[row][27]);
                            SheetData2Fields.ContainerRemarks = sheet_data2[row][28];
                            SheetData2Fields.ContainerWHLoc = sheet_data2[row][29];
                            SheetData2Fields.ContainerPriority = sheet_data2[row][30];
                            SheetData2List.push(SheetData2Fields);

                            if (sheet_data2[row][2] != "" && typeof sheet_data2[row][2] != "undefined") { //ISO Code
                                if (document.getElementById("ContainerISOCodeList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerISOCodeList", sheet_data2[row][2].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the ISO Code field doesn’t contains this value ' + "[" + sheet_data2[row][2] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][3] != "" && typeof sheet_data2[row][3] != "undefined") { //Size
                                if (document.getElementById("ContainerSizeList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerSizeList", sheet_data2[row][3].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Size field doesn’t contains this value ' + "[" + sheet_data2[row][3] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][4] != "" && typeof sheet_data2[row][4] != "undefined") { //Type
                                if (document.getElementById("ContainerTypeList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerTypeList", sheet_data2[row][4].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Type field doesn’t contains this value ' + "[" + sheet_data2[row][4] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][9] != "" && typeof sheet_data2[row][9] != "undefined") { //Cargo nature
                                if (document.getElementById("ContainerCargoNatureList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerCargoNatureList", sheet_data2[row][9].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Cargo Nature field doesn’t contains this value ' + "[" + sheet_data2[row][9] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][11] != "" && typeof sheet_data2[row][11] != "undefined") { //FCL LCL
                                if (document.getElementById("ContainerFCLLCLList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerFCLLCLList", sheet_data2[row][11].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the FCL/LCL field doesn’t contains this value ' + "[" + sheet_data2[row][11] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][12] != "" && typeof sheet_data2[row][12] != "undefined") { //Primary Secondary
                                if (document.getElementById("ContainerPrimarySecondaryList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerPrimarySecondaryList", sheet_data2[row][12].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Primary/Secondary field doesn’t contains this value ' + "[" + sheet_data2[row][12] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][13] != "" && typeof sheet_data2[row][13] != "undefined") { //Group Code
                                if (document.getElementById("ContainerGroupCodeList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerGroupCodeList", sheet_data2[row][13].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Group Code field doesn’t contains this value ' + "[" + sheet_data2[row][13] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][16] != "" && typeof sheet_data2[row][16] != "undefined") { //Scan Type
                                if (document.getElementById("ContainerScanTypeList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerScanTypeList", sheet_data2[row][16].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the scan type field doesn’t contains this value ' + "[" + sheet_data2[row][16] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][17] != "" && typeof sheet_data2[row][17] != "undefined") { //Scan Location
                                if (document.getElementById("ContainerScanLocationList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerScanLocationList", sheet_data2[row][17].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the scan location field doesn’t contains this value ' + "[" + sheet_data2[row][17] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][18] != "" && typeof sheet_data2[row][18] != "undefined") { //Delivery mode
                                if (document.getElementById("ContainerDeliveryModeList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerDeliveryModeList", sheet_data2[row][18].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Delivery Mode field doesn’t contains this value ' + "[" + sheet_data2[row][18] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][19] != "" && typeof sheet_data2[row][19] != "undefined") { //Hold
                                if (document.getElementById("ContainerHoldList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerHoldList", sheet_data2[row][19].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Hold field doesn’t contains this value ' + "[" + sheet_data2[row][19] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][21] != "" && typeof sheet_data2[row][21] != "undefined") { //Hold Agency
                                if (document.getElementById("ContainerHoldAgencyList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerHoldAgencyList", sheet_data2[row][21].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Hold Agency field doesn’t contains this value ' + "[" + sheet_data2[row][21] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }

                            if (sheet_data2[row][25] != "" && typeof sheet_data2[row][25] != "undefined") { //Claim Details
                                if (document.getElementById("ContainerClaimDetailsList").options.length > 0) {
                                    if (!Findthevalueinlist("ContainerClaimDetailsList", sheet_data2[row][25].toString().trim())) {
                                        showErrorPopup('Error - [Row - ' + row + '], The dropdown values in the Claim Details field doesn’t contains this value ' + "[" + sheet_data2[row][25] + "]" + ', Please update the values in excel based on their dropdown list.', 'red');
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    showErrorPopup('Kindly Enter the data in Container Sheet on Excel, but General & Liner Sheet data is Updated.', 'red');
                    return;
                }

                $('#cover-spin').show(0);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../CFS/CFSImport.aspx/ReadingDataFromExcel",
                    data: JSON.stringify({ SheetDataList: SheetDataList, SheetData1List: SheetData1List, SheetData2List: SheetData2List }),
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "Saved") {
                            showErrorPopup(data.d, 'green');
                        }
                        else {
                            showErrorPopup(data.d, 'red');
                        }
                        setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    },
                    error: function (result) {
                        showErrorPopup(result.responseText, 'red');
                        setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
                    }
                });
            };
        }
    }
    catch (Error) {
        showErrorPopup(Error.message, 'red');
        return;
    }
}

function excelSerialToDateTime(excelSerial, includeTime = false) {
    // Check if the input is a number
    const isNumber = !isNaN(Number(excelSerial));

    if (isNumber) {
        // The Excel date starts from January 1, 1900
        const baseDate = new Date(1900, 0, 1);

        // Excel's leap year bug: we subtract an extra day (because Excel treats 1900 as a leap year)
        const days = Math.floor(excelSerial) - 2;

        // Calculate the date by adding the days to the base date
        const jsDate = new Date(baseDate.getTime() + days * 24 * 60 * 60 * 1000);

        // Format the date as yyyy-MM-dd
        const year = jsDate.getFullYear();
        const month = String(jsDate.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed
        const day = String(jsDate.getDate()).padStart(2, '0');

        let formattedDate = `${year}-${month}-${day}`;

        // If includeTime is true, handle the fractional part (the time)
        if (includeTime) {
            // Get the fractional part of the serial number (the time)
            const fractionalDay = excelSerial - Math.floor(excelSerial);

            // Convert fractional day to milliseconds and add it to the base date
            const msInADay = 24 * 60 * 60 * 1000;
            const timeInMs = Math.round(fractionalDay * msInADay);

            // Create a new date object for the time
            const time = new Date(timeInMs);

            // Extract the time components (HH:mm)
            const hours = String(time.getUTCHours()).padStart(2, '0');
            const minutes = String(time.getUTCMinutes()).padStart(2, '0');

            // Append the time to the date string
            formattedDate += ` ${hours}:${minutes}`;
        }

        return formattedDate;
    }
    else {
        return excelSerial;
    }
}
function JobNoWrite() { document.getElementById("MainJobNoDummy").value = document.getElementById("MainJobNo").value; }

function EnableDisab(e) { if (e.value != "") { if (e.value == "Yes") { document.getElementById("LoadContODCDimension").disabled = false; } else { document.getElementById("LoadContODCDimension").disabled = true; } } }

function openModal(e) {
    var JobNo = document.getElementById("MainJobNo").value;
    if (JobNo != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../CFS/CFSImport.aspx/GettheModalStructure",
            data: JSON.stringify({ JobNoValue: JobNo, ElementId: e.id }),
            dataType: "json",
            success: function (data) {
                if (data.d[0] != "") {
                    showErrorPopup(data.d[0], 'red');
                }
                else {
                    document.getElementById("modal").innerHTML = data.d[1];
                }
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            },
            error: function (result) {
                showErrorPopup(result.responseText, 'red');
                setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
            }
        });
    }
    else { return; }
    document.getElementById("modal").style.display = "block";
    document.getElementById("overlay").style.display = "block";
}
function closeModal() {
    document.getElementById("modal").style.display = "none";
    document.getElementById("overlay").style.display = "none";
}
//Newly Added
function GetDataStructure() {
    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../CFS/CFSImport.aspx/GetDataStructure",
        data: JSON.stringify({
            Year: document.getElementById("Year").value,
            Month: document.getElementById("Month").value
        }),
        dataType: "json",
        success: function (response) {
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);

            var data = response.d;

            if (data.ErrorMsg != "") {
                showErrorPopup(ErrorMsg, 'red');
                return;
            }
            var BarChart = data.Bar.split(',').map(Number);
            var PieChart = data.Pie.split(',').map(Number);

            var baroptions = {
                chart: {
                    type: 'bar',
                    height: 350,
                    toolbar: {
                        show: true,
                        tools: {
                            download: true
                        }
                    }
                },
                series: [{
                    name: 'Handled Containers',
                    data: BarChart
                }],
                xaxis: {
                    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
                },
                colors: ['#3B50DF']
            };

            // Bar Chart Configuration                        
            document.querySelector("#barChart").innerHTML = "";
            var barChart = new ApexCharts(document.querySelector("#barChart"), baroptions);
            barChart.render();

            var pieoptions = {
                chart: {
                    type: 'pie',
                    height: 350,
                    toolbar: {
                        show: true,
                        tools: {
                            download: true
                        }
                    }
                },
                series: PieChart,
                labels: ['Empty Truck In', 'Empty Container In', 'Loaded Container In', 'Seal Cutting', 'Examination', 'Section-49', 'De-Stuffing', 'Loaded Container Out', 'FCL Cargo Out', 'Empty Container Out', 'Empty Truck Out'],
                colors: ['#FF4560', '#008FFB', '#00E396', '#775DD0', '#FEB019', '#FF66C3', '#33B2DF', '#546E7A', '#D4526E', '#F86624', '#F6A766'],
            };

            // Pie Chart Configuration
            document.querySelector("#pieChart").innerHTML = "";
            var pieChart = new ApexCharts(document.querySelector("#pieChart"), pieoptions);
            pieChart.render();
        },
        error: function (result) {
            showErrorPopup(result.responseText, 'red');
            setTimeout(() => { $('#cover-spin').hide(0); }, 3000);
        }
    });
}
//Newly Added
//--------------------------Added Manually--------------------------------------


let SessionUserName = localStorage.getItem("UserName");

function GettingScheduleValues() {

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/ScheduleDetails.aspx/GettingScheduleValuesFromAPI",
        data: "{'UserID':'" + SessionUserName + "', 'Year':'" + document.getElementById("ContentPlaceHolder1_YearValue").value + "'}",
        dataType: "json",
        success: function (data) {
            if (data.d == "Success") {
                CommonErrormsg('success', 'Extraction Success.');
                GettingScheduleDataFromDB();
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

function GettingScheduleDataFromDB() {
    if (document.getElementById("ContentPlaceHolder1_YearValue").value != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/ScheduleDetails.aspx/GettingScheduleDataFromDB",
            data: "{'Year':'" + document.getElementById("ContentPlaceHolder1_YearValue").value + "'}",
            dataType: "json",
            success: function (data) {
                $("#ScheduledataAppend").html(data.d);

                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Kindly Add Year.');
    }
}

function onmouseoverFunc(Event, e) {
    Event.preventDefault();
    e.classList.add('zoomed');
}

function onmouseoutFunc(e) {
    e.classList.remove('zoomed');
}

function RemoveImageFromDBandFiles(ImageID, DeleteorNot) {
    if (DeleteorNot == "Delete") {
        var Confirmation = confirm("Do you wish to delete this record from the database?");
        if (!Confirmation) {
            return;
        }
    }

    $('#cover-spin').show(0);
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Formula1/ScheduleDetails.aspx/UpdateScheduleName",
        data: "{'ImageID':'" + ImageID + "', 'DeleteCode':'" + DeleteorNot + "'}",
        dataType: "json",
        success: function (data) {
            if (data.d == "Updated") {
                if (DeleteorNot == "Delete") {
                    GettingScheduleDataFromDB();
                }
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

function RaceTimingFunc(TableID) {
    if (TableID != "") {
        $('#cover-spin').show(0);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "../Formula1/ScheduleDetails.aspx/GettingRaceTimeDataFromDB",
            data: "{'TableID':'" + TableID + "'}",
            dataType: "json",
            success: function (response) {
                var data = JSON.parse(response.d);
                $("#RaceTimingDataAppend").html(data.RaceInfoStyle);
                if (data.DateTime != "") {
                    localStorage.setItem("PassedDateTime", data.DateTime);
                    CalculateDateTime();
                }
                setDateForClock();
                document.getElementById("cover-spin").style.display = "none";
            },
            error: function (result) {
                CommonErrormsg('error', 'Error in Ajax');
                document.getElementById("cover-spin").style.display = "none";
            }
        });
    }
    else {
        CommonErrormsg('error', 'Something Went Wrong.');
    }
}

//Clock JS
function setDateForClock() {

    const secondHand = document.querySelector('.second-hand');
    const minsHand = document.querySelector('.min-hand');
    const hourHand = document.querySelector('.hour-hand');

    const now = new Date();

    const seconds = now.getSeconds();
    const secondsDegrees = ((seconds / 60) * 360) + 90;
    secondHand.style.transform = `rotate(${secondsDegrees}deg)`;

    const mins = now.getMinutes();
    const minsDegrees = ((mins / 60) * 360) + ((seconds / 60) * 6) + 90;
    minsHand.style.transform = `rotate(${minsDegrees}deg)`;

    const hour = now.getHours();
    const hourDegrees = ((hour / 12) * 360) + ((mins / 60) * 30) + 90;
    hourHand.style.transform = `rotate(${hourDegrees}deg)`;

    setInterval(setDateForClock, 1000);
}

function Changetabstoactive(e) {
    if (e == "TrackTime") {
        document.getElementById("MyTime").classList.remove('active');
        document.getElementById(e).classList.add('active');
        document.getElementById("MyTimeSchedule").style.display = "none";
        document.getElementById("TrackTimeSchedule").style.display = "block";
    }
    if (e == "MyTime") {
        document.getElementById("TrackTime").classList.remove('active');
        document.getElementById(e).classList.add('active');
        document.getElementById("MyTimeSchedule").style.display = "block";
        document.getElementById("TrackTimeSchedule").style.display = "none";
    }
}

function CalculateDateTime() {

    // Example datetime string
    let passedDatetimeStr = localStorage.getItem("PassedDateTime");

    // Parse the passed datetime string
    let passedDatetime = parseCustomDate(passedDatetimeStr);

    // Get the current datetime
    let currentDatetime = new Date();

    // Condition to check if the current datetime is greater than the passed datetime
    if (currentDatetime > passedDatetime) {
        return;
    }

    // Calculate the difference in milliseconds
    const differenceInMilliseconds = passedDatetime - currentDatetime;

    // Calculate the difference in days, hours, and minutes
    let differenceInSeconds = Math.floor(differenceInMilliseconds / 1000);
    let differenceInMinutes = Math.floor(differenceInSeconds / 60);
    let differenceInHours = Math.floor(differenceInMinutes / 60);
    let differenceInDays = Math.floor(differenceInHours / 24);

    differenceInHours = differenceInHours % 24;
    differenceInMinutes = differenceInMinutes % 60;
   
    if (differenceInDays.toString().length == "1") {
        document.getElementById("CountDowndays").innerText = "0" + differenceInDays;
    }
    else { document.getElementById("CountDowndays").innerText = differenceInDays; }
    if (differenceInHours.toString().length == "1") {
        document.getElementById("CountDownhours").innerText = "0" + differenceInHours;
    }
    else { document.getElementById("CountDownhours").innerText = differenceInHours; }
    if (differenceInMinutes.toString().length == "1") {
        document.getElementById("CountDownminutes").innerText = "0" + differenceInMinutes;
    }
    else { document.getElementById("CountDownminutes").innerText = differenceInMinutes; }

    function parseCustomDate(dateStr) {
        // Split the date and time parts
        let [datePart, timePart, period] = dateStr.split(' ');

        // Split and rearrange the date part (dd/mm/yyyy to yyyy-mm-dd)
        let [day, month, year] = datePart.split('/').map(Number);
        //let [month, day, year] = datePart.split('/').map(Number); For Server Date Format

        // Create a new Date object
        let date = new Date(year, month - 1, day);

        // Extract the hours, minutes, and seconds from the time part
        let [hours, minutes, seconds] = timePart.split(':').map(Number);

        // Adjust hours for PM period
        if (period === 'PM' && hours !== 12) {
            hours += 12;
        } else if (period === 'AM' && hours === 12) {
            hours = 0;
        }

        // Set the hours, minutes, and seconds to the date object
        date.setHours(hours, minutes, seconds);

        return date;
    }

    setInterval(() => CalculateDateTime(), 60000);
}

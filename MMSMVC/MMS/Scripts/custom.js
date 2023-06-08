

$(document).ready(function () {


    var GetDate = new Date();
    GetDate.setDate(GetDate.getDate());

    $(".datepicker").datepicker({ startDate: GetDate, autoclose: true });

    /******************************** Begin Meeting Creation******************************/



    $(".Meeting_Department").change(function () {       
        var dept_name = $(".Meeting_Department option:selected").text();
        $("#Participants").text("");
        
        $.ajax({
            //url: '/Search/GetParticipants',
            url: rootPath + '/Search/GetParticipants',
            data: JSON.stringify({ 'deptName': dept_name }),
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                

                var listItems = [];

                $.each(data, function (item, val) {
                    listItems.push('<option value="' + val.Employee_Id + '">' + val.Employee_Name + '</option>');

                });

                $("#Participants").append(listItems.join(''));

            },
            error: function (data, textStatus, jqXHR) {
                CustomAlert("Please login and try again..");
                //alert(data.error);
            }
        });


    });


    $("#move_nxt").click(function () {

        //var selecteditem = $("#Participants :selected").text();
        var added_listitem = [];

        $("#Participants :selected").each(function (key, val) {

            added_listitem.push('<option value="' + $(this).val() + '">' + $(this).text() + '</option>');

            $(this).remove();
           
            // Meeting Chairperson add//
            $(".meeting_chairperson").
              append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");

            // Meeting Chairperson add//
            $(".temp_meeting_chairperson").
              append("<option value='" + $(this).text() + "'>" + $(this).text() + "</option>");

            // Responsible Person //
            $(".resp").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");

            $("#hidden_participants").val($(this).val() + "|");
        });

        $("#Added_Participants").append(added_listitem.join(''));


    });


    $("#move_prv").click(function () {

 

        var added_listitem = [];

        $("#Added_Participants :selected").each(function (item, val) {
            added_listitem.push('<option value="' + $(this).text() + '">' + $(this).text() + '</option>');
            $(this).remove();
          
            $(".meeting_chairperson").
              append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");

            // Meeting Chairperson add//
            $(".temp_meeting_chairperson").
              append("<option value='" + $(this).text() + "'>" + $(this).text() + "</option>");

            // Responsible Person //
            $(".resp").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
            $("#hidden_participants").val($(this).val() + "|");
        });

        $("#Participants").append(added_listitem.join(''));

    });    

    $(".Meeting_duration").timepicker({
        timeFormat: 'HH:mm',
        minTime: new Date(0, 0, 0, 1, 0, 0),
        maxTime: new Date(0, 0, 0, 12, 0, 0),
        startHour: 0,
        startTime: new Date(0, 0, 0, 1, 0, 0),
        interval: 30

    });



    $(".meeting_hours").timepicker({
        showPeriod: true,
        showLeadingZero: true,
        startTime: new Date(0, 0, 0, 9, 0, 0),
        interval: 15
    });


    /******************************End Meeting************************/

    /***************************Begin Task ******************************************/

    $("#DepartmentTypeID").change(function () {
        var dept_name = $("#DepartmentTypeID option:selected").text();
        $("#Participants").text("");

        $.ajax({

            //url: '/CreateTask/GetTaskParticipants',
            url: rootPath + 'CreateTask/GetTaskParticipants',
            data: JSON.stringify({ 'deptName': dept_name }),
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                var listItems = [];

                $.each(data, function (item, val) {

                    listItems.push('<option value="' + val.Employee_Id + '">' + val.Employee_Name + '</option>');

                });

                $("#Participants").append(listItems.join(''));

            },
            error: function (data, textStatus, jqXHR) {
                alert(data.error);
            }
        });


    });


    /***************************End Task ******************************************/




    $(".meeting-form").validate();

    $(".task-form").validate();

    NewIdGeneration();

});

var i = 1;

function NewIdGeneration() {
    $(document).ready(function () {

        $("#createTable td.newId").each(function (i) {

            $(this).text(i + 1);

        });

    });
}


function DepartmentAutoSelection(selectedDept, selectedPerson) {

    $.ajax({

        //url: '/Search/GetParticipants',
        url: rootPath + '/Search/GetParticipants',
        data: JSON.stringify({ 'deptName': selectedDept }),
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            var listItems = [];

            $.each(data, function (item, val) {

                listItems.push('<option value="' + val.Employee_Id + '">' + val.Employee_Name + '</option>');

            });

            $("#Participants").append(listItems.join(''));

        },
        error: function (data, textStatus, jqXHR) {
            alert(data.error);
        }
    });



    var added_listitem = [];
    added_listitem.push('<option value="' + selectedPerson + '">' + selectedPerson + '</option>');


    $("#Added_Participants,.meeting_chairperson").append(added_listitem.join(''));

    $(".meeting_chairperson option:last-child").attr("selected", "selected");

    //$(".Meeting_Department").change();



}
function GetBlinkImg() {
    return "<img src='/Content/Images/blink_square.gif'/>";

}
function ToJavaScriptDate(date) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(date);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}
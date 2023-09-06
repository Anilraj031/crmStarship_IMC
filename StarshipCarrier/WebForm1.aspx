<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="StarshipCarrier.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Starship Carrier Details</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
</head>
<body onload ="getFreightTest()">
    <form>
        <div style="margin-left:5%; margin-right:5%; height:550px">
            <table class="table table-bordered table-hover table-responsive" id="carrier_list1">
            <thead class='table-success'>        
                <tr><th>Select</th><th>Ship From</th><th>Carrier Name</th><th>SCAC Code</th><th>Total Charge</th><th>Days</th><th>Estimated Delivery</th></tr>
            </thead>
                <tbody>
                </tbody>
            </table>
            <button id="selectFreigt" type="button" class="btn btn-success btn-sm" >Select Freight</button>
            <button id="afterClicked"  class="btn btn-success btn-sm disabled" style="display:none;">
                <span class="spinner-border spinner-border-sm"></span>
                Please Wait..
            </button>
            <button id="close" type="button" class="btn btn-danger btn-sm" onclick="closeWindow();">Cancel</button>
        </div>
    </form>
    
</body>
</html>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script>
    function onlyOne(checkbox) {
        var checkboxes = document.getElementsByName('check')
        checkboxes.forEach((item) => {
            if (item !== checkbox) item.checked = false
        })
    }

    function closeWindow() {
        window.parent.postMessage({ freight: 'close' }, '*')
    }
    $(function () {
        //Assign Click event to Button.
        $("#selectFreigt").click(function () {
            var message = "Carrier   Scac     Country\n";
            //Loop through all checked CheckBoxes in GridView.
            var freight;
            var days;
            var shipfrom;
            var shipvalue;
            $("#carrier_list1 input[type=checkbox]:checked").each(function () {
                var row = $(this).closest("tr")[0];
                message += row.cells[1].innerHTML;
                message += "   " + row.cells[2].innerHTML;
                message += "   " + row.cells[3].innerHTML;
                message += "\n";
                freight = row.cells[4].innerHTML;
                days = row.cells[6].innerHTML;
                shipfrom = row.cells[1].innerHTML;
            });
            if (freight == undefined) {
                alert("Please select any one freight!")
            }
            else {
                if (shipfrom == 'Hebron') {
                    shipvalue = 5;
                }
                if (shipfrom == 'Houston') {
                    shipvalue = 7;
                }
                if (shipfrom == 'MOSINEE') {
                    shipvalue = 13;
                }
                if (shipfrom == 'Live Oak') {
                    shipvalue = 100000000;
                }
                if (shipfrom == 'Sturgis') {
                    shipvalue = 9;
                }
                if (shipfrom == 'OGDEN') {
                    shipvalue = 100000001;
                }
                if (shipfrom == 'Pontoon') {
                    shipvalue = 14;
                }
                //alert(freight);
                document.getElementById("selectFreigt").style.display = "none";
                document.getElementById("afterClicked").style.display = "inline";
                //alert(shipvalue);
                window.parent.postMessage({ freight: freight, days: days, shiplocation: shipvalue }, '*');
            }
            //window.parent.postMessage(freight, '*');

            //alert(freight);
            return false;
        });
    });

    function getFreightTest() {
        //var checkUrl = window.location.href;
        // alert(checkUrl);
        const locationCodes = ["2", "7", "10", "11", "12", "13","0","23","5"];
        locationCodes.forEach(getFreightTest1);
    }

    function getFreightTest1(value) {
        var string = window.location.href;
        //var string = "https://starshipcarrier.azurewebsites.net/getCarrier/Quo205263/7/0/0/ST%20Louis/MO/63110/3750";
        let result = string.slice(53,);
        var result2 = result.split("/");
        var quote = result2[0];
        var location = value;
        var city = unescape(result2[4]);
        var state = result2[5];
        var zip = result2[6];
        var weight = result2[7];
        var parameter = { "Location": location, "Location_Id": 2, "SOPNUMBE": quote, "ADDRESS1": 0, "ADDRESS2": 0, "CITY": city, "STATE": state, "ZIPCODE": zip, "COUNTRY": "USA", "TotalWeight": weight };
        var pageUrl = '<%= ResolveUrl("~/WebForm1.aspx/CalculateFreight")%>';
        $.ajax({
            type: "POST",
            url: pageUrl,
            data: JSON.stringify(parameter),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: fnsuccesscallback,
            error: fnerrorcallback
        });
        function fnsuccesscallback(data) {
            //alert(data.d);
            var charge = data.d[3];
            var percentage = (15 / 100) * charge;
            var total = (Number(charge) + Number(percentage)).toFixed(2);
            $(carrier_list1).find('tbody').append("<tr style='background-color:azure'><td><input type = 'checkbox' name='check' onclick='onlyOne(this)'></td><td>" + data.d[0] + "</td><td>" + data.d[1] + "</td><td>" + data.d[2] + "</td><td>" + total + "</td><td>" + data.d[4] + "</td><td>" + data.d[5] + "</td></tr>");
        }
        function fnerrorcallback(result) {
            alert(result.statusText);
        }


    }
</script>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetQuote.aspx.cs" Inherits="StarshipCarrier.GetQuote" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Starship Carrier Details</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <style>
        .getdata input{
            margin-left:20px;
            border-radius:3px;
            border:1px solid gray;
            border-color:gray !important;
        }
    </style>
</head>
<body>
    <form id="fm" runat="server" class="mt-5">
        <div id="form1" class="container getdata">
            <label>Weight </label><input id="weight" type="text" placeholder="Enter total weight" />
            <label>Zipcode </label><input id="zipcode" type="text" placeholder="Enter location zipcode" />
            <label>Ship Date </label><input id="shipdate" type="date" placeholder="Enter shipping date" />
            <a id="btnfirst" class="btn btn-sm btn-success" onclick="getdata();">Get Quote</a>
        </div>
    </form>
      <div id="loaddata1" class="container mt-2 loaddata" style="margin-left:5%; margin-right:5%;display:none;">
            <table class="table table-bordered table-hover table-responsive" id="carrier_list1">
            <thead class='table-success'>        
                <tr><th>Ship From</th><th>Carrier Name</th><th>SCAC Code</th><th>Total Charge</th><th>Days</th><th>Estimated Delivery</th></tr>
            </thead>
                <tbody>
                </tbody>
            </table>
        </div>
</body>
</html>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script>
    getdate();
    document.getElementById("btnsecond").style.display = "none";

    function getdata() {
        //var x = document.getElementById("loaddata1");
        //x.style.display = "block";
        document.getElementById("loaddata1").style.display = "block";
        var w = document.getElementById("weight").value;
        var z = document.getElementById("zipcode").value;
        if (w == '' || z == '') {
            alert("Please Confirm weight or zipcode");
        }
        else {
            getFreightTest();
        }
    }

    function getFreightTest() {
        //var checkUrl = window.location.href;
        // alert(checkUrl);
        const locationCodes = ["2", "7", "10", "11", "12", "13","0","23","5"];
        locationCodes.forEach(getFreightTest1);
    }

    function getFreightTest1(value) {
        $("#carrier_list1 tr>td").remove();
        var date = document.getElementById("shipdate").value;
        var zip = document.getElementById("zipcode").value;
        var weight = document.getElementById("weight").value;
        var location = value;
        var parameter = { "Location1": location, "Shipdate1": date, "ZIPCODE1": zip, "TotalWeight1": weight };
        var pageUrl = '<%= ResolveUrl("~/GetQuote.aspx/CalculateFreight")%>';
        $.ajax({
            type: 'POST',
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
            $(carrier_list1).find('tbody').append("<tr style='background-color:azure'><td>" + data.d[0] + "</td><td>" + data.d[1] + "</td><td>" + data.d[2] + "</td><td>" + total + "</td><td>" + data.d[4] + "</td><td>" + data.d[5] + "</td></tr>");
        }
        function fnerrorcallback(result) {
            alert(result.responseText);
        }
    }

    function getdate() {
        var today = new Date();
        document.getElementById("shipdate").value = today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2);
    }
</script>
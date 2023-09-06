using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using VTechnologies.StarShip.WebServices.Model;

namespace StarshipCarrier
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        StringBuilder html = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*https://localhost:44399/Defalut.aspx?name=anil */
            Route myRoute = RouteData.Route as Route;
            //if (myRoute != null && myRoute.Url == "getCarrier/{Quotenumber}/{Location}/{Address1}/{Address2}/{City}/{State}/{Zipcode}/{Weight}")
            //{
            //    string Quotenumber = RouteData.Values["Quotenumber"].ToString();
            //    string Location = RouteData.Values["Location"].ToString();
            //    string Address1 = RouteData.Values["Address1"].ToString();

            //    string Address2 = RouteData.Values["Address2"].ToString();
            //    string City = RouteData.Values["City"].ToString();
            //    string State = RouteData.Values["State"].ToString();
            //    string Zipcode = RouteData.Values["Zipcode"].ToString();
            //    string Weight = RouteData.Values["Weight"].ToString();

            //    //html.Append("<thead class='table-success'>");
            //    html.Append("<tr><th>Select</th><th>Ship From</th><th>Carrier Name</th><th>SCAC Code</th><th>Total Charge</th><th>Days</th><th>Estimated Delivery</th>");
            //    html.Append("</thead><tbody>");

            //    //RateShop(Int32.Parse(Location), 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));
            //    RateShop(2, 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));
            //    RateShop(7, 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));
            //    RateShop(10, 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));
            //    RateShop(11, 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));
            //    RateShop(12, 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));
            //    RateShop(13, 2, Quotenumber, Address1, Address2, City, State, Zipcode, "USA", Double.Parse(Weight));

            //    html.Append("</tbody>");
            //    html.Append("</table>");
            //    carrier_list.Controls.Add(new Literal { Text = html.ToString() });
            //}
            //else
            //{
            //    RateShop(2, 2, "ORD1516", "Will Call", "833 Roosevelt Rd", "Taylorville", "IL", "62568", "USA", 500);
            //}
            
        }

        public void RateShop(int Location, int Location_Id, string SOPNUMBE, String ADDRESS1,
                            String ADDRESS2, String CITY, String STATE, String ZIPCODE,
                                String COUNTRY, Double TotalWeight)
        {
            /* Instantiate Carrier Transactions proxy class*/
            StarShipService.CarrierTransactionsClient starShipCarrierClient = new StarShipService.CarrierTransactionsClient();
            StarShipService.DataTransactionsClient starShipDataClient = new StarShipService.DataTransactionsClient();
            /*' Create an Identity object*/
            VTechnologies.StarShip.WebServices.Model.Identity identity = new VTechnologies.StarShip.WebServices.Model.Identity
            {
                ApplicationName = "WebApplication1",
                ApplicationVersion = "1.0",
                DeveloperKey = "INTER-3139"
            };


            /*' Create an Authorization object*/
            /*VTechnologies.StarShip.WebServices.Model.*/
            VTechnologies.StarShip.WebServices.Model.ClientAuthentication clientAuthorization = new VTechnologies.StarShip.WebServices.Model.ClientAuthentication
            {
                UserID = "admin",
                Password = "admin",
                LocationID = 2
            };

            /*' Load a few helper objects from StarShip*/
            /*VTechnologies.StarShip.WebServices.Model.GetCompanyInfoRequest companyRequest = new VTechnologies.StarShip.WebServices.Model.GetCompanyInfoRequest();

            /*GetCompanyInfoResponse getCompanyInfoResponse = starShipDataClient.GetCompanyInfo(new GetCompanyInfoRequest
            {
                Authentication = clientAuthorization,
                Identity = identity
            }); */

            VTechnologies.StarShip.WebServices.Model.GetCompanyInfoResponse getCompanyInfoResponse = starShipDataClient.GetCompanyInfo(new VTechnologies.StarShip.WebServices.Model.GetCompanyInfoRequest
            {
                Authentication = clientAuthorization,
                Identity = identity
            });

            /*' Check for valid GetCompanyInfoResponse response*/
            if (getCompanyInfoResponse.ResponseInfo.ResultCode < VTechnologies.StarShip.WebServices.Model.ResultCode.Success)
            {

            }
            /*' Check for at least one Company Info address
            if(getCompanyInfoResponse.CompanyInfo.PartyList.Count <= 0)
            {}*/

            /*Dim companyInfoParty As VTechnologies.StarShip.WebServices.Model.Party = getCompanyInfoResponse.CompanyInfo.PartyList(Location)*/
            VTechnologies.StarShip.WebServices.Model.Party companyInfoParty = new VTechnologies.StarShip.WebServices.Model.Party();
            companyInfoParty = getCompanyInfoResponse.CompanyInfo.PartyList[Location];

            /*' Create a package to ship and set properties*/
            VTechnologies.StarShip.WebServices.Model.Pack package = new VTechnologies.StarShip.WebServices.Model.Pack();

            /*'04/08/2019 -Bibash- Following Line of adding Actual weight was suggested by VTechnologies to counter the API issues.Both Actual Weight and Tar Weight are to be provided */
            package.ActualWeight = new VTechnologies.StarShip.WebServices.Model.Weight();
            package.ActualWeight.Value = (decimal)TotalWeight;
            /* 04/02/2019 - SRD - Following lines have been added and the lines above have been commended out due to possible bug */
            package.TareWeight = new VTechnologies.StarShip.WebServices.Model.Weight();
            package.TareWeight.Value = (decimal)TotalWeight;
            package.Name = "Shop Rate Pack";
            package.NMFCClass = NMFCClassEnum.e60;
            package.PackagingType = VTechnologies.StarShip.WebServices.Model.PackagingTypeEnum.Pallet;
            /*package.PackQty = 1;

            /*' Create a sender object for the package origin
            ' (Use IDataTransactions.GetCompanyInfo transaction for a list of sender objects from StarShip)
            ' AccountInfoList contains a list of setup accounts specific to the CompanyInfo and is recommended*/
            VTechnologies.StarShip.WebServices.Model.Sender sender = new VTechnologies.StarShip.WebServices.Model.Sender();
            sender.AccountInfoList = companyInfoParty.AccountInfoList;
            sender.Address = companyInfoParty.Address;

            /*' Create a recipient object for the package destination*/
            VTechnologies.StarShip.WebServices.Model.Recipient recipient = new VTechnologies.StarShip.WebServices.Model.Recipient();
            recipient.Address = new VTechnologies.StarShip.WebServices.Model.Address();
            recipient.Address.Address1 = ADDRESS1;
            recipient.Address.Address2 = ADDRESS2;
            recipient.Address.City = CITY;
            recipient.Address.StateProvinceCode = STATE;
            recipient.Address.PostalCode = ZIPCODE;
            recipient.Address.CountryCode = "US";
            recipient.Address.LocationType = (LocationType)QuoteLocationType.Residence;

            /*Create a shipment */
            VTechnologies.StarShip.WebServices.Model.APIShipment shipment = new VTechnologies.StarShip.WebServices.Model.APIShipment();
            shipment.Sender = sender;
            shipment.Recipient = recipient;

            /*shipment.Packs[0] = package;*/
            shipment.Packs = new VTechnologies.StarShip.WebServices.Model.Pack[] { package };
            /*shipment.Packs[0] = package; */
            shipment.Packs[0].PackQty = 1;
            /*shipment.ShipDate = DateAdd(DateInterval.Day, 20, DateTime.Now).[Date]; */
            shipment.ShipDate = DateTime.Today.AddDays(20);
            shipment.ShipCarrier = new VTechnologies.StarShip.WebServices.Model.ShipCarrier();
            shipment.ShipCarrier.CarrierType = VTechnologies.StarShip.WebServices.Model.CarrierType.Freight;

            VTechnologies.StarShip.WebServices.Model.FSIDocType docType = new VTechnologies.StarShip.WebServices.Model.FSIDocType();
            docType = FSIDocType.Shipment;
            /*' Create RateParams object */
            VTechnologies.StarShip.WebServices.Model.RateParams rateParams = new VTechnologies.StarShip.WebServices.Model.RateParams();
            rateParams.DeliveryBy = DateTime.Now.AddDays(30);
            /* Must be delivered within 30 days of today
            Create request object */
            VTechnologies.StarShip.WebServices.Model.RateShopRequest request = new VTechnologies.StarShip.WebServices.Model.RateShopRequest();
            request.Authentication = clientAuthorization;
            request.Identity = identity;
            request.Shipment = shipment;
            request.RateParams = rateParams;

            /*' Execute RateShop transaction request to server and get back a response object */
            RateShopResponse response = starShipCarrierClient.RateShop(request);

            Decimal TotalCharges = 0;
            String strCarrier = "";
            int days =0;
            string scac = "";
            string estimated_date = "";
            string rateMethod = "";
            string shipFrom = companyInfoParty.AddressID.ToString();

            if (response.Shipment.RateInfo.RateQuotes != null)
            {
                /* Iterate and display all rate quotes returned */
                foreach (VTechnologies.StarShip.WebServices.Model.RateQuote rateQuote in response.Shipment.RateInfo.RateQuotes)
                {
                    if(rateQuote.CarrierServiceName == "ABF Freight LTL")
                        {

                    }
                    else{
                        if( TotalCharges > rateQuote.ShipmentRate.CustomCharges.Total || TotalCharges == 0) 
                            {
                            TotalCharges = rateQuote.ShipmentRate.CustomCharges.Total;
                            strCarrier = rateQuote.CarrierServiceName;
                            days = rateQuote.Days;
                            scac = rateQuote.CarrierSCAC;
                            estimated_date = rateQuote.EstimatedDelivery.ToShortDateString();
                            rateMethod = rateQuote.RateMethod.ToString();
                        }
                    }

                }
                html.Append("<tr style='background-color:azure'>");
                html.Append("<td><input type = 'checkbox' name='check' onclick='onlyOne(this)'></td>");
                html.Append("<td>" + shipFrom + "</td>");
                html.Append("<td>" + strCarrier + "</td>");
                html.Append("<td>" + scac + "</td>");
                html.Append("<td>" + TotalCharges.ToString() + "</td>");
                html.Append("<td>" + days + "</td>");
                html.Append("<td>" + estimated_date + "</td>");
                html.Append("</tr>");

            }
            else
            {
                html.Append("<tr style = 'background-color:antiquewhite'>");
                html.Append("<td><input type = 'checkbox' '></td>");
                html.Append("<td>" + shipFrom + "</td>");
                html.Append("<td>Not found</td>");
                html.Append("<td>N/A</td>");
                html.Append("<td>N/A</td>");
                html.Append("<td>N/A</td>");
                html.Append("<td>N/A</td>");
                html.Append("</tr>");
            }


        }


        [WebMethod]
        public static string[] CalculateFreight(int Location, int Location_Id, string SOPNUMBE, String ADDRESS1,
                            String ADDRESS2, String CITY, String STATE, String ZIPCODE,
                                String COUNTRY, Double TotalWeight)
        {
            string[] freightDetails = new string[6];

            /* Instantiate Carrier Transactions proxy class*/
            StarShipService.CarrierTransactionsClient starShipCarrierClient = new StarShipService.CarrierTransactionsClient();
            StarShipService.DataTransactionsClient starShipDataClient = new StarShipService.DataTransactionsClient();
            /*' Create an Identity object*/
            VTechnologies.StarShip.WebServices.Model.Identity identity = new VTechnologies.StarShip.WebServices.Model.Identity
            {
                ApplicationName = "WebApplication1",
                ApplicationVersion = "1.0",
                DeveloperKey = "INTER-3139"
            };


            /*' Create an Authorization object*/
            /*VTechnologies.StarShip.WebServices.Model.*/
            VTechnologies.StarShip.WebServices.Model.ClientAuthentication clientAuthorization;
             if (Location == 23)
            {
                clientAuthorization = new VTechnologies.StarShip.WebServices.Model.ClientAuthentication
                {
                    UserID = "admin",
                    Password = "admin",
                    LocationID = 23 //database starship, table location, code ,23 for sanford
                };
            }
            else if (Location == 5)
            {
                clientAuthorization = new VTechnologies.StarShip.WebServices.Model.ClientAuthentication
                {
                    UserID = "admin",
                    Password = "admin",
                    LocationID = 5 //database starship, table location, code ,23 for sanford
                };
            }
            else
            {
                clientAuthorization = new VTechnologies.StarShip.WebServices.Model.ClientAuthentication
                {
                    UserID = "admin",
                    Password = "admin",
                    LocationID = 22 //database starship, table location, code ,22 for pontoon
                };
            }

            /*' Load a few helper objects from StarShip*/
            /*VTechnologies.StarShip.WebServices.Model.GetCompanyInfoRequest companyRequest = new VTechnologies.StarShip.WebServices.Model.GetCompanyInfoRequest();

            /*GetCompanyInfoResponse getCompanyInfoResponse = starShipDataClient.GetCompanyInfo(new GetCompanyInfoRequest
            {
                Authentication = clientAuthorization,
                Identity = identity
            }); */

            VTechnologies.StarShip.WebServices.Model.GetCompanyInfoResponse getCompanyInfoResponse = starShipDataClient.GetCompanyInfo(new VTechnologies.StarShip.WebServices.Model.GetCompanyInfoRequest
            {
                Authentication = clientAuthorization,
                Identity = identity
            });

            /*' Check for valid GetCompanyInfoResponse response*/
            if (getCompanyInfoResponse.ResponseInfo.ResultCode < VTechnologies.StarShip.WebServices.Model.ResultCode.Success)
            {

            }
            /*' Check for at least one Company Info address
            if(getCompanyInfoResponse.CompanyInfo.PartyList.Count <= 0)
            {}*/

            /*Dim companyInfoParty As VTechnologies.StarShip.WebServices.Model.Party = getCompanyInfoResponse.CompanyInfo.PartyList(Location)*/
            VTechnologies.StarShip.WebServices.Model.Party companyInfoParty = new VTechnologies.StarShip.WebServices.Model.Party();
            if (Location == 23)
            {
                companyInfoParty = getCompanyInfoResponse.CompanyInfo.PartyList[0];
            }
            else if (Location == 5)
            {
                companyInfoParty = getCompanyInfoResponse.CompanyInfo.PartyList[0];
            }
            else
            {
                companyInfoParty = getCompanyInfoResponse.CompanyInfo.PartyList[Location];
            }

            /*' Create a package to ship and set properties*/
            VTechnologies.StarShip.WebServices.Model.Pack package = new VTechnologies.StarShip.WebServices.Model.Pack();

            /*'04/08/2019 -Bibash- Following Line of adding Actual weight was suggested by VTechnologies to counter the API issues.Both Actual Weight and Tar Weight are to be provided */
            package.ActualWeight = new VTechnologies.StarShip.WebServices.Model.Weight();
            package.ActualWeight.Value = (decimal)TotalWeight;
            /* 04/02/2019 - SRD - Following lines have been added and the lines above have been commended out due to possible bug */
            package.TareWeight = new VTechnologies.StarShip.WebServices.Model.Weight();
            package.TareWeight.Value = (decimal)TotalWeight;
            package.Name = "Shop Rate Pack";
            package.NMFCClass = NMFCClassEnum.e60;
            package.PackagingType = VTechnologies.StarShip.WebServices.Model.PackagingTypeEnum.Pallet;
            /*package.PackQty = 1;

            /*' Create a sender object for the package origin
            ' (Use IDataTransactions.GetCompanyInfo transaction for a list of sender objects from StarShip)
            ' AccountInfoList contains a list of setup accounts specific to the CompanyInfo and is recommended*/
            VTechnologies.StarShip.WebServices.Model.Sender sender = new VTechnologies.StarShip.WebServices.Model.Sender();
            sender.AccountInfoList = companyInfoParty.AccountInfoList;
            sender.Address = companyInfoParty.Address;

            /*' Create a recipient object for the package destination*/
            VTechnologies.StarShip.WebServices.Model.Recipient recipient = new VTechnologies.StarShip.WebServices.Model.Recipient();
            recipient.Address = new VTechnologies.StarShip.WebServices.Model.Address();
            recipient.Address.Address1 = ADDRESS1;
            recipient.Address.Address2 = ADDRESS2;
            recipient.Address.City = CITY;
            recipient.Address.StateProvinceCode = "";//STATE;
            recipient.Address.PostalCode = ZIPCODE;
            recipient.Address.CountryCode = "US";
            //recipient.Address.LocationType = (LocationType)QuoteLocationType.Unknown;
            recipient.Address.LocationType = (LocationType)QuoteLocationType.Residence;

            /*Create a shipment */
            VTechnologies.StarShip.WebServices.Model.APIShipment shipment = new VTechnologies.StarShip.WebServices.Model.APIShipment();
            shipment.Sender = sender;
            shipment.Recipient = recipient;

            /*shipment.Packs[0] = package;*/
            shipment.Packs = new VTechnologies.StarShip.WebServices.Model.Pack[] { package };
            /*shipment.Packs[0] = package; */
            shipment.Packs[0].PackQty = 1;
            /*shipment.ShipDate = DateAdd(DateInterval.Day, 20, DateTime.Now).[Date]; */
            shipment.ShipDate = DateTime.Today.AddDays(20);
            shipment.ShipCarrier = new VTechnologies.StarShip.WebServices.Model.ShipCarrier();
            shipment.ShipCarrier.CarrierType = VTechnologies.StarShip.WebServices.Model.CarrierType.Freight;

            VTechnologies.StarShip.WebServices.Model.FSIDocType docType = new VTechnologies.StarShip.WebServices.Model.FSIDocType();
            docType = FSIDocType.Shipment;
            /*' Create RateParams object */
            VTechnologies.StarShip.WebServices.Model.RateParams rateParams = new VTechnologies.StarShip.WebServices.Model.RateParams();
            rateParams.DeliveryBy = DateTime.Now.AddDays(30);
            /* Must be delivered within 30 days of today
            Create request object */
            VTechnologies.StarShip.WebServices.Model.RateShopRequest request = new VTechnologies.StarShip.WebServices.Model.RateShopRequest();
            request.Authentication = clientAuthorization;
            request.Identity = identity;
            request.Shipment = shipment;
            request.RateParams = rateParams;

            /*' Execute RateShop transaction request to server and get back a response object */
            RateShopResponse response = starShipCarrierClient.RateShop(request);

            Decimal TotalCharges = 0;
            String strCarrier = "";
            int days = 0;
            string scac = "";
            string estimated_date = "";
            string rateMethod = "";
            string shipFrom = "";
            //for pontoon location which is not in party list, can be obtained from location ID i.e 22 of location table
            if (Location == 0)
            {
                shipFrom = "Pontoon";
            }
            else if (Location == 23)
            {
                shipFrom = "Sanford";
            }
            else if (Location == 5)
            {
                shipFrom = "Hebron";
            }
            else
            {
                shipFrom = companyInfoParty.AddressID.ToString();
            }

            if (response.Shipment.RateInfo.RateQuotes != null)
            {
                /* Iterate and display all rate quotes returned */
                foreach (VTechnologies.StarShip.WebServices.Model.RateQuote rateQuote in response.Shipment.RateInfo.RateQuotes)
                {
                    if (rateQuote.CarrierServiceName == "ABF Freight LTL")
                    {

                    }
                    else
                    {
                        if (TotalCharges > rateQuote.ShipmentRate.CustomCharges.Total || TotalCharges == 0)
                        {
                            TotalCharges = rateQuote.ShipmentRate.CustomCharges.Total;
                            strCarrier = rateQuote.CarrierServiceName;
                            days = rateQuote.Days;
                            scac = rateQuote.CarrierSCAC;
                            estimated_date = rateQuote.EstimatedDelivery.ToShortDateString();
                            rateMethod = rateQuote.RateMethod.ToString();
                        }
                    }

                }

                freightDetails[0] = shipFrom;
                freightDetails[1] = strCarrier;
                freightDetails[2] = scac;
                freightDetails[3] = TotalCharges.ToString();
                freightDetails[4] = days.ToString();
                freightDetails[5] = estimated_date;
            }
            else
            {
                freightDetails[0] = shipFrom;
                freightDetails[1] = "No carrier found";
                freightDetails[2] = "na";
                freightDetails[3] = "na";
                freightDetails[4] = "na";
                freightDetails[5] = "na";
            }
            return freightDetails;
        }
    }
}
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

public partial class _Default : System.Web.UI.Page
{
    private readonly string NewLine = Environment.NewLine;

    protected void Page_Load(object sender, EventArgs e)
    {
        Button1.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(Button1, null) + ";");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string stockSymbol = "";
        string stockName = "";
        string stockPrice = "";
        string stockChange = "";
        string stockDate = "";
        string stockTime = "";
        string stockOpen = "";
        string stockHigh = "";
        string stockLow = "";
        string stockPreviousClose = "";
        string stockPercentageChange = "";
        string stockAnnRange = "";
        string result = "";
        
        //call and consume webservice
        ServiceStocks.StockQuoteSoapClient astockquote = new ServiceStocks.StockQuoteSoapClient("StockQuoteSoap");
        result = astockquote.GetQuote(TextBox1.Text);

        // Load XML and Parse String for useful data

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(result);
        XmlNodeList xnList = xml.SelectNodes("/StockQuotes/Stock");
        foreach (XmlNode xn in xnList)
        {
            stockSymbol = xn["Symbol"].InnerText;
            stockName = xn["Name"].InnerText;
            stockPrice = xn["Last"].InnerText;
            stockChange = xn["Change"].InnerText;
            stockDate = xn["Date"].InnerText;
            stockTime = xn["Time"].InnerText;
            stockOpen = xn["Open"].InnerText;
            stockHigh = xn["High"].InnerText;
            stockLow = xn["Low"].InnerText;
            stockPreviousClose = xn["PreviousClose"].InnerText;
            stockPercentageChange = xn["PercentageChange"].InnerText;
            stockAnnRange = xn["AnnRange"].InnerText;
        }

        TextBox2.Text = "Stock Name: " + stockName + NewLine + "Stock Symbol: " + stockSymbol + NewLine  + "Stock Date :" + stockDate + NewLine +  "Stock Time: " + stockTime+NewLine+ "Stock Price: " + stockPrice + NewLine+
                        "Change:" + stockChange + NewLine + "Open Price :" + stockOpen + NewLine + "Stock High Price :" + stockHigh + NewLine + "Stock Low: " + stockLow + NewLine + "Previous Close Price :" + stockPreviousClose + NewLine+ "Percentage Change :"+ stockPercentageChange +NewLine + "Stock Annual Range :"+ stockAnnRange ;
        AnalyzeStock(Convert.ToDouble(stockPrice));
    }

    private void AnalyzeStock(double stockPrice)
    {
        var regex = new Regex("(?:Stock Annual Range)[^\\d]+(?<annual_low>\\d+\\.\\d+)[\\-\\s]{3}(?<annual_high>\\d+\\.\\d+)",
        RegexOptions.CultureInvariant
        | RegexOptions.Compiled
        );
        var results = regex.Matches(TextBox2.Text);
        var annualLow = Convert.ToDouble(results[0].Groups["annual_low"].Value);
        var annualHigh = Convert.ToDouble(results[0].Groups["annual_high"].Value);
        var annualPercentage = ((stockPrice - annualLow)/(annualHigh - annualLow));

        string Recommendation = string.Empty;

        //var PercentChangedOver10Percent = annualLow * 2;
        ////var PercentChangedSinceAnnualHigh = stockPrice * 1.10;

        //if (PercentChangedOver10Percent >= stockPrice)
        //    Recommendation = "The stock has increased 10% since annual low price of " + annualLow;

        // Launch alert if any condidtion is met
        if (annualPercentage > .8)
        {
            if (annualPercentage > .95)
                Recommendation = "Recommendation: Sell a lot!!!";
            else Recommendation = "Recommendation: Sell a few.";
        }
        else if (annualPercentage < .2)
        {
            if (annualPercentage < .05)
                Recommendation = "Recommendation: Buy Everything You Can!!!";
            else if (annualPercentage < .1)
            {
                Recommendation = "Recommendation: Buy some.";
            }
            else Recommendation = "Recommendation: Buy a little.";
        }

        if (!string.IsNullOrEmpty(Recommendation))
        {
            Advice.Text = Recommendation;
            //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + Recommendation + "');", true);
        }
    }
}
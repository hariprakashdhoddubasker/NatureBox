using System;
using System.Collections.Generic;
using System.Text;

namespace NatureBox.Service
{
    internal static class Constants
    {
        internal static string HtmlStartTag => "<!DOCTYPE html><html>";
        internal static string HtmlEndTag => "</html>";
        internal static string HeadStartTag => "<head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">";
        internal static string HeadEndTag => "</head>";

        internal static string BodyStartTag => "<body>";
        internal static string BodyEndTag => "</body>";
        internal static string H1Tag => "<h1 id=\"heading\">{0}</h1>";
        internal static string H2Tag => "<h2>{0}</h2>";
        internal static string Break => "<br>";
        internal static string SearchBoxTag => "<input type=\"text\" id=\"txtId\" onkeyup=\"myFunction()\" placeholder=\"Search for Error Messages..\" title=\"Type in a Error codes\" style=\"margin-bottom:5px;\">";

        internal static string TableStartTag => "<table id=\"myTable\" {0}>";
        internal static string TableHeaderTag => "<th bgcolor=\"#78BE1F\" style=\"width:{0}%; color:white;\">{1}</th>";
        internal static string TableRowStartTag => "<tr{0}>";
        internal static string TableRowEndTag => "</tr>";
        internal static string TableDataTag => "<td>{0}</td>";
        internal static string BGTableDataTag => "<td style=\"font-weight:bold; font-size: 24px; color:white;\" bgcolor=\"#78BE1F\" colspan=\"2\">{0}</td>";
        internal static string TableAlternativeDataTag => "<td bgcolor=\"#E4FFCF\">{0}</td>";
        internal static string TableEndTag => "</table>";

        internal static string StyleTag => "<style>* {box-sizing: border-box;} " +
                                            "#myTable {border-collapse: collapse; border: 1px solid #ddd;font-size: 18px;} " +
                                            "#myTable th, #myTable td {text-align: left;padding: 5px;border: 1px solid #ddd;} " +
                                            "#myTable tr {border-bottom: 1px solid #ddd;} " +
                                            "#myTable tr.header, #myTable tr:hover {background-color: #f1f1f1;}" + "#background{position:absolute;z-index:0;background:white;display:block;min-height:50%;min-width:50%;color:yellow;}"
                            + "#content{position:absolute;z-index:1;}"
                            + "#bg-text{color:#def9bb;font-size:150px;transform:rotate(350deg);-webkit-transform:rotate(350deg);}"
            + "#heading {margin: 1em 0 0.5em 0;font-weight: normal;position: relative;text-shadow: 0 -1px rgba(0,0,0,0.6);font-size: 28px;line-height: 40px;background: #78BE1F;border: 1px solid #fff;padding: 5px 15px;color: white;border-radius: 0 10px 0 10px;box-shadow: inset 0 0 5px rgba(53,86,129, 0.5);font-family: 'Muli', sans-serif;}"
                                            + " </style>";
    }
}

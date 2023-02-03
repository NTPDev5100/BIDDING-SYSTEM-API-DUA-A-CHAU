using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class PrintStampTemplate
    {
        public static string StampTemplate()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div>");
            html.Append("<span>{content-left}</span>");
            html.Append("</div>");

            html.Append("<div>");
            html.Append("<div style=\"display:flex\">");
            html.Append(" <div style=\"margin-right: 2px\">");
            html.Append("<img src=\"{qrcode}\" width=\"50\" height=\"50\"/>");
            html.Append("</div>");
            html.Append("<div><span>{name}</span> <br/> <span>6.33-6.37.4.01</span> <br/> <span>{code}</span> <br/> </div>");
            html.Append("</div>");
            html.Append("<div> <span>Price: </span> <span>{price}</span> </div>");
            html.Append("</div>");
            return html.ToString();
        }
    }
}

using System;
using System.IO;
using System.Web.UI.WebControls;
using DiffMatchPatch;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.Services.DiffMatchPatch
{
	public class DiffMatchPatchService
	{
		public static string Execute(string text1 ,string text2)
		{
			var dmp = new diff_match_patch();
			var diff = dmp.diff_main(text1, text2);
			dmp.diff_cleanupSemantic(diff);

			var str = dmp.diff_prettyHtml(diff);
			str = "<html><head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head><body><font size=\"4\">"
						+ str + "  </font></body></html>";

			str = str.Replace("&para;", "");

			//str = str.Replace("<del style=\"background:#ffe6e6;\">", "<button style=\"background-color:red;\">");
			//str = str.Replace("</del>", "</button>");

			//str = str.Replace("<ins style=\"background:#e6ffe6;\">", "<button style=\"background-color:lightblue;\">");
			//str = str.Replace("</ins>", "</button>");

			//str = str.Replace("<del style=\"background:#ffe6e6;\">", "<label style=\"background-color:pink;\">&nbsp;<del>");
			//str = str.Replace("</del>", "</del>&nbsp;</label>");
			//str = str.Replace("<ins style=\"background:#e6ffe6;\">", "<label style=\"background-color:lightblue;\">&nbsp;");
			//str = str.Replace("</ins>", "&nbsp;</label>");

			//str = str.Replace("background:#ffe6e6;", "background-color:pink;");
			//str = str.Replace("background:#e6ffe6;", "background-color:lightblue;");

			//<ins style="background:#e6ffe6;">

			return str;
		}
	}
}

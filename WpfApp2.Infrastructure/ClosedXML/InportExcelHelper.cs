using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Data;
using System.IO;

namespace WpfApp2.Infrastructure.ClosedXML
{
	public static class InportExcelHelper
	{
		public static DataTable ExtractData()
		{
			var ofDialog = new OpenFileDialog
			{
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				Title = "インポートするExcelファイルの選択",
				Filter = "Excelファイル | *.xlsx"
			};

			string filename;
			if (ofDialog.ShowDialog() == true)
			{
				filename = ofDialog.FileName;
			}
			else
			{
				return null;
			}

			using (var wb = new XLWorkbook(filename, XLEventTracking.Disabled))
			{
				IXLWorksheet ws = wb.Worksheet(1);
				DataTable table = ws.RangeUsed().AsTable().AsNativeDataTable();
				table.TableName = Path.GetFileNameWithoutExtension(filename);
				return table;
			}
		}
	}
}

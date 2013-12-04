namespace JoinFile
{
    using Ionic.Zip;
    using SpreadsheetGear;
    using System.IO;
    using System;
    using System.Windows.Forms;

    public static class ExcelUtility
    {
        public static string password = "we168";

        public static IWorkbook workbook;

        public static IWorksheet worksheet;

        public static IRange cells;

        ////public static void JoinFiles(string source, string destination)
        ////{
        ////    try
        ////    {
        ////        if (string.IsNullOrEmpty(source.Trim()) || string.IsNullOrEmpty(destination.Trim()))
        ////        {
        ////            throw new Exception("Source and Destination path would not be empty.");
        ////        }

        ////        IWorkbookSet workbookSet = Factory.GetWorkbookSet();
        ////        var beginColumn = 1;
        ////        using (ZipFile zip = new ZipFile(source))
        ////        {
        ////            var a = zip.EntryFileNames.ToList();
        ////            var entry1 = zip[a[0]];
        ////            int i = 1;
        ////            MemoryStream stream1 = new MemoryStream();
        ////            entry1.Extract(stream1);
        ////            workbook = workbookSet.Workbooks.OpenFromStream(stream1, password);
        ////            worksheet = workbook.Worksheets[0];
        ////            cells = worksheet.Cells;
        ////            IRange dataRange;
        ////            var beginRowSourceFile = worksheet.WindowInfo.SplitRows;
        ////            var beginSplitColumn = worksheet.WindowInfo.SplitColumns;
        ////            var currentRow = worksheet.UsedRange.RowCount + 1;
        ////            var columnCount = worksheet.UsedRange.ColumnCount;
        ////            for (; i < a.Count; i++)
        ////            {
        ////                var entry = zip[a[i]];
        ////                MemoryStream stream = new MemoryStream();
        ////                entry.Extract(stream);
        ////                IWorkbook workbook1 = workbookSet.Workbooks.OpenFromStream(stream, password);
        ////                IWorksheet worksheet1 = workbook1.Worksheets[0];
        ////                var beginRow = worksheet1.WindowInfo.SplitRows;
        ////                var rowCount = worksheet1.UsedRange.RowCount - beginRow + 1;
        ////                worksheet1.Cells[beginRow, beginColumn, beginRow + rowCount, beginColumn + columnCount].Copy(cells[currentRow, beginColumn]);
        ////                currentRow += worksheet1.UsedRange.RowCount - beginRow + 1;
        ////                workbook1.Close();
        ////                stream.Close();
        ////            }

        ////            if (cells[currentRow - 1, 1].Value.ToString().Equals("Total"))
        ////            {
        ////                dataRange = cells[beginRowSourceFile, beginColumn, currentRow - 2, beginColumn + columnCount - 1];
        ////            }
        ////            else
        ////            {
        ////                dataRange = cells[beginRowSourceFile, beginColumn, currentRow - 1, beginColumn + columnCount - 1];
        ////            }

        ////            dataRange.Borders[BordersIndex.InsideHorizontal].LineStyle = LineStyle.None;
        ////            dataRange.Borders[BordersIndex.InsideHorizontal].LineStyle = LineStyle.Continous;
        ////            dataRange.Borders[BordersIndex.InsideHorizontal].Color = System.Drawing.Color.LightGray;

        ////            for (i = 1; i < beginSplitColumn; i++)
        ////            {
        ////                string headerName = cells[beginRowSourceFile - 1, i].Value.ToString();
        ////                if (!headerName.ContainsStrings("date,day,month,year,league,match,home,away"))
        ////                {
        ////                    cells[0, i].EntireColumn.AutoFit();
        ////                }
        ////            }

        ////            for (; i < beginSplitColumn + columnCount; i++)
        ////            {
        ////                cells[0, i].EntireColumn.AutoFit();
        ////            }

        ////            workbook.SaveAs(destination, FileFormat.OpenXMLWorkbook, password);
        ////            workbook.Close();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ////MessageBox.Show("There're some error. Please make sure your input file is correct.", "Error", MessageBoxButtons.OK);
        ////        MessageBox.Show(ex.StackTrace, "Error", MessageBoxButtons.OK);
        ////    }
        ////}

        #region .Net 2.0
        public static void JoinFiles(string source, string destination)
        {
            ////try
            ////{
                if (string.IsNullOrEmpty(source.Trim()) || string.IsNullOrEmpty(destination.Trim()))
                {
                    throw new Exception("Source and Destination path would not be empty.");
                }

                IWorkbookSet workbookSet = Factory.GetWorkbookSet();
                var beginColumn = 1;
                using (ZipFile zip = new ZipFile(source))
                {
                    string[] a = new string[zip.EntryFileNames.Count];
                    zip.EntryFileNames.CopyTo(a, 0);
                    var entry1 = zip[a[0]];
                    int i = 1;
                    MemoryStream stream1 = new MemoryStream();
                    entry1.Extract(stream1);
                    workbook = workbookSet.Workbooks.OpenFromStream(stream1, password);
                    worksheet = workbook.Worksheets[0];
                    cells = worksheet.Cells;
                    IRange dataRange;
                    var beginRowSourceFile = worksheet.WindowInfo.SplitRows;
                    var beginSplitColumn = worksheet.WindowInfo.SplitColumns;
                    var currentRow = worksheet.UsedRange.RowCount + 1;
                    var columnCount = worksheet.UsedRange.ColumnCount;
                    for (; i < zip.EntryFileNames.Count; i++)
                    {
                        var entry = zip[a[i]];
                        MemoryStream stream = new MemoryStream();
                        entry.Extract(stream);
                        IWorkbook workbook1 = workbookSet.Workbooks.OpenFromStream(stream, password);
                        IWorksheet worksheet1 = workbook1.Worksheets[0];
                        var beginRow = worksheet1.WindowInfo.SplitRows;
                        var rowCount = worksheet1.UsedRange.RowCount - beginRow + 1;
                        worksheet1.Cells[beginRow, beginColumn, beginRow + rowCount, beginColumn + columnCount].Copy(cells[currentRow, beginColumn]);
                        currentRow += worksheet1.UsedRange.RowCount - beginRow + 1;
                        workbook1.Close();
                        stream.Close();
                    }

                    if (ToStringAcceptNullValue(cells[currentRow - 1, 1].Value).Equals("Total"))
                    {
                        dataRange = cells[beginRowSourceFile, beginColumn, currentRow - 2, beginColumn + columnCount - 1];
                    }
                    else
                    {
                        dataRange = cells[beginRowSourceFile, beginColumn, currentRow - 1, beginColumn + columnCount - 1];
                    }

                    dataRange.Borders[BordersIndex.InsideHorizontal].LineStyle = LineStyle.None;
                    dataRange.Borders[BordersIndex.InsideHorizontal].LineStyle = LineStyle.Continous;
                    dataRange.Borders[BordersIndex.InsideHorizontal].Color = System.Drawing.Color.LightGray;

                    for (i = 1; i < beginSplitColumn; i++)
                    {
                        string headerName;
                        if (cells[beginRowSourceFile - 1, i].Value == null)
                        {
                            // If previous row is null then header is a next previous row instead.
                            headerName = ToStringAcceptNullValue(cells[beginRowSourceFile - 2, i].Value);
                        }
                        else
                        {
                            headerName = ToStringAcceptNullValue(cells[beginRowSourceFile - 1, i].Value);
                        }
                        
                        if (!ContainsStrings(headerName, "date,day,month,year,league,match,home,away"))
                        {
                            cells[0, i].EntireColumn.AutoFit();
                        }
                    }

                    for (; i < beginSplitColumn + columnCount; i++)
                    {
                        cells[0, i].EntireColumn.AutoFit();
                    }

                    workbook.SaveAs(destination, FileFormat.OpenXMLWorkbook, password);
                    workbook.Close();
                }
            ////}
            ////catch (Exception ex)
            ////{
            ////    MessageBox.Show("There're some error. Please make sure your input file is correct.", "Error", MessageBoxButtons.OK);
            ////}
        }

        public static bool ContainsStrings(string str, string stringList)
        {
            var arr = stringList.Split(new char[] { ',' });
            foreach (var item in arr)
            {
                if (str.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToStringAcceptNullValue(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            if (obj is string)
            {
                return obj.ToString();
            }

            return string.Empty;
        }
        #endregion

        //public static bool ContainsStrings(this string str, string stringList)
        //{
        //    var arr = stringList.Split(new char[] { ',' });
        //    return arr.Any(a => str.Equals(a, StringComparison.OrdinalIgnoreCase) == true);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using System.IO;
using System.Drawing.Printing;
using System.Xml;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

namespace NFrmDAS
{
    class Print
    {
        object objPrintBoxNo = new object();
        public void PrintBarcode(string text,string printerName,string production,out string errorMsg)
        {
            errorMsg = "";
            Microsoft.Office.Interop.Excel.Application applicationClass = new Microsoft.Office.Interop.Excel.Application();
            try
            {

                Bitmap bitmap = CreateCode(text, 100, 120);
                SavePicture(bitmap);
                Microsoft.Office.Interop.Excel.Workbook workBook = applicationClass.Workbooks.Add(); //新建一个工作薄
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.Add();//新建一个sheet页
                workSheet.Name = "箱贴"; //sheet页的名字
                workSheet.Cells.EntireRow.AutoFit(); //sheet页中的单元格自动调整列高
                workSheet.Cells.EntireColumn.AutoFit();//sheet页中的单元格自动调整列宽

                workSheet.Shapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "Picture.bmp", MsoTriState.msoFalse, MsoTriState.msoTrue, 20, 20, 280, 105);


                Range rangeOrderNumber = (Range)workSheet.get_Range("A11", "F12");
                rangeOrderNumber.Merge(0);
                rangeOrderNumber.Font.Size = 20;
                rangeOrderNumber.Value = production;
                rangeOrderNumber.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                workSheet.PageSetup.PrintArea = "A1:F12";//打印区域设置
                //  workSheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperUser;
                workSheet.PageSetup.Zoom = false; //打印时页面设置,必须设置为false,页高,页宽才有效
                workSheet.PageSetup.FitToPagesWide = 1; //设置页面缩放的页宽为1页宽
                workSheet.PageSetup.FitToPagesTall = 1; //设置页面缩放的页高自动
                workSheet.PageSetup.TopMargin = 5; //上边距为0
                workSheet.PageSetup.BottomMargin = 5; //下边距为0
                workSheet.PageSetup.LeftMargin = 0; //左边距为0
                workSheet.PageSetup.RightMargin = 0; //右边距为0
                //applicationClass.Visible = true;
                //workSheet.PrintPreview();
                workSheet.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Kill(applicationClass);
                GC.Collect();
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                Kill(applicationClass);
            }

        }
        public void PrintBoxNo(int FLAG,string OrderNo,string OrderDesc,string OrderTotalNumber,string boxNo,int number,string printerName,out string sMsg)  //打印箱贴条码
        {
            lock (objPrintBoxNo)
            {
                sMsg = "";
                Microsoft.Office.Interop.Excel.Application applicationClass = new Microsoft.Office.Interop.Excel.Application();
                try
                {
                    
                   // Bitmap bitmap = CreateCode(OrderNo+number.ToString().PadLeft(3,'0'), 100, 120);
                   // SavePicture(bitmap);
                    Microsoft.Office.Interop.Excel.Workbook workBook = applicationClass.Workbooks.Add(); //新建一个工作薄
                    Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.Add();//新建一个sheet页
                    workSheet.Name = "箱贴"; //sheet页的名字
                    workSheet.Cells.EntireRow.AutoFit(); //sheet页中的单元格自动调整列高
                    workSheet.Cells.EntireColumn.AutoFit();//sheet页中的单元格自动调整列宽

                    Range rangeOrderCode = (Range)workSheet.get_Range("B1", "C1");
                    rangeOrderCode.Merge(0);
                    rangeOrderCode.RowHeight = 110;
                  //  workSheet.Shapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "Picture.bmp", MsoTriState.msoFalse, MsoTriState.msoTrue, 110, 5, 320, 105);

                    Range rangeOrderDesc = (Range)workSheet.get_Range("B2", "C2");
                    rangeOrderDesc.Merge(0);
                    rangeOrderDesc.Font.Size = 60;
                    rangeOrderDesc.RowHeight = 130;
                    rangeOrderDesc.Value = OrderDesc;
                    rangeOrderDesc.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                    Range rangeOrderNo = (Range)workSheet.get_Range("B3", "C3");
                    rangeOrderNo.Merge(0);
                    rangeOrderNo.Font.Size = 60;
                    rangeOrderNo.RowHeight = 140;
                    rangeOrderNo.Value = "订单号：";
                    rangeOrderNo.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    rangeOrderNo.NumberFormat = "@";
                    rangeOrderNo.Font.FontStyle = FontStyle.Bold;
                    rangeOrderNo.Value = OrderNo;

                    Range rangeOrderNumber = (Range)workSheet.get_Range("B4", "B4");
                    rangeOrderNumber.Font.Size = 40;
                    rangeOrderNumber.RowHeight = 100;
                    rangeOrderNumber.ColumnWidth = 30;
                    rangeOrderNumber.Value = "数量：";
                    Range range_OrderNumber = (Range)workSheet.get_Range("C4", "C4");
                    range_OrderNumber.Font.Size = 50;
                    if (FLAG == 0)// 赠品箱
                    {
                        range_OrderNumber.Value = "POP";
                    }
                    else
                    {
                        range_OrderNumber.Value = string.Format("{0}支共{1}支", boxNo, OrderTotalNumber);
                    }

                    Range rangeBoxNo = (Range)workSheet.get_Range("B5", "B5");
                    rangeBoxNo.Font.Size = 40;
                    rangeBoxNo.RowHeight = 100;
                    rangeBoxNo.Value = "箱数：";
                    Range range_BoxNo = (Range)workSheet.get_Range("C5", "C5");
                    range_BoxNo.Font.Size = 40;
                    range_BoxNo.ColumnWidth = 80;
                    range_BoxNo.Value =string.Format("第{0}箱",number);

                    workSheet.PageSetup.PrintArea = "B1:C6";//打印区域设置
                  //  workSheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperUser;
                    workSheet.PageSetup.Zoom = false; //打印时页面设置,必须设置为false,页高,页宽才有效
                    workSheet.PageSetup.FitToPagesWide = 1; //设置页面缩放的页宽为1页宽
                    workSheet.PageSetup.FitToPagesTall = 1; //设置页面缩放的页高自动
                    workSheet.PageSetup.TopMargin = 5; //上边距为0
                    workSheet.PageSetup.BottomMargin = 5; //下边距为0
                    workSheet.PageSetup.LeftMargin = 0; //左边距为0
                    workSheet.PageSetup.RightMargin = 0; //右边距为0
                    //applicationClass.Visible = true;
                    //workSheet.PrintPreview();
                   workSheet.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    Kill(applicationClass);
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    sMsg = ex.ToString();
                    Kill(applicationClass);
                }
            }  
        }

        public string SavePicture(Bitmap bit)
        {
            bit.Save(System.AppDomain.CurrentDomain.BaseDirectory + "Picture.bmp");
            return System.AppDomain.CurrentDomain.BaseDirectory + "Picture.bmp";

        }
        public Bitmap CreateCode(string Text, int Height, int Width)
        {
            try
            {
                ZXing.BarcodeWriter BarcodeWriter = new ZXing.BarcodeWriter();
                BarcodeWriter.Format = ZXing.BarcodeFormat.CODE_128;
                ZXing.Common.EncodingOptions options = new ZXing.Common.EncodingOptions()
                {
                    Width = Width,
                    Height = Height,
                    Margin = 2
                };
                BarcodeWriter.Options = options;
                Bitmap map = BarcodeWriter.Write(Text);
                return map;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        object objPrintDetail = new object();
        public Microsoft.Office.Interop.Excel.Worksheet printBody(bool PrintEndLine,int PAGENUMBER, int PrintEndPage, System.Data.DataTable dt1, Microsoft.Office.Interop.Excel.Worksheet worksheet,int lineNumber, out string msg)
        {
           // int TableCount = dt1.Rows.Count;
            msg = string.Empty;
            try
            {
                for (int j = PAGENUMBER; j < PrintEndPage; j++)
                {
                    
                    if (j == PrintEndPage - 1)
                    {
                        if (PrintEndLine)
                        {
                            Range range_Column1Count = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, 1], worksheet.Cells[12 + j + lineNumber, 1]];
                            range_Column1Count.Value = dt1.Rows[j]["2"].ToString();

                            Range range_Column6Count = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, 10], worksheet.Cells[12 + j + lineNumber, 11]];
                            range_Column6Count.Merge(0);

                            range_Column6Count.Value = dt1.Rows[j]["6"].ToString();

                            Range range_TableEnd = worksheet.Range[worksheet.Cells[j + 11 + lineNumber, 1], worksheet.Cells[j + 11 + lineNumber, 12]];
                            range_TableEnd.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                            range_TableEnd.Font.Size = 20;
                            range_TableEnd.RowHeight = 25;
                            range_TableEnd.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                            Range rangeTableEnd = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, 1], worksheet.Cells[j + 12 + lineNumber, 12]];
                            // rangeTableEnd.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                            rangeTableEnd.Font.Size = 20;
                            rangeTableEnd.RowHeight = 40;
                            rangeTableEnd.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 1)
                            {
                                Range range_Column5 = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, i + 1], worksheet.Cells[12 + j + lineNumber, i + 3]];
                                range_Column5.Merge(0);
                                range_Column5.Value = dt1.Rows[j][i + 2].ToString();
                            }
                            else if (i == 2)
                            {
                                Range rangeData = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, i + 3], worksheet.Cells[12 + j + lineNumber, i + 7]];
                                rangeData.Merge(0);
                                rangeData.Value = dt1.Rows[j][i + 2].ToString();
                            }
                            else if (i == 0)
                            {
                                Range rangeData = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, i + 1], worksheet.Cells[12 + j + lineNumber, i + 1]];
                                rangeData.Merge(0);
                                rangeData.Value = dt1.Rows[j][i].ToString();
                            }
                            else if (i == 3)
                            {
                                Range rangeData = worksheet.Range[worksheet.Cells[j + 12 + lineNumber, i + 7], worksheet.Cells[12 + j + lineNumber, i + 8]];
                                rangeData.Merge(0);
                                rangeData.Value = dt1.Rows[j][i + 2].ToString();
                            }
                        }
                        Range range = worksheet.Range[worksheet.Cells[12 + j + lineNumber, 1], worksheet.Cells[12 + j + lineNumber, 12]];
                        range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        range.Font.Size = 20;
                        range.RowHeight = 25;
                    }
                }
                return worksheet;
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
                worksheet = null;
                return worksheet;
            }
        }
        public Microsoft.Office.Interop.Excel.Worksheet PrintBoxDetailTitle(int PAGENUMBER,Microsoft.Office.Interop.Excel.Worksheet worksheet, string orderNo, string orderDesc, string boxNumber, string YL1, string YL2, string YL3, string YL4, string YL5, string YL6, string YL7, string YL8, string YL9, string YL10,out string msg)
        {
            msg = string.Empty;
            try
            {
                Range rangePrintTime = worksheet.get_Range(string.Format("A{0}", PAGENUMBER+1), string.Format("A{0}", PAGENUMBER+1));
                rangePrintTime.Value = "打印时间";
                rangePrintTime.Font.Size = 13;
                rangePrintTime.ColumnWidth = 15;
                rangePrintTime.RowHeight = 30;

                Range range_PrintTime = worksheet.get_Range(string.Format("B{0}", PAGENUMBER+1), string.Format("B{0}", PAGENUMBER+1));
                range_PrintTime.Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                range_PrintTime.Font.Size = 15;
                range_PrintTime.NumberFormat = "yyyy-MM-dd HH:mm:ss";
                range_PrintTime.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                Range rangeTitle = worksheet.get_Range(string.Format("D{0}", PAGENUMBER+1), string.Format("H{0}", PAGENUMBER+1));
                rangeTitle.Merge(0);
                rangeTitle.Value = "装箱清单";
                rangeTitle.Font.Size = 30;
                rangeTitle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                Range rangeCage = worksheet.get_Range(string.Format("D{0}", PAGENUMBER + 2), string.Format("H{0}", PAGENUMBER + 3));
                rangeCage.Merge(0);
                rangeCage.Font.Size = 15;
                rangeCage.Value = "51711 卡西欧（中国）良品仓库";
                rangeCage.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                Range rangeAddCard = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 2), string.Format("J{0}", PAGENUMBER + 2));
                rangeAddCard.Value = "配保修卡";

                Range rangeAddWatchBox = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 3), string.Format("J{0}", PAGENUMBER + 3));
                rangeAddWatchBox.Value = "配表盒";

                Range rangeAddLocator = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 4), string.Format("J{0}", PAGENUMBER + 4));
                rangeAddLocator.Value = "配产地标签";

                Range rangeAddBag = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 5), string.Format("J{0}", PAGENUMBER + 5));
                rangeAddBag.Value = "配牛皮纸袋";

                Range rangeAddOCBox = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 6), string.Format("J{0}", PAGENUMBER + 6));
                rangeAddOCBox.Value = "配OC表盒";

                Range rangeAddSHN = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 7), string.Format("J{0}", PAGENUMBER + 7));
                rangeAddSHN.Value = "配纸袋BG、SHN";
                rangeAddSHN.ColumnWidth = 12;

                Range rangeAddGift = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 8), string.Format("J{0}", PAGENUMBER + 8));
                rangeAddGift.Value = "礼品";

                Range rangeOther = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 9), string.Format("J{0}", PAGENUMBER + 9));
                rangeOther.Value = "其它";

                Range rangeLines = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 2), string.Format("K{0}", PAGENUMBER + 9));
                rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeTop).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rangeLines.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rangeLines.Borders.get_Item(XlBordersIndex.xlInsideVertical).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rangeLines.Font.Size = 10;
                rangeLines.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                Range rangeOutNumber = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 5), string.Format("A{0}", PAGENUMBER + 5));
                rangeOutNumber.Merge(0);

                rangeOutNumber.Value = "代理商：";

                Range range_OutNumber = worksheet.get_Range(string.Format("B{0}", PAGENUMBER + 5), string.Format("H{0}", PAGENUMBER + 5));
                range_OutNumber.Merge(0);
                range_OutNumber.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                range_OutNumber.NumberFormat = "@";
                range_OutNumber.Value = orderDesc;

                Range rangeOrderNo = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 7), string.Format("A{0}", PAGENUMBER + 7));
                rangeOrderNo.Value = "订 单：";

                Range range_OrderNo = worksheet.get_Range(string.Format("B{0}", PAGENUMBER + 7), string.Format("H{0}", PAGENUMBER + 7));
                range_OrderNo.Merge(0);
                range_OrderNo.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                range_OrderNo.NumberFormat = "@";
                range_OrderNo.Value = orderNo + "   " + string.Format("[第{0}箱]", boxNumber);

                Range range_1 = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 5), string.Format("E{0}", PAGENUMBER + 7));
                range_1.Font.Size = 20;
                range_1.RowHeight = 20;

                Range range_2 = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 11), string.Format("L{0}", PAGENUMBER + 15));
                range_2.Font.Size = 18;
                range_2.RowHeight = 20;

                Range rangeTableColumn3 = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 11), string.Format("A{0}", PAGENUMBER + 11));
                rangeTableColumn3.Value = "序号";

                Range rangeTableColumn4 = worksheet.get_Range(string.Format("B{0}", PAGENUMBER + 11), string.Format("D{0}", PAGENUMBER + 11));
                rangeTableColumn4.Merge(0);
                rangeTableColumn4.Value = "产品名称";

                Range rangeTableColumn5 = worksheet.get_Range(string.Format("E{0}", PAGENUMBER + 11), string.Format("I{0}", PAGENUMBER + 11));
                rangeTableColumn5.Merge(0);
                rangeTableColumn5.Value = "中文名称";

                Range rangeTableColumn8 = worksheet.get_Range(string.Format("J{0}", PAGENUMBER + 11), string.Format("K{0}", PAGENUMBER + 11));
                rangeTableColumn8.Merge(0);
                rangeTableColumn8.Value = "装箱数量";

                Range rangeTableColumn = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 10), string.Format("L{0}", PAGENUMBER + 10));
                rangeTableColumn.RowHeight = 20;
                rangeTableColumn.Font.Size = 13;
                rangeTableColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //  rangeTableColumn.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                Range rangeTableColumn1 = worksheet.get_Range(string.Format("A{0}", PAGENUMBER + 11), string.Format("L{0}", PAGENUMBER + 11));
                rangeTableColumn1.Font.Size = 23;
                rangeTableColumn1.RowHeight = 30;
                rangeTableColumn1.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                rangeTableColumn1.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                Range range_AddCard = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 2), string.Format("K{0}", PAGENUMBER + 2));
                range_AddCard.Value = "配保修卡";
                range_AddCard.Value = YL1;

                Range range_AddWatchBox = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 3), string.Format("K{0}", PAGENUMBER + 3));
                range_AddWatchBox.Value = "配表盒";
                range_AddWatchBox.Value = YL2;

                Range range_AddLocator = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 4), string.Format("K{0}", PAGENUMBER + 4));
                range_AddLocator.Value = "配产地标签";
                range_AddLocator.Value = YL3;

                Range range_AddBag = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 5), string.Format("K{0}", PAGENUMBER + 5));
                range_AddBag.Value = "配牛皮纸袋";
                range_AddBag.Value = YL4;

                Range range_AddOCBox = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 6), string.Format("K{0}", PAGENUMBER + 6));
                range_AddOCBox.Value = "配OC表盒";
                range_AddOCBox.Value = YL5;

                Range range_AddSHN = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 7), string.Format("K{0}", PAGENUMBER + 7));
                range_AddSHN.Value = "配纸袋BG、SHN";
                range_AddSHN.ColumnWidth = 12;
                range_AddSHN.Value = YL6;

                Range range_AddGift = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 8), string.Format("K{0}", PAGENUMBER + 8));
                range_AddGift.Value = "礼品";
                range_AddGift.Value = YL7;

                Range range_Other = worksheet.get_Range(string.Format("K{0}", PAGENUMBER + 9), string.Format("K{0}", PAGENUMBER + 9));
                range_Other.Value = "其它";
                range_Other.Value = YL8;

                return worksheet;
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
                worksheet = null;
                return worksheet;
            }
        }
        public void PrintBoxDetailBody(System.Data.DataTable dt1,int PAGENUMBER,Microsoft.Office.Interop.Excel.Worksheet worksheet,out string msg)
        {
            msg = string.Empty;
            try
            {
                if (dt1.Rows.Count < 30)
                { 
                
                }
                else if (dt1.Rows.Count > 30 && dt1.Rows.Count < 60)
                { 
                 
                }
                else if (dt1.Rows.Count > 30 && dt1.Rows.Count < 90)
                { 
                 
                }
             
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
            }
        }
        public void PrintBoxDetail(System.Data.DataTable dt1,string printerName,string boxNumber, out string sMsg) //打印装箱清单
        {
            lock (objPrintDetail)
            {
                sMsg = "";
                Microsoft.Office.Interop.Excel.Application applicationClass = new Microsoft.Office.Interop.Excel.Application(); //创建Excel对象
                try
                {
                    if (dt1 == null)
                    {
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        return;
                    }
                    int TableCount = dt1.Rows.Count;
                    DataView dv = new DataView(dt1);
                    System.Data.DataTable dt2 = dv.ToTable(true,new string[]{"2","3"});
                    string orderNo = dt2.Rows[0]["2"].ToString();
                    string orderDesc = dt2.Rows[0]["3"].ToString();

                    System.Data.DataTable dt3 = dv.ToTable(true, new string[] { "YL1", "YL2", "YL3", "YL4", "YL5", "YL6", "YL7", "YL8", "YL9", "YL10" });
                    string YL1 = dt3.Rows[0]["YL1"].ToString();
                    string YL2 = dt3.Rows[0]["YL2"].ToString();
                    string YL3 = dt3.Rows[0]["YL3"].ToString();
                    string YL4 = dt3.Rows[0]["YL4"].ToString();
                    string YL5 = dt3.Rows[0]["YL5"].ToString();
                    string YL6 = dt3.Rows[0]["YL6"].ToString();
                    string YL7 = dt3.Rows[0]["YL7"].ToString();
                    string YL8 = dt3.Rows[0]["YL8"].ToString();
                    string YL9 = dt3.Rows[0]["YL9"].ToString();
                    string YL10 = dt3.Rows[0]["YL10"].ToString();

                    Microsoft.Office.Interop.Excel.Workbook workbook = applicationClass.Workbooks.Add();
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets.Add();
                    worksheet.Name = "装箱清单";



                    Range rangePrintTime = worksheet.get_Range("A1", "A1");
                    rangePrintTime.Value = "打印时间";
                    rangePrintTime.Font.Size = 13;
                    rangePrintTime.ColumnWidth = 15;
                    rangePrintTime.RowHeight = 30;

                    Range range_PrintTime = worksheet.get_Range("B1,B1");
                    range_PrintTime.Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    range_PrintTime.Font.Size = 15;
                    range_PrintTime.NumberFormat = "yyyy-MM-dd HH:mm:ss";
                    range_PrintTime.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                    Range rangeTitle = worksheet.get_Range("D1", "H1");
                    rangeTitle.Merge(0);
                    rangeTitle.Value = "装箱清单";
                    rangeTitle.Font.Size = 30;
                    rangeTitle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    Range rangeCage = worksheet.get_Range("D2", "H3");
                    rangeCage.Merge(0);
                    rangeCage.Font.Size = 15;
                    rangeCage.Value = "51711 卡西欧（中国）良品仓库";
                    rangeCage.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    Range rangeAddCard = worksheet.get_Range("J2", "J2");
                    rangeAddCard.Value = "配保修卡";

                    Range rangeAddWatchBox = worksheet.get_Range("J3", "J3");
                    rangeAddWatchBox.Value = "配表盒";

                    Range rangeAddLocator = worksheet.get_Range("J4", "J4");
                    rangeAddLocator.Value = "配产地标签";

                    Range rangeAddBag = worksheet.get_Range("J5", "J5");
                    rangeAddBag.Value = "配牛皮纸袋";

                    Range rangeAddOCBox = worksheet.get_Range("J6", "J6");
                    rangeAddOCBox.Value = "配OC表盒";

                    Range rangeAddSHN = worksheet.get_Range("J7", "J7");
                    rangeAddSHN.Value = "配纸袋BG、SHN";
                    rangeAddSHN.ColumnWidth = 12;

                    Range rangeAddGift = worksheet.get_Range("J8", "J8");
                    rangeAddGift.Value = "礼品";

                    Range rangeOther = worksheet.get_Range("J9", "J9");
                    rangeOther.Value = "其它";

                    Range rangeLines = worksheet.get_Range("J2", "K9");
                    rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeRight).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    rangeLines.Borders.get_Item(XlBordersIndex.xlEdgeTop).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    rangeLines.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    rangeLines.Borders.get_Item(XlBordersIndex.xlInsideVertical).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    rangeLines.Font.Size = 10;
                    rangeLines.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    Range rangeOutNumber = worksheet.get_Range("A5", "A5");
                    rangeOutNumber.Merge(0);
                    
                    rangeOutNumber.Value = "代理商：";

                    Range range_OutNumber = worksheet.get_Range("B5", "H5");
                    range_OutNumber.Merge(0);
                    range_OutNumber.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    range_OutNumber.NumberFormat = "@";
                    range_OutNumber.Value = orderDesc;

                    Range rangeOrderNo = worksheet.get_Range("A7", "A7");
                    rangeOrderNo.Value = "订 单：";

                    Range range_OrderNo = worksheet.get_Range("B7", "H7");
                    range_OrderNo.Merge(0);
                    range_OrderNo.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    range_OrderNo.NumberFormat = "@";
                    range_OrderNo.Value =orderNo +"   "+string.Format("[第{0}箱]",boxNumber);

                    Range range_1 = worksheet.get_Range("A5", "E7");
                    range_1.Font.Size = 20;
                    range_1.RowHeight = 20;

                    Range range_2 = worksheet.get_Range("A11", "L15");
                    range_2.Font.Size = 18;
                    range_2.RowHeight = 20;

                    Range rangeTableColumn3 = worksheet.get_Range("A11", "A11");
                    rangeTableColumn3.Value = "序号";

                    Range rangeTableColumn4 = worksheet.get_Range("B11", "D11");
                    rangeTableColumn4.Merge(0);
                    rangeTableColumn4.Value = "产品名称";

                    Range rangeTableColumn5 = worksheet.get_Range("E11", "I11");
                    rangeTableColumn5.Merge(0);
                    rangeTableColumn5.Value = "中文名称";

                    Range rangeTableColumn8 = worksheet.get_Range("J11", "K11");
                    rangeTableColumn8.Merge(0);
                    rangeTableColumn8.Value = "装箱数量";

                    Range rangeTableColumn = worksheet.get_Range("A10", "L10");
                    rangeTableColumn.RowHeight = 20;
                    rangeTableColumn.Font.Size = 13;
                    rangeTableColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                  //  rangeTableColumn.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                    Range rangeTableColumn1 = worksheet.get_Range("A11", "L11");
                    rangeTableColumn1.Font.Size = 23;
                    rangeTableColumn1.RowHeight = 30;
                    rangeTableColumn1.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rangeTableColumn1.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                    for (int j = 0; j < TableCount; j++)
                    {
                        if (j==30 || j==60 || j==90)
                        {
                            worksheet.HPageBreaks.Add(worksheet.Range[string.Format("A{0}", 12 + j)]);
                        }
                        if (j == TableCount - 1)
                        {
                            Range range_Column1Count = worksheet.Range[worksheet.Cells[j + 12, 1], worksheet.Cells[12 + j, 1]];
                            range_Column1Count.Value = dt1.Rows[j]["2"].ToString();

                            Range range_Column6Count = worksheet.Range[worksheet.Cells[j + 12, 10], worksheet.Cells[12 + j, 11]];
                            range_Column6Count.Merge(0);
                          
                            range_Column6Count.Value = dt1.Rows[j]["6"].ToString();

                            Range range_AddCard = worksheet.get_Range("K2", "K2");
                            range_AddCard.Value = "配保修卡";
                            range_AddCard.Value =YL1;

                            Range range_AddWatchBox = worksheet.get_Range("K3", "K3");
                            range_AddWatchBox.Value = "配表盒";
                            range_AddWatchBox.Value =YL2;

                            Range range_AddLocator = worksheet.get_Range("K4", "K4");
                            range_AddLocator.Value = "配产地标签";
                            range_AddLocator.Value =YL3;

                            Range range_AddBag = worksheet.get_Range("K5", "K5");
                            range_AddBag.Value = "配牛皮纸袋";
                            range_AddBag.Value =YL4;

                            Range range_AddOCBox = worksheet.get_Range("K6", "K6");
                            range_AddOCBox.Value = "配OC表盒";
                            range_AddOCBox.Value = YL5;

                            Range range_AddSHN = worksheet.get_Range("K7", "K7");
                            range_AddSHN.Value = "配纸袋BG、SHN";
                            range_AddSHN.ColumnWidth = 12;
                            range_AddSHN.Value =YL6;

                            Range range_AddGift = worksheet.get_Range("K8", "K8");
                            range_AddGift.Value = "礼品";
                            range_AddGift.Value =YL7;

                            Range range_Other = worksheet.get_Range("K9", "K9");
                            range_Other.Value = "其它";
                            range_Other.Value =YL8;

                            Range range_TableEnd = worksheet.Range[worksheet.Cells[j + 11, 1], worksheet.Cells[j + 11, 12]];
                            range_TableEnd.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                            range_TableEnd.Font.Size = 20;
                            range_TableEnd.RowHeight = 40;
                            range_TableEnd.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                            Range rangeTableEnd = worksheet.Range[worksheet.Cells[j + 12, 1], worksheet.Cells[j + 12, 12]];
                           // rangeTableEnd.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                            rangeTableEnd.Font.Size = 20;
                            rangeTableEnd.RowHeight = 40;
                            rangeTableEnd.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (i == 1)
                                {
                                    Range range_Column5 = worksheet.Range[worksheet.Cells[j + 12, i + 1], worksheet.Cells[12 + j, i + 3]];
                                    range_Column5.Merge(0);
                                    range_Column5.Value = dt1.Rows[j][i+2].ToString();
                                }
                                else if (i ==2)
                                {
                                    Range rangeData = worksheet.Range[worksheet.Cells[j + 12, i + 3], worksheet.Cells[12 + j, i + 7]];
                                    rangeData.Merge(0);
                                    rangeData.Value = dt1.Rows[j][i+2].ToString();
                                }
                                else if(i==0)
                                {
                                    Range rangeData = worksheet.Range[worksheet.Cells[j + 12, i + 1], worksheet.Cells[12 + j, i + 1]];
                                    rangeData.Merge(0);
                                    rangeData.Value = dt1.Rows[j][i].ToString();
                                }
                                else if (i == 3)
                                {
                                    Range rangeData = worksheet.Range[worksheet.Cells[j + 12, i +7], worksheet.Cells[12 + j, i + 8]];
                                    rangeData.Merge(0);
                                    rangeData.Value = dt1.Rows[j][i+2].ToString();
                                }
                            }
                            Range range = worksheet.Range[worksheet.Cells[12 + j, 1], worksheet.Cells[12 + j, 12]];
                            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                          //  range.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                            range.Font.Size = 20;
                            range.RowHeight = 25;

                        }
                    }

                    worksheet.PageSetup.PrintArea = string.Format("A1:L{0}", 13 + TableCount + 4);//打印区域设置
                    //worksheet.PageSetup.CenterFooter = "第1页，共1页"; //页脚
                    worksheet.PageSetup.Zoom = false; //打印时页面设置,必须设置为false,页高,页宽才有效
                    worksheet.PageSetup.FitToPagesWide = 1; //设置页面缩放的页宽为1页宽
                    worksheet.PageSetup.FitToPagesTall = false; //设置页面缩放的页高自动
                  //  worksheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperUser;
                    worksheet.PageSetup.TopMargin = 5; //上边距为0
                    worksheet.PageSetup.BottomMargin = 5; //下边距为0
                    worksheet.PageSetup.LeftMargin = 0; //左边距为0
                    worksheet.PageSetup.RightMargin = 0; //右边距为0
                    //applicationClass.Visible = true;
                    //worksheet.PrintPreview(); //打印预览

                    worksheet.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing,printerName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    Kill(applicationClass);
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    sMsg = ex.ToString();
                    Kill(applicationClass);
                }
            }  
        }

        public void PrintDetail_Test(System.Data.DataTable dt1, string printerName, string boxNumber, out string sMsg)
        {
            Microsoft.Office.Interop.Excel.Application applicationClass = new Microsoft.Office.Interop.Excel.Application(); //创建Excel对象
            sMsg = string.Empty;
            string msg = string.Empty;
            try
            { 
                if (dt1 == null)
                {
                    sMsg = "表是NULL";
                    return;
                }
                if (dt1.Rows.Count == 0)
                {
                    sMsg = "表没有数据";
                    return;
                }
                int TableCount = dt1.Rows.Count;
                DataView dv = new DataView(dt1);
                System.Data.DataTable dt2 = dv.ToTable(true, new string[] { "2", "3" });
                string orderNo = dt2.Rows[0]["2"].ToString();
                string orderDesc = dt2.Rows[0]["3"].ToString();

                System.Data.DataTable dt3 = dv.ToTable(true, new string[] { "YL1", "YL2", "YL3", "YL4", "YL5", "YL6", "YL7", "YL8", "YL9", "YL10" });
                string YL1 = dt3.Rows[0]["YL1"].ToString();
                string YL2 = dt3.Rows[0]["YL2"].ToString();
                string YL3 = dt3.Rows[0]["YL3"].ToString();
                string YL4 = dt3.Rows[0]["YL4"].ToString();
                string YL5 = dt3.Rows[0]["YL5"].ToString();
                string YL6 = dt3.Rows[0]["YL6"].ToString();
                string YL7 = dt3.Rows[0]["YL7"].ToString();
                string YL8 = dt3.Rows[0]["YL8"].ToString();
                string YL9 = dt3.Rows[0]["YL9"].ToString();
                string YL10 = dt3.Rows[0]["YL10"].ToString();

                Microsoft.Office.Interop.Excel.Workbook workbook = applicationClass.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets.Add();
                worksheet.Name = "装箱清单";
                int printArea = 0;
                int pageNumber = TableCount / 33;
                printArea = 17 + TableCount + 12 * (pageNumber);
                for (int i = 0; i < pageNumber+1; i++)
                {
                    
                    if (pageNumber == 0)
                    {
                        worksheet = PrintBoxDetailTitle(pageNumber * i, worksheet, orderNo, orderDesc, boxNumber, YL1, YL2, YL3, YL4, YL5, YL6, YL7, YL8, YL9, YL10, out msg);
                        worksheet = printBody(true, 0, dt1.Rows.Count, dt1, worksheet, 0, out msg); 
                    }
                    else if (pageNumber > 0)
                    {
                        worksheet = PrintBoxDetailTitle(45 * i, worksheet, orderNo, orderDesc, boxNumber, YL1, YL2, YL3, YL4, YL5, YL6, YL7, YL8, YL9, YL10, out msg);
                        if (i == pageNumber)
                        {
                            worksheet = printBody(true, 33*i, dt1.Rows.Count, dt1, worksheet, 12*i, out msg); 
                        }
                        else
                        {
                            worksheet = printBody(false, 33*i, 34*(i+1), dt1, worksheet, 12*i, out msg);
                            worksheet.HPageBreaks.Add(worksheet.Range[string.Format("A{0}", 12 + 34*(i+1))]);
                        }
                    }
                      
                }
                  worksheet.PageSetup.PrintArea = string.Format("A1:L{0}",printArea);//打印区域设置
                  //worksheet.PageSetup.CenterFooter = "第1页，共1页"; //页脚
                  worksheet.PageSetup.Zoom = false; //打印时页面设置,必须设置为false,页高,页宽才有效
                  worksheet.PageSetup.FitToPagesWide = 1; //设置页面缩放的页宽为1页宽
                  worksheet.PageSetup.FitToPagesTall = false; //设置页面缩放的页高自动
                  //  worksheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperUser;
                  worksheet.PageSetup.TopMargin = 5; //上边距为0
                  worksheet.PageSetup.BottomMargin = 5; //下边距为0
                  worksheet.PageSetup.LeftMargin = 0; //左边距为0
                  worksheet.PageSetup.RightMargin = 0; //右边距为0
                  //applicationClass.Visible = true;
                  //worksheet.PrintPreview(); //打印预览

                  worksheet.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, Type.Missing, printerName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                  Kill(applicationClass);
                  GC.Collect();
                
            }
            catch(Exception ex)
            {
                sMsg = ex.ToString();
                Kill(applicationClass);
            }
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        private static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            try
            {
                IntPtr t = new IntPtr(excel.Hwnd); //得到这个句柄，具体作用是得到 这块内存入口
                int k = 0;
                GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k  
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);//得到对进程k的引用 
                p.Kill();     //关闭进程k 
            }
            catch
            {
            }
        }
    }
}

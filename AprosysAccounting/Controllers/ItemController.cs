using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;

namespace AprosysAccounting.Controllers
{
    public class ItemController : Appcode.BaseAprosysAccountingController
    {
        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        public void DownloadExcel(JQueryDataTableParamModel Param)
        {
            try
            {
                string tmpDirPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/temp/");
                string DetagalioReportTemplate = "Master_Template_Excel";
                string getfile = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/getexcel/" + DetagalioReportTemplate + ".xlsx");
                string masterfilePath = tmpDirPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
                var existingFile = new FileInfo(getfile);
                existingFile = existingFile.CopyTo(masterfilePath);
                var obj = BL_Item.LoadItems(Param);
                //var obj = BL_FilePortalDownloadFile.GetFilePortalDownloadDataForExcel(Param, Request);
                FormatExcelItemReports(obj.ToList(), masterfilePath);

                using (var package = new ExcelPackage(existingFile))
                {
                    //Get the work book in the file
                    var workBook = package.Workbook;

                    if (workBook != null)
                    {
                        var exportData = new MemoryStream();
                        workBook.Worksheets.Delete(1);
                        package.SaveAs(exportData);
                        string saveAsFileName = ("Item " + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".xlsx").Replace(" / ", " - ");
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                        Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                        Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);

                        Response.BinaryWrite(exportData.ToArray());
                        Response.Flush();
                        Response.End();
                    }
                }
                string[] files = System.IO.Directory.GetFiles(tmpDirPath, "*.xlsx");
                if (files.Any())
                {
                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                }

            }
            finally
            {
                Response.Cookies["ava_cv"].Value = Request.QueryString["cv"];
                Response.Cookies["ava_cv"].Expires = DateTime.UtcNow.AddMinutes(5);

            }
        }


        private void FormatExcelItemReports(List<BO_Item> obj, string stream)
        {
            System.Drawing.Color colFromHex;
            if (obj != null)
            {
                string filePath = stream;
                var existingFile = new FileInfo(filePath);
                using (var package = new ExcelPackage(existingFile))
                {
                    //Get the work book in the file


                    var workBook = package.Workbook;
                    if (workBook != null)
                    {
                        string Display_name = "Item Inventory";

                        string sponsorname = Display_name;

                        workBook.Worksheets.Add(sponsorname);
                        var currentWorksheet = workBook.Worksheets[sponsorname];
                        #region  Write Excel story start from here


                        #region header
                        List<string> header = new List<string> { "Name", "Code", "Stock", "Stock In Ltrs.", "Stock In Amount", "Oil Grade", "Packing In Ltrs", "Qty in Carton" };
                        var columnList = header;

                        int startingRow = 1;


                        #region ---Formatting---
                        #endregion
                        for (int i = 1; i < columnList.Count + 1; i++)
                        {
                            currentWorksheet.Cells[startingRow, i].Value = columnList[i - 1];
                            currentWorksheet.Column(i).Width = 10;

                        }
                        DateTime objj = new DateTime();
                        for (int i = 0; i < obj.Count; i++)
                        {
                            currentWorksheet.Cells[startingRow + 1, 1].Value = obj[i].name;
                            currentWorksheet.Cells[startingRow + 1, 2].Value = obj[i].itemCode;
                            currentWorksheet.Cells[startingRow + 1, 3].Value = obj[i].stock;
                            currentWorksheet.Cells[startingRow + 1, 4].Value = obj[i].stock * obj[i].packingInLitre;
                            currentWorksheet.Cells[startingRow + 1, 5].Value = obj[i].stockInAmount;
                            currentWorksheet.Cells[startingRow + 1, 6].Value = obj[i].oilGrade;
                            currentWorksheet.Cells[startingRow + 1, 7].Value = obj[i].packingInLitre;
                            currentWorksheet.Cells[startingRow + 1, 8].Value = obj[i].quantityInCarton;


                            if (obj[i].stockInAmount != null)
                            {

                                // objj = Convert.ToDateTime(obj[i].stockInAmount);
                                
                                currentWorksheet.Cells[startingRow + 1, 5].Value = Convert.ToDecimal(obj[i].stockInAmount);// ("yyyy-MM-dd");
                            }


                            //if (obj[i].DownloadDate != null)
                            //{
                            //    objj = Convert.ToDateTime(obj[i].DownloadDate);
                            //    currentWorksheet.Cells[startingRow + 1, 7].Value = objj.ToString("yyyy-MM-dd");
                            //}
                            startingRow++;
                            if (startingRow % 2 != 0)
                            {
                                currentWorksheet.Cells[startingRow, 1, startingRow, columnList.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                currentWorksheet.Cells[startingRow, 1, startingRow, columnList.Count].Style.Fill.BackgroundColor.SetColor(colFromHex = System.Drawing.ColorTranslator.FromHtml("#DCDCDC"));
                                currentWorksheet.Cells[startingRow, 1, startingRow, columnList.Count].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            }
                        }

                        #endregion
                        #endregion

                        #region---font and background style--
                        currentWorksheet.Cells[1, 1, 1, columnList.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        currentWorksheet.Cells[1, 1, 1, columnList.Count].Style.Fill.BackgroundColor.SetColor(colFromHex = System.Drawing.ColorTranslator.FromHtml("#81ea82"));
                        currentWorksheet.Cells[1, 1, 1, columnList.Count].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                        #endregion

                        currentWorksheet.Cells.AutoFitColumns();
                    }
                    package.Save();

                }
            }
        }

        public JsonResult GetItemList()
        {
            var List = BL_Item.GetItemList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public string SaveItem(string paramitem)
        {

            var _item = BL_Common.Deserialize<BO_Item>(paramitem);
            return BL_Common.Serialize(BL_Item.SaveItem(_item, UserAprosysAccounting.id));
        }

        public ActionResult LoadItemTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Item.LoadItemTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public string DeleteItem(int itemId)
        {
            var pl = BL_Item.DeleteItem(itemId, UserAprosysAccounting.id);
            return BL_Common.Serialize(pl);
        }
        public ActionResult GetItemByID(int itemId)
        {
            BO_Item obj = new BussinessObject.BO_Item();
            obj = BL_Item.GetItemByID(itemId);
            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public string GetItemQuantityforSale(int itemId)
        {
            string qty = BL_Item.GetItemQuantityforSale(itemId).ToString();

            return BL_Common.Serialize(qty);
        }
        public JsonResult GetServiceNameList()
        {
            var List = BL_Item.GetServiceNameList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public string GetActingAccounts()
        {
            var List = BL_Item.GetActingAccounts();
            return BL_Common.Serialize(List);
        }

        public string GetOilGradeData()
        {
            return BL_Common.Serialize(BL_Item.GetOilGradeData());

        }
    }
}
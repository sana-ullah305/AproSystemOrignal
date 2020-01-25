using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;

namespace AprosysAccounting.BussinessLogics
{
    public class BL_Item
    {
        public static string SaveItem(BO_Item _item, int userId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                try
                {
                    //var objcheck = db.Items.Where(x => x.ItemCode.ToLower() == _item.itemCode.ToLower() || x.Name.ToLower() == _item.name.ToLower()).FirstOrDefault();
                    //if (objcheck != null && _item.id == 0)
                    //{
                    //    if (objcheck.ItemCode == _item.itemCode)
                    //    {
                    //        return "Code Already Exists";
                    //    }
                    //    if (objcheck.Name == _item.name)
                    //    {
                    //        return "Name Already Exists";
                    //    }
                    //}
                    var obj = _item.id == 0 ? new ApprosysAccDB.Item() : db.Items.Where(x => x.Id == _item.id).FirstOrDefault();
                    if (_item.id > 0)
                    {
                        var checkCust = db.Items.Where(x => x.Id != _item.id && x.IsActive == true && (x.Name.ToLower() == _item.name.ToLower() || x.ItemCode.ToLower() == _item.itemCode.ToLower())).FirstOrDefault();
                        if (checkCust != null)
                        {
                            if (checkCust.ItemCode.ToLower() == _item.itemCode.ToLower()) { return "Code Already Exists"; }
                            if (checkCust.Name.ToLower() == _item.name.ToLower()) { return "Name Already Exists"; }


                        }
                    }
                    if (obj != null && obj.Id > 0)
                    {
                        obj.ModifiedBy = userId;
                        obj.ModifiedOn = BL_Common.GetDatetime();

                    }
                    obj.Id = _item.id;
                    obj.ItemTypeId = 1;
                    obj.Name = _item.name;
                    obj.ItemCode = _item.itemCode ?? "";
                    obj.MinQty = _item.minQuantity;
                    obj.Unit = _item.unit;
                    obj.PurchasePrice = _item.purchasePrice;
                    obj.SellPrice = _item.sellPrice;
                    obj.TaxPercent = _item.taxPercent;
                    obj.Description = _item.description;
                    obj.IsActive = true;
                    obj.IsTaxable = _item.isTaxable;
                    obj.OilGradeId = _item.oilGradeId;
                    obj.QuantityInCarton = _item.quantityInCarton;
                    obj.PackingInLitre = _item.packingInLitre;

                    if (_item.id == 0)
                    {
                        obj.CreatedBy = userId;
                        obj.CreatedOn = BL_Common.GetDatetime();
                        var objcheck = db.Items.Where(x => x.Id != _item.id && x.IsActive == true && (x.Name.ToLower() == _item.name.ToLower() || x.ItemCode.ToLower() == _item.itemCode.ToLower())).FirstOrDefault();
                        if (objcheck != null)
                        {
                            if (objcheck.ItemCode.ToLower() == _item.itemCode.ToLower()) { return "Code Already Exists"; }
                            if (objcheck.Name.ToLower() == _item.name.ToLower()) { return "Name Already Exists"; }


                        }
                        db.Items.Add(obj);
                    }
                    db.SaveChanges();
                    return "success";

                }
                catch { throw; }
            }
        }
        public static List<BO_Item> GetItemList()
        {
            List<BO_Item> obj = null;
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                obj = (from v in db_aa.Items
                       where v.IsActive == true && v.ItemTypeId == 1
                       select new BO_Item
                       {
                           id = v.Id,
                           name = (v.Name ?? ""),
                           itemCode = v.ItemCode,
                           unit = v.Unit,
                           purchasePrice = v.PurchasePrice ?? 0,
                           sellPrice = v.SellPrice ?? 0,
                           taxPercent = v.TaxPercent ?? 0,
                           isTaxable = v.IsTaxable ?? false
                       }).ToList();

                return obj;
            }
        }

        public static MYJSONTblCustom LoadItemTable(JQueryDataTableParamModel Param, HttpRequestBase Request)
        {
            var _itemlist = LoadItems(Param);//it shoult take startDate, Enddate,VendorId
            IEnumerable<BO_Item> filteredCategories;
            if (!string.IsNullOrEmpty(Param.sSearch))
            {
                filteredCategories = _itemlist
                   .Where(
                    c => c.id.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.name.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.itemCode.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.unit.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.minQuantity.ToString().ToLower().Contains(Param.sSearch.ToLower())

                    || c.oilGrade != null && c.oilGrade.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.quantityInCarton != null && c.quantityInCarton.ToString().Contains(Param.sSearch.ToLower())
                    || c.packingInLitre != null && c.packingInLitre.ToString().Contains(Param.sSearch.ToLower())

                    || c.purchasePrice.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.sellPrice.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.taxPercent.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    || c.description.ToString().ToLower().Contains(Param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredCategories = _itemlist;
            }
            Func<BO_Item, dynamic> orderingFunction = null;
            int iSortColums = Convert.ToInt32(Param.iSortingCols);

            if (iSortColums > 0)
            {
                var Sortable0 = Convert.ToBoolean(Request["bSortable_0"]);
                var Sortable1 = Convert.ToBoolean(Request["bSortable_1"]);
                var Sortable2 = Convert.ToBoolean(Request["bSortable_2"]);
                var Sortable3 = Convert.ToBoolean(Request["bSortable_3"]);
                var Sortable4 = Convert.ToBoolean(Request["bSortable_4"]);
                var Sortable5 = Convert.ToBoolean(Request["bSortable_5"]);
                IOrderedEnumerable<BO_Item> query = null;
                int[] iSortCol = new int[iSortColums];
                string[] sSortDir = new string[iSortColums];
                for (int _i = 0; _i < iSortCol.Length; _i++)
                {
                    int i = _i;
                    iSortCol[i] = Convert.ToInt32(Request["iSortCol_" + i + ""]);
                    if (iSortCol[i] == 0) { orderingFunction = (c => iSortCol[i] == 0 && Sortable0 ? c.name : ""); }
                    else if (iSortCol[i] == 1) { orderingFunction = (c => iSortCol[i] == 1 && Sortable1 ? c.itemCode : ""); }
                    else if (iSortCol[i] == 2) { orderingFunction = (c => iSortCol[i] == 2 && Sortable2 ? c.unit : ""); }
                    else if (iSortCol[i] == 3) { orderingFunction = (c => iSortCol[i] == 3 && Sortable3 ? c.taxPercent : 0); }
                    else if (iSortCol[i] == 4) { orderingFunction = (c => iSortCol[i] == 4 && Sortable4 ? c.purchasePrice : 0); }
                    else if (iSortCol[i] == 5) { orderingFunction = (c => iSortCol[i] == 5 && Sortable5 ? c.sellPrice : 0); }
                    sSortDir[i] = Request["sSortDir_" + i + ""]; // asc or desc
                    //  var sortDirection = Request["sSortDir_0"];
                    if (sSortDir[i] == "asc")
                    {
                        query = (i == 0) ? filteredCategories.OrderBy(orderingFunction) : query.ThenBy(orderingFunction);
                    }
                    else
                    {
                        query = (i == 0) ? filteredCategories.OrderByDescending(orderingFunction) : query.ThenByDescending(orderingFunction);
                    }
                    filteredCategories = query;

                }

            }

            var displayedOffers = filteredCategories.Skip(Param.iDisplayStart).Take(Param.iDisplayLength);
            var result = from c in displayedOffers
                         select new
                         {
                             id = c.id,
                             name = c.name,
                             itemCode = c.itemCode,
                             unit = c.unit,
                             minQuantity = c.minQuantity,
                             purchasePrice = c.purchasePrice,
                             sellPrice = c.sellPrice,
                             taxPercent = c.taxPercent,
                             description = c.description,
                             stock = c.stock,
                             oilGrade = c.oilGrade,
                             quantityInCarton = c.quantityInCarton ?? 0,
                             packingInLitre = c.packingInLitre ?? 0,
                             stockInAmount = c.stockInAmount ?? 0,
                             stockInLtrs = c.stockInLtrs
                         };

            MYJSONTblCustom _MYJSONTbl = new MYJSONTblCustom();
            _MYJSONTbl.sEcho = Param.sEcho;
            _MYJSONTbl.iTotalRecords = _itemlist.Count();
            _MYJSONTbl.iTotalDisplayRecords = filteredCategories.Count();
            _MYJSONTbl.aaData = result;
            return _MYJSONTbl;

        }
        public static IList<BO_Item> LoadItems(JQueryDataTableParamModel Param)
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {


                List<BO_Item> List = (from _item in db_aa.Items
                                          //join og in db_aa.OilGrades on _item.OilGradeId equals og.Id
                                      where _item.IsActive == true
                                      orderby _item.Id descending
                                      select new BO_Item

                                      {
                                          id = _item.Id,
                                          name = _item.Name ?? "",
                                          itemCode = _item.ItemCode ?? "",
                                          unit = _item.Unit ?? "",
                                          description = _item.Description ?? "",
                                          purchasePrice = _item.PurchasePrice ?? 0,
                                          sellPrice = _item.SellPrice ?? 0,
                                          taxPercent = _item.TaxPercent ?? 0,
                                          minQuantity = _item.MinQty ?? 0,
                                          //oilGrade = og.OilGade,
                                          oilGradeId = _item.OilGradeId ?? 0,
                                          quantityInCarton = _item.QuantityInCarton ?? 0,
                                          packingInLitre = _item.PackingInLitre ?? 0

                                      }).OrderByDescending(x => x.id).ToList();
                IDictionary<int, string> oilGradesDict = db_aa.OilGrades.Select(z => new { z.Id, z.OilGade }).ToDictionary(z => Convert.ToInt32(z.Id), q => q.OilGade);
                var itemStock = db_aa.GetStockList(0).ToList();
                foreach (var item in List)
                {

                    var stock = itemStock.Where(x => x.ItemId == item.id).FirstOrDefault();
                    item.stock = stock == null ? 0 : stock.QTY;

                    if (item.oilGradeId > 0)
                    { item.oilGrade = oilGradesDict[item.oilGradeId]; }
                    // Stock in Amount by Fifo based
                    decimal? amount = 0;
                    var itemPurchased = db_aa.Acc_GL.Where(x => x.ItemId == item.id && (x.TranTypeId == 1 || x.TranTypeId == 7) && x.IsActive == true).ToList();
                    foreach (var objpurchase in itemPurchased)
                    {
                        amount = amount + (objpurchase.QuantityBalance * objpurchase.UnitPrice);
                    }
                    item.stockInAmount = amount;

                }
                if (Param.SearchType != 0)
                {
                    if (Param.SearchType == 1 && Param.SearchValue != null && Param.SearchValue != "")// && Param.SearchValue != null && Param.SearchValue != " ")
                    {
                        List = List.Where(x => x.name.ToLower().Contains(Param.SearchValue.Trim().ToLower())).ToList();
                    }
                    else if (Param.SearchType == 2)// && Param.SearchValue != null && Param.SearchValue != " ")
                    {
                        List = List.Where(x => x.itemCode.ToLower().Contains(Param.SearchValue.Trim().ToLower())).ToList();

                    }
                }
                List<BO_Item> ListtoReturn = new List<BO_Item>();
                ListtoReturn = List;
                return ListtoReturn;
            }
        }
        public static string DeleteItem(int _itemId, int userID)
        {

            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var vendTransaction = db.Acc_GL.Where(x => x.ItemId == _itemId && x.IsActive == true).ToList();// && x.Quantity == x.QuantityBalance).ToList();
                if (vendTransaction == null || vendTransaction.Count > 0) { return "Transaction is Performed against Item, it can not be deleted "; }

                var obj = db.Items.Where(x => x.Id == _itemId).FirstOrDefault();

                if (obj != null && obj.Id > 0)
                {
                    obj.ModifiedBy = userID;
                    obj.ModifiedOn = BL_Common.GetDatetime();

                }
                obj.IsActive = false;

                //        db.Items.Add(obj);
                db.SaveChanges();
                return "success";
            }
        }

        public static BO_Item GetItemByID(int itemId)
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.Items.Where(x => x.Id == itemId).FirstOrDefault();
                BO_Item _item = new BussinessObject.BO_Item();
                if (obj != null && obj.Id > 0)
                {
                    _item.id = obj.Id;
                    _item.itemCode = obj.ItemCode ?? "";
                    _item.name = obj.Name ?? "";
                    _item.unit = obj.Unit ?? "";
                    _item.minQuantity = obj.MinQty ?? 0;
                    _item.purchasePrice = obj.PurchasePrice ?? 0;
                    _item.sellPrice = obj.SellPrice ?? 0;
                    _item.description = obj.Description ?? "";
                    _item.taxPercent = obj.TaxPercent ?? 0;
                    _item.isTaxable = obj.IsTaxable ?? false;
                    _item.oilGradeId = obj.OilGradeId ?? 0;
                    _item.quantityInCarton = obj.QuantityInCarton ?? 0;
                    _item.packingInLitre = obj.PackingInLitre ?? 0m;

                }
                return _item;

            }
        }

        public static decimal GetItemQuantityforSale(int itemId)
        {
            decimal qty = 0;
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                var obj = db.GetStockList(itemId).FirstOrDefault();
                if (obj != null)
                {
                    qty = obj.QTY ?? 0;
                }

            }
            return qty;
        }

        public static List<BO_ServicesProvide> GetServiceNameList()
        {

            List<BO_ServicesProvide> lst_serviceName = new List<BO_ServicesProvide>();
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                BO_ServicesProvide obj;
                var lst = db_aa.GetServiceName().ToList();
                foreach (var item in lst.ToList())
                {
                    obj = new BO_ServicesProvide();
                    obj.id = item.CoaId;
                    obj.name = item.TreeName;
                    obj.cost = item.Cost ?? 0;
                    obj.serviceCode = item.ServiceCode ?? "";
                    lst_serviceName.Add(obj);
                }
            }
            return lst_serviceName;
        }

        public static List<KeyValuePair<int, string>> GetActingAccounts()
        {
            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                List<KeyValuePair<int, string>> accL = new List<KeyValuePair<int, string>>();
                accL = db_aa.Acc_COA.Where(x => x.CoaId == 132 || x.CoaId == 133).ToList().Select(x => new KeyValuePair<int, string>(x.CoaId, x.TreeName)).ToList();
                return accL;
            }
        }
        public static string GetItemNameByItemId(int ItemId)
        {

            using (AprosysAccountingEntities db_aa = new AprosysAccountingEntities())
            {
                string name = db_aa.Items.Where(x => x.Id == ItemId).FirstOrDefault().Name.ToString();
                return name;
            }
        }

        public static List<KeyValuePair<int, string>> GetOilGradeData()
        {
            using (AprosysAccountingEntities db = new AprosysAccountingEntities())
            {
                return db.OilGrades.Where(x => x.IsActive == true).Select(z => new { z.Id, z.OilGade }).ToDictionary(z => Convert.ToInt32(z.Id), q => String.Format("{0}", q.OilGade)).OrderBy(x => x.Value).ToList();
            }
        }

       
    }
}
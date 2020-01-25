using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class JQueryDataTableParamModel
    {
        public int iColumns { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public string sSearch_0 { get; set; }
        public string sSearch_1 { get; set; }
        public string sSearch_2 { get; set; }
        public string sSearch_3 { get; set; }
        public string sSearch_4 { get; set; }
        public int SearchType { get; set; }
        public string SearchValue { get; set; }
        public int VendorID { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string ddlDepartmentSearch { get; set; }
        public bool Searchcheckbox { get; set; }
        public int PatientID { get; set; }
        public int employeeID { get; set; }
        public string doctorList { get; set; }
        public int pl_authNumber { get; set; }
        public string pl_mp_firstName { get; set; }
        public string pl_mp_lastName { get; set; }
        public string pl_dr_firstName { get; set; }
        public string pl_dr_lastName { get; set; }
        public string pl_ap_firstName { get; set; }
        public string pl_ap_lastName { get; set; }
        public string pl_ap_boxNumber { get; set; }
        public string pl_ap_dob { get; set; }
        public string pl_mp_dob { get; set; }
    }
}
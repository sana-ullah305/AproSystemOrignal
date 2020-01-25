using Newtonsoft.Json;
using System;
using System.Linq;
namespace AprosysAccounting.BussinessLogics
{
    public class BL_Common
    {
        public static string Serialize(object obj)
        {
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });
            return result;
        }

        public static object Deserialize(string json, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static DateTime GetDatetime()
        {
            return DateTime.Now;
        }
        public static string GetLastVoucher(int transType)
        {
            using (ApprosysAccDB.AprosysAccountingEntities db = new ApprosysAccDB.AprosysAccountingEntities())
            {
                var alpha = db.GetLastVoucherNo(transType).FirstOrDefault();
                return alpha;
            }
        }
    }
}
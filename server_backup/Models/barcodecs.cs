using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;

namespace BarCode.Models
{
    public class barcodecs
    {
        public string generateBarcode(string varietyName, string sampleNumber, string rank)
        {
            try
            {
                /*string[] charPool = "1-2-3-4-5-6-7-8-9-0".Split('-');
                StringBuilder rs = new StringBuilder();
                int length = 6;
                Random rnd = new Random();
                while (length-- > 0)
                {
                    int index = (int)(rnd.NextDouble() * charPool.Length);
                    if (charPool[index] != "-")
                    {
                        rs.Append(charPool[index]);
                        charPool[index] = "-";
                    }
                    else
                        length++;
                }*/
                StringBuilder rs = new StringBuilder();
                rs.Append(varietyName.Replace(" ", "").ToUpper()).Append(sampleNumber.Replace(" ", "").ToUpper()).Append(rank.Replace(" ", "").ToUpper());
                return rs.ToString();
            }
            catch (Exception)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return "";
        }

        //31 December 2012 Prapti

        public Byte[] getBarcodeImage(string barcode, string file)
        {
            try
            {
                BarCode39 _barcode = new BarCode39();
                int barSize = 16;
                string fontFile = HttpContext.Current.Server.MapPath("~/fonts/free3of9.ttf");
                return (_barcode.Code39(barcode, barSize, false, file, fontFile));
            }
            catch (Exception)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return null;
        }
    }
}

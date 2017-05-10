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
        //public string generateBarcode(string varietyName, string sampleNumber, string rank)
        public string generateBarcode(string varietyName, string sampleNumber)
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
                //varietyName = varietyName.Replace("", "");
                //rs.Append(varietyName.Replace(" ", "").ToUpper()).Append(sampleNumber.Replace(" ", "").ToUpper()).Append(rank.Replace(" ", "").ToUpper());
                var code = "";
                var accessionNo = "";

                String[] varietyParts = varietyName.Split(' ');

              if (!varietyParts[0].Equals("") && !varietyParts[1].Equals(""))
              {
                    //Drop the first letter; if TDr or TDa it will now be Dr or Da
                    if (varietyParts[0].Length < 3)
                      {
                          code = varietyParts[0];
                      }
                    else if (varietyParts[0].Length == 3)
                    {
                        code = varietyParts[0].Substring(1);
                    }
                    else {
                        code = varietyParts[0].Substring(0,2);
                      }

                    String[] spaceVal = varietyParts[1].Split(' ');

                    if (!spaceVal[0].Equals(""))
                    {
                        if (spaceVal[0].IndexOf('-') > 0)
                        {
                            String[] dashVal = spaceVal[0].Split('-');

                            //Remove the year part
                            if (dashVal[0].IndexOf("/") > 0)
                            {
                                String[] slashVal = dashVal[0].Split('/');
                                if (slashVal.Length > 1 )
                                {
                                    if (!slashVal[1].Equals(""))
                                        accessionNo = slashVal[1];
                                    else
                                        accessionNo = slashVal[0];
                                }
                                    else
                                    accessionNo = slashVal[0];
                            }
                            else
                            {
                                accessionNo = dashVal[0];
                            }                            
                        }
                        else
                        {
                            //Remove the year part
                            String[] slashVal = spaceVal[0].Split('/');
                            if (slashVal.Length > 1 )
                            {
                                if (!slashVal[1].Equals(""))
                                    accessionNo = slashVal[1];
                                else
                                    accessionNo = slashVal[0];
                            }
                            else
                                accessionNo = slashVal[0];
                        }
                    }
                  varietyName = code + "" + accessionNo;
              }
              else if (!varietyParts[0].Equals(""))
              {
                  if (varietyParts[0].Length < 3)
                  {
                      varietyName = varietyParts[0];
                  }
                  else if (varietyParts[0].Length == 3)
                  {
                      if (varietyParts[0].Equals("TDr") || varietyParts[0].Equals("TDa"))
                          varietyName = varietyParts[0].Substring(1);
                      else
                          varietyName = varietyParts[0];
                  }
                  else
                  {
                      varietyName = varietyParts[0].Substring(0, 2);
                  }
              }
                /*var firstIndx = varietyName.IndexOf(' ');
                //
                varietyName = varietyName.Substring(firstIndx + 1);

                if (varietyName.IndexOf(" ")>0) {
                    
                    if (!varietyParts[0].Equals("") && !varietyParts[1].Equals(""))
                        varietyName = varietyParts[1];
                    else if(!varietyParts[0].Equals("") && varietyParts[1].Equals(""))
                        varietyName = varietyParts[0];
                }
                else if (varietyName.Length > 7)
                {
                    var indx = varietyName.IndexOf(" ");
                    varietyName = varietyName.Substring(indx+1);
                }
                */
                rs.Append(varietyName.Replace("(", "/").Replace(")", "/").Replace(" ", "").ToUpper()).Append("/").Append(sampleNumber.Replace(" ", "").ToUpper());
                return rs.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Barcode: " + ex.ToString() + "  " + ex.Message);
            }
            return "";
        }

        //31 December 2012 Prapti

        public Byte[] getBarcodeImage(string barcode, string file)
        {
            try
            {
                BarCode39 _barcode = new BarCode39();
                int barSize = 14;
                string fontFile = HttpContext.Current.Server.MapPath("~/fonts/free3of9.ttf");
                return (_barcode.Code39(barcode, barSize, false, file, fontFile));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Barcode: " + ex.ToString() + "  " + ex.Message);
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// NRE 07-Jun-2014 Test program for running trams web update
//     14-Jun-2014      Add parameter to indicate if WebDB should be updated. Option to be used in debugging
//     22-Jun-2014      Pass parameter of sTRAMSWebDir
//     06-Jul-2014      Pass parameter of SqlServerPassword
//     29-Aug-2014 1191 Additional parameter of bSearchAll 
namespace trams_web_dbupdate
{
    class Program
    {
        static void Main(string[] args)
            
        {

              Boolean bOk;
              String sWebDB = null ;
              String sMainDB = null;
              String sTRAMSWebDir = null;
              String sRefreshWebDB= "true";
              String sDebug = "false";
              Boolean bRefreshWebDB = true;
              Boolean bDebug = false;
              String sPWD = null;

              String sErrorMessage = null;
              String sForW = null;

              String sFtpSite = null;
              String sAuditRoot = null;
              String sSearchAll = null;
              Boolean bSearchAll = false;

              try {
          
                for (int i = 0; i < args.Length; i++)
                {
                  string s = args[i];

                  if (i == 0) // F= ftp W = web interface
                  {
                      sForW = s; 
                  }

                  if (sForW == "W")
                  {
                      if (i == 1)   //web database - sql server
                      {
                          sWebDB = s;
                      }
                      if (i == 2)
                      {
                          sMainDB = s;// Source database - generally TRAMS main db on I: drive
                      }
                      if (i == 3)
                      {
                          sTRAMSWebDir = s;// Source database - generally TRAMS main db on I: drive
                      }
                      if (i == 4)
                      {
                          sRefreshWebDB = s;// Refresh web db?
                      }
                      if (i == 5)
                      {
                          sDebug = s;// Debug
                      }
                      if (i == 6)
                      {
                          sPWD = s;// Debug
                      }
                  }
                  if (sForW == "F")
                  {
                      if (i == 1)   //web database - sql server
                      {
                          sFtpSite = s;
                      }
                      if (i == 2)
                      {
                          sAuditRoot = s;// Source database - generally TRAMS main db on I: drive
                      }
                      if (i == 3)
                      {
                          sSearchAll = s;// Search all
                      }
                      if (i == 4)
                      {
                          sDebug = s;// Debug
                      }
                  }

                }
                //    Console.WriteLine("sWebDB = " + sWebDB);
                //    Console.WriteLine("sMainDB = " + sMainDB);
                //    Console.WriteLine("sRefreshWebDB = " + sRefreshWebDB);
                //    Console.WriteLine("sDebug = " + sDebug);

                if (sRefreshWebDB == "false") { bRefreshWebDB = false; }
                if (sSearchAll == "true") { bSearchAll = true; }
                if (sDebug == "true")  { bDebug = true;}

                //    Console.WriteLine("bRefreshWebDB = " + bRefreshWebDB);
                //    Console.WriteLine("bDebug = " + bDebug);

                TRAMSDataTransfer.TDataTransfer tdt = new TRAMSDataTransfer.TDataTransfer();

                if (sForW == "W")
                {  
                    bOk = tdt.bWebRefresh(sWebDB, sMainDB,sTRAMSWebDir, bRefreshWebDB, bDebug, sPWD);
                }
                if (sForW == "F")
                {
                    bOk = tdt.bFtp(sFtpSite, sAuditRoot, bSearchAll,bDebug);
                }

              }
              catch (Exception e) {
                sErrorMessage = "trams_web_dbupdate, error = " + e.HResult + "; Description = " + e.Message;
                Console.WriteLine(sErrorMessage);
              }
              finally {
              //
              }
        }
    }
}

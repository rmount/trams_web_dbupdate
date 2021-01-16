using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Data.SqlClient;

// NRE 07-Jun-2014 Test program for running trams web update
//     14-Jun-2014      Add parameter to indicate if WebDB should be updated. Option to be used in debugging
//     22-Jun-2014      Pass parameter of sTRAMSWebDir
//     06-Jul-2014      Pass parameter of SqlServerPassword
//     29-Aug-2014 1191 Additional parameter of bSearchAll 
//     19-Mar-2015 1350 New method sftp
//     13-Jul-2015 1318 Comment out clause that was causing webupdate default parameters to be overridden only if debug is on
//     30-Dec-2020 3383 Most parameters moved to config, leaving only parameter of T3 or T4
//     14-Jan-2021 3396 Provide method to check if tramswebserver db is available

namespace trams_web_dbupdate
{
    class Program
    {


        static void Main(string[] args)
            
        {

              Boolean bOk;
              // String sWebDB = null ;
              // String sMainDB = null;
              // String sTRAMSWebDir = null;
              // String sRefreshWebDB= "true";
              // String sDebug = "false";
              // Boolean bRefreshWebDB = true;
              // Boolean bDebug = false;
              // String sPWD = null;

              String sErrorMessage = null;
              // String sForW = null;

              // String sFtpSite = null;
              // String sAuditRoot = null;
              // String sSearchAll = "false";
              // Boolean bSearchAll = false;

              // String sUserName = null;
              // String sPassword  = null;
              // String sSshHostKeyFingerprint = null;
              // String sCredentials = " ";
              // Boolean bCredentials = false;


              String sT3OrT4 = "T3";

             //  String sVersion = "2.0";
              String sVersion = "3.0";




              try {


          
                    for (int i = 0; i < args.Length; i++)
                    {
                        string s = args[i];
                        if (i == 0)
                        {
                            sT3OrT4 = s;
                        }
                    }

                    Console.WriteLine("sVersion = " + sVersion);
                    Console.WriteLine("sT3OrT4 = " + sT3OrT4);

                    TRAMSDataTransfer.TDataTransfer tdt = new TRAMSDataTransfer.TDataTransfer();


                    if (sT3OrT4 == "T3" || sT3OrT4 == "T4")
                    {

                        bOk = tdt.bWebRefresh(sT3OrT4);
                    }

                    if (sT3OrT4 == "TTM")
                    {
                        bOk = pingserver();
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

        static public Boolean pingserver(){

            Ping ping = new Ping();

            String sTramsMasterServer = ConfigurationManager.AppSettings["TramsMasterServer"];
            String sTramsMasterDB = ConfigurationManager.AppSettings["TramsMasterDB"];
            String sTramsMasterAccount = ConfigurationManager.AppSettings["TramsMasterAccount"];
            String sTramsMasterAccountpwd = ConfigurationManager.AppSettings["TramsMasterAccountpwd"];
            String sMasterDBCOnnection = "";

            try
            {
                Console.WriteLine("TramsMasterServer = "+sTramsMasterServer);
                Console.WriteLine("TramsMasterDB = " + sTramsMasterDB);
                Console.WriteLine("TramsMasterAccount = " + sTramsMasterAccount);
                Console.WriteLine("Pinging " + sTramsMasterServer + " ...");
                PingReply pingreply = ping.Send(sTramsMasterServer, 100);
                if (pingreply.Status == IPStatus.Success)
                {
                    Console.WriteLine("Address: {0}", pingreply.Address);
                    Console.WriteLine("status: {0}", pingreply.Status);
                    Console.WriteLine("Round trip time: {0}", pingreply.RoundtripTime);

                    // Try daabase connection

                    sMasterDBCOnnection = "Data Source=" + sTramsMasterServer + ";" +
                               "Database=" + sTramsMasterDB + ";" +
                               "User Id=" + sTramsMasterAccount + ";";

                    byte[] data = Convert.FromBase64String(sTramsMasterAccountpwd);
                    String decodedSqlServerPassword = Encoding.UTF8.GetString(data);

                    Console.WriteLine("Testing database connection to "+sMasterDBCOnnection + " ...");
                    sMasterDBCOnnection = sMasterDBCOnnection + ";Password=" + decodedSqlServerPassword + ";";

                    using (var connection = new SqlConnection(sMasterDBCOnnection))
                    {
                        var query = "SELECT COUNT(*) FROM tblaudit";
                        var command = new SqlCommand(query, connection);
                        connection.Open();
                        Console.WriteLine("SQL Connection successful.");
                        command.ExecuteScalar();
                        Console.WriteLine("SQL Query execution successful.");
                    }


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (PingException ex)
            {
                Console.WriteLine(ex);
                return false;
            } 
    }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace IDU_REST.Logic.Connection
{
    public class Company
    {
        private static Mutex poolLock = new Mutex();
        private static List<Connection> connectionPool = new List<Connection>();
        
        public static SAPbobsCOM.Company GetCompany(string Servername,string Dbuser,string Dbpassword ,string SAPDBName, string SAPUser ,string SAPUserPassword ,string SAPLicense )
        {
            SAPbobsCOM.Company returnCompany  = null;
            string identity = Servername + ";";
            identity = identity + Dbuser + ";";
            identity = identity + Dbpassword + ";";
            identity = identity + SAPDBName + ";";
            identity = identity + SAPUser + ";";
            identity = identity + SAPUserPassword + ";";
            identity = identity + SAPLicense + ";";

            poolLock.WaitOne();

            foreach (var con in connectionPool)
	        {
		        if (identity == con.identity &! con.inUse == false)
                {
                    returnCompany = con.companyInstance;
                    con.inUse = true;
                    poolLock.ReleaseMutex();
                    return returnCompany;
                }
	        }

            poolLock.ReleaseMutex();
            try 
	        {	        
		        Connection newCon = new Connection(Servername, Dbuser, Dbpassword, SAPDBName, SAPUser, SAPUserPassword, SAPLicense);

                poolLock.WaitOne();
                connectionPool.Add(newCon);
                newCon.inUse = true;
                poolLock.ReleaseMutex();
                return newCon.companyInstance;
	        }
	        catch (Exception ex)
	        {		
		        throw ex;
	        }
        }
    }
}
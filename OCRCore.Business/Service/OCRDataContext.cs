using OCRCore.Common.Helper;

namespace OCRCore.Business.Service
{
    public class OCRDataContext : BaseDao
    {
        private static string dbConnString = ConfigHelper.GetConnString("db.config");
        private static string dbUser = ConfigHelper.GetString("db.username");
        private static string dbPass = ConfigHelper.GetString("db.password");

        public OCRDataContext() : base(dbConnString, dbUser, dbPass)
        {

        }

        public OCRDataContext(string pDbConnString, string pDbUser, string pDbPass) : base(pDbConnString, pDbUser, pDbPass)
        {

        }

        //TODO...
    }
}

using Hot.EconetBundle.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hot.Web.Provider.Interfaces.Services
{ 
    public class BundleRepository
    {
        static readonly string SP_BUNDLE_LIST_PROCEDURE = "xBundles_List";
        static readonly string SP_BUNDLE_GET_PROCEDURE = "xBundles_Get";

        public static BundleProduct Hydrate(SqlDataReader sqlDataReader)
        {
            return new BundleProduct
            {
                BundleId = (int)sqlDataReader["BundleId"],
                BrandId = (int)sqlDataReader["BrandId"],
                Name = (string)sqlDataReader["Name"],
                Network = (string)sqlDataReader["Network"],
                Description = Convert.IsDBNull(sqlDataReader["Description"]) ? "" : (string)sqlDataReader["Description"],
                ProductCode = (string)sqlDataReader["ProductCode"],
                Amount = (int)sqlDataReader["Amount"],
                ValidityPeriod = (int)sqlDataReader["ValidityPeriod"]
            };
        }

        public static BundleProduct Get(long Id, SqlConnection sqlConnection, SqlTransaction sqlTransaction = null)
        {
            BundleProduct retval = null;
            using (SqlCommand sqlCommand = new SqlCommand(SP_BUNDLE_GET_PROCEDURE, sqlConnection, sqlTransaction))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("Id", Id);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.Read()) retval = Hydrate(sqlDataReader);
                    sqlDataReader.Close();
                }
            }
            return retval;
        }

        public static BundleProduct Get(string ProductCode, SqlConnection sqlConnection, SqlTransaction sqlTransaction = null)
        {
            BundleProduct retval = null;

            List<BundleProduct> list = List(sqlConnection, sqlTransaction);
            try
            {
                retval = (from BundleProduct item in list where item.ProductCode == ProductCode select item).Single();
            }
            catch
            {
                return null;
            }
            return retval;
        }
        public static BundleProduct Get(int BrandId, int Amount, SqlConnection sqlConnection, SqlTransaction sqlTransaction = null)
        {
            BundleProduct retval = null;
            using (SqlCommand sqlCommand = new SqlCommand(SP_BUNDLE_LIST_PROCEDURE, sqlConnection, sqlTransaction))
            {
                List<BundleProduct> list = new List<BundleProduct>();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(Hydrate(sqlDataReader));
                    }
                    sqlDataReader.Close();
                    try
                    {
                        retval = (from BundleProduct item in list where item.BrandId == BrandId && item.Amount == Amount select item).Single();
                    }
                    catch (Exception)
                    {
                        return null;
                    }


                }
            }
            return retval;
        }
        public static List<BundleProduct> List(SqlConnection sqlConnection, SqlTransaction sqlTransaction = null)
        {
            List<BundleProduct> retval = new List<BundleProduct>();
            using (SqlCommand sqlCommand = new SqlCommand(SP_BUNDLE_LIST_PROCEDURE, sqlConnection, sqlTransaction))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        retval.Add(Hydrate(sqlDataReader));
                    }
                    sqlDataReader.Close();
                }
            }
            return retval;
        }

    }
}
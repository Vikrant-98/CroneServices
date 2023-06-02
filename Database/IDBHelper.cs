using CouponUnblockQualifier.Models.Database;
using System.Data;

namespace CouponUnblockQualifier.Database
{
    public interface IDBHelper
    {
        Task<DataTable> ExecuteQueryGetStoreDetails(CommonRequestDto storeDetailsRequest);
        //Task<DataTable> ExecuteQueryGetStoreDetails(StoreDetailsRequest storeDetailsRequest);
    }
}

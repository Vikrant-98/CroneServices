namespace CouponUnblockQualifier.Models.Database
{
    public class CommonRequestDto
    {
        public string CommandText { get; set; } = String.Empty;
        public string MasterConnectionString { get; set; } = String.Empty;
        public string TenantConnectionString { get; set; } = String.Empty;
        public bool Result { get; set; } = false;
    }
}

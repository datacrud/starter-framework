namespace Project.Core
{
    public static class DataFilter
    {
        public const string SoftDelete = "IsDeleted";
        public const string IsActive = "IsActive";
        public const string MustHaveTenant = "MustHaveTenant";
        public const string MustHaveCompany = "MustHaveCompany";
        public const string MustHaveBranch = "MustHaveBranch";


        public static class Parameters
        {
            /// <summary>
            /// "tenantId".
            /// </summary>
            public const string TenantId = "tenantId";

            /// <summary>
            /// "companyId".
            /// </summary>
            public const string CompanyId = "companyId";

            /// <summary>
            /// "branchId".
            /// </summary>
            public const string BranchId = "branchId";

            /// <summary>
            /// "isDeleted".
            /// </summary>
            public const string IsDeleted = "isDeleted";

            /// <summary>
            /// "isActive".
            /// </summary>
            public const string Active = "active";
        }
    }
}
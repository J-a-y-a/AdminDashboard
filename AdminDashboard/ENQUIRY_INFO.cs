//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminDashboard
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENQUIRY_INFO
    {
        public int LeadsId { get; set; }
        public string TEI_PHONENO { get; set; }
        public string TEI_ID { get; set; }
        public string TEI_CUSTOMER_FIRSTNAME { get; set; }
        public string TEI_CUSTOMER_LASTNAME { get; set; }
        public Nullable<System.DateTime> TEI_CALL_DATE { get; set; }
        public string TEI_CALL_TIME { get; set; }
        public string TEI_ENQUIRYFOR { get; set; }
        public string ENQ_VENTURE { get; set; }
        public string ENQ_SOURCE { get; set; }
        public string OTHER_ENQ_SOURCE { get; set; }
        public string MEDIA_ENQ_SOURCE { get; set; }
        public string MEDIA_SUB_ENQ_SOURCE { get; set; }
        public Nullable<int> CampaignCode { get; set; }
        public string WALKIN_TYPE { get; set; }
        public string FirstSourceOfEnquiry { get; set; }
        public string OTHER_STATUS { get; set; }
    }
}

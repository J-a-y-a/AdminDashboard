using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminDashboard.ViewModels
{
    public class AssignCalls_VM
    {
        [Required(ErrorMessage = "Field Required")]
        [RegularExpression(@"^(?:(\+\d{1,3}[- ]?)?(0)?[6789]\d{9,14}|\w+@\w+\.\w{2,3})$", ErrorMessage = "Invalid Phone / E-Mail...")]
        public string TEI_PHONENO { get; set; }

        //[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Mail...")]
        //public string TEI_EMAIL { get; set; }

        [Required(ErrorMessage = "User Required")]
        public string TEI_ID { get; set; }
        public string TEI_CUSTOMER_FIRSTNAME { get; set; }
        public string TEI_CUSTOMER_LASTNAME { get; set; }
        public Nullable<System.DateTime> TEI_ASSIGN_DATE { get; set; }
        public string TEI_ASSING_TIME { get; set; }
        public string TEI_ASSIGN_ID { get; set; }

        [Required(ErrorMessage = "Enquiry Type Required")]
        public string TEI_ENQUIRY_TYPE { get; set; }

        [Required(ErrorMessage = "Call Date Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TEI_CALL_DATE { get; set; }
        public Nullable<decimal> TEI_SNO { get; set; }

        [Required(ErrorMessage = "User Type Required")]
        public string TEI_CALLER_TYPE { get; set; }
        public string TEI_CALL_STATE { get; set; }
        public string TEI_OPTION { get; set; }
        public string SourceCd { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TEI_PURCHASE_DATE { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public string TCM_ID { get; set; }
        public string TCM_NAME { get; set; }
        public int TCM_DESIGNATION { get; set; }
        public int AssignCount { get; set; }
        public string Flag { get; set; }
        public string ReAssignUserId { get; set; }
        public string ENQ_SOURCE { get; set; }
        public string OTHER_ENQ_SOURCE { get; set; }
        public string MEDIA_ENQ_SOURCE { get; set; }
        public string MEDIA_SUB_ENQ_SOURCE { get; set; }
        public string FirstSourceOfEnquiry { get; set; }

    }
}
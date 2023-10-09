using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AdminDashboard
{
    public class GlobalClass
    {
        public static DataTable GetReAssignTable()
        {
            DataTable dtReAssign = new DataTable();
            dtReAssign.Columns.AddRange(new DataColumn[45] {
                            new DataColumn("LeadsId", typeof(int)),
                            new DataColumn("TEI_PHONENO", typeof(string)),
                            new DataColumn("TEI_ID", typeof(string)),
                            new DataColumn("TEI_CUSTOMER_FIRSTNAME",typeof(string)),
                            new DataColumn("TEI_CUSTOMER_LASTNAME",typeof(string)),
                            new DataColumn("TEI_CALLBACK_DATE",typeof(DateTime)),
                            new DataColumn("TEI_CALLBACK_TIME",typeof(string)),
                            new DataColumn("TEI_CALLER_REMARKS",typeof(string)),
                            new DataColumn("TEI_POSSIBILITY",typeof(string)),
                            new DataColumn("TEI_STAGE",typeof(string)),
                            new DataColumn("TEI_EXECUTIVE_CODE",typeof(string)),
                            new DataColumn("TEI_OPTION",typeof(string)),
                            new DataColumn("TEI_CALL_DATE",typeof(DateTime)),
                            new DataColumn("TEI_CALL_TIME",typeof(string)),
                            new DataColumn("TEI_ENQUIRYFOR",typeof(string)),
                            new DataColumn("TEI_REVIEWEDBY",typeof(string)),
                            new DataColumn("TEI_ASSIGN_DATE",typeof(DateTime)),
                            new DataColumn("TEI_ASSING_TIME",typeof(string)),
                            new DataColumn("TEI_ASSIGN_ID",typeof(string)),
                            new DataColumn("TEI_ENQUIRY_TYPE",typeof(string)),
                            new DataColumn("TEI_SNO",typeof(int)),
                            new DataColumn("TEI_CALLER_TYPE",typeof(string)),
                            new DataColumn("TEI_CALL_STATE",typeof(string)),
                            new DataColumn("TEI_CUSTOMER_COMMENTS",typeof(string)),
                            new DataColumn("TEI_CALL_LOGS",typeof(string)),
                            new DataColumn("SourceCd",typeof(string)),
                            new DataColumn("TEI_VENTURE",typeof(string)),
                            new DataColumn("ENQ_VENTURE",typeof(string)),
                            new DataColumn("ENQ_SOURCE",typeof(string)),
                            new DataColumn("OTHER_ENQ_SOURCE",typeof(string)),
                            new DataColumn("MEDIA_ENQ_SOURCE",typeof(string)),
                            new DataColumn("MEDIA_SUB_ENQ_SOURCE",typeof(string)),
                            new DataColumn("CampaignCode",typeof(int)),
                            new DataColumn("REF_VENTURE",typeof(string)),
                            new DataColumn("REF_CODE",typeof(int)),
                            new DataColumn("WALKIN_TYPE",typeof(string)),
                            new DataColumn("TEI_PURCHASE_DATE",typeof(DateTime)),
                            new DataColumn("PRICE",typeof(decimal)),
                            new DataColumn("SourceCategory",typeof(string)),
                                new DataColumn("PurchaseId",typeof(int)),
                                new DataColumn("Purchase_BatchId",typeof(int)),
                            new DataColumn("FirstSourceOfEnquiry",typeof(string)),
                            new DataColumn("OTHER_STATUS",typeof(string)),
                                new DataColumn("CUSTLEAD_ID",typeof(int)),
                                new DataColumn("CUSTLEAD_CTRL",typeof(int))
                        });
            return dtReAssign;
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe_identity_demo.ViewModels
{
    public class PBIReportViewModel
    {
        public string ContentAbove { get; set; }
        public string ContentBelow { get; set; }


        // identifier for report in customer's own system
        public string ReportCode { get; set; }

        [Required]
        [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$")]
        public Guid? ReportId { get; set; }

        [Required]
        [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$")]
        public Guid? GroupId { get; set; }
        
        public string PageId { get; set; }
    }
}

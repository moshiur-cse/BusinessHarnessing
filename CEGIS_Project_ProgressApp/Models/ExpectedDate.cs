using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class ExpectedDate
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar")]
        [DisplayName("Expected Date")]
        public string Dates { get; set; }
        public virtual List<ProjectInfo> ProjectInfos { get; set; }
    }
}
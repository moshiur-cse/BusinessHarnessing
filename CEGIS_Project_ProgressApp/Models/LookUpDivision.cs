using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class LookUpDivision
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DivisionId { get; set; }
        [DisplayName("Full Name")]
        [Column(TypeName = "varchar")]

        public string DivFullName { get; set; }

        [DisplayName("Division")]
        [Column(TypeName = "varchar")]
        public string DivShortName { get; set; }
        public virtual List<ProjectInfo> ProjectInfos { get; set; }
    }
}



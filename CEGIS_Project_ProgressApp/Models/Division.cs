using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class Division
    {
        
        public int id { get; set; }

        [DisplayName("Full Name")]
        [Column(TypeName = "varchar")]
        public string FullName { get; set; }

        [DisplayName("Division")]
        [Column(TypeName = "varchar")]
        public string ShortName { get; set; }
        public virtual List<ProjectInfo> ProjectInfos { get; set; }
    }
}
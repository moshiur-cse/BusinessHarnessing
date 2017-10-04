using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class ProjectType
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar")]
        [DisplayName("Type")]
        public string TypeName { get; set; }
        public string TypeFullName { get; set; }
        public virtual List<ProjectInfo> ProjectInfos { get; set; }
    }
}
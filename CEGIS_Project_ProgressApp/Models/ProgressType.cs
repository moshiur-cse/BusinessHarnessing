using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class ProgressType
    {
        public int Id { get; set; }           
        public virtual List<ProjectInfo> ProjectInfos { get; set; }
        public string Progress { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class UpdateHistory
    {     
        public  int Id { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual ProjectInfo ProjectInfos { get; set; }
        public int ProgressTypeId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdateTime { get; set; }


    }
}
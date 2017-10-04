using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CEGIS_Project_ProgressApp.Models
{
    public class ProjectInfo
    {
        public int Id { get; set; }

        [DisplayName("Project Name")]
        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Enter Project Name")]
        // [StringLength(10, MinimumLength = 4)]
        public string ProjectName { get; set; }

        [DisplayName("Division")]
        [Required(ErrorMessage = "Select Division")]
        public int DivisionId { get; set; }
        //[ForeignKey("DivisionId")]        
        public virtual Division Division { get; set; }

        [Column(TypeName = "varchar")]
        //[Required(ErrorMessage = "Enter Project Name")]
        public string Client { get; set; }
        [DisplayName("Focal Person")]
        [Column(TypeName = "varchar")]
        //[Required(ErrorMessage = "Enter Focal Person")]
        public string FocalPerson { get; set; }
        public int ProgressTypeId { get; set; }
        [ForeignKey("ProgressTypeId")]
        public virtual ProgressType ProgressType { get; set; }

        [DisplayName("Contact Value")]
        //[Required(ErrorMessage = "Enter Contact Value")]
        public double ContactValue { get; set; }

        [Required(ErrorMessage = "Enter Type")]
        public int ProjectTypeId { get; set; }
        //[ForeignKey("ProjectTypeId")]
        public virtual ProjectType ProjectType { get; set; }

        public double Probility { get; set; }
        public int Duration { get; set; }

        [DisplayName("Expected Date")]
        public int ExpectedDateId { get; set; }
        [ForeignKey("ExpectedDateId")]
        public virtual ExpectedDate ExpectedDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime DateTimes { get; set; }

        [DisplayName("Initial")]
        [Column(TypeName = "varchar")]
        public string UserInitial { get; set; }
        public virtual List<UpdateHistory> UpdateHistorys { get; set; }



    }
}
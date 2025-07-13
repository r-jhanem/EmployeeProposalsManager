using CompalintsSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompalintsSystem.Core.Models
{
    public class UploadsComplainte : IEntityBase
    {
        public UploadsComplainte()
        {
            UploadDate = DateTime.Now;
        }
        public int Id { set; get; }
        [Required(ErrorMessage = "يجب ان تقوم بكتابة هذه الحقل ")]
        public string TitleComplaint { get; set; }

        [Required(ErrorMessage = "يجب ان تقوم بكتابة هذه الحقل ")]
        public virtual int TypeComplaintId { get; set; }
        [ForeignKey("TypeComplaintId")]
        public virtual TypeComplaint TypeComplaint { get; set; }
        [Required(ErrorMessage = "يجب ان تقوم بكتابة هذه الحقل ")]
        public string DescComplaint { get; set; }
        public int? SocietyId { get; set; }
        public virtual Society Society { get; set; }
        public int StatusCompalintId { get; set; } = 1;
        public virtual StatusCompalint StatusCompalint { get; set; }
        public int StagesComplaintId { get; set; } = 1;
        public virtual StagesComplaint StagesComplaint { get; set; }
        public string PropBeneficiarie { get; set; }
        [Required(ErrorMessage = "يجب ان تقوم بختيار المنطقة   ")]
        public virtual int CollegesId { get; set; }
        [ForeignKey("CollegesId")]
        public virtual Colleges Colleges { get; set; }
        [Required(ErrorMessage = "يجب ان تقوم بختيار المنطقة   ")]

        public virtual int DepartmentsId { get; set; }
        [ForeignKey("DepartmentsId")]

        public virtual Departments Departments { get; set; }
        [Required(ErrorMessage = "يجب ان تقوم بختيار المنطقة   ")]

        public virtual int SubDepartmentsId { get; set; }
        [ForeignKey("SubDepartmentsId")]

        public virtual SubDepartments SubDepartments { get; set; }
        public int? ComplatinClassfactionId { get; set; }

        [ForeignKey("ComplatinClassfactionId")]

        public virtual ComplatinClassfaction ComplatinClassfaction { get; set; }

        public virtual ICollection<Compalints_Solution> Compalints_Solutions { get; set; }
        public virtual ICollection<ComplaintsRejected> ComplaintsRejecteds { get; set; }
        public virtual ICollection<UpComplaintCause> ComplaintsUp { get; set; }

        public string UserId { get; set; }
        public string UserRoleName { get; set; }
        //public virtual ApplicationUser User { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public decimal Size { get; set; }
        public string ContentType { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }
        public string Today { get; set; } /*= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);*/
        public string ReturnedTo { get; set; }


    }
}

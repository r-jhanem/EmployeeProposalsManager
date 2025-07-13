using CompalintsSystem.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompalintsSystem.Core.Models
{
    public class UsersCommunication : IEntityBase
    {
        //public UsersCommunication()
        //{
        //    Id = Guid.NewGuid().ToString();
        //}

        public int Id { get; set; }
        public string reportSubmitterId { get; set; }
        public virtual ApplicationUser reportSubmitter { get; set; }
        public string reportSubmitterName { get; set; }
        public string reporteeName { get; set; }
        public string BenfPhoneNumber { get; set; }
        public virtual int CollegesId { get; set; }
        [ForeignKey("CollegesId")]
        public virtual Colleges Colleges { get; set; }
        public virtual int DepartmentsId { get; set; }
        [ForeignKey("DepartmentsId")]
        public virtual Departments Departments { get; set; }
        public virtual int SubDepartmentsId { get; set; }
        [ForeignKey("SubDepartmentsId")]
        public virtual SubDepartments SubDepartments { get; set; }
        public virtual int TypeCommuncationId { get; set; }
        [ForeignKey("TypeCommuncationId")]
        public virtual TypeCommunication TypeCommunication { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string Titile { get; set; }
        public string reason { get; set; }

    }
}
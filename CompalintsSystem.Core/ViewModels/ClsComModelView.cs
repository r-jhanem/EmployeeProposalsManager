using CompalintsSystem.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CompalintsSystem.Core.ViewModels
{
    public class ClsComModelView
    {

        public int Id { get; set; }





        public int ClsTypeEmplloyId { get; set; }
        public string ClsEmplloyOfficerId { get; set; }





        public int ClsCompltintClssictionId { get; set; }


        public ComplatinClassfaction ClsCompltintClssiction { get; set; }


        public int ClsComplaintId { get; set; }


        public UploadsComplainte ClsComplaint { get; set; }



     
        public int SelectedCountryId { get; set; }
        public int SelectedCityId { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> Cities { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace CompalintsSystem.Core.ViewModels
{
    public class TransferComplaintToAnotherUser
    {
        public int ConplaintId { get; set; }
        [Required(ErrorMessage = "يجب ان تختار موظف قبل ان تقوم بإعادة التوجية")]
        public string UserId { get; set; }
    }
}

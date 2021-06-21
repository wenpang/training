using System.ComponentModel.DataAnnotations;

namespace Training.Models
{
    public class DemoViewModel
    {
        [Required(ErrorMessage = "為必填欄位")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "應由數字組成(10碼)")]
        public string ProductID { get; set; }

        public string ProductNM { get; set; }

        [Range(1, 9999, ErrorMessage = "應為1-9999")]
        public int Quantity { get; set; }

        public bool IsDisable { get; set; }
    }
}

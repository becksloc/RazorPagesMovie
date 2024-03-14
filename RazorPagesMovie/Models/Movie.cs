using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [DisplayName("Tên phim")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string? Title { get; set; }
        
        [DisplayName("Ngày phát hành")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Đạo diễn")]
        [StringLength(50)]
        public string? Director { get; set; }

        [DisplayName("Thể loại")]
        public string? Genre { get; set; }

        [DisplayName("Giá tiền")]
        [Required(ErrorMessage = "Không được bỏ trống và phải là số")]
        public decimal Price { get; set; }
    }
}

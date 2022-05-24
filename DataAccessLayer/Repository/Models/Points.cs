using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquaresAPI.DataAcccessLayer.Repository.Models
{
    public class Points
    {
        [Key]
        public int PointID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

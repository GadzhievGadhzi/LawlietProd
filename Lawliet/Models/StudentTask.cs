using System.ComponentModel.DataAnnotations;

namespace Lawliet.Models {
    public class StudentTask {
        public string Id { get; set; }
        public string? Titile { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
        public List<User> Users { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public StudentTask() {
            Users = new List<User>();
        }
    }
}

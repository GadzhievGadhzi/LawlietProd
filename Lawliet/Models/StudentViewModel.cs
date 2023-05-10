namespace Lawliet.Models {
    public class StudentViewModel {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronomyc { get; set; }
        public string Group { get; set; }
        public List<int> Ratings { get; set; } = new List<int>();
    }
}

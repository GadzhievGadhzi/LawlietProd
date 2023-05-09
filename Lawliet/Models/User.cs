using Lawliet.Interfaces;

namespace Lawliet.Models {
    public class User : IDataModel {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? AboutMe { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public string? PictureUrl { get; set; }
        public string? Group { get; set; }
        public List<LessonTopic> Topics { get; set; }
        public List<StudentTask> StudentTasks { get; set; }

        public User() {
            Topics = new List<LessonTopic>();
            StudentTasks = new List<StudentTask>();
        }
    }
}
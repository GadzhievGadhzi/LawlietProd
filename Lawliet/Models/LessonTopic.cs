using Lawliet.Interfaces;

namespace Lawliet.Models {
    public class LessonTopic : IDataModel {
        public string Id { get; set; }
        public string? ShortTitle { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime DateCreation { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
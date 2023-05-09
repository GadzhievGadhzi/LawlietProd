using Lawliet.Data;
using Lawliet.Models;
using Lawliet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lawliet.Controllers {

    public record class TopicId(string Id);

    public class TopicController : Controller {
        private readonly CachingService _cachingService;
        private readonly UserDataContext _context;
        public TopicController(UserDataContext context, CachingService cachingService) {
            _cachingService = cachingService;
            _context = context;
        }

        public IActionResult Remove(string id) {
            DataRepository<LessonTopic> repository = new DataRepository<LessonTopic>(_context);
            repository.Remove(_cachingService.GetObjectFromCache<LessonTopic>(id));
            _cachingService.RemoveObjectFromCache(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Add() {

            

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(LessonTopic topic) {
            topic.Id = new Random().Next(10000, 100000).ToString();
            topic.UserId = HttpContext.Request.Cookies["id"].ToString();
            await _cachingService.AddObjectFromCache(topic);
            return View();
        }

        [HttpGet] 
        public IActionResult Edit(string id) {
            return View(new TopicId(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(LessonTopic topic) {
            topic.UserId = _cachingService.GetObjectFromCache<LessonTopic>(topic.Id).UserId;
            _cachingService.UpdateObject<LessonTopic>(topic);
            _cachingService.RemoveObjectFromCache(topic.Id);
            await _cachingService.AddObjectFromCache(topic);
            return RedirectToAction("Index", "Home");
        }
    }
}
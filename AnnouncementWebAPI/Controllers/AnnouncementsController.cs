using AnnouncementWebAPI.Models;
using AnnouncementWebAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnnouncementWebAPI.DTO;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementWebAPI.Controllers
{

    [Route("api/announcements")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository announcementRepository;

        public AnnouncementController(IAnnouncementRepository announcementRepository)
        {
            this.announcementRepository = announcementRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var announcements = await announcementRepository.GetAllAnnouncementsAsync();

            if (announcements == null || announcements.Count == 0)
            {
                return StatusCode(404, "No announcements found."); // 404 Not Found
            }

            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSimilarAnnouncements(int id)
        {
            var similarAnnouncements = await announcementRepository.GetAnnouncementByIdAsync(id);
            if (similarAnnouncements.Count == 0)
            {
                return StatusCode(404, "Announcement with this id was not found!"); // 404 Not Found
            }

            return Ok(similarAnnouncements);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnouncement([FromBody] AnnouncementDto announcementDto)
        {
            var isAdded = await announcementRepository.AddAnnouncementAsync(announcementDto);
            if (isAdded)
                return StatusCode(200, "Announcement successfully added!"); // 200 OK
            else
                return StatusCode(409, "Announcement with the same title or description already exists!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] AnnouncementDto updateDto)
        {
            var isUpdated = await announcementRepository.UpdateAnnouncementAsync(id, updateDto.Title, updateDto.Description);
            if (!isUpdated)
                return StatusCode(409, "A similar announcement already exists."); // 409 Conflict

            return StatusCode(200, "Announcement successfully updated!"); // 200 OK
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var isDeleted = await announcementRepository.DeleteAnnouncementAsync(id);
            if (isDeleted)
                return StatusCode(200, "Announcement successfully deleted!"); // 200 OK


            return StatusCode(404, "Announcement with this id was not found!"); // 404 Not Found

        }

    }

}

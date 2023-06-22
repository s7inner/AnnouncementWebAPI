using AnnouncementWebAPI.Data;
using AnnouncementWebAPI.DTO;
using AnnouncementWebAPI.Models;
using AnnouncementWebAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementWebAPI.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AnnouncementContext dbContext;
        private readonly IAnnouncementService announcementService;

        public AnnouncementRepository(AnnouncementContext dbContext, IAnnouncementService announcementService)
        {
            this.dbContext = dbContext;
            this.announcementService = announcementService;
        }

        public async Task<List<Announcement>> GetAllAnnouncementsAsync()
        {
            return await dbContext.Announcements.ToListAsync();
        }
            
        public async Task<List<Announcement>> GetAnnouncementByIdAsync(int id)
        {
            List<Announcement> ListSimilarAnnouncements = new List<Announcement>();

            // Find Announcement by id
            List<Announcement> announcements = await GetAllAnnouncementsAsync();
            Announcement AnnouncementById = announcements.FirstOrDefault(a => a.Id == id);

            if (AnnouncementById == null)
            {
                Console.WriteLine("No announcement found with the specified ID.");
                return ListSimilarAnnouncements;
            }

            ListSimilarAnnouncements.Add(AnnouncementById);

            // Find first repeated word in AnnouncementById
            string repeatedWord = await announcementService.FindFirstRepeatedWordAsync(AnnouncementById);
            string comparison = string.IsNullOrEmpty(repeatedWord) ? AnnouncementById.Title.ToLower() : repeatedWord.ToLower();

            // Check other announcements for the presence of the comparison word
            foreach (Announcement announcement in announcements)
            {
                if (ListSimilarAnnouncements.Count >= 4)
                    break;

                if (announcement.Id != AnnouncementById.Id &&
                    (announcement.Title.ToLower().Contains(comparison) ||
                    announcement.Description.ToLower().Contains(comparison)))
                {
                    ListSimilarAnnouncements.Add(announcement);
                }
            }

            return ListSimilarAnnouncements;
        }

             
        public async Task<bool> AddAnnouncementAsync(AnnouncementDto announcementDto)
        {
            var existingAnnouncement = await dbContext.Announcements
                .FirstOrDefaultAsync(a => a.Title == announcementDto.Title || a.Description == announcementDto.Description);

            if (existingAnnouncement != null)
            {
                // Return false if an announcement with the same Title or Description already exists
                return false;
            }

            var announcement = new Announcement
            {
                Title = announcementDto.Title,
                Description = announcementDto.Description,
                DateAdded = DateTime.Now // Set the current date and time
            };

            dbContext.Announcements.Add(announcement);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateAnnouncementAsync(int id, string title, string description)
        {
            var existingAnnouncement = await dbContext.Announcements.FindAsync(id);
            if (existingAnnouncement == null)
                return false;

            bool isDuplicate = await dbContext.Announcements.AnyAsync(a =>
                a.Id != id && (a.Title == title || a.Description == description));

            if (isDuplicate)
                return false;

            if (!string.IsNullOrEmpty(title))
                existingAnnouncement.Title = title;

            if (!string.IsNullOrEmpty(description))
                existingAnnouncement.Description = description;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            var announcement = await dbContext.Announcements.FindAsync(id);
            if (announcement == null)
                return false;

            dbContext.Announcements.Remove(announcement);
            await dbContext.SaveChangesAsync();
            return true;
        }

    }


}

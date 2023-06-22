using AnnouncementWebAPI.DTO;
using AnnouncementWebAPI.Models;

namespace AnnouncementWebAPI.Repositories
{
    public interface IAnnouncementRepository
    {
        Task<List<Announcement>> GetAllAnnouncementsAsync();
        Task<List<Announcement>> GetAnnouncementByIdAsync(int id);
        Task<bool> DeleteAnnouncementAsync(int id);
        Task<bool> AddAnnouncementAsync(AnnouncementDto announcementDto);
        Task<bool> UpdateAnnouncementAsync(int id, string title, string description);
    }
}

using AnnouncementWebAPI.Models;

namespace AnnouncementWebAPI.Services
{
    public interface IAnnouncementService
    {
        Task<string> FindFirstRepeatedWordAsync(Announcement announcement);
    }

}

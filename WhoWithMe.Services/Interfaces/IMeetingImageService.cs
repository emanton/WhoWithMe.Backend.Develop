using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Model.Meeting;

namespace WhoWithMe.Services.Interfaces
{
    public interface IMeetingImageService
    {
        Task<int> CreateMeetingImages(long meetingId, IEnumerable<IFormFile> meetingImages);
        Task<int> DeleteMeetingImages(List<long> meetingImageIds);
        Task<List<MeetingImage>> GetMeetingImages(PaginationMeetingId pagMet);
    }
}

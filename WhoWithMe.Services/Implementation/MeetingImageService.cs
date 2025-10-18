using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Data;
using WhoWithMe.Data;
using System.Linq;
using WhoWithMe.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using WhoWithMe.DTO.Model.Meeting;

namespace WhoWithMe.Services.Implementation
{
    public class MeetingImageService : IMeetingImageService
    {
        private readonly IContext _context;

        public MeetingImageService(IContext context)
        {
            _context = context;
        }

        public async Task<int> CreateMeetingImages(long meetingId, IEnumerable<IFormFile> meetingImages)
        {
            if (meetingImages == null) return 0;

            foreach (var formFile in meetingImages)
            {
                string imageUrl = await S3StorageService.UploadMeetingFile(meetingId, formFile);
                var meetingImage = new MeetingImage { ImageUrl = imageUrl, MeetingId = meetingId };
                _context.SetAsAdded(meetingImage);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMeetingImages(List<long> meetingImageIds)
        {
            if (meetingImageIds == null || meetingImageIds.Count == 0) return 0;

            foreach (var id in meetingImageIds)
            {
                var meetingImage = await _context.Set<MeetingImage>().FirstOrDefaultAsync(x => x.Id == id);
                if (meetingImage != null)
                {
                    _context.SetAsDeleted(meetingImage);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<List<MeetingImage>> GetMeetingImages(PaginationMeetingId pagMet)
        {
            return await _context.Set<MeetingImage>()
                .Where(x => x.MeetingId == pagMet.MeetingId)
                .Skip(pagMet.Offset)
                .Take(pagMet.Count)
                .ToListAsync();
        }
    }
}

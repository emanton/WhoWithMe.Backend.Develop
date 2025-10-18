using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Data;
using Core.Data.Repositories;
using System.Linq;
using WhoWithMe.Services.Helpers;
using WhoWithMe.DTO.Model.Meeting;

namespace WhoWithMe.Services.Implementation
{
    public class MeetingImageService : IMeetingImageService
    {
        private readonly IRepository<MeetingImage> _meetingImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MeetingImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _meetingImageRepository = _unitOfWork.GetRepository<MeetingImage>();
        }

        public async Task<int> CreateMeetingImages(long meetingId, IEnumerable<IFormFile> meetingImages)
        {
            if (meetingImages == null) return 0;

            foreach (var formFile in meetingImages)
            {
                string imageUrl = await S3StorageService.UploadMeetingFile(meetingId, formFile);
                var meetingImage = new MeetingImage { ImageUrl = imageUrl, MeetingId = meetingId };
                _meetingImageRepository.Insert(meetingImage);
            }

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteMeetingImages(List<long> meetingImageIds)
        {
            if (meetingImageIds == null || meetingImageIds.Count == 0) return 0;

            foreach (var id in meetingImageIds)
            {
                var meetingImage = await _meetingImageRepository.GetSingleAsync(x => x.Id == id);
                if (meetingImage != null)
                {
                    _meetingImageRepository.Delete(meetingImage);
                }
            }

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<MeetingImage>> GetMeetingImages(PaginationMeetingId pagMet)
        {
            return await _meetingImageRepository.GetAllAsync(pagMet.Count, pagMet.Offset, x => x.MeetingId == pagMet.MeetingId);
        }
    }
}

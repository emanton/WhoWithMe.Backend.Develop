using System;
using FluentValidation;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO.Meeting.Abstract;

namespace WhoWithMe.Services.Validators
{
    public class MeetingCreateValidator : AbstractValidator<MeetingBaseDTO>
    {
        public MeetingCreateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start date must be in the future");
            RuleFor(x => x.CreatorId).GreaterThan(0).WithMessage("CreatorId is required");
            RuleFor(x => x.CityId).GreaterThan(0).WithMessage("CityId is required");
        }
    }
}

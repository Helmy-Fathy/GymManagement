using AutoMapper;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapPlan();
            MapSession();
        }

        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>()
               .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
               .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HelathRecordViewModel));
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));

            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlanName));
        }

        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategryName));

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }

    }
}

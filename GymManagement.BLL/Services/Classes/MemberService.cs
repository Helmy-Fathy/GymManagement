using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            var membersViewModel = members.Select(m => new MemberViewModel()
            {
                Name = m.Name,
                Email = m.Email,
                Gender = m.Gender.ToString(),
                Phone = m.PhoneNumber,
                Photo = m.Photo,
                Id = m.Id
            });


            return membersViewModel;

        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //Check Email
            var emailExist = await _memberRepository.AnyAsync(x => x.Email == model.Email, ct);
            //Check Phone
            var phoneExist = await _memberRepository.AnyAsync(x => x.PhoneNumber == model.Phone, ct);
            //Email or Phone exist Return false
            if (emailExist || phoneExist) return false;
            //Else Return true Add member
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Gender = model.Gender,
                PhoneNumber = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                },
                HealthRecord = new HealthRecord()
                {
                    Height = model.HelathRecordViewModel.Height,
                    Weight = model.HelathRecordViewModel.weight,
                    BloodType = model.HelathRecordViewModel.BloodType,
                    Note = model.HelathRecordViewModel.Note,
                }

            };
            var result = await _memberRepository.AddAsync(member);
            return result > 0;
        }
    }
}

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
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
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
            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            //Check Phone
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.PhoneNumber == model.Phone, ct);
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

            _unitOfWork.GetRepository<Member>().Add(member); // Add Local
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);

            if (member == null) return null;

            var model = new MemberViewModel()
            {
                Name = member.Name,
                Phone = member.PhoneNumber,
                Email = member.Email,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Addres = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
            };

            var activeMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate > DateTime.Now);

            if (activeMembership is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId, ct);
                model.PlanName = activePlan?.Name;
                model.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                model.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }

            return model;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId, CancellationToken ct = default)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == MemberId, ct: ct);
            if (healthRecord == null) return null;
            else
                return new HealthRecordViewModel()
                {
                    weight = healthRecord.Weight,
                    Height = healthRecord.Height,
                    BloodType = healthRecord.BloodType,
                    Note = healthRecord.Note,
                };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member == null) return null;
            else return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Phone = member.PhoneNumber,
                Email = member.Email,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
                Photo = member.Photo,
            };
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member == null) return false;

            //Check Email
            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id, ct);
            //Check Phone
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.PhoneNumber == model.Phone && x.Id != id, ct);
            //Email or Phone exist Return false
            if (emailExist || phoneExist) return false;

            member.Email = model.Email;
            member.PhoneNumber = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

             _unitOfWork.GetRepository<Member>().Update(member); //Update Local
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

        public async Task<bool> RemoveMemberAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member == null) return false;

            var hasFutureBookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == MemberId && b.Session.StartDate > DateTime.Now, ct);
            if (hasFutureBookings) return false;

             _unitOfWork.GetRepository<Member>().Delete(member); // Delete Local
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}

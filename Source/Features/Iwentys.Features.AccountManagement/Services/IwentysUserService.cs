﻿using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.AccountManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.AccountManagement.Services
{
    public class IwentysUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _userRepository;

        public IwentysUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
        }

        public Task<IwentysUserInfoDto> Get(int id)
        {
            return _userRepository
                .Get()
                .Where(u => u.Id == id)
                .Select(u => new IwentysUserInfoDto(u))
                .SingleAsync();
        }

        public async Task<IwentysUserInfoDto> AddGithubUsername(int id, string githubUsername)
        {
            bool isUsernameUsed = await _userRepository.Get().AnyAsync(s => s.GithubUsername == githubUsername);
            if (isUsernameUsed)
                throw InnerLogicException.StudentExceptions.GithubAlreadyUser(githubUsername);

            //TODO: implement github access validation
            //throw new NotImplementedException("Need to validate github credentials");
            var user = await _userRepository.GetById(id);
            user.GithubUsername = githubUsername;
            _userRepository.Update(user);

            //TODO: implement eventing without direct reference
            //await _achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            await _unitOfWork.CommitAsync();

            return new IwentysUserInfoDto(await _userRepository.FindByIdAsync(id));
        }
    }
}
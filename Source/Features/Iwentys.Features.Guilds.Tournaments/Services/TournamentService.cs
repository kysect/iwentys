﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Tournaments.Domain;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Tournaments.Services
{
    public class TournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<TournamentEntity> _tournamentRepository;
        private readonly IGenericRepository<CodeMarathonTournamentEntity> _codeMarathonTournamentRepository;
        private readonly GithubIntegrationService _githubIntegrationService;

        public TournamentService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _tournamentRepository = _unitOfWork.GetRepository<TournamentEntity>();
            _codeMarathonTournamentRepository = _unitOfWork.GetRepository<CodeMarathonTournamentEntity>();
            _githubIntegrationService = githubIntegrationService;
        }

        public async Task<List<TournamentInfoResponse>> GetAsync()
        {
            List<TournamentEntity> tournaments = await _tournamentRepository
                .Get()
                .ToListAsync();

            return tournaments.SelectToList(TournamentInfoResponse.Wrap);
        }

        public List<TournamentInfoResponse> GetActive()
        {
            return _tournamentRepository
                .Get()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .Select(TournamentInfoResponse.FromEntity)
                .ToList();
        }

        public async Task<TournamentInfoResponse> GetAsync(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _tournamentRepository.FindByIdAsync(tournamentId);
            return TournamentInfoResponse.Wrap(tournamentEntity);
        }

        public async Task<TournamentLeaderboardDto> GetLeaderboard(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _tournamentRepository.FindByIdAsync(tournamentId);
            return tournamentEntity
                .WrapToDomain(_githubIntegrationService, _unitOfWork)
                .GetLeaderboard();
        }

        public async Task<TournamentInfoResponse> CreateCodeMarathon(AuthorizedUser user, CreateCodeMarathonTournamentArguments arguments)
        {
            var systemAdminUser = (await _studentRepository.GetByIdAsync(user.Id)).EnsureIsAdmin();
            
            var codeMarathonTournamentEntity = CodeMarathonTournamentEntity.Create(systemAdminUser, arguments);

            await _tournamentRepository.InsertAsync(codeMarathonTournamentEntity.Tournament);
            await _codeMarathonTournamentRepository.InsertAsync(codeMarathonTournamentEntity);
            await _unitOfWork.CommitAsync();
            
            return TournamentInfoResponse.Wrap(codeMarathonTournamentEntity.Tournament);
        }
    }
}
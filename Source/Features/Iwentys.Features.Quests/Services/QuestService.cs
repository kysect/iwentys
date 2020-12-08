﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Services
{
    public class QuestService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IQuestRepository _questRepository;

        private readonly AchievementProvider _achievementProvider;

        public QuestService(IStudentRepository studentRepository, IQuestRepository questRepository, AchievementProvider achievementProvider)
        {
            _studentRepository = studentRepository;
            _questRepository = questRepository;
            _achievementProvider = achievementProvider;
        }

        public async Task<List<QuestInfoResponse>> GetCreatedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> entities = await _questRepository
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .ToListAsync();

            return entities.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetCompletedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(q => q.State == QuestState.Completed && q.Responses.Any(r => r.StudentId == user.Id))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetActiveAsync()
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(QuestEntity.IsActive)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetArchivedAsync()
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(QuestEntity.IsArchived)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<QuestInfoResponse> CreateAsync(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            QuestEntity quest = await _questRepository.CreateAsync(student, createQuest);
            _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return QuestInfoResponse.Wrap(quest);
        }

        public async Task<QuestInfoResponse> SendResponseAsync(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(id);
            if (questEntity.State != QuestState.Active || questEntity.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            await _questRepository.SendResponseAsync(questEntity, user.Id);
            QuestEntity updatedQuest = await _questRepository.ReadByIdAsync(id);
            return QuestInfoResponse.Wrap(updatedQuest);
        }

        public async Task<QuestInfoResponse> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            questEntity = await _questRepository.SetCompletedAsync(questEntity, userId);
            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return QuestInfoResponse.Wrap(questEntity);
        }

        public async Task<QuestInfoResponse> RevokeAsync(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(questId);
            
            questEntity.Revoke(author);
            
            QuestEntity updatedQuest = await _questRepository.UpdateAsync(questEntity);
            return QuestInfoResponse.Wrap(updatedQuest);
        }
    }
}
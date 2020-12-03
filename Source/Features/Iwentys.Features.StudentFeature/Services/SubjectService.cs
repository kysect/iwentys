﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Features.StudentFeature.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.StudentFeature.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IGroupSubjectRepository _groupSubjectRepository;

        public SubjectService(ISubjectRepository subjectRepository, IGroupSubjectRepository groupSubjectRepository)
        {
            _subjectRepository = subjectRepository;
            _groupSubjectRepository = groupSubjectRepository;
        }

        public async Task<List<SubjectProfileResponse>> Get()
        {
            List<SubjectEntity> subjects = await _subjectRepository.Read()
                .ToListAsync();

            return subjects.SelectToList(SubjectProfileResponse.Wrap);

        }

        public async Task<SubjectProfileResponse> Get(int id)
        {
            SubjectEntity subject = await _subjectRepository
                .Read()
                .FirstAsync(s => s.Id == id);

            return SubjectProfileResponse.Wrap(subject);
        }

        public async Task<List<SubjectProfileResponse>> GetGroupSubjects(int groupId)
        {
            List<SubjectEntity> subjectEntities = await _groupSubjectRepository
                .GetSubjectsForDto(new StudySearchParameters {GroupId = groupId})
                .ToListAsync();

            return subjectEntities.SelectToList(SubjectProfileResponse.Wrap);
        }
    }
}
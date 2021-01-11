﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Features.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Repositories
{
    public interface ISubjectActivityRepository
    {
        IReadOnlyCollection<SubjectActivity> GetStudentActivities(StudySearchParametersDto searchParametersDto);
    }

    public static class StudyDbContextExtensions
    {
        public static IReadOnlyCollection<SubjectActivity> GetStudentActivities(this IStudyDbContext dbContext, StudySearchParametersDto searchParametersDto)
        {
            var query =
                from sa in dbContext.SubjectActivities
                join sg in dbContext.StudyGroups on sa.Student.GroupMember.GroupId equals sg.Id
                join gs in dbContext.GroupSubjects on sa.GroupSubjectId equals gs.Id
                select new { SubjectActivities = sa, StudyGroups = sg, GroupSubjects = gs };

            query.Include(r => r.StudyGroups)
                .Include(r => r.GroupSubjects)
                .ThenInclude(gs => gs.Subject);

            query = query
                .WhereIf(searchParametersDto.GroupId, q => q.StudyGroups.Id == searchParametersDto.GroupId)
                .WhereIf(searchParametersDto.SubjectId, q => q.GroupSubjects.SubjectId == searchParametersDto.SubjectId)
                .WhereIf(searchParametersDto.CourseId, q => q.StudyGroups.StudyCourseId == searchParametersDto.CourseId)
                .WhereIf(searchParametersDto.StudySemester, q => q.GroupSubjects.StudySemester == searchParametersDto.StudySemester);

            return query
                .Select(_ => _.SubjectActivities)
                .ToList();
        }
    }
}
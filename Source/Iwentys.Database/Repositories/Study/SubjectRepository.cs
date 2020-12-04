﻿using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;

namespace Iwentys.Database.Repositories.Study
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<SubjectEntity> Read()
        {
            return _dbContext.Subjects;
        }
    }
}
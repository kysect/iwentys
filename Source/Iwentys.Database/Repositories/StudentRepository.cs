﻿using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class StudentRepository : IGenericRepository<StudentEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public StudentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StudentEntity> CreateAsync(StudentEntity entity)
        {
            EntityEntry<StudentEntity> createdEntity = await _dbContext.Students.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public IQueryable<StudentEntity> Read()
        {
            return _dbContext.Students
                .Include(s => s.Group)
                .Include(s => s.Achievements)
                .ThenInclude(a => a.Achievement)
                .Include(s => s.SubjectActivities)
                .ThenInclude(a => a.GroupSubject)
                .ThenInclude(sg => sg.Subject)
                .Include(s => s.GithubUserEntity)
                .Include(s => s.GuildMember)
                .ThenInclude(gm => gm.Guild);
        }

        public Task<StudentEntity> ReadByIdAsync(int key)
        {
            return Read().FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<StudentEntity> UpdateAsync(StudentEntity entity)
        {
            EntityEntry<StudentEntity> createdEntity = _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.Students.Where(s => s.Id == key).DeleteFromQueryAsync();
        }
    }
}
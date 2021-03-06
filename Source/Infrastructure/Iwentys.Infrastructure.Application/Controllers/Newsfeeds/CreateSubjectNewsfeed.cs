﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Newsfeeds;
using Iwentys.Domain.Newsfeeds.Dto;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Newsfeeds
{
    public static class CreateSubjectNewsfeed
    {
        public class Query : IRequest<Response>
        {
            public NewsfeedCreateViewModel CreateViewModel { get; }
            public AuthorizedUser AuthorizedUser { get; }
            public int SubjectId { get; }

            public Query(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
            {
                CreateViewModel = createViewModel;
                AuthorizedUser = authorizedUser;
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public Response(NewsfeedViewModel newsfeeds)
            {
                Newsfeeds = newsfeeds;
            }

            public NewsfeedViewModel Newsfeeds { get; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser author = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                Subject subject = await _context.Subjects.GetById(request.SubjectId);

                SubjectNewsfeed newsfeedEntity;
                if (author.CheckIsAdmin(out SystemAdminUser admin))
                {
                    newsfeedEntity = SubjectNewsfeed.CreateAsSystemAdmin(request.CreateViewModel, admin, subject);
                }
                else
                {
                    Student student = await _context.Students.GetById(author.Id);
                    newsfeedEntity = SubjectNewsfeed.CreateAsGroupAdmin(request.CreateViewModel, student.EnsureIsGroupAdmin(), subject);
                }

                _context.SubjectNewsfeeds.Add(newsfeedEntity);

                NewsfeedViewModel result = await _context
                    .Newsfeeds
                    .Where(n => n.Id == newsfeedEntity.NewsfeedId)
                    .Select(NewsfeedViewModel.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}
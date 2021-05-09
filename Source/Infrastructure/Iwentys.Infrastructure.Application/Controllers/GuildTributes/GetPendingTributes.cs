﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildTributes
{
    public class GetPendingTributes
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user)
            {
                User = user;
            }

            public AuthorizedUser User { get; set; }
        }

        public class Response
        {
            public Response(List<TributeInfoResponse> tributes)
            {
                Tributes = tributes;
            }

            public List<TributeInfoResponse> Tributes { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly GithubIntegrationService _githubIntegrationService;
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepositoryNew;
            private readonly IGenericRepository<Tribute> _guildTributeRepository;
            private readonly IGenericRepository<GithubProject> _studentProjectRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _githubIntegrationService = githubIntegrationService;
                _unitOfWork = unitOfWork;
                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepositoryNew = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
                _guildTributeRepository = _unitOfWork.GetRepository<Tribute>();
            }

            protected override Response Handle(Query request)
            {
                Guild guild = _guildMemberRepository.ReadForStudent(request.User.Id) ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, null);

                List<TributeInfoResponse> result = _guildTributeRepository
                    .Get()
                    .Where(t => t.GuildId == guild.Id)
                    .Where(t => t.Project.OwnerUserId == request.User.Id)
                    .Select(TributeInfoResponse.FromEntity)
                    .ToList();

                return new Response(result);
            }
        }
    }
}
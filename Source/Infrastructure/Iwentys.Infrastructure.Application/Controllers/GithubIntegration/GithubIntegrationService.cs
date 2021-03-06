﻿using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Infrastructure.Application.Controllers.GithubIntegration
{
    public class GithubIntegrationService
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        public readonly GithubRepositoryApiAccessor Repository;
        public readonly GithubUserApiAccessor User;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IwentysDbContext context)
        {
            _githubApiAccessor = githubApiAccessor;
            User = new GithubUserApiAccessor(githubApiAccessor, context);
            Repository = new GithubRepositoryApiAccessor(githubApiAccessor, User, context);
        }

        public OrganizationInfoDto FindOrganizationInfo(string organizationName)
        {
            return _githubApiAccessor.FindOrganizationInfo(organizationName);
        }
    }
}
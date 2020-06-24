﻿using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Entities;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Iwentys.Database.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Tests.Tools
{
    public class TestCaseContext
    {
        public readonly IUserProfileRepository UserProfileRepository;

        public readonly IUserProfileService UserProfileService;
        public readonly IGuildProfileService GuildProfileService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            IwentysDbContext context = TestDatabaseProvider.GetDatabaseContext();
            UserProfileRepository = new UserProfileRepository(context);

            UserProfileService = new UserProfileService(UserProfileRepository);
            GuildProfileService = new GuildProfileService(new GuildProfileRepository(context), UserProfileRepository);
        }

        public TestCaseContext WithNewUser(out UserProfile userInfo, UserType userType = UserType.Common)
        {
            userInfo = new UserProfile
            {
                Id = RandomProvider.Random.Next(999999),
                Role =  userType
            };
            userInfo = UserProfileRepository.Create(userInfo);

            return this;
        }

        public TestCaseContext WithGuild(UserProfile userInfo, out GuildProfileDto guildProfile)
        {
            guildProfile = GuildProfileService.Create(userInfo.Id, new GuildCreateArgumentDto());

            return this;
        }
    }
}
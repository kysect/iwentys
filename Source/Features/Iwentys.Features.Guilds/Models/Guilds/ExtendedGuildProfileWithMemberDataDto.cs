﻿using System.Collections.Generic;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class ExtendedGuildProfileWithMemberDataDto : GuildProfileDto
    {
        public ExtendedGuildProfileWithMemberDataDto(GuildEntity guild)
            : base(guild)
        {
        }

        public UserMembershipState UserMembershipState { get; set; }

        public List<GithubRepositoryInfoDto> PinnedRepositories { get; set; }
    }
}
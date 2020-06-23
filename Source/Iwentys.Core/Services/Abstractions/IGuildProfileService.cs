﻿using Iwentys.Database.Transferable.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildProfileService
    {
        GuildProfileDto Create(int creator, GuildCreateArgumentDto arguments);
        GuildProfileDto Update(int creator, GuildUpdateArgumentDto arguments);

        GuildProfileDto[] Get();
        GuildProfileDto Get(int id);
    }
}
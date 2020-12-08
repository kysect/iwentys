﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Models.Guilds;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Guilds
{
    public class GuildControllerClient
    {
        public GuildControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }


        public Task<List<GuildProfileShortInfoWithLeaderDto>> GetOverview(int skip = 0, int take = 20)
        {
            //TODO: rework it later
            return Client.GetFromJsonAsync<List<GuildProfileShortInfoWithLeaderDto>>($"/api/guild?skip={skip}&take={take}");
        }

        public Task<ExtendedGuildProfileWithMemberDataDto> Get(int id)
        {
            return Client.GetFromJsonAsync<ExtendedGuildProfileWithMemberDataDto>($"/api/guild/{id}");
        }

        public Task<GuildProfileDto> GetForMember(int memberId)
        {
            //TODO: fix
            return Task.FromResult<GuildProfileDto>(null);
            //return Client.GetFromJsonAsync<GuildProfileDto>($"/api/guild/for-member?memberId={memberId}");
        }

        public async Task<GithubRepositoryInfoDto> AddPinnedProject(int guildId, CreateProjectRequestDto createProject)
        {
            HttpResponseMessage responseMessage = await Client.PostAsJsonAsync($"/api/guild/{guildId}/pinned", createProject);
            return await responseMessage.Content.ReadFromJsonAsync<GithubRepositoryInfoDto>();
        }

        public async Task DeletePinnedProject(int guildId, long repositoryId)
        {
            await Client.DeleteAsync($"/api/guild/{guildId}/pinned/{repositoryId}");
        }

        public Task<GuildMemberLeaderBoardDto> GetGuildMemberLeaderBoard(int guildId)
        {
            return Client.GetFromJsonAsync<GuildMemberLeaderBoardDto>($"/api/guild/{guildId}/member-leaderboard");
        }
    }
}
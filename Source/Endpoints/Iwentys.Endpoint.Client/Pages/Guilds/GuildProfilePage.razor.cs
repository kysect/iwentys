﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage
    {
        private GuildProfileDto _guild;
        private GuildMemberLeaderBoardDto _memberLeaderBoard;
        private List<NewsfeedViewModel> _newsfeeds;
        private List<AchievementInfoDto> _achievements;
        private TributeInfoResponse _activeTribute;
        private TournamentInfoResponse _activeTournament;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _guild = await ClientHolder.ApiGuildGetAsync(GuildId);
            _newsfeeds = (await ClientHolder.ApiNewsfeedGuildGetAsync(GuildId)).ToList();
            _memberLeaderBoard = await ClientHolder.ApiGuildMemberLeaderboardAsync(_guild.Id);

            try
            {
                _activeTribute = await ClientHolder.ApiGuildTributeGetForStudentActiveAsync();
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                Logger.Log(LogLevel.Error, e, "Failed to fetch data.");
            }

            try
            {
                _activeTournament = await ClientHolder.ApiTournamentsForGuildAsync(_guild.Id);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, e, "Failed to fetch data.");
            }

            
            _achievements = (await ClientHolder.ApiAchievementsGuildsAsync(GuildId)).ToList();
        }

        private string LinkToCreateNewsfeedPage()
        {
            return $"/newsfeed/create-guild/{GuildId}";
        }
    }
}

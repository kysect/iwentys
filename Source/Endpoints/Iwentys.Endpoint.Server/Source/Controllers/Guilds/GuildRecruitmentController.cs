﻿using System.Threading.Tasks;
using Iwentys.Endpoint.Server.Source.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.StudentFeature;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Guilds
{
    [Route("api/guild/recruitment")]
    [ApiController]
    public class GuildRecruitmentController : ControllerBase
    {
        private GuildRecruitmentService _guildRecruitmentService;

        public GuildRecruitmentController(GuildRecruitmentService guildRecruitmentService)
        {
            _guildRecruitmentService = guildRecruitmentService;
        }

        [HttpPost("{guildId}/")]
        public async Task<ActionResult<GuildRecruitmentEntity>> Create([FromRoute] int guildId, [FromQuery] string description)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GuildRecruitmentEntity recruitmentEntity = await _guildRecruitmentService.Create(guildId, user.Id, description);
            return Ok(recruitmentEntity);
        }
    }
}
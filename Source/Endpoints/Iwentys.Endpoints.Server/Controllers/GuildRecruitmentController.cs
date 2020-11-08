﻿using System.Threading.Tasks;
using Iwentys.Endpoints.OldServer.Tools;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.StudentFeature;
using Iwentys.Models.Entities.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.OldServer.Controllers
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
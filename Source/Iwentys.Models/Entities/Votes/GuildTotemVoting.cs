﻿using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Models.Entities.Votes
{
    public class GuildTotemVoting
    {
        public Guild Guild { get; set; }
        public int GuildProfileId { get; set; }

        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public Student TotemCandidate { get; set; }
        public int TotemCandidateId { get; set; }
    }
}
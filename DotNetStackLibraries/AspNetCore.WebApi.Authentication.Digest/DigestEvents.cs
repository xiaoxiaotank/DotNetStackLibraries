using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.Digest
{
    public class DigestEvents
    {
        public DigestEvents(Func<GetPasswordContext, Task<string>> onGetPassword)
        {
            OnGetPassword = onGetPassword;
        }

        public Func<GetPasswordContext, Task<string>> OnGetPassword { get; set; } = context => throw new NotImplementedException($"{nameof(OnGetPassword)} must be implemented!");

        public Func<DigestChallengeContext, Task> OnChallenge { get; set; } = context => Task.CompletedTask;

        public virtual Task<string> GetPassword(GetPasswordContext context) => OnGetPassword(context);

        public virtual Task Challenge(DigestChallengeContext context) => OnChallenge(context);
    }
}

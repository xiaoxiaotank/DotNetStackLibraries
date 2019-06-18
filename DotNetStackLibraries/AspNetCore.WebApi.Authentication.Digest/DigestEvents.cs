using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.Digest
{
    public class DigestEvents
    {
        public Func<ValidateCredentialsContext, Task<string>> OnValidateCredentials { get; set; } = context => Task.FromResult(string.Empty);

        public Func<DigestChallengeContext, Task> OnChallenge { get; set; } = context => Task.CompletedTask;

        public virtual Task<string> ValidateCredentials(ValidateCredentialsContext context) => OnValidateCredentials(context);

        public virtual Task Challenge(DigestChallengeContext context) => OnChallenge(context);
    }
}

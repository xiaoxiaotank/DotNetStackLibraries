using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.Basic
{
    public class BasicEvents
    {
        public Func<ValidateCredentialsContext, Task> OnValidateCredentials { get; set; } = context => Task.CompletedTask;

        public Func<BasicChallengeContext, Task> OnChallenge { get; set; } = context => Task.CompletedTask;

        public virtual Task ValidateCredentials(ValidateCredentialsContext context) => OnValidateCredentials(context);

        public virtual Task Challenge(BasicChallengeContext context) => OnChallenge(context);
    }
}

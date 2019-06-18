using AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DigestExtensions
    {
        public static AuthenticationBuilder AddDigest(this AuthenticationBuilder builder)
           => builder.AddDigest(DigestDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddDigest(this AuthenticationBuilder builder, Action<DigestOptions> configureOptions)
            => builder.AddDigest(DigestDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddDigest(this AuthenticationBuilder builder, string authenticationScheme, Action<DigestOptions> configureOptions)
            => builder.AddDigest(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddDigest(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<DigestOptions> configureOptions)
            => builder.AddScheme<DigestOptions, DigestHandler>(authenticationScheme, displayName, configureOptions);
    }
}

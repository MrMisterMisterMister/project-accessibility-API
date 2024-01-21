namespace API.Extensions
{
    public static class SecurityMiddelwareExtension
    {
        public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder app)
        {
            // Enable X-Content-Type-Options header to prevent MIME-sniffing
            app.UseXContentTypeOptions();

            // Set Referrer-Policy header to control information sent with navigations away from a document.
            // Using NoReferrer(): The Referer header will be omitted entirely for improved privacy and security.
            app.UseReferrerPolicy(opt => opt.NoReferrer());

            // Enable XXssProtection header to defend against cross-site scripting attacks.
            // Using EnabledWithBlockMode(): Instruct the browser to block the response if a cross-site scripting 
            // attack is detected, enhancing security.
            app.UseXXssProtection(opt => opt.EnabledWithBlockMode());

            // Enable X-Frame-Options header to prevent your site from being framed, defending against clickjacking.
            // Using Deny(): This option does not allow the page to be displayed in a frame, enhancing security 
            // by avoiding framing.
            app.UseXfo(opt => opt.Deny());

            // Enable Content Security Policy (CSP) to protect the site from various types of attacks, allowing only approved content
            app.UseCsp(opt => opt
                .BlockAllMixedContent()
                .StyleSources(s => s.Self())
                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ScriptSources(s => s.Self().CustomSources("https://apis.google.com"))
                .StyleSources(s => s.Self().CustomSources("https://apis.google.com"))
                .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com"))
                .ImageSources(s => s.Self().CustomSources("https://www.google.com", "https://*.googleusercontent.com"))
                .ConnectSources(s => s.Self().CustomSources("wss://clodsire.nl"))
            );

            app.Use(async (context, next) =>
            {
                // Middleware to set Strict-Transport-Security header, enforcing the use of HTTPS
                // for the specified website for a duration of 31,536,000 seconds, which is equivalent to one year.
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");

                // Middleware to set Permissions-Policy header, controlling features and APIs that can be used in the browser
                // dunno what we'll use gyroscope and magnetometer for but you never know...
                context.Response.Headers.Add("Permissions-Policy", "geolocation=(); midi=(); notifications=(); push=(); sync-xhr=(); accelerometer=(); gyroscope=(); magnetometer=(); payment=(); usb=(); vr=(); camera=(); microphone=(); speaker=(); vibrate=(); ambient-light-sensor=(); autoplay=(); encrypted-media=(); execute-clipboard=(); document-domain=(); fullscreen=(); imagecapture=(); lazyload=(); legacy-image-formats=(); oversized-images=(); unoptimized-lossy-images=(); unoptimized-lossless-images=(); unsized-media=(); vertical-scroll=(); web-share=(); xr-spatial-tracking=();");

                await next.Invoke();
            });

            return app;
        }
    }
}
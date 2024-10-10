using GestionDeCursos.Data.Helpers;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GestionDeCursos.Api.Helpers
{
    public class CustomUrlCultureProvider : IRequestCultureProvider
    {
        private CultureInfo defaultCulture;
        private CultureInfo defaultUICulture;
        private const int indexOfCulture = 2;

        public CustomUrlCultureProvider(RequestCulture requestCulture)
        {
            defaultCulture = requestCulture.Culture;
            defaultUICulture = requestCulture.UICulture;
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            try
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException(nameof(httpContext));
                }

                var path = httpContext.Request.Path.Value.Split("/");
                var culture = path[indexOfCulture]?.ToString();
                string twoLetterRegex = @"^[a-z]{2}(-[A-Z]{2})*$";

                if (culture == null || !Regex.IsMatch(culture, twoLetterRegex) ||
                    (culture != GlobalHelper.Language.English &&
                    culture != GlobalHelper.Language.Spanish))
                {
                    return Task.FromResult(new ProviderCultureResult(defaultCulture.TwoLetterISOLanguageName, defaultUICulture.TwoLetterISOLanguageName));
                }

                return Task.FromResult(new ProviderCultureResult(culture, culture));
            }
            catch (Exception)
            {
                return Task.FromResult(new ProviderCultureResult(defaultCulture.TwoLetterISOLanguageName, defaultUICulture.TwoLetterISOLanguageName));
            }
        }
    }
}

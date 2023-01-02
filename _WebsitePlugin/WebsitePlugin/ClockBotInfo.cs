//
// Activity Pub Website Plugin - Extensions to Pretzel for activitypub.shendrick.net.
// Copyright (C) 2023 Seth Hendrick
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Text.Json;
using KristofferStrube.ActivityStreams;
using KristofferStrube.ActivityStreams.JsonLD;
using Pretzel.Logic.Templating.Context;
using Pretzel.SethExtensions;
using Pretzel.SethExtensions.ActivityPub;

namespace WebsitePlugin
{
    public class ClockBotInfo
    {
        public string WebFinger { get; init; } = "";

        /// <summary>
        /// The username of the bot.  This becomes "preferred username"
        /// in activity pub.
        /// </summary>
        public string UserName { get; init; } = "";

        /// <summary>
        /// The full name of the bot.  This becomes
        /// "name" in activity pub.
        /// </summary>
        public string FullName { get; init; } = "";

        /// <summary>
        /// URL to the bot's specific web finger json file.
        /// </summary>
        public Uri? WebFingerUrl { get; init; } = null;

        /// <summary>
        /// The URL to the profile JSON information.
        /// </summary>
        public Uri? ProfileUrl { get; init; } = null;

        /// <summary>
        /// The URL to the following JSON information.
        /// </summary>
        public Uri? FollowingUrl { get; init; } = null;

        /// <summary>
        /// The URL to the followers JSON information.
        /// </summary>
        public Uri? FollowersUrl { get; init; } = null;

        /// <summary>
        /// The URL to the outbox JSON information.
        /// </summary>
        public Uri? OutboxUrl { get; init; } = null;

        /// <summary>
        /// The URL to the inbox JSON information.
        /// </summary>
        public Uri? InboxUrl { get; init; } = null;

        /// <summary>
        /// URL to more information about this bot.
        /// This becomes "url" in activity pub.
        /// </summary>
        public Uri? BotInfoUrl { get; init; } = null;

        /// <summary>
        /// The website giving more information about
        /// the clock the bot is emulating.
        /// 
        /// This becomes an attachement in activity pub.
        /// </summary>
        public Uri? Website { get; init; } = null;

        /// <summary>
        /// The wikipedia page to more information about the
        /// clock the bot is emulating.
        /// 
        /// This becomes an attachement in activity pub.
        /// </summary>
        public Uri? Wikipedia { get; init; } = null;

        /// <summary>
        /// URL to the source code of the bot.
        /// This becomes an attachement in activity pub.
        /// </summary>
        public Uri? GitHub { get; init; } = null;

        /// <summary>
        /// URL to the icon of the clock bot.
        /// </summary>
        public Uri? IconPath { get; init; } = null;

        /// <summary>
        /// When the bot was created.
        /// 
        /// This becomes "created" in activity pub.
        /// </summary>
        public DateTime? Created { get; init; } = null;

        /// <summary>
        /// Summary of the clock that is being emulated.
        /// 
        /// This becomes "summary" in actvity pub.
        /// </summary>
        public string Summary { get; init; } = "";

        /// <summary>
        /// The public key.  Mastodon needs this.
        /// </summary>
        public string PublicKeyContents { get; init; } = "";

        /// <summary>
        /// The time zone the clock is being emulated in is located.
        /// </summary>
        public TimeZoneInfo TimeZone { get; init; } = TimeZoneInfo.Local;
    }

    public static class ClockBotInfoExtensions
    {
        public static ClockBotInfo ToClockBotInfo(
            this Pretzel.Logic.Templating.Context.Page page,
            SiteContext siteContext
        )
        {
            IDictionary<string, object> bag = page.Bag;

            string ReadDict( string key )
            {
                if( bag.ContainsKey( key ) == false )
                {
                    throw new KeyNotFoundException(
                        $"Could not find key for page {page.Id}: {key}"
                    );
                }

                string? value = bag[key]?.ToString();
                if( value is null )
                {
                    throw new InvalidOperationException(
                        $"Got null value for key '{key}' on page: {page.Id}"
                    );
                }

                return value;
            }

            Uri? TryReadUrl( string key, UriKind uriKind )
            {
                if( bag.ContainsKey( key ) == false )
                {
                    return null;
                }

                string? value = bag[key]?.ToString();
                if( value is null )
                {
                    return null;
                }

                return new Uri( value, uriKind );
            }

            string baseDir = Environment.GetEnvironmentVariable( "APP_BASE_KEY_DIRECTORY" ) ?? "";
            FileInfo publicKeyFile;
            if( string.IsNullOrWhiteSpace( baseDir ) )
            {
                publicKeyFile = new FileInfo( ReadDict( "publickey" ) );
            }
            else
            {
                publicKeyFile = new FileInfo(
                    Path.Combine( baseDir, ReadDict( "publickey" ) )
                );
            }

            string? dynamicUri = siteContext.Config["clockbotservice"].ToString();
            if( dynamicUri is null )
            {
                throw new KeyNotFoundException(
                    $"Could not find 'clockbotservice' in the site config"
                );
            }

            string userName = ReadDict( "username" );
            string baseDynamicUrl = siteContext.UrlCombine( $"{dynamicUri}/{userName}" );

            string baseStaticUrl = siteContext.UrlCombine( $"bots/clockbots/{userName}" );

            string iconUrl = siteContext.UrlCombine( ReadDict( "icon" ) );

            return new ClockBotInfo
            {
                Created = ProfileExtensions.ParseCreatedDate( ReadDict( "created" ) ),

                // Bot information is static.
                BotInfoUrl = new Uri( $"{baseStaticUrl}/index.html" ),

                // Followers is dyanmic, and we need inbox service.
                FollowersUrl = new Uri( $"{baseDynamicUrl}/followers.json" ),

                // Following we can generate statically with config.yml.
                FollowingUrl = new Uri( $"{baseStaticUrl}/following.json" ),
                FullName = ReadDict( "fullname" ),
                GitHub = TryReadUrl( "github", UriKind.Absolute ),
                IconPath = new Uri( iconUrl, UriKind.Absolute ),

                // Inbox needs to accept POST requests, must use inbox service.
                InboxUrl = new Uri( $"{baseDynamicUrl}/inbox.json" ),

                // Outbox is dynamic, must use service.
                OutboxUrl = new Uri( $"{baseDynamicUrl}/outbox.json" ),

                // Profile we can statically generate, put in static section.
                ProfileUrl = new Uri( $"{baseStaticUrl}/profile.json" ),
                PublicKeyContents = ProfileExtensions.ReadPublicKey( publicKeyFile.FullName ),
                Summary = ReadDict( "summary" ),
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById( ReadDict( "timezone" ) ),
                UserName = userName,
                WebFinger = $"{userName}@{siteContext.GetSiteUrlWithoutHttp()}",

                // web finger information is static.
                WebFingerUrl = new Uri( $"{baseStaticUrl}/webfinger.json" ),
                Website = TryReadUrl( "website", UriKind.Absolute ),
                Wikipedia = TryReadUrl( "wikipedia", UriKind.Absolute )
            };
        }

        public static Service ToService( this ClockBotInfo clockBot )
        {
            var extensionData = new Dictionary<string, JsonElement>();
            extensionData.AddMastodonExtensions();

            var profile = new Service
            {
                // ID must be the same as the URL
                // to this page (its a self-reference
                Id = clockBot.ProfileUrl?.ToString(),
                Type = new string[] { "Service" },
                Published = clockBot.Created,

                Inbox = new Link
                {
                    Href = clockBot.InboxUrl
                },
                Outbox = new Link
                {
                    Href = clockBot.OutboxUrl
                },
                Followers = new Link
                {
                    Href = clockBot.FollowersUrl
                },
                Following = new Link
                {
                    Href = clockBot.FollowingUrl
                },

                PreferredUsername = clockBot.UserName,
                Name = new string[]
                {
                    clockBot.FullName
                },
                Summary = new string[]
                {
                    clockBot.Summary
                },

                // The URL we'll make it go to more information about the bot.
                Url = new Link[]
                {
                    new Link
                    {
                        Href = clockBot.BotInfoUrl
                    }
                }
            };

            if( clockBot.IconPath is not null )
            {
                profile.AddIcon( clockBot.IconPath.ToString() );
            }

            var attachments = new List<IObjectOrLink>();
            if( clockBot.Website is not null )
            {
                attachments.Add( ServiceExtensions.CreateWebsiteAttachment( clockBot.GitHub?.ToString() ) );
            }

            if( clockBot.Wikipedia is not null )
            {
                attachments.Add(
                    new PropertyValue
                    {
                        Name = new string[] { "Wikipedia" },
                        Type = new string[] { "PropertyValue" },
                        Value = ServiceExtensions.GetAttachmentUrl( clockBot.Wikipedia?.ToString() )
                    }
                );
            }

            attachments.Add(
                new PropertyValue
                {
                    Name = new string[] { "Time Zone" },
                    Type = new string[] { "PropertyValue" },
                    Value = clockBot.TimeZone.DisplayName
                }
            );
            profile.Attachment = attachments;

            if( clockBot.GitHub is not null )
            {
                attachments.Add( ServiceExtensions.CreateGithubAttachment( clockBot.GitHub?.ToString() ) );
            }

            var key = new ProfilePublicKey
            {
                // ID Must match the Profile's ID.
                Id = $"{profile.Id}#main-key",
                Owner = profile.Id?.ToString(),
                PublicKeyPem = clockBot.PublicKeyContents
            };

            extensionData["publicKey"] = JsonSerializer.SerializeToElement( key );

            profile.ExtensionData = extensionData;

            return profile;
        }

        public static WebFinger ToWebFinger( this ClockBotInfo clockBot )
        {
            var webFingerLinks = new List<WebFingerLinks>
            {
                new WebFingerLinks
                {
                    Rel = "self",
                    Type = "application/activity+json",
                    Href = clockBot.ProfileUrl
                },
                new WebFingerLinks
                {
                    Rel = "http://webfinger.net/rel/profile-page",
                    Type = "text/html",
                    Href = clockBot.BotInfoUrl
                }
            };

            if( clockBot.IconPath is not null )
            {
                webFingerLinks.Add(
                    new WebFingerLinks
                    {
                        Rel = "http://webfinger.net/rel/avatar",
                        Type = $"image/{Path.GetExtension( clockBot.IconPath.ToString() ).TrimStart( '.' )}",
                        Href = clockBot.IconPath
                    }
                );
            }

            return new WebFinger
            {
                Subject = $"acct:{clockBot.WebFinger}",
                Links = webFingerLinks.ToArray()
            };
        }
    
        public static OrderedCollection ToFollowing( this ClockBotInfo currentBot, IList<string> following )
        {
            return new OrderedCollection
            {
                JsonLDContext = new ITermDefinition[]
                {
                    new ReferenceTermDefinition( new Uri( "https://www.w3.org/ns/activitystreams") )
                },
                OrderedItems = following.Select(
                    f => new Link
                    {
                        Href = new Uri( f )
                    }
                ).ToArray<IObjectOrLink>(),
                Type = new string[]{ "OrderedCollection" },
                TotalItems = (uint)following.Count,

                // ID should be a self-reference.
                Id = currentBot.FollowingUrl?.ToString()
            };
        }
    
    }
}

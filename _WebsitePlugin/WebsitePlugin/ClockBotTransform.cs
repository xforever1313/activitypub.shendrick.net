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

using System.Composition;
using System.Text.Json;
using KristofferStrube.ActivityStreams;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;

namespace WebsitePlugin
{
    [Export( typeof( IBeforeProcessingTransform ) )]
    public class ClockBotTransform : IBeforeProcessingTransform
    {
        public void Transform( SiteContext siteContext )
        {
            Console.WriteLine( "Creating Activitypub" );

            IEnumerable<ClockBotInfo> clockBots = siteContext.GetClockBotInformation();
            foreach( ClockBotInfo clockBotInfo in clockBots )
            {
                AddProfilePage( clockBotInfo, siteContext );
                AddWebFinger( clockBotInfo, siteContext );
                AddFollowing( clockBotInfo, clockBots, siteContext );
                AddHtaccess( clockBotInfo, siteContext );
            }
        }

        private void AddProfilePage( ClockBotInfo clockBotInfo, SiteContext siteContext )
        {
            var service = clockBotInfo.ToService();
            string jsonString = JsonSerializer.Serialize(
                service,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            var profilePage = new RawPage
            {
                Title = $"{clockBotInfo.UserName} Profile",
                Content = jsonString,
                File = Path.Combine( siteContext.GetClockBotInputStaticPath( clockBotInfo.UserName ), "profile.json" ),
                Filepath = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), "profile.json" ),
                OutputFile = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), "profile.json" ),
                Bag = new Dictionary<string, object>()
            };
            profilePage.Url = new LinkHelper().EvaluateLink( siteContext, profilePage );

            siteContext.Pages.Add( profilePage );
        }

        private void AddWebFinger( ClockBotInfo clockBotInfo, SiteContext siteContext )
        {
            var webFinger = clockBotInfo.ToWebFinger();
            string jsonString = JsonSerializer.Serialize(
                webFinger,
                  new JsonSerializerOptions
                  {
                      WriteIndented = true
                  }
            );

            var webFingerPage = new RawPage
            {
                Title = $"{clockBotInfo.UserName} Webfinger",
                Content = jsonString,
                File = Path.Combine( siteContext.GetClockBotInputStaticPath( clockBotInfo.UserName ), "webfinger.json" ),
                Filepath = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), "webfinger.json" ),
                OutputFile = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), "webfinger.json" ),
                Bag = new Dictionary<string, object>()
            };
            webFingerPage.Url = new LinkHelper().EvaluateLink( siteContext, webFingerPage );

            siteContext.Pages.Add( webFingerPage );
        }
    
        private void AddFollowing( ClockBotInfo currentBot, IEnumerable<ClockBotInfo> allBots, SiteContext siteContext )
        {
            var following = new List<string>();

            const string key = "additional_follows";
            if( siteContext.Config.ContainsKey( key ) )
            {
                IEnumerable<string>? additionalFollows = siteContext.Config[key] as IEnumerable<string>;
                if( additionalFollows is null )
                {
                    throw new ArgumentException( $"'{key}' must be a list type." );
                }

                following.AddRange( additionalFollows );
            }

            foreach( ClockBotInfo clockBot in allBots )
            {
                if( currentBot.WebFinger == clockBot.WebFinger )
                {
                    continue;
                }

                // Following seems to be the profile URL, use that.
                if( clockBot.ProfileUrl is not null )
                {
                    following.Add( clockBot.ProfileUrl.ToString() );
                }
            }

            var followingCollection = currentBot.ToFollowing( following );
            string jsonString = JsonSerializer.Serialize(
                followingCollection,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                } 
            );

            var followingPage = new RawPage
            {
                Title = $"{currentBot.UserName} following",
                Content = jsonString,
                File = Path.Combine( siteContext.GetClockBotInputStaticPath( currentBot.UserName ), "following.json" ),
                Filepath = Path.Combine( siteContext.GetClockBotOutputStaticPath( currentBot.UserName ), "following.json" ),
                OutputFile = Path.Combine( siteContext.GetClockBotOutputStaticPath( currentBot.UserName ), "following.json" ),
                Bag = new Dictionary<string, object>()
            };
            followingPage.Url = new LinkHelper().EvaluateLink( siteContext, followingPage );

            siteContext.Pages.Add( followingPage );
        }
    
        private void AddHtaccess( ClockBotInfo clockBotInfo, SiteContext siteContext )
        {
            const string content =
@"<Files webfinger.json>
    ForceType application/jrd+json
</Files>
<Files *.json>
    ForceType application/activity+json
</Files>
";

            var htaccessPage = new RawPage
            {
                Title = $"{clockBotInfo.UserName} htaccess",
                Content = content,
                File = Path.Combine( siteContext.GetClockBotInputStaticPath( clockBotInfo.UserName ), ".htaccess" ),
                Filepath = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), ".htaccess" ),
                OutputFile = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), ".htaccess" ),
                Bag = new Dictionary<string, object>()
            };
            htaccessPage.Url = new LinkHelper().EvaluateLink( siteContext, htaccessPage );

            siteContext.Pages.Add( htaccessPage );
        }
    }
}
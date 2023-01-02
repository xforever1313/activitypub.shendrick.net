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

            // Step 1: Get all pages that are of the bot type.
            //         We'll need to create the activitypub pages
            //         of them.

            foreach( ClockBotInfo clockBotInfo in siteContext.GetClockBotInformation() )
            {
                AddProfilePage( clockBotInfo, siteContext );
                AddWebFinger( clockBotInfo, siteContext );
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
    }
}
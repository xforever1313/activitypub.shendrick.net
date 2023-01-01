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

            var layoutsToGrab = new string[]
            {
                "clockbot"
            };

            IEnumerable<Page> botPages = siteContext.Pages.Where(
                p => p.Bag.ContainsKey( "layout" ) && layoutsToGrab.Contains( p.Bag["layout"].ToString() )
            ).ToArray();

            foreach( Page botPage in botPages )
            {
                ClockBotInfo clockBotInfo = botPage.ToClockBotInfo( siteContext );

                string pageContent = GetProfileJson( clockBotInfo );

                var profilePage = new RawPage
                {
                    Title = $"{clockBotInfo.UserName} Profile",
                    Content = pageContent,
                    File = Path.Combine( siteContext.GetClockBotInputStaticPath( clockBotInfo.UserName ), "profile.json" ),
                    Filepath = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), "profile.json" ),
                    OutputFile = Path.Combine( siteContext.GetClockBotOutputStaticPath( clockBotInfo.UserName ), "profile.json" ),
                    Bag = new Dictionary<string, object>()
                };
                profilePage.Url = new LinkHelper().EvaluateLink( siteContext, profilePage );

                siteContext.Pages.Add( profilePage );
            }
        }

        private string GetProfileJson( ClockBotInfo clockInfo )
        {
            var service = clockInfo.ToService();
            string jsonString = JsonSerializer.Serialize(
                service,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            return jsonString;
        }
    }
}
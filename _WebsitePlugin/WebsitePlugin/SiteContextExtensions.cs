﻿//
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

using Pretzel.Logic.Templating.Context;

namespace WebsitePlugin
{
    public static class SiteContextExtensions
    {
        public static string GetClockBotInputStaticPath( this SiteContext siteContext, string userName )
        {
            return Path.Combine(
                siteContext.SourceFolder,
                "bots",
                "clockbots",
                userName
            );
        }

        public static string GetClockBotOutputStaticPath( this SiteContext siteContext, string userName )
        {
            return Path.Combine(
                siteContext.OutputFolder,
                "bots",
                "clockbots",
                userName
            );
        }

        public static IEnumerable<ClockBotInfo> GetClockBotInformation( this SiteContext siteContext )
        {
            var layoutsToGrab = new string[]
            {
                "clockbot"
            };

            return siteContext.Pages.Where(
                p => p.Bag.ContainsKey( "layout" ) && layoutsToGrab.Contains( p.Bag["layout"].ToString() )
            ).Select(
                p => p.ToClockBotInfo( siteContext )
            ).ToArray();
        }
    }
}

---
layout: page
title: Clock bots
description: About the various Clock Bots that exist on this website.
tags: [bot, clock, list]
---
@using WebsitePlugin;

## Inspiration behind Clock Bots

Many years ago, I stumbled upon the Twitter account [@@big_ben_clock](https://twitter.com/big_ben_clock).  This account's premise is simple, every hour, it will tweet "BONG" equal to the hour; emulating the clock striking an hour and ringing bells in real life.

I don't live in London, so I ended up creating a similar Twitter account for the clock tower I my community college, Hudson Valley Community College.  Its Twitter account is [@@HVCC_Clock](https://twitter.com/HVCC_Clock).

Recently, I discovered the power of [ActivityPub](https://en.wikipedia.org/wiki/ActivityPub), the technology that powers [Mastodon](https://en.wikipedia.org/wiki/Mastodon_(social_network)), [PeerTube](https://en.wikipedia.org/wiki/PeerTube), and many other sites in the "[Fediverse](https://en.wikipedia.org/wiki/Fediverse)".

I wanted to recreate @@HVCC_Clock in Mastodon, and while I could have picked a Mastodon instance to host the bot, I wanted a challenge for myself.  So, instead, I recreated it in ActivityPub, so any website in the Fediverse can follow it.

But why stop at just one clock, when I can have multiple?  That's when I decided to expand and add more clock bots across multiple time zones.

I hope these clock bots give you some joy in your Fediverse feeds!  There's a list of who to follow below!

## List of Clock Bots

Simply copy the "Webfinger" column into the search function of your ActivityPub-supported application (such as Mastodon), and you can follow it!

| **Name** | **Webfinger** | **Location** | **TimeZone** |
| --- | --- | --- | --- |
@foreach( ClockBotInfo clockBot in Model.Site.GetClockBotInformation() ){
@:| [@(clockBot.get_FullName())](@clockBot.get_BotInfoUrl().LocalPath) | @@@(clockBot.get_WebFinger()) | @(clockBot.get_Location()) | @(clockBot.get_TimeZone()) |
}

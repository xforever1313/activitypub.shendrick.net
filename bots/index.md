---
layout: page
title: Bots
description: About the various Bots that exist on this website.
tags: [bot, list]
---

@using WebsitePlugin;

Here is a list of all the bots you can follow.  Simply copy the "Webfinger" column into the search function of your ActivityPub-supported application (such as Mastodon), and you can follow it!

## List of [Clock Bots](/bots/clockbots/)

| **Name** | **Webfinger** | **Location** | **TimeZone** |
| --- | --- | --- | --- |
@foreach( ClockBotInfo clockBot in Model.Site.GetClockBotInformation() ){
@:| [@(clockBot.get_FullName())](@clockBot.get_BotInfoUrl().LocalPath) | @@@(clockBot.get_WebFinger()) | @(clockBot.get_Location()) | @(clockBot.get_TimeZone()) |
}

---
layout: page
title: Privacy Policy
---

## Who we are

Our website address is: @Model.Site.Config["url"].

## What personal data we collect and why we collect it

### Access Logs

We do keep access logs.  This includes the IP address and user agent of who accessed the website.  You can not opt-out of this.  Access logs are kept for 5 days before being purged.

An example entry of the access log, where x.x.x.x is the IP address:

```txt
x.x.x.x - - [28/Dec/2020:12:46:09 -0800] "GET /robots.txt HTTP/1.1" 404 4394 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:84.0) Gecko/20100101 Firefox/84.0"
```

### Following List

If you choose to follow one of our bots, your ActivityPub user name is stored inside of a database.  This so if someone queries who is following a bot, we can produce a list of followers.  This means that if you follow a bot, your username becomes public.  If you do not want your username to be public, do not follow any of our bots.

### Cookies

This website does not produce any additional cookies.

### Embedded content from other websites

Pages on this site may include embedded content (e.g. videos, images, articles, etc.). Embedded content from other websites behaves in the exact same way as if the visitor has visited the other website.

These websites may collect data about you, use cookies, embed additional third-party tracking, and monitor your interaction with that embedded content, including tracking your interaction with the embedded content if you have an account and are logged in to that website.

### Analytics

We don't currently collect analytics.  However, embedded content from other sites (e.g. YouTube videos or Disqus) might for their own purposes.

### How long we retain your data

Access logs are kept for 5 days before being purged.  No other data is collected.

If you follow one of our bots, your ActivityPub username will be stored in a database until you unfollow the bot, in which case we'll do our best to remove it.  If you migrate ActivityPub instances _before_ unfollowing one of our bots, we can't guarantee your username will be removed from our database.  In which case, contact us and we'll see what we can do.

### What rights you have over your data

Other than a list of who is following our bots, we don't collect data that can be tied to an individual.  The closest thing is the IP address in the access logs.  IP Addresses can be spoofed, or behind a NAT, so we most likely couldn't tie it to a specific individual even if we tried.

### Where we send your data

No where.

### Contact information

Email: @Model.Site.Config["contact"]

### Additional information

None

### How we protect your data

The only data we keep is a list of who is following the bot.  This information is public anyways when one follows the bot, so there's no data to protect in this regard.

### What data breach procedures we have in place

There's no data _to_ breach.  This site is a static website, everything is out in the open.  You can even see the source code here: [@Model.Site.Config["github"]](@Model.Site.Config["github"]).

### What third parties we receive data from

None

### What automated decision making and/or profiling we do with user data

None

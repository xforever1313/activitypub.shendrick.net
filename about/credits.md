---
layout: page
title: Credits
description: Third-party and Open Source Credits
tags: [credits, foss, open, source]
---

There are two portions of this website, a "static" portion (things that rarely change) and a "dynamic" portion (such as ActivityPub outboxes and inboxes).  This page gives the third-party and open source licenses used to make the static website possible.

Credits for the dynamic portions of this site can be accessed with the links below:

* [Static Website Inbox Credits](/inboxservice/Credits/)
* [ClockBot Credits](/clockbots/Credits/)

@using Pretzel.Logic.Templating.Context
@Include( "credits.md", Model, typeof( PageContext ) )
@Include( "actpub_credits.md", Model, typeof( PageContext ) )

## dotenv.net

Used with cake to build website.

* **Code:** [https://github.com/bolorundurowb/dotenv.net](https://github.com/bolorundurowb/dotenv.net)
* **License:** [MIT](https://github.com/bolorundurowb/dotenv.net/blob/master/LICENSE)

```txt
MIT License

Copyright (c) 2017 Bolorunduro Winner-Timothy B

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

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

## Leaflet.js

Used for interactive maps.

* **Website:** [https://leafletjs.com/](https://leafletjs.com/)
* **Code:** [https://github.com/Leaflet/Leaflet/](https://github.com/Leaflet/Leaflet/)
* **License:** [BSD 2-Clause License](https://github.com/Leaflet/Leaflet/blob/main/LICENSE)

```txt
BSD 2-Clause License

Copyright (c) 2010-2022, Volodymyr Agafonkin
Copyright (c) 2010-2011, CloudMade
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
```

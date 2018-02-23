OAuth2VK.NET
=========

ASP MVC 4 Internet Project with Vkontakte Open Auth 2 Client

Client call VkontakteClient in App_Start directory. To setup it just register client with OAuthWebSecurity.RegisterClient method. Also you need to specify client id (vkontakte application id) client secret (application secret from settings page https://vk.com/apps?act=settings) and permission scope (you can user VkontakteClient.Permissions to semplify setup of permission). One more settings connect with domain name, to work properly vkontakte should know your domain name, type it in the settings form. That's it!

Licence
=========

Copyright (c) 2018 Vladimir Shmidt

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

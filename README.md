OAuth2VK.NET
=========

ASP MVC 4 Internet Project with Vkontakte Open Auth 2 Client

Client call VkontakteClient in App_Start directory. To setup it just register client with OAuthWebSecurity.RegisterClient method. Also you need to specify client id (vkontakte application id) client secret (application secret from settings page https://vk.com/apps?act=settings) and permission scope (you can user VkontakteClient.Permissions to semplify setup of permission). One more settings connect with domain name, to work properly vkontakte should know your domain name, type it in the settings form. That's it!


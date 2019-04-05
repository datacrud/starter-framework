
angular.module("app")
    .service("UrlService", [
        function () {
            "use strict";

            var self = this;

            self.url = "http://localhost:54652/";

            self.urls = [];
            self.urls.baseUrl = self.url;
            self.urls.baseApi = self.url + "api/";

            self.urls.TokenUrl = self.urls.baseUrl + "token";
            self.urls.AccountUrl = self.urls.baseApi + "Account";
            self.urls.RoleUrl = self.urls.baseApi + "Role";
            self.urls.ResourceUrl = self.urls.baseApi + "Resource";
            self.urls.PermissionUrl = self.urls.baseApi + "Permission";
            self.urls.ProfileUrl = self.urls.baseApi + "Profile";
            self.urls.UserUrl = self.urls.baseApi + "User";
            self.urls.HomeUrl = self.urls.baseApi + "Home";
            self.urls.DashboardUrl = self.urls.baseApi + "Dashboard";

            return self.urls;
        }
    ]);

angular.module("app")
    .service("LocalDataStorageService", [
        function () {
            "use strict";

            var app = "app_";

            var setToken = function (data) {
                var self = this;
                self.token = data.token_type + " " + data.access_token;
                localStorage.setItem(app + "token", self.token);
            };
            var getToken = function () {
                return (localStorage.getItem(app + "token") !== undefined) ? localStorage.getItem(app + "token") : undefined;
            };
            var deleteToken = function () {
                localStorage.removeItem(app + "token");
            };


            var setExpiresIn = function (date) {
                localStorage.setItem(app + "ExpiresIn", date);
            };
            var getExpiresIn = function () {
                return (localStorage.getItem(app + "ExpiresIn") !== undefined) ? localStorage.getItem(app + "ExpiresIn") : undefined;
            };
            var deleteExpiresIn = function () {
                localStorage.removeItem(app + "ExpiresIn");
            };


            var setUserInfo = function (user) {
                localStorage.setItem(app + "User", angular.toJson(user));
            };
            var getUserInfo = function () {
                return (localStorage.getItem(app + "User") !== undefined) ? angular.fromJson(localStorage.getItem(app + "User")) : undefined;
            };
            var deleteUserInfo = function () {
                localStorage.removeItem(app + "User");
            };


            var setUserRole = function (role) {
                localStorage.setItem(app + "UserRole", angular.toJson(role));
            };
            var getUserRole = function () {
                return (angular.fromJson(localStorage.getItem(app + "UserRole")) === undefined) ? undefined : angular.fromJson(localStorage.getItem("UserRole"));
            };
            var deleteUserRole = function () {
                localStorage.removeItem(app + "UserRole");
            };


            var logout = function () {
                deleteToken();
                deleteUserInfo();
                deleteExpiresIn();
                deleteUserRole();
            };


            return {
                setToken: setToken,
                getToken: getToken,
                deleteToken: deleteToken,

                setExpiresIn: setExpiresIn,
                getExpiresIn: getExpiresIn,
                deleteExpiresIn: deleteExpiresIn,

                setUserInfo: setUserInfo,
                getUserInfo: getUserInfo,
                deleteUserInfo: deleteUserInfo,

                setUserRole: setUserRole,
                getUserRole: getUserRole,
                deleteUserRole: deleteUserRole,

                logout: logout
            };
        }
    ]);
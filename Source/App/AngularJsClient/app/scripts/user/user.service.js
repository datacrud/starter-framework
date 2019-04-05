angular.module("app")
    .service("UserService", [
        "$q", "$http", "UrlService",
        function ($q, $http, urlService) {
            "use strict";

            var roles = function() {
                var self = this;
                self.deferred = $q.defer();

                var successCallBack = function(response) {
                    console.log(response);
                    self.deferred.resolve(response.data);
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.get(urlService.RoleUrl).then(successCallBack, errorCallback);

                return self.deferred.promise;
            };


            var users = function() {
                var self = this;
                self.deferred = $q.defer();

                var successCallBack = function(response) {
                    console.log(response);
                    self.deferred.resolve(response.data);
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.get(urlService.UserUrl + "/GetUsers").then(successCallBack, errorCallback);

                return self.deferred.promise;
            };


            var saveUser = function(model) {
                var self = this;
                self.deferred = $q.defer();

                var successCallback = function(response) {
                    console.log(response);
                    if (response.data.Result.Succeeded) {
                        self.deferred.resolve(response.data.Result.Succeeded);
                    } else {
                        self.deferred.reject(false);
                    }
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.post(urlService.UserUrl + "/CreateUser", JSON.stringify(model)).then(successCallback, errorCallback);

                return self.deferred.promise;
            };


            var getUser = function(id) {
                var self = this;
                self.deferred = $q.defer();

                var successCallBack = function(response) {
                    console.log(response);
                    self.deferred.resolve(response.data);
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.get(urlService.UserUrl + "/GetUser", { params: { id: id } }).then(successCallBack, errorCallback);

                return self.deferred.promise;
            };


            var updateUser = function(model) {
                var self = this;
                self.deferred = $q.defer();

                var successCallBack = function(response) {
                    console.log(response);
                    if (response.data.Result.Succeeded) {
                        self.deferred.resolve(response.data.Result.Succeeded);
                    } else {
                        self.deferred.reject(false);
                    }
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.put(urlService.UserUrl + "/UpdateUser", JSON.stringify(model)).then(successCallBack, errorCallback);

                return self.deferred.promise;
            };

            var removeUser = function(id) {
                var self = this;
                self.deferred = $q.defer();

                var successCallBack = function(response) {
                    console.log(response);
                    if (response.data.Result.Succeeded) {
                        self.deferred.resolve(response.data.Result.Succeeded);
                    } else {
                        self.deferred.reject(false);
                    }
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.delete(urlService.UserUrl + "/DeleteUser", { params: { id: id } }).then(successCallBack, errorCallback);

                return self.deferred.promise;
            };

            return {
                roles: roles,
                users: users,
                saveUser: saveUser,
                getUser: getUser,
                updateUser: updateUser,
                removeUser: removeUser
            };

        }
    ]);
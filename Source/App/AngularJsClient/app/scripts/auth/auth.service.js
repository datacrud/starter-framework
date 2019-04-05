angular.module("app")
    .service("AuthService", [
        "$q", "UrlService", "$http", "LocalDataStorageService",
        function ($q, urlService, $http, localDataStorageService) {
            "use strict";

            var userProfile = function() {
                var self = this;
                self.deferred = $q.defer();

                var success = function(response) {
                    console.log(response);
                    localDataStorageService.setUserInfo(response.data);
                    self.deferred.resolve(response.data);
                };
                var error = function(error) {
                    console.log(error);
                    self.deferred.reject(error);
                };
                $http.get(urlService.ProfileUrls.UserProfile).then(success, error);

                return self.deferred.promise;
            };


            var authenticate = function(model) {
                var self = this;
                self.deferred = $q.defer();

                Date.prototype.addDays = function(days) {
                    this.setDate(this.getDate() + parseInt(days));
                    return this;
                };

                self.transformRequest = function(obj) {
                    var str = [];
                    for (var p in obj)
                        if (obj.hasOwnProperty(p))str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                };

                self.successCallback = function(response) {
                    console.log(response);
                    if (response.status === 200) {
                        localDataStorageService.setToken(response.data);
                        localDataStorageService.setExpiresIn(new Date().addDays(13));
                        self.deferred.resolve(true);
                    } else {
                        self.deferred.reject(false);
                    }
                };

                self.errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(false);
                };

                self.httpAuthenticate = function() {
                    localDataStorageService.deleteToken();
                    $http({
                        method: "POST",
                        url: urlService.TokenUrl,
                        headers: { 'Content-Type': "application/x-www-form-urlencoded" },
                        transformRequest: self.transformRequest,
                        data: { username: model.Username, password: model.Password, grant_type: model.grant_type }
                    }).then(self.successCallback, self.errorCallback);
                };

                if (localDataStorageService.getToken() !== null) {
                    if (new Date(localDataStorageService.getExpiresIn()) < new Date()) {
                        localDataStorageService.logout();
                        self.deferred.reject(false);
                    } else {
                        self.deferred.resolve(true);
                    }

                } else {
                    if (model !== undefined) {
                        self.httpAuthenticate();
                    } else {
                        self.deferred.reject(false);
                    }
                }


                return self.deferred.promise;
            };


            var authorize = function(route) {
                var self = this;
                self.deferred = $q.defer();

                var successCallback = function(response) {
                    console.log(response);
                    if (response.status === 200) {
                        if (response.data !== true)
                            self.deferred.reject(false);
                        else
                            self.deferred.resolve(true);
                    }

                    self.deferred.reject(false);
                };
                var errorCallback = function(error) {
                    console.log(error);
                    self.deferred.reject(false);
                };
                self.requestModel = { Route: route };

                $http.post(urlService.PermissionUrls.CheckPermission, JSON.stringify(self.requestModel)).then(successCallback, errorCallback);

                return self.deferred.promise;
            };


            return {
                userProfile: userProfile,
                authenticate: authenticate,
                authorize: authorize
            };

        }
    ]);
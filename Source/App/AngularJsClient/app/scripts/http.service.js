angular.module("app")
    .service("HttpService", [
        "$q", "$http",
        function ($q, $http) {
            "use strict";

            var get = function (url) {
                var self = this;
                self.deferred = $q.defer();

                self.success = function (response) {
                    console.log(response);
                    if (response.status === 200) {
                        return self.deferred.resolve(response.data);
                    } else {
                        return self.deferred.reject(response);
                    }
                };
                self.error = function (error) {
                    console.log(error);
                    return self.deferred.reject(error);
                };
                $http.get(url).then(self.success, self.error);

                return self.deferred.promise;
            };

            var getByParams = function (url, data) {
                var self = this;
                self.deferred = $q.defer();

                self.success = function (response) {
                    console.log(response);
                    if (response.status === 200) {
                        return self.deferred.resolve(response.data);
                    } else {
                        return self.deferred.reject(response);
                    }
                };
                self.error = function (error) {
                    console.log(error);
                    return self.deferred.reject(error);
                };
                $http.get(url, { params: data }).then(self.success, self.error); //{params : data =~ {request : id}}

                return self.deferred.promise;
            };


            var add = function (url, data) {
                var self = this;
                self.deferred = $q.defer();

                self.success = function (response) {
                    console.log(response);
                    if (response.status === 200) {
                        return self.deferred.resolve(response.data);
                    } else {
                        return self.deferred.reject(response);
                    }
                };
                self.error = function (error) {
                    console.log(error);
                    return self.deferred.reject(error);
                };
                $http.post(url, JSON.stringify(data)).then(self.success, self.error);

                return self.deferred.promise;
            };


            var update = function (url, data) {
                var self = this;
                self.deferred = $q.defer();

                self.success = function (response) {
                    console.log(response);
                    if (response.status === 200) {
                        return self.deferred.resolve(response.data);
                    } else {
                        return self.deferred.reject(response);
                    }
                };
                self.error = function (error) {
                    console.log(error);
                    return self.deferred.reject(error);
                };
                $http.put(url, JSON.stringify(data)).then(self.success, self.error);

                return self.deferred.promise;
            };

            var remove = function (url, id) {
                var self = this;
                self.deferred = $q.defer();

                self.success = function (response) {
                    console.log(response);
                    if (response.status === 200) {
                        return self.deferred.resolve(response.data);
                    } else {
                        return self.deferred.reject(response);
                    }
                };
                self.error = function (error) {
                    console.log(error);
                    return self.deferred.reject(error);
                };
                $http.delete(url, { params: { request: id } }).then(self.success, self.error);

                return self.deferred.promise;
            };


            return {
                get: get,
                getByParams: getByParams,
                add: add,
                update: update,
                remove: remove
            };
        }
    ]);
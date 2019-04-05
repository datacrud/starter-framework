angular.module("app")
    .service("AppService", [
        "$q", "$http", "LocalDataStorageService",
        function($q, $http, localDataStorageService) {
            "use strict";

            var nextRoute = function() {
                var routeResponse = { ToState: "root.access-denied", IsLoggedIn: true, Broadcast: "loggedIn" };
                var user = localDataStorageService.getUserInfo();

                if (user === undefined) {
                    localDataStorageService.logout();
                    routeResponse = { ToState: "root.login", IsLoggedIn: false, Broadcast: "loggedOut" };
                } else if (user !== null) {
                    var flag = true;

                    for (var i = 0; i < user.RoleNames.length; i++) {

                        if (user.RoleNames[i] === "SystemAdmin") {
                            flag = false;
                            routeResponse.ToState = "root.dashboard";
                        } else if (user.RoleNames[i] === "Admin") {
                            flag = false;
                            routeResponse.ToState = "root.dashboard";
                        }
                    }

                    if (flag) routeResponse.ToState = "root.access-denied";
                }

                return routeResponse;
            };


            return {
                nextRoute: nextRoute
            };

        }
    ]);
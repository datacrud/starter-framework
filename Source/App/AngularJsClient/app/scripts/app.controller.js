angular.module("app")
    .controller("AppController", [
        "$scope", "$http", "AppService", "$rootScope", "$state", "LocalDataStorageService",
        function ($scope, $http, appService, $rootScope, $state, localDataStorageService) {
            "use strict";

            var init = function() {
                $scope.isLoggedIn = localDataStorageService.getToken();
                $scope.user = localDataStorageService.getUserInfo();
                if ($scope.user !== null) {
                    $scope.isAdminUser = ($scope.user.RoleNames[0] === "Admin") ? true : false;
                    $scope.isSystemAdminUser = ($scope.user.RoleNames[0] === "SystemAdmin") ? true : false;
                }
            };

            $scope.logout = function() {
                localDataStorageService.logout();
                $rootScope.$broadcast("loggedOut");
                $state.go("root.login");
            };


            $rootScope.$on("loggedIn", function (event, args) {
                console.log(event);
                init();
            });


            $rootScope.$on("loggedOut", function (event, args) {
                console.log(event);
                init();
            });


            $scope.defaultRoute = function() {
                var nextState = appService.nextRoute();
                $scope.isLoggedIn = nextState.IsLoggedIn;
                $rootScope.$broadcast(nextState.Broadcast);
                $state.go(nextState.ToState);
            };


            $scope.copyright = new Date();
            $scope.version = "Version: 0.0.1";
       
            init();
        }
    ]);
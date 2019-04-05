angular.module("app", ["ngAnimate", "ui.router", "ngGrid", "ui.bootstrap", "checklist-model", "cgBusy", "bsTable"])
    .run([
        "$rootScope", "$state", "$stateParams", "AuthService",
        function ($rootScope, $state, $stateParams, authService) {
            "use strict";

            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;

            $rootScope.alert = {
                type: null,
                msg: null
            };

            $rootScope.$on("$stateChangeStart", function(event, toState, toStateParams) {

                var isLogin = toState.name === "root.login";
                if (isLogin) return;

                var isAccessDenied = toState.name === "root.access-denied";
                if (isAccessDenied) return;


                var authorized = function() {
                    var success = function(isAuthorized) {
                        if (!isAuthorized) {
                            event.preventDefault();
                            $state.go("root.access-denied");
                        }
                    };
                    var error = function(error) {
                        console.log(error);
                        event.preventDefault();
                        $state.go("root.access-denied");
                    };
                    authService.authorize(toState.name).then(success, error);
                };


                var successCallback = function(isAuthenticated) {
                    if (isAuthenticated) {
                        authorized();
                    } else {
                        event.preventDefault();
                        $state.go("root.login");
                    }
                };
                var errorCallback = function(error) {
                    console.log(error);
                    event.preventDefault();
                    $state.go("root.login");
                };
                authService.authenticate().then(successCallback, errorCallback);
               

            });
        }
    ])
    .config([
        "$urlRouterProvider", "$stateProvider", "$httpProvider",
        function($urlRouterProvider, $stateProvider, $httpProvider) {

            $httpProvider.interceptors.push("tokenInterceptor");

            $urlRouterProvider.otherwise("/");

            $stateProvider
                .state("root", {
                    abstract: true,
                    url: "",
                    template: "<div ui-view class=\"container-fluid slide\"></div>",
                    controller: "AppController"
                })
                .state("root.about", {
                    url: "/about",
                    views: {
                        "": {
                            templateUrl: "app/views/about/about.tpl.html",
                            controller: "AboutController"
                        }
                    }
                })
                .state("root.contact", {
                    url: "/contact",
                    views: {
                        "": {
                            templateUrl: "app/views/contact/contact.tpl.html",
                            controller: "ContactController"
                        }
                    }
                });
        }
    ]);



angular.module("app")
    .factory("tokenInterceptor", [
        "LocalDataStorageService",
        function (localDataStorageService) {

            var haderInjector = {
                request: function (config) {
                    if (localDataStorageService.getToken() !== null)
                        config.headers["Authorization"] = localDataStorageService.getToken();

                    return config;
                }
            };

            return haderInjector;
        }
    ]);
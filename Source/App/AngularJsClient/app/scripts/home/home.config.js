angular.module("app")
    .config([
        "$stateProvider", "$urlRouterProvider",
        function ($stateProvider, $urlRouterProvider) {
            "use strict";

            $stateProvider
                .state("root.home", {
                    url: "/home",
                    views: {
                        "": {
                            templateUrl: "app/views/home/home.tpl.html",
                            controller: "HomeController"
                        }
                    }
                });
        }
    ]);
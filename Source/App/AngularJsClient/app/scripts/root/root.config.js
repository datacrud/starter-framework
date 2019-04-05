angular.module("app")
    .config([
        "$stateProvider", "$urlRouterProvider",
        function ($stateProvider, $urlRouterProvider) {
            "use strict";

            $stateProvider
                .state("root.root", {
                    url: "/",
                    views: {
                        "": {
                            templateUrl: "app/views/root/root.tpl.html",
                            controller: "RootController"
                        }
                    }
                });
        }
    ]);
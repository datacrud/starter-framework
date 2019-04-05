angular.module("app")
    .config([
        "$stateProvider", "$urlRouterProvider",
        function ($stateProvider, $urlRouterProvider) {
            "use strict";

            $stateProvider
                .state("root.dashboard", {
                    url: "/dashboard",
                    views: {
                        "": {
                            templateUrl: "app/views/dashboard/dashboard.tpl.html",
                            controller: "DashboardController"
                        }
                    }
                });
        }
    ]);
angular.module("app")
    .controller("RootController", [
        "$scope", "UrlService",
        function($scope, urlService) {
            "use strict";

            var init = function() {
                $scope.list = [];
                $scope.selections = [];

                $scope.loadGridData();
            };

            $scope.loadGridData = function() {
                $scope.list = [
                    {
                        Name: " Test",
                        Position: "Test",
                        Office: "Test",
                        Age: "40",
                        StartDate: new Date(),
                        Salary: 50000
                    }
                ];
            };


            init();
        }
    ]);
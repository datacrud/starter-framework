angular.module("app")
    .controller("DashboardController", [
        "$scope", "UrlService",
        function($scope, urlService) {
            "use strict";

            var init = function() {
                $scope.list = [];
                $scope.selections = [];

                $scope.loadGridData();
            };


            $scope.loadGridData = function() {
                $scope.myData = [
                    {
                        Id: 1,
                        Date: new Date().toLocaleDateString(),
                        Status: "Initialize"
                    }
                ];
            };


            var columnDefs = [
                {
                    field: "Id",
                    displayName: "Id",
                    cellTemplate: "<div  ng-click=\"detail(row.entity)\" style=\"padding-left: 5px\" ng-bind=\"row.getProperty(col.field)\"></div>"
                },
                { field: "Date", displayName: "Date" },
                { field: "Status", displayName: "Status" }
            ];

            $scope.gridOptions = {
                data: "myData",
                columnDefs: columnDefs,
                selectedItems: $scope.selections,
                multiSelect: false,
                enableRowSelection: true
                //enableCellSelection: true,
                //enablePinning: true,
            };


            $scope.detail = function(row) {
                $state.go("root.dashboard.detail", { id: row.Id });
            };


            init();
        }
    ]);
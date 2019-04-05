angular.module("app")
    .controller("ConfirmModalInstanceController", [
        "$scope", "$uibModalInstance", "action", "data",
        function($scope, $uibModalInstance, action, data) {
            "use strict";

            $scope.action = action;
            $scope.data = data;

            $scope.response = {
                action: $scope.action,
                data: $scope.data,
                isConfirm: true
            };

            $scope.ok = function() {
                $uibModalInstance.close($scope.response);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss("cancel");
            };
        }
    ]);
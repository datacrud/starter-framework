angular.module("app")
    .controller("RoleController", [
        "$scope", "RoleService", "UrlService", "HttpService", "AlertService",
        function ($scope, roleService, urlService, httpService, alertService) {
            "use strict";

            var init = function () {
                $scope.promise = null;
                $scope.model = { Id: "", Name: "" };
                $scope.list = [];
                $scope.isUpdateMode = false;
                $scope.loadRoles();
            };


            $scope.loadRoles = function() {
                $scope.promise = httpService.get(urlService.RoleUrl).then(function (data) {
                    console.log(data);
                    $scope.list = data;
                }, function(error) {
                    console.log(error);
                    alertService.showAlert(alertService.alertType.success, "Roles load failed, Please refresh the page again or check your internet connection", true);
                });
            };

            $scope.save = function() {
                if ($scope.isUpdateMode) $scope.update();

                else {
                    $scope.promise = httpService.add(urlService.RoleUrl, $scope.model).then(function (data) {
                        console.log(data);
                        alertService.showAlert(alertService.alertType.success, "Success", false);
                        init();
                    }, function(error) {
                        alertService.showAlert(alertService.alertType.danger, "Failed, role already exist, please try again with diffrent one", true);
                        console.log(error);
                    });
                }
            };

            $scope.loadRole = function(id) {
                $scope.promise = httpService.getByParams(urlService.RoleUrl, { request: id }).then(function (data) {
                    console.log(data);
                    $scope.model = data;
                }, function(error) {
                    console.log(error);
                    alertService.showAlert(alertService.alertType.success, "Role load failed, Please try again or check your internet connection", true);
                });
            };

            $scope.edit = function(id) {
                $scope.loadRole(id);
                $scope.isUpdateMode = true;
            };


            $scope.update = function() {
                $scope.promise = httpService.update(urlService.RoleUrl, $scope.model).then(function (data) {
                    console.log(data);
                    init();
                    alertService.showAlert(alertService.alertType.success, "Success", false);
                }, function(error) {
                    console.log(error);
                    alertService.showAlert(alertService.alertType.success, "Role update failed, Please try again!", true);
                });
            };

            $scope.delete = function(id) {
                $scope.promise = httpService.remove(urlService.RoleUrl, id).then(function(data) {
                    console.log(data);
                    init();
                    alertService.showAlert(alertService.alertType.success, "Success", false);
                }, function(error) {
                    console.log(error);
                    alertService.showAlert(alertService.alertType.success, "Role delete failed, Please try again!", true);
                });
            };

            $scope.remove = function(size, data, action) {
                alertService.showConfirmDialog(size, data, action, false).then(function(response) {
                    console.log(response);
                    if (response.isConfirm) {
                        $scope.delete(response.data.Id);
                    }
                }, function(error) {
                    console.log(error);
                });
            };

            $scope.cancel = function() {
                init();
            };

            init();
        }
    ]);

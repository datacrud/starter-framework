angular.module("app")
    .controller("PermissionController", [
        "$scope", "UrlService", "HttpService", "AlertService",
        function ($scope, urlService, httpService, alertService) {
            "use strict";

            var init = function () {
                $scope.promise = null;
                $scope.list = [];
                $scope.resources = [];
                $scope.roles = [];
                $scope.selectedRole = "";
                $scope.rolePermissions = [];
                $scope.permission = { Resources: [] };
                $scope.isPublicEnum = { True: true, False: false };
                $scope.isUpdateMode = false;
                $scope.loadRoles();
            };

            $scope.loadRoles = function() {
                $scope.promise = httpService.get(urlService.RoleUrl).then(function (data) {
                    console.log(data);
                    $scope.roles = data;
                    $scope.loadResources();
                }, function(error) {
                    console.log(error);
                });
            };

            $scope.loadResources = function() {
                $scope.promise = httpService.get(urlService.ResourceUrl + "/GetPrivateResources").then(function (data) {
                    console.log(data);
                    $scope.resources = data;
                }, function(error) {
                    console.log(error);
                });
            };

            $scope.save = function() {
                if ($scope.selectedRole === "") alertService.showAlert(alertService.alertType.warning, "Please select a role first", true);

                else {
                    $scope.createList();
                    $scope.promise = httpService.add(urlService.PermissionUrl + "/AddList", $scope.list).then(function (data) {
                        console.log(data);
                        $scope.loadRolePermissions($scope.selectedRole);
                        alertService.showAlert(alertService.alertType.success, "Permissions modification of " + $scope.selectedRole.Name + " role succeed.", false);
                    }, function(error) {
                        console.log(error);
                        alertService.showAlert(alertService.alertType.danger, "Permissions modification of " + $scope.selectedRole.Name + " role failed.", true);
                    });
                }
            };

            $scope.createList = function() {
                $scope.list = [];
                for (var i = 0; i < $scope.permission.Resources.length; i++) {
                    var permission = { RoleId: $scope.selectedRole.Id, RoleName: $scope.selectedRole.Name, ResourceId: $scope.permission.Resources[i].Id };
                    $scope.list.push(permission);
                }

                if ($scope.list.length === 0) {
                    $scope.list.push({ RoleId: $scope.selectedRole.Id, RoleName: $scope.selectedRole.Name, ResourceId: "00000000-0000-0000-0000-000000000000" });
                }
            };


            $scope.loadRolePermissions = function(role) {
                $scope.selectedRole = role;
                $scope.promise = httpService.getByParams(urlService.PermissionUrl + "/GetListById", { request: role.Id }).then(function (data) {
                    console.log(data);
                    $scope.rolePermissions = data;
                    $scope.permission.Resources = [];
                    $scope.bindRolePermissions();
                    $scope.isUpdateMode = true;
                }, function(error) {
                    console.log(error);
                });
            };


            //$scope.selection = [];
            //$scope.toggleSelection = function (resource) {
            //    var idx = $scope.selection.indexOf(resource);

            //    if (idx > -1) {
            //        $scope.selection.splice(idx, 1);
            //    }
            //    else {
            //        $scope.selection.push(resource);
            //    }
            //};

            $scope.bindRolePermissions = function() {
                for (var i = 0; i < $scope.rolePermissions.length; i++) {
                    $scope.permission.Resources.push($scope.rolePermissions[i].Resource);
                }
            };         

            $scope.checkAll = function () {
                if ($scope.selectedRole === "")
                    alertService.showAlert(alertService.alertType.warning, "Please select a role fist", true);
                else
                    $scope.permission.Resources = angular.copy($scope.resources);
            };
            $scope.uncheckAll = function () {
                if ($scope.selectedRole === "")
                    alertService.showAlert(alertService.alertType.warning, "Please select a role fist", true);
                $scope.permission.Resources = [];
            };

            $scope.cancel = function () {
                init();
            };

            init();
        }
    ]);
angular.module("app")
    .controller("UserController", [
        "$scope", "UserService", "AlertService",
        function ($scope, userService, alertService) {
            "use strict";

            var init = function () {
                $scope.promise = "";
                $scope.model = { FirstName: "", LastName: "", Email: "", PhoneNumber: "", UserName: "", Password: "", RetypePassword: "", RoleId: "" };
                $scope.list = [];
                $scope.roles = [];
                $scope.isUpdateMode = false;
                $scope.isChangedPassword = false;
                $scope.getRoles();
            };

            $scope.getRoles = function() {
                var success = function(response) {
                    console.log(response);
                    $scope.roles = response;
                    $scope.getUsers();
                };
                var error = function(error) {
                    console.log(error);
                    $scope.roles = [];
                };
                userService.roles().then(success, error);
            };

            $scope.getRole = function(id) {
                for (var i = 0; i < $scope.roles.length; i++) {
                    if ($scope.roles[i].Id === id)
                        return $scope.roles[i];
                }
            };

            $scope.getUsers = function() {
                var success = function(response) {
                    console.log(response);
                    $scope.list = response;
                };
                var error = function(error) {
                    console.log(error);
                    $scope.list = [];
                };
                $scope.promise = userService.users().then(success, error);
            };


            $scope.save = function() {
                if (!$scope.isUpdateMode) {
                    var success = function(response) {
                        console.log(response);
                        init();
                        alertService.showAlert(alertService.alertType.success, "Success", false);
                    };
                    var error = function(error) {
                        console.log(error);
                        alertService.showAlert(alertService.alertType.danger, "Failed!, Please, try again or refresh you page  or check your internet connection", true);
                    };
                    if ($scope.model.PasswordHash === $scope.model.RetypePassword) {
                        $scope.promise = userService.saveUser($scope.model).then(success, error);
                    } else {
                        alertService.showAlert(alertService.alertType.warning, "Password and Retype Password Could not match.", true);
                    }
                } else {
                    $scope.update();
                }
            };


            $scope.edit = function(id) {
                $scope.isUpdateMode = true;
                $scope.getUser(id);
            };


            $scope.getUser = function(id) {
                var success = function(response) {
                    console.log(response);
                    $scope.model = response;
                    $scope.model.PasswordHash = "";
                    $scope.model.RetypePassword = "";
                    $scope.model.RoleId = response.Roles[0].RoleId;
                };
                var error = function(error) {
                    console.log(error);
                    init();
                };
                $scope.promise = userService.getUser(id).then(success, error);
            };


            $scope.update = function() {
                var success = function(response) {
                    console.log(response);
                    init();
                    alertService.showAlert(alertService.alertType.success, "Success", false);
                };
                var error = function(error) {
                    console.log(error);
                    alertService.showAlert(alertService.alertType.danger, "Failed!, Please, try again or refresh you page  or check your internet connection", true);
                };

                if ($scope.model.PasswordHash === $scope.model.RetypePassword) {
                    $scope.promise = userService.updateUser($scope.model).then(success, error);
                } else {
                    alertService.showAlert(alertService.alertType.warning, "Password and Retype Password Could not match.", true);
                }
            };


            $scope.delete = function(id) {
                var success = function(response) {
                    console.log(response);
                    init();
                    alertService.showAlert(alertService.alertType.success, "Success", false);
                };
                var error = function(error) {
                    console.log(error);
                    alertService.showAlert(alertService.alertType.danger, "Failed!, Please, try again or refresh you page  or check your internet connection", true);
                };
                $scope.promise = userService.removeUser(id).then(success, error);
            };

            $scope.remove = function(size, data, action) {
                data.Name = data.UserName; //there has no default name property, so, custom name property added
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

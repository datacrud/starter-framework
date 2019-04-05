
angular.module("app")
    .service("AlertService", [
        "$rootScope", "$timeout", "$q", "$uibModal", "$log",
        function ($rootScope, $timeout, $q, $uibModal, $log) {
            "use strict";

            var alert = function (isAlert, type, msg, autoHide) {
                this.isAlert = isAlert;
                this.type = type;
                this.msg = msg;
                this.autoHide = autoHide;
                this.close = function () {
                    this.isAlert = false;
                };
            };

            this.showAlert = function (type, msg, closable) {
                $rootScope.alert = new alert(true, type, msg, !closable);
                if (!closable) {
                    $timeout(function () {
                        $rootScope.alert.close();
                    }, 2000);
                }
            };

            this.closeAlert = function () {
                $rootScope.alert.close();
            };

            this.alertType = {
                warning: "warning",
                success: "success",
                error: "error",
                info: "info",
                danger: "danger"
            };


            this.showConfirmDialog = function (size, data, action, configuration) {
                var self = this;
                self.deferred = $q.defer();

                if (!configuration)
                    configuration = {
                        template: "app/views/modal/confirm.modal.tpl.html",
                        controller: "ConfirmModalInstanceController"
                    };

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: configuration.template,
                    controller: configuration.controller,
                    size: size,
                    resolve: {
                        action: function () {
                            return action;
                        },
                        data: function () {
                            return data;
                        }
                    }
                });

                modalInstance.result.then(function (modalResponse) {
                    self.deferred.resolve(modalResponse);
                }, function () {
                    $log.info("Modal dismissed at: " + new Date());
                    self.deferred.reject(false);
                });

                return self.deferred.promise;
            };

            this.actionType = {
                add: "add",
                save: "save",
                change: "change",
                edit: "edit",
                modify: "modify",
                update: "update",
                'delete': "delete",
                remove: "remove",
                load: "load",
                get: "get"
            };

            return {
                showAlert: this.showAlert,
                closeAlert: this.closeAlert,
                alertType: this.alertType,
                showConfirmDialog: this.showConfirmDialog,
                actionType: this.actionType
            };
        }
    ]);
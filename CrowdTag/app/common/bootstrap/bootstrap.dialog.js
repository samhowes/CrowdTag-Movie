(function () {
    'use strict';

    var bootstrapModule = angular.module('common.bootstrap', ['ui.bootstrap']);

    bootstrapModule.factory('bootstrap.dialog', ['$modal', '$q', '$templateCache', modalDialog]);

    function modalDialog($modal, $q, $templateCache) {
        var service = {
            deleteDialog: deleteDialog,
            confirmationDialog: confirmationDialog,
            entityCreatorDialog: entityCreatorDialog
        };

        $templateCache.put('modalDialog.tpl.html', 
            '<div>' +
            '    <div class="modal-header">' +
            '        <button type="button" class="close" data-dismiss="modal" aria-hidden="true" data-ng-click="cancel()">&times;</button>' +
            '        <h3>{{title}}</h3>' +
            '    </div>' +
            '    <div class="modal-body">' +
            '        <p>{{message}}</p>' +
            '    </div>' +
            '    <div class="modal-footer">' +
            '        <button class="btn btn-primary" data-ng-click="ok()">{{okText}}</button>' +
            '        <button class="btn btn-info" data-ng-click="cancel()">{{cancelText}}</button>' +
            '    </div>' +
            '</div>');

        return service;

        function deleteDialog(itemName) {
            var title = 'Confirm Delete';
            itemName = itemName || 'item';
            var msg = 'Delete ' + itemName + '?';

            return confirmationDialog(title, msg);
        }

        function entityCreatorDialog(entity, entityName, data) {

            var modalOptions = {
                templateUrl: String.format('/app/common/bootstrap/{0}Dialog.html', entityName),
                controller: EntityCreator,
                keyboard: true,
                resolve: {
                    data: function () {
                        var promiseList = [];
                        for (var prop in data) {
                            var promise = $q.when(data[prop])
                                .then(set(data,prop));
                            promiseList.push(promise);
                        }
                        return $q.all(promiseList)
                            .then(function(promise) {
                                return data;
                            });

                        function set(object, prop) {
                            return function(results) {
                                object[prop] = results;
                            }
                        }
                    },
                    entity: function () {
                        return entity;
                    },
                    options: function() {
                        return {
                            title: 'Add ingredient',
                            message: 'Select an ingredient to add',
                            okText: 'Save & Close',
                            cancelText: 'Cancel'
                        };
                    }
                }
            };

            return $modal.open(modalOptions).result;
        }

        function confirmationDialog(title, msg, okText, cancelText) {

            var modalOptions = {
                templateUrl: 'modalDialog.tpl.html',
                controller: ModalInstance,
                keyboard: true,
                resolve: {
                    options: function () {
                        return {
                            title: title,
                            message: msg,
                            okText: okText,
                            cancelText: cancelText
                        };
                    }
                }
            };

            return $modal.open(modalOptions).result; 
        }
    }

    var ModalInstance = ['$scope', '$modalInstance', 'options',
        function ($scope, $modalInstance, options) {
            $scope.title = options.title || 'Title';
            $scope.message = options.message || '';
            $scope.okText = options.okText || 'OK';
            $scope.cancelText = options.cancelText || 'Cancel';
            $scope.ok = function () { $modalInstance.close('ok'); };
            $scope.cancel = function () { $modalInstance.dismiss('cancel'); };
        }];

    EntityCreator.$inject = ['$scope', '$modalInstance', 'data', 'entity', 'options'];

    function EntityCreator($scope, $modalInstance, data, entity, options) {
        $scope.title = options.title || 'Title';
        $scope.message = options.message || '';
        $scope.okText = options.okText || 'OK';
        $scope.cancelText = options.cancelText || 'Cancel';
        $scope.ok = function() { $modalInstance.close('ok'); };
        $scope.cancel = function () { $modalInstance.dismiss('cancel'); };

        $scope.entity = entity;
        $scope.data = data;
    }
})();
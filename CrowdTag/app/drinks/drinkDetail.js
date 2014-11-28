(function () {
    'use strict';

    var controllerId = 'drinkDetail';

    angular
        .module('app')
        .controller(controllerId, drinkDetail);

    drinkDetail.$inject = ['$location', '$scope', '$routeParams', '$window', 'common', 'config', 'datacontext'];

    function drinkDetail($location, $scope, $routeParams, $window, common, config, datacontext) {
        /* jshint validthis:true */
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'error');

        vm.goBack = goBack;
        vm.cancel = cancel;
        vm.canSave = canSave();
        vm.isSaving = false;
        vm.save = save; 
        vm.deleteDrink = deleteDrink; 
        vm.hasChanges = false;
        vm.drink = undefined;   
        vm.title = controllerId;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        activate();

        function activate() {
            onHasChanges();
            common.activateController([getRequestedDrink()], controllerId);
        }

        function cancel() {
            goBack();
        }

        function canSave() {return vm.hasChanges && !vm.isSaving;}

        function deleteDrink() {
            //TODO fill this in
        }

        function getRequestedDrink() {
            var val = $routeParams.id;

            if (val === 'new') {
                vm.drink = datacontext.createDrink(); //todo
                return vm.drink;
            }

            return datacontext.getDrinkById(val)
                .then(function(data) {
                    vm.drink = data.entity || data;
                    return vm.drink;
                }, function(error) {
                    logError('Unable to get the drink: ' + val);
                    gotoDrinks(); 
                });
        }

        function goBack() {
            datacontext.cancel();
            $window.history.back();
        }

        function gotoDrinks() { $location.path('/drinks'); }

        function onHasChanges() {
            $scope.$on(config.events.hasChangesChanged,
                function(event, data) {
                    vm.hasChanges = data.hasChanges;
                });
        }

        function save() {
            if (!canSave()) { return $q.when(null); }

            vm.isSaving = true;
            return datacontext.saveDrink(vm.drink)
                .then(function(saveResult) {
                    vm.isSaving = false;
                    logSuccess('Save successful!', saveResult, true);
                }, function (error) {
                    vm.isSaving = false;    
                    logError('Error while saving', error, true);
                });
        }
    }
})();

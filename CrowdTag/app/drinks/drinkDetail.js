(function () {
    'use strict';

    var controllerId = 'drinkDetail';

    angular
        .module('app')
        .controller(controllerId, drinkDetail);

    drinkDetail.$inject = ['$location', '$scope', '$routeParams', '$window', 'bootstrap.dialog', 'common', 'config', 'datacontext', 'model'];

    function drinkDetail($location, $scope, $routeParams, $window, bsDialog, common, config, datacontext, model) {
        /* jshint validthis:true */
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var lookups = datacontext.lookupCachedData;
        var entityNames = model.entityNames;

        vm.addIngredient = addIngredient;
        vm.goBack = goBack;
        vm.cancel = cancel;
        vm.canSave = canSave();
        vm.isSaving = false;
        vm.removeIngredient = removeIngredient;
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
            onDestroy();
            common.activateController([getRequestedDrink()], controllerId);
        }

        function addIngredient() {

            var entityName = entityNames.ingredientApplication;
            var entity = datacontext.createEntity(entityName);

            var data = {
                ingredients: datacontext.getIngredients(),
                lookups: {
                    measurementTypes: lookups.measurementTypes
                }
            };

            return bsDialog.entityCreatorDialog(entity, entityName, data)
                .then(success, failed);

            function success() {
                entity.drink = vm.drink;
            }

            function failed() {
                datacontext.markDeleted(entity);
            }
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

        function onDestroy() {
            $scope.$on('$destroy', function() {
                datacontext.cancel();
            });
        }

        function onHasChanges() {
            $scope.$on(config.events.hasChangesChanged,
                function(event, data) {
                    vm.hasChanges = data.hasChanges;
                });
        }

        function removeIngredient(ingredientApplication) {
            var text = 'Ingredient: ' + ingredientApplication.ingredient.name;
            return bsDialog.deleteDialog(text)
                .then(confirmDelete);

            function confirmDelete() {
                ingredientApplication.drink = null; // hide from the view
                datacontext.markDeleted(ingredientApplication);
            }
        }

        function save() {
            if (!canSave()) { return $q.when(null); }

            vm.isSaving = true;
            return datacontext.save()
                .then(function(saveResult) {
                    vm.isSaving = false;
                }, function (error) {
                    vm.isSaving = false;
                });
        }
    }
})();

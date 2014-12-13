(function () {
    'use strict';

    var controllerId = 'ingredientDetail';

    angular
        .module('app')
        .controller(controllerId, ingredientDetail);

    ingredientDetail.$inject = ['$location', '$scope', '$routeParams', '$window', 'bootstrap.dialog', 'common', 'config', 'datacontext', 'model'];

    function ingredientDetail($location, $scope, $routeParams, $window, bsDialog, common, config, datacontext, model) {
        /* jshint validthis:true */
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var lookups = datacontext.lookupCachedData;
        var entityNames = model.entityNames;

        //vm.addIngredient = addIngredient;
        vm.goBack = goBack;
        vm.cancel = cancel;
        vm.canSave = canSave();
        vm.isSaving = false;
        //vm.removeIngredient = removeIngredient;
        vm.save = save; 
        vm.deleteIngredient = deleteIngredient;
        vm.hasChanges = false;
        vm.ingredient = undefined;
        vm.title = controllerId;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        activate();

        function activate() {
            onHasChanges();
            onDestroy();
            common.activateController([getRequestedIngredient()], controllerId);
        }

        //function addIngredient() {

        //    var entityName = entityNames.ingredientApplication;
        //    var entity = datacontext.createEntity(entityName);

        //    var data = {
        //        ingredients: datacontext.getIngredients(),
        //        lookups: {
        //            measurementTypes: lookups.measurementTypes
        //        }
        //    };

        //    return bsDialog.entityCreatorDialog(entity, entityName, data)
        //        .then(success, failed);

        //    function success() {
        //        entity.drink = vm.drink;
        //    }

        //    function failed() {
        //        datacontext.markDeleted(entity);
        //    }
        //}

        function cancel() {
            goBack();
        }

        function canSave() {return vm.hasChanges && !vm.isSaving;}

        function deleteIngredient() {
            //TODO fill this in
        }

        function getRequestedIngredient() {
            var val = $routeParams.id;

            if (val === 'new') {
                vm.ingredient = datacontext.createIngredient(); //todo create this
                return vm.ingredient;
            }

            return datacontext.getIngredientById(val)
                .then(function(data) {
                    vm.ingredient = data.entity || data;
                    return vm.ingredient;
                }, function(error) {
                    logError('Unable to get the ingredient: ' + val);
                    gotoIngredients();
                });
        }

        function goBack() {
            datacontext.cancel();
            $window.history.back();
        }

        function gotoIngredients() { $location.path('/ingredients'); }

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

        //function removeIngredient(ingredientApplication) {
        //    var text = 'Ingredient: ' + ingredientApplication.ingredient.name;
        //    return bsDialog.deleteDialog(text)
        //        .then(confirmDelete);

        //    function confirmDelete() {
        //        ingredientApplication.drink = null; // hide from the view
        //        datacontext.markDeleted(ingredientApplication);
        //    }
        //}

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

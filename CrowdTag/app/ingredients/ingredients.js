(function () {
    'use strict';

    var controllerId = 'ingredients';

    angular
        .module('app')
        .controller(controllerId, drinks);

    drinks.$inject = ['$location', 'common', 'config', 'datacontext'];

    function drinks($location, common, config, datacontext) {
        /* jshint validthis:true */
        var vm = this;
        var log = common.logger.getLogFn(controllerId);
        var lookups = datacontext.lookupCachedData;
        var keyCodes = config.keyCodes;

        //Bindables
        vm.gotoIngredient = gotoIngredient;
        vm.title = 'Ingredients';
        vm.search = search;      
        vm.ingredientSearch = '';
        vm.ingredients = [];
        vm.ingredientCategories = lookups.ingredientCategories; //todo: only get non-nullos
        vm.filteredIngredients = [];
        vm.refresh = refresh;   

        activate();

        function activate() {
            common.activateController([getIngredients()], controllerId)
                .then(function() { log('Activated Ingredients view'); });
        }

        function applyFilter() {
            //TODO do this for real
            vm.filteredIngredients = vm.ingredients;
        }

        function getIngredients(forceRefresh) {
            return datacontext
                .getIngredients(forceRefresh) //todo
                .then(function(data) {
                    vm.ingredients = data;
                    applyFilter();
                    return vm.ingredients;
                });
        }

        function gotoIngredient(ingredient) {
            if (ingredient && ingredient.id) {
                $location.path('/ingredients/' + ingredient.id);
            }
        }

        function refresh() { getIngredients(true); }

        function search($event) {
            if ($event.keyCode === keyCodes.esc) {
                vm.ingredientSearch = '';
            }

            applyFilter();
        }
    }
})();

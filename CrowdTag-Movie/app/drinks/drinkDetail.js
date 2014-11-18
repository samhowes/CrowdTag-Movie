(function () {
    'use strict';

    var controllerId = 'drinkDetail';

    angular
        .module('app')
        .controller(controllerId, drinkDetail);

    drinkDetail.$inject = ['$location', '$routeParams', '$window', 'common', 'datacontext'];

    function drinkDetail($location, $routeParams, $window, common, datacontext) {
        /* jshint validthis:true */
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');

        vm.goBack = goBack;
        vm.cancel = cancel;
        vm.save = save; 
        vm.deleteDrink = deleteDrink; 
        vm.hasChanges = false;
        vm.drink = undefined;   
        vm.title = controllerId;

        activate();

        function activate() {
            common.activateController([getRequestedDrink()], controllerId);
        }

        function cancel() {
            goBack();
        }

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

        function goBack() { $window.history.back(); }

        function gotoDrinks() { $location.path('/drinks'); }

        function save() {
            //TODO: Fill this in
        }
    }
})();

(function () {
    'use strict';

    var controllerId = 'drinks';

    angular
        .module('app')
        .controller(controllerId, drinks);

    drinks.$inject = ['$location', 'common', 'config', 'datacontext'];

    function drinks($location, common, config, datacontext) {
        /* jshint validthis:true */
        var vm = this;
        var log = common.logger.getLogFn(controllerId);
        var keyCodes = config.keyCodes;

        //Bindables
        vm.gotoDrink = gotoDrink; 
        vm.title = 'Drinks';
        vm.search = search;      
        vm.drinkSearch = '';
        vm.drinks = [];
        vm.filteredDrinks = [];
        vm.refresh = refresh;   

        activate();

        function activate() {
            common.activateController([getDrinks()], controllerId)
                .then(function() { log('Activated Drinks view'); });
        }

        function applyFilter() {
            //TODO do this for real
            vm.filteredDrinks = vm.drinks;
        }

        function getDrinks(forceRefresh) {
            return datacontext.getDrinks(forceRefresh).then(function(data) {
                vm.drinks = data;
                applyFilter(); 
                return vm.drinks;
            });
        }

        function gotoDrink(drink) {
            if (drink && drink.iD) {
                $location.path('/drinks/' + drink.iD);
            }
        }

        function refresh() { getDrinks(true); }

        function search($event) {
            if ($event.keyCode === keyCodes.esc) {
                vm.speakerSearch = '';
            }

            applyFilter();
        }
    }
})();

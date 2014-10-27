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
        var keyCodes = config.keyCodes; //TODO: Copy these

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
            if (drink && drink.id) {
                //TODO use $location to do this
                //$location.path('/speaker/' + speaker.id);
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

(function () {
    'use strict';

    angular
        .module('app')
        .controller('drinkDetails', drinkDetails);

    drinkDetails.$inject = ['$location']; 

    function drinkDetails($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'drinkDetails';

        activate();

        function activate() { }
    }
})();

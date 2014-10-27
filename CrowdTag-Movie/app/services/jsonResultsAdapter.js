(function () {
    'use strict';

    angular
        .module('app')
        .value('jsonResultsAdapter', jsonResultsAdapter);

    jsonResultsAdapter.$inject = ['config'];

    function jsonResultsAdapter(config) {
        this.getData = getData;

        function getData() { }
    }
})();
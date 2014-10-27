(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular
        .module('app')
        .factory(serviceId, datacontext);

    datacontext.$inject = ['common', 'config', 'entityManagerFactory', 'model'];

    function datacontext(common, config,emFactory, model) {
        var entityNames = model.entityNames;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId); // info
        var logError = getLogFn(serviceId, 'error'); //error
        var logSuccess = getLogFn(serviceId, 'success');
        var manager = emFactory.newManager();
        var $q = common.$q;

        var service = {
            getDrinks: getDrinks,
            getPeople: getPeople,
            getMessageCount: getMessageCount
        };

        return service;

        function getMessageCount() { return $q.when(72); }

        function getPeople() {
            var people = [
                { firstName: 'John', lastName: 'Papa', age: 25, location: 'Florida' },
                { firstName: 'Ward', lastName: 'Bell', age: 31, location: 'California' },
                { firstName: 'Colleen', lastName: 'Jones', age: 21, location: 'New York' },
                { firstName: 'Madelyn', lastName: 'Green', age: 18, location: 'North Dakota' },
                { firstName: 'Ella', lastName: 'Jobs', age: 18, location: 'South Dakota' },
                { firstName: 'Landon', lastName: 'Gates', age: 11, location: 'South Carolina' },
                { firstName: 'Haley', lastName: 'Guthrie', age: 35, location: 'Wyoming' }
            ];
            return $q.when(people);
        }

        function getDrinks(forceRefresh) {
            return breeze.EntityQuery.from('Drinks')
                .using(manager)
                .execute()
                .then(querySucceded, queryFailed);

            function querySucceded(data) {
                log('Retrieved [Drinks]', data, true);
                return data.results;
            }
        }

        function queryFailed(error) {
            var msg = config.appErrorPrefix + 'Error retrieving data.' + error.message;
            logError(msg, error);
        }
    }
})();
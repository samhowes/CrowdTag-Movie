(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular
        .module('app')
        .factory(serviceId, datacontext);

    datacontext.$inject = ['common', 'config', 'entityManagerFactory', 'model','resources'];

    function datacontext(common, config,emFactory, model, resources) {
        var entityNames = model.entityNames;
        var events = config.events;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId); // info
        var logError = getLogFn(serviceId, 'error'); //error
        var logSuccess = getLogFn(serviceId, 'success');
        var manager = emFactory.newManager();
        var $q = common.$q;

        var service = {
            cancel: cancel,
            getDrinks: getDrinks,
            getPeople: getPeople,
            getMessageCount: getMessageCount,
            getDrinkById: getDrinkById,
            save: save,
            saveDrink: saveDrink
        };

        init();

        return service;

        function init() {
            onHasChangesChanged();
        }

        function cancel() {
            if (!manager.hasChanges()) {
                return;
            }
            manager.rejectChanges();
            logSuccess('Canceled changes', null, true);
        }

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

        function getDrinkById(id, forceRemote) {
            var entityName = entityNames.drink;

            if (!forceRemote) {
                var entity = manager.getEntityByKey(entityName, id);

                if (entity) {
                    logSuccess('Retrieved [' + entityName + '] id: ' + id + ' from cache.', entity, true);
                    return $q.when(entity);
                }
            }

            return breeze.EntityQuery
                .from(String.format('Drinks/{0}', id))
                .using(manager)
                .execute()
                .then(querySucceded, queryFailed);

            function querySucceded(data) {
                if (!(data.results.length > 0)) {
                    logError('Could not find [' + entityName + '] id:' + id, null, true);
                }

                var entity = data.results[0];
                return entity;
            }
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

        function onHasChangesChanged() {
            manager.hasChangesChanged.subscribe(function(eventArgs) {
                var data = { hasChanges: eventArgs.hasChanges };
                common.$broadcast(events.hasChangesChanged, data);
            });
        }

        function queryFailed(error) {
            var msg = config.appErrorPrefix + 'Error retrieving data.' + error.message;
            logError(msg, error);
        }

        function save() {
            return manager.saveChanges()
                .then(saveSucceeded, saveFailed);

            function saveSucceeded(result) {
                logSuccess('Saved data', result, true);
            }

            function saveFailed(error) {
                var msg = config.appErrorPrefix + 'Save failed: ' +
                    breeze.saveErrorMessageService.getErrorMessage(error);
                error.message = msg;
                logError(msg, error);
                throw error;
            }
        }

        function saveDrink(drink) {
            if (!drink) throw new { name: 'ArgumentNullException' };

            return resources.Drinks.save(drink)
                .$promise
                .then(function(response) {
                    logSuccess('Saved [Drink] successfully!', response, true);
                });

        }
    }
})();
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
        var primePromise = undefined;
        var $q = common.$q;

        var service = {
            cancel: cancel,
            getDrinks: getDrinks,
            getPeople: getPeople,
            getMessageCount: getMessageCount,
            getDrinkById: getDrinkById,
            prime: prime,
            save: save
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
            
            return manager.fetchEntityByKey(entityName, id, !forceRemote)
                .then(querySucceded, queryFailed);

            function querySucceded(data) {
                if (!data.entity) {
                    logError('Could not find [' + entityName + '] id:' + id, null, true);
                }

                if (data.fromCache) {
                    logSuccess('Retrieved [' + entityName + '] id: ' + id + ' from cache.', data.entity, true);
                }

                return data.entity;
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

        function prime() {
            if (primePromise) return primePromise;

            primePromise = $q.all([manager.fetchMetadata(function() { log('Retrieved metadata!', true); })])
                .then(extendMetadata);

            return primePromise.then(success);

            function success() {
                log('Primed the data', true);
            }

            function extendMetadata() {
                var metadataStore = manager.metadataStore;

                registerResourceNames(metadataStore);
            }

            function registerResourceNames(metadataStore) {
                var drinkType = metadataStore.getEntityType(entityNames.drink);
                var drinkResourceName = 'Drinks';

                set(drinkResourceName, drinkType);  

                var types = metadataStore.getEntityTypes();

                types.forEach(function (type) {
                    if (type instanceof breeze.EntityType) {
                        set(type.shortName, type);
                    }
                })

                function set(resourceName, entityType) {
                    metadataStore.setEntityTypeForResourceName(resourceName, entityType);
                }
            }
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
    }
})();
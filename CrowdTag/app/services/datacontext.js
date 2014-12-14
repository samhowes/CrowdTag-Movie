(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular
        .module('app')
        .factory(serviceId, datacontext);

    datacontext.$inject = ['common', 'config', 'entityManagerFactory', 'model','resources'];

    function datacontext(common, config, emFactory, model, resources) {
        var $q = common.$q;

        var entityNames = model.entityNames;
        var events = config.events;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId); // info
        var logError = getLogFn(serviceId, 'error'); //error
        var logSuccess = getLogFn(serviceId, 'success');
        var manager = emFactory.newManager();
        var currentUser = null;                // retrieved on prime
        var primePromise = undefined;
        var unmappedEntityProperties = model.unmappedEntityProperties;

        var exceptionalResources = {
            lookups: 'Lookups'
        };

        var storeMeta = {
            isLoaded: {
                // will be populated with: entityName: false;
            }
        };

        var entityNamesForResources = {
            'Drinks': entityNames.drink,
            'Ingredients': entityNames.ingredient
        };

        var resourcesForEntityNames = {
            // will be the inverse of entityNamesForResources
        };

        var service = {
            cancel: cancel,
            createEntity: createEntity,
            createIngredient:createIngredient,
            getDrinks: getDrinks,
            getIngredients: getIngredients,
            getIngredientById: getIngredientById,
            getPeople: getPeople,
            getMessageCount: getMessageCount,
            getDrinkById: getDrinkById,
            lookupCachedData: undefined, // assigned in getLookups
            markDeleted: markDeleted,
            prime: prime,
            save: save,
            setLookups:setLookups
        };

        init();

        return service;

        function init() {
            initResourceEntityMap();
            initStoreMeta();
            onHasChangesChanged();
            setupEventForEntityCreated();
        }

        function initResourceEntityMap() {
            for (var _resource in entityNamesForResources) {
                var _entityName = entityNamesForResources[_resource];
                resourcesForEntityNames[_entityName] = _resource;
            }
        }

        function initStoreMeta() {
            for (var entityName in entityNames) {
                storeMeta.isLoaded[name] = false;
            }
        }

        function cancel() {
            if (!manager.hasChanges()) {
                return;
            }
            manager.rejectChanges();
            logSuccess('Canceled changes', null, true);
        }

        function createDrink() {
            return createEntity(entityNames.drink);
        }

        function createIngredient() {
            return createEntity(entityNames.ingredient);
        }

        function createEntity(entityName) {
            return manager.createEntity(entityName);
        }

        function getLookups() {
            return breeze.EntityQuery
                .from(exceptionalResources.lookups)
                .using(manager)
                .execute()
                .then(success, _queryFailed);

            function success(data) {
                log('Retrieved [Lookups]', data, true);

                var lookups = data.results[0];

                lookups.ingredientCategories.forEach(function(ic) {
                    ic.isIngredientCategory = true;
                });

                model.createNullos(manager);


                return true;
            }
        }

        function setLookups() {
            currentUser = _getEntitiesLocal(entityNames.user)[0];

            var predicate = breeze.Predicate.create(unmappedEntityProperties.isIngredientCategory, '==', true);
            var order = '_order, name';

            service.lookupCachedData = {
                measurementTypes: _getEntitiesLocal(entityNames.measurementType),
                ingredientCategories: _getEntitiesLocal(entityNames.ingredientCategory, order, predicate),
                tagCategories: _getEntitiesLocal(entityNames.tagCategory, order)
            };
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
                .then(querySucceded, _queryFailed);

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
            return getEntities(entityNames.drink, forceRefresh);
        }

        function getEntities(entityName, forceRefresh) {
            var fromCache = (!forceRefresh && _areItemsLoaded(entityName));

            if (fromCache) {
                return getEntitiesLocal(entityName);
            }
            
            return _getEntities(entityName)
                    .then(querySucceded, _queryFailed);

            function querySucceded(data) {
                log(String.format('Retrieved [{0}] from server', String.pluralize(entityName)), data, true);
                _areItemsLoaded(entityName, true);
                return data.results;
            }
        }

        function getEntitiesLocal(entityName) {

            var entities = _getEntitiesLocal(entityName);
            return $q.when(entities)
                .then(querySucceded, _queryFailed);

            function querySucceded(results) {
                log(String.format('Retrieved [{0}] from cache', String.pluralize(entityName)), results, true);
                return results;
            }
        }

        function getIngredients(forceRefresh) {
            return getEntities(entityNames.ingredient, forceRefresh);
        }
        
        function getIngredientById(id, forceRemote) {
            var entityName = entityNames.ingredient;

            return manager.fetchEntityByKey(entityName, id, !forceRemote)
                .then(querySucceded, _queryFailed);

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

        function interceptEntityCreation(creationArgs) {
            var entity = creationArgs.entity;
            entity.submitter = currentUser;
            entity.createdDateTime = Date();
        }

        // Forget certain changes by removing them from the entity's originalValues
        // this function becomes unnecessary if breeze decides that 
        // unmapped properties are not recorded in original values
        //
        // We do this so we can remove the isIngredientCategory properties from
        // the originalValues of an entity. Otherwise, when the object's changes
        // are canceled these values will also reset: isIngredientCategory will go
        // from false to true, and force the controller to refetch the
        // entity from the server.
        function interceptPropertyChange(changeArgs) {
            var changedProp = changeArgs.args.propertyName;
            if (unmappedEntityProperties[changedProp]) {
                delete changeArgs.entity.entityAspect.originalValues[changedProp];
            }
        }

        function markDeleted(entity) {
            entity.entityAspect.setDeleted();
        }

        function onHasChangesChanged() {
            manager.hasChangesChanged.subscribe(function(eventArgs) {
                var data = { hasChanges: eventArgs.hasChanges };
                common.$broadcast(events.hasChangesChanged, data);
            });
        }

        function prime() {
            if (primePromise) return primePromise;

            primePromise = $q.all([getLookups()]).then(function() {
                extendMetadata();
            });

            return primePromise.then(success);

            function success() {
                setLookups();
                log('Primed the data', true);
            }

            function extendMetadata() {
                var metadataStore = manager.metadataStore;

                registerResourceNames(metadataStore);
            }

            function registerResourceNames(metadataStore) {
                var types = metadataStore.getEntityTypes();

                types.forEach(function (type) {
                    if (type instanceof breeze.EntityType) {
                        if (type.baseEntityType) {
                            // If it is a subtype, its resource won't automatically be set
                            set(String.pluralize(type.shortName), type);
                        }
                        set(type.shortName, type);
                    }
                });

                function set(resourceName, entityType) {
                    metadataStore.setEntityTypeForResourceName(resourceName, entityType);
                }
            }
        }

        function save(saveList) {
            return manager.saveChanges(saveList)
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

        function setupEventForEntityCreated() {
            manager.entityChanged.subscribe(function(changeArgs) {
                var entityAspect = changeArgs.entity.entityAspect;
                if (changeArgs.entityAction === breeze.EntityAction.Attach &&
                    entityAspect.entityState === breeze.EntityState.Added) {
                    interceptEntityCreation(changeArgs);
                }
            });
        }

        function _areDrinksLoaded(value) {
            return _areItemsLoaded(entityNames.drink, value);
        }

        function _areTagsLoaded(value) {
            return _areItemsLoaded(entityNames.tag, value);
        }

        function _areItemsLoaded(key, value) {
            if (value === undefined) {
                return storeMeta.isLoaded[key]; // Get
            }

            return storeMeta.isLoaded[key] = value; // set
        }

        function _getEntities(entityName) {
            var resource = resourcesForEntityNames[entityName];
            return breeze.EntityQuery.from(resource)
                .using(manager)
                .execute();
        }

        function _getEntitiesLocal(entityName, ordering, predicate) {
            var entities = breeze.EntityQuery
                .from(entityName)
                .orderBy(ordering)
                .where(predicate)
                .using(manager)
                .executeLocally();
            return entities;
        }

        function _queryFailed(error) {
            var msg = config.appErrorPrefix + 'Error retrieving data.' + error.message;
            logError(msg, error);
        }

        function _resourceForEntityName(entityName, resource) {
            return resource || resourcesForEntityNames[entityName];
        }
    }
})();
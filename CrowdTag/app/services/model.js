(function() {
    'use strict';

    var serviceId = 'model';

    angular
        .module('app')
        .factory(serviceId, model);

    model.$inject = [];

    function model() {
        
        var entityNames = {
            drink: 'Drink',
            tag: 'Tag',
            tagCategory: 'TagCategory',
            tagApplication: 'TagApplication',
            ingredientApplication: 'IngredientApplication',
            ingredientCategory: 'TagCategory',
            ingredient: 'Ingredient',
            measurementType: 'MeasurementType',
            user: 'User'
        };

        var unmappedEntityProperties = {
            isIngredientCategory: 'isIngredientCategory',
            _order: '_order'
        };

        var service = {
            configureMetadataStore: configureMetadataStore,
            createNullos: createNullos,
            entityNames: entityNames,
            unmappedEntityProperties: unmappedEntityProperties
        };

        return service;

        function configureMetadataStore(metadataStore) {
            registerDrink(metadataStore);
            registerIngredientApplication(metadataStore);
            registerTagCategory(metadataStore);
            //todo add validations for nullos

            //metadataStore.setEntityTypeForResourceName('Drinks', entityNames.drink);
        }

        function createNullos(manager) {
            var unchanged = breeze.EntityState.Unchanged;

            createNullo(entityNames.ingredientCategory, {id:0, name: getDisplayName(entityNames.ingredientCategory), isIngredientCategory: true, _order: 0 });
            createNullo(entityNames.ingredientCategory, {id:-2, name:'Create new category...', isIngredientCategory: true, isCreateNew:true, _order: 10 });


            function createNullo(entityName, values) {
                var initialValues = values || { name: getDisplayName(entityName) };
                return manager.createEntity(entityName, initialValues, unchanged);
            }

            function getDisplayName(entityName) {
                return ' [Select a ' + entityName.toLowerCase() + ']';
            }
        }

        function registerDrink(metadataStore) {
            
            metadataStore.registerEntityTypeCtor(entityNames.drink, Drink);

            function Drink() { }

            Object.defineProperty(Drink.prototype, 'tagsFormatted', {
                get: function () {
                    if (!this.tagApplications.length) return "";
                    var names = this.tagApplications.map(function (tagApp) { return tagApp.tag.name; });
                    return names.join(', ');
                }
            });


        }

        function registerIngredientApplication(metadataStore) {

            metadataStore.registerEntityTypeCtor(entityNames.ingredientApplication, IngredientApplication);

            function IngredientApplication() { }

            Object.defineProperty(IngredientApplication.prototype, 'amountFormatted', {
                get: function() {
                    var display;
                    
                    if (!this.measurementType) {
                        display = '';
                    } else if (!this.amount) {
                        display = this.measurementType.name;
                    } else {
                        display = this.amount + " " + this.measurementType.name;
                    }

                    return display;
                }

            });
        }

        function registerTagCategory(metadataStore) {
            metadataStore.registerEntityTypeCtor(entityNames.tagCategory, TagCategory);

            function TagCategory() {
                this.isIngredientCategory = false;
                this._order = 1;
                this.isCreateNew = false;
            }
        }
    }

})();
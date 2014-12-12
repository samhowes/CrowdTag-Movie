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
            tagApplication: 'TagApplication',
            ingredientApplication: 'IngredientApplication',
            ingredient: 'Ingredient',
            measurementType: 'MeasurementType',
            user: 'User'
        };

        var service = {
            configureMetadataStore: configureMetadataStore,
            entityNames: entityNames,
        };

        return service;

        function configureMetadataStore(metadataStore) {
            registerDrink(metadataStore);
            registerIngredientApplication(metadataStore);
            //metadataStore.setEntityTypeForResourceName('Drinks', entityNames.drink);

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
    }

})();
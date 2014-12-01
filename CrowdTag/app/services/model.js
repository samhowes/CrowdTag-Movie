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
            tag: 'Tag'
        };

        var service = {
            configureMetadataStore: configureMetadataStore,
            entityNames: entityNames,
        };

        return service;

        function configureMetadataStore(metadataStore) {
            registerDrink(metadataStore);

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
    }

})();
(function() {
    'use strict';
    
    var serviceId = 'entityManagerFactory';
    angular
        .module('app')
        .factory(serviceId, ['breeze', 'config', 'model', emFactory]);

    function emFactory(breeze, config, model) {
        var breezeConfig = config.breeze;

        // Convert server-side PascalCase to client-side camelCase property names
        breeze.NamingConvention.camelCase.setAsDefault();
        // Do not validate when we attach a newly created entity to an EntityManager.
        // We could also set this per entityManager
        new breeze.ValidationOptions({ validateOnAttach: false }).setAsDefault();
        
        var serviceName = breezeConfig.remoteServiceName;
        var hasServerMetadata = breezeConfig.hasServerMetadata;
        var metadataStore = createMetadataStore();

        var provider = {
            metadataStore: metadataStore,
            newManager: newManager
        };
        
        return provider;

        function createMetadataStore() {
            var store = new breeze.MetadataStore();
            model.configureMetadataStore(store);   
            return store;
        }

        function newManager() {

            var dataService = new breeze.DataService({
                hasServerMetadata: hasServerMetadata,
                serviceName: serviceName,
                //jsonResultsAdapter: model.jsonResultsAdapter
            });

            var mgr = new breeze.EntityManager({
                dataService: dataService,
                metadataStore: metadataStore
            });
            return mgr;
        }
    }
})();
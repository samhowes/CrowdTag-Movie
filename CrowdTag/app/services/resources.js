(function () {
    'use strict';

    var serviceId = 'resources';

    angular
        .module('app')
        .factory(serviceId, resources);

    resources.$inject = ['$resource', 'config'];

    function resources($resource, config) {
        var resourceNames = ['Drinks'];

        var service = {
            // Lazy loaded properties:
            // Drinks
        };

        init();

        return service;

        function init() {
            defineLazyLoadedResources();
        }

        function defineLazyLoadedResources() {
            resourceNames.forEach(function(name) {
                Object.defineProperty(service, name, {
                    configurable: true,
                    get: function() {
                        // lazy load this the first time
                        var api = config.api[name.toLowerCase()] + '/:id';
                        var resource = $resource(api, {id: '@id'});
                        // and then make sure it is only loaded once
                        Object.defineProperty(service, name, {
                            value: resource,
                            configurable: false,
                            enumerable: true
                        });
                        return resource;
                    }
                });
            });
        }
    }
})();
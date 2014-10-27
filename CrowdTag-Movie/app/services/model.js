(function() {
    'use strict';

    var serviceId = 'model';

    angular
        .module('app')
        .factory(serviceId, model);

    model.$inject = ['breeze', 'common', 'config'];

    function model(breeze, common, config) {
        var DT = breeze.DataType;
        var entityNamespace = config.breeze.entityNamespace;
        var jsonResultsAdapter = createJsonResultsAdapter();

        var entityNames = {
            drink: 'Drink',
            tag: 'Tag'
        };


        var service = {
            configureMetadataStore: configureMetadataStore,
            entityNames: entityNames,
            jsonResultsAdapter: jsonResultsAdapter
        };

        return service;

        function configureMetadataStore(metadataStore) {

            addDrinkItemType(metadataStore);
            addTagItemType(metadataStore);
            //TODO: add other entities

        }

        function addDrinkItemType(metadataStore) {

            registerDrinkCtor(metadataStore);
            var drinkItemType = {
                shortName: entityNames.drink,
                namespace: entityNamespace,
                dataProperties: {
                    iD: type(DT.Int64, true),
                    name: type(DT.String),
                    description: type(DT.String),
                    createdDateTime: type(DT.DateTime),
                    updatedDateTime: type(DT.DateTime)
                },
                navigationProperties: {
                    tags: {
                        entityTypeName: getFullEntityName(entityNames.tag, entityNamespace),
                        isScalar: false,
                        associationName: createAssociationName(entityNames.drink, entityNames.tag)
                    }
                }
            };

            metadataStore.addEntityType(drinkItemType);

            function registerDrinkCtor(mdStore) {
                

                function Drink() {}

                Object.defineProperty(Drink.prototype, 'tagsFormatted', {
                    get: function () {
                        if (!this.tags.length) return "";
                        var names = this.tags.map(function(tag) { return tag.name; });
                        return names.join(', ');
                    }
                });

                mdStore.registerEntityTypeCtor(entityNames.drink, Drink);
                
            }
        }

        function addTagItemType(metadataStore) {
            var tagItemType = {
                shortName: entityNames.tag, 
                namespace: entityNamespace,
                dataProperties: {
                    iD: type(DT.Int64, true),
                    categoryID: type(DT.Int64),
                    taggedItemId: type(DT.Int64),
                    name: type(DT.String),
                    createdDateTime: type(DT.DateTime),
                    updatedDateTime: type(DT.DateTime)
                },
                navigationProperties: {
                    drink: {
                        entityTypeName: getFullEntityName(entityNames.drink, entityNamespace),
                        isScalar: true,
                        associationName: createAssociationName(entityNames.drink, entityNames.tag) /*,
                        foreignKeyNames: ["taggedItemId"]*/
                    }
                }
            };

            metadataStore.addEntityType(tagItemType);
        }

        function createAssociationName(sourceEntity, destEntity) {
            return String.format("{0}_{1}", sourceEntity, destEntity);
        }

        function createJsonResultsAdapter() {
            var adapter = {
                name: "drinks",
                visitNode: visitNode
            };
            return new breeze.JsonResultsAdapter(adapter);

            function visitNode(node, parseContext, nodeContext) {
                // Drink parser
                if (node.ID && node.Recipe !== undefined) {
                    return { entityType: entityNames.drink };
                }

                // Tag parser
                if (node.ID && node.TaggedItemId !== undefined) {
                    return { entityType: entityNames.tag };
                }
            }
        }

        function getFullEntityName(entityName, namespace) {
            return String.format("{0}:#{1}", entityName, namespace);
        }

        function type(dataType, isPartOfKey) {
            if (isPartOfKey === undefined) isPartOfKey = false;
            return {
                dataType: dataType,
                isPartOfKey: isPartOfKey
            };
        }
    }

})();
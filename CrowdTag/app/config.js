(function() {
    'use strict';

    var app = angular.module('app');

    // Configure Toastr
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var keyCodes = {
        backspace: 8,
        tab: 9,
        enter: 13,
        esc: 27,
        space: 32,
        pageup: 33,
        pagedown: 34,
        end: 35,
        home: 36,
        left: 37,
        right: 39,
        down: 40,
        insert: 45,
        del: 46
    };

    var apiPrefix = 'api';
    var api = {
        drinks: apiPrefix + '/Drinks'
    };

    // For use with the HotTowel-Angular-Breeze add-on that uses Breeze
    var breezeConfig = {
        entityNamespace: 'CrowdTagDrinks',
        hasServerMetadata: true,
        remoteServiceName: 'api/breeze'
    };

    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        hasChangesChanged: 'hasChangesChanged',
        spinnerToggle: 'spinner.toggle'
    };

    var imageSettings = {
        imageBasePath: '../content/images/photos/',
        unknownPersonImageSource: 'unknown_drink.jpg'
    }

    var config = {
        api: api,
        appErrorPrefix: '[CT Error] ', //Configure the exceptionHandler decorator
        breeze: breezeConfig,
        docTitle: 'CrowdTag: ',
        events: events,
        imageSettings: imageSettings,
        keyCodes: keyCodes,
        version: '0.1'
    };

    app.value('config', config);
    
    app.config(['$logProvider', function ($logProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }]);
    
    //#region Configure the common services via commonConfig
    app.config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
    }]);
    //#endregion

    app.run(['common', function(common) {
        common.installStringFormat();
    }]);

})();
﻿var WSF = WSF || {};
(function () {

    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).
    WSF.appPath = WSF.appPath || '/';

    //Converts given path to absolute path using WSF.appPath variable.
    WSF.toAbsAppPath = function (path) {
        if (path.indexOf('/') == 0) {
            path = path.substring(1);
        }

        return WSF.appPath + path;
    };

    /* LOCALIZATON ***********************************************/
    //Implements Localization API that simplifies usage of localization scripts generated by WSF.

    WSF.localization = WSF.localization || {};

    WSF.localization.defaultSourceName = undefined;

    WSF.localization.localize = function (key, sourceName) {
        sourceName = sourceName || WSF.localization.defaultSourceName;

        var source = WSF.localization.values[sourceName];

        if (source == undefined) {
            WSF.log.warn('Could not find localization source: ' + sourceName);
            return key;
        }

        var value = source[key];
        return value == undefined ? key : value;
    };

    WSF.localization.getSource = function (sourceName) {
        return function (key) {
            return WSF.localization.localize(key, sourceName);
        };
    };

    WSF.localization.isCurrentCulture = function (name) {
        return WSF.localization.currentCulture
            && WSF.localization.currentCulture.name
            && WSF.localization.currentCulture.name.indexOf(name) == 0;
    };

    /* AUTHORIZATION **********************************************/
    //Implements Authorization API that simplifies usage of authorization scripts generated by WSF.

    WSF.auth = WSF.auth || {};

    WSF.auth.allPermissions = WSF.auth.allPermissions || {};

    WSF.auth.grantedPermissions = WSF.auth.grantedPermissions || {};

    WSF.auth.hasPermission = function (permissionName) {
        return WSF.auth.allPermissions[permissionName] != undefined && WSF.auth.grantedPermissions[permissionName] != undefined;
    };

    WSF.auth.hasAnyOfPermissions = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (WSF.auth.hasPermission(arguments[i])) {
                return true;
            }
        }

        return false;
    };

    WSF.auth.hasAllOfPermissions = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (!WSF.auth.hasPermission(arguments[i])) {
                return false;
            }
        }

        return true;
    };

    /* SETTINGS **************************************************/
    //Implements Settings API that simplifies usage of setting scripts generated by WSF.

    WSF.setting = WSF.setting || {};

    WSF.setting.values = WSF.setting.values || {};

    WSF.setting.get = function (name) {
        return WSF.setting.values[name];
    };

    WSF.setting.getBoolean = function (name) {
        return WSF.setting.values[name] == 'true';
    };

    WSF.setting.getInt = function (name) {
        return parseInt(WSF.setting.values[name]);
    };

    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log

    WSF.log = WSF.log || {};

    WSF.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    WSF.log.level = WSF.log.levels.DEBUG;

    WSF.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel != undefined && logLevel < WSF.log.level) {
            return;
        }

        console.log(logObject);
    };

    WSF.log.debug = function (logObject) {
        WSF.log.log("DEBUG: ", WSF.log.levels.DEBUG);
        WSF.log.log(logObject, WSF.log.levels.DEBUG);
    };

    WSF.log.info = function (logObject) {
        WSF.log.log("INFO: ", WSF.log.levels.INFO);
        WSF.log.log(logObject, WSF.log.levels.INFO);
    };

    WSF.log.warn = function (logObject) {
        WSF.log.log("WARN: ", WSF.log.levels.WARN);
        WSF.log.log(logObject, WSF.log.levels.WARN);
    };

    WSF.log.error = function (logObject) {
        WSF.log.log("ERROR: ", WSF.log.levels.ERROR);
        WSF.log.log(logObject, WSF.log.levels.ERROR);
    };

    WSF.log.fatal = function (logObject) {
        WSF.log.log("FATAL: ", WSF.log.levels.FATAL);
        WSF.log.log(logObject, WSF.log.levels.FATAL);
    };

    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it

    WSF.notify = WSF.notify || {};

    WSF.notify.success = function (message, title) {
        WSF.log.warn('WSF.notify.success is not implemented!');
    };

    WSF.notify.info = function (message, title) {
        WSF.log.warn('WSF.notify.info is not implemented!');
    };

    WSF.notify.warn = function (message, title) {
        WSF.log.warn('WSF.notify.warn is not implemented!');
    };

    WSF.notify.error = function (message, title) {
        WSF.log.warn('WSF.notify.error is not implemented!');
    };

    /* MESSAGE **************************************************/
    //Defines Message API, not implements it

    WSF.message = WSF.message || {};

    WSF.message.info = function (message, title) {
        WSF.log.warn('WSF.message.info is not implemented!');
        alert((title || '') + ' ' + message);
    };

    WSF.message.warn = function (message, title) {
        WSF.log.warn('WSF.message.warn is not implemented!');
        alert((title || '') + ' ' + message);
    };

    WSF.message.error = function (message, title) {
        WSF.log.warn('WSF.message.error is not implemented!');
        alert((title || '') + ' ' + message);
    };

    /* UI *******************************************************/

    WSF.ui = WSF.ui || {};

    /* UI BLOCK */
    //Defines UI Block API, not implements it

    WSF.ui.block = function (elm) {
        WSF.log.warn('WSF.ui.block is not implemented!');
    };

    WSF.ui.unblock = function (elm) {
        WSF.log.warn('WSF.ui.unblock is not implemented!');
    };

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    WSF.ui.setBusy = function (elm, optionsOrPromise) {
        WSF.log.warn('WSF.ui.setBusy is not implemented!');
    };

    WSF.ui.clearBusy = function (elm) {
        WSF.log.warn('WSF.ui.clearBusy is not implemented!');
    };

    /* UTILS ***************************************************/

    WSF.utils = WSF.utils || {};

    /* Creates a name namespace.
    *  Example:
    *  var taskService = WSF.utils.createNamespace(WSF, 'services.task');
    *  taskService will be equal to WSF.services.task
    *  first argument (root) must be defined first
    ************************************************************/
    WSF.utils.createNamespace = function (root, ns) {
        var parts = ns.split('.');
        for (var i = 0; i < parts.length; i++) {
            if (typeof root[parts[i]] == 'undefined') {
                root[parts[i]] = {};
            }

            root = root[parts[i]];
        }

        return root;
    };

    /* Formats a string just like string.format in C#.
    *  Example:
    *  _formatString('Hello {0}','Halil') = 'Hello Halil'
    ************************************************************/
    WSF.utils.formatString = function () {
        if (arguments.length == 0) {
            return null;
        }

        var str = arguments[0];
        for (var i = 1; i < arguments.length; i++) {
            var placeHolder = '{' + (i - 1) + '}';
            str = str.replace(placeHolder, arguments[i]);
        }

        return str;
    };

})();
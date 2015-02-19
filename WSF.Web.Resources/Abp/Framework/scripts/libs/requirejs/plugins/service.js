﻿define(function () {
    return {
        load: function (name, req, onload, config) {
            var url = WSF.appPath + 'api/WSFServiceProxies/Get?name=' + name;
            req([url], function (value) {
                onload(value);
            });
        }
    };
});
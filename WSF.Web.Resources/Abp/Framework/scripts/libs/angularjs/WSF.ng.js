(function (WSF, angular) {

    if (!angular) {
        return;
    }

    var WSFModule = angular.module('WSF', []);

    WSFModule.config([
        '$httpProvider', function ($httpProvider) {
            $httpProvider.interceptors.push([ '$q', function ($q) {

                var defaultError = {
                    message: 'Ajax request is not succeed!',
                    details: 'Error detail is not sent by server.'
                };

                return {

                    'request': function (config) {
                        if (endsWith(config.url, '.cshtml')) {
                            config.url = WSF.appPath + 'WSFAppView/Load?viewUrl=' + config.url;
                        }

                        return config;
                    },

                    'response': function (response) {
                        if (!response.config || !response.config.WSF || !response.data) {
                            return response;
                        }

                        var originalData = response.data;
                        var defer = $q.defer();

                        if (originalData.success === true) {
                            response.data = originalData.result;
                            defer.resolve(response);
                        } else { //data.success === false
                            if (originalData.error) {
                                if (originalData.error.details) {
                                    WSF.message.error(originalData.error.details, originalData.error.message);
                                } else {
                                    WSF.message.error(originalData.error.message);
                                }
                            } else {
                                originalData.error = defaultError;
                            }

                            WSF.log.error(originalData.error.message + ' | ' + originalData.error.details);

                            response.data = originalData.error;
                            defer.reject(response);

                            if (originalData.unAuthorizedRequest && !originalData.targetUrl) {
                                location.reload();
                            }
                        }

                        if (originalData.targetUrl) {
                            location.href = originalData.targetUrl;
                        }

                        return defer.promise;
                    },

                    'responseError': function (error) {
                        WSF.message.error(error.data, error.statusText);
                        WSF.log.error(error);
                        return $q.reject(error);
                    }

                };
            }]);
        }
    ]);

    function endsWith(str, suffix) {
        if (suffix.length > str.length) {
            return false;
        }

        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    }

})((WSF || (WSF = {})), (angular || undefined));

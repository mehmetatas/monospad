angular.module("monospad").service("monospad", ["$http", "$rootScope", "clientData", function ($http, $rootScope, clientData) {
    var send = function (method, controller, action, data, success, error, complete, block) {
        var token = clientData.token();

        var opts = {
            method: method,
            url: "/api/" + controller + "/" + action,
            headers: {
                'monospad-auth-token': token || ""
            }
        };

        if (method === "GET" || method === "DELETE") {
            opts.params = data;
        } else {
            opts.data = data;
        }

        $rootScope.blocker.block = block;
        
        return $http(opts)
            .success(function (resp, status, headers, config) {
                $rootScope.blocker.block = false;
                if (resp.ResponseCode === 0 && success) {
                    success(resp);
                } else if (resp.ResponseCode !== 0) {
                    if (error) {
                        error(resp);
                    } else {
                        alert(resp.ResponseCode + ": " + resp.ResponseMessage);
                    }
                }
            })
            .error(function (resp, status, headers, config) {
                $rootScope.blocker.block = false;
                if (error) {
                    error({
                        ResponseCode: -1,
                        ResponseMessage: "ooops! something went terribly wrong :("
                    });
                } else {
                    alert("ooops! something went terribly wrong :(");
                }
            })
            .finally(function () {
                if (complete) {
                    complete();
                }
            });
    };

    this.post = function (controller, action, data, success, error, complete, block) {
        return send("POST", controller, action, data, success, error, complete, block);
    };

    this.put = function (controller, action, data, success, error, complete, block) {
        return send("PUT", controller, action, data, success, error, complete, block);
    };

    this.get = function (controller, action, data, success, error, complete, block) {
        return send("GET", controller, action, data, success, error, complete, block);
    };

    this.delete = function (controller, action, data, success, error, complete, block) {
        return send("DELETE", controller, action, data, success, error, complete, block);
    };
}]);
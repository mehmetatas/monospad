angular.module("monospad").service("clientData", ["$localStorage", function ($localStorage) {
    var data = $localStorage.clientData || {};

    var getOrSet = function (key, value) {
        if (typeof value === "undefined") {
            return data[key];
        }

        data[key] = value;
        $localStorage.clientData = data;
        return value;
    };

    this.token = function (value) {
        return getOrSet("token", value);
    };
}]);
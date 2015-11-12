angular.module("monospad").config([
    "$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider, $urlRouterProvider, $locationProvider) {
        "use strict";

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

        function basepath(uri) {
            return "/app/html/" + uri + "";
        }

        // default route to dashboard
        $urlRouterProvider.otherwise("/");

        //
        // app.routes
        // -----------------------------------
        $stateProvider
            .state("app", {
                url: "/",
                templateUrl: basepath("app.html"),
                controller: "appCtrl"
            });
    }
]);
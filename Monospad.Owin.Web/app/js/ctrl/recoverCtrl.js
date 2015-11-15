angular.module("monospad").controller("recoverCtrl", ["$scope", "$rootScope", "$state", "$stateParams", "user", "clientData", function ($scope, $rootScope, $state, $stateParams, user, clientData) {
    $rootScope.blocker = {};

    $scope.resetPassword = function () {
        user.resetPassword({
            Token: $stateParams.token,
            NewPassword: $scope.newPassword
        }, function (resp) {
            clientData.token(resp.Data.Token);
            $state.go("app");
        });
    };

    clientData.token(null);
}]);
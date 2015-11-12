angular.module("monospad").service("note", [
    "monospad", function(monospad) {
        var controller = "note";
        this.saveNote = function(data, success, error, complete) {
            monospad.post(controller, "save", data, success, error, complete);
        };
        this.getContent = function (data, success, error, complete) {
            monospad.get(controller, "getContent", data, success, error, complete, true);
        };
        this.deleteNote = function (data, success, error, complete) {
            monospad.post(controller, "delete", data, success, error, complete, true);
        };
    }
]).service("user", [
    "monospad", function(monospad) {
        var controller = "user";
        this.signup = function (data, success, error, complete) {
            monospad.post(controller, "signup", data, success, error, complete, true);
        };
        this.signin = function (data, success, error, complete) {
            monospad.post(controller, "signin", data, success, error, complete, true);
        };
        this.signinWithToken = function (data, success, error, complete) {
            monospad.post(controller, "signinWithToken", data, success, error, complete, true);
        };
        this.signout = function (data, success, error, complete) {
            monospad.post(controller, "signout", data, success, error, complete, true);
        };
        this.recoverPassword = function (data, success, error, complete) {
            monospad.post(controller, "recoverPassword", data, success, error, complete, true);
        }; 
    }
]);
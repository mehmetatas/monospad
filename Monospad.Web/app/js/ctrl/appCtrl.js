angular.module("monospad").controller("appCtrl", ["$scope", "$rootScope", "$timeout", "note", "user", "clientData", function ($scope, $rootScope, $timeout, note, user, clientData) {
    var saveTimeout;
    var skipSave;

    $scope.loggedin = false;
    $scope.current = null;
    $scope.notes = [];
    $scope.signinInfo = {};

    $rootScope.blocker = {};

    var validateSignInInfo = function () {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (!re.test($scope.signinInfo.Email)) {
            alert("invalid email address!");
            return false;
        }

        if ($scope.authMode !== 3 && (!$scope.signinInfo.Password || $scope.signinInfo.Password.length < 4)) {
            alert("password must be 4-20 characters long!");
            return false;
        }

        return true;
    };

    var scrollDiv = function (divId, scrollTop, scrollDuration) {
        var tick = 15;

        var div = document.getElementById(divId);

        scrollTop -= div.clientHeight / 2;

        var stepCount = Math.floor(scrollDuration / tick);
        var step = (scrollTop - div.scrollTop) / stepCount;

        var interval = setInterval(function () {
            if (stepCount-- === 0) {
                div.scrollTop = scrollTop;
                clearInterval(interval);
            } else {
                div.scrollTop += step;
            }
        }, tick);
    };

    var processAuthSuccess = function (resp) {
        $scope.loggedin = true;
        $scope.authMode = 0;
        $scope.notes = resp.Data.Notes;
        if ($scope.current && $scope.current.Content) {
            $scope.current.Content = $scope.notes[0].Content;
            $scope.current.selected = true;
        }
        clientData.token(resp.Data.Token);
        $scope.signinInfo = {};
    };

    var ensureSignedOut = function () {
        console.log("ensureSignedOut");
        $scope.loggedin = false;
        clientData.token(null);
        $scope.notes = [];
        $scope.current = null;
        $scope.signinInfo = {};
    };

    var ensureAuth = function (resp) {
        if (resp.ResponseCode === 20 || resp.ResponseCode === 21) {
            ensureSignedOut();
        }
        alert(resp.ResponseCode + ": " + resp.ResponseMessage);
    };

    var signup = function () {
        var req = {
            Email: $scope.signinInfo.Email,
            Password: $scope.signinInfo.Password
        };

        if ($scope.current) {
            req.UnsavedNoteContent = $scope.current.Content;
        }

        user.signup(req, processAuthSuccess);
    };

    var signin = function () {
        var req = {
            Email: $scope.signinInfo.Email,
            Password: $scope.signinInfo.Password
        };

        if ($scope.current) {
            req.UnsavedNoteContent = $scope.current.Content;
        }

        user.signin(req, processAuthSuccess);
    };

    var recoverPassword = function () {
        var req = {
            Email: $scope.signinInfo.Email
        };
        
        user.recoverPassword(req, function() {
            alert("a password recovery mail has sent to your email address: " + $scope.signinInfo.Email);
            ensureSignedOut();
        });
    };

    // Note

    $scope.newNote = function () {
        if ($scope.current && $scope.current.Id) {
            $scope.current.selected = false;
            $scope.current = { selected: true, Title: "" };
            $scope.notes.splice(0, 0, $scope.current);
        } else if (!$scope.current) {
            $scope.current = { selected: true, Title: "" };
            $scope.notes.splice(0, 0, $scope.current);
        }
        scrollDiv("notes", 0, 250);
    };

    $scope.loadNote = function (n) {
        if ($scope.current) {
            $scope.current.selected = false;
        }

        n.selected = true;

        if (n.Content || !n.Id) {
            skipSave = true;
            $scope.current = n;
            return;
        }

        note.getContent({ Id: n.Id }, function (resp) {
            n.Content = resp.Data.Content;
            skipSave = true;
            $scope.current = n;
        }, ensureAuth);
    };

    $scope.deleteNote = function (n) {
        if (!confirm("are you sure you want to delete '" + n.Title + "'?")) {
            return;
        }

        note.deleteNote({ Id: n.Id }, function () {
            var i = $scope.notes.indexOf(n);

            if (i > -1) {
                $scope.notes.splice(i, 1);
            }

            if (n === $scope.current) {
                skipSave = true;
                $scope.current = null;
            }
        }, ensureAuth);
    };

    $scope.$watch("current.Content", function () {
        if (!$scope.loggedin) {
            return;
        }

        if (skipSave) {
            skipSave = false;
            return;
        }

        var curr = $scope.current;

        if (!curr.Content) {
            $timeout.cancel(saveTimeout);
            saveTimeout = null;

            note.saveNote({
                Id: curr.Id,
                Content: curr.Content
            });

            curr.Title = "";
            curr.Summary = "";
            return;
        }

        if (saveTimeout) {
            return;
        }

        saveTimeout = $timeout(function () {
            curr.saving = true;

            note.saveNote({
                Id: curr.Id,
                Content: curr.Content
            }, function (resp) {
                var data = resp.Data;

                curr.Id = data.Id;
                curr.Title = data.Title;
                curr.Summary = data.Summary;

                if ($scope.notes.indexOf(curr) < 0) {
                    curr.selected = true;
                    $scope.notes.splice(0, 0, curr);
                }
            }, ensureAuth, function () {
                saveTimeout = null;
                curr.saving = false;
            });
        }, 1000);
    });

    // Auth

    $scope.auth = function () {
        if (!validateSignInInfo()) {
            return;
        }

        if ($scope.authMode === 1) {
            signup();
        } else if ($scope.authMode === 2) {
            signin();
        } else if ($scope.authMode === 3) {
            recoverPassword();
        } else {
            $scope.authMode = 0;
        }
    };

    $scope.signout = function () {
        if (!clientData.token()) {
            return;
        }

        user.signout({ Token: clientData.token() }, null, null, ensureSignedOut);
    };

    var signinWithToken = function () {
        if (!clientData.token()) {
            return;
        }

        user.signinWithToken({ Token: clientData.token() }, function (resp) {
            $scope.loggedin = true;
            $scope.notes = resp.Data.Notes;
        }, function (resp) {
            clientData.token(null);
            console.log(resp.ResponseCode + ":" + resp.ResponseMessage);
        });
    };

    signinWithToken();

    // Editor

    document.getElementById("editor").addEventListener("keydown", function (e) {
        var keyCode = e.keyCode || e.which;

        if (keyCode !== 9) {
            return;
        }

        e.preventDefault();

        var txt = e.target;

        var start = txt.selectionStart;
        var end = txt.selectionEnd;

        txt.value = txt.value.substring(0, start) + "    " + txt.value.substring(end);
        txt.selectionStart = txt.selectionEnd = start + 4;
    });
}]);
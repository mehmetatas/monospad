angular.module("monospad", ["ngStorage", "ngRoute"])
	.config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
	    $locationProvider.html5Mode(true);

	    $routeProvider
            .when("/", {
                templateUrl: "/app/html/app.html",
                controller: "appCtrl"
            })
            .when("/recover/:token", {
                templateUrl: "/app/html/recover.html",
                controller: "recoverCtrl"
            })
            .when("/note/:token", {
                templateUrl: "/app/html/note.html",
                controller: "noteCtrl"
            })
            .otherwise({
                redirectTo: "/"
            });
	}])
    .controller("appCtrl", [
        "$scope", "$rootScope", "$timeout", "$window", "api", "clientData", "block", function ($scope, $rootScope, $timeout, $window, api, clientData, block) {
            var saveTimeout;
            var skipSave;

            $scope.loggedin = false;
            $scope.current = null;
            $scope.notes = [];
            $scope.signinInfo = {};

            $rootScope.blocker = {};

            $scope.showContentPanel = false;

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
                $scope.searchKey = "";
                $scope.authMode = 0;
                $scope.notes = resp.Data.Notes;
                if ($scope.current && $scope.current.Content) {
                    $scope.current = $scope.notes[0];
                    $scope.current.selected = true;
                }
                clientData.token(resp.Data.Token);
                $scope.signinInfo = {};
            };

            var ensureSignedOut = function () {
                $scope.loggedin = false;
                $scope.searchKey = "";
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

                block(
                    api.user.signup(req, processAuthSuccess)
                );
            };

            var signin = function () {
                var req = {
                    Email: $scope.signinInfo.Email,
                    Password: $scope.signinInfo.Password
                };

                if ($scope.current) {
                    req.UnsavedNoteContent = $scope.current.Content;
                }

                block(
                    api.user.signin(req, processAuthSuccess)
                );
            };

            var recoverPassword = function () {
                var req = {
                    Email: $scope.signinInfo.Email
                };

                block(
                    api.user.recoverPassword(req, function () {
                        alert("a password recovery mail has sent to your email address: " + $scope.signinInfo.Email);
                        ensureSignedOut();
                    })
                );
            };

            var saveToServer = function () {
                if (skipSave) {
                    skipSave = false;
                    return;
                }

                var curr = $scope.current;

                if (!curr.Content) {
                    $timeout.cancel(saveTimeout);
                    saveTimeout = null;

                    api.note.saveNote({
                        Id: curr.Id,
                        Content: curr.Content
                    });

                    curr.Title = "";
                    curr.Summary = "";
                    return;
                }

                if (saveTimeout) {
                    $timeout.cancel(saveTimeout);
                }

                saveTimeout = $timeout(function () {
                    api.note.saveNote({
                        Id: curr.Id,
                        Content: curr.Content
                    }, function (resp) {
                        var data = resp.Data;

                        curr.Id = data.Id;
                        curr.Title = data.Title;
                        curr.Summary = data.Summary;

                        if ($scope.notes.indexOf(curr) < 0) {
                            if (!$scope.current) {
                                curr.selected = true;
                                $scope.current = curr;
                            }
                            $scope.notes.splice(0, 0, curr);
                        }

                        curr.saved = true;
                        $timeout(function () {
                            curr.saved = false;
                        }, 2000);

                    }, ensureAuth, function () {
                        saveTimeout = null;
                    });
                }, 1000);
            };

            var saveToLocal = function () {

            };

            var isMobile = function () {
                return $window.innerWidth < 768;
            };

            var isMobileDevice = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);

            // Common

            $scope.toggleContentPanel = function (state) {
                if (isMobile()) {
                    $scope.showContentPanel = state;
                }
            };

            $scope.backToList = function () {
                $scope.toggleContentPanel(false);
                if ($scope.current) {
                    if (!$scope.current.Title) {
                        $scope.deleteNote($scope.current);
                    }

                    $scope.current.selected = false;
                    skipSave = true;
                    $scope.current = null;
                }
            };

            var resizeTimeout;
            angular.element($window).bind("resize", function () {
                if (resizeTimeout) {
                    $timeout.cancel(resizeTimeout);
                }
                resizeTimeout = $timeout(function () {
                    $scope.showContentPanel = $scope.current && isMobile();
                }, 100);
            });

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
                $scope.searchKey = "";
                $scope.toggleContentPanel(true);
                scrollDiv("noteList", 0, 250);
                document.getElementById("editor").focus();
            };

            $scope.loadNote = function (n) {
                if (n.selected) {
                    skipSave = true;
                    n.selected = false;
                    $scope.current = null;
                    return;
                }

                if ($scope.current) {
                    $scope.current.selected = false;
                }

                n.selected = true;
                $scope.toggleContentPanel(true);

                if (n.Content || !n.Id) {
                    skipSave = true;
                    $scope.current = n;
                    return;
                }

                block(
                    api.note.getContent({ Id: n.Id }, function (resp) {
                        n.Content = resp.Data.Content;
                        skipSave = true;
                        $scope.current = n;
                    }, ensureAuth)
                );
            };

            $scope.deleteNote = function (n) {
                if (n.Title && !confirm("are you sure you want to delete '" + n.Title + "'?")) {
                    return;
                }

                block(
                    api.note.deleteNote({ Id: n.Id }, function () {
                        var i = $scope.notes.indexOf(n);

                        if (i > -1) {
                            $scope.notes.splice(i, 1);
                        }

                        if (n === $scope.current) {
                            skipSave = true;
                            $scope.current = null;
                        }
                    }, ensureAuth)
                );
            };

            $scope.shareNote = function (n) {
                prompt("copy url", "https://monospad.com/note/" + n.AccessToken);
            };

            $scope.$watch("current.Content", function () {
                if ($scope.loggedin) {
                    saveToServer();
                } else {
                    saveToLocal();
                }
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

                if (!confirm("are you sure you want to sign out?")) {
                    return;
                }

                block(
                    api.user.signout({ Token: clientData.token() }, null, null, ensureSignedOut)
                );
            };

            $scope.changePassword = function () {
                block(
                    api.user.changePassword({
                        NewPassword: $scope.newPassword
                    }, function (resp) {
                        $scope.newPassword = "";
                        $scope.showChangePassword = false;
                        clientData.token(resp.Data.Token);
                        alert("password changed!");
                    })
                );
            };

            var signinWithToken = function () {
                if (!clientData.token()) {
                    return;
                }

                block(
                    api.user.signinWithToken({ Token: clientData.token() }, function (resp) {
                        $scope.loggedin = true;
                        $scope.notes = resp.Data.Notes;
                    }, function (resp) {
                        clientData.token(null);
                        console.log(resp.ResponseCode + ":" + resp.ResponseMessage);
                    })
                );
            };

            signinWithToken();

            // Editor

            var editor = document.getElementById("editor");

            var downTimeout;
            var touchStartTime;

            var getUrl = function (txt) {
                var start = txt.selectionStart;
                var end = txt.selectionEnd;

                if (start !== end) {
                    if (isMobileDevice) {
                        end = start;
                    } else {
                        return null;
                    }
                }

                var val = txt.value;

                while (start > -1 &&
                    val.charAt(start) !== " " &&
                    val.charAt(start) !== "\n" &&
                    val.charAt(start) !== "\r" &&
                    val.charAt(start) !== "\t") {
                    start--;
                }

                while (end < val.length &&
                    val.charAt(end) !== " " &&
                    val.charAt(end) !== "\n" &&
                    val.charAt(end) !== "\r" &&
                    val.charAt(end) !== "\t") {
                    end++;
                }

                var text = val.substring(start + 1, end);

                if (text.indexOf("http://") === 0 ||
                    text.indexOf("https://") === 0) {
                    return text;
                }

                return null;
            };

            var handleMouseDown = function () {
                downTimeout = $timeout(function () {
                    var url = getUrl(editor);

                    if (url) {
                        window.open(url);
                    }
                }, 1000);
            };

            var handleMouseUp = function () {
                if (downTimeout) {
                    $timeout.cancel(downTimeout);
                    downTimeout = null;
                }
            };

            var clickHandler = function () {
                var url = getUrl(editor);

                if (!url) {
                    return;
                }

                var a = document.createElement("a");
                a.setAttribute("href", url);
                a.setAttribute("target", "_blank");

                var dispatch = document.createEvent("HTMLEvents");
                dispatch.initEvent("click", true, true);
                a.dispatchEvent(dispatch);

                //editor.removeEventListener("click", clickHandler);
            };

            //var handleTouchStart = function () {
            //    touchStartTime = new Date();
            //};

            //var handleTouchEnd = function () {
            //    var touchDuration = new Date() - touchStartTime;

            //    if (touchDuration < 1000) {
            //        return;
            //    }

            //    editor.addEventListener("click", clickHandler);

            //    var dispatch = document.createEvent("HTMLEvents");
            //    dispatch.initEvent("click", true, true);
            //    editor.dispatchEvent(dispatch);
            //};

            if (isMobileDevice) {
                //editor.addEventListener("touchstart", handleTouchStart);
                //editor.addEventListener("touchend", handleTouchEnd);
                editor.addEventListener("click", clickHandler);
            } else {
                editor.addEventListener("mousedown", handleMouseDown);
                editor.addEventListener("mouseup", handleMouseUp);
            }

            editor.addEventListener("keydown", function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode !== 9) {
                    return;
                }

                e.preventDefault();

                var txt = e.target;

                var start = txt.selectionStart;
                var end = txt.selectionEnd;

                var val = txt.value;

                var newLine = "\n";

                var isMultilineSelection = val.substring(start, end).indexOf(newLine) > 0;

                if (!isMultilineSelection) {
                    txt.value = val.substring(0, start) + "    " + val.substring(end);
                    txt.selectionStart = txt.selectionEnd = start + 4;
                    return;
                }

                var initialLineBreakIndex = -1;
                var tmp = 0;
                while (tmp < start) {
                    initialLineBreakIndex = tmp;
                    tmp = val.indexOf(newLine, tmp + 1);
                }

                var lastLineBreakIndex = val.indexOf(newLine, end);
                if (lastLineBreakIndex < 0) {
                    lastLineBreakIndex = val.length;
                }

                var before = val.substring(0, initialLineBreakIndex + 1);
                var selection = val.substring(initialLineBreakIndex + 1, lastLineBreakIndex);
                var after = val.substring(lastLineBreakIndex);

                var lines = selection.split(newLine);
                selection = "";
                for (var i = 0; i < lines.length; i++) {
                    if (e.shiftKey) {
                        if (lines[i].indexOf("    ") === 0) {
                            selection += lines[i].substring(4) + newLine;
                        }
                        else if (lines[i].indexOf("   ") === 0) {
                            selection += lines[i].substring(3) + newLine;
                        }
                        else if (lines[i].indexOf("  ") === 0) {
                            selection += lines[i].substring(2) + newLine;
                        }
                        else if (lines[i].indexOf(" ") === 0) {
                            selection += lines[i].substring(1) + newLine;
                        }
                        else {
                            selection += lines[i] + newLine;
                        }
                    } else {
                        selection += "    " + lines[i] + newLine;
                    }
                }
                selection = selection.substring(0, selection.length - 1);

                txt.value = before + selection + after;
                txt.selectionStart = initialLineBreakIndex + 1;
                txt.selectionEnd = initialLineBreakIndex + selection.length + 1;
            });
        }
    ])
	.controller("recoverCtrl", ["$scope", "$rootScope", "$location", "$routeParams", "api", "clientData", "block", function ($scope, $rootScope, $location, $routeParams, api, clientData, block) {
	    $rootScope.blocker = {};

	    block(
            $scope.resetPassword = function () {
                api.user.resetPassword({
                    Token: $routeParams.token,
                    NewPassword: $scope.newPassword
                }, function (resp) {
                    clientData.token(resp.Data.Token);
                    $location.url("/");
                });
            }
        );

	    clientData.token(null);
	}])
	.controller("noteCtrl", ["$scope", "$rootScope", "$location", "$routeParams", "api", "clientData", "block", function ($scope, $rootScope, $location, $routeParams, api, clientData, block) {
	    $rootScope.blocker = {};

	    block(
            api.note.getNoteByAccessCode({ AccessCode: $routeParams.token },
                function (resp) {
                    $scope.content = resp.Data.Content;
                })
        );
	}])
    .directive("ngEnter", function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive("ngEsc", function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 27) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEsc);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive("a", function () {
        return {
            restrict: "E",
            link: function (scope, elem, attrs) {
                if (attrs.ngClick || attrs.href === "" || attrs.href === "#") {
                    elem.on("click", function (e) {
                        e.preventDefault();
                    });
                }
            }
        };
    })
    .factory("block", ["$rootScope", function ($rootScope) {
        return function (promise) {
            $rootScope.blocker.block = true;
            promise.finally(function () {
                $rootScope.blocker.block = false;
            });
        };
    }])
    .service("api", [
        "$rootScope", "$http", "clientData", function ($rootScope, $http, clientData) {
            var send = function (method, controller, action, data, success, error, complete) {
                var token = clientData.token();

                var opts = {
                    method: method,
                    url: "/api/" + controller + "/" + action,
                    headers: {
                        "monospad-auth-token": token || ""
                    }
                };

                if (method === "GET" || method === "DELETE") {
                    opts.params = data;
                } else {
                    opts.data = data;
                }

                return $http(opts)
                    .success(function (resp) {
                        if (resp.ResponseCode === 0 && success) {
                            success(resp);
                        } else if (resp.ResponseCode !== 0) {
                            if (error) {
                                error(resp);
                            } else {
                                alert(resp.ResponseMessage + " [" + resp.ResponseCode + "]");
                            }
                        }
                    })
                    .error(function () {
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

            var buildService = function (serviceName, serviceDef) {
                var service = {};

                for (var prop in serviceDef) {
                    if (!serviceDef.hasOwnProperty(prop)) {
                        continue;
                    }

                    var value = serviceDef[prop];

                    if (prop === "GET" || prop === "POST" || prop === "DELETE" || prop === "PUT") {
                        var methods = value.split(",");
                        for (var i = 0; i < methods.length; i++) {
                            var methodName = methods[i];

                            service[methodName] = (function (p, m) {
                                return function (data, success, error, complete) {
                                    return send(p, serviceName, m, data, success, error, complete);
                                };
                            })(prop, methodName);
                        }
                    }
                }

                return service;
            };

            for (var serviceName in serviceDefs) {
                if (serviceDefs.hasOwnProperty(serviceName)) {
                    this[serviceName] = buildService(serviceName, serviceDefs[serviceName]);
                }
            }
        }
    ])
    .service("clientData", [
        "$localStorage", function ($localStorage) {
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
        }
    ]);
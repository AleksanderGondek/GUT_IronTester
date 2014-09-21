var IronTesterAngularApp = angular.module("IronTesterAngularApp", []);
IronTesterAngularApp.value('$', $);

IronTesterAngularApp.controller("allRequestsController", function($, $scope) {
    $scope.requests = [];
    // Scope request content example ;d
    //[
    //    {
    //        requestId: "000000000000001",
    //        requestState: "SomeState1",
    //        requestProgress: "23",
    //        requestFailureReason: null,
    //        requestClass: null
    //    },
    //    {
    //        requestId: "000000000000002",
    //        requestState: "SomeState2",
    //        requestProgress: "63",
    //        requestFailureReason: null,
    //        requestClass: null
    //    },
    //    {
    //        requestId: "000000000000003",
    //        requestState: "SomeState3",
    //        requestProgress: "93",
    //        requestFailureReason: null,
    //        requestClass: null
    //    }
    //];

    $scope.allRequestsHub = $.connection.allRequestsHub;

    $scope.getRequestClass = function(requestState) {
        switch(requestState) {
            case "Failed":
                return "danger";
            case "Cancelled":
                return "warning";
            case "Finished":
                return "success";
            default:
                return null;
        }
    };

    $scope.allRequestsHub.client.requestStateChanged = function (requestId, requestState, requestProgress, requestFailureReason) {
        var newRequest = {
            requestId: requestId,
            requestState: requestState,
            requestProgress: requestProgress,
            requestFailureReason: requestFailureReason,
            requestClass: $scope.getRequestClass(requestState)
            };

        //Check if already exists
        var existedInTable = false;
        for (var i = 0; i < $scope.requests.length; i++) {
            if ($scope.requests[i].requestId == newRequest.requestId) {
                $scope.requests[i].requestState = newRequest.requestState;
                $scope.requests[i].requestProgress = newRequest.requestProgress;
                $scope.requests[i].requestFailureReason = newRequest.requestFailureReason;
                existedInTable = true;

                if ($scope.requests[i].requestState == "Finished") {
                    $scope.requests[i].requestProgress = "100";
                }
            }
        }

        if (!existedInTable) {
            $scope.requests.push(newRequest);
        }

        $scope.$apply();
    };

    $scope.allRequestsHub.client.consumeStats = function (timestamp, totalNumberOfRequests, failedRequests, cancelledRequests,
    validatedRequests, initializedRequests, buildRequests, testedRequests, finishedRequests) {
        //Do nothing
    };

    $scope.pleaseCancel = function (requestId) {
        var r = confirm("Do you really want to cancel: "+requestId + " ?");
        if (r == true) {
            $scope.allRequestsHub.server.cancel(requestId);
        }
    };

    $scope.pleaseRestart = function (requestId) {
        var r = confirm("Do you really want to restart: " + requestId + " ?");
        if (r == true) {
            $scope.allRequestsHub.server.restart(requestId);
        }
    };

    $.connection.hub.start();

    //Get all requests so far (recevied by this endpoint)
    //$scope.allRequestsHub.server.getAllRequests();
});
var IronTesterAngularApp = angular.module("IronTesterAngularApp", []);
IronTesterAngularApp.value('$', $);

IronTesterAngularApp.controller("allRequestsController", function($, $scope) {
    $scope.requests = [
        {
            requestId: "000000000000001",
            requestState: "SomeState1",
            requestProgress: "23",
            requestFailureReason: null
        },
        {
            requestId: "000000000000002",
            requestState: "SomeState2",
            requestProgress: "63",
            requestFailureReason: null
        },
        {
            requestId: "000000000000003",
            requestState: "SomeState3",
            requestProgress: "93",
            requestFailureReason: null
        }
    ];

    $scope.allRequestsHub = $.connection.allRequestsHub; 

    $scope.allRequestsHub.client.requestStateChanged = function (requestId, requestState, requestProgress, requestFailureReason) {
        var newRequest = {
            requestId: requestId,
            requestState: requestState,
            requestProgress: requestProgress,
            requestFailureReason: requestFailureReason
        };

        $scope.requests.push(newRequest);

        $scope.$apply();
    };

    $.connection.hub.start();
});
MahmudsUMSApp.factory('AllocatedRoomsService', ['$http', function ($http) {

    var AllocatedRoomsService = {};
    AllocatedRoomsService.getAllocatedRooms = function () {
        return $http.get('/AllocatedRooms/JsonData');
    };
    return AllocatedRoomsService;

}]);
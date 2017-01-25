MahmudsUMSApp.factory('AssignedCoursesService', ['$http', function ($http) {

    var AssignedCoursesService = {};
    AssignedCoursesService.getAssignedCourses = function () {
        return $http.get('/AssignedCourses/JsonData');
    };
    return AssignedCoursesService;

}]);
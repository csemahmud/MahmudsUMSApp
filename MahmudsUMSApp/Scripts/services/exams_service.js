MahmudsUMSApp.factory('ExamsService', ['$http', function ($http) {

    var ExamsService = {};
    ExamsService.getExams = function () {
        return $http.get('/Exams/JsonData');
    };
    return ExamsService;

}]);
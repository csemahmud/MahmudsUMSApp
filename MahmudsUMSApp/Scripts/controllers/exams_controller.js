MahmudsUMSApp.controller('ExamsController',
    function ($scope, ExamsService, ALL_STR) {
        getExams(ALL_STR);
        function getExams(ALL_STR) {
            ExamsService.getExams()
            .success(function (responce) {
                $scope.students = responce.students;
                console.log($scope.students);

                $scope.student = null;

                $scope.message = "Enrolled Courses : ";
                $scope.regNo = ALL_STR;
                $scope.allStr = ALL_STR;

                $scope.limitRange = [];
                $scope.pageSize = 1;
                $scope.limitRange.push(1);
                $scope.selectedPage = 1;

                $scope.resetPage = function () {
                    $scope.selectedPage = 1;
                };

                $scope.changePage = function () {
                    $scope.student = null;
                    for (var i = 0; i < $scope.students.length; i++) {
                        if ($scope.students[i].RegNo == $scope.regNo) {
                            $scope.student = $scope.students[i];
                            break;
                        }
                    }

                    $scope.limitRange = [];
                    $scope.limitRange.push(1);
                    $scope.selectedPage = 1;

                    if ($scope.student != null) {

                        var maxPageSize = 10;

                        if ($scope.student.Exams.length < 10) {
                            maxPageSize = $scope.student.Exams.length;
                        }

                        for (var i = 2; i <= maxPageSize; i++) {
                            $scope.limitRange.push(i);
                        }

                        $scope.pageSize = 5;

                        if ($scope.student.Exams.length < 5) {
                            $scope.pageSize = $scope.student.Exams.length;
                        }

                    } else {
                        $scope.pageSize = 1;
                    }
                };

                $scope.getEnrollmentDate = function (dateStr) {
                    return new Date(dateStr);
                };

                $scope.selectPage = function (pageNo) {
                    $scope.selectedPage = pageNo;
                };

                $scope.getBtnClass = function (pageNo) {
                    return $scope.selectedPage == pageNo ? "active" : "";
                };
            })
            .error(function (error) {
                $scope.status = "Unable to load Exam data: " + error.message;
                console.log($scope.status);
                alert($scope.status);
            });
        }
    });
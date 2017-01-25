MahmudsUMSApp.controller('AssignedCoursesController',
    function ($scope, AssignedCoursesService, ALL_STR) {
        getAssignedCourses(ALL_STR);
        function getAssignedCourses(ALL_STR) {
            AssignedCoursesService.getAssignedCourses()
            .success(function (responce) {
                $scope.assignedCourses = responce.assignedCourses;
                console.log($scope.assignedCourses);

                $scope.message = "Assigned Courses : ";
                $scope.deptCode = ALL_STR;
                $scope.allStr = ALL_STR;

                $scope.limitRange = [];
                var maxPageSize = 10;
                if ($scope.assignedCourses.length < 10) {
                    maxPageSize = $scope.assignedCourses.length;
                }

                for (var i = 1; i <= maxPageSize; i++) {
                    $scope.limitRange.push(i);
                }

                $scope.pageSize = 5;
                if ($scope.assignedCourses.length < 5) {
                    $scope.pageSize = $scope.assignedCourses.length;
                }

                $scope.selectedPage = 1;
                $scope.resetPage = function () {
                    $scope.selectedPage = 1;
                };

                $scope.resetPageNSize = function () {
                    $scope.pageSize = 5;
                    $scope.selectedPage = 1;

                    $scope.limitRange = [];
                    maxPageSize = 10;

                    if ($scope.deptCode != $scope.allStr) {
                        var count = 0;

                        for (var i = 0; i < $scope.assignedCourses.length; i++) {
                            if ($scope.assignedCourses[i].DeptCode == $scope.deptCode) {
                                count++;
                            }
                        }

                        if (count < 10) {
                            maxPageSize = count;
                        }

                        for (var i = 1; i <= maxPageSize; i++) {
                            $scope.limitRange.push(i);
                        }

                        if (count < 5) {
                            $scope.pageSize = count;
                        }
                    } else {
                        if ($scope.assignedCourses.length < 10) {
                            maxPageSize = $scope.assignedCourses.length;
                        }

                        for (var i = 1; i <= maxPageSize; i++) {
                            $scope.limitRange.push(i);
                        }

                        if ($scope.assignedCourses.length < 5) {
                            $scope.pageSize = $scope.assignedCourses.length;
                        }
                    }
                };

                $scope.isCourseSelected = function (course) {
                    return $scope.deptCode == $scope.allStr || $scope.deptCode == course.DeptCode;
                };

                $scope.selectPage = function (pageNo) {
                    $scope.selectedPage = pageNo;
                };

                $scope.getBtnClass = function (pageNo) {
                    return $scope.selectedPage == pageNo ? "active" : "";
                };
            })
            .error(function (error) {
                $scope.status = "Unable to load Assigned Courses data: " + error.message;
                console.log($scope.status);
                alert($scope.status);
            });
        }
    });
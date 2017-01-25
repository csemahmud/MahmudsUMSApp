MahmudsUMSApp.controller('AllocatedRoomsController',
    function ($scope, AllocatedRoomsService, ALL_STR) {
        getAllocatedRooms(ALL_STR);
        function getAllocatedRooms(ALL_STR) {
            AllocatedRoomsService.getAllocatedRooms()
            .success(function (responce) {
                $scope.courses = responce.courses;
                console.log($scope.courses);

                $scope.message = "Courses with Allocated Rooms : ";
                $scope.deptCode = ALL_STR;
                $scope.allStr = ALL_STR;

                $scope.limitRange = [];
                var maxPageSize = 10;
                if ($scope.courses.length < 10) {
                    maxPageSize = $scope.courses.length;
                }

                for (var i = 1; i <= maxPageSize; i++) {
                    $scope.limitRange.push(i);
                }

                $scope.pageSize = 5;
                if ($scope.courses.length < 5) {
                    $scope.pageSize = $scope.courses.length;
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

                        for (var i = 0; i < $scope.courses.length; i++) {
                            if ($scope.courses[i].DeptCode == $scope.deptCode) {
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
                        if ($scope.courses.length < 10) {
                            maxPageSize = $scope.courses.length;
                        }

                        for (var i = 1; i <= maxPageSize; i++) {
                            $scope.limitRange.push(i);
                        }

                        if ($scope.courses.length < 5) {
                            $scope.pageSize = $scope.courses.length;
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
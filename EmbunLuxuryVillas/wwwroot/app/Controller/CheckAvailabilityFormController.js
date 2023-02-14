var checkAvailabilityFormController = ["$scope", "$http", function ($scope, $http) {
    $scope.searchHotel = function (checkAvailabilityForm) {
        if (!checkAvailabilityForm.$invalid) {
            let currentDate = new Date();
            let formatedCurrentDate = (currentDate.getMonth() + 1) + "/" + (currentDate.getDate() + 7) + "/" + currentDate.getFullYear();
            let formatedNextDate = (currentDate.getMonth() + 1) + "/" + (currentDate.getDate() + 8) + "/" + currentDate.getFullYear();

            if (!$('#checkInDatePicker').val())
                $('#checkInDatePicker').val(formatedCurrentDate);

            if (!$('#checkOutDatePicker').val())
                $('#checkOutDatePicker').val(formatedNextDate);

            window.open("/Reservation?startDate=" + $('#checkInDatePicker').val() + "&endDate=" + $('#checkOutDatePicker').val(), "_blank");
        }
    };
}];
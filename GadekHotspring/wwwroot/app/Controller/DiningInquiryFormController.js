var diningInquiryFormController = ["$scope", "$http", "$filter", function ($scope, $http, $filter) {
    $scope.form = {
        Salutation: "Mr",
        Name: "",
        Email: "",
        Phone: "",
        DateTime: "",
        DiningOption: "Weekday lunch",
        Adults: null,
        Children: null,
        Message: ""
    };
    $scope.DiningDate = "";
    $scope.DiningTime = new Date(1970, 1, 1, 12, 00, 00);

    $scope.onDiningInquiryFormSubmit = function () {
        $scope.DiningDate = new Date($scope.DiningDate);

        $scope.DiningDate = $filter('date')($scope.DiningDate, 'dd MMM yyyy');
        $scope.form.DateTime = `${$scope.DiningDate} ${$scope.DiningTime}`;

        $http.post("/Home/SendDiningInquiryEmail", $scope.form).then(function successCallback(response) {
            swal({
                title: 'Email sent!',
                text: `We will get back to you as soon as possible`,
                type: 'success'
            });
            diningInquiryForm.reset();
            $scope.EventDateFrom = "";
            $scope.EventDateTo = "";
        }, function errorCallback(response) {
            swal("Oops!", response.statusText, "error");
        });
    }
}]
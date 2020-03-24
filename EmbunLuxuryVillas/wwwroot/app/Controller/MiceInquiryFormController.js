var miceInquiryFormController = ["$scope", "$http", "$filter", function ($scope, $http, $filter) {
    $scope.form = {
        Package: [],
        EventDate: "",
        NoPax: null,
        Budget: null,
        Salutation: "Mr",
        Name: "",
        CompanyName: "",
        Email: "",
        Phone: "",
        Message: ""
    };
    $scope.EventDateFrom = "";
    $scope.EventDateTo = "";

    $scope.onMiceInquiryFormSubmit = function () {
        var packages = $('.chosen-select').val();
        $scope.form.Package = packages.join(', ');
        $scope.EventDateFrom = new Date($scope.EventDateFrom);
        $scope.EventDateTo = new Date($scope.EventDateTo);

        $scope.EventDateFrom = $filter('date')($scope.EventDateFrom, 'dd MMM yyyy');
        $scope.EventDateTo = $filter('date')($scope.EventDateTo, 'dd MMM yyyy');
        $scope.form.EventDate = `${$scope.EventDateFrom} - ${$scope.EventDateTo} `;

        $http.post("/Home/SendMiceInquiryEmail", $scope.form).then(function successCallback(response) {
            swal({
                title: 'Email sent!',
                text: `We will get back to you as soon as possible`,
                type: 'success'
            });
            miceInquiryForm.reset();
            $scope.EventDateFrom = "";
            $scope.EventDateTo = "";
            $('.chosen-select').trigger('chosen:updated');
        }, function errorCallback(response) {
            swal("Oops!", response.statusText, "error");
        });
    }
}]
var tourPackageInquiryFormController = ["$scope", "$http","$filter", function ($scope, $http, $filter) {
    $scope.form = {
        Package: [],
        TravelDate:"",
        Salutation: "Mr",
        Name: "",
        CompanyName: "",
        Email: "",
        Phone: "",
        Message: ""
    };
    $scope.TravelDateFrom = "";
    $scope.TravelDateTo = "";

    $scope.onTourPackageInquiryFormSubmit = function () {
        var packages = $('.chosen-select').val();
        $scope.form.Package = packages.join(', ');
        $scope.TravelDateFrom = new Date($scope.TravelDateFrom);
        $scope.TravelDateTo = new Date($scope.TravelDateTo);

        $scope.TravelDateFrom = $filter('date')($scope.TravelDateFrom, 'dd MMM yyyy');
        $scope.TravelDateTo = $filter('date')($scope.TravelDateTo, 'dd MMM yyyy');
        $scope.form.TravelDate = `${$scope.TravelDateFrom} - ${$scope.TravelDateTo} `;

        $http.post("/Home/SendTourPackageInquiryEmail", $scope.form).then(function successCallback(response) {
            swal({
                title: 'Email sent!',
                text: `We will get back to you as soon as possible`,
                type: 'success'
            });
            tourPackageInquiryForm.reset();
            $scope.TravelDateFrom = "";
            $scope.TravelDateTo = "";
            $('.chosen-select').trigger('chosen:updated');
        }, function errorCallback(response) {
            swal("Oops!", response.statusText, "error");
        });
    }
}]
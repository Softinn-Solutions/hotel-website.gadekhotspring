var eventInquiryFormController = ["$scope", "$http", "$filter", function ($scope, $http, $filter) {
    $scope.form = {
        Package: [],
        Name: "",
        CompanyName: "",
        Email: "",
        Phone: "",
        Message: ""
    };

    $scope.onEventInquiryFormSubmit = function () {
        var packages = $('.chosen-select').val();
        $scope.form.Package = packages.join(', ');

        $http.post("/Home/SendEventInquiryEmail", $scope.form).then(function successCallback(response) {
            swal({
                title: 'Email sent!',
                text: `We will get back to you as soon as possible`,
                type: 'success'
            });
            eventInquiryForm.reset();

            $('.chosen-select').trigger('chosen:updated');
        }, function errorCallback(response) {
            swal("Oops!", response.statusText, "error");
        });
    }
}]
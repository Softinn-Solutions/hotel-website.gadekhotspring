var contactFormController = ["$scope","$http",function ($scope, $http) {
    $scope.form = {
        Name: "",
        Email: "",
        Subject: "",
        Phone: "",
        Message: ""
    };

    $scope.onContactFormSubmit = function () {
        $http.post("/Home/SendEmail", $scope.form).then(function successCallback(response) {
            swal("Email sent!", response.statusText, "success");
            contactForm.reset();
        }, function errorCallback(response) {
            swal("Oops!", response.statusText, "error");
        });
    }
}]
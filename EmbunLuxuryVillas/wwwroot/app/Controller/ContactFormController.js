var contactFormController = ["$scope", "$http", function ($scope, $http) {
    $scope.form = {
        Salutation: "Mr",
        Name: "",
        Email: "",
        Subject: "",
        Phone: "",
        Message: ""
    };

    $scope.isLoading = false;

    $scope.onContactFormSubmit = function () {
        $scope.isLoading = true;
        $http.post("/Home/SendEmail", $scope.form).then(function successCallback(response) {
            $scope.isLoading = false;
            swal("Email sent!", response.statusText, "success");
            contactForm.reset();
        }, function errorCallback(response) {
            $scope.isLoading = false;
            swal("Oops!", response.statusText, "error");
        });
    }
}]
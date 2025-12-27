var app = angular.module("cloudApp", []);

app.controller("LoginController", function ($scope, $http) {

  $scope.user = {};
  $scope.loading = false;

  $scope.login = function () {
    console.log("Login button clicked");

    $scope.loading = true;
    $scope.successMessage = "";
    $scope.errorMessage = "";

    $http.post(
      "http://localhost:5116/api/auth/login",
      {
        email: $scope.user.email,
        password: $scope.user.password
      },
      {
        withCredentials: true
      }
    )
    .then(function (response) {
      console.log("API response:", response.data);

      if (response.data.success) {
        $scope.successMessage = response.data.message;
      } else {
        $scope.errorMessage = response.data.message;
      }
    })
    .catch(function (error) {
      console.error("Login error:", error);
      $scope.errorMessage = "Login failed";
    })
    .finally(function () {
      $scope.loading = false;
    });
  };
});

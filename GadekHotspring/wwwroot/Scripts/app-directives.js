var softinn;
(function (softinn) {
    var directives;
    (function (directives) {
        'use strict';
        var comboDate = (function () {
            function comboDate() {
                this.require = 'ngModel';
            }
            comboDate.prototype.link = function ($scope, element, attributes) {
                $scope.$watch(attributes['ngModel'], function (newValue) {
                    var tempElement = element;
                    var newTime = moment(newValue).format("h:mm a");
                    tempElement.combodate('destroy');
                    tempElement.combodate('setValue', newTime);
                });
            };
            comboDate.Factory = function () {
                var directive = function () {
                    return new comboDate();
                };
                directive['$inject'] = [];
                return directive;
            };
            return comboDate;
        }());
        directives.comboDate = comboDate;
    })(directives = softinn.directives || (softinn.directives = {}));
})(softinn || (softinn = {}));

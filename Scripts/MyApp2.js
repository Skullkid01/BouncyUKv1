var app = angular.module('myApp', ['ui.calendar']);
app.controller('myNgController', ['$scope', '$http', 'uiCalendarConfig', function ($scope, $http, uiCalendarConfig) {

    $scope.SelectedEvent = null;
    var isFirstTime = true;

    $scope.events = [];
    $scope.eventSources = [$scope.events];


    $http.get('/books/getbooking', {
        cache: true,
        params: {}
    }).then(function (data) {
        $scope.events.slice(0, $scope.events.length);
        angular.forEach(data.data, function (value) {
            $scope.events.push({
                title: value.username,
                date: new Date(parseInt(value.BookingDate.substr(6))),
                BookRef: value.Booking,
                Address: value.DeliverAddress,
                Postal: value.PostalCode,
                Reference: value.Refer,
                Cell: value.Cell,
                Time: new Date(parseInt(value.Time.substr(6))),
                stick: true
            });
        });
    });

    $scope.uiConfig = {
        calendar: {
            height: 450,
            editable: true,
            displayEventTime: false,
            header: {
                left: 'month basicWeek ',
                center: 'title',
                right: 'today prev,next'
            },
            eventClick: function (event) {
                $scope.SelectedEvent = event;
            },

            eventAfterAllRender: function () {
                if ($scope.events.length > 0 && isFirstTime) {
                    
                    uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', $scope.events[0].start);
                    isFirstTime = false;
                }
            }
        }
    };

}])
//basicDay agendaWeek agendaDay
function logg(a, b) {
    console.log(a, b);
}

$(function () {
    var broadcaster = $.connection.firstHub;
    broadcaster.client.displayText = function (name, message) {
        logg(name, message);
        $('#messages').append('<li>' + name + ' said: ' + message + '</li>');
    };

    $.connection.hub.start().done(function () {
        $("#join").click(function () {
            logg("join clicked");
            broadcaster.server.join($('#groupName').val());
            broadcaster.state.GroupName = $('#groupName').val();
        });

        $("#leave").click(function () {
            logg("leave clicked");
            broadcaster.server.leave($('#groupName').val());
            broadcaster.state.GroupName = null;
        });


        $("#broadcast").click(function () {
            logg("broadcast clicked");
            broadcaster.server.broadcastMessage({
                Name: $('#name').val(),
                Message: $('#message').val()
            });
        });
    });
});
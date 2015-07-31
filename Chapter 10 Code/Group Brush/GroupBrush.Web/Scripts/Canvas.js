var hubProxy = null;
var userList = [];
var lastCursorPositionTime = 0;
var lastDrawTime = 0;
var lastSequence = 0;
var isBrushDown = false;
var firstTouch = null;
var brushPositions = [];
var otherBrushCursors = [];
var isDrawingContinued = false;
var cleanUpCursorCanvasTimer = null;
function Position(x, y) {
    this.x = x;
    this.y = y;
}
function OtherBrush(userName, x, y) {
    this.UserName = userName;
    this.x = x;
    this.y = y;
    this.updateTime = Date.now();
}
function CurrentBrushData() {
    this.Color = $('div.color.tool.selected').data('colorvalue') || 1;
    this.Size = $('select#sizes').val() || 1;
    this.Type = $('div.brush.tool.selected').data('brushtype') || 1;
}
function getCanvasId() {
    var canvasId = null;
    var queryString = window.location.search.substring(1);
    var queryStringArray = queryString.split('&');
    for (var x = 0; x < queryStringArray.length; x++) {
        var keyValue = queryStringArray[x].split('=');
        if (keyValue.length > 1) {
            var queryKey = keyValue[0];
            var queryValue = keyValue[1];
            if (queryKey == 'canvasId') {
                canvasId = queryStringArray[x];
            }
        }
    }
    if (canvasId == undefined || canvasId.length != 45) {
        window.location('/');
    }
    return canvasId;
}
function connectionChange(contentToShow) {
    $('div.connectionContent').hide();
    if (contentToShow != undefined)
        $(contentToShow).show();
}
$(document).ready(function () {
    $('div.color.tool').click(function () {
        $('div.color.tool').removeClass('selected');
        $(this).addClass('selected');
    });
    $('div.brush.tool').click(function () {
        $('div.brush.tool').removeClass('selected');
        $(this).addClass('selected');
    });
    $('div#reloadCanvasContent button#btnReload').click(function () {
        connect();
    });
    connect();
});
function connect() {
    connectionChange('div#loadingCanvasContent');
    var canvasId = getCanvasId();
    var connection = $.hubConnection('/signalr', { qs: canvasId });
    hubProxy = connection.createHubProxy('CanvasHub');
    hubProxy.on('MoveOtherCursor', function (userName, x, y) {
        drawOtherBrush(userName, x, y)
    });
    hubProxy.on('UserChatMessage', function (message) {
        $('#chatMessages').append('<li>' + message + '</li>')
    });
    hubProxy.on('UserConnected', function (userName) {
        userConnected(userName);
    });
    hubProxy.on('UserDisconnected', function (userName) {
        userDisconnected(userName);
    });
    hubProxy.on('DrawCanvasBrushAction', function (canvasBrushAction) {
        drawCanvasBrushAction(canvasBrushAction);
    });
    connection.reconnecting(function () {
        connectionChange('div#loadingCanvasContent');
    });
    connection.reconnected(function () {
        syncRoom();
    });
    connection.disconnected(function () {
        connectionChange('div#reloadCanvasContent');
    });
    connection.start().done(function () {
        $('#btnSendMessage').click(function () {
            hubProxy.invoke('SendChatMessage', $('#chatInput').val()).done(function ()
            { $('#chatInput').val(''); });
        });
        var canvasTouch = document.getElementById('scratchCanvas');
        canvasTouch.addEventListener('touchstart', touchStart, false);
        canvasTouch.addEventListener('touchmove', touchMove, false);
        canvasTouch.addEventListener('touchend', touchEnd, false);
        canvasTouch.addEventListener('touchleave', touchEnd, false);
        canvasTouch.addEventListener('touchcancel', touchEnd, false);
        if (window.navigator.msPointerEnabled) {
            canvasTouch.addEventListener('MSPointerDown', msTouchStart, false);
            canvasTouch.addEventListener('MSPointerMove', msTouchMove, false);
            canvasTouch.addEventListener('MSPointerUp', msTouchEnd, false);
        }
        else {
            var canvas = $('#scratchCanvas');
            canvas.bind('mousemove', mouseMove);
            canvas.bind('mousedown', mouseDown);
            canvas.bind('mouseup', mouseUp);
        }
        syncRoom();
    });
}
function syncRoom() {
    connectionChange('div#syncingCanvasContent');
    hubProxy.invoke('SyncToRoom', lastSequence).done(function (canvasSnapShot) {
        userList = [];
        $.each(canvasSnapShot.Users, function () {
            userConnected(this);
        });
        $.each(canvasSnapShot.Actions, function () {
            drawCanvasBrushAction(this);
        });
        $('h1#CanvasName').html(canvasSnapShot.CanvasName);
        $('span#CanvasDescription').html(canvasSnapShot.CanvasDescription);
        connectionChange('div#canvasContent');
    }).fail(function () {
        connectionChange('div#reloadCanvasContent');
    });
}
function drawOtherBrush(userName, x, y) {
    otherBrushCursors[userName] = new OtherBrush(userName, x, y);
    drawAllBrushes();
}
function drawAllBrushes() {
    if (cleanUpCursorCanvasTimer != null) clearTimeout(cleanUpCursorCanvasTimer);
    var dirtyCanvas = false;
    var c = document.getElementById("cursorCanvas");
    var ctx = c.getContext("2d");
    ctx.fillStyle = "#000000";
    ctx.clearRect(0, 0, 600, 400);
    for (var key in otherBrushCursors) {
        var currentBrush = otherBrushCursors[key];
        if (Date.now() - currentBrush.updateTime < 1000) {
            dirtyCanvas = true;
            ctx.fillRect(currentBrush.x - 5, currentBrush.y - 5, 10, 10);
            ctx.fillText(currentBrush.UserName, currentBrush.x + 10, currentBrush.y);
        }
    }
    if (dirtyCanvas) cleanUpCursorCanvasTimer = setTimeout(function () { drawAllBrushes(); }, 500);
}
function drawCanvasBrushAction(canvasBrushAction) {
    var c = document.getElementById('drawingCanvas');
    var ctx = c.getContext("2d");
    ctx.beginPath();
    lastSequence = canvasBrushAction.Sequence;
    if (canvasBrushAction.Type == 1 || canvasBrushAction.Type == 2) {
        var brushActionSize = canvasBrushAction.Size;
        ctx.strokeStyle = canvasBrushAction.Color;
        ctx.lineWidth = brushActionSize;
        if (canvasBrushAction.BrushPositions.length > 0) {
            ctx.moveTo(canvasBrushAction.BrushPositions[0].X, canvasBrushAction.
            BrushPositions[0].Y);
        }
        for (var x = 0; x < canvasBrushAction.BrushPositions.length; x++) {
            var position = canvasBrushAction.BrushPositions[x];
            ctx.lineTo(position.X, position.Y);
            if (canvasBrushAction.Type == 1)
                ctx.stroke();
            else if (canvasBrushAction.Type == 2)
                ctx.clearRect(position.X, position.Y, brushActionSize, brushActionSize)
        }
    }
    else if (canvasBrushAction.Type == 3) {
        ctx.fillStyle = canvasBrushAction.Color;
        ctx.fillRect(0, 0, 600, 400);
    }
    else if (canvasBrushAction.Type == 4) {
        ctx.clearRect(0, 0, 600, 400);
    }
    ctx.closePath();
}
function getDrawPosition(e) {
    var canvasRect = document.getElementById('scratchCanvas').getBoundingClientRect();
    var xPos = e.clientX - canvasRect.left;
    var yPos = e.clientY - canvasRect.top;
    return new Position(xPos, yPos);
}
function mouseMove(e) {
    canvasBrushMove(getDrawPosition(e));
}
function mouseDown(e) {
    canvasBrushDown(getDrawPosition(e));
}
function mouseUp(e) {
    canvasBrushUp(getDrawPosition(e));
}
function msTouchStart(e) {
    e.preventDefault();
    if (firstTouch == null && e.buttons == 1) {
        firstTouch = e.pointerId;
        canvasBrushDown(new Position(e.clientX, e.clientY));
    }
}
function msTouchMove(e) {
    e.preventDefault();
    if (firstTouch == e.pointerId) {
        canvasBrushMove(new Position(e.clientX, e.clientY));
    }
}
function msTouchEnd(e) {
    e.preventDefault();
    if (e.buttons == 0 && firstTouch == e.pointerId) {
        canvasBrushUp(new Position(e.clientX, e.clientY));
        firstTouch = null;
    }
}
function touchStart(e) {
    e.preventDefault();
    if (firstTouch == null && e.changedTouches.length > 0) {
        var touchData = e.changedTouches[0];
        firstTouch = touchData.identifier;
        canvasBrushDown(new Position(touchData.pageX, touchData.pageY));
    }
}
function touchMove(e) {
    e.preventDefault();
    for (var t = 0; t < e.changedTouches.length; t++) {
        if (e.changedTouches[t].identifier == firstTouch) {
            var touchData = e.changedTouches[t];
            canvasBrushMove(new Position(touchData.pageX, touchData.pageY));
        }
    }
}
function touchEnd(e) {
    e.preventDefault();
    for (var t = 0; t < e.changedTouches.length; t++) {
        if (e.changedTouches[t].identifier == firstTouch) {
            firstTouch = null;
            var touchData = e.changedTouches[t];
            canvasBrushUp(new Position(touchData.pageX, touchData.pageY));
        }
    }
}
function canvasBrushMove(position) {
    if (isBrushDown) {
        storeDrawCoordinates(position);
    }
    if (Date.now() - lastCursorPositionTime > 100) {
        lastCursorPositionTime = Date.now()
        hubProxy.invoke('MoveCursor', position.x, position.y);
    }
}
function canvasBrushDown(position) {
    isBrushDown = true;
    storeDrawCoordinates(position);
    return true;
}
function canvasBrushUp(position) {
    isBrushDown = false;
    storeDrawCoordinates(position);
    sendBrushData(brushPositions);
    brushPositions = [];
    isDrawingContinued = false;
    var c = document.getElementById("scratchCanvas");
    var ctx = c.getContext("2d");
    ctx.closePath();
    ctx.clearRect(0, 0, 600, 400);
}
function sendBrushData(brushPositionsData) {
    var currentBrushData = new CurrentBrushData();
    var brushData = {
        BrushPositions: brushPositionsData,
        ClientSequenceId: Date.now(),
        Color: currentBrushData.Color,
        Size: currentBrushData.Size,
        Type: currentBrushData.Type
    }
    hubProxy.invoke('SendDrawCommand', brushData).fail(function (error) {
    });
}
function storeDrawCoordinates(position) {
    var c = document.getElementById("scratchCanvas");
    var ctx = c.getContext("2d");
    var allowChange = false;
    if (brushPositions.length > 0) {
        var lastPosition = brushPositions[brushPositions.length - 1];
        if (Math.abs(lastPosition.x - position.x) >= 1 || Math.abs(lastPosition.y - position.y)
        >= 1) allowChange = true;
    }
    else {
        allowChange = true;
    }
    if (!isDrawingContinued) {
        ctx.beginPath();
        isDrawingContinued = true;
        ctx.moveTo(position.x, position.y);
    }
    var currentBrushData = new CurrentBrushData();
    ctx.strokeStyle = currentBrushData.Color;
    ctx.lineWidth = currentBrushData.Size;
    ctx.lineTo(position.x, position.y);
    ctx.stroke();
    brushPositions.push(position);
    if (Date.now() - lastDrawTime > 50) {
        lastDrawTime = Date.now();
        var tempPositions = brushPositions.splice(0);
        brushPositions.push(position);
        sendBrushData(tempPositions);
    }
}
function userConnected(userName) {
    var alreadyExists = false;
    for (var x = 0; x < userList.length; x++) {
        if (userList[x] == userName) {
            alreadyExists = true;
            break;
        }
    }
    if (!alreadyExists) userList.push(userName);
    drawUserList();
}
function userDisconnected(userName) {
    for (var x = userList.length - 1; x > -1; x--) {
        if (userList[x] == userName) {
            userList.splice(x, 1);
        }
    }
    drawUserList();
}
function drawUserList() {
    var userListHTML = [];
    for (var x = 0; x < userList.length; x++) {
        userListHTML.push('<li>');
        userListHTML.push(userList[x]);
        userListHTML.push('</li>');
    }
    $('#userList').html(userListHTML.join(''));
}
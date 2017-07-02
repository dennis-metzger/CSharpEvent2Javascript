var app = require('http').createServer(handler);
var io = require('socket.io').listen(app);
var fs = require('fs');
 
var mySocket = 0;
 
var dgram = require("dgram");
var udpSocket = dgram.createSocket({type: 'udp4', reuseAddr: true});

app.listen(3000);
 

function handler (req, res) {
  fs.readFile(__dirname + '/index.html',
  function (err, data) {
    if (err) {
      res.writeHead(500);
      return res.end('Error loading index.html');
    }
    res.writeHead(200, {'Content-Type': 'text/html'});
    res.end(data);
  });
}
 
io.sockets.on('connection', function (socket) {
  console.log('Webpage connected');
  mySocket = socket;

  socket.on('jsEvent', function (data) {
    console.log("Sending => " + data);
    udpSocket.send(data, 41182, '127.0.0.1');
  });
});

udpSocket.on("message", function (msg, rinfo) {
  console.log("Received => " + msg);
  if (mySocket != 0) {
     mySocket.emit('eventReceived', "" + msg);
     mySocket.broadcast.emit('eventReceived', "" + msg);
  }
});
 
udpSocket.on("listening", function () {
  var address = udpSocket.address();
  console.log("UDP server listening to " + address.address + ":" + address.port);
});
 
udpSocket.bind(41181);

var previous = null;
var profileEventLoop = function() {
    var ts = new Date().getTime();
    if (previous) {
      console.log(ts - previous);
    }
    previous = ts;

  setTimeout(profileEventLoop, 1000);
}

setImmediate(profileEventLoop);
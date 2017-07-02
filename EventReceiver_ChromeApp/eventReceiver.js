var socket = chrome.sockets.udp;

var Client = function() {
	var socketInfo;

	this.listen = function(ip, port, callback) {

		socket.create({}, function(_socketInfo) {
			socketInfo = _socketInfo;

			socket.bind(socketInfo.socketId, ip, port, function(connectResult) {
        		socket.onReceive.addListener(function(result) {
        			if(result.socketId === socketInfo.socketId) {
        				var decodedString = String.fromCharCode.apply(null, new Uint8Array(result.data));
        				callback(decodedString);
        			}
      			});
      		});
    	});
	};

	this.send = function(ip, port, data) {
		//var bytes = new TextEncoder('ascii').decode(data);
		var bytes = this.str2ab(data);
		socket.send(socketInfo.socketId, bytes, ip, port, function(sendInfo) {});
	};

	this.str2ab = function(str) {
	  var buf = new ArrayBuffer(str.length); // 2 bytes for each char
	  var bufView = new Uint8Array(buf);
	  for (var i=0, strLen=str.length; i < strLen; i++) {
	    bufView[i] = str.charCodeAt(i);
	  }
	  return buf;
	}

};

function eventReceived(data) {
	var contentDiv = document.getElementById('received');
	contentDiv.innerHTML = '<span>' + data + '</span></br>';
	contentDiv.scrollTop = contentDiv.scrollHeight;

	c.send('127.0.0.1', 41182, data.replace('"event"', '"ping"')); 
}


var c = new Client();
c.listen('0.0.0.0', 41181, eventReceived); 
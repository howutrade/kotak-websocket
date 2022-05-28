# kotak-websocket
Connect Kotak Websocket from .Net Clients

Kotak websocket uses Socket.IO server

Socket.io is a multi chat/bi-directional communication system built on the top of raw websockets (it's also supports HTTP long-polling).

Python and Js clients are available for socket.io, but neither DotNet client nor any getting started article available for DotNet!?.

If you try send message to socket.io server from any .Net websocket clients, mostly you won't get any response.

This is because the socket.io server expects messages to be sent in certain format (socket.io protocol). Check this below link for socket.io protocol.

https://github.com/socketio/socket.io-protocol

Kotak's offcial documentaion has example in Js which uses socket.io, so anyone trying to connect using raw websocket client might have failed to make it work.

Check the Kotak's js websocket documentation here.

https://tradeapi.kotaksecurities.com/devportal/apis/a38b95f8-c52e-4037-a8d0-12c12e2efff7/documents/4deb9ff6-b1b1-48c1-8d60-f665b9ba5c2d

## Let's get started

## Connection:

### Js Example:

```
<script>
const socket = io("https://wstreamer.kotaksecurities.com?access_token=a38b95f878c52eer4037ioa8d0456l12c12e2efff7", { path: "/feed/", transports: ['websocket', 'polling'] });
</script>
```

### Raw Websocket:
Here the path argument has to be included in the Url

```
websocket = new Websocket("wss://wstreamer.kotaksecurities.com/feed/?EIO=3&transport=websocket&access_token=a38b95f878c52eer4037ioa8d0456l12c12e2efff7")
```

*EIO=3 is the socket.io engine version used in Kotak server
*Also note the url scheme in Js (https://) and in raw socket (wss://)

## Subscribe for Quotes:

### Js Example:

```
<script>
socket.emit('pageload', "{'inputtoken', 'insttoken'}");
</script>
```

### Raw Websocket:
Here we need to send the message with prefix 42
42["pageload", {"inputtoken":"insttoken"}]

```
WebSocket.Send("42[" & """" & "pageload" & """" & ", {" & """" & "inputtoken" & """" & ":" & """" & insttoken & """" & "}]")
```

*for multiple tokens send insttoken as csv token1,token2,token3

Unsubscribe is same as above except change "pageload" to "pageunload"

## Other points:

Socket.io server expects a ping message from the client every 25 seconds else, it will disconnect.

For safer side send ping every 10 secs from websocket client. The following is accepted as Ping by socket.io

```
WebSocket.Send("3")
```

There are delays in connection at sometimes that we reported to Kotak team.

You have to send message only after you receive welcome message from socket.io server.

Kotak socket sends the welcome message as "broadcast" event at sometime and "message" event at some other times.
You will be able to subscribe and receive quotes if the welcome message is sent us "message" from kotak socket.
If the welcome message is received as "broadcast", disconnect and connect gain with socket.io server.

### Sample messages received from socket.io on connect

```
40
0{"sid":"bRapKCZCWDMNT 2_AAen","upgrades":[],"pingInterval":25000,"pingTimeout":5000}
42["broadcast", "welcome to Streamer Io (Build: 20201120).")
```
```
40
0{"sid":"_QmZIS7Diwsp459VAAEq", "upgrades":[],"pingInterval":25000,"pingTimeout":5000}
42["message","welcome to Streamer IO (Build: 20200709). ")
```

## Price Fields Supported by Kotak Websocket:

0. reqnum (ignore)
1. token
2. Best buy price
3. Best buy quantity
4. Best sell price
5. Best sell quantity
6. Last trade price
7. High price
8. Low price
9. Average trade price
10. Closing price
11. Open price
12. Net change percentage
13. Total sell quantity
14. Total buy quantity
15. Total trade quantity
16. Open Interest
17. Total trade value
18. Last trade quantity
19. Last trade time
20. Net change
21. Upper circuit limit
22. Lower circuit limit

The live quotes are received in the same sequence as above in the websocket.

### Sample Response:

```
42["getdata",["0167101","44597","5628.0000","1700","5630.0000","5800","5629.0000","5725.0000","5603.0000","5666.0000","5735.0000","5706.0000","-1.8483","179500","160500","5323200","7877","3016123380000.00","1","31/12/2021 23:03:02","-106.0000","5964.0000","5506.0000"]]
```

## References:
https://stackoverflow.com/questions/24564877/what-do-these-numbers-mean-in-socket-io-payload

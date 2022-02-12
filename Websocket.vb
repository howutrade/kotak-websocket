Module KotakSocket
    ''' <summary>
    ''' Basic Example
    ''' Modify as required
    ''' Add error handling
    ''' </summary>

    Private Websock As WebSocket

    Sub main()
        Dim ConsumerKey As String = "xxxxxxx"
        Dim ConsumerSecret As String = "yyyyyy"

        Dim token As String = GetTokenWebSock(ConsumerKey, ConsumerSecret)
        If Not String.IsNullOrEmpty(token) Then
            Connect(token)
            '//Start the Ping loop after successful connect
        End If

        Console.ReadKey()
    End Sub

    Private Function GetTokenWebSock(ByVal ConsumerKey As String, ByVal ConsumerSecret As String) As String
        Try
            Dim auth As String = ConsumerKey & ":" & ConsumerSecret
            Dim authBase64 As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth))
            Dim json As String = "{" & """" & "authentication" & """" & ": " & """" & authBase64 & """" & "}"

            Using client As New WebClient
                client.Headers(HttpRequestHeader.ContentType) = "application/json"
                Dim postBytes As Byte() = Encoding.ASCII.GetBytes(json)
                Dim respBytes As Byte() = client.UploadData("https://wstreamer.kotaksecurities.com/feed/auth/token", "POST", postBytes)
                Dim respString As String = Encoding.ASCII.GetString(respBytes)
                Dim token As String '= //parse the json response with json client and extract the token
                Return token
            End Using
        Catch Ex As Exception
        End Try
        Return ""
    End Function

    Private Sub Connect(ByVal tokenWebsock As String)
        Websock = New WebSocket("wss://wstreamer.kotaksecurities.com" & "/feed/?EIO=3&transport=websocket&access_token=" & tokenWebsock, "", WebSocketVersion.Rfc6455)

        Websock.Security.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 '//Onll Tls2 supported by Kotak

        'AddHandler
        AddHandler Websock.Opened, AddressOf WebSockOpened
        AddHandler Websock.Closed, AddressOf WebSockClosed
        AddHandler Websock.MessageReceived, AddressOf WebSockMessageReceived
        AddHandler Websock.Error, AddressOf WebSockError

        Websock.Open()
    End Sub

    Private Sub WebSockOpened(sender As Object, e As System.EventArgs)
        Console.WriteLine("socket opened")
    End Sub

    Private Sub WebSockMessageReceived(sender As Object, e As WebSocket4Net.MessageReceivedEventArgs)
        Console.WriteLine("message received from socket: " & e.Message)
        '//Do data parsing here
    End Sub

    Private Sub WebSockClosed(sender As Object, e As System.EventArgs)
        Console.WriteLine("socket closed")
        '//Do retry logic here
    End Sub

    Private Sub WebSockError(sender As Object, e As SuperSocket.ClientEngine.ErrorEventArgs)
        Console.WriteLine("socket error: " & e.Exception.Message)
        '//Do retry logic here
    End Sub

    Private Sub PingWebsocket()
        Do
            Thread.Sleep(10000) 'CHECK
            If Websock Is Nothing Then Continue Do
            Websock.Send("3")
        Loop
    End Sub
End Module

# TaskProject
Application and server for work with user informational cards.
Every card has title and image.

Window's application:

![preview](https://user-images.githubusercontent.com/58385485/157473364-d9b9ca16-6b2e-42df-b165-0fc53858c648.jpg)
## Server

_Server is based on .NET Core 3.0_

To start server run following file: ***Server/Server/bin/Release/netcoreapp3.0/win-x64/Server.exe***

### Server api

<hr>

**(HttpGet)**

- http://{server address}/api/card - return list of cards.
- http://{server address}/api/card/{card id} - return card info by card id.
- http://{server address}/api/card/{card id}/image - return card image by card id.

<hr>

**(HttpPost)**

- http://{server address}/api/card - create new card.

***Arguments:***

1. Title={card title}
2. Image={card image}

*You can make following post request using cURL to make new card:*

***curl -X 'POST' 'http://{server address}/api/card?&Title={card title}' -H 'accept: */*' -H 'Content-Type: multipart/form-data' -F 'image=@{path to the image}'***

<hr>

**(HttpDelete)**

- http://{server address}/api/card -  delete cards using ids.

***Arguments:***

1. Ids={sequence of the ids}

*You can make following delete request using cURL to delete cards using their ids:* 

***curl -X 'DELETE' 'http://{server address}/api/card/delete?ids={card id}&ids={card id}...' -H 'accept: */*' -H 'Content-Type: text/plain'***

<hr>

**(HttpPut)**

- http://{server address}/api/simpleupdate - change card title using card id.

***Arguments:***

1. Id={card id}
2. Title={new card title}

*You can make following put request using cURL to change card title using card id:* 

***curl -X 'PUT' 'http://{server address}/api/card/simpleupdate?id={card id}&title={new card title}' -H "Content-Type: text/plain" -H "Content-Length: 0"***

- http://{server address}/api/card/fullupdate - change card title and image using card id.

***Arguments:***

1. Id={card id}
2. Title={new card title}
3. Image={new card image}

*You can make following put request using cURL to change card title and image using card id:* 

***curl -X 'PUT' 'http://{server address}/api/card/fullupdate?id={card id}&title={card title}' -H 'accept: */*' -H 'Content-Type: multipart/form-data' -F 'image=@"{image path}'***

<hr>

## Client

_Client is based on .NET Framework 4.7_

To start client run following file: ***Client/Client/bin/Debug/Client.exe***







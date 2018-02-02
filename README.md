# MyChat 
### MyChat is a web application built upon the following technologies:

 * ASP.NET Core 2.0 (server-side)
 * Angular 4 (front-end client app )
 * Node.js (for client-side packages managment)
 * MongoDB (storage)
 * RabbitMQ (message broker)
 * Web STOMP (allows client-broker communication)

##### In order to run the app locally, Nodejs, MongoDB and RabbitMQ have to be running on the local machine (the latter two on their default ports).
##### Also, plugin [rabbitmq-web-stomp](https://www.rabbitmq.com/web-stomp.html) has to be enabled on the local RabbitMQ broker.

##### The app provides a cookie-based staple authentication system that makes use of .NET Core Identity, allowing users to register an account, look up other usernames and start conversations with them.
##### Every chat and message is stored on creation, allowing the app to fetch data and display it on startup. 

##### The system keeps track of every message state transition (displaying wheter each message has been Sent, Delivered, Read, etc.). 

###### Although basic communication is completely functional, please note this is an embrional stage of the app and a lot has still to be implemented.

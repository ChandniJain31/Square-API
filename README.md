# Squares-API
## Purpose
The SquaresAPI is designed to enable the enterprise consumers to identify squares from coordinates.

This API allows a user to import a list of points, add and delete a point to an existing list. On top of that this API can identify all the squares which can be generated from the list of points added by the user.

A point is a pair of integer X and Y coordinates. A square is a set of 4 points that when connected make up, well, a square. 

#### Example of a list of points that make up a square:
```[(-1;1), (1;1), (1;-1), (-1;-1)]```



### API Functinality

* A user can import a list of points.
* A user can add a point to an existing list.
* A user can delete a point from an existing list.
* A user can retrieve the squares identified from all the points.


### API request/response contracts

* POST -	token			[Send JWT bearer token and refresh token. Returns OK 200 response if user is valid and token is generated.]
* POST -	refreshtoken	[Fetch new JWT bearer token and refresh token once existing token gets expired. Returns OK 200 response if refresh token generated.]

* POST - 	Point/Import	[Returns Created 201 response if points imported successfully.]
* POST -	Point/Add 	   [Returns Created 201 response if new point created successfully. If the point already exists, it returns BadRequest 400 response.]
* POST - 	Point/Delete	[Returns NoContent 204 response.]
* GET  - 	GetSquares	   [Returns Ok 200 response with collection of object if any squares can be identified else returns NotFound 404 response.]


## Technologies And Tools
Web API Project is created with:
* .Net 6 SDK [.Net Core 6.0]
* Microsoft SQL Server 2018
* Microsoft Visual Studio 2022
* MS Test Unit Test Project

## Setup
 * MS SQL Server database is used for persistent storgae.
 * Database Default connection is pointed to MSSQLLocalDB server and Database 'SquareAPI'. Connectionstring can be changed in appsettings.json file.
 * Automatic migration will be performed when API starts. 
 * Logging is done using NLog. Log files can be found in Logs folder in base directory of SquareAPI and named as SquareAPILog_{date}.log file.
 * JWT Authentication is performed to the Web API consumers, so consumer have to pass Authorzation header in request with valid Bearer token.
 * Bearer token has expiration time 120 minutes, that is configured in appsettings.json. 
 * Secret key is configured in appsettings.json.
 * For testing purpose, three users are created as follows- 'admin','user1','user2'. Their Password are same as their username. 

## Launch
 * Web API is configured to host at URL- https://localhost:44728
 * Swagger is launched at https://localhost:44728/swagger/index.html
 * WebApiTestProject is MS Test Project which contains Unit Test cases for Square API. 
   

# READ ME
-----------------------------

There's no need of db dump, as it has a db initializer integrated.
The only thing to do is run 

	Add-migration NewTestEnv
	
	update-database

After the first time it runs, the testing users created are:

	admin@admin.com Psw: administrator123
	Role: Admin

	client@client.com Psw: password123
	Role: Client


# List of allowed calls for unregistered users:

Get the list of movies
------------

Parameters accepted: /{pagesize:int?}/{pagenumber:int?}/{sortby:int?}

Sortby values { 0: title, 1: likes }

Endpoint movie/list/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}

Sample requests:

	GET https://localhost:44384/movie/list
	GET https://localhost:44384/movie/list/4/0/1
		
		
Sample response 

	{
	  "list": [
		{
		  "id": 2,
		  "title": "HARRY POTTER AND THE SORCERER\u0027S STONE",
		  "description": "Harry Potter and the Sorcerer\u0027s Stone adapts its source material faithfully while condensing the novel\u0027s overstuffed narrative into an involving -- and often downright exciting -- big-screen magical caper.",
		  "rentalPrice": 3,
		  "salePrice": 200.5,
		  "img": "https://resizing.flixster.com/Q5W7m_i_f24Q_a4zLeRxNvx1WAs=/206x305/v2/https://flxt.tmsimg.com/assets/p28630_p_v8_at.jpg",
		  "stock": 0,
		  "availability": true,
		  "countLikes": 0
		},
		{
		  "id": 1,
		  "title": "KAMP KORAL: SPONGEBOB",
		  "description": "From Nickelodeon, KAMP KORAL: SPONGEBOB\u0027S UNDER YEARS is the first-ever SpongeBob SquarePants spinoff. The CG-animated prequel series follows 10-year-old SpongeBob SquarePants and his pals during summer sleepaway camp where they spend their time building underwater campfires, catching wild jellyfish and swimming in Lake Yuckymuck at the craziest camp in the kelp forest, Kamp Koral.",
		  "rentalPrice": 3,
		  "salePrice": 200.5,
		  "img": "https://resizing.flixster.com/hVncxQGAjTZrbIUFtxJEPPb1dU0=/180x257/v2/https://resizing.flixster.com/NjNMaJNZgDiAtQUs8y5x77oLTTQ=/ems.ZW1zLXByZC1hc3NldHMvdHZzZXJpZXMvM2FmMmI0ZjAtZTg5MC00YjAzLWJiOGItNzg3YjVlMzczNThmLmpwZw==",
		  "stock": 0,
		  "availability": true,
		  "countLikes": 0
		}
	  ],
	  "totalitems": 2
	}
		
Search movies query
-----
Parameters accepted: /{q}/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}

(If empty the default page size will be 2 items per page)

Endpoint /Movie/Search/{q}

Sample call 
	
	GET https://localhost:44384/movie/Search/harry
		
Sample Response:
		
	{
	  "list": [
		{
		  "id": 2,
		  "title": "HARRY POTTER AND THE SORCERER\u0027S STONE",
		  "description": "Harry Potter and the Sorcerer\u0027s Stone adapts its source material faithfully while condensing the novel\u0027s overstuffed narrative into an involving -- and often downright exciting -- big-screen magical caper.",
		  "rentalPrice": 3,
		  "salePrice": 200.5,
		  "img": "https://resizing.flixster.com/Q5W7m_i_f24Q_a4zLeRxNvx1WAs=/206x305/v2/https://flxt.tmsimg.com/assets/p28630_p_v8_at.jpg",
		  "stock": 0,
		  "availability": true,
		  "countLikes": 0
		}
	  ],
	  "totalitems": 1
	}

Get a movie details
-------------
Parametes: {id}

Endpoint /movie/Detail/{id}

Sample Request:
	
	GET https://localhost:44384/movie/Detail/1
		
Sample Response:

	{
	  "id": 1,
	  "title": "KAMP KORAL: SPONGEBOB",
	  "description": "From Nickelodeon, KAMP KORAL: SPONGEBOB\u0027S UNDER YEARS is the first-ever SpongeBob SquarePants spinoff. The CG-animated prequel series follows 10-year-old SpongeBob SquarePants and his pals during summer sleepaway camp where they spend their time building underwater campfires, catching wild jellyfish and swimming in Lake Yuckymuck at the craziest camp in the kelp forest, Kamp Koral.",
	  "rentalPrice": 3,
	  "salePrice": 200.5,
	  "img": "https://resizing.flixster.com/hVncxQGAjTZrbIUFtxJEPPb1dU0=/180x257/v2/https://resizing.flixster.com/NjNMaJNZgDiAtQUs8y5x77oLTTQ=/ems.ZW1zLXByZC1hc3NldHMvdHZzZXJpZXMvM2FmMmI0ZjAtZTg5MC00YjAzLWJiOGItNzg3YjVlMzczNThmLmpwZw==",
	  "stock": 5,
	  "availability": true,
	  "countLikes": 0
	}
		
		
Register
------------
Any user registered this way will be assigned the role of client, only admins can create other admin.

You can create a user but to confirm email, you need to follow these steps.

1 - If you already have an email service add the data on the appsettings.json or appsettings.Development.json. 

If you don't, I used mailtrap.io, it's free and easy to configure, I recommend this last option.

2 - Receive the data on the inbox of mailtrap and copy the link on the browser.

3 - It's also needed for forget password
		
Sample request
	
	POST https://localhost:44384/Register
	Content-Type: application/json

	{
		"Email": "newuser3@admin.com",
		"Password": "password123",
		"ConfirmPassword": "password123"
	}
		
Sample Response(value is the userid):

	{
	  "message": "New user registered",
	  "value": "adce201f-7f02-44b9-af83-7c59db4e58af",
	  "isSuccess": true,
	  "errorList": null
	}
		
ConfirmEmail
------------
Parameters userid and token

Method GET

Endpoint ConfirmEmail?userid={userid}&token={encryptedtoken}

Sample email:

	https://localhost:44384/ConfirmEmail?userid=adce201f-7f02-44b9-af83-7c59db4e58af&token=Q2ZESjhDYjFRQWRJV3hsQ2tDSUUraDBkKzJ4b1YxbWR4a0QwR3hraitWZVdVS0xnSnZFYnBKOGJjL2h3dndCNFVkbk1jQXJCaUZ5N2pTTGZHUGI1WGJWQXpRMUpQZlpCOStPVjc3UDJWTEJJb3Q5bVUwK1UzV0tZMUp5WE93aSs5cEpHZTNNdUs0YkhOc0tnWTJrUmJjVFFXU2RFN1Uxdll4STUxUWNCVmtpNnM5emExanBNZlZ3cmN5ZS9yeDZLNXJBS2ZZdjIyeGNHeldXbGNYOEppMnJ1Q0NLUWRXYU5EcXNBNXk0UVY4RDFveXNBdGhCM2kwODBBbTZqaW9VUENCY0grQT09
		

Sample response

	{
	  "message": "Mail confirmed",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}
		
Login
-------
sample request 
	
	POST https://localhost:44384/login
	Content-Type: application/json

	{
		"Email": "newuser3@admin.com",
		"Password": "password123"
	}
		
Sample response received (token as message field)
		
	{
	  "message": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ",
	  "value": "af7a1c9a-6039-4f97-a075-382b5c98c062",
	  "isSuccess": true,
	  "errorList": null
	}


#List of sample calls for Admins:

Add Movie
------
sample request 
	
	POST https://localhost:44384/movie/Add HTTP/2.0
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

	{
		"id": 0,
		"title": "KAMP KORAL: SPONGEBOB",
		"description": "From Nickelodeon, KAMP KORAL: SPONGEBOB'S UNDER YEARS is the first-ever SpongeBob SquarePants spinoff. The CG-animated prequel series follows 10-year-old SpongeBob SquarePants and his pals during summer sleepaway camp where they spend their time building underwater campfires, catching wild jellyfish and swimming in Lake Yuckymuck at the craziest camp in the kelp forest, Kamp Koral.",
		"rentalprice": 3.00,
		"saleprice": 200.50,
		"img": "https://resizing.flixster.com/hVncxQGAjTZrbIUFtxJEPPb1dU0=/180x257/v2/https://resizing.flixster.com/NjNMaJNZgDiAtQUs8y5x77oLTTQ=/ems.ZW1zLXByZC1hc3NldHMvdHZzZXJpZXMvM2FmMmI0ZjAtZTg5MC00YjAzLWJiOGItNzg3YjVlMzczNThmLmpwZw==",
		"stock": 5,
		"availability": true
	}
	
Sample response

	{
	  "message": "New movie saved",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}
	
Edit movie
-----
Endpoint movie/Edit

Sample request

	POST https://localhost:44384/movie/Edit HTTP/2.0
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

	{
		"id": 2,
		"title": "Old movie: HARRY POTTER AND THE SORCERER'S STONE",
		"description": "Harry Potter and the Sorcerer's Stone adapts its source material faithfully while condensing the novel's overstuffed narrative into an involving -- and often downright exciting -- big-screen magical caper.",
		"rentalprice": 3.00,
		"saleprice": 200.50,
		"img": "https://resizing.flixster.com/Q5W7m_i_f24Q_a4zLeRxNvx1WAs=/206x305/v2/https://flxt.tmsimg.com/assets/p28630_p_v8_at.jpg",
		"stock": 15,
		"availability": true
	}
	
Sample response

	{
	  "message": "Changes saved",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}

Delete movie
----------
sample request 

	PUT https://localhost:44384/movie/Delete/2 HTTP/2.0
	Content-Type: "DELETE"
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

Sample response:

	{
	  "message": "deleted id:2",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}
	
Complete movies list (not filtered by availability)
-----------
Parameters: Admin/List/{availability:int?}/{pagesize:int?}/{pagenumber:int?}/{sortby:int?}

availability values {0: unavariable movies, 1: available movies, 2: all}

If no data is entered the default value is to bring all (only for admins)

sample request

	GET https://localhost:44384/movie/admin/list/0
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

Sample response
	
	{
	  "list": [
		{
		  "id": 3,
		  "title": "HARRY POTTER AND THE SORCERER\u0027S STONE",
		  "description": "Harry Potter and the Sorcerer\u0027s Stone adapts its source material faithfully while condensing the novel\u0027s overstuffed narrative into an involving -- and often downright exciting -- big-screen magical caper.",
		  "rentalPrice": 3,
		  "salePrice": 200.5,
		  "img": "https://resizing.flixster.com/Q5W7m_i_f24Q_a4zLeRxNvx1WAs=/206x305/v2/https://flxt.tmsimg.com/assets/p28630_p_v8_at.jpg",
		  "stock": 0,
		  "availability": true,
		  "countLikes": 0
		},
		{
		  "id": 1,
		  "title": "KAMP KORAL: SPONGEBOB",
		  "description": "From Nickelodeon, KAMP KORAL: SPONGEBOB\u0027S UNDER YEARS is the first-ever SpongeBob SquarePants spinoff. The CG-animated prequel series follows 10-year-old SpongeBob SquarePants and his pals during summer sleepaway camp where they spend their time building underwater campfires, catching wild jellyfish and swimming in Lake Yuckymuck at the craziest camp in the kelp forest, Kamp Koral.",
		  "rentalPrice": 3,
		  "salePrice": 200.5,
		  "img": "https://resizing.flixster.com/hVncxQGAjTZrbIUFtxJEPPb1dU0=/180x257/v2/https://resizing.flixster.com/NjNMaJNZgDiAtQUs8y5x77oLTTQ=/ems.ZW1zLXByZC1hc3NldHMvdHZzZXJpZXMvM2FmMmI0ZjAtZTg5MC00YjAzLWJiOGItNzg3YjVlMzczNThmLmpwZw==",
		  "stock": 0,
		  "availability": true,
		  "countLikes": 0
		}
	  ],
	  "totalitems": 2
	}
	
search for admins
--------
Endpoint /movie/Admin/Search/Bob

Just like the search function for unregistered user/clients but with the possibility of filtering by availability

sample request

	GET https://localhost:44384/movie/Admin/Search/Bob
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

Sample response:
	
	{
	  "list": [
		{
		  "id": 1,
		  "title": "KAMP KORAL: SPONGEBOB",
		  "description": "From Nickelodeon, KAMP KORAL: SPONGEBOB\u0027S UNDER YEARS is the first-ever SpongeBob SquarePants spinoff. The CG-animated prequel series follows 10-year-old SpongeBob SquarePants and his pals during summer sleepaway camp where they spend their time building underwater campfires, catching wild jellyfish and swimming in Lake Yuckymuck at the craziest camp in the kelp forest, Kamp Koral.",
		  "rentalPrice": 3,
		  "salePrice": 200.5,
		  "img": "https://resizing.flixster.com/hVncxQGAjTZrbIUFtxJEPPb1dU0=/180x257/v2/https://resizing.flixster.com/NjNMaJNZgDiAtQUs8y5x77oLTTQ=/ems.ZW1zLXByZC1hc3NldHMvdHZzZXJpZXMvM2FmMmI0ZjAtZTg5MC00YjAzLWJiOGItNzg3YjVlMzczNThmLmpwZw==",
		  "stock": 0,
		  "availability": true,
		  "countLikes": 0
		}
	  ],
	  "totalitems": 1
	}
	
Get list of roles
-----------
sample request

	POST https://localhost:44384/Admin/Rolelist
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ
	
Sample response:
	
	[
	  {
		"id": "3fcb6001-7277-40b0-849c-f90125639b3d",
		"name": "Client",
		"normalizedName": "CLIENT",
		"concurrencyStamp": "67287c6e-4088-4665-b4ef-d78eecaaef71"
	  },
	  {
		"id": "c2a3b0b5-a1ed-4824-98d7-5d4ee418c14a",
		"name": "Admin",
		"normalizedName": "ADMIN",
		"concurrencyStamp": "d9b5dc93-f7c1-4d84-a3bc-1e57e1fcd2c9"
	  }
	]
	
Create role
-----------
sample request 

	POST https://localhost:44384/Admin/CreateRole
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

	{
		"Name": "tester",
		"Permissions": [
			"pay",
			"view",
			"edit",
			"create"
		]
	}
	
sample response (value is the id):

	{
	  "message": "New role created",
	  "value": "fa2f91ba-2c2c-4efd-b6f2-954ed6cd7e23",
	  "isSuccess": true,
	  "errorList": null
	}
	
Assign or unassing a role to a user
------
sample request

	POST https://localhost:44384/Admin/UpdateUserRoles
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

	{
		"UserId": "0bde5787-1930-4054-8185-1ad7dd7bdbf6",
		"RoleId": "047febd3-e3f9-42d1-9628-aa275eea3d17"
	}
	
sample response:

	{
	  "message": "Assigned",
	  "value": "UserID:adce201f-7f02-44b9-af83-7c59db4e58af Role: tester",
	  "isSuccess": true,
	  "errorList": null
	}
	
List of operations (purchases, rents, etc)
-------------
Parameters to filter, inside the body

	{
		"UserId":"",
		"Type": "", // ("PURCHASE", "RENT")
		"State": "", // ("PAID", "TO_PAY","FAIL", "CANCELLED", "RETURNED", "PENALTY_PAID")
		"FromDate": "",
		"ToDate":"",
		"MovieId": ""
	}
	
sample request 
	
	POST https://localhost:44384/Operation/List
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzY1ODEsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.gJiPe7InQ25oSpOjpL3wTGBFs-g40DpDj1piq3C9q-k

	{
	}

list of people who didn't return the movies yet.
-----------
sample request

	POST https://localhost:44384/Operation/Admin/UnreturnedList
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiOTIxODcyZWQtNjgwYi00ZDAzLWJmNjQtYmYyM2ZlNmVhMjRlIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU3MDg5MzEsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.4Orve0wm3EXkK_q12KFH7EGj5bP4HrsW9UHdf8rJ2lw
	
Register a new user as admin or other roles
--------------
sample request

	POST https://localhost:44384/Register
	Content-Type: application/json

	{
		"Email": "newuser3@admin.com",
		"Password": "password123",
		"ConfirmPassword": "password123",
		"Roles": ["ca1ba776-f31b-4ff3-8f06-7b7d0d23a6fe", "9322ee80-3bb9-46f1-95b6-782ab6a7ad59"]
	}
	
Sample Response(value is userid):

	{
	  "message": "New user registered",
	  "value": "adce201f-7f02-44b9-af83-7c59db4e58af",
	  "isSuccess": true,
	  "errorList": null
	}
	
# List of actions allowed for Clients and Admins

Like
----
enpoint 
GET movie/like/{movieid}

if the user already liked the movie, the like is removed

sample request 

	GET https://localhost:44384/movie/like/1
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6Im5ld3VzZXIzQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZGI5M2I0MzAtMjNhZi00ZDc4LWI2ZTktMDNkMmVkZmQyODFhIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ2xpZW50IiwiZXhwIjoxNjE1ODM4ODE2LCJpc3MiOiJNeXNlbGYiLCJhdWQiOiJNeXNlbGYifQ.3BL6BBIuozbDoeDsrHT8QOlO-Tf37-5JkmsxikubqU4

sample response

	{
	  "message": "Like added",
	  "value": "2",
	  "isSuccess": true,
	  "errorList": null
	}

Logout
-----
sample request

	POST https://localhost:44384/logout
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQGFkbWluLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYWY3YTFjOWEtNjAzOS00Zjk3LWEwNzUtMzgyYjVjOThjMDYyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MTU4MzMxMTMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.2Ux4uZl3XTQLJG9cjTjptVsN6N6TlQwDat2McubpWwQ

sample response

	{
	  "message": "Logged out",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}

Add operations
-------
sample request
	
	POST https://localhost:44384/Operation/Add
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImNsaWVudEBjbGllbnQuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxZmRmNDYzNi0yN2RiLTQ1MjgtYjRlNi0zY2IxMDYwYTRkNWIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDbGllbnQiLCJleHAiOjE2MTU4Mzc3NDMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.8MdxJRuT0022B1w9wTcv1MDDcyqJ_FgB531sflvBLgI

sample response

	{
		"MovieId": 1,
		"Type": "RENT",
		"DaysBeforeDueDate": 3,
		"UserId": "1fdf4636-27db-4528-b4e6-3cb1060a4d5b",
		"Price": 10.00,
		"Status": "PAID"
	}
	
Edit
----------
sample request

	POST https://localhost:44384/Operation/Edit
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImNsaWVudEBjbGllbnQuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxZmRmNDYzNi0yN2RiLTQ1MjgtYjRlNi0zY2IxMDYwYTRkNWIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDbGllbnQiLCJleHAiOjE2MTU4Mzc3NDMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.8MdxJRuT0022B1w9wTcv1MDDcyqJ_FgB531sflvBLgI

	{
		"Id": 1,
		"MovieId": 1,
		"Type": "RENT",
		"UserId": "921872ed-680b-4d03-bf64-bf23fe6ea24e",
		"Status": "RETURNED"
	}
	
Sample response

	{
	  "message": "operation saved",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}
		
My list of unreturned movies
--------------
sample request

	POST https://localhost:44384/Operation/Unreturned
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImNsaWVudEBjbGllbnQuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxZmRmNDYzNi0yN2RiLTQ1MjgtYjRlNi0zY2IxMDYwYTRkNWIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDbGllbnQiLCJleHAiOjE2MTU4Mzc3NDMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.8MdxJRuT0022B1w9wTcv1MDDcyqJ_FgB531sflvBLgI
	
sample response: 

	[
	  {
		"id": 2,
		"movie": null,
		"userId": "1fdf4636-27db-4528-b4e6-3cb1060a4d5b",
		"operatorUserId": "1fdf4636-27db-4528-b4e6-3cb1060a4d5b",
		"type": "RENT",
		"price": 10,
		"date": "2021-03-15T11:55:58.7784861",
		"status": "TO_PAY",
		"dueDate": "2021-03-18T11:56:01.572375",
		"penaltyPrice": 103,
		"details": null
	  }
	]
	
My list of operations
----------
sample request

	POST https://localhost:44384/Operation/MyList
	Content-Type: application/json
	Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImNsaWVudEBjbGllbnQuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxZmRmNDYzNi0yN2RiLTQ1MjgtYjRlNi0zY2IxMDYwYTRkNWIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDbGllbnQiLCJleHAiOjE2MTU4Mzc3NDMsImlzcyI6Ik15c2VsZiIsImF1ZCI6Ik15c2VsZiJ9.8MdxJRuT0022B1w9wTcv1MDDcyqJ_FgB531sflvBLgI

	{
		"UserId": "1fdf4636-27db-4528-b4e6-3cb1060a4d5b"
	}
	
Sample response

	[
	  {
		"id": 1,
		"movie": null,
		"userId": "1fdf4636-27db-4528-b4e6-3cb1060a4d5b",
		"operatorUserId": "1fdf4636-27db-4528-b4e6-3cb1060a4d5b",
		"type": "RENT",
		"price": 10,
		"date": "2021-03-15T11:49:44.4266603",
		"status": "PAID",
		"dueDate": "2021-03-18T11:49:44.4338258",
		"penaltyPrice": 103,
		"details": null
	  }
	]

# List of actions that can be done for unregistered users, client and admins

Forget password 
---------
Endpoint: ForgetPassword?email={email}

sample request

	GET https://localhost:44384/ForgetPassword?email=newuser3@admin.com
	
sample response

	{
	  "message": "Email sent",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}
	
Reset password(all needed data is sent by mail)
--------
sample email body
	
	https://localhost:44384/ResetPassword 
	 Method: Post 
	 Body: Token, UserId, NewPassword, ConfirmPassword 
	 UserId: db93b430-23af-4d78-b6e9-03d2edfd281a 
	 Token: Q2ZESjhDYjFRQWRJV3hsQ2tDSUUraDBkKzJ6OGxDbGgyN1B2Rnl3aE9NTFZ1RFNQYXRhVFhlWnRPUzVUeWEvakxiVVQ0TGNXZkVtOXBpYTIrWjd6cTZ5M1orNzh6MVlrOXcyamdiVlBvZ2J5TUtmeWdqQlE1U0U0bEJLMDlDTWlWVWxOSDVQU0NkM0VoSi9JaTlsajBWUVVaSmhZWFoyN1ltTXQ3Z1ZPdHBpVnQrZHVzWnZPQ1VGdnBGcno0WmNndWtjRVA4OUFXU29tZG5JY0hKVkFrQTNXM2J3QkpxcFdTV2lwWTZDeFplb1Q3TExL 

sample request

	POST https://localhost:44384/ResetPassword
	Content-Type: application/json

	{
		"Token": "Q2ZESjhDYjFRQWRJV3hsQ2tDSUUraDBkKzJ6OGxDbGgyN1B2Rnl3aE9NTFZ1RFNQYXRhVFhlWnRPUzVUeWEvakxiVVQ0TGNXZkVtOXBpYTIrWjd6cTZ5M1orNzh6MVlrOXcyamdiVlBvZ2J5TUtmeWdqQlE1U0U0bEJLMDlDTWlWVWxOSDVQU0NkM0VoSi9JaTlsajBWUVVaSmhZWFoyN1ltTXQ3Z1ZPdHBpVnQrZHVzWnZPQ1VGdnBGcno0WmNndWtjRVA4OUFXU29tZG5JY0hKVkFrQTNXM2J3QkpxcFdTV2lwWTZDeFplb1Q3TExL",
		"UserId": "db93b430-23af-4d78-b6e9-03d2edfd281a",
		"NewPassword":"password1234",
		"ConfirmPassword":"password1234"
	}
	
sample response

	{
	  "message": "Please login with your new password",
	  "value": null,
	  "isSuccess": true,
	  "errorList": null
	}

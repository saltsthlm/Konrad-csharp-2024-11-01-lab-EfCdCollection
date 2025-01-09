## Background - what the heck is a CD?

Back in the days we didn't have mp3-files. We stored our music on plastic discs called CDs. It was very important to keep track on which CDs you had, their genres and which artist played on which CDs etc.

Since your music library was a physical pile of discs it was pretty hard to know what music you had, after about 30 CDs. Our dream was to have some kind of application where we could search for this etc.

This exercise is built as an homage of that dream

## The data model

- A CD has a name, an artist name, a description and a purchased date
- A Genre has a name
- Each CD have exactly one Genre
- Each Genre can have many many CDs

## The exercise

Build a Web API that:

- stores it data in an SQL server (use the supplied Docker image, as during the week)
  - create migrations for your model so that we can run `dotnet ef database update` and get the database in the correct state
  - you don't need to seed the database with data

- Has the following features:
  - Post to create a new CD (POST `/api/CDs/`)
  - List all CDs in the database (GET `/api/CDs/`)
    - Add a parameter to filter by genre
    - This parameter can be empty which should list all CDs
  - Get one CD and all it's related data (GET `/api/CDs/{id}`)
  - Add Artist to a CD (PUT `/api/CDs/{id}/artist`, send artist in the body of the request)
  - Add Genre to a CD (PUT `/api/CDs/{id}/genre`, send genre name in the body of the request)

- Write tests that verifies that your code works on a suitable level (unit or integration tests) using the techniques we learned during the week.
- You can use the Swagger or CURL to verify your application end-to-end. Swagger will create CURL commands for you you to create these.
  - You can also use [Postman](https://www.postman.com/), that is installed on your computers, or [Insomnia](https://insomnia.rest/) if you rather use an UI

### Solution technical requirements

- that you have used the supplied `docker-compose.yml` file 
- that the database is named `CdCollectionYourName`
- that your API runs on `http://localhost:3000/api/CDs/`

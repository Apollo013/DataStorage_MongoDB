# MongoDB_Repository_Pattern
A DotNet app that utilizes the repoitory pattern for accessing a MongoDB database.

---

Developed with Visual Studio 2015 Community

---

###TECHS
|TECH|
|----|
|MongoDB|
|C#|
|Linq|

---

###Features
|Feature|
|-------|
|Repository Pattern for MongoDB|
|A 'CollectionName' attribute for entities that specifies the name of the collection. Reflection is then used to retrieve the name; see 'GetCollectionName()' helper method within the 'MongoRepository' class|
|Extension classes for IQueryable(T), IFindFluent(T,T) & IEnumerable(T)|
|CRUD Operations: Add, Update, Replace & Delete|
|Query Operations: GetById, ToList, Find, FirstOrDefault, Last & Paging|
|'FilterDefinitionBuilder' - A type-safe API for building up both simple and complex MongoDB queries|
|Numerous test units|


###Resources
|Title|Author|Website|
|-----|------|-------|
|[Introduction to MongoDb with .NET part 1: background] (https://dotnetcodr.com/2016/03/14/introduction-to-mongodb-with-net-part-1-background/) | Andras Nemes | dotnetcodr.com |
|[MongoDB in .NET part 1: foundations] (https://dotnetcodr.com/2014/07/28/mongodb-in-net-part-1-foundations/) | Andras Nemes | dotnetcodr.com |
|[MongoRepository](https://github.com/esendir/MongoRepository)| esendir | GitHub |
|[MongoRepository](https://github.com/RobThree/MongoRepository)| RobThree | GitHub |
|[The MongoDB 3.2 Manual](https://docs.mongodb.com/manual/)| | MongoDB |
|[Install MongoDB Community Edition on Windows](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/)| | MongoDB |
|[IMongoDB Limits and Thresholds](https://docs.mongodb.com/manual/reference/limits/)| | MongoDB |
|[Getting Started with MongoDB (C# Edition)](https://docs.mongodb.com/getting-started/csharp/)| | MongoDB |
|[Collection Methods](https://docs.mongodb.com/manual/reference/method/js-collection/)| | MongoDB |
|[Database Commands](https://docs.mongodb.com/manual/reference/command/)| | MongoDB |
|[LINQ](http://mongodb.github.io/mongo-csharp-driver/2.2/reference/driver/crud/linq/)| | MongoDB |
|[Authentication](http://mongodb.github.io/mongo-csharp-driver/2.2/reference/driver/authentication/)| | MongoDB |
|[Manage Users and Roles](https://docs.mongodb.com/manual/tutorial/manage-users-and-roles/)| | MongoDB |
|[Security Checklist](https://docs.mongodb.com/manual/administration/security-checklist/)| | MongoDB |
|[Built-In Roles](https://docs.mongodb.com/manual/core/security-built-in-roles/)| | MongoDB |
|[Enable Auth](https://docs.mongodb.com/manual/tutorial/enable-authentication/)| | MongoDB |

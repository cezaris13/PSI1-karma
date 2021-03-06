# Team members:
Arentas, Martinas, Pijus

# Setup
1. Open solution file (karma.sln) with visual studio
1. Build the project
1. PM Console: `dotnet user-secrets set "SendGridApiKey" "ValueGivenByDevelopers"`
1. Launch (after launch the app should be accessible on localhost)

# DB update
1. Change KarmaContext or any model
1. run `dotnet ef migrations add NameOfMigration --context KarmaContext` in package manager console
1. run `dotnet ef database update --context=KarmaContext` in package manager console

Notes:
- DB browser for sqlite is great for checking DB itself.
- Path of DB file can be found by checking KarmaContext().DbPath (or by setting a breakpoint in KarmaContext constructor)

# Tasks
- [x] Get greenlight for our idea during lecture
- [x] Transfer milestones to ~~trello~~ github board
- [x] Split milestones into smaller tasks and start working on them
- [ ] ...



Notes for lecture:
 - 'When working with Web applications, use a context instance per request.' https://docs.microsoft.com/en-us/ef/ef6/fundamentals/working-with-dbcontext

# OneCoreApp

## EF Core Commands
```shell
$ dotnet tool install --global dotnet-ef
$ dotnet ef migrations add MyFirstMigration
$ dotnet ef database update
$ dotnet ef migrations remove
$ dotnet ef migrations script
```
More Info @ https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx

## Run Locally

```shell
$ cd <repo_path>/src/OneCore
$ dotnet run
```

## Publish

```shell
$ dotnet publish -c Release -r win-x64 --output ./dist OneCore.sln
```

## Swagger Doc
`https://{host}:{port}/swagger`
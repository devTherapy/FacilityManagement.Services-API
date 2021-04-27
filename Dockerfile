FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /src
COPY *.sln .

# copy and restore all projects
COPY FacilityManagement.Services.Test/*.csproj FacilityManagement.Services.Test/
COPY FacilityManagement.Common.Utilities/*.csproj FacilityManagement.Common.Utilities/
COPY FacilityManagement.Services.Models/*.csproj FacilityManagement.Services.Models/
COPY FacilityManagement.Services.Core/*.csproj FacilityManagement.Services.Core/
COPY FacilityManagement.Services.Data/*.csproj FacilityManagement.Services.Data/
COPY FacilityManagement.Services.DTOs/*.csproj FacilityManagement.Services.DTOs/
COPY FacilityManagement.Services.API/*.csproj FacilityManagement.Services.API/

RUN dotnet restore

# Copy everything else
COPY . .

#Testing
FROM base AS testing
WORKDIR /src/FacilityManagement.Services.API
WORKDIR /src/FacilityManagement.Services.Core
WORKDIR /src/FacilityManagement.Services.Data
WORKDIR /src/FacilityManagement.Services.Models
WORKDIR /src/FacilityManagement.Services.DTOs
WORKDIR /src/FacilityManagement.Common.Utilities

RUN dotnet build

WORKDIR /src/FacilityManagement.Services.Test
RUN dotnet test

#Publishing
FROM base AS publish
WORKDIR /src/FacilityManagement.Services.API
RUN dotnet publish -c Release -o /src/publish


#Get the runtime into a folder called app
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
COPY --from=publish /src/FacilityManagement.Services.Data/PreSeeder/data/Categories.json .
COPY --from=publish /src/FacilityManagement.Services.Data/PreSeeder/data/Comments.json .
COPY --from=publish /src/FacilityManagement.Services.Data/PreSeeder/data/Ratings.json .
COPY --from=publish /src/FacilityManagement.Services.Data/PreSeeder/data/Users.json .
COPY --from=publish /src/FacilityManagement.Services.Data/PreSeeder/data/Complaints.json .
COPY --from=publish /src/FacilityManagement.Services.Data/PreSeeder/data/Replies.json .
COPY --from=publish /src/FacilityManagement.Services.API/StaticFiles/InvitationA.html .
COPY --from=publish /src/FacilityManagement.Services.API/StaticFiles/InvitationB.html .
COPY --from=publish /src/FacilityManagement.Services.API/StaticFiles/ForgotPassword.html .

ENTRYPOINT ["dotnet", "FacilityManagement.Services.API.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet FacilityManagement.Services.API.dll
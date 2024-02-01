# Training Logger

Main purpose of this application is gather activities data and create reports based on this data. Activities are being downloaded automatically from Strava when uploaded, thanks to webhook that Strava API is exposing.

Initial idea was to download activites from Garmin, but access to API is quite expensive.

# Used technologies

- .NET 8
- Postgresql
- Docker
- NSubstitue
- Xunit
- Bogus

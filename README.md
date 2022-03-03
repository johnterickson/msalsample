Two parameters: withBroker, withIWA
Three configs: net48;netcoreapp3.1;net6

Tests on AADJ windows:
```
dotnet run -f net48 -- false false : MsalUiRequiredException: Only some brokers (WAM) can log in the current OS account.
dotnet run -f net48 -- false true  : ONLY works when on VPN
dotnet run -f net48 -- true false  : SUCCESS
dotnet run -f net48 -- true true   : ONLY works when on VPN
dotnet run -f netcoreapp3.1 -- false false : MsalUiRequiredException: Only some brokers (WAM) can log in the current OS account.
dotnet run -f netcoreapp3.1 -- false true  : ONLY works when on VPN
dotnet run -f netcoreapp3.1 -- true false  : SUCCESS
dotnet run -f netcoreapp3.1 -- true true   : ONLY works when on VPN
dotnet run -f net6 -- false false : fails
dotnet run -f net6 -- false true  : ONLY works when on VPN
dotnet run -f net6 -- true false  : fails*
dotnet run -f net6 -- true true   : ONLY works when on VPN
```

*I think net6 should work but I can't convince the build to let me link in `Microsoft.Identity.Client.Desktop`


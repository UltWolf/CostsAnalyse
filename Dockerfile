FROM microsoft/dotnet:3.0-sdk AS build-env
WORKDIR /app/CostsAnalyse/CostsAnalyse

RUN cd app/CostsAnalyse/CostsAnalyse; dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out
 
COPY --from=build-env /app/out .
CMD dotnet CostsAnalyse.dll

                                                                                                      